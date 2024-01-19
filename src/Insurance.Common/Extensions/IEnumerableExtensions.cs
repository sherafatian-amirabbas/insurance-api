namespace Insurance.Common.Extensions
{
    public static class IEnumerableExtensions
    {
        public static IEnumerable<TResult> LeftJoin<TOuter, TInner, TKey, TResult>(this IEnumerable<TOuter> outer, 
            IEnumerable<TInner> inner, 
            Func<TOuter, TKey> outerKeySelector, 
            Func<TInner, TKey> innerKeySelector, 
            Func<TOuter, TInner?, TResult> resultSelector)
        {
            var items = outer.GroupJoin(inner, outerKeySelector, innerKeySelector, (outerItem, innerSet) =>
                  new { outerItem, innerItems = innerSet.DefaultIfEmpty() }
              )
              .SelectMany(u => u.innerItems.Select(innerItem => resultSelector(u.outerItem, innerItem)));

            return items;
        }
    }
}
