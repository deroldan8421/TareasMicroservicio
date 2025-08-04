
Prueba Técnica – Microservicio de Gestión de Tareas

Este proyecto implementa un microservicio en .NET 8 con autenticación JWT, que permite crear, consultar, actualizar y eliminar tareas. También incluye un cliente web sencillo (HTML + JavaScript) que consume la API REST.

---

## Estructura del Proyecto

---
### 🗂️ Estructura del Proyecto

```plaintext
TareasMicroservicio/
├── Backend/                  # API REST en .NET
│   ├── Controllers/
│   │   ├── AuthController.cs
│   │   └── TareasController.cs
│   ├── Data/
│   │   └── TareaRepository.cs
│   ├── Interfaces/
│   │   └── ITareaRepository.cs
│   ├── Models/
│   │   ├── Tarea.cs
│   │   └── LoginRequest.cs
│   ├── Program.cs
│   ├── appsettings.json
│   ├── TareasMicroservicio.csproj
│   └── wwwroot/
│       └── index.html        # Cliente web integrado (opcional)
│
├── Frontend/                # Cliente web independiente
│   └── index.html
│
├── Database/
│   └── script_tareas_completo.sql
│
├── README.md
└── TareasMicroservicio.sln

---

## Requisitos Previos

- [.NET SDK 8](https://dotnet.microsoft.com/en-us/download/dotnet/8.0)
- SQL Server (Express o superior)
- Visual Studio 2022+ o Visual Studio Code
- Navegador web moderno

---

## Ejecución del Backend (API REST)

1. Clona este repositorio:

   git clone https://github.com/deroldan8421/TareasMicroservicio.git
   

2. Abre la solución con Visual Studio o VS Code.

3. Verifica que la cadena de conexión en `appsettings.json` sea correcta para tu entorno:

   "ConnectionStrings": {
     "DefaultConnection": "Server=TU_SERVIDOR;Database=TareasDb;Trusted_Connection=True;TrustServerCertificate=True;"
   }   

4. Ejecuta el backend con:

   dotnet run   

   Por defecto, el API estará disponible en:  
   http://localhost:5007/api/tareas

---

## Base de Datos

Se incluye el archivo `script_base_datos.sql` en la raíz del proyecto. Este archivo contiene:

- Instrucciones `CREATE DATABASE`, `CREATE TABLE` e `INSERT`.
- Un procedimiento almacenado `ObtenerTareasPorEstado`.
- Ejemplos de subconsulta y auto-join para demostrar conocimientos en SQL.

### Restaurar la base de datos

1. Abre SQL Server Management Studio (SSMS)
2. Ejecuta el contenido de `script_base_datos.sql`


---

## Autenticación JWT

Credenciales de prueba:

- **Usuario: admin  
- **Clave: admin123

Endpoint de login:

POST http://localhost:5007/api/auth/login


Request JSON:

{
  "usuario": "admin",
  "clave": "admin123"
}

La respuesta incluirá un token JWT, necesario para acceder a todos los endpoints protegidos.

---

## Ejecución del Cliente Web

1. Abre el archivo:

   /Frontend/index.html
   

2. Ingresa con las credenciales `admin / admin123`.

3. Accede a las funcionalidades:

   - Crear tarea
   - Editar y eliminar tareas
   - Listar tareas
   - Cerrar sesión

El cliente incluye validaciones y envía el token JWT automáticamente en las peticiones.

---

## Pruebas con Postman

- Agrega el token JWT en el encabezado `Authorization`:

  Authorization: Bearer <tu_token>

- Puedes probar los endpoints:

  - `GET /api/tareas`
  - `GET /api/tareas/{id}`
  - `POST /api/tareas`
  - `PUT /api/tareas/{id}`
  - `DELETE /api/tareas/{id}`

---

## Consideraciones Técnicas

- Arquitectura basada en principios SOLID y Clean Code.
- API desarrollada sin ORM (solo ADO.NET).
- Controlador de autenticación separado (`AuthController`).
- Middleware y configuración de JWT centralizada en `Program.cs`.

---

## Autor

Desarrollado por Diego Edison Roldan Aguilar como parte de la prueba técnica para la empresa HASSQL SAS.
