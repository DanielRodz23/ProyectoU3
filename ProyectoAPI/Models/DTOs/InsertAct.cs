namespace ProyectoAPI.Models.DTOs
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
