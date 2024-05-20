using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProyectoU3.Models.Entities
{
    [Table("Actividades")]
    public class Actividades
    {
        [PrimaryKey]
        public int id { get; set; }
        [NotNull]
        public string titulo { get; set; } = null!;
        [NotNull]
        public string? descripcion { get; set; }

        public DateTime? fechaRealizacion { get; set; }

        public int idDepartamento { get; set; }

        public DateTime fechaCreacion { get; set; }

        public DateTime fechaActualizacion { get; set; }
        [NotNull]
        public int estado { get; set; }
        [NotNull]
        public string departamento { get; set; } = null!;

        public string Url { get { return "https://doubledapi.labsystec.net/Images/" + id.ToString() + ".png"; } }
    }
}
