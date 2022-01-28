using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ProjetArchiLog.Library.Models;
using static ProjetArchiLog.Library.Utils.ExistUtil;
using static ProjetArchiLog.Library.Utils.LambdaUtil;

namespace ProjetArchiLog.Library.Extensions
{
    public static class SortingExtension
    {
        public static IQueryable<TModel> HandleSorting<TModel>(this IQueryable<TModel> query, SortingParams Params)
        {
            if (!Params.HasSort())
                return query;

            String[] sortingParams = Params.GetParams();

            String collumn = sortingParams[0].Split("-")[0];
            if (!ExistProperty<TModel>(collumn))
                throw new Exception();

            IOrderedQueryable<TModel> sortedQuery = sortingParams[0].ToLower().Contains("desc") ?
                query.OrderByDescending(ToLambda<TModel>(collumn)) :
                query.OrderBy(ToLambda<TModel>(collumn));

            for (int i = 1; i < sortingParams.Length; i++)
            {
                collumn = sortingParams[i].Split("-")[0];
                if (!ExistProperty<TModel>(collumn))
                    throw new Exception();

                sortedQuery = sortingParams[i].ToLower().Contains("desc") ?
                    sortedQuery.ThenByDescending(ToLambda<TModel>(collumn)) :
                    sortedQuery.ThenBy(ToLambda<TModel>(collumn));
            }

            return sortedQuery;
        }
    }
}
