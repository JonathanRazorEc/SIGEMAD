using AutoMapper;
using DGPCE.Sigemad.Application.Contracts.Persistence;
using DGPCE.Sigemad.Application.Contracts.RegistrosActualizacion;
using DGPCE.Sigemad.Application.Dtos.ConvocatoriasCECOD;
using DGPCE.Sigemad.Application.Dtos.DeclaracionesZAGEP;
using DGPCE.Sigemad.Application.Exceptions;
using DGPCE.Sigemad.Application.Features.ConvocatoriasCECOD.Commands;
using DGPCE.Sigemad.Application.Features.DeclaracionesZAGEP.Commands.ManageDeclaracionesZAGEP;
using DGPCE.Sigemad.Application.Specifications.ActuacionesRelevantesDGPCE;
using DGPCE.Sigemad.Domain.Modelos;
using Microsoft.Extensions.Logging;
using Moq;

namespace DGPCE.Sigemad.Application.Tests.Features.ConvocatoriasCECOD.Commands;
public class ManageConvocatoriaCECODCommandHandlerTests
{
    private readonly Mock<ILogger<ManageConvocatoriaCECODCommandHandler>> _loggerMock;
    private readonly Mock<IUnitOfWork> _unitOfWorkMock;
    private readonly Mock<IMapper> _mapperMock;
    private readonly Mock<IRegistroActualizacionService> _registroActualizacionMock;

    public ManageConvocatoriaCECODCommandHandlerTests()
    {
        _loggerMock = new Mock<ILogger<ManageConvocatoriaCECODCommandHandler>>();
        _unitOfWorkMock = new Mock<IUnitOfWork>();
        _mapperMock = new Mock<IMapper>();
        _registroActualizacionMock = new Mock<IRegistroActualizacionService>();
    }


    [Fact]
    public async Task Handle_WithValidRequestAndExistingConvocatoriaCECOD_ShouldUpdateConvocatoriaCECOD()
    {
        // Arrange
        var command = new ManageConvocatoriaCECODCommand
        {
            IdRegistroActualizacion = 1,
            IdSuceso = 1,
            Detalles = new List<ManageConvocatoriaCECODDto>
        {
            new ManageConvocatoriaCECODDto
            {
                Id = 1,
                FechaInicio = new DateOnly(2022, 1, 1),
                FechaFin = new DateOnly(2022, 1, 10),
                Lugar = "Sede PCYE",
                Convocados = "10",
                Participantes = "5",
                Observaciones = "Observaciones 1 ACTUALIZADO"
            }
        }
        };

        var actuacionRelevante = new ActuacionRelevanteDGPCE
        {
            Id = 1,
            IdSuceso = 1,
            ConvocatoriasCECOD = new List<ConvocatoriaCECOD>
        {
            new ConvocatoriaCECOD
            {
                 Id = 1,
                FechaInicio = new DateOnly(2022, 1, 1),
                FechaFin = new DateOnly(2022, 1, 10),
                Lugar = "Sede ministerio",
                Convocados = "9",
                Participantes = "2",
                Observaciones = "Observaciones 1"
            }
        }
        };

        _unitOfWorkMock.Setup(uow => uow.Repository<ActuacionRelevanteDGPCE>().GetByIdWithSpec(It.IsAny<ActuacionRelevanteDGPCESpecification>()))
            .ReturnsAsync(actuacionRelevante);

        _unitOfWorkMock.Setup(uow => uow.Complete())
            .ReturnsAsync(1);

        var handler = new ManageConvocatoriaCECODCommandHandler(_loggerMock.Object, _unitOfWorkMock.Object, _mapperMock.Object, _registroActualizacionMock.Object);

        // Act
        var response = await handler.Handle(command, CancellationToken.None);

        // Assert
        _unitOfWorkMock.Verify(uow => uow.Repository<ActuacionRelevanteDGPCE>().UpdateEntity(It.IsAny<ActuacionRelevanteDGPCE>()), Times.Once);
        _unitOfWorkMock.Verify(uow => uow.Complete(), Times.Once);
        Assert.Equal(1, response.IdActuacionRelevante);
    }



