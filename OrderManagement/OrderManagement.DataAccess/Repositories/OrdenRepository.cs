using Dapper;
using Microsoft.Data.SqlClient;
using OrderManagement.Entities.Entities;
using System.Data;

namespace OrderManagement.DataAccess.Repositories
{
    public class OrdenRepository : iRepository<Orden>
    {
       
        /// Listar todas las órdenes
       
        public IEnumerable<Orden> List()
        {
            var parameter = new DynamicParameters();

            try
            {
                using var db = new SqlConnection(OrderManagementContext.ConnectionString);
                var result = db.Query<Orden>(
                    ScriptDataBase.Orden_Listar,
                    parameter,
                    commandType: CommandType.StoredProcedure
                );

                return result ?? new List<Orden>();
            }
            catch (Exception)
            {
                return new List<Orden>();
            }
        }

       
        /// Buscar orden por ID
       
        public Orden Find(int? id)
        {
            if (id == null || id <= 0)
            {
                return null;
            }

            var parameter = new DynamicParameters();
            parameter.Add("@OrdenId", id, DbType.Int64, ParameterDirection.Input);

            try
            {
                using var db = new SqlConnection(OrderManagementContext.ConnectionString);
                var result = db.QueryFirstOrDefault<Orden>(
                    ScriptDataBase.Orden_Buscar,
                    parameter,
                    commandType: CommandType.StoredProcedure
                );

                return result;
            }
            catch (Exception)
            {
                return null;
            }
        }

       
        /// Insertar nueva orden
       
