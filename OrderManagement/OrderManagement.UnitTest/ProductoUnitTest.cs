using Moq;
using Xunit;
using OrderManagement.BusinessLogic.Services;
using OrderManagement.DataAccess.Repositories;
using OrderManagement.Entities.Entities;

namespace OrderManagement.UnitTest
{
    public class ProductoUnitTest
    {
        private readonly Mock<ProductoRepository> _productoRepository;
        private readonly OrderManagementServices _services;

        public ProductoUnitTest()
        {
            _productoRepository = new Mock<ProductoRepository>();

            _services = new OrderManagementServices(
                _productoRepository.Object,  // ProductoRepository
                null,                        // ClienteRepository
                null,                        // OrdenRepository
                null                         // DetalleOrdenRepository
            );
        }



        [Fact]
        public void InsertarProducto_Exitoso()
        {
            var producto = new Producto
            {
                ProductoId = 0,
                Nombre = "Producto de Prueba",
                Descripcion = "Descripcion de prueba",
                Precio = 15.00m,
                Existencia = 100
            };

            // Setup retorna RequestStatus con CodeStatus 1 (éxito)
            _productoRepository
                .Setup(repo => repo.Insert(It.IsAny<Producto>()))
                .Returns(new RequestStatus { CodeStatus = 1, MessageStatus = "Producto creado exitosamente" });

            var result = _services.InsertarProducto(producto);

            Assert.True(result.Success);
            Assert.Equal("Producto creado exitosamente", result.Message);
            Assert.NotNull(result.Data);

            // Verifica que se llamó al repositorio exactamente una vez
            _productoRepository.Verify(repo => repo.Insert(It.IsAny<Producto>()), Times.Once);
        }

  

        [Fact]
        public void EliminarProducto_NoExiste_RetornaError()
        {
            _productoRepository
                .Setup(repo => repo.Find(999))
                .Returns((Producto)null);

            var result = _services.EliminarProducto(999);

            Assert.False(result.Success);
            Assert.Equal("Producto no encontrado", result.Message);
            _productoRepository.Verify(repo => repo.Delete(It.IsAny<int?>()), Times.Never);
        }

        [Fact]
        public void EliminarProducto_IdNulo_RetornaError()
        {
            var result = _services.EliminarProducto(null);

            Assert.False(result.Success);
            Assert.Equal("El ID del producto es inválido", result.Errors.First());
            _productoRepository.Verify(repo => repo.Delete(It.IsAny<int?>()), Times.Never);
        }

   

       
    }
}