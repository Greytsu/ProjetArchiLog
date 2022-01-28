using ProjetArchiLog.Library.Data;
using ProjetArchiLog.Library.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProjetArchiLog.Library.Extensions;
using static ProjetArchiLog.Library.Extensions.ParamsExtension;
using Serilog;
using System.Collections;
using ProjetArchiLog.Library.Utils;

namespace ProjetArchiLog.Library.Controllers.v1
{
    [ApiController]
    public class BaseController<TContext, TModel> : ControllerBase where TContext : BaseDbContext where TModel : BaseModel
    {
        protected readonly TContext _context;
        protected static readonly String[] ApiParams = { "Sort", "Page", "Size", "Fields" };

        public BaseController(TContext context)
        {
            _context = context;
        }

        [HttpGet]
        [Route("api/v1/[controller]")]
        public async Task<ActionResult<IEnumerable<TModel>>> GetAll([FromQuery] PaginationParams paginationModel, [FromQuery] SortingParams sortParams, [FromQuery] string? fields)
        {
            Log.Information("GET ALL {0}" , typeof(TModel).Name);

            PaginationParams validPaginationParams = new PaginationParams(paginationModel);

            List<dynamic> badParams = CheckAllParams<TModel>(ApiParams, this.Request.Query.Keys, sortParams, fields);
            if (badParams.Count > 0)
                return BadRequest(badParams);

            var getRequest = _context.Set<TModel>().Where(x => !x.IsDeleted);

            getRequest = getRequest.HandleSorting(sortParams);

            this.Response.Headers.Add("Link", string.Join(",", validPaginationParams.PagingHeader<TContext, TModel>(_context, this.Request)));

            return await getRequest
                .Skip((validPaginationParams.page - 1) * validPaginationParams.size)
                .Take(validPaginationParams.size)
                .ToListAsync();
        }

        [HttpGet]
        [Route("api/v1/[controller]/search")]
        public async Task<ActionResult<IEnumerable<TModel>>> Search([FromQuery] PaginationParams paginationModel, [FromQuery] SortingParams SortParams, [FromQuery] string? fields)
        {
            Log.Information("SEARCH MODEL{0}" , typeof(TModel).Name);

            PaginationParams validPaginationParams = new PaginationParams(paginationModel);

            List<dynamic> BadParams = CheckAllParams<TModel>(ApiParams, this.Request.Query.Keys, SortParams, fields);
            if (BadParams.Count > 0)
                return BadRequest(BadParams);

            var GetRequest = _context.Set<TModel>().Where(x => !x.IsDeleted);

            GetRequest = GetRequest.HandleSearch(this.Request.Query);

            GetRequest = GetRequest.HandleSorting(SortParams);

            this.Response.Headers.Add("Link", string.Join(",", validPaginationParams.PagingHeader<TContext, TModel>(_context, this.Request)));

            return await GetRequest
                .Skip((validPaginationParams.page - 1) * validPaginationParams.size)
                .Take(validPaginationParams.size)
                .ToListAsync();
        }

        [HttpGet]
        [Route("api/v1/[controller]/{id}")]
        public async Task<ActionResult<TModel>> GetOneById(Guid id)
        {
            var customer =  await _context.Set<TModel>().SingleOrDefaultAsync(x => x.Id == (id) && !x.IsDeleted);

            if (customer == null)
            {
                return NotFound();
            }

            return customer;
        }

        [HttpPut]
        [Route("api/v1/[controller]/{id}")]
        public async Task<IActionResult> PutCustomer(Guid id, TModel model)
        {
            if (id != model.Id)
            {
                return BadRequest();
            }

            if (!ModelExists(id))
            {
                return NotFound();
            }

            _context.Entry(model).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                    throw;
            }

            return NoContent();
        }

        [HttpPost]
        [Route("api/v1/[controller]")]
        public async Task<ActionResult<TModel>> PostCustomer(TModel model)
        {
            _context.Set<TModel>().Add(model);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetOneById", new { id = model.Id }, model);
        }

        [HttpDelete]
        [Route("api/v1/[controller]/{id}")]
        public async Task<IActionResult> DeleteCustomer(Guid id)
        {
            var context = _context.Set<TModel>();
            var item = await context.FindAsync(id);
            if (item == null)
            {
                return NotFound();
            }

            context.Remove(item);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool ModelExists(Guid id)
        {
            return _context.Set<TModel>().Any(e => e.Id == id && !e.IsDeleted);
        }
    }
}
