using System.Linq.Expressions;
using System.Reflection;
using ProjetArchiLog.Library.Extensions;

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
        
        public static IQueryable<TModel> ToLambdaFilter<TModel>(this IQueryable<TModel> query, string propertyName, string propertyValues)
        {
            Type? type = typeof(TModel).GetProperty(propertyName, BindingFlags.IgnoreCase | BindingFlags.Public | BindingFlags.Instance)?.PropertyType;
            if (!(type == typeof(string) || type == typeof(int) || type == typeof(DateTime)))
            {
                throw new Exception("This property type can't be filtered : " + propertyName);
            }

            var parameter = Expression.Parameter(typeof(TModel));
            var property = Expression.Property(parameter, propertyName);
            BinaryExpression filteredQuery = null;
            
            if (propertyValues.StartsWith("[") && propertyValues.EndsWith("]"))
            {

                if (type == typeof(string))
                    throw new Exception("String type can't be filtered by an array");

                var range = propertyValues.Replace("[", "").Replace("]", "").Split(",");
                if (range.Length != 2)
                    throw new Exception("Range filtering only accepts 2 arguments");

                if (range[0] != "")
                {
                    filteredQuery = Expression.GreaterThanOrEqual(property, Expression.Convert(Expression.Constant(Convert.ChangeType(range[0], type)), type));
                }

                if (range[1] != "")
                {
                    var condition = Expression.LessThanOrEqual(property, Expression.Convert(Expression.Constant(Convert.ChangeType(range[1], type)), type));
                    filteredQuery = filteredQuery == null ? condition : Expression.And(filteredQuery, condition);
                }


            }
            else
            {
                foreach (var value in propertyValues.Split(","))
                {
                    var condition = Expression.Equal(property, Expression.Convert(Expression.Constant(Convert.ChangeType(value, type)), type));
                    filteredQuery = filteredQuery == null ? condition : Expression.Or(filteredQuery, condition);
                }
            }


            if (filteredQuery != null)
                return query.Where(Expression.Lambda<Func<TModel, bool>>(filteredQuery, parameter));

            throw new Exception("Failed to filter");
        }

        public static IQueryable<TModel> ToLambaIfStatement<TModel>(this IQueryable<TModel> query, string ifStatement, string propertyName, string value)
        {
            MethodInfo methodInfo = typeof(string).GetMethod(ifStatement, new[] { typeof(string) });
            var parameter = Expression.Parameter(typeof(TModel));
            var property = Expression.Property(parameter, propertyName);
            var condition = Expression.Call(property, methodInfo, new Expression[] { Expression.Constant(value, typeof(string)) });

            return query.Where(Expression.Lambda<Func<TModel, bool>>(condition, parameter));
        }
    }
}
