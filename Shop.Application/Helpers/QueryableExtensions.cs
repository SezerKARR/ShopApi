namespace Shop.Application.Helpers;

using System.Linq.Expressions;

public static class QueryableExtensions
{
    public static IQueryable<T>? ApplySorting<T>(IQueryable<T>? query, string? sortBy, bool isDescending) 
    {
        if (query == null || string.IsNullOrWhiteSpace(sortBy))
            return query;

        var propertyInfo = typeof(T).GetProperty(sortBy);
        if (propertyInfo == null)
            throw new ArgumentException($"'{sortBy}' is not a valid property of type '{typeof(T).Name}'.");

        var parameter = Expression.Parameter(typeof(T), "x");
        var property = Expression.Property(parameter, propertyInfo.Name);
        var lambda = Expression.Lambda(property, parameter);

        var methodName = isDescending ? "OrderByDescending" : "OrderBy";

        var resultExpression = Expression.Call(typeof(Queryable), methodName, new[] { typeof(T), propertyInfo.PropertyType }, query.Expression, Expression.Quote(lambda));

        return query.Provider.CreateQuery<T>(resultExpression);
    }

    public static IQueryable<T>? ApplyFilter<T>(
        IQueryable<T>? query,
        string? filterBy,
        string? filterValue)
    {
        // Check if query is null before proceeding
        if (query == null || string.IsNullOrWhiteSpace(filterBy) || string.IsNullOrWhiteSpace(filterValue))
            return query;

        var propertyInfo = typeof(T).GetProperty(filterBy);
        if (propertyInfo == null)
            throw new ArgumentException($"'{filterBy}' is not a valid property of type '{typeof(T).Name}'.");

        var parameter = Expression.Parameter(typeof(T), "x");
        var property = Expression.Property(parameter, propertyInfo.Name);

        var toLowerMethod = typeof(string).GetMethod("ToLower", Type.EmptyTypes);
        if (toLowerMethod != null)
        {
            var propertyToLower = Expression.Call(property, toLowerMethod);

            var filterValueExpression = Expression.Constant(filterValue.ToLower());
            var containsMethod = typeof(string).GetMethod("Contains", new[] { typeof(string) });
            if (containsMethod != null)
            {
                var containsExpression = Expression.Call(propertyToLower, containsMethod, filterValueExpression);

                var lambda = Expression.Lambda<Func<T, bool>>(containsExpression, parameter);

                // Ensure query is not null before calling Where
                return query.Where(lambda);
            }
        }
        return query;
    }
}
