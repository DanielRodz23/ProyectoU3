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

            return ctx.Departamentos.Where(x => x.Username == user && x.Password == password).FirstOrDefault();
        }
        public async Task<Departamentos?> GetIncludeActividades(int id)
        {
            return ctx.Departamentos.Include(x => x.Actividades).Include(x => x.InverseIdSuperiorNavigation).FirstOrDefault(x => x.Id == id);
        }
        public IEnumerable<Departamentos> GetDepartamentos()
        {
            var data = ctx.Departamentos.Where(x => x.Username.Contains("@realmail.com")).ToList();
            data.ForEach(x => x.Password = "");
            return data;
            //return ctx.Departamentos.ForEachAsync(x => x.Password = "");
        }
        public void DeleteDepartment(int id)
        {
            var depa = ctx.Departamentos.Include(x => x.Actividades).Include(x => x.InverseIdSuperiorNavigation).FirstOrDefault(x => x.Id == id);
            if (depa != null)
            {
                foreach (var item in depa.Actividades.ToList())
                {
                    var entity = ctx.Actividades.Find(item.Id);
                    if (entity != null)
                    {
                        ctx.Remove(entity);
                        ctx.SaveChanges();
                    }
                }
                foreach (var item in depa.InverseIdSuperiorNavigation.ToList())
                {
                    var entity = ctx.Departamentos.Find(item.Id);
                    if (entity != null)
                    {
                        entity.IdSuperior = depa.IdSuperior;
                        ctx.Update(entity);
                        ctx.SaveChanges();
                    }
                }
                var departamento = ctx.Departamentos.Find(id);
                if (departamento != null)
                {
                    ctx.Remove(departamento);
                }
                ctx.SaveChanges();

            }
        }
    }
}