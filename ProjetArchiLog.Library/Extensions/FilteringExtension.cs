using System.Collections;
using Microsoft.AspNetCore.Http;
using ProjetArchiLog.Library.Models;
using ProjetArchiLog.Library.Utils;
using Serilog;

namespace ProjetArchiLog.Library.Extensions
{
    public static class FilteringExtension
    {
        public static IQueryable<TModel> HandleFiltering<TModel>(this IQueryable<TModel> Query, IQueryCollection QueryParams)
        {
            Log.Information("Filtering");
            FilteringParams<TModel> filteringParams = new FilteringParams<TModel>(QueryParams);

            foreach (var filter in filteringParams.Filters)
            {
                Query = Query.ToLambdaFilter<TModel>(filter.Key, filter.Value);
            }

            return Query;
        }
    }
}
