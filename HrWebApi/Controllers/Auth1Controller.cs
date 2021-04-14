using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using HrWebApi.Models;
using HrWebApi.ViewModels;
using System.Globalization;
using Microsoft.EntityFrameworkCore;

namespace HrWebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class Auth1Controller : ControllerBase
    {
        HrDepContext db;
        public Auth1Controller(HrDepContext context)
        {
            CultureInfo culture = new CultureInfo("ru-RU");
            db = context;
            if (!db.User.Any())
            {
                db.User.Add(new User { Name = "Tom", Email = "tom@mail.ru", Password = BCrypt.Net.BCrypt.HashPassword("asdasd"), BirthDate = Convert.ToDateTime("2009/02/03", culture), RegDate = DateTime.Now });
                db.User.Add(new User { Name = "Alice", Email = "alice@mail.ru", Password = BCrypt.Net.BCrypt.HashPassword("asdasd"), BirthDate = Convert.ToDateTime("2004/01/23", culture), RegDate = DateTime.Now });
                db.SaveChanges();
            }

        }

        public async Task<ActionResult<IEnumerable<User>>> Index() {

            return await db.User.ToListAsync();
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<User>>> GetUser()
        {

            return await db.User.Include(u => u.Company).Include(u=>u.Role).ToListAsync();
        }
    }
}
