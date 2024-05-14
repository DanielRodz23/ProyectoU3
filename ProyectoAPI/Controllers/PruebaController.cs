using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ProyectoAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class PruebaController : ControllerBase
    {
        [HttpGet]
        public async Task<IActionResult> GetCookies()
        {
            var context = HttpContext;
            var id = User.Identities.SelectMany(x=>x.Claims).FirstOrDefault(x=>x.Type=="id").Value;
            return Ok(id);
        }
    }
}