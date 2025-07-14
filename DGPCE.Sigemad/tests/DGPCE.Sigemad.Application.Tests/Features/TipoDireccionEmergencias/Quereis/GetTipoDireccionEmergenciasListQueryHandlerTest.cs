using DGPCE.Sigemad.Application.Contracts.Persistence;
using DGPCE.Sigemad.Application.Features.TipoDireccionEmergencias.Quereis.GetTipoDireccionEmergenciasList;
using DGPCE.Sigemad.Domain.Modelos;
using FluentAssertions;
using Moq;

namespace DGPCE.Sigemad.Application.Tests.Features.TipoDireccionEmergencias.Quereis;
public class GetTipoDireccionEmergenciasListQueryHandlerTest
{

    private readonly Mock<IUnitOfWork> _unitOfWorkMock;
    private readonly GetTipoDireccionEmergenciasListQueryHandler _handler;

    public GetTipoDireccionEmergenciasListQueryHandlerTest()
    {
        _unitOfWorkMock = new Mock<IUnitOfWork>();
        _handler = new GetTipoDireccionEmergenciasListQueryHandler(_unitOfWorkMock.Object);
    }


    [Fact]
    public async Task Handle_ValidQuery_ShouldReturnListTipoDireccionEmergencias()
    {
        // Arrange
        var request = new GetTipoDireccionEmergenciasListQuery();

        var tipoDireccionEmergencias = new List<TipoDireccionEmergencia>
        {
            new TipoDireccionEmergencia { Id = 1, Descripcion = "Estatal" },
            new TipoDireccionEmergencia { Id = 2, Descripcion = "Autonómica" },
            new TipoDireccionEmergencia { Id = 3, Descripcion = "Municipal" }
        };

        _unitOfWorkMock.Setup(m => m.Repository<TipoDireccionEmergencia>().GetAllAsync()).ReturnsAsync(tipoDireccionEmergencias);

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
    public async Task Handle_WithNoTipoDireccionEmergencias_ShouldReturnEmptyList()
    {
        // Arrange
        var query = new GetTipoDireccionEmergenciasListQuery();
        var tipoDireccionEmergencias = new List<TipoDireccionEmergencia>();

        _unitOfWorkMock.Setup(m => m.Repository<TipoDireccionEmergencia>().GetAllAsync()).ReturnsAsync(tipoDireccionEmergencias);

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        Assert.Empty(result);
    }


    [Fact]
    public async Task Handle_WithNullTipoDireccionEmergencias_ShouldReturnEmptyList()
    {
        // Arrange
        var request = new GetTipoDireccionEmergenciasListQuery();

        _unitOfWorkMock.Setup(m => m.Repository<TipoDireccionEmergencia>().GetAllAsync())
            .ReturnsAsync((IReadOnlyList<TipoDireccionEmergencia>)null);

        // Act
        var result = await _handler.Handle(request, CancellationToken.None);

        // Assert
        result.Should().BeNull();
    }


}
