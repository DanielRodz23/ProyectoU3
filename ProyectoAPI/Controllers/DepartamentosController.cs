using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ProyectoAPI.Helpers;
using ProyectoAPI.Models.DTOs;
using ProyectoAPI.Models.Entities;
using ProyectoAPI.Repositories;

namespace ProyectoAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class DepartamentosController(DepartamentosRepository departamentosRepository, ActividadesRepository actividadesRepository, IMapper mapper) : ControllerBase
    {
        private readonly DepartamentosRepository departamentosRepository = departamentosRepository;
        private readonly ActividadesRepository actividadesRepository = actividadesRepository;
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
            departamentoDTO.Password = Encriptacion.StringToSHA512(departamentoDTO.Password);
            var newDep = mapper.Map<Departamentos> (departamentoDTO);
            departamentosRepository.Insert(newDep);
            return Ok();
        }
        [HttpGet]
        public async Task<IActionResult> GetDepartments()
        {
            var context = HttpContext;
            var claimid = User.Identities.SelectMany(x => x.Claims).FirstOrDefault(x => x.Type == "id");
            if (claimid == null) return BadRequest();
            var CurrentUser = departamentosRepository.Get(int.Parse(claimid.Value));
            if (CurrentUser == null) return BadRequest();
            if (CurrentUser.IdSuperior != null) return Forbid();

            var data = departamentosRepository.GetDepartamentos();
            var dataMaped = mapper.Map<IEnumerable<DepartamentosDTO>>(data);
            return Ok(dataMaped);
        }
        [HttpGet("Delete/{id:int}")]
        public async Task<IActionResult> DeleteDepartments(int id)
        {
            var context = HttpContext;
            var claimid = User.Identities.SelectMany(x => x.Claims).FirstOrDefault(x => x.Type == "id");
            if (claimid == null) return BadRequest();
            var CurrentUser = departamentosRepository.Get(int.Parse(claimid.Value));
            if (CurrentUser == null) return BadRequest();
            if (CurrentUser.IdSuperior != null) return Forbid();



            var usuario = await departamentosRepository.GetIncludeActividades(id);
            if (usuario == null) return BadRequest();
            if (usuario.IdSuperior == null) return Forbid();

            foreach (var item in usuario.Actividades.ToList())
            {
                actividadesRepository.Delete(item);
            }
            departamentosRepository.Delete(id);
            return Ok();
        }
    }
}