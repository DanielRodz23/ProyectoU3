using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using ProyectoAPI.Controllers.Repositories;
using ProyectoAPI.Models.Entities;

namespace ProyectoAPI.Repositories
{
    public class DepartamentosRepository : Repository<Departamentos>
    {
        public DepartamentosRepository(ItesrcneActividadesContext ctx) : base(ctx)
        {
        }
        public async Task<Departamentos?> LoginDepartamento(string user, string password)
        {

            return ctx.Departamentos.Where(x=>x.Username == user && x.Password == password).FirstOrDefault();
        }
        public async Task<Departamentos?> GetIncludeActividades(int id)
        {
            return ctx.Departamentos.Include(x => x.Actividades).FirstOrDefault(x => x.Id == id);
        }
    }
}