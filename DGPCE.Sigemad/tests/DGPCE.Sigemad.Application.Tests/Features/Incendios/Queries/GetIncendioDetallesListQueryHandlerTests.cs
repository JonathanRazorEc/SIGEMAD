//using DGPCE.Sigemad.Application.Contracts.Persistence;
//using DGPCE.Sigemad.Application.Features.Incendios.Queries.GetIncendioDetalles;
//using DGPCE.Sigemad.Application.Features.Incendios.Queries.GetIncendioDetallesList;
//using DGPCE.Sigemad.Application.Specifications.Evoluciones;
//using DGPCE.Sigemad.Domain.Modelos;
//using FluentAssertions;
//using Microsoft.Extensions.Logging;
//using Moq;

//namespace DGPCE.Sigemad.Application.Tests.Features.Incendios.Queries;
//public class GetIncendioDetallesListQueryHandlerTests
//{
//    private readonly Mock<IUnitOfWork> _unitOfWorkMock;
//    private readonly Mock<ILogger<GetIncendioDetallesListQueryHandler>> _loggerMock;
//    private readonly GetIncendioDetallesListQueryHandler _handler;

//    public GetIncendioDetallesListQueryHandlerTests()
//    {
//        _unitOfWorkMock = new Mock<IUnitOfWork>();
//        _loggerMock = new Mock<ILogger<GetIncendioDetallesListQueryHandler>>();
//        _handler = new GetIncendioDetallesListQueryHandler(_loggerMock.Object, _unitOfWorkMock.Object);
//    }

//    [Fact]
//    public async Task Handle_WithValidIdIncendio_ShouldReturnDetallesList()
//    {
//        // Arrange
//        var idIncendio = 1;
//        var request = new GetIncendioDetallesListQuery(idIncendio);
//        var detallesEvolucion = new List<Evolucion>
//        {
//            new Evolucion
//            {
//                FechaCreacion = DateTime.UtcNow,
//                EntradaSalida = new EntradaSalida { Descripcion = "Entrada" },
//                Tecnico = new ApplicationUser { Nombre = "Tecnico 1" }
//            },
//            new Evolucion
//            {
//                FechaCreacion = DateTime.UtcNow.AddMinutes(-10),
//                EntradaSalida = new EntradaSalida { Descripcion = "Salida" },
//                Tecnico = new ApplicationUser { Nombre = "Tecnico 2" }
//            }
//        };

//        _unitOfWorkMock.Setup(u => u.Repository<Evolucion>().GetAllWithSpec(It.IsAny<DetalleEvolucionByIdSpecification>()))
//            .ReturnsAsync(detallesEvolucion);

//        // Act
//        var result = await _handler.Handle(request, CancellationToken.None);

//        // Assert
//        result.Should().NotBeNull();
//        result.Should().HaveCount(2);
//        result[0].Registro.Should().Be("Entrada");
//        result[0].Tecnico.Should().Be("Tecnico 1");
//        result[1].Registro.Should().Be("Salida");
//        result[1].Tecnico.Should().Be("Tecnico 2");
//    }

//    [Fact]
//    public async Task Handle_WithNoDetalles_ShouldReturnEmptyList()
//    {
//        // Arrange
//        var idIncendio = 1;
//        var request = new GetIncendioDetallesListQuery(idIncendio);
//        var detallesEvolucion = new List<Evolucion>();

//        _unitOfWorkMock.Setup(u => u.Repository<Evolucion>().GetAllWithSpec(It.IsAny<DetalleEvolucionByIdSpecification>()))
//            .ReturnsAsync(detallesEvolucion);

//        // Act
//        var result = await _handler.Handle(request, CancellationToken.None);

//        // Assert
//        result.Should().NotBeNull();
//        result.Should().BeEmpty();
//    }

//    [Fact]
//    public async Task Handle_WithNullDetalles_ShouldReturnEmptyList()
//    {
//        // Arrange
//        var idIncendio = 1;
//        var request = new GetIncendioDetallesListQuery(idIncendio);

//        _unitOfWorkMock.Setup(u => u.Repository<Evolucion>().GetAllWithSpec(It.IsAny<DetalleEvolucionByIdSpecification>()))
//            .ReturnsAsync((IReadOnlyList<Evolucion>)null);

//        // Act
//        var result = await _handler.Handle(request, CancellationToken.None);

//        // Assert
//        result.Should().NotBeNull();
//        result.Should().BeEmpty();
//    }

//}
