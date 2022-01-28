using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Primitives;
using static ProjetArchiLog.Library.Extensions.ParamsExtension;

namespace ProjetArchiLog.Library.Services
{
    public static class UriService
    {
        public static Uri GetUri(this HttpRequest request, int page, int size)
        {
            Uri uri = new Uri(string.Concat(request.Scheme, "://", request.Host.ToUriComponent(), request.Path.ToUriComponent()));
            IEnumerable<KeyValuePair<string, StringValues>> queryParams = request.ParamsWithoutPaging();

            string modifiedUri = QueryHelpers.AddQueryString(uri.ToString(), "page", page.ToString());
            modifiedUri = QueryHelpers.AddQueryString(modifiedUri, "size", size.ToString());
            modifiedUri = QueryHelpers.AddQueryString(modifiedUri, queryParams);

            return new Uri(modifiedUri);
        }
    }
}
