using AutoMapper;
using DGPCE.Sigemad.Application.Contracts.Persistence;
using DGPCE.Sigemad.Application.Features.Fases.Queries.GetFasesEmergenciaByIdPlanEmergenciaList;
using DGPCE.Sigemad.Application.Features.Fases.Vms;
using DGPCE.Sigemad.Application.Specifications;
using DGPCE.Sigemad.Domain.Modelos;
using FluentAssertions;
using Moq;
namespace DGPCE.Sigemad.Application.Tests.Features.FasesEmergencia;

public class GetFasesEmergenciaListByIdPlanEmergenciaQueryHandlerTest
{
    private readonly Mock<IUnitOfWork> _unitOfWorkMock;
    private readonly Mock<IMapper> _mapperMock;
    private readonly GetFasesEmergenciaByIdPlanEmergenciaListQueryHandler _handler;

    public GetFasesEmergenciaListByIdPlanEmergenciaQueryHandlerTest()
    {
        _unitOfWorkMock = new Mock<IUnitOfWork>();
        _mapperMock = new Mock<IMapper>();
        _handler = new GetFasesEmergenciaByIdPlanEmergenciaListQueryHandler(_unitOfWorkMock.Object, _mapperMock.Object);
    }

    [Fact]
    public async Task Handle_ValidQuery_ShouldReturnListFaseEmergenciaVm()
    {
        // Arrange
        var request = new GetFasesEmergenciaByIdPlanEmergenciaListQuery(1);

        var fasesEmergencias = new List<FaseEmergencia>
        {
            new FaseEmergencia { Id = 1, Descripcion = "Fase A", IdPlanEmergencia = 1 },
            new FaseEmergencia { Id = 2, Descripcion = "Fase B", IdPlanEmergencia = 1 }
        };

        var fasesEmergenciaVm = new List<FaseEmergenciaVm>
        {
            new FaseEmergenciaVm { Id = 1, Descripcion = "Fase A" },
            new FaseEmergenciaVm { Id = 2, Descripcion = "Fase B" }
        };

        _unitOfWorkMock.Setup(m => m.Repository<FaseEmergencia>().GetAllWithSpec(It.IsAny<ISpecification<FaseEmergencia>>()))
            .ReturnsAsync(fasesEmergencias);

        _mapperMock.Setup(m => m.Map<IReadOnlyList<FaseEmergencia>, IReadOnlyList<FaseEmergenciaVm>>(fasesEmergencias))
            .Returns(fasesEmergenciaVm);

        // Act
        var result = await _handler.Handle(request, CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result.Should().HaveCount(2);

        result[0].Id.Should().Be(1);
        result[0].Descripcion.Should().Be("Fase A");

        result[1].Id.Should().Be(2);
        result[1].Descripcion.Should().Be("Fase B");
    }

    [Fact]
    public async Task Handle_WithNoFasesEmergencia_ShouldReturnEmptyList()
    {
        // Arrange
        var query = new GetFasesEmergenciaByIdPlanEmergenciaListQuery(8);
        var fasesEmergenciaList = new List<FaseEmergencia>();

        _unitOfWorkMock.Setup(m => m.Repository<FaseEmergencia>().GetAllWithSpec(It.IsAny<ISpecification<FaseEmergencia>>()))
            .ReturnsAsync(fasesEmergenciaList);

        _mapperMock.Setup(m => m.Map<IReadOnlyList<FaseEmergencia>, IReadOnlyList<FaseEmergenciaVm>>(fasesEmergenciaList))
            .Returns(new List<FaseEmergenciaVm>());

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        Assert.Empty(result);
    }


    [Fact]
    public async Task Handle_WithAllFasesEmergencia_ShouldReturnEmptyList()
    {
        // Arrange
        var request = new GetFasesEmergenciaByIdPlanEmergenciaListQuery(null);
        var fasesEmergencias = new List<FaseEmergencia>
        {
            new FaseEmergencia { Id = 1, Descripcion = "Fase A", IdPlanEmergencia = 1 },
            new FaseEmergencia { Id = 2, Descripcion = "Fase B", IdPlanEmergencia = 2 },
            new FaseEmergencia { Id = 3, Descripcion = "Fase C", IdPlanEmergencia = 3 },
            new FaseEmergencia { Id = 4, Descripcion = "Fase D", IdPlanEmergencia = 4 }
        };

        var fasesEmergenciaVm = new List<FaseEmergenciaVm>
        {
            new FaseEmergenciaVm { Id = 1, Descripcion = "Fase A" },
            new FaseEmergenciaVm { Id = 2, Descripcion = "Fase B" },
            new FaseEmergenciaVm { Id = 3, Descripcion = "Fase C" },
            new FaseEmergenciaVm { Id = 4, Descripcion = "Fase D"}
        };

        _unitOfWorkMock.Setup(m => m.Repository<FaseEmergencia>().GetAllWithSpec(It.IsAny<ISpecification<FaseEmergencia>>()))
       .ReturnsAsync(fasesEmergencias);

        _mapperMock.Setup(m => m.Map<IReadOnlyList<FaseEmergencia>, IReadOnlyList<FaseEmergenciaVm>>(fasesEmergencias))
            .Returns(fasesEmergenciaVm);

        // Act
        var result = await _handler.Handle(request, CancellationToken.None);

       
        // Assert
        result.Should().NotBeNull();
        result.Should().HaveCount(4);

        result[0].Id.Should().Be(1);
        result[0].Descripcion.Should().Be("Fase A");

        result[1].Id.Should().Be(2);
        result[1].Descripcion.Should().Be("Fase B");

        result[2].Id.Should().Be(3);
        result[2].Descripcion.Should().Be("Fase C");

        result[3].Id.Should().Be(4);
        result[3].Descripcion.Should().Be("Fase D");
    }
}
