// ReSharper disable All
namespace Monads.Optional
{
    public struct Optional<T> : IEquatable<Optional<T>> where T : class
    {
        private T _value;

        public bool IsNone => _value is null;
        public bool IsSome => !IsNone;

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

        public static Optional<T> Some(T pValue)
        {
            return new Optional<T> { _value = pValue };
        }

        public static Optional<T> None()
        {
            return new Optional<T>();
        }

        public Optional<TResult> Map<TResult>(Func<T, TResult> pTransformation) where TResult : class
        {
            return IsSome ? Optional<TResult>.Some(pTransformation(_value)) : Optional<TResult>.None();
        }

        public Optional<TResult> Bind<TResult>(Func<T, Optional<TResult>> pTransformation) where TResult : class
        {
            return IsSome ? pTransformation(_value) : Optional<TResult>.None();
        }

        public Optional<T> Tap(Action<T> pWhenSome)
        {
            if (IsSome)
                pWhenSome.Invoke(_value);

            return this;
        }

        public Optional<T> TapNone(Action pWhenNone)
        {
            if (IsNone)
                pWhenNone.Invoke();

            return this;
        }

        public TResult Match<TResult>(Func<T, TResult> pWhenSome, Func<TResult> pWhenNone)
        {
            return IsSome ? pWhenSome.Invoke(_value) : pWhenNone.Invoke();
        }

        public void Match(Action<T> pWhenSome, Action pWhenNone)
        {
            if (IsSome)
                pWhenSome.Invoke(_value);
            else
                pWhenNone.Invoke();
        }

        public Optional<T> Where(Func<T, bool> pPredicate)
        {
            return IsSome && pPredicate(_value) ? this : None();
        }

        public Optional<T> WhereNot(Func<T, bool> pPredicate)
        {
            return IsSome && !pPredicate(_value) ? this : None();
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

        public override bool Equals(object? pOther)
        {
            return pOther is Optional<T> optional && Equals(optional);
        }

        public bool Equals(Optional<T> pOther)
        {
            return EqualityComparer<T>.Default.Equals(_value, pOther._value);
        }

        public override int GetHashCode()
        {
            return _value?.GetHashCode() ?? 0;
        }
    }
}
