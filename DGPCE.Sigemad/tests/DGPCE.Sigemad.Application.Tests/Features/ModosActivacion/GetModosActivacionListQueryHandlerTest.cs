using DGPCE.Sigemad.Application.Contracts.Caching;
using DGPCE.Sigemad.Application.Contracts.Persistence;
using DGPCE.Sigemad.Application.Features.ModosActivacion.Queries.GetModosActivacionList;
using DGPCE.Sigemad.Domain.Modelos;
using FluentAssertions;
using Moq;

namespace DGPCE.Sigemad.Application.Tests.Features.ModosActivacion;

public class GetModosActivacionListQueryHandlerTest
{
    private readonly Mock<IUnitOfWork> _unitOfWorkMock;
    private readonly GetModosActivacionListQueryHandler _handler;
    private readonly Mock<ISIGEMemoryCache> _memoryCacheMock;

    public GetModosActivacionListQueryHandlerTest()
    {
        _unitOfWorkMock = new Mock<IUnitOfWork>();
        _memoryCacheMock = new Mock<ISIGEMemoryCache>();
        _handler = new GetModosActivacionListQueryHandler(_unitOfWorkMock.Object, _memoryCacheMock.Object);
    }

    [Fact]
    public async Task Handle_ValidQuery_ShouldReturnListModosActivacion()
    {
        // Arrange
        var request = new GetModosActivacionListQuery();

        var modosActivacion = new List<ModoActivacion>
        {
            new ModoActivacion { Id = 1, Descripcion = "Rapid Mapping" },
            new ModoActivacion { Id = 2, Descripcion = "Risk and Recovery" },
        };

        _unitOfWorkMock.Setup(m => m.Repository<ModoActivacion>().GetAllAsync()).ReturnsAsync(modosActivacion);

        // Act
        var result = await _handler.Handle(request, CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result.Should().HaveCount(2);

        result[0].Id.Should().Be(1);
        result[0].Descripcion.Should().Be("Rapid Mapping");
        result[1].Id.Should().Be(2);
        result[1].Descripcion.Should().Be("Risk and Recovery");
    }


    [Fact]
    public async Task Handle_WithNoModosActivacion_ShouldReturnEmptyList()
    {
        // Arrange
        var query = new GetModosActivacionListQuery();
        var modosActivacion = new List<ModoActivacion>();

        _unitOfWorkMock.Setup(m => m.Repository<ModoActivacion>().GetAllAsync()).ReturnsAsync(modosActivacion);

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        Assert.Empty(result);
    }

    [Fact]
    public async Task Handle_WithNullModosActivacion_ShouldReturnEmptyList()
    {
        // Arrange
        var request = new GetModosActivacionListQuery();

        _unitOfWorkMock.Setup(m => m.Repository<ModoActivacion>().GetAllAsync())
            .ReturnsAsync((IReadOnlyList<ModoActivacion>)null);

        // Act
        var result = await _handler.Handle(request, CancellationToken.None);

        // Assert
        result.Should().BeNull();
    }
}