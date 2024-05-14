using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ProyectoAPI.Models.Entities;

namespace ProyectoAPI.Controllers.Repositories
{
    public class Repository<T> where T : class
    {

        public ItesrcneActividadesContext ctx;
        public Repository(ItesrcneActividadesContext ctx)
        {
            this.ctx = ctx;
        }

        public virtual IEnumerable<T> GetAll()
        {
            return ctx.Set<T>();
        }

        public virtual T? Get(object id)
        {
            return ctx.Find<T>(id);
        }

        public virtual void Insert(T entity)
        {
            ctx.Add(entity);
            ctx.SaveChanges();
        }

        public virtual void Update(T entity)
        {
            ctx.Update(entity);
            ctx.SaveChanges();
        }

        public virtual void Delete(T entity)
        {
            ctx.Remove(entity);
            ctx.SaveChanges();
        }

        public virtual void Delete(object id)
        {
            var entity = ctx.Find<T>(id);
            if (entity != null)
            {
                ctx.Remove(entity);
            }
            ctx.SaveChanges();
        }
    }
}