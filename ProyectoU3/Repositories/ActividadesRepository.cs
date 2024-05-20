using ProyectoU3.Models.Entities;
using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProyectoU3.Repositories
{
    public class ActividadesRepository
    {
        SQLiteConnection context;
        public ActividadesRepository()
        {
            string ruta = FileSystem.AppDataDirectory + "/actividades.db3";
            context = new SQLiteConnection(ruta);
            context.CreateTable<Actividades>();
        }

        public void Insert(Actividades L)
        {
            context.Insert(L);
        }

        public IEnumerable<Actividades> GetAll()
        {
            return context.Table<Actividades>()
               .OrderBy(x => x.fechaCreacion);
        }

        public Actividades? Get(int id)
        {
            return context.Find<Actividades>(id);
        }

        public void InsertOrReplace(Actividades L)
        {
            context.InsertOrReplace(L);
        }

        public void Update(Actividades L)
        {
            context.Update(L);
        }

        public void Delete(Actividades L)
        {
            context.Delete(L);
        }
        public void DeleteAll()
        {
            context.DeleteAll<Actividades>();
        }
    }
}
