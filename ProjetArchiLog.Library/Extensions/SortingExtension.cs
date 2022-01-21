using ProjetArchiLog.Library.Models;
using ProjetArchiLog.Library.Utils;

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
            Console.WriteLine(Collumn);

            IOrderedQueryable<TModel> SortedQuery = SortingParams[0].ToLower().Contains("desc") ?
                Query.OrderByDescending(LambdaUtil.ToLambda<TModel>(Collumn)) :
                Query.OrderBy(LambdaUtil.ToLambda<TModel>(Collumn));

            for (int i = 1; i < SortingParams.Length; i++)
            {
                Collumn = SortingParams[i].Split("-")[0];
                Console.WriteLine(Collumn);

                SortedQuery = SortingParams[i].ToLower().Contains("desc") ?
                    SortedQuery.ThenByDescending(LambdaUtil.ToLambda<TModel>(Collumn)) :
                    SortedQuery.ThenBy(LambdaUtil.ToLambda<TModel>(Collumn));
            }

            return SortedQuery;
        }
    }
}
