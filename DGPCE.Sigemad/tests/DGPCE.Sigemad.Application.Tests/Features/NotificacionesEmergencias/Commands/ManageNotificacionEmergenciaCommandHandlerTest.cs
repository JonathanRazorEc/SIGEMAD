using AutoMapper;
using DGPCE.Sigemad.Application.Contracts.Persistence;
using DGPCE.Sigemad.Application.Contracts.RegistrosActualizacion;
using DGPCE.Sigemad.Application.Dtos.ActivacionSistema;
using DGPCE.Sigemad.Application.Dtos.NotificacionesEmergencias;
using DGPCE.Sigemad.Application.Exceptions;
using DGPCE.Sigemad.Application.Features.ActivacionesSistemas.Commands.ManageActivacionSistema;
using DGPCE.Sigemad.Application.Features.NotificacionesEmergencias.Commands.ManageNotificacionEmergencia;
using DGPCE.Sigemad.Application.Specifications;
using DGPCE.Sigemad.Domain.Modelos;
using Microsoft.Extensions.Logging;
using Moq;
using System.Linq.Expressions;

namespace DGPCE.Sigemad.Application.Tests.Features.NotificacionesEmergencias.Commands;
public class ManageNotificacionEmergenciaCommandHandlerTest
{

    private readonly Mock<ILogger<ManageNotificacionEmergenciaHandler>> _loggerMock;
    private readonly Mock<IUnitOfWork> _unitOfWorkMock;
    private readonly Mock<IMapper> _mapperMock;
    private readonly ManageNotificacionEmergenciaHandler _handler;
    private readonly Mock<IRegistroActualizacionService> _registroActualizacionMock;

    public ManageNotificacionEmergenciaCommandHandlerTest()
    {
        _loggerMock = new Mock<ILogger<ManageNotificacionEmergenciaHandler>>();
        _unitOfWorkMock = new Mock<IUnitOfWork>();
        _mapperMock = new Mock<IMapper>();
        _registroActualizacionMock = new Mock<IRegistroActualizacionService>();

        _handler = new ManageNotificacionEmergenciaHandler(
            _loggerMock.Object,
            _unitOfWorkMock.Object,
            _mapperMock.Object,
            _registroActualizacionMock.Object
        );
    }

    [Fact]
    public async Task Handle_ShouldCreateNewActuacionRelevante_WhenIdActuacionRelevanteIsNull()
    {
        // Arrange
        var command = new ManageNotificacionEmergenciaCommand
        {
            IdSuceso = 1,
            Detalles = new List<ManageNotificacionEmergenciaDto>
            {
                new ManageNotificacionEmergenciaDto
                {
                    IdTipoNotificacion = 1,
                    OrganosNotificados = "Ministerio del interior",
                    FechaHoraNotificacion = DateTime.Now
                }
            }
        };

        var notificacionEmergenciaDto = command.Detalles.First();
        var notificacionEmergencia = new NotificacionEmergencia
        {
            Id = 0,
            IdTipoNotificacion = notificacionEmergenciaDto.IdTipoNotificacion,
            OrganosNotificados = notificacionEmergenciaDto.OrganosNotificados,
            FechaHoraNotificacion = notificacionEmergenciaDto.FechaHoraNotificacion
        };

        _unitOfWorkMock.Setup(uow => uow.Repository<Suceso>().GetByIdAsync(It.IsAny<int>()))
            .ReturnsAsync(new Suceso { Id = 1 });

        _unitOfWorkMock.Setup(uow => uow.Repository<TipoNotificacion>().GetAsync(It.IsAny<Expression<Func<TipoNotificacion, bool>>>()))
            .ReturnsAsync(new List<TipoNotificacion> { new TipoNotificacion { Id = 1 } });


        _mapperMock.Setup(m => m.Map<NotificacionEmergencia>(It.IsAny<ManageNotificacionEmergenciaDto>()))
       .Returns(notificacionEmergencia);

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
        var command = new ManageNotificacionEmergenciaCommand
        {
            IdSuceso = 1,
            Detalles = new List<ManageNotificacionEmergenciaDto>
            {
                new ManageNotificacionEmergenciaDto
                {
                    IdTipoNotificacion = 1,
                    OrganosNotificados = "Ministerio del interior",
                    FechaHoraNotificacion = DateTime.Now
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
        var command = new ManageNotificacionEmergenciaCommand
        {
            IdRegistroActualizacion = 1,
            IdSuceso = 1,
            Detalles = new List<ManageNotificacionEmergenciaDto>
            {
                new ManageNotificacionEmergenciaDto
                {
                    IdTipoNotificacion = 1,
                    OrganosNotificados = "Ministerio del interior",
                    FechaHoraNotificacion = DateTime.Now
                }
            }
        };

        var actuacionRelevante = new ActuacionRelevanteDGPCE { Id = 1, IdSuceso = 1 };

        _unitOfWorkMock.Setup(uow => uow.Repository<ActuacionRelevanteDGPCE>().GetByIdWithSpec(It.IsAny<ISpecification<ActuacionRelevanteDGPCE>>()))
            .ReturnsAsync(actuacionRelevante);

        var notificacionEmergenciaDto = command.Detalles.First();
        var notificacionEmergencia = new NotificacionEmergencia
        {
            Id = 0,
            IdTipoNotificacion = notificacionEmergenciaDto.IdTipoNotificacion,
            OrganosNotificados = notificacionEmergenciaDto.OrganosNotificados,
            FechaHoraNotificacion = notificacionEmergenciaDto.FechaHoraNotificacion
        };

        _unitOfWorkMock.Setup(uow => uow.Repository<Suceso>().GetByIdAsync(It.IsAny<int>()))
               .ReturnsAsync(new Suceso { Id = 1 });

        _unitOfWorkMock.Setup(uow => uow.Repository<TipoNotificacion>().GetAsync(It.IsAny<Expression<Func<TipoNotificacion, bool>>>()))
            .ReturnsAsync(new List<TipoNotificacion> { new TipoNotificacion { Id = 1 } });


        _mapperMock.Setup(m => m.Map<NotificacionEmergencia>(It.IsAny<ManageNotificacionEmergenciaDto>()))
       .Returns(notificacionEmergencia);

        _unitOfWorkMock.Setup(uow => uow.Repository<ActuacionRelevanteDGPCE>().AddEntity(It.IsAny<ActuacionRelevanteDGPCE>()))
            .Callback<ActuacionRelevanteDGPCE>(entity => entity.Id = 1);
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
        var command = new ManageNotificacionEmergenciaCommand
        {
            IdRegistroActualizacion = 1,
            IdSuceso = 1,
            Detalles = new List<ManageNotificacionEmergenciaDto>
            {
                new ManageNotificacionEmergenciaDto
                {
                    IdTipoNotificacion = 1,
                    OrganosNotificados = "Ministerio del interior",
                    FechaHoraNotificacion = DateTime.Now
                }
            }
        };

        _unitOfWorkMock.Setup(uow => uow.Repository<ActuacionRelevanteDGPCE>().GetByIdWithSpec(It.IsAny<ISpecification<ActuacionRelevanteDGPCE>>()))
            .ReturnsAsync((ActuacionRelevanteDGPCE)null);

        // Act & Assert
        await Assert.ThrowsAsync<NotFoundException>(() => _handler.Handle(command, CancellationToken.None));
    }
}
