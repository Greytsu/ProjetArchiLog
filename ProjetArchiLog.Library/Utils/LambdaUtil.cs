using System.Linq.Expressions;
using System.Reflection;

namespace ProjetArchiLog.Library.Utils
{
    public static class LambdaUtil
    {
        public static Expression<Func<TModel, object>> ToLambda<TModel>(string propertyName)
        {
            var parameter = Expression.Parameter(typeof(TModel));
            var property = Expression.Property(parameter, propertyName);
            var propAsObject = Expression.Convert(property, typeof(object));

            return Expression.Lambda<Func<TModel, object>>(propAsObject, parameter);
        }

        public static IQueryable<TModel> ToLambaIfStatement<TModel>(this IQueryable<TModel> Query, string ifStatement, string propertyName, string value)
        {
            MethodInfo methodInfo = typeof(string).GetMethod(ifStatement, new[] { typeof(string) });
            var parameter = Expression.Parameter(typeof(TModel));
            var property = Expression.Property(parameter, propertyName);
            var condition = Expression.Call(property, methodInfo, new Expression[] { Expression.Constant(value, typeof(string)) });

            return Query.Where(Expression.Lambda<Func<TModel, bool>>(condition, parameter));
        }
    }
}
