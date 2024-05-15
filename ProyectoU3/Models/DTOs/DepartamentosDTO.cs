using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProyectoU3.Models.DTOs
{
    public class DepartamentosDTO
    {
        public int Id { get; set; }

        public string Nombre { get; set; } = null!;

        public string Username { get; set; } = null!;

        public string Password { get; set; } = null!;

        public int? IdSuperior { get; set; }
    }
}
