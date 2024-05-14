using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProyectoAPI.Models.DTOs
{
    public class ActividadesDTO
    {
        public int Id { get; set; }

        public string Titulo { get; set; } = null!;

        public string? Descripcion { get; set; }

        public DateOnly? FechaRealizacion { get; set; }

        public int IdDepartamento { get; set; }

        public DateTime FechaCreacion { get; set; }

        public DateTime FechaActualizacion { get; set; }

        public int Estado { get; set; }

    }
}