using Microsoft.AspNetCore.WebUtilities;

namespace ProjetArchiLog.Library.Services
{
    public class UriService
    {
        public static Uri GetUri(string route, int page, int size)
        {
            var uri = new Uri(route);
            var modifiedUri = QueryHelpers.AddQueryString(uri.ToString(), "page", page.ToString());
            modifiedUri = QueryHelpers.AddQueryString(modifiedUri, "size", size.ToString());
            //uri = QueryHelpers.AddQueryString(_baseUri, queryParams);

            return new Uri(modifiedUri);
        }
    }
}
