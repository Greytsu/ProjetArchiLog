using Microsoft.AspNetCore.Mvc;
using ProjetArchiLog.API.Data;
using ProjetArchiLog.API.Models;
using ProjetArchiLog.Library.Controllers.v1;

namespace ProjetArchiLog.API.Controllers.v1
{
    public class ProductsController : BaseController<ArchiDbContext, Product>
    {
        public ProductsController(ArchiDbContext context) : base(context)
        {
        }

    }
}
