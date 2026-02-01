using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using OrderManagement.API.DTOs;
using OrderManagement.BusinessLogic.Services;
using OrderManagement.Entities.Entities;

namespace OrderManagement.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ClienteController : ControllerBase
    {
        private readonly OrderManagementServices _clienteService;
        private readonly IMapper _mapper;

        public ClienteController(OrderManagementServices clienteService, IMapper mapper)
        {
            _clienteService = clienteService;
            _mapper = mapper;
        }

        /// <summary>
        /// GET /api/Cliente/Listar - Listar todos los clientes
        /// </summary>
        [HttpGet("Listar")]
        public IActionResult Listar()
        {
            var serviceResult = _clienteService.ListarClientes();

            if (serviceResult.Success)
            {
                var clientes = (IEnumerable<Cliente>)serviceResult.Data;
                var clientesDto = _mapper.Map<IEnumerable<ClienteDto>>(clientes);

                var response = ApiResponse<IEnumerable<ClienteDto>>.SuccessResponse(clientesDto);
                return Ok(response);
            }

            var errorResponse = ApiResponse<IEnumerable<ClienteDto>>.ErrorResponse(
                serviceResult.Message,
                serviceResult.Errors
            );
            return BadRequest(errorResponse);
        }

        /// <summary>
        /// GET /api/Cliente/Buscar/{id} - Obtener cliente por ID
        /// </summary>
        [HttpGet("Buscar/{id}")]
        public IActionResult Buscar(int id)
        {
            if (id <= 0)
            {
                var response = ApiResponse<ClienteDto>.ErrorResponse(
                    "ID inválido",
                    "El ID debe ser mayor a 0"
                );
                return BadRequest(response);
            }

            var serviceResult = _clienteService.BuscarCliente(id);

            if (serviceResult.Success)
            {
                if (serviceResult.Data == null)
                {
                    var notFoundResponse = ApiResponse<ClienteDto>.ErrorResponse(
                        "Cliente no encontrado",
                        "No existe un cliente con el ID especificado"
                    );
                    return NotFound(notFoundResponse);
                }

                var cliente = (Cliente)serviceResult.Data;
                var clienteDto = _mapper.Map<ClienteDto>(cliente);

                var response = ApiResponse<ClienteDto>.SuccessResponse(clienteDto);
                return Ok(response);
            }

            var errorResponse = ApiResponse<ClienteDto>.ErrorResponse(
                serviceResult.Message,
                serviceResult.Errors
            );
            return BadRequest(errorResponse);
        }

        /// <summary>
        /// POST /api/Cliente/Insertar - Crear nuevo cliente
        /// </summary>
        [HttpPost("Insertar")]
        public IActionResult Insertar([FromBody] ClienteDto item)
        {
            // Validación del modelo
            if (!ModelState.IsValid)
            {
                var errors = ModelState.Values
                    .SelectMany(v => v.Errors)
                    .Select(e => e.ErrorMessage)
                    .ToList();

                var validationResponse = ApiResponse<ClienteDto>.ErrorResponse(
                    "Error de validación",
                    errors
                );
                return BadRequest(validationResponse);
            }

            // Validación: ClienteId debe ser 0
            if (item.ClienteId != 0)
            {
                var response = ApiResponse<ClienteDto>.ErrorResponse(
                    "Error al crear el cliente",
                    "El ClienteId debe ser 0 para clientes nuevos"
                );
                return BadRequest(response);
            }

            // Validación: Nombre requerido (3-100 caracteres)
            if (string.IsNullOrWhiteSpace(item.Nombre) || item.Nombre.Length < 3 || item.Nombre.Length > 100)
            {
                var response = ApiResponse<ClienteDto>.ErrorResponse(
                    "Error al crear el cliente",
                    "El nombre es requerido y debe tener entre 3 y 100 caracteres"
                );
                return BadRequest(response);
            }

            // Validación: Identidad requerida
            if (string.IsNullOrWhiteSpace(item.Identidad))
            {
                var response = ApiResponse<ClienteDto>.ErrorResponse(
                    "Error al crear el cliente",
                    "La identidad es requerida"
                );
                return BadRequest(response);
            }

            var cliente = _mapper.Map<Cliente>(item);
            var serviceResult = _clienteService.InsertarCliente(cliente);

            if (serviceResult.Success)
            {
                var clienteCreado = (Cliente)serviceResult.Data;
                var clienteDto = _mapper.Map<ClienteDto>(clienteCreado);

                var response = ApiResponse<ClienteDto>.SuccessResponse(
                    clienteDto,
                    "Cliente creado exitosamente"
                );
                return Ok(response);
            }

            var errorResponse = ApiResponse<ClienteDto>.ErrorResponse(
                serviceResult.Message,
                serviceResult.Errors
            );
            return BadRequest(errorResponse);
        }

        /// <summary>
        /// PUT /api/Cliente/Actualizar/{id} - Actualizar cliente existente
        /// </summary>
        [HttpPut("Actualizar/{id}")]
        public IActionResult Actualizar(int id, [FromBody] ClienteDto item)
        {
            if (id <= 0)
            {
                var response = ApiResponse<ClienteDto>.ErrorResponse(
                    "ID inválido",
                    "El ID debe ser mayor a 0"
                );
                return BadRequest(response);
            }

            // Validación del modelo
            if (!ModelState.IsValid)
            {
                var errors = ModelState.Values
                    .SelectMany(v => v.Errors)
                    .Select(e => e.ErrorMessage)
                    .ToList();

                var validationResponse = ApiResponse<ClienteDto>.ErrorResponse(
                    "Error de validación",
                    errors
                );
                return BadRequest(validationResponse);
            }

            // Validación: Nombre requerido (3-100 caracteres)
            if (string.IsNullOrWhiteSpace(item.Nombre) || item.Nombre.Length < 3 || item.Nombre.Length > 100)
            {
                var response = ApiResponse<ClienteDto>.ErrorResponse(
                    "Error al actualizar el cliente",
                    "El nombre es requerido y debe tener entre 3 y 100 caracteres"
                );
                return BadRequest(response);
            }

            // Validación: Identidad requerida
            if (string.IsNullOrWhiteSpace(item.Identidad))
            {
                var response = ApiResponse<ClienteDto>.ErrorResponse(
                    "Error al actualizar el cliente",
                    "La identidad es requerida"
                );
                return BadRequest(response);
            }

            // Verificar que el cliente existe
            var existeResult = _clienteService.BuscarCliente(id);
            if (!existeResult.Success || existeResult.Data == null)
            {
                var notFoundResponse = ApiResponse<ClienteDto>.ErrorResponse(
                    "Cliente no encontrado",
                    "No existe un cliente con el ID especificado"
                );
                return NotFound(notFoundResponse);
            }

            item.ClienteId = id; // Asegurar que el ID coincida
            var cliente = _mapper.Map<Cliente>(item);
            var serviceResult = _clienteService.ActualizarCliente(cliente);

            if (serviceResult.Success)
            {
                var clienteActualizado = (Cliente)serviceResult.Data;
                var clienteDto = _mapper.Map<ClienteDto>(clienteActualizado);

                var response = ApiResponse<ClienteDto>.SuccessResponse(
                    clienteDto,
                    "Cliente actualizado exitosamente"
                );
                return Ok(response);
            }

            var errorResponse = ApiResponse<ClienteDto>.ErrorResponse(
                serviceResult.Message,
                serviceResult.Errors
            );
            return BadRequest(errorResponse);
        }

        /// <summary>
        /// DELETE /api/Cliente/Eliminar/{id} - Eliminar cliente
        /// </summary>
        [HttpDelete("Eliminar/{id}")]
        public IActionResult Eliminar(int id)
        {
            if (id <= 0)
            {
                var response = ApiResponse<object>.ErrorResponse(
                    "ID inválido",
                    "El ID debe ser mayor a 0"
                );
                return BadRequest(response);
            }

            var serviceResult = _clienteService.EliminarCliente(id);

            if (serviceResult.Success)
            {
                var response = ApiResponse<object>.SuccessResponse(
                    null,
                    "Cliente eliminado exitosamente"
                );
                return Ok(response);
            }

            var errorResponse = ApiResponse<object>.ErrorResponse(
                serviceResult.Message,
                serviceResult.Errors
            );
            return BadRequest(errorResponse);
        }
    }
}