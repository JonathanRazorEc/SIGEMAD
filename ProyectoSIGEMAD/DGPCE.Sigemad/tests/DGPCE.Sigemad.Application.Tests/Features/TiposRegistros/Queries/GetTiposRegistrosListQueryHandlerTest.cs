using DGPCE.Sigemad.Application.Contracts.Persistence;
using DGPCE.Sigemad.Application.Features.TiposRegistros.Queries.GetTiposRegistrosList;
using DGPCE.Sigemad.Domain.Modelos;
using FluentAssertions;
using Moq;

namespace DGPCE.Sigemad.Application.Tests.Features.TiposRegistros.Queries;
public class GetTiposRegistrosListQueryHandlerTest
{
    private readonly Mock<IUnitOfWork> _unitOfWorkMock;
    private readonly GetTiposRegistrosListQueryHandler _handler;

    public GetTiposRegistrosListQueryHandlerTest()
    {
        _unitOfWorkMock = new Mock<IUnitOfWork>();
        _handler = new GetTiposRegistrosListQueryHandler(_unitOfWorkMock.Object);
    }


    [Fact]
    public async Task Handle_ValidQuery_ShouldReturnListTiposRegistros()
    {
        // Arrange
        var request = new GetTiposRegistrosListQuery();

        var tiposRegistros = new List<TipoRegistro>
        {
            new TipoRegistro { Id = 1, Descripcion = "Evolución" },
            new TipoRegistro { Id = 2, Descripcion = "Resumen" }
        };

        _unitOfWorkMock.Setup(m => m.Repository<TipoRegistro>().GetAllAsync()).ReturnsAsync(tiposRegistros);

        // Act
        var result = await _handler.Handle(request, CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result.Should().HaveCount(2);

        result[0].Id.Should().Be(1);
        result[0].Descripcion.Should().Be("Evolución");
        result[1].Id.Should().Be(2);
        result[1].Descripcion.Should().Be("Resumen");
    }

    [Fact]
    public async Task Handle_WithNoTiposRegistros_ShouldReturnEmptyList()
    {
        // Arrange
        var query = new GetTiposRegistrosListQuery();
        var tiposRegistroList = new List<TipoRegistro>();

        _unitOfWorkMock.Setup(m => m.Repository<TipoRegistro>().GetAllAsync()).ReturnsAsync(tiposRegistroList);

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        Assert.Empty(result);
    }

    [Fact]
    public async Task Handle_WithNullTiposRegistros_ShouldReturnEmptyList()
    {
        // Arrange
        var request = new GetTiposRegistrosListQuery();

        _unitOfWorkMock.Setup(m => m.Repository<TipoRegistro>().GetAllAsync())
            .ReturnsAsync((IReadOnlyList<TipoRegistro>)null);

        // Act
        var result = await _handler.Handle(request, CancellationToken.None);

        // Assert
        result.Should().BeNull();
    }

}
