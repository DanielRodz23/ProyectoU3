using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProyectoU3.Helpers
{
    public static class ApiUriHelper
    {
        //TODO Obtener los urls neccesarios para que funcione

        //GET:  Para traer todas las actividades que pueda ver el usuario
        public static string ListaActividadesUri { get { return "api/Actividades"; } }
        //POST: Para agregar una lista 
        public static string InsertActividadUri { get { return "api/Actividades"; } }
        //PUT:  Para actualizar una actividad
        public static string UpdateActividadUri { get { return "api/Actividades"; } }

        //GET:  Para obtener una lista de actividades en borrador
        public static string GetAllBorradoresUri { get { return "api/Actividades/GetMyBorradores"; } }
        //GET:  Para traer una actividad en especifico ya sea borrador o actividad
        public static string ActividadConcatenarUri { get { return "api/Actividades/GetActividadOrBorrador/"; } }

        //POST: Para agregar un departamento
        public static string InsertDepartamentoUri { get { return "api/Departamentos"; } }
    }
}
