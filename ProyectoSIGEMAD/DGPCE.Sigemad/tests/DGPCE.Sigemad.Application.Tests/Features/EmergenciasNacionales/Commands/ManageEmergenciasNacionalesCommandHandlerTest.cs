using AutoMapper;
using DGPCE.Sigemad.Application.Contracts.Persistence;
using DGPCE.Sigemad.Application.Contracts.RegistrosActualizacion;
using DGPCE.Sigemad.Application.Dtos.EmergenciasNacionales;
using DGPCE.Sigemad.Application.Features.EmergenciasNacionales.Commands.ManageEmergenciasNacionales;
using DGPCE.Sigemad.Application.Specifications;
using DGPCE.Sigemad.Domain.Modelos;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using Moq;

namespace DGPCE.Sigemad.Application.Tests.Features.EmergenciasNacionales.Commands;
public class ManageEmergenciasNacionalesCommandHandlerTests
{
    private readonly Mock<ILogger<ManageEmergenciasNacionalesCommandHandler>> _loggerMock;
    private readonly Mock<IUnitOfWork> _unitOfWorkMock;
    private readonly Mock<IMapper> _mapperMock;
    private readonly ManageEmergenciasNacionalesCommandHandler _handler;
    private readonly Mock<IRegistroActualizacionService> _registroActualizacionMock;

    public ManageEmergenciasNacionalesCommandHandlerTests()
    {
        _loggerMock = new Mock<ILogger<ManageEmergenciasNacionalesCommandHandler>>();
        _unitOfWorkMock = new Mock<IUnitOfWork>();
        _mapperMock = new Mock<IMapper>();
        _registroActualizacionMock = new Mock<IRegistroActualizacionService>();

        _handler = new ManageEmergenciasNacionalesCommandHandler(_loggerMock.Object, _unitOfWorkMock.Object, _mapperMock.Object, _registroActualizacionMock.Object);
    }

