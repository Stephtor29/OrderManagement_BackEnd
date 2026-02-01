using Microsoft.Data.SqlClient;
using OrderManagement.DataAccess;
using OrderManagement.DataAccess.Repositories;
using OrderManagement.Entities.Entities;

namespace OrderManagement.BusinessLogic.Services
{
    public class OrderManagementServices
    {
        private readonly ProductoRepository _productoRepository;
        private readonly ClienteRepository _clienteRepository;
        private readonly OrdenRepository _ordenRepository;
        private readonly DetalleOrdenRepository _detalleOrdenRepository;


        public OrderManagementServices(ProductoRepository productoRepository, ClienteRepository clienteRepository,
            OrdenRepository ordenRepository, DetalleOrdenRepository detalleOrdenRepository)
        {
            _productoRepository = productoRepository;
            _clienteRepository = clienteRepository;
            _ordenRepository = ordenRepository;
            _detalleOrdenRepository = detalleOrdenRepository;
        }

        #region Producto

        public ServiceResult ListarProductos()
        {
            var result = new ServiceResult();
            try
            {
                var list = _productoRepository.List();

                if (list == null || !list.Any())
                {
                    return result.Ok(new List<Producto>(), "No hay productos registrados");
                }

                return result.Ok(list);
            }
            catch (Exception ex)
            {
                return result.Error("Error al listar los productos", ex.Message);
            }
        }

        public ServiceResult BuscarProducto(int? id)
        {
            var result = new ServiceResult();
            try
            {
                if (id == null || id <= 0)
                {
                    return result.Error("ID inválido", "El ID del producto debe ser mayor a 0");
                }

                var producto = _productoRepository.Find(id);

                if (producto == null)
                {
                    return result.Error("Producto no encontrado", "No existe un producto con el ID especificado");
                }

                return result.Ok(producto);
            }
            catch (Exception ex)
            {
                return result.Error("Error al buscar el producto", ex.Message);
            }
        }

        public ServiceResult InsertarProducto(Producto item)
        {
            var result = new ServiceResult();
            try
            {
                if (item == null)
                {
                    return result.Error("Error al crear el producto", "Los datos del producto son inválidos");
                }

                // Validaciones adicionales
                if (string.IsNullOrWhiteSpace(item.Nombre))
                {
                    return result.Error("Error al crear el producto", "El nombre del producto es requerido");
                }

                if (item.Precio <= 0)
                {
                    return result.Error("Error al crear el producto", "El precio debe ser mayor a 0");
                }

                if (item.Existencia < 0)
                {
                    return result.Error("Error al crear el producto", "La existencia no puede ser negativa");
                }

                // Asignar fecha de creación
                item.CreatedAt = DateTime.Now;

                var requestStatus = _productoRepository.Insert(item);

                if (requestStatus.CodeStatus == 0)
                {
                    return result.Error("Error al crear el producto", requestStatus.MessageStatus);
                }

                // Si la inserción fue exitosa, buscar el producto creado
                // Nota: Idealmente el SP debería retornar el ID del producto creado
                return result.Ok(item, "Producto creado exitosamente");
            }
            catch (Exception ex)
            {
                return result.Error("Error al crear el producto", ex.Message);
            }
        }

        public ServiceResult ActualizarProducto(Producto item)
        {
            var result = new ServiceResult();
            try
            {
                if (item == null)
                {
                    return result.Error("Error al actualizar el producto", "Los datos del producto son inválidos");
                }

                if (item.ProductoId <= 0)
                {
                    return result.Error("Error al actualizar el producto", "El ID del producto es inválido");
                }

                // Validaciones adicionales
                if (string.IsNullOrWhiteSpace(item.Nombre))
                {
                    return result.Error("Error al actualizar el producto", "El nombre del producto es requerido");
                }

                if (item.Precio <= 0)
                {
                    return result.Error("Error al actualizar el producto", "El precio debe ser mayor a 0");
                }

                if (item.Existencia < 0)
                {
                    return result.Error("Error al actualizar el producto", "La existencia no puede ser negativa");
                }

                // Verificar que el producto existe
                var productoExiste = _productoRepository.Find(item.ProductoId);
                if (productoExiste == null)
                {
                    return result.Error("Producto no encontrado", "No existe un producto con el ID especificado");
                }

                var requestStatus = _productoRepository.Update(item);

                if (requestStatus.CodeStatus == 0)
                {
                    return result.Error("Error al actualizar el producto", requestStatus.MessageStatus);
                }

                return result.Ok(item, "Producto actualizado exitosamente");
            }
            catch (Exception ex)
            {
                return result.Error("Error al actualizar el producto", ex.Message);
            }
        }

