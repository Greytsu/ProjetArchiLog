using System.Linq.Expressions;

namespace ProjetArchiLog.Library.Utils
{
    internal class LambdaUtil
    {
        public static Expression<Func<TModel, object>> ToLambda<TModel>(string propertyName)
        {
            var parameter = Expression.Parameter(typeof(TModel));
            var property = Expression.Property(parameter, propertyName);
            var propAsObject = Expression.Convert(property, typeof(object));

            return Expression.Lambda<Func<TModel, object>>(propAsObject, parameter);
        }
    }
}
