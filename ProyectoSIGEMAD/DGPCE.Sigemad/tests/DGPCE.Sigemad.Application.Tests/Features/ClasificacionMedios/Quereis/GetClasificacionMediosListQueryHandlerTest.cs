using DGPCE.Sigemad.Application.Contracts.Caching;
using DGPCE.Sigemad.Application.Contracts.Persistence;
using DGPCE.Sigemad.Application.Features.ClasificacionMedios.Quereis.GetClasificacionMediosList;
using DGPCE.Sigemad.Domain.Modelos;
using FluentAssertions;
using Moq;


namespace DGPCE.Sigemad.Application.Tests.Features.ClasificacionesMedios.Quereis;
public class GetClasificacionMediosListQueryHandlerTest
{
    private readonly Mock<IUnitOfWork> _unitOfWorkMock;
    private readonly GetClasificacionMediosListQueryHandler _handler;
    private readonly Mock<ISIGEMemoryCache> _memoryCacheMock;

    public GetClasificacionMediosListQueryHandlerTest()
    {
        _unitOfWorkMock = new Mock<IUnitOfWork>();
        _memoryCacheMock = new Mock<ISIGEMemoryCache>();
        _handler = new GetClasificacionMediosListQueryHandler(_unitOfWorkMock.Object, _memoryCacheMock.Object);
    }

    [Fact]
    public async Task Handle_ValidQuery_ShouldReturnListClasificacionMedios()
    {
        // Arrange
        var request = new GetClasificacionMediosListQuery();

        var clasificacionMedios = new List<ClasificacionMedio>
        {
            new ClasificacionMedio { Id = 1, Descripcion = "Vehículos" },
            new ClasificacionMedio { Id = 2, Descripcion = "Medios humanos" },
            new ClasificacionMedio { Id = 3, Descripcion = "Maquinaria pesada" }
        };

        _unitOfWorkMock.Setup(m => m.Repository<ClasificacionMedio>().GetAllAsync()).ReturnsAsync(clasificacionMedios);

        // Act
        var result = await _handler.Handle(request, CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result.Should().HaveCount(3);

        result[0].Id.Should().Be(1);
        result[0].Descripcion.Should().Be("Vehículos");
        result[1].Id.Should().Be(2);
        result[1].Descripcion.Should().Be("Medios humanos");
        result[2].Id.Should().Be(3);
        result[2].Descripcion.Should().Be("Maquinaria pesada");
    }


    [Fact]
    public async Task Handle_WithNoClasificacionMedios_ShouldReturnEmptyList()
    {
        // Arrange
        var query = new GetClasificacionMediosListQuery();
        var clasificacionMediosList = new List<ClasificacionMedio>();

        _unitOfWorkMock.Setup(m => m.Repository<ClasificacionMedio>().GetAllAsync()).ReturnsAsync(clasificacionMediosList);

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        Assert.Empty(result);
    }


    [Fact]
    public async Task Handle_WithNullClasificacionMedios_ShouldReturnEmptyList()
    {
        // Arrange
        var request = new GetClasificacionMediosListQuery();

        _unitOfWorkMock.Setup(m => m.Repository<ClasificacionMedio>().GetAllAsync())
            .ReturnsAsync((IReadOnlyList<ClasificacionMedio>)null);

        // Act
        var result = await _handler.Handle(request, CancellationToken.None);

        // Assert
        result.Should().BeNull();
    }

}
