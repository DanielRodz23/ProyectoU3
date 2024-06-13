using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.Json;
using ProyectoAPI.Controllers.Repositories;
using ProyectoAPI.Helpers;
using ProyectoAPI.Models.Entities;

namespace ProyectoAPI.Repositories
{
    public class ActividadesRepository : Repository<Actividades>
    {
        public ActividadesRepository(ItesrcneActividadesContext ctx) : base(ctx)
        {
        }
        public async Task<IEnumerable<Actividades>> GetAllActividadesPublicadasAsync(int? id, DateTime fecha)
        {
            List<Actividades> listactividades = new();

            List<Departamentos> hijos=new();

            var userActual = ctx.Departamentos.Include(x=>x.InverseIdSuperiorNavigation).Include(x=>x.Actividades).Where(x => x.Id == id).First();

            foreach (var item in userActual.Actividades)
            {
                if (item.FechaActualizacion>fecha)
                {
                    listactividades.Add(item);
                }
            }
            hijos.AddRange(userActual.InverseIdSuperiorNavigation);
            while (hijos.Count != 0)
            {
                var hijes = hijos.ToList();
                hijos.Clear();
                foreach (var item in hijes)
                {
                    var dep = ctx.Departamentos.Include(x=>x.Actividades).Include(x=>x.InverseIdSuperiorNavigation).First(x=>x.Id == item.Id);
                    foreach (var act in dep.Actividades)
                    {
                        if (act.FechaActualizacion > fecha)
                        {
                            listactividades.Add(act);
                        }
                    }
                    hijos.AddRange(dep.InverseIdSuperiorNavigation);
                }
            }
            //var hijos = ctx.
            return listactividades.Where(x=>x.FechaActualizacion>fecha && x.Estado == (int)Estado.Publicado).OrderByDescending(x=>x.FechaRealizacion);
        }
        public async Task<IEnumerable<Actividades>> GetMyBorradorAsync(int idUser, DateTime fecha)
        {
            //var query = ctx.Departamentos
            //    .Include(x => x.Actividades)
            //    .ThenInclude(x=>x.IdDepartamentoNavigation)
            //    .Where(x => x.Id == idUser)
            //    .Select(x => x.Actividades.Where(e => e.Estado == (int)Estado.Borrador && e.FechaActualizacion>fecha).First());
            var query= ctx.Actividades.Where(x=>x.IdDepartamento==idUser);

            return query;
        }
        public async Task<Actividades> GetIncludeDepa(int id)
        {
            return ctx.Actividades.Include(x=>x.IdDepartamentoNavigation).Where(x => x.Id == id).First();
        }
    }
}