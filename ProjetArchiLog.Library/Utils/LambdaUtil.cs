using System.Linq.Expressions;
using System.Reflection;

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
            var type = typeof(TModel).GetProperty(propertyName, BindingFlags.IgnoreCase | BindingFlags.Public | BindingFlags.Instance)?.PropertyType;
            
            var parameter = Expression.Parameter(typeof(TModel));
            var property = Expression.Property(parameter, propertyName);
            Expression filteredQuery = null;

            if (type == typeof(string))
            {
                Console.WriteLine("string");
                if (propertyValues.Contains("["))
                    throw new Exception("String type can't be filtered by an array");

                MethodInfo? methodInfo = typeof(string).GetMethod("Equals", new[] { typeof(string) });
                foreach (var param in propertyValues.Split(","))
                {
                    var condition = Expression.Call(property, methodInfo, new Expression[] { Expression.Constant(param, typeof(string)) });
                    filteredQuery = filteredQuery == null ? condition : Expression.Or(filteredQuery, condition);
                }
            }
            
            else if (type == typeof(int))
            {
                Console.WriteLine("int");
                if (propertyValues.Contains("["))
                {
                    var paramValues = propertyValues.Replace("[", "").Replace("]","").Split(",");
                    if (paramValues.Length == 2)
                    {
                        int paramInt = 0;
                        Int32.TryParse(paramValues[0], out paramInt);

                        MethodInfo? methodInfo = typeof(int).GetMethod("CompareTo", new[] { typeof(int) });
                        if(methodInfo == null) Console.WriteLine("null");
                        var condition = Expression.Call(property, methodInfo, new Expression[] { Expression.Constant(paramInt, typeof(int)) });
                        filteredQuery = condition;
                        
                        Int32.TryParse(paramValues[1], out paramInt);
                        
                        methodInfo = typeof(int).GetMethod("CompareTo", new[] { typeof(int) });
                        condition = Expression.Call(property, methodInfo, new Expression[] { Expression.Constant(paramInt, typeof(int)) });
                        filteredQuery = condition;
                    }
                }
                else
                {
                    MethodInfo? methodInfo = typeof(int).GetMethod("Equals", new[] { typeof(int) });
                    foreach (string param in propertyValues.Split(","))
                    {
                        int paramInt = 0;
                        Int32.TryParse(param, out paramInt);
                        
                        var condition = Expression.Call(property, methodInfo, new Expression[] { Expression.Constant(paramInt, typeof(int)) });
                        filteredQuery = filteredQuery == null ? condition : Expression.Or(filteredQuery, condition);
                    }
                }
            }
            
            else if (type == typeof(DateTime))
            {
                Console.WriteLine("DateTime");
            }
            
            
            if(filteredQuery != null)
                return Query.Where(Expression.Lambda<Func<TModel, bool>>(filteredQuery, parameter));

            throw new Exception("This property can't be filtered");
        }
    }
}
