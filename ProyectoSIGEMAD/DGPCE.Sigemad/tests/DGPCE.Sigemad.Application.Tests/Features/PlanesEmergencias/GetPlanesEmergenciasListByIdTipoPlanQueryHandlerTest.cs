using AutoMapper;
using DGPCE.Sigemad.Application.Contracts.Persistence;
using DGPCE.Sigemad.Application.Features.PlanesEmergencias.Queries.GetPlanesEmergenciasByIdTipoPlan;
using DGPCE.Sigemad.Application.Features.PlanesEmergencias.Vms;
using DGPCE.Sigemad.Application.Specifications;
using DGPCE.Sigemad.Domain.Modelos;
using FluentAssertions;
using Moq;


namespace DGPCE.Sigemad.Application.Tests.Features.PlanesEmergencias;
public class GetPlanesEmergenciasListByIdTipoPlanQueryHandlerTest
{
    public class GetPlanesEmergenciasByIdTipoPlanQueryHandlerTest
    {
        private readonly Mock<IUnitOfWork> _unitOfWorkMock;
        private readonly Mock<IMapper> _mapperMock;
        private readonly GetPlanesEmergenciasListByIdTipoPlanQueryHandler _handler;

        public GetPlanesEmergenciasByIdTipoPlanQueryHandlerTest()
        {
            _unitOfWorkMock = new Mock<IUnitOfWork>();
            _mapperMock = new Mock<IMapper>();
            _handler = new GetPlanesEmergenciasListByIdTipoPlanQueryHandler(_unitOfWorkMock.Object, _mapperMock.Object);
        }

        [Fact]
        public async Task Handle_ValidQuery_ShouldReturnListPlanesEmergenciaVm()
        {
            // Arrange
            var request = new GetPlanesEmergenciasListByIdTipoPlanQuery(1);

            var planesEmergencias = new List<PlanEmergencia>
        {
            new PlanEmergencia { Id = 1, Descripcion = "Plan A", IdTipoPlan = 1 },
            new PlanEmergencia { Id = 2, Descripcion = "Plan B", IdTipoPlan = 1 }
        };

            var planesEmergenciaVm = new List<PlanEmergenciaVm>
        {
            new PlanEmergenciaVm { Id = 1, Descripcion = "Plan A" },
            new PlanEmergenciaVm { Id = 2, Descripcion = "Plan B"}
        };

            _unitOfWorkMock.Setup(m => m.Repository<PlanEmergencia>().GetAllWithSpec(It.IsAny<ISpecification<PlanEmergencia>>()))
                .ReturnsAsync(planesEmergencias);

            _mapperMock.Setup(m => m.Map<IReadOnlyList<PlanEmergencia>, IReadOnlyList<PlanEmergenciaVm>>(planesEmergencias))
                .Returns(planesEmergenciaVm);

            // Act
            var result = await _handler.Handle(request, CancellationToken.None);

            // Assert
            result.Should().NotBeNull();
            result.Should().HaveCount(2);

            result[0].Id.Should().Be(1);
            result[0].Descripcion.Should().Be("Plan A");

            result[1].Id.Should().Be(2);
            result[1].Descripcion.Should().Be("Plan B");

        }


        [Fact]
        public async Task Handle_WithNoTipoPlanes_ShouldReturnEmptyList()
        {
            // Arrange
            var query = new GetPlanesEmergenciasListByIdTipoPlanQuery(8);
            var planesEmergenciaList = new List<PlanEmergencia>();

            _unitOfWorkMock.Setup(m => m.Repository<PlanEmergencia>().GetAllWithSpec(It.IsAny<ISpecification<PlanEmergencia>>()))
                .ReturnsAsync(planesEmergenciaList);

            _mapperMock.Setup(m => m.Map<IReadOnlyList<PlanEmergencia>, IReadOnlyList<PlanEmergenciaVm>>(planesEmergenciaList))
                .Returns(new List<PlanEmergenciaVm>());

            // Act
            var result = await _handler.Handle(query, CancellationToken.None);

            // Assert
            Assert.Empty(result);
        }
    }

}
