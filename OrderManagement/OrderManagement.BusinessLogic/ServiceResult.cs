namespace OrderManagement.BusinessLogic.Services
{
    /// Resultado de operaciones en la capa de servicio
    public class ServiceResult
    {
        public bool Success { get; set; }
        public string Message { get; set; } = string.Empty;
        public List<string> Errors { get; set; } = new List<string>();
        public object? Data { get; set; }

        public ServiceResult() { }

        /// Retorna resultado exitoso
        public ServiceResult Ok(object? data, string message = "")
        {
            Success = true;
            Message = message;
            Data = data;
            Errors = new List<string>();
            return this;
        }

        /// Retorna resultado de error
        public ServiceResult Error(string message, List<string>? errors = null)
        {
            Success = false;
            Message = message;
            Data = null;
            Errors = errors ?? new List<string> { message };
            return this;
        }

        /// Retorna resultado de error con lista de errores
     
        public ServiceResult Error(string message, params string[] errors)
        {
            Success = false;
            Message = message;
            Data = null;
            Errors = errors.ToList();
            return this;
        }
    }
}