    [Fact]
    public async Task Handle_WithValidRequestAndNewConvocatoriaCECOD_ShouldAddConvocatoriaCECOD()
    {
        // Arrange
        var command = new ManageConvocatoriaCECODCommand
        {
            IdSuceso = 1,
            Detalles = new List<ManageConvocatoriaCECODDto>
        {
            new ManageConvocatoriaCECODDto
            {
                Id = 0,
                FechaInicio = new DateOnly(2022, 1, 1),
                FechaFin = new DateOnly(2022, 1, 10),
                Lugar = "Sede PCYE",
                Convocados = "10",
                Participantes = "5",
                Observaciones = "Observaciones 1 ACTUALIZADO"
            }
        }
        };

        var suceso = new Suceso
        {
            Id = 1,
            Borrado = false
        };

        var convocatoriaCECODDto = command.Detalles.First();
        var convocatoriaCECOD = new ConvocatoriaCECOD
        {
            Id = 0,
            FechaInicio = convocatoriaCECODDto.FechaInicio,
            FechaFin = convocatoriaCECODDto.FechaFin,
            Lugar = convocatoriaCECODDto.Lugar,
            Convocados = convocatoriaCECODDto.Convocados,
            Participantes = convocatoriaCECODDto.Participantes,
            Observaciones = convocatoriaCECODDto.Observaciones
        };

        _unitOfWorkMock.Setup(uow => uow.Repository<Suceso>().GetByIdAsync(It.IsAny<int>()))
            .ReturnsAsync(suceso);

        _mapperMock.Setup(m => m.Map<ConvocatoriaCECOD>(It.IsAny<ManageConvocatoriaCECODDto>()))
            .Returns(convocatoriaCECOD);

        _unitOfWorkMock.Setup(uow => uow.Repository<ActuacionRelevanteDGPCE>().AddEntity(It.IsAny<ActuacionRelevanteDGPCE>()))
            .Callback<ActuacionRelevanteDGPCE>(actuacionRelevanteDGPCE =>
            {
                actuacionRelevanteDGPCE.Id = 1;
            });

        _unitOfWorkMock.Setup(u => u.Complete())
            .ReturnsAsync(1);

        var handler = new ManageConvocatoriaCECODCommandHandler(_loggerMock.Object, _unitOfWorkMock.Object, _mapperMock.Object, _registroActualizacionMock.Object);

        // Act
        var response = await handler.Handle(command, CancellationToken.None);

        // Assert
        _unitOfWorkMock.Verify(uow => uow.Repository<ActuacionRelevanteDGPCE>().AddEntity(It.IsAny<ActuacionRelevanteDGPCE>()), Times.Once);
        _unitOfWorkMock.Verify(uow => uow.Complete(), Times.Once);
        Assert.Equal(1, response.IdActuacionRelevante);
    }


    [Fact]
    public async Task Handle_WithInvalidIdActuacionRelevante_ShouldThrowNotFoundException()
    {
        // Arrange
        var command = new ManageConvocatoriaCECODCommand
        {
            IdRegistroActualizacion = 1,
            IdSuceso = 1,
            Detalles = new List<ManageConvocatoriaCECODDto>
        {
            new ManageConvocatoriaCECODDto
            {
                Id = 1,
                FechaInicio = new DateOnly(2022, 1, 1),
                FechaFin = new DateOnly(2022, 1, 10),
                Lugar = "Sede PCYE",
                Convocados = "10",
                Participantes = "5",
                Observaciones = "Observaciones 1 ACTUALIZADO"
            }
        }
        };

        _unitOfWorkMock.Setup(uow => uow.Repository<ActuacionRelevanteDGPCE>().GetByIdWithSpec(It.IsAny<ActuacionRelevanteDGPCESpecification>()))
            .ReturnsAsync((ActuacionRelevanteDGPCE)null);

        var handler = new ManageConvocatoriaCECODCommandHandler(_loggerMock.Object, _unitOfWorkMock.Object, _mapperMock.Object, _registroActualizacionMock.Object);

        // Act & Assert
        await Assert.ThrowsAsync<NotFoundException>(() => handler.Handle(command, CancellationToken.None));
    }

    [Fact]
    public async Task Handle_WithValidRequestAndExistingActuacionRelevante_ShouldDeleteDeclaracionZAGEP()
    {
        // Arrange
        var command = new ManageConvocatoriaCECODCommand
        {
            IdRegistroActualizacion = 1,
            IdSuceso = 1,
            Detalles = new()
        };

        var actuacionRelevante = new ActuacionRelevanteDGPCE
        {
            Id = 1,
            ConvocatoriasCECOD = new List<ConvocatoriaCECOD>
        {
            new ConvocatoriaCECOD
            {
                Id = 1,
                FechaInicio = new DateOnly(2022, 1, 1),
                FechaFin = new DateOnly(2022, 1, 10),
                Lugar = "Sede PCYE",
                Convocados = "10",
                Participantes = "5",
                Observaciones = "Observaciones 1 ACTUALIZADO"
            }
        }
        };

        _unitOfWorkMock.Setup(uow => uow.Repository<ActuacionRelevanteDGPCE>().GetByIdWithSpec(It.IsAny<ActuacionRelevanteDGPCESpecification>()))
            .ReturnsAsync(actuacionRelevante);

        _unitOfWorkMock.Setup(uow => uow.Repository<ConvocatoriaCECOD>().DeleteEntity(It.IsAny<ConvocatoriaCECOD>()))
            .Verifiable();

        _unitOfWorkMock.Setup(u => u.Complete())
            .ReturnsAsync(1); // Asegúrate de que Complete devuelva un valor mayor que 0

        var handler = new ManageConvocatoriaCECODCommandHandler(_loggerMock.Object, _unitOfWorkMock.Object, _mapperMock.Object, _registroActualizacionMock.Object);

        // Act
        var response = await handler.Handle(command, CancellationToken.None);

        // Assert
        _unitOfWorkMock.Verify(uow => uow.Repository<ConvocatoriaCECOD>().DeleteEntity(It.IsAny<ConvocatoriaCECOD>()), Times.Once);
        _unitOfWorkMock.Verify(uow => uow.Repository<ActuacionRelevanteDGPCE>().UpdateEntity(It.IsAny<ActuacionRelevanteDGPCE>()), Times.Once);
        _unitOfWorkMock.Verify(uow => uow.Complete(), Times.Once);
        Assert.Equal(1, response.IdActuacionRelevante);
    }

}

