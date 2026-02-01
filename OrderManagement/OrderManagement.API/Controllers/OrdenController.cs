using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using OrderManagement.API.DTOs;
using OrderManagement.BusinessLogic.Services;
using OrderManagement.Entities.Entities;

namespace OrderManagement.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class OrdenController : ControllerBase
    {
        private readonly OrderManagementServices _ordenService;
        private readonly IMapper _mapper;

        public OrdenController(OrderManagementServices ordenService, IMapper mapper)
        {
            _ordenService = ordenService;
            _mapper = mapper;
        }

        /// <summary>
        /// POST /api/Orden/Crear - Crear nueva orden con detalles
        /// </summary>
        [HttpPost("Crear")]
        public IActionResult Crear([FromBody] CrearOrdenDto request)
        {
            // Validación del modelo
            if (!ModelState.IsValid)
            {
                var errors = ModelState.Values
                    .SelectMany(v => v.Errors)
                    .Select(e => e.ErrorMessage)
                    .ToList();

                var validationResponse = ApiResponse<OrdenDto>.ErrorResponse(
                    "Error de validación",
                    errors
                );
                return BadRequest(validationResponse);
            }

            // Validación: OrdenId debe ser 0
            if (request.OrdenId != 0)
            {
                var response = ApiResponse<OrdenDto>.ErrorResponse(
                    "Error al crear la orden",
                    "El OrdenId debe ser 0 para órdenes nuevas"
                );
                return BadRequest(response);
            }

            // Validación: Debe tener al menos un detalle
            if (request.Detalle == null || !request.Detalle.Any())
            {
                var response = ApiResponse<OrdenDto>.ErrorResponse(
                    "Error al crear la orden",
                    "Debe incluir al menos un producto en la orden"
                );
                return BadRequest(response);
            }

            // Convertir detalles a lista de tuplas
            var detalles = request.Detalle
                .Select(d => (d.ProductoId, d.Cantidad))
                .ToList();

            // Llamar al servicio
            var serviceResult = _ordenService.CrearOrden(request.ClienteId, detalles);

            if (serviceResult.Success)
            {
                var orden = (Orden)serviceResult.Data;
                var ordenDto = _mapper.Map<OrdenDto>(orden);

                var response = ApiResponse<OrdenDto>.SuccessResponse(
                    ordenDto,
                    "Orden creada exitosamente"
                );
                return Ok(response);
            }

            var errorResponse = ApiResponse<OrdenDto>.ErrorResponse(
                serviceResult.Message,
                serviceResult.Errors
            );
            return BadRequest(errorResponse);
        }

        /// <summary>
        /// GET /api/Orden/Listar - Listar todas las órdenes
        /// </summary>
        [HttpGet("Listar")]
        public IActionResult Listar()
        {
            var serviceResult = _ordenService.ListarOrdenes();

            if (serviceResult.Success)
            {
                var ordenes = (IEnumerable<Orden>)serviceResult.Data;
                var ordenesDto = _mapper.Map<IEnumerable<OrdenDto>>(ordenes);

                var response = ApiResponse<IEnumerable<OrdenDto>>.SuccessResponse(ordenesDto);
                return Ok(response);
            }

            var errorResponse = ApiResponse<IEnumerable<OrdenDto>>.ErrorResponse(
                serviceResult.Message,
                serviceResult.Errors
            );
            return BadRequest(errorResponse);
        }

        /// <summary>
        /// GET /api/Orden/Buscar/{id} - Obtener orden por ID con sus detalles
        /// </summary>
        [HttpGet("Buscar/{id}")]
        public IActionResult Buscar(int id)
        {
            if (id <= 0)
            {
                var response = ApiResponse<OrdenDto>.ErrorResponse(
                    "ID inválido",
                    "El ID debe ser mayor a 0"
                );
                return BadRequest(response);
            }

            var serviceResult = _ordenService.BuscarOrden(id);

            if (serviceResult.Success)
            {
                if (serviceResult.Data == null)
                {
                    var notFoundResponse = ApiResponse<OrdenDto>.ErrorResponse(
                        "Orden no encontrada",
                        "No existe una orden con el ID especificado"
                    );
                    return NotFound(notFoundResponse);
                }

                var orden = (Orden)serviceResult.Data;
                var ordenDto = _mapper.Map<OrdenDto>(orden);

                var response = ApiResponse<OrdenDto>.SuccessResponse(ordenDto);
                return Ok(response);
            }

            var errorResponse = ApiResponse<OrdenDto>.ErrorResponse(
                serviceResult.Message,
                serviceResult.Errors
            );
            return BadRequest(errorResponse);
        }
    }
}