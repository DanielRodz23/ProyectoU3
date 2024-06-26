using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
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
            if (CurrentUser.IdSuperior != null) return Forbid();
            #endregion
            //TODO validar
            departamentoDTO.Password = Encriptacion.StringToSHA512(departamentoDTO.Password);
            var newDep = mapper.Map<Departamentos>(departamentoDTO);
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
        [HttpGet("nombre")]
        public async Task<IActionResult> GetDepa()
        {
            var context = HttpContext;
            var claimid = User.Identities.SelectMany(x => x.Claims).FirstOrDefault(x => x.Type == "id");
            if (claimid == null) return BadRequest();
            var user = departamentosRepository.Get(int.Parse(claimid.Value));
            if (user == null) return BadRequest();
            return Ok(user.Nombre);
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
            var actividades = usuario.Actividades.ToList();

            foreach (var item in actividades)
            {
                item.Estado = (int)Estado.Eliminado;
                actividadesRepository.Update(item);
            }

            var scopeFactory = HttpContext.RequestServices.GetRequiredService<IServiceScopeFactory>();

            _ = Task.Run(async () =>
            {
                // Crear un nuevo scope para la tarea en segundo plano
                using (var scope = scopeFactory.CreateScope())
                {
                    var scopedDepartamentosRepository = scope.ServiceProvider.GetRequiredService<DepartamentosRepository>();

                    try
                    {
                        await Task.Delay(7000); // Espera de 11 segundos

                        scopedDepartamentosRepository.DeleteDepartment(id);
                    }
                    catch (Exception ex)
                    {
                        // Maneja la excepci�n, registra o env�a notificaci�n
                        Console.WriteLine($"Error during background operation: {ex.Message}");
                    }
                }
            });

            //_ = Task.Run(async () =>
            //{
            //    await Task.Delay(11000);
            //    try
            //    {

            //        var newid = id;
            //        var usuario = await departamentosRepository.GetIncludeActividades(newid);
            //        var actividades = usuario?.Actividades.ToList() ?? new List<Actividades>();
            //        foreach (var item in actividades)
            //        {
            //            actividadesRepository.Delete(item);
            //        }
            //        foreach (var item in usuario.InverseIdSuperiorNavigation.ToList())
            //        {
            //            item.IdSuperior = usuario.IdSuperior;
            //            departamentosRepository.Update(item);
            //        }
            //        departamentosRepository.Delete(newid);
            //    }
            //    catch (Exception ex)
            //    {
            //    }
            //});
            return Ok();
        }
        async Task DeleteDepa(int id)
        {
            Thread.Sleep(11000);

            var usuario = await departamentosRepository.GetIncludeActividades(id);
            var actividades = usuario?.Actividades.ToList() ?? new List<Actividades>();
            foreach (var item in actividades)
            {
                actividadesRepository.Delete(item);
            }
            foreach (var item in usuario.InverseIdSuperiorNavigation.ToList())
            {
                item.IdSuperior = usuario.IdSuperior;
                departamentosRepository.Update(item);
            }
            departamentosRepository.Delete(id);
        }
    }
}