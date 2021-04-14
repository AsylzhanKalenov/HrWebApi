using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using HrWebApi.Data;
using HrWebApi.Models;
using HrWebApi.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;


namespace HrWebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        HrDepContext db;

        CultureInfo culture = new CultureInfo("ru-RU");
        public AuthController(HrDepContext context)
        {
            db = context;
            //db.User.Add(new User { Name = "Tom", Email = "tom@mail.ru", Password = BCrypt.Net.BCrypt.HashPassword("asdasd"), BirthDate = Convert.ToDateTime("2009/02/03", culture), RegDate = DateTime.Now, RoleId = 3, CompanyId = 2 });
            //db.User.Add(new User { Name = "Alice", Email = "alice1255@mail.ru", Password = BCrypt.Net.BCrypt.HashPassword("asdasd"), Status = 0, BirthDate = Convert.ToDateTime("2004/01/23", culture), RegDate = DateTime.Now, RoleId = 3, CompanyId = 3 });
            //db.User.Add(new User { Name = "BOb", Email = "alice12@mail.ru", Password = BCrypt.Net.BCrypt.HashPassword("asdasd"), Status = 1, BirthDate = Convert.ToDateTime("2004/01/23", culture), RegDate = DateTime.Now, RoleId = 3, CompanyId = 2 });
            //db.User.Add(new User { Name = "Asalice", Email = "alice125@mail.ru", Password = BCrypt.Net.BCrypt.HashPassword("asdasd"), Status = 1, BirthDate = Convert.ToDateTime("2004/01/23", culture), RegDate = DateTime.Now, RoleId = 3, CompanyId = 1 });
            //db.User.Add(new User { Name = "JAck", Email = "alice4575@mail.ru", Password = BCrypt.Net.BCrypt.HashPassword("asdasd"), Status = 2, BirthDate = Convert.ToDateTime("2004/01/23", culture), RegDate = DateTime.Now, RoleId = 3, CompanyId = 1 });
            //db.User.Add(new User { Name = "Dan", Email = "alice1232@mail.ru", Password = BCrypt.Net.BCrypt.HashPassword("asdasd"), BirthDate = Convert.ToDateTime("2004/01/23", culture), RegDate = DateTime.Now, RoleId = 3, CompanyId = 3 });

            db.SaveChanges();

        }
        [HttpGet]
        public async Task<ActionResult<IEnumerable<User>>> Get()
        {

            var users = await db.User.Include(u => u.Company).Include(u => u.Role).Include(u => u.Position).Where(u => u.Id!=1).ToListAsync();

            return new ObjectResult(users);
        }


        // GET api/users/email
        [HttpGet("{email}")]
        public async Task<ActionResult<User>> Get(string email)
        {
            User user = await db.User.FirstOrDefaultAsync(x => x.Email == email);
            if (user != null)
                return new ObjectResult(user);
            return NotFound();
        }
        [Route("Signin")]
        [HttpPost]
        public async Task<ActionResult<User>> Signin(Signin param)
        {
            User user = await db.User.FirstOrDefaultAsync(x => x.Email == param.Email);
            if (user != null && BCrypt.Net.BCrypt.Verify(param.Password, user.Password))
                return new ObjectResult(user);
            return NotFound();
        }
        [Route("GetUser")]
        [HttpGet]
        public async Task<ActionResult<IEnumerable<User>>> GetUser(string email, string name, string company, int position)
        {
            var res = await db.User.Include(u => u.Company).Include(u => u.Role).Include(u => u.Position).Where(u=>u.Id != 1).OrderBy(o=>o.Id).ToListAsync();

            if (email != null) {
                res = res.Where(x => x.Email == email).ToList();
            }
            if (name != null)
            {
                res = res.Where(x => x.Name==name).ToList();
            }
            if (company != null)
            {
                res = res.Where(x => x.Company.Name == company).ToList();
            }
            if (position != null)
            {
                res = res.Where(x => x.PositionId == position).ToList();
            }

            return res;
        }

        // POST api/users
        [HttpPost]
        public async Task<ActionResult<RegisViewModel>> Post(RegisViewModel user)
        {
            // обработка частных случаев валидации
            if (user.Name == "admin")
            {
                ModelState.AddModelError("Name", "Недопустимое имя пользователя - admin");
            }
            User user1 = await db.User.FirstOrDefaultAsync(x => x.Email == user.Email);
            if (user1 != null)
                ModelState.AddModelError("Email", "Пользователь с таким именем уже существует - admin");
            // если есть лшибки - возвращаем ошибку 400
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            User _user = new User { Email = user.Email, Name = user.Name, Surname = user.Surname, Lastname = user.Lastname, Password = BCrypt.Net.BCrypt.HashPassword(user.Password), BirthDate = user.BirthDate, Phone = user.Phone, RegDate = DateTime.Now, RoleId = 3, CompanyId = 3 };
            // если ошибок нет, сохраняем в базу данных
            db.User.Add(_user);
            await db.SaveChangesAsync();
            return Ok(user);
        }

        [Route("EditUser/{id}")]
        [HttpPut]
        public async Task<ActionResult<User>> EditUser(int id, User user)
        {
            if (id != user.Id)
            {
                return BadRequest();
            }
            // обработка частных случаев валидации
            if (user.Name == "admin")
            {
                ModelState.AddModelError("Name", "Недопустимое имя пользователя - admin");
            }

            if (db.User.FirstOrDefault(u=> u.Email==user.Email && u.Id != user.Id)!=null)
            {
                return BadRequest();
            }
            db.Entry(user).State = EntityState.Modified;

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!UserExists(id))
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
        private bool UserExists(int id)
        {
            return db.Company.Any(e => e.Id == id);
        }
    }
}
