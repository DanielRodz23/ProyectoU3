using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
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
        private readonly TokenValidationParameters tknValidationParameters;

        public LoginController(JwtTokenGenerator tokenGenerator, DepartamentosRepository departamentosRepository, TokenValidationParameters tknValidationParameters)
        {
            this.tokenGenerator = tokenGenerator;
            this.departamentosRepository = departamentosRepository;
            this.tknValidationParameters = tknValidationParameters;
        }
        [HttpPost]
        public async Task<IActionResult> LoginPost(LoginModel model)
        {
            model.password = Encriptacion.StringToSHA512(model.password);
            var depa = await departamentosRepository.LoginDepartamento(model.username, model.password);
            if (depa == null) return NotFound();
            var tkn = tokenGenerator.GetToken(depa);
            return Ok(tkn);
        }
        [HttpGet("/{token}")]
        public async Task<IActionResult> Validar(string token)
        {
            JwtSecurityTokenHandler TokenHandler = new ();
            try
            {
                SecurityToken securityToken;
                var valid = TokenHandler.ValidateToken(token, tknValidationParameters, out securityToken);
                return Ok();
            }
            catch
            {
                return NotFound();
            }
        }
    }
}