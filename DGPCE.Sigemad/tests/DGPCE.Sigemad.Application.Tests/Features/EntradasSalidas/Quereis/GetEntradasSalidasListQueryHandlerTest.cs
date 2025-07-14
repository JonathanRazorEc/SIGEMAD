using DGPCE.Sigemad.Application.Contracts.Caching;
using DGPCE.Sigemad.Application.Contracts.Persistence;
using DGPCE.Sigemad.Application.Features.EntradasSalidas.Quereis.GetEntradaSalidaList;
using DGPCE.Sigemad.Domain.Modelos;
using FluentAssertions;
using Moq;


namespace DGPCE.Sigemad.Application.Tests.Features.EntradasSalidas.Quereis;
public class GetEntradasSalidasListQueryHandlerTest
{
    private readonly Mock<IUnitOfWork> _unitOfWorkMock;
    private readonly GetEntradaSalidaListQueryHandler _handler;
    private readonly Mock<ISIGEMemoryCache> _memoryCacheMock;

    public GetEntradasSalidasListQueryHandlerTest()
    {
        _unitOfWorkMock = new Mock<IUnitOfWork>();
        _memoryCacheMock = new Mock<ISIGEMemoryCache>();
        _handler = new GetEntradaSalidaListQueryHandler(_unitOfWorkMock.Object, _memoryCacheMock.Object);
    }

    [Fact]
    public async Task Handle_ValidQuery_ShouldReturnListEntradasSalidas()
    {
        // Arrange
        var request = new GetEntradaSalidaListQuery();

        var entradasSalidas = new List<EntradaSalida>
        {
            new EntradaSalida { Id = 1, Descripcion = "Entrada" },
            new EntradaSalida { Id = 2, Descripcion = "Interior" },
            new EntradaSalida { Id = 3, Descripcion = "Salida" }
        };

        _unitOfWorkMock.Setup(m => m.Repository<EntradaSalida>().GetAllAsync()).ReturnsAsync(entradasSalidas);

        // Act
        var result = await _handler.Handle(request, CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result.Should().HaveCount(3);

        result[0].Id.Should().Be(1);
        result[0].Descripcion.Should().Be("Entrada");
        result[1].Id.Should().Be(2);
        result[1].Descripcion.Should().Be("Interior");
        result[2].Id.Should().Be(3);
        result[2].Descripcion.Should().Be("Salida");
    }


    [Fact]
    public async Task Handle_WithNoEntradasSalidas_ShouldReturnEmptyList()
    {
        // Arrange
        var query = new GetEntradaSalidaListQuery();
        var entradasSalidas = new List<EntradaSalida>();

        _unitOfWorkMock.Setup(m => m.Repository<EntradaSalida>().GetAllAsync()).ReturnsAsync(entradasSalidas);

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        Assert.Empty(result);
    }

    [Fact]
    public async Task Handle_WithNullEntradasSalidas_ShouldReturnEmptyList()
    {
        // Arrange
        var request = new GetEntradaSalidaListQuery();

        _unitOfWorkMock.Setup(m => m.Repository<EntradaSalida>().GetAllAsync())
            .ReturnsAsync((IReadOnlyList<EntradaSalida>)null);

        // Act
        var result = await _handler.Handle(request, CancellationToken.None);

        // Assert
        result.Should().BeNull();
    }

}
