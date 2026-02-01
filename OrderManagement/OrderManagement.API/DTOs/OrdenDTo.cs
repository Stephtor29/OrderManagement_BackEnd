using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace OrderManagement.API.DTOs
{
    
    /// DTO para crear una nueva orden
    
    public class CrearOrdenDto
    {
        public long OrdenId { get; set; } = 0;

        [Required(ErrorMessage = "El ClienteId es requerido")]
        [Range(1, long.MaxValue, ErrorMessage = "El ClienteId debe ser mayor a 0")]
        public long ClienteId { get; set; }

        [Required(ErrorMessage = "Debe incluir al menos un detalle")]
        [MinLength(1, ErrorMessage = "Debe incluir al menos un producto en la orden")]
        public List<CrearDetalleOrdenDto> Detalle { get; set; } = new List<CrearDetalleOrdenDto>();
    }

    
    /// DTO para crear un detalle de orden
    
    public class CrearDetalleOrdenDto
    {
        [Required(ErrorMessage = "El ProductoId es requerido")]
        [Range(1, long.MaxValue, ErrorMessage = "El ProductoId debe ser mayor a 0")]
        public long ProductoId { get; set; }

        [Required(ErrorMessage = "La cantidad es requerida")]
        [Range(1, int.MaxValue, ErrorMessage = "La cantidad debe ser mayor a 0")]
        public int Cantidad { get; set; }
    }


    /// DTO para retornar una orden completa

    public class OrdenDto
    {
        public long OrdenId { get; set; }
        public string ClienteNombre { get; set; }

        public decimal Impuesto { get; set; }
        public decimal Subtotal { get; set; }
        public decimal Total { get; set; }
        public DateTime FechaCreacion { get; set; }

        public List<DetalleOrdenDto> Detalles { get; set; }
    }


    /// DTO para retornar un detalle de orden

    public class DetalleOrdenDto
    {
        public long DetalleOrdenId { get; set; }
        public long ProductoId { get; set; }
        public int Cantidad { get; set; }
        public decimal Impuesto { get; set; }
        public decimal Subtotal { get; set; }
        public decimal Total { get; set; }
    }
}