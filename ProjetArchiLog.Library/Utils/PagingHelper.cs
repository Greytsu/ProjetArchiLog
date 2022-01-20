using Microsoft.AspNetCore.Http;
using ProjetArchiLog.Library.Data;
using ProjetArchiLog.Library.Models;
using ProjetArchiLog.Library.Services;

namespace ProjetArchiLog.Library.Utils
{
    public class PagingHelper<TContext, TModel> where TContext : BaseDbContext where TModel : BaseModel
    {
        private const string LinkHeaderTemplate = "<{0}>; rel=\"{1}\"";
        public Uri firstPage { get; private set; }
        public Uri lastPage { get; private set; }
        public Uri ?previousPage { get; private set; }
        public Uri ?nextPage { get; private set; }
        public PagingHelper(TContext _context, HttpRequest request, PaginationParams paginationParams)
        {
            string route = string.Concat(request.Scheme, "://", request.Host.ToUriComponent(), request.Path.ToUriComponent());

            int totalItem = _context.Set<TModel>().Count();
            int totalPage = totalItem > 0
                ? (int)Math.Ceiling(totalItem / (double)paginationParams.size)
                : 0;

            this.firstPage = UriService.GetUri(route, 1, paginationParams.size);
            this.lastPage = UriService.GetUri(route, totalPage, paginationParams.size);
            this.previousPage = paginationParams.page > 1 ? UriService.GetUri(route, paginationParams.page - 1, paginationParams.size) : null;
            this.nextPage = paginationParams.page >= 1 && paginationParams.page < totalPage ? UriService.GetUri(route, paginationParams.page + 1, paginationParams.size) : null;
        }

        public List<String> PagingHeader()
        {
            List<String> header = new List<String>();

            if (this.firstPage != null) header.Add(string.Format(LinkHeaderTemplate, this.firstPage, "first"));
            if (this.lastPage != null) header.Add(string.Format(LinkHeaderTemplate, this.lastPage, "last"));
            if (this.previousPage != null) header.Add(string.Format(LinkHeaderTemplate, this.previousPage, "prev"));
            if (this.nextPage != null) header.Add(string.Format(LinkHeaderTemplate, this.nextPage, "next"));

            return header;
        }
    }
}
