namespace OrderManagement.DataAccess.Repositories
{
    public class ScriptDataBase
    {
        #region Productos
        public static string Produ_Listar = "SP_Productos_Listar";
        public static string Produ_Buscar = "SP_Productos_Buscar";
        public static string Produ_Insertar = "SP_Productos_Insertar";
        public static string Produ_Actualizar = "SP_Productos_Actualizar";
        public static string Produ_Eliminar = "SP_Productos_Eliminar";
        #endregion

        #region Clientes
        #region Clientes
        public static string Client_Listar = "SP_Clientes_Listar";
        public static string Client_Buscar = "SP_Clientes_Buscar";
        public static string Client_BuscarPorIdentidad = "SP_Clientes_BuscarPorIdentidad";
        public static string Client_Insertar = "SP_Clientes_Insertar";
        public static string Client_Actualizar = "SP_Clientes_Actualizar";
        public static string Client_Eliminar = "SP_Clientes_Eliminar";
        #endregion
        #endregion

        #region Ordenes
        public static string Orden_Insertar = "SP_Ordenes_Insertar";
        public static string Orden_Listar = "SP_Ordenes_Listar";
        public static string Orden_Buscar = "SP_Ordenes_Buscar";
        public static string Orden_Actualizar = "SP_Ordenes_Actualizar";
        public static string Producto_ActualizarExistencia = "SP_Productos_ActualizarExistencia";
        #endregion

        #region DetalleOrden
        public static string DetalleOrden_Insertar = "SP_DetalleOrden_Insertar";
        public static string DetalleOrden_PorOrden = "SP_DetalleOrden_PorOrden";
        #endregion
    }
}