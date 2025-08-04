using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TareasMicroservicio.Data;
using TareasMicroservicio.Models;

namespace TareasMicroservicio.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class TareasController : ControllerBase
    {
        private readonly ITareaRepository _repository;

        public TareasController(ITareaRepository repository)
        {
            _repository = repository;
        }

        // GET: api/tareas
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Tarea>>> ObtenerTodas()
        {
            try
            {
                var tareas = await _repository.ObtenerTodasAsync();
                return Ok(tareas);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = "Error al obtener las tareas: " + ex.Message });
            }
        }

        // GET: api/tareas/{id}
        [HttpGet("{id}")]
        public async Task<ActionResult<Tarea>> ObtenerPorId(int id)
        {
            try
            {
                var tarea = await _repository.ObtenerPorIdAsync(id);
                if (tarea == null)
                    return NotFound(new { error = $"No se encontró una tarea con id {id}" });

                return Ok(tarea);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = "Error al obtener la tarea: " + ex.Message });
            }
        }

        // POST: api/tareas
        [HttpPost]        
        public async Task<ActionResult> Crear([FromBody] Tarea nuevaTarea)
        {
            if (nuevaTarea == null)
                return BadRequest(new { error = "La tarea no puede ser nula." });

            if (!Request.ContentType?.Contains("application/json") ?? true)
                return BadRequest(new { error = "El Content-Type debe ser application/json." });

            if (string.IsNullOrWhiteSpace(nuevaTarea.Titulo))
                return BadRequest(new { error = "El título es obligatorio." });

            var estadosPermitidos = new[] { "Pendiente", "En Proceso", "Completada" };
            if (!estadosPermitidos.Contains(nuevaTarea.Estado))
                return BadRequest(new { error = "El estado debe ser: Pendiente, En Proceso o Completada." });

            if (nuevaTarea.FechaVencimiento != null && nuevaTarea.FechaVencimiento < DateTime.Now)
                return BadRequest(new { error = "La fecha de vencimiento debe ser futura." });

            if (nuevaTarea.FechaCreacion == default)
                nuevaTarea.FechaCreacion = DateTime.Now;

            try
            {
                var idGenerado = await _repository.CrearAsync(nuevaTarea);
                nuevaTarea.Id = idGenerado;

                return CreatedAtAction(nameof(ObtenerPorId), new { id = nuevaTarea.Id }, nuevaTarea);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = "Error al crear la tarea: " + ex.Message });
            }
        }


        // PUT: api/tareas/{id}
        [HttpPut("{id}")]
        public async Task<ActionResult> Actualizar(int id, [FromBody] Tarea tareaActualizada)
        {
            if (tareaActualizada == null)
                return BadRequest(new { error = "Los datos de la tarea son requeridos." });

            if (!Request.ContentType?.Contains("application/json") ?? true)
                return BadRequest(new { error = "El Content-Type debe ser application/json." });

            try
            {
                var existente = await _repository.ObtenerPorIdAsync(id);
                if (existente == null)
                    return NotFound(new { error = $"No existe una tarea con id {id}" });

                if (string.IsNullOrWhiteSpace(tareaActualizada.Titulo))
                    return BadRequest(new { error = "El título es obligatorio." });

                var estadosPermitidos = new[] { "Pendiente", "En Proceso", "Completada" };
                if (!estadosPermitidos.Contains(tareaActualizada.Estado))
                    return BadRequest(new { error = "El estado debe ser: Pendiente, En Proceso o Completada." });

                if (tareaActualizada.FechaVencimiento != null && tareaActualizada.FechaVencimiento < DateTime.Now)
                    return BadRequest(new { error = "La fecha de vencimiento debe ser futura." });

                tareaActualizada.Id = id;
                tareaActualizada.FechaCreacion = existente.FechaCreacion;

                var actualizado = await _repository.ActualizarAsync(tareaActualizada);
                if (!actualizado)
                    return StatusCode(500, new { error = "No se pudo actualizar la tarea." });

                return NoContent();
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = "Error al actualizar la tarea: " + ex.Message });
            }
        }


        // DELETE: api/tareas/{id}
        [HttpDelete("{id}")]
        public async Task<ActionResult> Eliminar(int id)
        {
            try
            {
                var existente = await _repository.ObtenerPorIdAsync(id);
                if (existente == null)
                    return NotFound(new { error = $"No existe una tarea con id {id}" });

                var eliminado = await _repository.EliminarAsync(id);
                if (!eliminado)
                    return StatusCode(500, new { error = "No se pudo eliminar la tarea." });

                return NoContent();
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = "Error al eliminar la tarea: " + ex.Message });
            }
        }

        // GET: api/tareas/estado/{estado}

        [HttpGet("estado/{estado}")]
        public async Task<ActionResult<IEnumerable<Tarea>>> ObtenerPorEstado(string estado)
        {
            try
            {
                var resultados = await _repository.ObtenerPorEstadoAsync(estado);
                return Ok(resultados);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = "Error al obtener las tareas por estado: " + ex.Message });
            }
        }


    }
}
