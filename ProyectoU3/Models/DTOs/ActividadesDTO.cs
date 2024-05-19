using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProyectoU3.Models.DTOs
{
    public class ActividadesDTO
    {
        public int id { get; set; }

        public string titulo { get; set; } = null!;

        public string? descripcion { get; set; }

        public DateOnly? fechaRealizacion { get; set; }

        public int idDepartamento { get; set; }

        public DateTime fechaCreacion { get; set; }

        public DateTime fechaActualizacion { get; set; }

        public int estado { get; set; }

        public string departamento { get; set; } = null!;

        public string Url { get { return "https://doubledapi.labsystec.net/Images/" + id.ToString() + ".png"; } }

    }
}
