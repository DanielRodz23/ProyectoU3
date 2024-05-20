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
    public class ActividadesController(ActividadesRepository actividadesRepository, DepartamentosRepository departamentosRepository, IMapper mapper) : ControllerBase
    {
        private readonly ActividadesRepository actividadesRepository = actividadesRepository;
        private readonly DepartamentosRepository departamentosRepository = departamentosRepository;
        private readonly IMapper mapper = mapper;

        [HttpGet("{fecha?}")]
        public async Task<IActionResult> GetAllActivities(DateTime? fecha)
        {
            var context = HttpContext;
            var idUsuario = User.Identities.SelectMany(x => x.Claims).FirstOrDefault(x => x.Type == "id");
            if (idUsuario == null) return BadRequest();
            var currentUser = departamentosRepository.Get(int.Parse(idUsuario.Value));
            if (currentUser == null) return NotFound();
            var actividades = await actividadesRepository.GetAllActividadesPublicadasAsync(currentUser.Id, fecha?? new DateTime(2000, 01, 01));
            var actividadesDTO = mapper.Map<IEnumerable<ActividadesDTO>>(actividades);
            return Ok(actividadesDTO);
        }
        [HttpGet("GetActividadOrBorrador/{id}")]
        public async Task<IActionResult> GetActividadOrBorrador(int id)
        {
            var context = HttpContext;
            var idUsuario = User.Identities.SelectMany(x => x.Claims).FirstOrDefault(x => x.Type == "id");
            if (idUsuario == null) return BadRequest();

            var activ = await actividadesRepository.GetIncludeDepa(id);
            if (activ == null) return NotFound();
            
            var actDto = mapper.Map<ActividadesDTO>(activ);
            return Ok(actDto);
        }
        [HttpGet("GetMyBorradores/{fecha?}")]
        public async Task<IActionResult> GetMyBorradores(DateTime? fecha)
        {
            var context = HttpContext;
            var idUsuario = User.Identities.SelectMany(x => x.Claims).FirstOrDefault(x => x.Type == "id");
            if (idUsuario == null) return BadRequest();
            var currentUser = departamentosRepository.Get(int.Parse(idUsuario.Value));
            //Si el usuario no existe
            if (currentUser == null) return NotFound();
            var myBorrador = await actividadesRepository.GetMyBorradorAsync(currentUser.Id, fecha??DateTime.MinValue);
            var borradorMapd = mapper.Map<IEnumerable<ActividadesDTO>>(myBorrador);
            return Ok(borradorMapd);
        }
        [HttpPost]
        public async Task<IActionResult> PostActividad(InsertAct actDTO)
        {
            ActividadesDTO actividadesDTO = new ActividadesDTO()
            {
                Titulo = actDTO.Titulo,
                Descripcion = actDTO.Descripcion,
                FechaRealizacion  = new DateOnly(actDTO.Anio, actDTO.Mes, actDTO.Dia),
                Estado = actDTO.Estado
            };
            var context = HttpContext;
            var claimTkn = User.Identities.SelectMany(x => x.Claims).FirstOrDefault(x => x.Type == "id");
            if (claimTkn == null) return BadRequest();
            var id = int.Parse(claimTkn.Value);
            actividadesDTO.IdDepartamento = id;
            //TODO validar aqui
            var actividades = mapper.Map<Actividades>(actividadesDTO);
            actividadesRepository.Insert(actividades);
            actividadesDTO = mapper.Map<ActividadesDTO>(actividades);
            return Ok(actividadesDTO);
        }
        [HttpPut]
        public async Task<ActionResult> UpdateActividad(ActividadesDTO actividadesDTO)
        {
            //Teoricamente es para agregar una actividad que ya estaba como borrador

            var actividad = actividadesRepository.Get(actividadesDTO.Id);
            if (actividad == null) return BadRequest();

            var context = HttpContext;
            var claimtkn = User.Identities.SelectMany(x => x.Claims).FirstOrDefault(x => x.Type == "id");
            if (claimtkn == null) return BadRequest();

            actividad.Titulo = actividadesDTO.Titulo;
            actividad.Descripcion = actividadesDTO.Descripcion;
            actividad.FechaRealizacion = actividadesDTO.FechaRealizacion;
            actividad.FechaActualizacion = DateTime.UtcNow;
            actividad.Estado = actividadesDTO.Estado;
            if (actividad.Estado == (int)Estado.Publicado) actividad.FechaCreacion = DateTime.UtcNow;
            else { actividad.FechaCreacion = actividadesDTO.FechaCreacion; }

            //TODO validar
            actividad.IdDepartamento = int.Parse(claimtkn.Value);
            actividadesRepository.Update(actividad);
            return Ok();

        }

        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete(int id)
        {
            //Traer usuario
            var context = HttpContext;
            var claimtkn = User.Identities.SelectMany(x => x.Claims).FirstOrDefault(x => x.Type == "id");
            if (claimtkn == null) return BadRequest();
            var usr = departamentosRepository.Get(claimtkn.Value);
            if (usr == null) return BadRequest();


            if (usr.Actividades.Any(x => x.Id == id))
            {
                var activ = actividadesRepository.Get(id);
                activ.Estado = (int)Estado.Eliminado;
                activ.FechaActualizacion = DateTime.UtcNow;
                actividadesRepository.Update(activ);
                return Ok();
            }
            return NotFound();
        }

    }
}