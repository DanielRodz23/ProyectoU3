using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.Json;
using ProyectoAPI.Controllers.Repositories;
using ProyectoAPI.Models.Entities;

namespace ProyectoAPI.Repositories
{
    public class ActividadesRepository : Repository<Actividades>
    {
        public ActividadesRepository(ItesrcneActividadesContext ctx) : base(ctx)
        {
        }
        public async Task<IEnumerable<Actividades>> GetAllActividadesPublicadasAsync(int? id)
        {
            List<Actividades> listactividades = new();

            List<Departamentos> hijos=new();

            var userActual = ctx.Departamentos.Include(x=>x.InverseIdSuperiorNavigation).Where(x => x.Id == id).First();
            var idSuperior = userActual.IdSuperior;
            if (idSuperior != null)
            {
                var ActividadesBros = ctx.Departamentos.Include(x=>x.InverseIdSuperiorNavigation).ThenInclude(x=>x.Actividades).Where(x=>x.Id==idSuperior).Select(x=>x.InverseIdSuperiorNavigation).First().Select(x=>x.Actividades).SelectMany(c=>c);
                listactividades.AddRange(ActividadesBros);

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
                        listactividades.Add(act);
                    }
                    hijos.AddRange(dep.InverseIdSuperiorNavigation);
                }
            }
            //var hijos = ctx.
            return listactividades.OrderByDescending(x=>x.FechaRealizacion);
        }
        public async Task<IEnumerable<Actividades>> GetMyBorradorAsync(int idUser)
        {
            var query = ctx.Departamentos
                .Include(x => x.Actividades)
                .Where(x => x.Id == idUser)
                .Select(x => x.Actividades.Where(e => e.Estado == 1).First());
            return query;
        }
    }
}