    [Fact]
    public async Task Handle_ShouldCreateNewActuacionRelevante_WhenIdActuacionRelevanteIsNull()
    {
        // Arrange
        var command = new ManageEmergenciasNacionalesCommand
        {
            IdRegistroActualizacion = null,
            IdSuceso = 1,
            EmergenciaNacional = new ManageEmergenciaNacionalDto
            {
                Autoridad = "ESDJM4555",
                DescripcionSolicitud = "ESDJM4555",
                FechaHoraSolicitud = new DateTime(2024, 11, 28, 3, 22, 24, 857, DateTimeKind.Utc),
                FechaHoraDeclaracion = new DateTime(2024, 11, 28, 3, 22, 24, 857, DateTimeKind.Utc),
                DescripcionDeclaracion = "ESDJM4555",
                FechaHoraDireccion = new DateTime(2024, 11, 28, 3, 22, 24, 857, DateTimeKind.Utc),
                Observaciones = "Nombre 123"
            }
        };
        var suceso = new Suceso { Id = 1, Borrado = false };

        // Configura los mocks
        _unitOfWorkMock.Setup(uow => uow.Repository<Suceso>().GetByIdAsync(command.IdSuceso))
            .ReturnsAsync(suceso);

        _unitOfWorkMock.Setup(uow => uow.Repository<ActuacionRelevanteDGPCE>().AddEntity(It.IsAny<ActuacionRelevanteDGPCE>()))
            .Callback<ActuacionRelevanteDGPCE>(actuacionRelevanteDGPCE =>
            {
                actuacionRelevanteDGPCE.Id = 1;
            });

        _unitOfWorkMock.Setup(u => u.Complete())
            .ReturnsAsync(1);

        _mapperMock.Setup(m => m.Map(It.IsAny<ManageEmergenciasNacionalesCommand>(), It.IsAny<ActuacionRelevanteDGPCE>()))
            .Callback((ManageEmergenciasNacionalesCommand src, ActuacionRelevanteDGPCE dest) =>
            {
                dest.EmergenciaNacional = new EmergenciaNacional
                {
                    Autoridad = src.EmergenciaNacional.Autoridad,
                    DescripcionSolicitud = src.EmergenciaNacional.DescripcionSolicitud,
                    FechaHoraSolicitud = src.EmergenciaNacional.FechaHoraSolicitud,
                    FechaHoraDeclaracion = src.EmergenciaNacional.FechaHoraDeclaracion,
                    DescripcionDeclaracion = src.EmergenciaNacional.DescripcionDeclaracion,
                    FechaHoraDireccion = src.EmergenciaNacional.FechaHoraDireccion,
                    Observaciones = src.EmergenciaNacional.Observaciones,
                    Borrado = false,
                    FechaEliminacion = null,
                    EliminadoPor = null
                };
            });

        var handler = new ManageEmergenciasNacionalesCommandHandler(_loggerMock.Object, _unitOfWorkMock.Object, _mapperMock.Object, _registroActualizacionMock.Object);

        // Act
        var result = await handler.Handle(command, CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result.Should().BeOfType<ManageEmergenciaNacionalResponse>();
        result.IdActuacionRelevante.Should().Be(1);

        // Verifica que se haya agregado la nueva actuación relevante
        _unitOfWorkMock.Verify(uow => uow.Repository<ActuacionRelevanteDGPCE>().AddEntity(It.IsAny<ActuacionRelevanteDGPCE>()), Times.Once);
        // Verifica que se haya llamado al método Complete
        _unitOfWorkMock.Verify(uow => uow.Complete(), Times.Once);
    }


    [Fact]
    public async Task Handle_ShouldUpdateExistingActuacionRelevante_WhenIdActuacionRelevanteIsProvided()
    {
        // Arrange
        var command = new ManageEmergenciasNacionalesCommand
        {
            IdRegistroActualizacion = 2,
            IdSuceso = 1,
            EmergenciaNacional = new ManageEmergenciaNacionalDto
            {
                Autoridad = "ESDJM4555",
                DescripcionSolicitud = "ESDJM4555",
                FechaHoraSolicitud = new DateTime(2024, 11, 28, 3, 22, 24, 857, DateTimeKind.Utc),
                FechaHoraDeclaracion = new DateTime(2024, 11, 28, 3, 22, 24, 857, DateTimeKind.Utc),
                DescripcionDeclaracion = "ESDJM4555",
                FechaHoraDireccion = new DateTime(2024, 11, 28, 3, 22, 24, 857, DateTimeKind.Utc),
                Observaciones = "Nombre 123"
            }
        };

        var actuacion = new ActuacionRelevanteDGPCE { Id = 2, Borrado = false };

        // Configura los mocks
        _unitOfWorkMock.Setup(uow => uow.Repository<ActuacionRelevanteDGPCE>().GetByIdWithSpec(It.IsAny<ISpecification<ActuacionRelevanteDGPCE>>()))
            .ReturnsAsync(actuacion);

        _unitOfWorkMock.Setup(uow => uow.Repository<ActuacionRelevanteDGPCE>().UpdateEntity(It.IsAny<ActuacionRelevanteDGPCE>()))
            .Callback<ActuacionRelevanteDGPCE>(actuacionRelevanteDGPCE =>
            {
                actuacionRelevanteDGPCE.Id = 2;
            });

        _unitOfWorkMock.Setup(u => u.Complete())
            .ReturnsAsync(1);

        _mapperMock.Setup(m => m.Map(It.IsAny<ManageEmergenciasNacionalesCommand>(), It.IsAny<ActuacionRelevanteDGPCE>()))
            .Callback((ManageEmergenciasNacionalesCommand src, ActuacionRelevanteDGPCE dest) =>
            {
                dest.EmergenciaNacional = new EmergenciaNacional
                {
                    Autoridad = src.EmergenciaNacional.Autoridad,
                    DescripcionSolicitud = src.EmergenciaNacional.DescripcionSolicitud,
                    FechaHoraSolicitud = src.EmergenciaNacional.FechaHoraSolicitud,
                    FechaHoraDeclaracion = src.EmergenciaNacional.FechaHoraDeclaracion,
                    DescripcionDeclaracion = src.EmergenciaNacional.DescripcionDeclaracion,
                    FechaHoraDireccion = src.EmergenciaNacional.FechaHoraDireccion,
                    Observaciones = src.EmergenciaNacional.Observaciones,
                    Borrado = false,
                    FechaEliminacion = null,
                    EliminadoPor = null
                };
            });


        var handler = new ManageEmergenciasNacionalesCommandHandler(_loggerMock.Object, _unitOfWorkMock.Object, _mapperMock.Object, _registroActualizacionMock.Object);

        // Act
        var result = await handler.Handle(command, CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result.Should().BeOfType<ManageEmergenciaNacionalResponse>();
        result.IdActuacionRelevante.Should().Be(2);

        // Verifica que se haya actualizado la actuación relevante existente
        _unitOfWorkMock.Verify(uow => uow.Repository<ActuacionRelevanteDGPCE>().UpdateEntity(It.IsAny<ActuacionRelevanteDGPCE>()), Times.Once);
        // Verifica que se haya llamado al método Complete
        _unitOfWorkMock.Verify(uow => uow.Complete(), Times.Once);
    }

    [Fact]
    public async Task Handle_ShouldLogicallyDeleteEmergenciaNacional_WhenNotProvidedInRequest()
    {
        // Arrange
        var command = new ManageEmergenciasNacionalesCommand { IdRegistroActualizacion = 1, IdSuceso = 1 };
        var actuacion = new ActuacionRelevanteDGPCE
        {
            Id = 1,
            IdSuceso = 1,
            EmergenciaNacional = new EmergenciaNacional
            {
                Autoridad = "ESDJM4555",
                DescripcionSolicitud = "ESDJM4555",
                FechaHoraSolicitud = new DateTime(2024, 11, 28, 3, 22, 24, 857, DateTimeKind.Utc),
                FechaHoraDeclaracion = new DateTime(2024, 11, 28, 3, 22, 24, 857, DateTimeKind.Utc),
                DescripcionDeclaracion = "ESDJM4555",
                FechaHoraDireccion = new DateTime(2024, 11, 28, 3, 22, 24, 857, DateTimeKind.Utc),
                Observaciones = "Nombre 123",
                Borrado = false
            }
        };

        _unitOfWorkMock.Setup(uow => uow.Repository<ActuacionRelevanteDGPCE>().GetByIdWithSpec(It.IsAny<ISpecification<ActuacionRelevanteDGPCE>>()))
            .ReturnsAsync(actuacion);

        _unitOfWorkMock.Setup(uow => uow.Repository<EmergenciaNacional>().DeleteEntity(It.IsAny<EmergenciaNacional>()))
            .Verifiable();

        _unitOfWorkMock.Setup(uow => uow.Repository<ActuacionRelevanteDGPCE>().UpdateEntity(It.IsAny<ActuacionRelevanteDGPCE>()))
            .Callback<ActuacionRelevanteDGPCE>(actuacionRelevanteDGPCE =>
            {
                actuacionRelevanteDGPCE.Id = 1;
            });

        _unitOfWorkMock.Setup(u => u.Complete())
            .ReturnsAsync(1);

        var handler = new ManageEmergenciasNacionalesCommandHandler(_loggerMock.Object, _unitOfWorkMock.Object, _mapperMock.Object, _registroActualizacionMock.Object);

        // Act
        var result = await handler.Handle(command, CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result.Should().BeOfType<ManageEmergenciaNacionalResponse>();
        result.IdActuacionRelevante.Should().Be(1);

        // Verifica que se haya eliminado lógicamente la emergencia nacional
        _unitOfWorkMock.Verify(uow => uow.Repository<EmergenciaNacional>().DeleteEntity(It.IsAny<EmergenciaNacional>()), Times.Once);
        // Verifica que se haya actualizado la actuación relevante
        _unitOfWorkMock.Verify(uow => uow.Repository<ActuacionRelevanteDGPCE>().UpdateEntity(It.IsAny<ActuacionRelevanteDGPCE>()), Times.Once);
        // Verifica que se haya llamado al método Complete
        _unitOfWorkMock.Verify(u => u.Complete(), Times.Once);
    }

}
