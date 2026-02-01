namespace OrderManagement.API.DTOs
{
    /// <summary>
    /// Respuesta estándar de la API
    /// </summary>
    /// <typeparam name="T">Tipo de dato retornado</typeparam>
    public class ApiResponse<T>
    {
        public bool Success { get; set; }
        public string Message { get; set; } = string.Empty;
        public List<string> Errors { get; set; } = new List<string>();
        public T? Data { get; set; }

        /// <summary>
        /// Constructor para respuesta exitosa
        /// </summary>
        public static ApiResponse<T> SuccessResponse(T data, string message = "")
        {
            return new ApiResponse<T>
            {
                Success = true,
                Message = message,
                Errors = new List<string>(),
                Data = data
            };
        }

        /// <summary>
        /// Constructor para respuesta de error
        /// </summary>
        public static ApiResponse<T> ErrorResponse(string message, List<string>? errors = null)
        {
            return new ApiResponse<T>
            {
                Success = false,
                Message = message,
                Errors = errors ?? new List<string>(),
                Data = default(T)
            };
        }

        /// <summary>
        /// Constructor para respuesta de error con un solo mensaje
        /// </summary>
        public static ApiResponse<T> ErrorResponse(string message, string error)
        {
            return new ApiResponse<T>
            {
                Success = false,
                Message = message,
                Errors = new List<string> { error },
                Data = default(T)
            };
        }
    }
}