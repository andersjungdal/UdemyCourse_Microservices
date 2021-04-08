using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Actio.Api.Controllers
{
    [Route("api/home")]
    public class HomeController : Controller
    {
        [HttpGet]
        public IActionResult Get() => Content("Hello from Actio API!");
    }
}
