using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BlogV6.Attributes;
using Microsoft.AspNetCore.Mvc;

namespace BlogV6.Controllers
{
    [ApiController]
    [Route("")]
    public class HomeController : ControllerBase
    {
        // Método health-check
        [HttpGet]
        //[ApiKey]
        public IActionResult HealthCheck()
        {
            return Ok();
        }
    }
}