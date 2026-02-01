using Dapper;
using Microsoft.Data.SqlClient;
using OrderManagement.Entities.Entities;

namespace OrderManagement.DataAccess.Repositories
{
    public class ClienteRepository : iRepository<Cliente>
    {
      
        /// Listar todos los clientes
      
        public IEnumerable<Cliente> List()
        {
            var parameter = new DynamicParameters();

            try
            {
                using var db = new SqlConnection(OrderManagementContext.ConnectionString);
                var result = db.Query<Cliente>(
                    ScriptDataBase.Client_Listar,
                    parameter,
                    commandType: System.Data.CommandType.StoredProcedure
                );

                return result ?? new List<Cliente>();
            }
            catch (Exception)
            {
                return new List<Cliente>();
            }
        }

      
        /// Buscar cliente por ID
      
        public Cliente Find(int? id)
        {
            if (id == null || id <= 0)
            {
                return null;
            }

            var parameter = new DynamicParameters();
            parameter.Add("@ClienteId", id, System.Data.DbType.Int64, System.Data.ParameterDirection.Input);

            try
            {
                using var db = new SqlConnection(OrderManagementContext.ConnectionString);
                var result = db.QueryFirstOrDefault<Cliente>(
                    ScriptDataBase.Client_Buscar,
                    parameter,
                    commandType: System.Data.CommandType.StoredProcedure
                );

                return result;
            }
            catch (Exception)
            {
                return null;
            }
        }

      
        /// Buscar cliente por identidad (para validar duplicados)
      
        public Cliente FindByIdentidad(string identidad)
        {
            if (string.IsNullOrWhiteSpace(identidad))
            {
                return null;
            }

            var parameter = new DynamicParameters();
            parameter.Add("@Identidad", identidad, System.Data.DbType.String, System.Data.ParameterDirection.Input);

            try
            {
                using var db = new SqlConnection(OrderManagementContext.ConnectionString);
                var result = db.QueryFirstOrDefault<Cliente>(
                    ScriptDataBase.Client_BuscarPorIdentidad,
                    parameter,
                    commandType: System.Data.CommandType.StoredProcedure
                );

                return result;
            }
            catch (Exception)
            {
                return null;
            }
        }

      
        /// Insertar nuevo cliente
      
        public RequestStatus Insert(Cliente item)
        {
            if (item == null)
            {
                return new RequestStatus
                {
                    CodeStatus = 0,
                    MessageStatus = "Los datos del cliente son inválidos"
                };
            }

            var parameter = new DynamicParameters();
            parameter.Add("@Nombre", item.Nombre, System.Data.DbType.String, System.Data.ParameterDirection.Input);
            parameter.Add("@Identidad", item.Identidad, System.Data.DbType.String, System.Data.ParameterDirection.Input);
            parameter.Add("@CreatedAt", item.CreatedAt, System.Data.DbType.DateTime, System.Data.ParameterDirection.Input);

            try
            {
                using var db = new SqlConnection(OrderManagementContext.ConnectionString);
                var result = db.QueryFirstOrDefault<RequestStatus>(
                    ScriptDataBase.Client_Insertar,
                    parameter,
                    commandType: System.Data.CommandType.StoredProcedure
                );

                if (result == null)
                {
                    return new RequestStatus
                    {
                        CodeStatus = 0,
                        MessageStatus = "Error desconocido al crear el cliente"
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

      
        /// Actualizar cliente existente
      
        public RequestStatus Update(Cliente item)
        {
            if (item == null)
            {
                return new RequestStatus
                {
                    CodeStatus = 0,
                    MessageStatus = "Los datos del cliente son inválidos"
                };
            }

            var parameter = new DynamicParameters();
            parameter.Add("@ClienteId", item.ClienteId, System.Data.DbType.Int64, System.Data.ParameterDirection.Input);
            parameter.Add("@Nombre", item.Nombre, System.Data.DbType.String, System.Data.ParameterDirection.Input);
            parameter.Add("@Identidad", item.Identidad, System.Data.DbType.String, System.Data.ParameterDirection.Input);

            try
            {
                using var db = new SqlConnection(OrderManagementContext.ConnectionString);
                var result = db.QueryFirstOrDefault<RequestStatus>(
                    ScriptDataBase.Client_Actualizar,
                    parameter,
                    commandType: System.Data.CommandType.StoredProcedure
                );

                if (result == null)
                {
                    return new RequestStatus
                    {
                        CodeStatus = 0,
                        MessageStatus = "Error desconocido al actualizar el cliente"
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

      
        /// Eliminar cliente
      
        public RequestStatus Delete(int? id)
        {
            if (id == null || id <= 0)
            {
                return new RequestStatus
                {
                    CodeStatus = 0,
                    MessageStatus = "El ID del cliente es inválido"
                };
            }

            var parameter = new DynamicParameters();
            parameter.Add("@ClienteId", id, System.Data.DbType.Int64, System.Data.ParameterDirection.Input);

            try
            {
                using var db = new SqlConnection(OrderManagementContext.ConnectionString);
                var result = db.QueryFirstOrDefault<RequestStatus>(
                    ScriptDataBase.Client_Eliminar,
                    parameter,
                    commandType: System.Data.CommandType.StoredProcedure
                );

                if (result == null)
                {
                    return new RequestStatus
                    {
                        CodeStatus = 0,
                        MessageStatus = "Error desconocido al eliminar el cliente"
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
    }
}