using AutoMapper;
using DGPCE.Sigemad.Application.Contracts.Persistence;
using DGPCE.Sigemad.Application.Features.PlanesSituaciones.Queries.GetPlanesSituacionesListByIdPlanIdFase;
using DGPCE.Sigemad.Application.Features.PlanesSituaciones.Vms;
using DGPCE.Sigemad.Application.Specifications;
using DGPCE.Sigemad.Domain.Modelos;
using FluentAssertions;
using Moq;

namespace DGPCE.Sigemad.Application.Tests.Features.PlanesSituaciones;
public class GetPlanesSituacionesListByIdTipoPlanQueryHandlerTest
{
    private readonly Mock<IUnitOfWork> _unitOfWorkMock;
    private readonly Mock<IMapper> _mapperMock;
    private readonly GetPlanesSituacionesByIdPlanIdFaseListQueryHandler _handler;

    public GetPlanesSituacionesListByIdTipoPlanQueryHandlerTest()
    {
        _unitOfWorkMock = new Mock<IUnitOfWork>();
        _mapperMock = new Mock<IMapper>();
        _handler = new GetPlanesSituacionesByIdPlanIdFaseListQueryHandler(_unitOfWorkMock.Object, _mapperMock.Object);
    }

    [Fact]
    public async Task Handle_ValidQuery_ShouldReturnListFaseEmergenciaVm()
    {
        // Arrange
        var request = new GetPlanesSituacionesByIdPlanIdFaseListQuery(1,1);

        var planesSituaciones = new List<PlanSituacion>
        {
            new PlanSituacion { Id = 1, Descripcion = "Fase A", IdPlanEmergencia = 1 ,IdFaseEmergencia = 1,Orden = 1,Nivel = "Nivel1",Situacion = "Situacion1",SituacionEquivalente="SituacionesEquivalentes1"},
            new PlanSituacion { Id = 2, Descripcion = "Fase B", IdPlanEmergencia = 1,IdFaseEmergencia = 1,Orden = 2,Nivel = "Nivel2",Situacion = "Situacion2",SituacionEquivalente="SituacionesEquivalentes2"}
        };

        var planesSituacionesVm = new List<PlanSituacionVm>
        {
            new PlanSituacionVm { Id = 1, Descripcion = "Fase A" },
            new PlanSituacionVm { Id = 2, Descripcion = "Fase B" }
        };

        _unitOfWorkMock.Setup(m => m.Repository<PlanSituacion>().GetAllWithSpec(It.IsAny<ISpecification<PlanSituacion>>()))
            .ReturnsAsync(planesSituaciones);

        _mapperMock.Setup(m => m.Map<IReadOnlyList<PlanSituacion>, IReadOnlyList<PlanSituacionVm>>(planesSituaciones))
            .Returns(planesSituacionesVm);

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
        var query = new GetPlanesSituacionesByIdPlanIdFaseListQuery(80, 80);
        var planesSituacionesList = new List<PlanSituacion>();

        _unitOfWorkMock.Setup(m => m.Repository<PlanSituacion>().GetAllWithSpec(It.IsAny<ISpecification<PlanSituacion>>()))
            .ReturnsAsync(planesSituacionesList);

        _mapperMock.Setup(m => m.Map<IReadOnlyList<PlanSituacion>, IReadOnlyList<PlanSituacionVm>>(planesSituacionesList))
            .Returns(new List<PlanSituacionVm>());

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        Assert.Empty(result);
    }


    [Fact]
    public async Task Handle_WithAllFasesEmergencia_ShouldReturnEmptyList()
    {
        // Arrange
        var request = new GetPlanesSituacionesByIdPlanIdFaseListQuery(1, 1);

        var planesSituaciones = new List<PlanSituacion>
        {
            new PlanSituacion { Id = 1, Descripcion = "Fase A", IdPlanEmergencia = 1 ,IdFaseEmergencia = 1,Orden = 1,Nivel = "Nivel1",Situacion = "Situacion1",SituacionEquivalente="SituacionesEquivalentes1"},
            new PlanSituacion { Id = 2, Descripcion = "Fase B", IdPlanEmergencia = 1,IdFaseEmergencia = 1,Orden = 2,Nivel = "Nivel2",Situacion = "Situacion2",SituacionEquivalente="SituacionesEquivalentes2"},
            new PlanSituacion { Id = 3, Descripcion = "Fase C", IdPlanEmergencia = 1 ,IdFaseEmergencia = 2,Orden = 1,Nivel = "Nivel3",Situacion = "Situacion1",SituacionEquivalente="SituacionesEquivalentes4"},
            new PlanSituacion { Id = 4, Descripcion = "Fase D", IdPlanEmergencia = 2,IdFaseEmergencia = 3,Orden = 2,Nivel = "Nivel4",Situacion = "Situacion2",SituacionEquivalente="SituacionesEquivalentes5"}
        };

        var planesSituacionesVm = new List<PlanSituacionVm>
        {
            new PlanSituacionVm { Id = 1, Descripcion = "Fase A" },
            new PlanSituacionVm { Id = 2, Descripcion = "Fase B" },
            new PlanSituacionVm { Id = 3, Descripcion = "Fase C" },
            new PlanSituacionVm { Id = 4, Descripcion = "Fase D" }
        };

        _unitOfWorkMock.Setup(m => m.Repository<PlanSituacion>().GetAllWithSpec(It.IsAny<ISpecification<PlanSituacion>>()))
            .ReturnsAsync(planesSituaciones);

        _mapperMock.Setup(m => m.Map<IReadOnlyList<PlanSituacion>, IReadOnlyList<PlanSituacionVm>>(planesSituaciones))
            .Returns(planesSituacionesVm);

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
