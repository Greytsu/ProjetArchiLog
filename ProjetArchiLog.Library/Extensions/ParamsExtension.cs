using Microsoft.AspNetCore.Http;
using ProjetArchiLog.Library.Models;
using System.Collections;

namespace ProjetArchiLog.Library.Extensions
{
    public static class ParamsExtension
    {

        //Ckecks if query's params are in models properties or in api's params
        public static List<string> CheckParamsKeys<TModel>(this ICollection<string> QueryParamsNames, String[] ApiParams)
        {
            List<string> ModelPropertiesNames = GetModelProperties<TModel>();

            List<string> BadParams = new();
            foreach (var paramValue in QueryParamsNames)
                if (!ApiParams.Contains(paramValue, StringComparer.OrdinalIgnoreCase) && !ModelPropertiesNames.Contains(paramValue, StringComparer.OrdinalIgnoreCase))
                    BadParams.Add(paramValue);

            return BadParams;
        }

        //Ckecks if fields's params are in models properties or in api's params
        public static List<string> CheckParamsProperties<TModel>(this String QueryParamsNames, String[] ApiParams)
        {

            if(QueryParamsNames == null || QueryParamsNames == "*")
                return new List<string>();

            string[] fields = QueryParamsNames.Split(",");
            List<string> ModelPropertiesNames = GetModelProperties<TModel>();

            List<string> BadParams = new();
            foreach (var paramValue in fields)
                if (!ApiParams.Contains(paramValue, StringComparer.OrdinalIgnoreCase) && !ModelPropertiesNames.Contains(paramValue, StringComparer.OrdinalIgnoreCase))
                    BadParams.Add(paramValue);

            return BadParams;
        }

        public static List<dynamic> CheckAllParams<TModel>(String[] ApiParams, ICollection<string> QueryParams, SortingParams SortParams, string Fields)
        {
            List<string> BadParamsKeys = QueryParams.CheckParamsKeys<TModel>(ApiParams);
            List<string> BadParamsFields = Fields.CheckParamsProperties<TModel>(ApiParams);
            List<string> BadParamsSort = SortParams.Sort.CheckParamsProperties<TModel>(ApiParams);

            var response = new List<dynamic>();

            if (BadParamsKeys.Count > 0)
            {
                response.Add(
                    new
                    {
                        error = "Incorrect query params",
                        fields = new
                        {
                            field = string.Join(", ", BadParamsKeys)
                        }
                    });
            }

            if (BadParamsFields.Count > 0)
            {
                response.Add(
                    new
                    {
                        error = "Incorrect propeties in param Fields",
                        fields = new
                        {
                            field = string.Join(", ", BadParamsFields)
                        }
                    });
            }

            if (BadParamsSort.Count > 0)
            {
                response.Add(
                    new
                    {
                        error = "Incorrect propeties in param Sort",
                        fields = new
                        {
                            field = string.Join(", ", BadParamsSort)
                        }
                    });
            }

            return response;
        }

        //returns a map of query params that are present in TModel
        public static IEnumerable<KeyValuePair<string, Microsoft.Extensions.Primitives.StringValues>> ExtractModelProperties<TModel>(this IQueryCollection QueryParams, bool stringOnly = false)
        {
            List<string> ModelPropertiesNames = GetModelProperties<TModel>();

            return stringOnly 
                ? QueryParams.IntersectBy(ModelPropertiesNames, x => x.Key.ToLower()).Where(x => x.GetType() == typeof(string))
                : QueryParams.IntersectBy(ModelPropertiesNames, x => x.Key.ToLower());

        }

        //returns a list of all the properties of TModel
        private static List<string> GetModelProperties<TModel>()
        {
            List<string> ModelPropertiesNames = new();
            var ModelProperties = typeof(TModel).GetProperties().ToList();
            foreach (var Property in ModelProperties)
                ModelPropertiesNames.Add(Property.Name.ToLower());

            return ModelPropertiesNames;
        }

    }
}
