using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using ProyectoAPI.Helpers;
using ProyectoAPI.Models.LoginModels;
using ProyectoAPI.Repositories;

namespace ProyectoAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class LoginController : ControllerBase
    {
        private readonly JwtTokenGenerator tokenGenerator;
        private readonly DepartamentosRepository departamentosRepository;

        public LoginController(JwtTokenGenerator tokenGenerator, DepartamentosRepository departamentosRepository)
        {
            this.tokenGenerator = tokenGenerator;
            this.departamentosRepository = departamentosRepository;
        }
        [HttpPost]
        public async Task<IActionResult> LoginPost(LoginModel model)
        {
            var depa = await departamentosRepository.LoginDepartamento(model.username, model.password);
            if (depa == null) return NotFound();
            var tkn = tokenGenerator.GetToken(depa);
            return Ok(tkn);
        }
    }
}