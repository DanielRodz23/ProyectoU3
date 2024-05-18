using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ProyectoAPI.Models.DTOs;
using ProyectoAPI.Models.Entities;
using ProyectoAPI.Repositories;

namespace ProyectoAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class DepartamentosController(DepartamentosRepository departamentosRepository, IMapper mapper) : ControllerBase
    {
        private readonly DepartamentosRepository departamentosRepository = departamentosRepository;
        private readonly IMapper mapper = mapper;

        [HttpPost]
        public async Task<IActionResult> PostDepartamento(DepartamentosDTO departamentoDTO)
        {
            //Validar que este usuario sea el Admin
            #region ValidarAdmin
            var context = HttpContext;
            var claimid = User.Identities.SelectMany(x => x.Claims).FirstOrDefault(x => x.Type == "id");
            if (claimid == null) return BadRequest();
            var CurrentUser = departamentosRepository.Get(int.Parse(claimid.Value));
            if (CurrentUser == null) return BadRequest();
            if (CurrentUser.IdSuperior!=null) return Forbid();
            #endregion
            
            //TODO validar
            var newDep = mapper.Map<Departamentos> (departamentoDTO);
            departamentosRepository.Insert(newDep);
            return Ok();
        }
    }
}