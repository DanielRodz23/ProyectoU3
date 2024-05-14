using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ProyectoAPI.Models.DTOs;
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
            var actividades = await actividadesRepository.GetAllActividadesPublicadasAsync(currentUser.IdSuperior);
            var actividadesDTO = mapper.Map<IEnumerable<ActividadesDTO>>(actividades);
            return Ok(actividadesDTO);
        }
        [HttpGet("GetMyBorrador")]
        public async Task<IActionResult> GetMyBorrador(){
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
    }
}