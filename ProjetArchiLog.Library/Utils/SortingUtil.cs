using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using static ProjetArchiLog.Library.Utils.LambdaUtil;

namespace ProjetArchiLog.Library.Utils
{
    internal class SortingUtil
    {
        //Method to sort a Linq Query
        public static IOrderedQueryable<TModel> HandleSorting<TModel>(IQueryable<TModel> Query, String[] Sorts)
        {
            String Collumn = Sorts[0].Split("-")[0];
            IOrderedQueryable<TModel> SortedQuery = Sorts[0].ToLower().Contains("desc") ?
                Query.OrderByDescending(ToLambda<TModel>(Collumn)) :
                Query.OrderBy(ToLambda<TModel>(Collumn));

            for (int i = 1; i < Sorts.Length; i++)
            {
                Collumn = Sorts[i].Split("-")[0];
                SortedQuery = Sorts[i].ToLower().Contains("desc") ?
                    SortedQuery.ThenByDescending(ToLambda<TModel>(Collumn)):
                    SortedQuery.ThenBy(ToLambda<TModel>(Collumn));
            }

            return SortedQuery;
        }
    }
}
