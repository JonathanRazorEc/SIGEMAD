using AutoMapper;
using DGPCE.Sigemad.Application.Contracts.Files;
using DGPCE.Sigemad.Application.Contracts.Persistence;
using DGPCE.Sigemad.Application.Contracts.RegistrosActualizacion;
using DGPCE.Sigemad.Application.Dtos.ActivacionesPlanes;
using DGPCE.Sigemad.Application.Exceptions;
using DGPCE.Sigemad.Application.Features.ActivacionesPlanesEmergencia.Commands.ManageActivacionPlanEmergencia;
using DGPCE.Sigemad.Application.Specifications;
using DGPCE.Sigemad.Domain.Modelos;
using Microsoft.Extensions.Logging;
using Moq;
using System.Linq.Expressions;

namespace DGPCE.Sigemad.Application.Tests.Features.ActivacionesPlanesEmergencia.Commands;
public class ManageActivacionPlanEmergenciaCommandHandlerTests
{
    private readonly Mock<ILogger<ManageActivacionPlanEmergenciaCommandHandler>> _loggerMock;
    private readonly Mock<IUnitOfWork> _unitOfWorkMock;
    private readonly Mock<IMapper> _mapperMock;
    private readonly Mock<IFileService> _fileServiceMock;
    private readonly ManageActivacionPlanEmergenciaCommandHandler _handler;
    private readonly Mock<IRegistroActualizacionService> _registroActualizacionMock;

    public ManageActivacionPlanEmergenciaCommandHandlerTests()
    {
        _loggerMock = new Mock<ILogger<ManageActivacionPlanEmergenciaCommandHandler>>();
        _unitOfWorkMock = new Mock<IUnitOfWork>();
        _mapperMock = new Mock<IMapper>();
        _fileServiceMock = new Mock<IFileService>();
        _registroActualizacionMock = new Mock<IRegistroActualizacionService>();

        _handler = new ManageActivacionPlanEmergenciaCommandHandler(
            _loggerMock.Object,
            _unitOfWorkMock.Object,
            _mapperMock.Object,
            _fileServiceMock.Object,
            _registroActualizacionMock.Object
        );
    }

    [Fact]
    public async Task Handle_ShouldCreateNewActuacionRelevante_WhenIdActuacionRelevanteIsNull()
    {
        // Arrange
        var command = new ManageActivacionPlanEmergenciaCommand
        {
            IdSuceso = 1,
            ActivacionesPlanes = new List<ManageActivacionPlanEmergenciaDto>
            {
                new ManageActivacionPlanEmergenciaDto
                {
                    IdTipoPlan = 1,
                    IdPlanEmergencia = 1,
                    FechaInicio = DateOnly.FromDateTime(DateTime.Now),
                    Autoridad = "Autoridad"
                }
            }
        };

        _unitOfWorkMock.Setup(uow => uow.Repository<Suceso>().GetByIdAsync(It.IsAny<int>()))
            .ReturnsAsync(new Suceso { Id = 1 });

        _unitOfWorkMock.Setup(uow => uow.Repository<TipoPlan>().GetAsync(It.IsAny<Expression<Func<TipoPlan, bool>>>()))
            .ReturnsAsync(new List<TipoPlan> { new TipoPlan { Id = 1 } });

        _unitOfWorkMock.Setup(uow => uow.Repository<PlanEmergencia>().GetAsync(It.IsAny<Expression<Func<PlanEmergencia, bool>>>()))
            .ReturnsAsync(new List<PlanEmergencia> { new PlanEmergencia { Id = 1 } });

        _unitOfWorkMock.Setup(uow => uow.Repository<ActuacionRelevanteDGPCE>().AddEntity(It.IsAny<ActuacionRelevanteDGPCE>()))
            .Callback<ActuacionRelevanteDGPCE>(entity => entity.Id = 1);
        _unitOfWorkMock.Setup(uow => uow.Complete()).ReturnsAsync(1);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(1, result.IdActuacionRelevante);
        _unitOfWorkMock.Verify(uow => uow.Repository<ActuacionRelevanteDGPCE>().AddEntity(It.IsAny<ActuacionRelevanteDGPCE>()), Times.Once);
        _unitOfWorkMock.Verify(uow => uow.Complete(), Times.Once);
    }