        public RequestStatus Insert(Orden item)
        {
            if (item == null)
            {
                return new RequestStatus
                {
                    CodeStatus = 0,
                    MessageStatus = "Los datos de la orden son inválidos"
                };
            }

            var parameter = new DynamicParameters();
            parameter.Add("@ClienteId", item.ClienteId, DbType.Int64, ParameterDirection.Input);
            parameter.Add("@Impuesto", item.Impuesto, DbType.Decimal, ParameterDirection.Input);
            parameter.Add("@Subtotal", item.Subtotal, DbType.Decimal, ParameterDirection.Input);
            parameter.Add("@Total", item.Total, DbType.Decimal, ParameterDirection.Input);
            parameter.Add("@FechaCreacion", item.FechaCreacion, DbType.DateTime, ParameterDirection.Input);
            parameter.Add("@OrdenId", dbType: DbType.Int64, direction: ParameterDirection.Output);

            try
            {
                using var db = new SqlConnection(OrderManagementContext.ConnectionString);
                var result = db.QueryFirstOrDefault<RequestStatus>(
                    ScriptDataBase.Orden_Insertar,
                    parameter,
                    commandType: CommandType.StoredProcedure
                );

                if (result == null)
                {
                    return new RequestStatus
                    {
                        CodeStatus = 0,
                        MessageStatus = "Error desconocido al crear la orden"
                    };
                }

                // Obtener el ID generado
                var ordenId = parameter.Get<long>("@OrdenId");
                item.OrdenId = ordenId;

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

       
        /// Insertar nueva orden con conexión y transacción existente
       
        public RequestStatus Insert(Orden item, SqlConnection connection, SqlTransaction transaction)
        {
            if (item == null)
            {
                return new RequestStatus
                {
                    CodeStatus = 0,
                    MessageStatus = "Los datos de la orden son inválidos"
                };
            }

            var parameter = new DynamicParameters();
            parameter.Add("@ClienteId", item.ClienteId, DbType.Int64, ParameterDirection.Input);
            parameter.Add("@Impuesto", item.Impuesto, DbType.Decimal, ParameterDirection.Input);
            parameter.Add("@Subtotal", item.Subtotal, DbType.Decimal, ParameterDirection.Input);
            parameter.Add("@Total", item.Total, DbType.Decimal, ParameterDirection.Input);
            parameter.Add("@FechaCreacion", item.FechaCreacion, DbType.DateTime, ParameterDirection.Input);
            parameter.Add("@OrdenId", dbType: DbType.Int64, direction: ParameterDirection.Output);

            try
            {
                var result = connection.QueryFirstOrDefault<RequestStatus>(
                    ScriptDataBase.Orden_Insertar,
                    parameter,
                    transaction: transaction,
                    commandType: CommandType.StoredProcedure
                );

                if (result == null)
                {
                    return new RequestStatus
                    {
                        CodeStatus = 0,
                        MessageStatus = "Error desconocido al crear la orden"
                    };
                }

                // Obtener el ID generado
                var ordenId = parameter.Get<long>("@OrdenId");
                item.OrdenId = ordenId;

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

       
        /// Actualizar orden existente - USA STORED PROCEDURE
       
        public RequestStatus Update(Orden item)
        {
            if (item == null)
            {
                return new RequestStatus
                {
                    CodeStatus = 0,
                    MessageStatus = "Los datos de la orden son inválidos"
                };
            }

            var parameter = new DynamicParameters();
            parameter.Add("@OrdenId", item.OrdenId, DbType.Int64, ParameterDirection.Input);
            parameter.Add("@Impuesto", item.Impuesto, DbType.Decimal, ParameterDirection.Input);
            parameter.Add("@Subtotal", item.Subtotal, DbType.Decimal, ParameterDirection.Input);
            parameter.Add("@Total", item.Total, DbType.Decimal, ParameterDirection.Input);

            try
            {
                using var db = new SqlConnection(OrderManagementContext.ConnectionString);
                var result = db.QueryFirstOrDefault<RequestStatus>(
                    ScriptDataBase.Orden_Actualizar,
                    parameter,
                    commandType: CommandType.StoredProcedure
                );

                if (result == null)
                {
                    return new RequestStatus
                    {
                        CodeStatus = 0,
                        MessageStatus = "Error desconocido al actualizar la orden"
                    };
                }

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

       
        /// Actualizar orden con conexión y transacción existente - USA STORED PROCEDURE
       
        public RequestStatus Update(Orden item, SqlConnection connection, SqlTransaction transaction)
        {
            if (item == null)
            {
                return new RequestStatus
                {
                    CodeStatus = 0,
                    MessageStatus = "Los datos de la orden son inválidos"
                };
            }

            var parameter = new DynamicParameters();
            parameter.Add("@OrdenId", item.OrdenId, DbType.Int64, ParameterDirection.Input);
            parameter.Add("@Impuesto", item.Impuesto, DbType.Decimal, ParameterDirection.Input);
            parameter.Add("@Subtotal", item.Subtotal, DbType.Decimal, ParameterDirection.Input);
            parameter.Add("@Total", item.Total, DbType.Decimal, ParameterDirection.Input);

            try
            {
                var result = connection.QueryFirstOrDefault<RequestStatus>(
                    ScriptDataBase.Orden_Actualizar,
                    parameter,
                    transaction: transaction,
                    commandType: CommandType.StoredProcedure
                );

                if (result == null)
                {
                    return new RequestStatus
                    {
                        CodeStatus = 0,
                        MessageStatus = "Error desconocido al actualizar la orden"
                    };
                }

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

       
        /// Eliminar orden (no implementado - las órdenes no se deben eliminar)
       
        public RequestStatus Delete(int? id)
        {
            return new RequestStatus
            {
                CodeStatus = 0,
                MessageStatus = "No se permite eliminar órdenes"
            };
        }


        public OrdenDto GetOrdenConDetalles(int? id)
        {
            if (id == null || id <= 0)
            {
                return null;
            }

            try
            {
                using var db = new SqlConnection(OrderManagementContext.ConnectionString);

                var parameter = new DynamicParameters();
                parameter.Add("@OrdenId", id, DbType.Int64, ParameterDirection.Input);

                OrdenDto ordenResult = null;

                db.Query<OrdenDto, DetalleOrdenDto, OrdenDto>(
                    ScriptDataBase.Orden_Buscar,
                    (orden, detalle) =>
                    {
                        ordenResult ??= orden;
                        ordenResult.Detalles ??= new List<DetalleOrdenDto>();

                        if (detalle?.DetalleOrdenId != 0)
                        {
                            ordenResult.Detalles.Add(detalle);
                        }

                        return ordenResult;
                    },
                    parameter,
                    splitOn: "DetalleOrdenId",
                    commandType: CommandType.StoredProcedure
                );

                return ordenResult;
            }
            catch (Exception)
            {
                return null;
            }
        }
    }
}