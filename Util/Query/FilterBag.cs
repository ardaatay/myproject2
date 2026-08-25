using System.Linq.Expressions;

namespace Util.Query
{
    public class FilterBag
    {
        private Dictionary<string, object> _filters = new();

        public FilterBag WithFilter<T>(Expression<Func<T, bool>> filter) where T : class
        {
            var key = ((filter.Body as BinaryExpression)?.Left as MemberExpression)?.Member.Name
                      ?? filter.ToString();

            _filters[key] = filter;
            return this;
        }

        public IQueryable<T> ApplyFilters<T>(IQueryable<T> query) where T : class
        {
            foreach (var filter in _filters.Values.OfType<Expression<Func<T, bool>>>())
                query = query.Where(filter);
            return query;
        }
    }
}