        public ServiceResult EliminarProducto(int? id)
        {
            var result = new ServiceResult();
            try
            {
                if (id == null || id <= 0)
                {
                    return result.Error("Error al eliminar el producto", "El ID del producto es inválido");
                }

                // Verificar que el producto existe antes de eliminar
                var productoExiste = _productoRepository.Find(id);
                if (productoExiste == null)
                {
                    return result.Error("Producto no encontrado", "No existe un producto con el ID especificado");
                }

                var requestStatus = _productoRepository.Delete(id);

                if (requestStatus.CodeStatus == 0)
                {
                    return result.Error("Error al eliminar el producto", requestStatus.MessageStatus);
                }

                return result.Ok(null, "Producto eliminado exitosamente");
            }
            catch (Exception ex)
            {
                return result.Error("Error al eliminar el producto", ex.Message);
            }
        }

        #endregion


        #region Cliente
          public ServiceResult ListarClientes()
        {
            var result = new ServiceResult();
            try
            {
                var list = _clienteRepository.List();

                if (list == null || !list.Any())
                {
                    return result.Ok(new List<Cliente>(), "No hay clientes registrados");
                }

                return result.Ok(list);
            }
            catch (Exception ex)
            {
                return result.Error("Error al listar los clientes", ex.Message);
            }
        }

        public ServiceResult BuscarCliente(int? id)
        {
            var result = new ServiceResult();
            try
            {
                if (id == null || id <= 0)
                {
                    return result.Error("ID inválido", "El ID del cliente debe ser mayor a 0");
                }

                var cliente = _clienteRepository.Find(id);

                if (cliente == null)
                {
                    return result.Error("Cliente no encontrado", "No existe un cliente con el ID especificado");
                }

                return result.Ok(cliente);
            }
            catch (Exception ex)
            {
                return result.Error("Error al buscar el cliente", ex.Message);
            }
        }

        public ServiceResult InsertarCliente(Cliente item)
        {
            var result = new ServiceResult();
            try
            {
                if (item == null)
                {
                    return result.Error("Error al crear el cliente", "Los datos del cliente son inválidos");
                }

                // Validaciones adicionales
                if (string.IsNullOrWhiteSpace(item.Nombre))
                {
                    return result.Error("Error al crear el cliente", "El nombre del cliente es requerido");
                }

                if (string.IsNullOrWhiteSpace(item.Identidad))
                {
                    return result.Error("Error al crear el cliente", "La identidad del cliente es requerida");
                }

                // Validar que la identidad sea única
                var clienteExistente = _clienteRepository.FindByIdentidad(item.Identidad);
                if (clienteExistente != null)
                {
                    return result.Error(
                        "Error al crear el cliente",
                        $"Ya existe un cliente con la identidad {item.Identidad}"
                    );
                }

                // Asignar fecha de creación
                item.CreatedAt = DateTime.Now;

                var requestStatus = _clienteRepository.Insert(item);

                if (requestStatus.CodeStatus == 0)
                {
                    return result.Error("Error al crear el cliente", requestStatus.MessageStatus);
                }

                return result.Ok(item, "Cliente creado exitosamente");
            }
            catch (Exception ex)
            {
                return result.Error("Error al crear el cliente", ex.Message);
            }
        }

