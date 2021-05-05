using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using HrWebApi.Models;

namespace HrWebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CheckCatsController : ControllerBase
    {
        private readonly HrDepContext _context;

        public CheckCatsController(HrDepContext context)
        {
            _context = context;
        }

        // GET: api/CheckCats
        [HttpGet]
        public async Task<ActionResult<IEnumerable<CheckCat>>> GetCheckCat()
        {
            return await _context.CheckCat.ToListAsync();
        }

        // GET: api/CheckCats/5
        [HttpGet("{id}")]
        public async Task<ActionResult<CheckCat>> GetCheckCat(int id)
        {
            var checkCat = await _context.CheckCat.FindAsync(id);

            if (checkCat == null)
            {
                return NotFound();
            }

            return checkCat;
        }

        // PUT: api/CheckCats/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPut("{id}")]
        public async Task<IActionResult> PutCheckCat(int id, CheckCat checkCat)
        {
            if (id != checkCat.Id)
            {
                return BadRequest();
            }

            _context.Entry(checkCat).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CheckCatExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/CheckCats
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPost]
        public async Task<ActionResult<CheckCat>> PostCheckCat(CheckCat checkCat)
        {
            CheckCat ch = await _context.CheckCat.FirstOrDefaultAsync(c=>c.UserId==checkCat.UserId && c.FileCatId==checkCat.FileCatId);

            if (ch != null) 
                ch.Comments = checkCat.Comments;
            else
            _context.CheckCat.Add(checkCat);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetCheckCat", new { id = checkCat.Id }, checkCat);
        }

        // DELETE: api/CheckCats/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<CheckCat>> DeleteCheckCat(int id)
        {
            var checkCat = await _context.CheckCat.FindAsync(id);
            if (checkCat == null)
            {
                return NotFound();
            }

            _context.CheckCat.Remove(checkCat);
            await _context.SaveChangesAsync();

            return checkCat;
        }

        private bool CheckCatExists(int id)
        {
            return _context.CheckCat.Any(e => e.Id == id);
        }
    }
}
