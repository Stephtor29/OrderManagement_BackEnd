using Dapper;
using Microsoft.Data.SqlClient;
using OrderManagement.Entities.Entities;
using System.Data;

namespace OrderManagement.DataAccess.Repositories
{
    public class DetalleOrdenRepository
    {
       
        /// Insertar detalle de orden con conexión y transacción existente
       
        public RequestStatus Insert(DetalleOrden item, SqlConnection connection, SqlTransaction transaction)
        {
            if (item == null)
            {
                return new RequestStatus
                {
                    CodeStatus = 0,
                    MessageStatus = "Los datos del detalle son inválidos"
                };
            }

            var parameter = new DynamicParameters();
            parameter.Add("@OrdenId", item.OrdenId, DbType.Int64, ParameterDirection.Input);
            parameter.Add("@ProductoId", item.ProductoId, DbType.Int64, ParameterDirection.Input);
            parameter.Add("@Cantidad", item.Cantidad, DbType.Int32, ParameterDirection.Input);
            parameter.Add("@Impuesto", item.Impuesto, DbType.Decimal, ParameterDirection.Input);
            parameter.Add("@Subtotal", item.Subtotal, DbType.Decimal, ParameterDirection.Input);
            parameter.Add("@Total", item.Total, DbType.Decimal, ParameterDirection.Input);
            parameter.Add("@DetalleOrdenId", dbType: DbType.Int64, direction: ParameterDirection.Output);

            try
            {
                var result = connection.QueryFirstOrDefault<RequestStatus>(
                    ScriptDataBase.DetalleOrden_Insertar,
                    parameter,
                    transaction: transaction,
                    commandType: CommandType.StoredProcedure
                );

                if (result == null)
                {
                    return new RequestStatus
                    {
                        CodeStatus = 0,
                        MessageStatus = "Error desconocido al crear el detalle de orden"
                    };
                }

                // Obtener el ID generado
                var detalleId = parameter.Get<long>("@DetalleOrdenId");
                item.DetalleOrdenId = detalleId;

                return result;
            }
            catch (Exception ex)
            {
                return new RequestStatus
                {
                    CodeStatus = 0,
                    MessageStatus = $"Error inesperado: {ex.Message}"
                };
            }
        }

       
        /// Obtener todos los detalles de una orden
       
        public IEnumerable<DetalleOrden> GetByOrdenId(long ordenId, SqlConnection connection, SqlTransaction transaction)
        {
            var parameter = new DynamicParameters();
            parameter.Add("@OrdenId", ordenId, DbType.Int64, ParameterDirection.Input);

            try
            {
                var result = connection.Query<DetalleOrden>(
                    ScriptDataBase.DetalleOrden_PorOrden,
                    parameter,
                    transaction: transaction,
                    commandType: CommandType.StoredProcedure
                );

                return result ?? new List<DetalleOrden>();
            }
            catch (Exception)
            {
                return new List<DetalleOrden>();
            }
        }

       
        /// Obtener todos los detalles de una orden (sin transacción)
       
        public IEnumerable<DetalleOrden> GetByOrdenId(long ordenId)
        {
            var parameter = new DynamicParameters();
            parameter.Add("@OrdenId", ordenId, DbType.Int64, ParameterDirection.Input);

            try
            {
                using var db = new SqlConnection(OrderManagementContext.ConnectionString);
                var result = db.Query<DetalleOrden>(
                    ScriptDataBase.DetalleOrden_PorOrden,
                    parameter,
                    commandType: CommandType.StoredProcedure
                );

                return result ?? new List<DetalleOrden>();
            }
            catch (Exception)
            {
                return new List<DetalleOrden>();
            }
        }

       
        /// Actualizar existencia de producto con conexión y transacción existente - USA STORED PROCEDURE
       
        public RequestStatus ActualizarExistenciaProducto(long productoId, int cantidad, SqlConnection connection, SqlTransaction transaction)
        {
            try
            {
                var parameter = new DynamicParameters();
                parameter.Add("@ProductoId", productoId, DbType.Int64, ParameterDirection.Input);
                parameter.Add("@Cantidad", cantidad, DbType.Int32, ParameterDirection.Input);

                var result = connection.QueryFirstOrDefault<RequestStatus>(
                    ScriptDataBase.Producto_ActualizarExistencia,
                    parameter,
                    transaction: transaction,
                    commandType: CommandType.StoredProcedure
                );

                if (result == null)
                {
                    return new RequestStatus
                    {
                        CodeStatus = 0,
                        MessageStatus = "Error desconocido al actualizar la existencia"
                    };
                }

                return result;
            }
            catch (Exception ex)
            {
                return new RequestStatus
                {
                    CodeStatus = 0,
                    MessageStatus = $"Error al actualizar existencia: {ex.Message}"
                };
            }
        }
    }
}