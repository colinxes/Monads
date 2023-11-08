namespace Monads.Optional
{
    public static class OptionalExtensions
    {
        public static Optional<T> Where<T>(this T? pObj, Func<T, bool> pPredicate) where T : class
        {
            return pObj is not null && pPredicate(pObj) ? Optional<T>.Some(pObj) : Optional<T>.None();
        }

        public static Optional<T> WhereNot<T>(this T? pObj, Func<T, bool> pPredicate) where T : class
        {
            return pObj is not null && !pPredicate(pObj) ? Optional<T>.Some(pObj) : Optional<T>.None();
        }
    }
}