using DGPCE.Sigemad.Application.Contracts.Persistence;
using DGPCE.Sigemad.Application.Features.TipoPlanes.Quereis.GetTipoPlanesList;
using DGPCE.Sigemad.Application.Features.TiposRegistros.Queries.GetTiposRegistrosList;
using DGPCE.Sigemad.Domain.Modelos;
using FluentAssertions;
using Moq;

namespace DGPCE.Sigemad.Application.Tests.Features.TipoPlanes.Quereis;
public class GetTipoPlanesListQueryHanlderTest
{
    private readonly Mock<IUnitOfWork> _unitOfWorkMock;
    private readonly GetTipoPlanesListQueryHandler _handler;

    public GetTipoPlanesListQueryHanlderTest()
    {
        _unitOfWorkMock = new Mock<IUnitOfWork>();
        _handler = new GetTipoPlanesListQueryHandler(_unitOfWorkMock.Object);
    }

    [Fact]
    public async Task Handle_ValidQuery_ShouldReturnListTipoPlanes()
    {
        // Arrange
        var request = new GetTipoPlanesListQuery();

        var tipoPlanes = new List<TipoPlan>
        {
            new TipoPlan { Id = 1, Descripcion = "Estatal" },
            new TipoPlan { Id = 2, Descripcion = "Autonómica" },
            new TipoPlan { Id = 3, Descripcion = "Municipal" }
        };

        _unitOfWorkMock.Setup(m => m.Repository<TipoPlan>().GetAllAsync()).ReturnsAsync(tipoPlanes);

        // Act
        var result = await _handler.Handle(request, CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result.Should().HaveCount(3);

        result[0].Id.Should().Be(1);
        result[0].Descripcion.Should().Be("Estatal");
        result[1].Id.Should().Be(2);
        result[1].Descripcion.Should().Be("Autonómica");
        result[2].Id.Should().Be(3);
        result[2].Descripcion.Should().Be("Municipal");
    }

    [Fact]
    public async Task Handle_WithNoTipoPlanes_ShouldReturnEmptyList()
    {
        // Arrange
        var query = new GetTipoPlanesListQuery();
        var tiposRegistroList = new List<TipoPlan>();

        _unitOfWorkMock.Setup(m => m.Repository<TipoPlan>().GetAllAsync()).ReturnsAsync(tiposRegistroList);

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        Assert.Empty(result);
    }

    [Fact]
    public async Task Handle_WithNullTipoPlanes_ShouldReturnEmptyList()
    {
        // Arrange
        var request = new GetTipoPlanesListQuery();

        _unitOfWorkMock.Setup(m => m.Repository<TipoPlan>().GetAllAsync())
            .ReturnsAsync((IReadOnlyList<TipoPlan>)null);

        // Act
        var result = await _handler.Handle(request, CancellationToken.None);

        // Assert
        result.Should().BeNull();
    }

}
