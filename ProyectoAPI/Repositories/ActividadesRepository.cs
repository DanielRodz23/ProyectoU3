using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using ProyectoAPI.Controllers.Repositories;
using ProyectoAPI.Models.Entities;

namespace ProyectoAPI.Repositories
{
    public class ActividadesRepository : Repository<Actividades>
    {
        public ActividadesRepository(ItesrcneActividadesContext ctx) : base(ctx)
        {
        }
        public async Task<IEnumerable<Actividades>> GetAllActividadesPublicadasAsync(int? idsuperior){
            if (idsuperior == null)
                return ctx.Actividades.Where(x=>x.Estado==2).OrderByDescending(x=>x.FechaCreacion);
            return ctx.Actividades.Include(x=>x.IdDepartamentoNavigation).Where(x => x.IdDepartamentoNavigation.IdSuperior>=idsuperior && x.Estado==2).OrderByDescending(x=>x.FechaCreacion);
        }
        public async Task<IEnumerable<Actividades>> GetMyBorradorAsync(int idUser){
            var query = ctx.Departamentos.Include(x=>x.Actividades).Where(x=>x.Id==idUser).Select(x=>x.Actividades.Where(e=>e.Estado==1));
            return (IEnumerable<Actividades>)query;
        }
    }
}