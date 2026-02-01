# OrderManagement BackEnd

API REST para un sistema de gestión de órdenes con clientes, productos y detalles de orden, desarrollada en .NET 8 con ASP.NET Core.

## Stack Tecnológico

- .NET 8 – ASP.NET Core Web API
- SQL Server – Base de datos relacional
- Dapper – Mapeo de datos (procedimientos almacenados)
- AutoMapper – Mapeo entre entidades y DTOs
- Entity Framework Core Power Tools – Generación de entidades desde la base de datos

## Estructura del Proyecto

```
OrderManagement_BackEnd/
├── OrderManagement.API/
│   ├── Controllers/
│   │   ├── ClientesController.cs
│   │   ├── ProductosController.cs
│   │   └── OrdenController.cs
│   ├── DTOs/
│   │   └── (CrearOrdenDto, OrdenDto, DetalleOrdenDto, etc.)
│   ├── Extensions/
│   │   └── MappingProfileExtensions.cs   ← Perfil de AutoMapper
│   └── Program.cs
├── OrderManagement.BusinessLogic/
│   └── Services/
│       └── OrderManagementServices.cs    ← Lógica de negocio
├── OrderManagement.DataAccess/
│   └── Repositories/
│       ├── OrdenRepository.cs
│       ├── DetalleOrdenRepository.cs
│       ├── ClienteRepository.cs
│       └── ProductoRepository.cs
├── OrderManagement.Entities/
│   └── Entities/                         ← Generadas por EF Core Power Tools
└── Scripts/
    └── (Scripts SQL de la base de datos)
```

## Requisitos Previos

- .NET 8 SDK
- SQL Server (local o instancia nombrada)
- Visual Studio 2022 o VS Code

## Instalación y Configuración

1. Clonar el repositorio

```bash
git clone https://github.com/Stephtor29/OrderManagement_BackEnd.git
cd OrderManagement_BackEnd
```

2. Crear la base de datos

Ejecutar el script SQL proporcionado  en SQL Server Management Studio. El script crea las tablas, índices y datos de prueba.

3. Configurar la conexión

Editar el archivo `appsettings.json` en `OrderManagement.API` con tu cadena de conexión:

```json
{
  "ConnectionStrings": {
    "OrderManagementDB": "Server=TU_SERVIDOR;Database=OrderManagementDB;Trusted_Connection=true;"
  }
}
```

## Endpoints Disponibles

### Clientes

| GET | `/api/Clientes` | Listar todos los clientes |
| GET | `/api/Clientes/{id}` | Obtener cliente por ID |
| POST | `/api/Clientes` | Crear nuevo cliente |
| PUT | `/api/Clientes/{id}` | Actualizar cliente |
| DELETE | `/api/Clientes/{id}` | Eliminar cliente |

### Productos

| GET | `/api/Productos` | Listar todos los productos |
| GET | `/api/Productos/{id}` | Obtener producto por ID |
| POST | `/api/Productos` | Crear nuevo producto |
| PUT | `/api/Productos/{id}` | Actualizar producto |
| DELETE | `/api/Productos/{id}` | Eliminar producto |

### Órdenes

| POST | `/api/Orden/Crear` | Crear nueva orden con detalles |
| GET | `/api/Orden/Listar` | Listar todas las órdenes |
| GET | `/api/Orden/Buscar/{id}` | Obtener orden por ID con detalles |

## Ejemplo de Uso – Crear Orden

Request:

```json
POST /api/Orden/Crear
{
  "ordenId": 0,
  "clienteId": 1,
  "detalle": [
    { "productoId": 1, "cantidad": 2 },
    { "productoId": 3, "cantidad": 1 }
  ]
}
```

Response:

```json
{
  "success": true,
  "message": "Orden creada exitosamente",
  "errors": [],
  "data": {
    "ordenId": 1,
    "clienteNombre": "Juan Pérez",
    "impuesto": 419.99,
    "subtotal": 2799.97,
    "total": 3219.96,
    "fechaCreacion": "2026-02-01T15:30:00",
    "detalles": [
      {
        "detalleOrdenId": 1,
        "productoId": 1,
        "cantidad": 2,
        "impuesto": 389.99,
        "subtotal": 2599.98,
        "total": 2989.97
      },
      {
        "detalleOrdenId": 2,
        "productoId": 3,
        "cantidad": 1,
        "impuesto": 13.50,
        "subtotal": 89.99,
        "total": 103.49
      }
    ]
  }
}
```

## Decisiones Técnicas

Dapper en lugar de EF Core para consultas. Se usó EF Core Power Tools únicamente para generar las entidades desde el esquema existente. Todas las consultas y operaciones se realizan mediante Stored Procedures con Dapper, lo que da mayor control sobre la lógica SQL y mejor rendimiento en consultas complejas como la búsqueda de órdenes con detalles.

Transacciones en la creación de órdenes. El método `CrearOrden` maneja toda la operación dentro de una sola transacción (`SqlTransaction`). Si cualquier paso falla (inserción de orden, detalle, o actualización de existencia), se hace rollback completo, garantizando consistencia en la base de datos.

Capas separadas (Controller → Service → Repository). El controller maneja validación de entrada y respuestas HTTP. El servicio contiene la lógica de negocio (validaciones de dominio, cálculos, transacciones). El repositorio se encarga exclusivamente de la comunicación con la base de datos.

## Formato de Respuesta

Todas las respuestas siguen el modelo `ApiResponse<T>`:

```json
{
  "success": true,
  "message": "",
  "errors": [],
  "data": {}
}
```

En caso de error:

```json
{
  "success": false,
  "message": "Descripción del error",
  "errors": ["Detalle específico del error"],
  "data": null
}
```
