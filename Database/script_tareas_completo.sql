-- Crear base de datos (si no existe)
IF NOT EXISTS (SELECT name FROM sys.databases WHERE name = 'TareasDB')
BEGIN
    CREATE DATABASE TareasDB;
END
GO

-- Usar la base de datos
USE TareasDB;
GO

-- Crear tabla Tareas
IF OBJECT_ID('Tareas', 'U') IS NOT NULL
    DROP TABLE Tareas;
GO

CREATE TABLE Tareas (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    Titulo NVARCHAR(100) NOT NULL,
    Descripcion NVARCHAR(500),
    FechaCreacion DATETIME NOT NULL DEFAULT GETDATE(),
    FechaVencimiento DATETIME,
    Estado NVARCHAR(50) NOT NULL
);
GO

-- Insertar datos de ejemplo
INSERT INTO Tareas (Titulo, Descripcion, FechaVencimiento, Estado)
VALUES 
('Prueba técnica', 'Realizar microservicio en C# sin ORM', '2025-08-04', 'Pendiente'),
('Revisión API', 'Verificar endpoints con Postman', '2025-08-05', 'En progreso'),
('Documentación', 'Completar README y comentarios', '2025-08-06', 'Completada');
GO

-- Crear procedimiento almacenado para obtener tareas por estado
IF OBJECT_ID('ObtenerTareasPorEstado', 'P') IS NOT NULL
    DROP PROCEDURE ObtenerTareasPorEstado;
GO

CREATE PROCEDURE ObtenerTareasPorEstado
    @Estado NVARCHAR(50)
AS
BEGIN
    SELECT Id, Titulo, Descripcion, FechaCreacion, FechaVencimiento, Estado
    FROM Tareas
    WHERE Estado = @Estado;
END;
GO

-- Subconsulta
SELECT * 
FROM Tareas
WHERE Estado IN (
    SELECT Estado
    FROM Tareas
    WHERE FechaVencimiento > GETDATE()
);
GO

-- Auto-join
SELECT t1.Id, t1.Titulo, t2.FechaVencimiento
FROM Tareas t1
JOIN Tareas t2 ON t1.Id = t2.Id
WHERE t1.Estado = 'Pendiente';
GO
