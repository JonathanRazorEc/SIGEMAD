using DGPCE.Sigemad.Application.Contracts.Persistence;
using DGPCE.Sigemad.Application.Features.TiposSistemasEmergencias.Queries.GetTiposSistemasEmergenciasList;
using DGPCE.Sigemad.Domain.Modelos;
using FluentAssertions;
using Moq;

namespace DGPCE.Sigemad.Application.Tests.Features.TiposSistemasEmergencias;

public class GetTiposSistemasEmergenciasListQueryHandlerTest
{
    private readonly Mock<IUnitOfWork> _unitOfWorkMock;
    private readonly GetTiposSistemasEmergenciasListQueryHandler _handler;

    public GetTiposSistemasEmergenciasListQueryHandlerTest()
    {
        _unitOfWorkMock = new Mock<IUnitOfWork>();
        _handler = new GetTiposSistemasEmergenciasListQueryHandler(_unitOfWorkMock.Object);
    }

    [Fact]
    public async Task Handle_ValidQuery_ShouldReturnListTiposSistemasEmergencias()
    {
        // Arrange
        var request = new GetTiposSistemasEmergenciasListQuery();

        var tiposSistemasEmergencias = new List<TipoSistemaEmergencia>
        {
            new TipoSistemaEmergencia { Id = 1, Descripcion = "Copernicus" },
            new TipoSistemaEmergencia { Id = 2, Descripcion = "UCPM" },
        };

        _unitOfWorkMock.Setup(m => m.Repository<TipoSistemaEmergencia>().GetAllAsync()).ReturnsAsync(tiposSistemasEmergencias);

        // Act
        var result = await _handler.Handle(request, CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result.Should().HaveCount(2);

        result[0].Id.Should().Be(1);
        result[0].Descripcion.Should().Be("Copernicus");
        result[1].Id.Should().Be(2);
        result[1].Descripcion.Should().Be("UCPM");
    }


    [Fact]
    public async Task Handle_WithNoTiposSistemasEmergencias_ShouldReturnEmptyList()
    {
        // Arrange
        var query = new GetTiposSistemasEmergenciasListQuery();
        var tiposSistemasEmergencias = new List<TipoSistemaEmergencia>();

        _unitOfWorkMock.Setup(m => m.Repository<TipoSistemaEmergencia>().GetAllAsync()).ReturnsAsync(tiposSistemasEmergencias);

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        Assert.Empty(result);
    }

    [Fact]
    public async Task Handle_WithNullTiposSistemasEmergencias_ShouldReturnEmptyList()
    {
        // Arrange
        var request = new GetTiposSistemasEmergenciasListQuery();

        _unitOfWorkMock.Setup(m => m.Repository<TipoSistemaEmergencia>().GetAllAsync())
            .ReturnsAsync((IReadOnlyList<TipoSistemaEmergencia>)null);

        // Act
        var result = await _handler.Handle(request, CancellationToken.None);

        // Assert
        result.Should().BeNull();
    }
}