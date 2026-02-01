using System.ComponentModel.DataAnnotations;

namespace OrderManagement.API.DTOs
{
    public class ClienteDto
    {
        public long ClienteId { get; set; }

        [Required(ErrorMessage = "El nombre es requerido")]
        [StringLength(100, MinimumLength = 3, ErrorMessage = "El nombre debe tener entre 3 y 100 caracteres")]
        public string Nombre { get; set; }

        [Required(ErrorMessage = "La identidad es requerida")]
        [StringLength(50, ErrorMessage = "La identidad no puede exceder 50 caracteres")]
        [RegularExpression(@"^\d{4}-\d{4}-\d{5}$", ErrorMessage = "La identidad debe tener el formato 0801-1990-12345")]
        public string Identidad { get; set; }
    }
}