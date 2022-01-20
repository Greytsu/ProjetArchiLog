using ProjetArchiLog.Library.Models;
using static ProjetArchiLog.Library.Utils.ExistUtil;
using static ProjetArchiLog.Library.Utils.LambdaUtil;

namespace ProjetArchiLog.Library.Extensions
{
    public static class SortingExtension
    {
        public static IQueryable<TModel> HandleSorting<TModel>(this IQueryable<TModel> Query, SortingParams Params)
        {
            if (!Params.HasSort())
                return Query;

            String[] SortingParams = Params.GetParams();

            String Collumn = SortingParams[0].Split("-")[0];
            if (!ExistProperty<TModel>(Collumn))
                throw new Exception();

            IOrderedQueryable<TModel> SortedQuery = SortingParams[0].ToLower().Contains("desc") ?
                Query.OrderByDescending(ToLambda<TModel>(Collumn)) :
                Query.OrderBy(ToLambda<TModel>(Collumn));

            for (int i = 1; i < SortingParams.Length; i++)
            {
                Collumn = SortingParams[i].Split("-")[0];
                if (!ExistProperty<TModel>(Collumn))
                    throw new Exception();

                SortedQuery = SortingParams[i].ToLower().Contains("desc") ?
                    SortedQuery.ThenByDescending(ToLambda<TModel>(Collumn)) :
                    SortedQuery.ThenBy(ToLambda<TModel>(Collumn));
            }

            return SortedQuery;
        }
    }
}
