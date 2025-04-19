using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;

namespace QueryParser;


// Evaluates a search term
public interface ISearchTermBase<TSource, U> where TSource : class
{
    // Returns a predicate according to search term operator / operands
    Expression<Func<TSource, bool>> CreatePredicate(Expression<Func<TSource, U>> target, SearchTerm term);
}

public class StringVariantProcessor<TSource> : ISearchTermBase<TSource, string> where TSource : class
{
    public Expression<Func<TSource, bool>> CreatePredicate(Expression<Func<TSource, string>> target, SearchTerm term)
    {
        var value = Expression.Constant(term.ValueAs<string>().ToLower(), typeof(string));
        Expression body;

        switch (term.Operator)
        {
            case Operator.Equals:
                body = Expression.Equal(target.Body, value);
                break;
            case Operator.Contains:
                // entity.StringProp.Tolower().Contains(X)
                var tolower = typeof(string).GetMethod(nameof(string.ToLower), Type.EmptyTypes);
                var contains = typeof(string).GetMethod(nameof(string.Contains), [typeof(string)]);
                
                body = Expression.Call(Expression.Call(target.Body, tolower), contains, value);
                break;
            default:
                throw new NotImplementedException();
        }

        return Expression.Lambda<Func<TSource, bool>>(body, target.Parameters[0]);
    }
}

public class IntegerVariantProcessor<TSource> : ISearchTermBase<TSource, int> where TSource : class
{
    public Expression<Func<TSource, bool>> CreatePredicate(Expression<Func<TSource, int>> target, SearchTerm term)
    {
        Expression body;
        switch (term.Operator)
        {
            case Operator.Equals:
                var literal = Expression.Constant(term.ValueAs<int>(), typeof(int));
                body = Expression.Equal(target.Body, literal);
                break;
            case Operator.In:
                // [1,2,3].Contains(x)
                body = Expression.Call(
                    typeof(Enumerable),
                    nameof(Enumerable.Contains),
                    [typeof(int)],
                    Expression.Constant(term.ValueAs<int[]>()),
                    target.Body);
                break;
            default:
                throw new NotImplementedException();
        }
        return Expression.Lambda<Func<TSource, bool>>(body, target.Parameters[0]);
    }
}

public class EntitySearchModelBuilder<T> where T : class
{
    protected record MapType(Type Type, object Expression);
   
    private readonly Dictionary<string, MapType> _fieldMapping = new Dictionary<string, MapType>();
    private readonly Dictionary<Type, object> _processors = new Dictionary<Type, object>()
    {
        { typeof(string), new StringVariantProcessor<T>() },
        { typeof(int), new IntegerVariantProcessor<T>() },
    };

    public EntitySearchModelBuilder<T> MapField<U>(string queryField, Expression<Func<T, U>> expression) // U: Primitive type
    {
        if (typeof(U) == typeof(string) || typeof(U) == typeof(int))
        {
            _fieldMapping[queryField] = new MapType(typeof(U), expression);
            return this;
        }
        throw new NotImplementedException("string and integer type are only supported");
    }

    public Expression<Func<T, bool>> BuildPredicate(SearchTerm searchTerm)
    {
        var info = _fieldMapping[searchTerm.Field];

        if (info.Type == typeof(int))
        {
            return ((IntegerVariantProcessor<T>)_processors[info.Type]).CreatePredicate(
                (Expression<Func<T, int>>)info.Expression, searchTerm);
        }
        else if (info.Type == typeof(string))
        {
            return ((StringVariantProcessor<T>)_processors[info.Type]).CreatePredicate(
                (Expression<Func<T, string>>)info.Expression, searchTerm);
        }
        throw new NotImplementedException("string and integer type are only supported");
    }

    public bool IsMapped(string fieldName) => _fieldMapping.ContainsKey(fieldName);
}

public static class IQueryableExtensions 
{
    public static IQueryable<T> AdvancedSearch<T>(this IQueryable<T> q, IEnumerable<SearchTerm>? terms, EntitySearchModelBuilder<T> searchModel) where T : class
    {
        if (terms == null) { return q; }

        foreach (var term in terms)
        {
            q = q.Where(searchModel.BuildPredicate(term));
        }

        return q;
    }
}