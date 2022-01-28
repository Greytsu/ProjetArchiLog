using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Linq.Expressions;
using ProjetArchiLog.Library.Extensions;
using ProjetArchiLog.Library.Models;
using Serilog;
using ProjetArchiLog.Library.Data;
using System.Linq.Expressions;
using ProjetArchiLog.Library.Utils;

namespace ProjetArchiLog.Library.Controllers.v2
{
    [Route("api/v2/[controller]")]
    [ApiController]
    public class BaseController<TContext, TModel> : ControllerBase where TContext : BaseDbContext where TModel : BaseModel
    {
        protected readonly TContext _context;

        public BaseController(TContext context)
        {
            _context = context;
        }

        // GET: api/[Controller]
        [HttpGet]
        public async Task<ActionResult<IEnumerable<TModel>>> GetModels([FromQuery] PaginationParams paginationModel)
        {
            PaginationParams validPaginationParams = new PaginationParams(paginationModel);
           
            var pagingHelper = new PagingHelper<TContext, TModel>(_context, this.Request, validPaginationParams);

            var GetRequest = _context.Set<TModel>().Where(x => !x.IsDeleted);

            this.Response.Headers.Add("Link", string.Join(",", pagingHelper.PagingHeader()));
            return await GetRequest
                .Skip((validPaginationParams.page - 1) * validPaginationParams.size)
                .Take(validPaginationParams.size)
                .ToListAsync();
        public async Task<ActionResult<IEnumerable<TModel>>> GetAll([FromQuery] SortingParams SortParams)
        {
            Log.Information("GET ALL " + typeof(TModel).Name);

            var GetRequest = _context.Set<TModel>().Where(x => !x.IsDeleted);

            try
            {
                GetRequest = GetRequest.HandleSorting(SortParams);
            }
            catch (Exception ex)
            {
                return BadRequest("Illegal sort parameter(s)");
            }
            

            return await GetRequest.ToListAsync();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<TModel>> GetModel(Guid id)
        {
            var customer =  await _context.Set<TModel>().SingleOrDefaultAsync(x => x.Id == (id) && !x.IsDeleted);

            if (customer == null)
            {
                return NotFound();
            }

            return customer;
        }

        [HttpPut("{id}")]
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
        public async Task<ActionResult<TModel>> PostCustomer(TModel model)
        {
            _context.Set<TModel>().Add(model);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetModel", new { id = model.Id }, model);
        }

        [HttpDelete("{id}")]
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
