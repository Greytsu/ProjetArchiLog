using Microsoft.AspNetCore.Mvc;
using ProjetArchiLog.API.Data;
using ProjetArchiLog.API.Models;
using ProjetArchiLog.Library.Controllers.v2;

namespace ProjetArchiLog.API.Controllers.v2
{
    public class ProductsController : BaseController<ArchiDbContext, Product>
    {
        public ProductsController(ArchiDbContext context) : base(context)
        {
        }

    }
}
