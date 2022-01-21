using System.Collections;

namespace ProjetArchiLog.Library.Extensions
{
    public static class ParamsCheckerExtension
    {

        //Ckecks if query's params are in models properties or in api's params
        public static List<string> CheckParams<TModel>(this ICollection<string> QueryParams, String[] ApiParams)
        {
            List<string> ModelPropertiesNames = new();
            var ModelProperties = typeof(TModel).GetProperties().ToList();
            foreach (var Property in ModelProperties)
                ModelPropertiesNames.Add(Property.Name);

            List<string> BadParams = new();
            foreach (var paramValue in QueryParams)
                if (!ApiParams.Contains(paramValue, StringComparer.OrdinalIgnoreCase) && !ModelPropertiesNames.Contains(paramValue, StringComparer.OrdinalIgnoreCase))
                    BadParams.Add(paramValue);

            return BadParams;
        }
    }
}
