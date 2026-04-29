// ReSharper disable All
namespace Monads.Result
{
    public readonly struct Result<TValue, TError> : IEquatable<Result<TValue, TError>>
    {
        private readonly TValue _value;
        private readonly TError _error;

        public bool IsSuccess { get; }
        public bool IsFailure => !IsSuccess;

        public static implicit operator Result<TValue, TError>(TValue pValue)
        {
            return Success(pValue);
        }

        public static bool operator ==(Result<TValue, TError> pLeft, Result<TValue, TError> pRight)
        {
            return pLeft.Equals(pRight);
        }

        public static bool operator !=(Result<TValue, TError> pLeft, Result<TValue, TError> pRight)
        {
            return !(pLeft == pRight);
        }

        public static Result<TValue, TError> Success(TValue pValue)
        {
            return new Result<TValue, TError>(true, pValue, default!);
        }

        public static Result<TValue, TError> Failure(TError pError)
        {
            return new Result<TValue, TError>(false, default!, pError);
        }

        public Result<TResult, TError> Map<TResult>(Func<TValue, TResult> pTransformation)
        {
            return IsSuccess ? Result<TResult, TError>.Success(pTransformation(_value)) : Result<TResult, TError>.Failure(_error);
        }

        public Result<TResult, TError> Bind<TResult>(Func<TValue, Result<TResult, TError>> pTransformation)
        {
            return IsSuccess ? pTransformation(_value) : Result<TResult, TError>.Failure(_error);
        }

        public Result<TValue, TErrorResult> MapError<TErrorResult>(Func<TError, TErrorResult> pTransformation)
        {
            return IsFailure ? Result<TValue, TErrorResult>.Failure(pTransformation(_error)) : Result<TValue, TErrorResult>.Success(_value);
        }

        public Result<TValue, TError> Tap(Action<TValue> pWhenSuccess)
        {
            if (IsSuccess)
                pWhenSuccess.Invoke(_value);

            return this;
        }

        public Result<TValue, TError> TapError(Action<TError> pWhenFailure)
        {
            if (IsFailure)
                pWhenFailure.Invoke(_error);

            return this;
        }

        public TResult Match<TResult>(Func<TValue, TResult> pWhenSuccess, Func<TError, TResult> pWhenFailure)
        {
            return IsSuccess ? pWhenSuccess.Invoke(_value) : pWhenFailure.Invoke(_error);
        }

        public void Match(Action<TValue> pWhenSuccess, Action<TError> pWhenFailure)
        {
            if (IsSuccess)
                pWhenSuccess.Invoke(_value);
            else
                pWhenFailure.Invoke(_error);
        }

        public TValue Fallback(TValue pWhenFailure)
        {
            return IsSuccess ? _value : pWhenFailure;
        }

        public TValue Fallback(Func<TError, TValue> pWhenFailure)
        {
            return IsSuccess ? _value : pWhenFailure(_error);
        }

        public TValue? GetValueOrDefault()
        {
            return IsSuccess ? _value : default;
        }

        public TError? GetErrorOrDefault()
        {
            return IsFailure ? _error : default;
        }

        public override bool Equals(object? pOther)
        {
            return pOther is Result<TValue, TError> result && Equals(result);
        }

        public bool Equals(Result<TValue, TError> pOther)
        {
            if (IsSuccess != pOther.IsSuccess)
                return false;

            return IsSuccess
                ? EqualityComparer<TValue>.Default.Equals(_value, pOther._value)
                : EqualityComparer<TError>.Default.Equals(_error, pOther._error);
        }

        public override int GetHashCode()
        {
            return IsSuccess
                ? HashCode.Combine(IsSuccess, _value)
                : HashCode.Combine(IsSuccess, _error);
        }

        private Result(bool pIsSuccess, TValue pValue, TError pError)
        {
            IsSuccess = pIsSuccess;
            _value = pValue;
            _error = pError;
        }
    }
}
