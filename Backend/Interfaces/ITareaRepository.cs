using TareasMicroservicio.Models;

namespace TareasMicroservicio.Data
{
    public interface ITareaRepository
    {
        Task<IEnumerable<Tarea>> ObtenerTodasAsync();
        Task<Tarea?> ObtenerPorIdAsync(int id);
        Task<int> CrearAsync(Tarea tarea);
        Task<bool> ActualizarAsync(Tarea tarea);
        Task<bool> EliminarAsync(int id);
        Task<IEnumerable<Tarea>> ObtenerPorEstadoAsync(string estado);
    }
}
