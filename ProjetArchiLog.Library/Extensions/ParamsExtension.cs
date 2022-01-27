﻿using Microsoft.AspNetCore.Http;
using System.Collections;

namespace ProjetArchiLog.Library.Extensions
{
    public static class ParamsExtension
    {

        //Ckecks if query's params are in models properties or in api's params
        public static List<string> CheckParams<TModel>(this ICollection<string> QueryParamsNames, String[] ApiParams)
        {
            List<string> ModelPropertiesNames = GetModelProperties<TModel>();

            List<string> BadParams = new();
            foreach (var paramValue in QueryParamsNames)
                if (!ApiParams.Contains(paramValue, StringComparer.OrdinalIgnoreCase) && !ModelPropertiesNames.Contains(paramValue, StringComparer.OrdinalIgnoreCase))
                    BadParams.Add(paramValue);

            return BadParams;
        }

        //returns a map of query params that are present in TModel
        public static IEnumerable<KeyValuePair<string, Microsoft.Extensions.Primitives.StringValues>> ExtractModelProperties<TModel>(this IQueryCollection QueryParams)
        {
            List<string> ModelPropertiesNames = GetModelProperties<TModel>();

            return QueryParams.IntersectBy(ModelPropertiesNames, x=>x.Key.ToLower());
                
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
