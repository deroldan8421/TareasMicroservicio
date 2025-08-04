using Microsoft.Data.SqlClient;
using System.Data;
using TareasMicroservicio.Models;

namespace TareasMicroservicio.Data
{
    public class TareaRepository : ITareaRepository
    {
        private readonly string? _connectionString;

        public TareaRepository(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection");
        }

        public async Task<IEnumerable<Tarea>> ObtenerTodasAsync()
        {
            var tareas = new List<Tarea>();

            using var connection = new SqlConnection(_connectionString);
            var query = "SELECT Id, Titulo, Descripcion, FechaCreacion, FechaVencimiento, Estado FROM Tareas";

            using var command = new SqlCommand(query, connection);
            await connection.OpenAsync();

            using var reader = await command.ExecuteReaderAsync();
            while (await reader.ReadAsync())
            {
                var tarea = new Tarea
                {
                    Id = reader.GetInt32(0),
                    Titulo = reader.GetString(1),
                    Descripcion = reader.IsDBNull(2) ? null : reader.GetString(2),
                    FechaCreacion = reader.GetDateTime(3),
                    FechaVencimiento = reader.IsDBNull(4) ? (DateTime?)null : reader.GetDateTime(4),
                    Estado = reader.GetString(5)
                };

                tareas.Add(tarea);
            }

            return tareas;
        }

        public async Task<Tarea?> ObtenerPorIdAsync(int id)
        {
            using var connection = new SqlConnection(_connectionString);
            var query = @"SELECT Id, Titulo, Descripcion, FechaCreacion, FechaVencimiento, Estado
                  FROM Tareas
                  WHERE Id = @Id";

            using var command = new SqlCommand(query, connection);
            command.Parameters.AddWithValue("@Id", id);

            await connection.OpenAsync();
            using var reader = await command.ExecuteReaderAsync();

            if (await reader.ReadAsync())
            {
                return new Tarea
                {
                    Id = reader.GetInt32(0),
                    Titulo = reader.GetString(1),
                    Descripcion = reader.IsDBNull(2) ? null : reader.GetString(2),
                    FechaCreacion = reader.GetDateTime(3),
                    FechaVencimiento = reader.IsDBNull(4) ? (DateTime?)null : reader.GetDateTime(4),
                    Estado = reader.GetString(5)
                };
            }

            return null;
        }

        public async Task<int> CrearAsync(Tarea tarea)
        {
            using var connection = new SqlConnection(_connectionString);
            var query = @"INSERT INTO Tareas (Titulo, Descripcion, FechaCreacion, FechaVencimiento, Estado)
                  OUTPUT INSERTED.Id
                  VALUES (@Titulo, @Descripcion, @FechaCreacion, @FechaVencimiento, @Estado);";

            using var command = new SqlCommand(query, connection);
            command.Parameters.AddWithValue("@Titulo", tarea.Titulo);
            command.Parameters.AddWithValue("@Descripcion", (object?)tarea.Descripcion ?? DBNull.Value);
            command.Parameters.AddWithValue("@FechaCreacion", tarea.FechaCreacion);
            command.Parameters.AddWithValue("@FechaVencimiento", (object?)tarea.FechaVencimiento ?? DBNull.Value);
            command.Parameters.AddWithValue("@Estado", tarea.Estado);

            await connection.OpenAsync();
            var result = await command.ExecuteScalarAsync();
            if (result is int id)
            {
                tarea.Id = id;
                return id;
            }
            else
            {
                throw new Exception("No se pudo obtener el Id insertado.");
            }
        }
        public async Task<bool> ActualizarAsync(Tarea tarea)
        {
            using var connection = new SqlConnection(_connectionString);
            var query = @"UPDATE Tareas
                  SET Titulo = @Titulo,
                      Descripcion = @Descripcion,
                      FechaVencimiento = @FechaVencimiento,
                      Estado = @Estado
                  WHERE Id = @Id";

            using var command = new SqlCommand(query, connection);
            command.Parameters.AddWithValue("@Titulo", tarea.Titulo);
            command.Parameters.AddWithValue("@Descripcion", (object?)tarea.Descripcion ?? DBNull.Value);
            command.Parameters.AddWithValue("@FechaVencimiento", (object?)tarea.FechaVencimiento ?? DBNull.Value);
            command.Parameters.AddWithValue("@Estado", tarea.Estado);
            command.Parameters.AddWithValue("@Id", tarea.Id);

            await connection.OpenAsync();
            int rowsAffected = await command.ExecuteNonQueryAsync();
            return rowsAffected > 0;
        }
        public async Task<bool> EliminarAsync(int id)
        {
            using var connection = new SqlConnection(_connectionString);
            var query = "DELETE FROM Tareas WHERE Id = @Id";

            using var command = new SqlCommand(query, connection);
            command.Parameters.AddWithValue("@Id", id);

            await connection.OpenAsync();
            int rowsAffected = await command.ExecuteNonQueryAsync();
            return rowsAffected > 0;
        }

        public async Task<IEnumerable<Tarea>> ObtenerPorEstadoAsync(string estado)
        {
            var tareas = new List<Tarea>();

            using var connection = new SqlConnection(_connectionString);
            using var command = new SqlCommand("ObtenerTareasPorEstado", connection)
            {
                CommandType = CommandType.StoredProcedure
            };
            command.Parameters.AddWithValue("@Estado", estado);

            await connection.OpenAsync();
            using var reader = await command.ExecuteReaderAsync();

            while (await reader.ReadAsync())
            {
                var tarea = new Tarea
                {
                    Id = reader.GetInt32(0),
                    Titulo = reader.GetString(1),
                    Descripcion = reader.IsDBNull(2) ? null : reader.GetString(2),
                    FechaCreacion = reader.GetDateTime(3),
                    FechaVencimiento = reader.IsDBNull(4) ? (DateTime?)null : reader.GetDateTime(4),
                    Estado = reader.GetString(5)
                };

                tareas.Add(tarea);
            }

            return tareas;
        }


    }
}
