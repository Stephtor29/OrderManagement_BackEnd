using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using OrderManagement.API.DTOs;
using OrderManagement.BusinessLogic.Services;
using OrderManagement.Entities.Entities;

namespace OrderManagement.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProductoController : ControllerBase
    {
        private readonly OrderManagementServices _orderServices;
        private readonly IMapper _mapper;

        public ProductoController(OrderManagementServices orderServices, IMapper mapper)
        {
            _orderServices = orderServices;
            _mapper = mapper;
        }

        /// <summary>
        /// GET /api/Producto/Listar - Listar todos los productos
        /// </summary>
        [HttpGet("Listar")]
        public IActionResult Listar()
        {
            var serviceResult = _orderServices.ListarProductos();

            if (serviceResult.Success)
            {
                var productos = (IEnumerable<Producto>)serviceResult.Data;
                var productosDto = _mapper.Map<IEnumerable<ProductoDTo>>(productos);

                var response = ApiResponse<IEnumerable<ProductoDTo>>.SuccessResponse(productosDto);
                return Ok(response);
            }

            var errorResponse = ApiResponse<IEnumerable<ProductoDTo>>.ErrorResponse(
                serviceResult.Message,
                serviceResult.Errors
            );
            return BadRequest(errorResponse);
        }

        /// <summary>
        /// GET /api/Producto/Buscar/{id} - Obtener producto por ID
        /// </summary>
        [HttpGet("Buscar/{id}")]
        public IActionResult Buscar(int id)
        {
            if (id <= 0)
            {
                var response = ApiResponse<ProductoDTo>.ErrorResponse(
                    "ID inválido",
                    "El ID debe ser mayor a 0"
                );
                return BadRequest(response);
            }

            var serviceResult = _orderServices.BuscarProducto(id);

            if (serviceResult.Success)
            {
                if (serviceResult.Data == null)
                {
                    var notFoundResponse = ApiResponse<ProductoDTo>.ErrorResponse(
                        "Producto no encontrado",
                        "No existe un producto con el ID especificado"
                    );
                    return NotFound(notFoundResponse);
                }

                var producto = (Producto)serviceResult.Data;
                var productoDto = _mapper.Map<ProductoDTo>(producto);

                var response = ApiResponse<ProductoDTo>.SuccessResponse(productoDto);
                return Ok(response);
            }

            var errorResponse = ApiResponse<ProductoDTo>.ErrorResponse(
                serviceResult.Message,
                serviceResult.Errors
            );
            return BadRequest(errorResponse);
        }

        /// <summary>
        /// POST /api/Producto/Insertar - Crear nuevo producto
        /// </summary>
        [HttpPost("Insertar")]
        public IActionResult Insertar([FromBody] ProductoDTo item)
        {
            // Validación del modelo
            if (!ModelState.IsValid)
            {
                var errors = ModelState.Values
                    .SelectMany(v => v.Errors)
                    .Select(e => e.ErrorMessage)
                    .ToList();

                var validationResponse = ApiResponse<ProductoDTo>.ErrorResponse(
                    "Error de validación",
                    errors
                );
                return BadRequest(validationResponse);
            }

            // Validación: ProductoId debe ser 0
            if (item.ProductoId != 0)
            {
                var response = ApiResponse<ProductoDTo>.ErrorResponse(
                    "Error al crear el producto",
                    "El ProductoId debe ser 0 para productos nuevos"
                );
                return BadRequest(response);
            }

            // Validación: Nombre requerido (3-100 caracteres)
            if (string.IsNullOrWhiteSpace(item.Nombre) || item.Nombre.Length < 3 || item.Nombre.Length > 100)
            {
                var response = ApiResponse<ProductoDTo>.ErrorResponse(
                    "Error al crear el producto",
                    "El nombre es requerido y debe tener entre 3 y 100 caracteres"
                );
                return BadRequest(response);
            }

            // Validación: Precio debe ser mayor a 0
            if (item.Precio <= 0)
            {
                var response = ApiResponse<ProductoDTo>.ErrorResponse(
                    "Error al crear el producto",
                    "El precio debe ser mayor a 0"
                );
                return BadRequest(response);
            }

            // Validación: Existencia debe ser >= 0
            if (item.Existencia < 0)
            {
                var response = ApiResponse<ProductoDTo>.ErrorResponse(
                    "Error al crear el producto",
                    "La existencia debe ser mayor o igual a 0"
                );
                return BadRequest(response);
            }

            var producto = _mapper.Map<Producto>(item);
            var serviceResult = _orderServices.InsertarProducto(producto);

            if (serviceResult.Success)
            {
                var productoCreado = (Producto)serviceResult.Data;
                var productoDto = _mapper.Map<ProductoDTo>(productoCreado);

                var response = ApiResponse<ProductoDTo>.SuccessResponse(
                    productoDto,
                    "Producto creado exitosamente"
                );
                return Ok(response);
            }

            var errorResponse = ApiResponse<ProductoDTo>.ErrorResponse(
                "Error al crear el producto",
                serviceResult.Errors
            );
            return BadRequest(errorResponse);
        }

        /// <summary>
        /// PUT /api/Producto/Actualizar/{id} - Actualizar producto existente
        /// </summary>
        [HttpPut("Actualizar/{id}")]
        public IActionResult Actualizar(int id, [FromBody] ProductoDTo item)
        {
            if (id <= 0)
            {
                var response = ApiResponse<ProductoDTo>.ErrorResponse(
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

                var validationResponse = ApiResponse<ProductoDTo>.ErrorResponse(
                    "Error de validación",
                    errors
                );
                return BadRequest(validationResponse);
            }

            // Validación: Nombre requerido (3-100 caracteres)
            if (string.IsNullOrWhiteSpace(item.Nombre) || item.Nombre.Length < 3 || item.Nombre.Length > 100)
            {
                var response = ApiResponse<ProductoDTo>.ErrorResponse(
                    "Error al actualizar el producto",
                    "El nombre es requerido y debe tener entre 3 y 100 caracteres"
                );
                return BadRequest(response);
            }

            // Validación: Precio debe ser mayor a 0
            if (item.Precio <= 0)
            {
                var response = ApiResponse<ProductoDTo>.ErrorResponse(
                    "Error al actualizar el producto",
                    "El precio debe ser mayor a 0"
                );
                return BadRequest(response);
            }

            // Validación: Existencia debe ser >= 0
            if (item.Existencia < 0)
            {
                var response = ApiResponse<ProductoDTo>.ErrorResponse(
                    "Error al actualizar el producto",
                    "La existencia debe ser mayor o igual a 0"
                );
                return BadRequest(response);
            }

            // Verificar que el producto existe
            var existeResult = _orderServices.BuscarProducto(id);
            if (!existeResult.Success || existeResult.Data == null)
            {
                var notFoundResponse = ApiResponse<ProductoDTo>.ErrorResponse(
                    "Producto no encontrado",
                    "No existe un producto con el ID especificado"
                );
                return NotFound(notFoundResponse);
            }

            item.ProductoId = id; // Asegurar que el ID coincida
            var producto = _mapper.Map<Producto>(item);
            var serviceResult = _orderServices.ActualizarProducto(producto);

            if (serviceResult.Success)
            {
                var productoActualizado = (Producto)serviceResult.Data;
                var productoDto = _mapper.Map<ProductoDTo>(productoActualizado);

                var response = ApiResponse<ProductoDTo>.SuccessResponse(
                    productoDto,
                    "Producto actualizado exitosamente"
                );
                return Ok(response);
            }

            var errorResponse = ApiResponse<ProductoDTo>.ErrorResponse(
                "Error al actualizar el producto",
                serviceResult.Errors
            );
            return BadRequest(errorResponse);
        }

        /// <summary>
        /// DELETE /api/Producto/Eliminar/{id} - Eliminar producto
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

            var serviceResult = _orderServices.EliminarProducto(id);

            if (serviceResult.Success)
            {
                var response = ApiResponse<object>.SuccessResponse(
                    null,
                    "Producto eliminado exitosamente"
                );
                return Ok(response);
            }

            var errorResponse = ApiResponse<object>.ErrorResponse(
                "Error al eliminar el producto",
                serviceResult.Errors
            );
            return BadRequest(errorResponse);
        }
    }
}