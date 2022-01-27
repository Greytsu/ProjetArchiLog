﻿using ProjetArchiLog.Library.Data;
using ProjetArchiLog.Library.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProjetArchiLog.Library.Extensions;
using Serilog;
using System.Collections;

namespace ProjetArchiLog.Library.Controllers.v1
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public abstract class BaseController<TContext, TModel> : ControllerBase where TContext : BaseDbContext where TModel : BaseModel
    {
        protected readonly TContext _context;
        protected static readonly String[] API_PARAMS = { "Sort", "Page", "Size" };

        public BaseController(TContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<TModel>>> GetAll([FromQuery] SortingParams SortParams)
        {
            Log.Information("GET ALL " + typeof(TModel).Name);

            //checking if bad params in request
            List<string> BadParams = this.Request.Query.Keys.CheckParams<TModel>(API_PARAMS);
            if (BadParams.Count > 0)
                return BadRequest("Incorrect query params : " + string.Join(", ", BadParams));

            var GetRequest = _context.Set<TModel>().Where(x => !x.IsDeleted);

            GetRequest = GetRequest.HandleSorting(SortParams);
            GetRequest = GetRequest.HandleFiltering<TModel>(this.Request.Query);

            return await GetRequest.ToListAsync();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<TModel>> GetOneById(Guid id)
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

            return CreatedAtAction("GetOneById", new { id = model.Id }, model);
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
