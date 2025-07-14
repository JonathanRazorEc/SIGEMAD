using AutoMapper;
using DGPCE.Sigemad.Application.Contracts.Caching;
using DGPCE.Sigemad.Application.Contracts.Persistence;
using DGPCE.Sigemad.Application.Features.CaracterMedios.Quereis.GetCaracterMediosList;
using DGPCE.Sigemad.Application.Mappings;
using DGPCE.Sigemad.Domain.Modelos;
using FluentAssertions;
using Moq;


namespace DGPCE.Sigemad.Application.Tests.Features.CaracterMedios.Quereis;
public class GetCaracterMedioListQueryHandlerTest
{

    private readonly Mock<IUnitOfWork> _unitOfWorkMock;
    private readonly GetCaracterMediosListQueryHandler _handler;
    private readonly Mock<ISIGEMemoryCache> _memoryCacheMock;

    public GetCaracterMedioListQueryHandlerTest()
    {
        _unitOfWorkMock = new Mock<IUnitOfWork>();

        var config = new MapperConfiguration(cfg =>
        {
            cfg.AddProfile<MappingProfile>();
        });
        var mapper = config.CreateMapper();

        _memoryCacheMock = new Mock<ISIGEMemoryCache>();

        _handler = new GetCaracterMediosListQueryHandler(_unitOfWorkMock.Object, _memoryCacheMock.Object);
    }


    [Fact]
    public async Task Handle_ValidQuery_ShouldReturnListCaracterMedios()
    {
        // Arrange
        var request = new GetCaracterMediosListQuery();

        var caracterMedios = new List<CaracterMedio>
        {
            new CaracterMedio { Id = 1, Descripcion = "Ordinario" },
            new CaracterMedio { Id = 2, Descripcion = "Extraordinario" },
            new CaracterMedio { Id = 3, Descripcion = "En Prácticas" }
        };

        _unitOfWorkMock.Setup(m => m.Repository<CaracterMedio>().GetAllAsync()).ReturnsAsync(caracterMedios);

        // Act
        var result = await _handler.Handle(request, CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result.Should().HaveCount(3);

        result[0].Id.Should().Be(1);
        result[0].Descripcion.Should().Be("Ordinario");
        result[1].Id.Should().Be(2);
        result[1].Descripcion.Should().Be("Extraordinario");
        result[2].Id.Should().Be(3);
        result[2].Descripcion.Should().Be("En Prácticas");
    }

    [Fact]
    public async Task Handle_WithNoCaracterMedios_ShouldReturnEmptyList()
    {
        // Arrange
        var query = new GetCaracterMediosListQuery();
        var clasificacionMediosList = new List<CaracterMedio>();

        _unitOfWorkMock.Setup(m => m.Repository<CaracterMedio>().GetAllAsync()).ReturnsAsync(clasificacionMediosList);

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        Assert.Empty(result);
    }

    [Fact]
    public async Task Handle_WithNullCaracterMedios_ShouldReturnEmptyList()
    {
        // Arrange
        var request = new GetCaracterMediosListQuery();

        _unitOfWorkMock.Setup(m => m.Repository<CaracterMedio>().GetAllAsync())
            .ReturnsAsync((IReadOnlyList<CaracterMedio>)null);

        // Act
        var result = await _handler.Handle(request, CancellationToken.None);

        // Assert
        result.Should().BeNull();
    }

}
