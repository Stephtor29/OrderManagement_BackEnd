using Dapper;
using Microsoft.Data.SqlClient;
using OrderManagement.Entities.Entities;

namespace OrderManagement.DataAccess.Repositories
{
    public class ProductoRepository : iRepository<Producto>
    {
      
        /// Listar todos los productos
        
        public IEnumerable<Producto> List()
        {
            var parameter = new DynamicParameters();

            try
            {
                using var db = new SqlConnection(OrderManagementContext.ConnectionString);
                var result = db.Query<Producto>(
                    ScriptDataBase.Produ_Listar,
                    parameter,
                    commandType: System.Data.CommandType.StoredProcedure
                );

                return result ?? new List<Producto>();
            }
            catch (Exception)
            {
                return new List<Producto>();
            }
        }

        /// Buscar producto por ID
   
        public Producto Find(int? id)
        {
            if (id == null || id <= 0)
            {
                return null;
            }

            var parameter = new DynamicParameters();
            parameter.Add("@ProductoId", id, System.Data.DbType.Int32, System.Data.ParameterDirection.Input);

            try
            {
                using var db = new SqlConnection(OrderManagementContext.ConnectionString);
                var result = db.QueryFirstOrDefault<Producto>(
                    ScriptDataBase.Produ_Buscar,
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

      
        /// Insertar nuevo producto
        
        public RequestStatus Insert(Producto item)
        {
            if (item == null)
            {
                return new RequestStatus
                {
                    CodeStatus = 0,
                    MessageStatus = "Los datos del producto son inválidos"
                };
            }

            var parameter = new DynamicParameters();
            parameter.Add("@Nombre", item.Nombre, System.Data.DbType.String, System.Data.ParameterDirection.Input);
            parameter.Add("@Descripcion", item.Descripcion, System.Data.DbType.String, System.Data.ParameterDirection.Input);
            parameter.Add("@Precio", item.Precio, System.Data.DbType.Decimal, System.Data.ParameterDirection.Input);
            parameter.Add("@Existencia", item.Existencia, System.Data.DbType.Int32, System.Data.ParameterDirection.Input);
            parameter.Add("@CreatedAt", item.CreatedAt, System.Data.DbType.DateTime, System.Data.ParameterDirection.Input);

            try
            {
                using var db = new SqlConnection(OrderManagementContext.ConnectionString);
                var result = db.QueryFirstOrDefault<RequestStatus>(
                    ScriptDataBase.Produ_Insertar,
                    parameter,
                    commandType: System.Data.CommandType.StoredProcedure
                );

                if (result == null)
                {
                    return new RequestStatus
                    {
                        CodeStatus = 0,
                        MessageStatus = "Error desconocido al crear el producto"
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

      
        /// Actualizar producto existente
        
        public RequestStatus Update(Producto item)
        {
            if (item == null)
            {
                return new RequestStatus
                {
                    CodeStatus = 0,
                    MessageStatus = "Los datos del producto son inválidos"
                };
            }

            var parameter = new DynamicParameters();
            parameter.Add("@ProductoId", item.ProductoId, System.Data.DbType.Int64, System.Data.ParameterDirection.Input);
            parameter.Add("@Nombre", item.Nombre, System.Data.DbType.String, System.Data.ParameterDirection.Input);
            parameter.Add("@Descripcion", item.Descripcion, System.Data.DbType.String, System.Data.ParameterDirection.Input);
            parameter.Add("@Precio", item.Precio, System.Data.DbType.Decimal, System.Data.ParameterDirection.Input);
            parameter.Add("@Existencia", item.Existencia, System.Data.DbType.Int32, System.Data.ParameterDirection.Input);

            try
            {
                using var db = new SqlConnection(OrderManagementContext.ConnectionString);
                var result = db.QueryFirstOrDefault<RequestStatus>(
                    ScriptDataBase.Produ_Actualizar,
                    parameter,
                    commandType: System.Data.CommandType.StoredProcedure
                );

                if (result == null)
                {
                    return new RequestStatus
                    {
                        CodeStatus = 0,
                        MessageStatus = "Error desconocido al actualizar el producto"
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

      
        /// Eliminar producto
        
        public RequestStatus Delete(int? id)
        {
            if (id == null || id <= 0)
            {
                return new RequestStatus
                {
                    CodeStatus = 0,
                    MessageStatus = "El ID del producto es inválido"
                };
            }

            var parameter = new DynamicParameters();
            parameter.Add("@ProductoId", id, System.Data.DbType.Int32, System.Data.ParameterDirection.Input);

            try
            {
                using var db = new SqlConnection(OrderManagementContext.ConnectionString);
                var result = db.QueryFirstOrDefault<RequestStatus>(
                    ScriptDataBase.Produ_Eliminar,
                    parameter,
                    commandType: System.Data.CommandType.StoredProcedure
                );

                if (result == null)
                {
                    return new RequestStatus
                    {
                        CodeStatus = 0,
                        MessageStatus = "Error desconocido al eliminar el producto"
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