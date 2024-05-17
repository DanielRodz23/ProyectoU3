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
    public class ActividadesController(ActividadesRepository actividadesRepository, DepartamentosRepository departamentosRepository, IMapper mapper) : ControllerBase
    {
        private readonly ActividadesRepository actividadesRepository = actividadesRepository;
        private readonly DepartamentosRepository departamentosRepository = departamentosRepository;
        private readonly IMapper mapper = mapper;

        [HttpGet]
        public async Task<IActionResult> GetAllActivities()
        {
            var context = HttpContext;
            var idUsuario = User.Identities.SelectMany(x => x.Claims).FirstOrDefault(x => x.Type == "id");
            if (idUsuario == null) return BadRequest();
            var currentUser = departamentosRepository.Get(int.Parse(idUsuario.Value));
            if (currentUser == null) return NotFound();
            var actividades = await actividadesRepository.GetAllActividadesPublicadasAsync(currentUser.Id);
            var actividadesDTO = mapper.Map<IEnumerable<ActividadesDTO>>(actividades);
            return Ok(actividadesDTO);
        }
        [HttpGet("GetActividadOrBorrador/{id}")]
        public async Task<IActionResult> GetActividadOrBorrador(int id)
        {
            var context = HttpContext;
            var idUsuario = User.Identities.SelectMany(x => x.Claims).FirstOrDefault(x => x.Type == "id");
            if (idUsuario == null) return BadRequest();

            var activ = actividadesRepository.Get(id);
            if (activ == null) return NotFound();
            if (activ.IdDepartamento!=int.Parse(idUsuario.Value))
            {
                return Forbid();
            }
            var actDto = mapper.Map<ActividadesDTO>(activ);
            return Ok(actDto);
        }
        [HttpGet("GetMyBorradores")]
        public async Task<IActionResult> GetMyBorradores(){
            var context = HttpContext;
            var idUsuario = User.Identities.SelectMany(x => x.Claims).FirstOrDefault(x => x.Type == "id");
            if (idUsuario == null) return BadRequest();
            var currentUser = departamentosRepository.Get(int.Parse(idUsuario.Value));
            //Si el usuario no existe
            if (currentUser==null) return NotFound();
            var myBorrador = await actividadesRepository.GetMyBorradorAsync(currentUser.Id);
            var borradorMapd = mapper.Map<IEnumerable<ActividadesDTO>>(myBorrador);
            return Ok(borradorMapd);
        }
        [HttpPost]
        public async Task<IActionResult> PostActividad(ActividadesDTO actividadesDTO)
        {
            var context = HttpContext;
            var claimTkn = User.Identities.SelectMany(x=>x.Claims).FirstOrDefault(x => x.Type=="id");
            if (claimTkn==null) return BadRequest();
            var id = int.Parse(claimTkn.Value);
            actividadesDTO.Id = 0;
            actividadesDTO.IdDepartamento = id;
            actividadesDTO.FechaCreacion = DateTime.UtcNow;
            //TODO validar aqui
            var actividades = mapper.Map<Actividades>(actividadesDTO);
            actividadesRepository.Insert(actividades);
            return Ok();
        }
        [HttpPut]
        public async Task<ActionResult> UpdateActividad(ActividadesDTO actividadesDTO)
        {
            //Teoricamente es para agregar una actividad que ya estaba como borrador
            
            var actividad = actividadesRepository.Get(actividadesDTO.Id);
            if (actividad==null) return BadRequest();
            
            var context = HttpContext;
            var claimtkn = User.Identities.SelectMany(x=>x.Claims).FirstOrDefault(x=>x.Type=="id");
            if (claimtkn==null) return BadRequest();

            actividad.Titulo = actividadesDTO.Titulo;
            actividad.Descripcion = actividadesDTO.Descripcion;
            actividad.FechaRealizacion = actividadesDTO.FechaRealizacion;
            actividad.Estado = actividadesDTO.Estado;
            if (actividad.Estado==2) actividad.FechaCreacion = DateTime.UtcNow;
            else {actividad.FechaCreacion = actividadesDTO.FechaCreacion; }

            //TODO validar
            actividad.IdDepartamento = int.Parse(claimtkn.Value);
            actividadesRepository.Update(actividad);
            return Ok();
            
        }

    }
}