        public ServiceResult ActualizarCliente(Cliente item)
        {
            var result = new ServiceResult();
            try
            {
                if (item == null)
                {
                    return result.Error("Error al actualizar el cliente", "Los datos del cliente son inválidos");
                }

                if (item.ClienteId <= 0)
                {
                    return result.Error("Error al actualizar el cliente", "El ID del cliente es inválido");
                }

                // Validaciones adicionales
                if (string.IsNullOrWhiteSpace(item.Nombre))
                {
                    return result.Error("Error al actualizar el cliente", "El nombre del cliente es requerido");
                }

                if (string.IsNullOrWhiteSpace(item.Identidad))
                {
                    return result.Error("Error al actualizar el cliente", "La identidad del cliente es requerida");
                }

                // Verificar que el cliente existe
                var clienteExiste = _clienteRepository.Find((int)item.ClienteId);
                if (clienteExiste == null)
                {
                    return result.Error("Cliente no encontrado", "No existe un cliente con el ID especificado");
                }

                // Validar que la identidad sea única (excepto para el mismo cliente)
                var clienteConMismaIdentidad = _clienteRepository.FindByIdentidad(item.Identidad);
                if (clienteConMismaIdentidad != null && clienteConMismaIdentidad.ClienteId != item.ClienteId)
                {
                    return result.Error(
                        "Error al actualizar el cliente",
                        $"Ya existe otro cliente con la identidad {item.Identidad}"
                    );
                }

                var requestStatus = _clienteRepository.Update(item);

                if (requestStatus.CodeStatus == 0)
                {
                    return result.Error("Error al actualizar el cliente", requestStatus.MessageStatus);
                }

                return result.Ok(item, "Cliente actualizado exitosamente");
            }
            catch (Exception ex)
            {
                return result.Error("Error al actualizar el cliente", ex.Message);
            }
        }

        public ServiceResult EliminarCliente(int? id)
        {
            var result = new ServiceResult();
            try
            {
                if (id == null || id <= 0)
                {
                    return result.Error("Error al eliminar el cliente", "El ID del cliente es inválido");
                }

                // Verificar que el cliente existe antes de eliminar
                var clienteExiste = _clienteRepository.Find(id);
                if (clienteExiste == null)
                {
                    return result.Error("Cliente no encontrado", "No existe un cliente con el ID especificado");
                }

                var requestStatus = _clienteRepository.Delete(id);

                if (requestStatus.CodeStatus == 0)
                {
                    return result.Error("Error al eliminar el cliente", requestStatus.MessageStatus);
                }

                return result.Ok(null, "Cliente eliminado exitosamente");
            }
            catch (Exception ex)
            {
                return result.Error("Error al eliminar el cliente", ex.Message);
            }
        }
        #endregion



        #region Orden

        
        /// Crear una orden con sus detalles (con transacción)
    
