using Microsoft.AspNetCore.Http;
using ProjetArchiLog.Library.Data;
using ProjetArchiLog.Library.Models;
using static ProjetArchiLog.Library.Services.UriService;

namespace ProjetArchiLog.Library.Utils
{
    public static class PagingExtensions
    {
        public static List<String> PagingHeader<TContext, TModel>(this PaginationParams paginationParams, TContext _context, HttpRequest request) where TContext : BaseDbContext where TModel : BaseModel
        {
            string LinkHeaderTemplate = "<{0}>; rel=\"{1}\"";
            List<String> header = new List<String>();

            int totalItem = _context.Set<TModel>().Count();
            int totalPage = totalItem > 0
                ? (int)Math.Ceiling(totalItem / (double)paginationParams.size)
                : 0;

            header.Add(string.Format(LinkHeaderTemplate, request.GetUri(1, paginationParams.size), "first"));
            header.Add(string.Format(LinkHeaderTemplate, request.GetUri(totalPage, paginationParams.size), "last"));
            if (paginationParams.page > 1) 
            header.Add(string.Format(LinkHeaderTemplate, request.GetUri(paginationParams.page - 1, paginationParams.size), "prev"));
            if (paginationParams.page >= 1 && paginationParams.page < totalPage) 
            header.Add(string.Format(LinkHeaderTemplate, request.GetUri(paginationParams.page + 1, paginationParams.size), "next"));

            return header;
        }
    }
}
