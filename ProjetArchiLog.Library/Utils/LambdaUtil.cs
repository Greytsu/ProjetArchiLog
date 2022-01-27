using System.Linq.Expressions;
using System.Reflection;
using Microsoft.VisualBasic.CompilerServices;

namespace ProjetArchiLog.Library.Utils
{
    public static  class LambdaUtil
    {
        public static Expression<Func<TModel, object>> ToLambda<TModel>(string propertyName)
        {
            var parameter = Expression.Parameter(typeof(TModel));
            var property = Expression.Property(parameter, propertyName);
            var propAsObject = Expression.Convert(property, typeof(object));

            return Expression.Lambda<Func<TModel, object>>(propAsObject, parameter);
        }
        
        public static IQueryable<TModel> ToLambdaFilter<TModel>(this IQueryable<TModel> Query, string propertyName, string propertyValues)
        {
            var type = typeof(TModel).GetProperty(propertyName, System.Reflection.BindingFlags.IgnoreCase | System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Instance)?.PropertyType;
            
            var parameter = Expression.Parameter(typeof(TModel));
            var property = Expression.Property(parameter, propertyName);

            if (type == typeof(string))
            {
                Console.WriteLine("string");
                if (propertyValues.Contains("["))
                    throw new Exception("String type can't be filtered by an array");

                MethodInfo? methodInfo = typeof(string).GetMethod("Equals", new[] { typeof(string) });
                Expression filteredQuery = null;
                foreach (var param in propertyValues.Split(","))
                {
                    var condition = Expression.Call(property, methodInfo, new Expression[] { Expression.Constant(param, typeof(string)) });
                    filteredQuery = filteredQuery == null ? condition : Expression.Or(filteredQuery, condition);
                }

                return Query.Where(Expression.Lambda<Func<TModel, bool>>(filteredQuery, parameter));
            }
            
            else if (type == typeof(int))
            {
                Console.WriteLine("int");
                if (propertyValues.Contains("["))
                {
                    var paramValues = propertyValues.Replace("[", "").Replace("]","").Split(",");
                    Console.WriteLine(paramValues);
                }
                else
                {
                    MethodInfo? methodInfo = typeof(int).GetMethod("Equals", new[] { typeof(int) });
                    Expression filteredQuery = null;
                    foreach (var param in propertyValues.Split(","))
                    {
                        var condition = Expression.Call(property, methodInfo, new Expression[] { Expression.Constant(param, typeof(int)) });
                        filteredQuery = filteredQuery == null ? condition : Expression.Or(filteredQuery, condition);
                    }
                }
            }
            
            else if (type == typeof(DateTime))
            {
                Console.WriteLine("DateTime");
            }

            var propAsObject = Expression.Convert(property, typeof(object));

            throw new Exception("This property can't be filtered");
        }
    }
}
