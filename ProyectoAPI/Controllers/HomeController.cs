using Microsoft.AspNetCore.Mvc;

namespace ProyectoAPI.Controllers
{
    public class HomeController : Controller
    {
        [HttpGet("/")]
        public IActionResult Index()
        {
            return View();
        }
    }
}
