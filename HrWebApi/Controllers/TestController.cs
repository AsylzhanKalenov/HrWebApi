using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace HrWebApi.Controllers
{
    public class TestController : Controller
    {
        [Route("[controller]")]
        public IActionResult Index()
        {
            return View();
        }
    }
}