        public ServiceResult CrearOrden(long clienteId, List<(long ProductoId, int Cantidad)> detalles)
        {
            var result = new ServiceResult();

            // Validar cliente
            var cliente = _clienteRepository.Find((int)clienteId);
            if (cliente == null)
            {
                return result.Error("Error al crear la orden", "El cliente especificado no existe");
            }

            // Validar que haya detalles
            if (detalles == null || !detalles.Any())
            {
                return result.Error("Error al crear la orden", "Debe incluir al menos un producto en la orden");
            }

            using var connection = new SqlConnection(OrderManagementContext.ConnectionString);
            connection.Open();
            using var transaction = connection.BeginTransaction();

            try
            {
                // 1. Crear la orden inicial (con totales en 0)
                var orden = new Orden
                {
                    ClienteId = clienteId,
                    Impuesto = 0,
                    Subtotal = 0,
                    Total = 0,
                    FechaCreacion = DateTime.Now
                };

                var insertOrdenResult = _ordenRepository.Insert(orden, connection, transaction);
                if (insertOrdenResult.CodeStatus == 0)
                {
                    transaction.Rollback();
                    return result.Error("Error al crear la orden", insertOrdenResult.MessageStatus);
                }

                // 2. Procesar cada detalle
                var listaDetalles = new List<DetalleOrden>();
                decimal totalImpuesto = 0;
                decimal totalSubtotal = 0;
                decimal totalGeneral = 0;

                foreach (var det in detalles)
                {
                    // Validar que el producto exista
                    var producto = _productoRepository.Find((int)det.ProductoId);
                    if (producto == null)
                    {
                        transaction.Rollback();
                        return result.Error(
                            "Error al procesar la orden",
                            $"El producto con ID {det.ProductoId} no existe"
                        );
                    }

                    // Validar existencia suficiente
                    if (producto.Existencia < det.Cantidad)
                    {
                        transaction.Rollback();
                        return result.Error(
                            "Error al procesar la orden",
                            $"El producto '{producto.Nombre}' no tiene suficientes existencias. Disponible: {producto.Existencia}, Solicitado: {det.Cantidad}"
                        );
                    }

                    // Calcular montos
                    decimal subtotal = det.Cantidad * producto.Precio;
                    decimal impuesto = subtotal * 0.15m; // 15% de impuesto
                    decimal total = subtotal + impuesto;

                    // Crear detalle
                    var detalle = new DetalleOrden
                    {
                        OrdenId = orden.OrdenId,
                        ProductoId = det.ProductoId,
                        Cantidad = det.Cantidad,
                        Subtotal = subtotal,
                        Impuesto = impuesto,
                        Total = total
                    };

                    var insertDetalleResult = _detalleOrdenRepository.Insert(detalle, connection, transaction);
                    if (insertDetalleResult.CodeStatus == 0)
                    {
                        transaction.Rollback();
                        return result.Error("Error al crear detalle de orden", insertDetalleResult.MessageStatus);
                    }

                    // Actualizar existencia del producto
                    var updateExistenciaResult = _detalleOrdenRepository.ActualizarExistenciaProducto(
                        det.ProductoId,
                        det.Cantidad,
                        connection,
                        transaction
                    );

                    if (updateExistenciaResult.CodeStatus == 0)
                    {
                        transaction.Rollback();
                        return result.Error("Error al actualizar existencia", updateExistenciaResult.MessageStatus);
                    }

                    listaDetalles.Add(detalle);

                   
                    totalSubtotal += subtotal;
                    totalImpuesto += impuesto;
                    totalGeneral += total;
                }

                orden.Subtotal = totalSubtotal;
                orden.Impuesto = totalImpuesto;
                orden.Total = totalGeneral;

                var updateOrdenResult = _ordenRepository.Update(orden, connection, transaction);
                if (updateOrdenResult.CodeStatus == 0)
                {
                    transaction.Rollback();
                    return result.Error("Error al actualizar totales de la orden", updateOrdenResult.MessageStatus);
                }

               
                transaction.Commit();

                orden.DetalleOrden = listaDetalles;

                return result.Ok(orden, "Orden creada exitosamente");
            }
            catch (Exception ex)
            {
                transaction.Rollback();
                return result.Error("Error al procesar la orden", ex.Message);
            }
        }

        
        /// Listar todas las órdenes
    
        public ServiceResult ListarOrdenes()
        {
            var result = new ServiceResult();
            try
            {
                var list = _ordenRepository.List();

                if (list == null || !list.Any())
                {
                    return result.Ok(new List<Orden>(), "No hay órdenes registradas");
                }

                return result.Ok(list);
            }
            catch (Exception ex)
            {
                return result.Error("Error al listar las órdenes", ex.Message);
            }
        }

        
        /// Buscar orden por ID con sus detalles
    
        public ServiceResult BuscarOrden(int? id)
        {
            var result = new ServiceResult();
            try
            {
                if (id == null || id <= 0)
                {
                    return result.Error("ID inválido", "El ID de la orden debe ser mayor a 0");
                }

                var orden = _ordenRepository.GetOrdenConDetalles(id);

                if (orden == null)
                {
                    return result.Error("Orden no encontrada", "No existe una orden con el ID especificado");
                }

                return result.Ok(orden);
            }
            catch (Exception ex)
            {
                return result.Error("Error al buscar la orden", ex.Message);
            }
        }

        #endregion
    }
}
   