    [Fact]
    public async Task Handle_ShouldThrowNotFoundException_WhenSucesoNotFound()
    {
        // Arrange
        var command = new ManageActivacionPlanEmergenciaCommand
        {
            IdSuceso = 1,
            ActivacionesPlanes = new List<ManageActivacionPlanEmergenciaDto>
            {
                new ManageActivacionPlanEmergenciaDto
                {
                    IdTipoPlan = 1,
                    IdPlanEmergencia = 1,
                    FechaInicio = DateOnly.FromDateTime(DateTime.Now),
                    Autoridad = "Autoridad"
                }
            }
        };

        _unitOfWorkMock.Setup(uow => uow.Repository<Suceso>().GetByIdAsync(It.IsAny<int>()))
            .ReturnsAsync((Suceso)null);

        // Act & Assert
        await Assert.ThrowsAsync<NotFoundException>(() => _handler.Handle(command, CancellationToken.None));
    }

    [Fact]
    public async Task Handle_ShouldUpdateExistingActuacionRelevante_WhenIdActuacionRelevanteIsProvided()
    {
        // Arrange
        var command = new ManageActivacionPlanEmergenciaCommand
        {
            IdRegistroActualizacion = 1,
            IdSuceso = 1,
            ActivacionesPlanes = new List<ManageActivacionPlanEmergenciaDto>
            {
                new ManageActivacionPlanEmergenciaDto
                {
                    IdTipoPlan = 1,
                    IdPlanEmergencia = 1,
                    FechaInicio = DateOnly.FromDateTime(DateTime.Now),
                    Autoridad = "Autoridad"
                }
            }
        };

        var actuacionRelevante = new ActuacionRelevanteDGPCE { Id = 1, IdSuceso = 1 };

        _unitOfWorkMock.Setup(uow => uow.Repository<ActuacionRelevanteDGPCE>().GetByIdWithSpec(It.IsAny<ISpecification<ActuacionRelevanteDGPCE>>()))
            .ReturnsAsync(actuacionRelevante);

        _unitOfWorkMock.Setup(uow => uow.Repository<TipoPlan>().GetAsync(It.IsAny<Expression<Func<TipoPlan, bool>>>()))
            .ReturnsAsync(new List<TipoPlan> { new TipoPlan { Id = 1 } });

        _unitOfWorkMock.Setup(uow => uow.Repository<PlanEmergencia>().GetAsync(It.IsAny<Expression<Func<PlanEmergencia, bool>>>()))
            .ReturnsAsync(new List<PlanEmergencia> { new PlanEmergencia { Id = 1 } });

        _unitOfWorkMock.Setup(uow => uow.Repository<ActuacionRelevanteDGPCE>().UpdateEntity(It.IsAny<ActuacionRelevanteDGPCE>()));
        _unitOfWorkMock.Setup(uow => uow.Complete()).ReturnsAsync(1);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(1, result.IdActuacionRelevante);
        _unitOfWorkMock.Verify(uow => uow.Repository<ActuacionRelevanteDGPCE>().UpdateEntity(It.IsAny<ActuacionRelevanteDGPCE>()), Times.Once);
        _unitOfWorkMock.Verify(uow => uow.Complete(), Times.Once);
    }

    [Fact]
    public async Task Handle_ShouldThrowNotFoundException_WhenActuacionRelevanteNotFound()
    {
        // Arrange
        var command = new ManageActivacionPlanEmergenciaCommand
        {
            IdRegistroActualizacion = 1,
            IdSuceso = 1,
            ActivacionesPlanes = new List<ManageActivacionPlanEmergenciaDto>
            {
                new ManageActivacionPlanEmergenciaDto
                {
                    IdTipoPlan = 1,
                    IdPlanEmergencia = 1,
                    FechaInicio = DateOnly.FromDateTime(DateTime.Now),
                    Autoridad = "Autoridad"
                }
            }
        };

        _unitOfWorkMock.Setup(uow => uow.Repository<ActuacionRelevanteDGPCE>().GetByIdWithSpec(It.IsAny<ISpecification<ActuacionRelevanteDGPCE>>()))
            .ReturnsAsync((ActuacionRelevanteDGPCE)null);

        // Act & Assert
        await Assert.ThrowsAsync<NotFoundException>(() => _handler.Handle(command, CancellationToken.None));
    }
}

