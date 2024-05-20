using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProyectoU3.Models.DTOs
{
    public class InsertAct
    {
        public string Titulo { get; set; } = null!;
        public string? Descripcion { get; set; }
        public int Estado { get; set; }
        public int Anio { get; set; }
        public int Mes { get; set; }
        public int Dia { get; set; }

    }
}
