// ReSharper disable All
namespace Monads.Optional
{
    public struct Optional<T> : IEquatable<Optional<T>> where T : class
    {
        private T _value;

        public bool IsNone => _value is null;
        public bool IsSome => !IsNone;

        #region Operators

        public static implicit operator Optional<T>(T? pValue)
        {
            return pValue is null ? None() : Some(pValue);
        }

        public static bool operator ==(Optional<T> pLeft, Optional<T> pRight)
        {
            return pLeft.Equals(pRight);
        }

        public static bool operator !=(Optional<T> pLeft, Optional<T> pRight)
        {
            return !(pLeft == pRight);
        }

        #endregion

        #region Factories

        public static Optional<T> Some(T pValue)
        {
            return new Optional<T> { _value = pValue };
        }

        public static Optional<T> None()
        {
            return new Optional<T>();
        }

        #endregion

        #region Equality

        public override int GetHashCode()
        {
            return _value?.GetHashCode() ?? 0;
        }

        public override bool Equals(object? pOther)
        {
            return pOther is Optional<T> optional && Equals(optional);
        }

        public bool Equals(Optional<T> pOther)
        {
            return EqualityComparer<T>.Default.Equals(_value, pOther._value);
        }

        #endregion

        #region Functions

        public Optional<T> WhenSome(Action<T> pWhenSome)
        {
            if (IsSome)
                pWhenSome.Invoke(_value);

            return this;
        }

        public Optional<T> WhenNone(Action pWhenNone)
        {
            if (IsNone)
                pWhenNone.Invoke();

            return this;
        }

        public T Fallback(T pWhenNone)
        {
            return _value ?? pWhenNone;
        }

        public T Fallback(Func<T> pWhenNone)
        {
            return IsSome ? _value : pWhenNone();
        }

        public T? GetValueOrDefault()
        {
            return IsSome ? _value : default;
        }

        public Optional<TResult> Transform<TResult>(Func<T, TResult> pTransformation) where TResult : class
        {
            return IsSome ? Optional<TResult>.Some(pTransformation(_value)) : Optional<TResult>.None();
        }

        public Optional<TResult> TransformOptional<TResult>(Func<T, Optional<TResult>> pTransformation) where TResult : class
        {
            return IsSome ? pTransformation(_value) : Optional<TResult>.None();
        }

        public void Switch(Action<T> pWhenSome, Action pWhenNone)
        {
            if (IsSome)
                WhenSome(pWhenSome);
            else
                WhenNone(pWhenNone);
        }

        public TResult Switch<TResult>(Func<T, TResult> pWhenSome, Func<TResult> pWhenNone)
        {
            return IsSome ? pWhenSome.Invoke(_value) : pWhenNone.Invoke();
        }

        public Optional<T> Where(Func<T, bool> pRedicate)
        {
            return IsSome && pRedicate(_value) ? this : None();
        }

        public Optional<T> WhereNot(Func<T, bool> pRedicate)
        {
            return IsSome && !pRedicate(_value) ? this : None();
        }

        #endregion
    }
}