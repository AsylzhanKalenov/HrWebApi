using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using HrWebApi.Models;
using HrWebApi.ViewModels;

namespace HrWebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserPrivsController : ControllerBase
    {
        private readonly HrDepContext _context;

        public UserPrivsController(HrDepContext context)
        {
            _context = context;
        }

        // GET: api/UserPrivs
        [HttpGet]
        public async Task<ActionResult<IEnumerable<UserPriv>>> GetUserPriv()
        {
            return await _context.UserPriv.ToListAsync();
        }

        // GET: api/UserPrivs/5
        [HttpGet("{userid}")]
        public async Task<ActionResult<PrivViewModel>> GetUserPriv(int userid)
        {
            var userPriv =  _context.UserPriv.FirstOrDefault(u=>u.UserId==userid);
            var child = await _context.Children.Where(c=>c.UserId == userid).ToArrayAsync<Children>();
            
            if (userPriv == null)
            {
                return NotFound();
            }

            PrivViewModel priv = new PrivViewModel
            {
                id = userPriv.Id,
                userid = userPriv.UserId,
                iin = userPriv.Iin,
                address = userPriv.Address,
                ismarrige = userPriv.IsMarriage
            };
            priv.Children = new Childrens[child.Length];
            for (int i = 0; i < child.Length; i++) {
                priv.Children[i] = new Childrens();
                priv.Children[i].fio = child[i].Surname ?? ("") + " ";
                priv.Children[i].fio += " ";
                priv.Children[i].fio += child[i].Name ?? ("");
                priv.Children[i].fio += " ";
                priv.Children[i].fio += child[i].Lastname ?? ("");
                priv.Children[i].BirthDate = child[i].BirthDate != null ? child[i].BirthDate : DateTime.Now;
            }

            return priv;
        }

        // PUT: api/UserPrivs/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPut("{id}")]
        public async Task<IActionResult> PutUserPriv(int id, UserPriv userPriv)
        {
            if (id != userPriv.Id)
            {
                return BadRequest();
            }

            _context.Entry(userPriv).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!UserPrivExists(id))
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

        // POST: api/UserPrivs
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPost]
        public async Task<ActionResult<PrivViewModel>> PostUserPriv(PrivViewModel userPriv)
        {

            UserPriv user1 = _context.UserPriv.FirstOrDefault(u=> u.UserId==userPriv.userid);

            if (user1 != null)
            {
                user1.Iin = userPriv.iin;
                user1.Address = userPriv.address;
                user1.IsMarriage = userPriv.ismarrige;

                _context.Entry(user1).State = EntityState.Modified;
            }
            else
            {
                user1 = new UserPriv { Iin = userPriv.iin, Address = userPriv.address, IsMarriage = userPriv.ismarrige, UserId = userPriv.userid };
                _context.UserPriv.Add(user1);
            }

            var dbchild =  _context.Children.Where(u=>u.UserId== userPriv.userid);

            _context.Children.RemoveRange(dbchild);

            foreach (Childrens c in userPriv.Children)
            {
                string[] str = c.fio.Split(' ');
                Children child = new Children();
                child.Surname = str[0];
                if (str.Length >= 2)
                    child.Name = str[1];
                if (str.Length >= 3)
                    child.Lastname = str[2];

                child.BirthDate = c.BirthDate;
                child.UserId = userPriv.userid;
                _context.Children.Add(child);
            }

            await _context.SaveChangesAsync();

            return CreatedAtAction("GetUserPriv", new { id = user1.Id }, userPriv);
        }

        // DELETE: api/UserPrivs/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<UserPriv>> DeleteUserPriv(int id)
        {
            var userPriv = await _context.UserPriv.FindAsync(id);
            if (userPriv == null)
            {
                return NotFound();
            }

            _context.UserPriv.Remove(userPriv);
            await _context.SaveChangesAsync();

            return userPriv;
        }

        private bool UserPrivExists(int id)
        {
            return _context.UserPriv.Any(e => e.Id == id);
        }
    }
}
