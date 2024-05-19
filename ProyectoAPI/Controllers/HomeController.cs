using Microsoft.AspNetCore.Mvc;
using ProyectoAPI.Models.LoginModels;

namespace ProyectoAPI.Controllers
{
    public class HomeController : Controller
    {
        private readonly HoraModel horaModel;

        public HomeController(HoraModel horaModel)
        {
            this.horaModel = horaModel;
        }
        [HttpGet("/")]
        public IActionResult Index()
        {
            return View(horaModel);
        }
    }
}
