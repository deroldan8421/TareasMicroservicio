namespace TareasMicroservicio.Models
{
    public class Tarea
    {
        public int Id { get; set; }

        public string Titulo { get; set; } = string.Empty;

        public string? Descripcion { get; set; }

        public DateTime FechaCreacion { get; set; }

        public DateTime? FechaVencimiento { get; set; } // Es opcional

        public string Estado { get; set; } = "Pendiente";
    }
}
