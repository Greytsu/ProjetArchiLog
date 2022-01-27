using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using static ProjetArchiLog.Library.Utils.LambdaUtil;

namespace ProjetArchiLog.Library.Extensions
{
    public static class SearchExtension
    {
        public static IQueryable<TModel> HandleSearch<TModel> (this IQueryable<TModel> Query, IQueryCollection QueryParams)
        {
            Console.WriteLine("test1");
            var SearchParams = QueryParams.ExtractModelProperties<TModel>();

            foreach (var param in SearchParams)
            {
                var properties = param.Key;
                var values = param.Value.ToString().Split(",");

                foreach (var value in values)
                {
                    var valueCopy = value.Replace("*", "");
                    string method = "Equals";

                    if (value.StartsWith("*") && value.EndsWith("*"))
                        method = "Contains";
                    else if (value.StartsWith("*"))
                        method = "EndsWith";
                    else if (value.EndsWith("*"))
                        method = "StartsWith";

                    Query = Query.ToLambaIfStatement<TModel>(method, properties, valueCopy);
                }

                Console.WriteLine(values);
            }

            return Query;
        }
    }
}
