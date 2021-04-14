using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using HrWebApi.Models;
using HrWebApi.ViewModels;
using System.IO;
using Microsoft.AspNetCore.Hosting;
using System.Net.Http;
using System.Net;

namespace HrWebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserDocsController : ControllerBase
    {
        private readonly HrDepContext _context;
        IWebHostEnvironment _appEnvironment;
        public UserDocsController(HrDepContext context, IWebHostEnvironment appEnvironment)
        {
            _context = context;
            _appEnvironment = appEnvironment;
            if (!_context.FileCat.Any()) {
                _context.FileCat.Add(new FileCat { Name = "Undefined file" });
                _context.FileCat.Add(new FileCat { Name = "Specialization" });
                _context.FileCat.Add(new FileCat { Name = "Udv" });
                _context.FileCat.Add(new FileCat { Name = "Pension" });
                _context.FileCat.Add(new FileCat { Name = "Certificates" });
                _context.FileCat.Add(new FileCat { Name = "Employhis" });
                _context.FileCat.Add(new FileCat { Name = "Addres" });
                _context.FileCat.Add(new FileCat { Name = "Conviction" });
                _context.FileCat.Add(new FileCat { Name = "Narcodisp" });
                _context.FileCat.Add(new FileCat { Name = "Psychodisp" });
                _context.FileCat.Add(new FileCat { Name = "Military" });
                _context.FileCat.Add(new FileCat { Name = "Docphoto" });
                _context.FileCat.Add(new FileCat { Name = "Refmainjob" });
                _context.FileCat.Add(new FileCat { Name = "Marriage" });
                _context.FileCat.Add(new FileCat { Name = "Cash" });
                _context.FileCat.Add(new FileCat { Name = "Forma086" });
                _context.SaveChanges();
            }
        }

        // GET: api/UserDocs
        [HttpGet]
        public async Task<ActionResult<IEnumerable<UserDoc>>> GetUserDoc()
        {
            return await _context.UserDoc.ToListAsync();
        }

        // GET: api/UserDocs/5
        [HttpGet("{id}")]
        public async Task<ActionResult<UserDoc>> GetUserDoc(int id)
        {
            var userDoc = await _context.UserDoc.FindAsync(id);

            if (userDoc == null)
            {
                return NotFound();
            }

            return userDoc;
        }

        [Route("GetUserAllDocs/{userid}")]
        [HttpGet]
        public async Task<ActionResult<IEnumerable<UserDoc>>> GetUserAllDocs(int userid)
        {
            return await _context.UserDoc.Where(u => u.UserId == userid).ToListAsync();
        }

        // PUT: api/UserDocs/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPut("{id}")]
        public async Task<IActionResult> PutUserDoc(int id, UserDoc userDoc)
        {
            if (id != userDoc.Id)
            {
                return BadRequest();
            }

            _context.Entry(userDoc).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!UserDocExists(id))
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

        // POST: api/UserDocs
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPost]
        public async Task<ActionResult> PostUserDoc()
        {
            string datefile = "12" + DateTime.Now.ToString("HHmmss");
            if (Request.HasFormContentType)
            {
                var form = Request.Form;
                int userid = 1;
                int filecatid = int.Parse(form["filecat"]);
                string useremail = form["useremail"];
                int count = 0;
                
                if (!String.IsNullOrEmpty(useremail)) {
                    User us = await _context.User.FirstOrDefaultAsync(x => x.Email == useremail);
                    userid = us.Id;
                }
                foreach (var formFile in form.Files)
                {
                    string[] cat = formFile.FileName.Split('_');

                    _context.UserDoc.Add(new UserDoc { SendedDate = DateTime.Now, FileName = await SaveFile(formFile, datefile, formFile.FileName, count), FileExtension = Path.GetExtension(formFile.FileName), Countturn = count, UserId = userid, FileCatsId = filecatid });
                    count++;
                }

                await _context.SaveChangesAsync();
            }

            //_context.UserDoc.Add(new UserDoc { SendedDate = DateTime.Now, FileName = await SaveFile(userDoc, datefile, "specialization"), FileExtension = Path.GetExtension(userDoc.FileName), UserId = 1, FileCatsId = 1});
            //_context.UserDoc.Add(new UserDoc { SendedDate = DateTime.Now, FileName = await SaveFile(userDoc.Udv, datefile, "udv"), FileExtension = Path.GetExtension(userDoc.Udv.FileName), UserId = 1, FileCatsId = 2 });
                
            return CreatedAtAction("GetUserDoc", new { id = 1 });
        }

        [Route("GetUserDocByUserId")]
        [HttpGet]
        public async Task<ActionResult<FileViewModel>> GetUserDocByUserId(int userid, int filecatid)
        {
            var userDoc = await _context.UserDoc.Where(u=> u.UserId == userid).ToListAsync();
            FileViewModel fff = new FileViewModel();
            fff.UserId = userid;
            fff.FileCatsId = filecatid;

            if (userDoc == null)
            {
                return NotFound();
            }
            fff.Specialization = new List<string>();
            foreach (UserDoc fi in userDoc.Where(u => u.FileCatsId == 2))
            {
                if (fi.FileName != null)
                {
                    Byte[] b = System.IO.File.ReadAllBytes(_appEnvironment.WebRootPath + "/FilesDocs/" + fi.FileName);
                    fff.Specialization.Add(fi.FileName);
                    fff.FileByte = b;
                }
            }
            fff.Udv = new List<string>();
            foreach (UserDoc fi in userDoc.Where(u => u.FileCatsId == 3))
            {
                if (fi.FileName != null)
                {
                    Byte[] b = System.IO.File.ReadAllBytes(_appEnvironment.WebRootPath + "/FilesDocs/" + fi.FileName);
                    fff.Udv.Add(fi.FileName);
                    fff.FileByte = b;
                }
            }
            fff.Pension = new List<string>();
            foreach (UserDoc fi in userDoc.Where(u => u.FileCatsId == 4))
            {
                if (fi.FileName != null)
                {
                    Byte[] b = System.IO.File.ReadAllBytes(_appEnvironment.WebRootPath + "/FilesDocs/" + fi.FileName);
                    fff.Pension.Add(fi.FileName);
                    fff.FileByte = b;
                }
            }
            fff.Certificates = new List<string>();
            foreach (UserDoc fi in userDoc.Where(u => u.FileCatsId == 5))
            {
                if (fi.FileName != null)
                {
                    Byte[] b = System.IO.File.ReadAllBytes(_appEnvironment.WebRootPath + "/FilesDocs/" + fi.FileName);
                    fff.Certificates.Add(fi.FileName);
                    fff.FileByte = b;
                }
            }
            fff.Employhis = new List<string>();
            foreach (UserDoc fi in userDoc.Where(u => u.FileCatsId == 6))
            {
                if (fi.FileName != null)
                {
                    Byte[] b = System.IO.File.ReadAllBytes(_appEnvironment.WebRootPath + "/FilesDocs/" + fi.FileName);
                    fff.Employhis.Add(fi.FileName);
                    fff.FileByte = b;
                }
            }
            fff.Addres = new List<string>();
            foreach (UserDoc fi in userDoc.Where(u => u.FileCatsId == 7))
            {
                if (fi.FileName != null)
                {
                    Byte[] b = System.IO.File.ReadAllBytes(_appEnvironment.WebRootPath + "/FilesDocs/" + fi.FileName);
                    fff.Addres.Add(fi.FileName);
                    fff.FileByte = b;
                }
            }
            fff.Conviction = new List<string>();
            foreach (UserDoc fi in userDoc.Where(u => u.FileCatsId == 8))
            {
                if (fi.FileName != null)
                {
                    Byte[] b = System.IO.File.ReadAllBytes(_appEnvironment.WebRootPath + "/FilesDocs/" + fi.FileName);
                    fff.Conviction.Add(fi.FileName);
                    fff.FileByte = b;
                }
            }
            fff.Narcodisp = new List<string>();
            foreach (UserDoc fi in userDoc.Where(u => u.FileCatsId == 9))
            {
                if (fi.FileName != null)
                {
                    Byte[] b = System.IO.File.ReadAllBytes(_appEnvironment.WebRootPath + "/FilesDocs/" + fi.FileName);
                    fff.Narcodisp.Add(fi.FileName);
                    fff.FileByte = b;
                }
            }
            fff.Psychodisp = new List<string>();
            foreach (UserDoc fi in userDoc.Where(u => u.FileCatsId == 10))
            {
                if (fi.FileName != null)
                {
                    Byte[] b = System.IO.File.ReadAllBytes(_appEnvironment.WebRootPath + "/FilesDocs/" + fi.FileName);
                    fff.Psychodisp.Add(fi.FileName);
                    fff.FileByte = b;
                }
            }
            fff.Military = new List<string>();
            foreach (UserDoc fi in userDoc.Where(u => u.FileCatsId == 11))
            {
                if (fi.FileName != null)
                {
                    Byte[] b = System.IO.File.ReadAllBytes(_appEnvironment.WebRootPath + "/FilesDocs/" + fi.FileName);
                    fff.Military.Add(fi.FileName);
                    fff.FileByte = b;
                }
            }
            fff.Docphoto = new List<string>();
            foreach (UserDoc fi in userDoc.Where(u => u.FileCatsId == 12))
            {
                if (fi.FileName != null)
                {
                    Byte[] b = System.IO.File.ReadAllBytes(_appEnvironment.WebRootPath + "/FilesDocs/" + fi.FileName);
                    fff.Docphoto.Add(fi.FileName);
                    fff.FileByte = b;
                }
            }
            fff.Refmainjob = new List<string>();
            foreach (UserDoc fi in userDoc.Where(u => u.FileCatsId == 13))
            {
                if (fi.FileName != null)
                {
                    Byte[] b = System.IO.File.ReadAllBytes(_appEnvironment.WebRootPath + "/FilesDocs/" + fi.FileName);
                    fff.Refmainjob.Add(fi.FileName);
                    fff.FileByte = b;
                }
            }
            fff.Marriage = new List<string>();
            foreach (UserDoc fi in userDoc.Where(u => u.FileCatsId == 14))
            {
                if (fi.FileName != null)
                {
                    Byte[] b = System.IO.File.ReadAllBytes(_appEnvironment.WebRootPath + "/FilesDocs/" + fi.FileName);
                    fff.Marriage.Add(fi.FileName);
                    fff.FileByte = b;
                }
            }
            fff.Cash = new List<string>();
            foreach (UserDoc fi in userDoc.Where(u => u.FileCatsId == 15))
            {
                if (fi.FileName != null)
                {
                    Byte[] b = System.IO.File.ReadAllBytes(_appEnvironment.WebRootPath + "/FilesDocs/" + fi.FileName);
                    fff.Cash.Add(fi.FileName);
                    fff.FileByte = b;
                }
            }
            fff.Forma086 = new List<string>();
            foreach (UserDoc fi in userDoc.Where(u => u.FileCatsId == 16))
            {
                if (fi.FileName != null)
                {
                    Byte[] b = System.IO.File.ReadAllBytes(_appEnvironment.WebRootPath + "/FilesDocs/" + fi.FileName);
                    fff.Forma086.Add(fi.FileName);
                    fff.FileByte = b;
                }
            }
            return new ObjectResult(fff);
        }

        [Route("GetImage/{image}")]
        [HttpGet]
        public IActionResult GetImage(string image)
        {
            Byte[] b = System.IO.File.ReadAllBytes(_appEnvironment.WebRootPath + "/FilesDocs/"+ image);   // You can use your own method over here.         
            
            return File(b, "image/png");
        }

        [Route("PostAllUserDoc")]
        [HttpPost]
        public async Task<ActionResult> PostAllUserDoc()
        {
            string datefile = "12" + DateTime.Now.ToString("HHmmss");
            if (Request.HasFormContentType)
            {
                var form = Request.Form;
                int userid = 1;
                int filecatid = 3;
                string useremail = form["useremail"];
                if (!String.IsNullOrEmpty(useremail))
                {

                    User us = await _context.User.FirstOrDefaultAsync(x => x.Email == useremail);
                    userid = us.Id;
                }
                foreach (var formFile in form.Files)
                {
                    string[] cat = formFile.FileName.Split('_');
                    switch (cat[0])
                    {
                        case "Specialization":
                            filecatid = 2;
                            break;
                        case "Udv":
                            filecatid = 3;
                            break;
                        case "Pension":
                            filecatid = 4;
                            break;
                        case "Certificates":
                            filecatid = 5;
                            break;
                        case "Employhis":
                            filecatid = 6;
                            break;
                        case "Addres":
                            filecatid = 7;
                            break;
                        case "Сonviction":
                            filecatid = 8;
                            break;
                        case "Narcodisp":
                            filecatid = 9;
                            break;
                        case "Psychodisp":
                            filecatid = 10;
                            break;
                        case "Military":
                            filecatid = 11;
                            break;
                        case "Docphoto":
                            filecatid = 12;
                            break;
                        case "Refmainjob":
                            filecatid = 13;
                            break;
                        case "Marriage":
                            filecatid = 14;
                            break;
                        case "Cash":
                            filecatid = 15;
                            break;
                        case "Forma086":
                            filecatid = 16;
                            break;
                        default:
                            filecatid = 1;
                            break;

                    }


                    _context.UserDoc.Add(new UserDoc { SendedDate = DateTime.Now, FileName = await SaveFile(formFile, datefile, formFile.FileName, 1), FileExtension = formFile.FileName, UserId = userid, FileCatsId = filecatid });
                }

                await _context.SaveChangesAsync();
            }

            //_context.UserDoc.Add(new UserDoc { SendedDate = DateTime.Now, FileName = await SaveFile(userDoc, datefile, "specialization"), FileExtension = Path.GetExtension(userDoc.FileName), UserId = 1, FileCatsId = 1});
            //_context.UserDoc.Add(new UserDoc { SendedDate = DateTime.Now, FileName = await SaveFile(userDoc.Udv, datefile, "udv"), FileExtension = Path.GetExtension(userDoc.Udv.FileName), UserId = 1, FileCatsId = 2 });

            return CreatedAtAction("GetUserDoc", new { id = 1 });
        }

        // DELETE: api/UserDocs/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<UserDoc>> DeleteUserDoc(int id)
        {
            var userDoc = await _context.UserDoc.FindAsync(id);
            if (userDoc == null)
            {
                return NotFound();
            }

            _context.UserDoc.Remove(userDoc);
            await _context.SaveChangesAsync();

            return userDoc;
        }

        [Route("EditUserDoc/{userid}")]
        [HttpPut]
        public async Task<IActionResult> EditUserDoc(int userid, UserDoc userDoc)
        {
            if (userid != userDoc.Id)
            {
                return BadRequest();
            }

            _context.Entry(userDoc).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!UserDocExists(userid))
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


        private bool UserDocExists(int id)
        {
            return _context.UserDoc.Any(e => e.Id == id);
        }

        public async Task<string> SaveFile(IFormFile file, string namedate, string filetype, int count)
        {

            string res = namedate+"_"+ count .ToString()+ "_"+filetype;
            string path = "/FilesDocs/" + res;
            using (var fileStream = new FileStream(_appEnvironment.WebRootPath + path, FileMode.Create))
            {
                await file.CopyToAsync(fileStream);
            }

            return res;
        }
    }
}
