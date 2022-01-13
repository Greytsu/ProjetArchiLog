#nullable disable
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProjetArchiLog.API.Data;
using ProjetArchiLog.API.Models;
using ProjetArchiLog.Library.Controllers;

namespace ProjetArchiLog.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : BaseController<ArchiDbContext, Product>
    { 
        public ProductsController(ArchiDbContext context):base(context)
        {
        }

    }
}
