using AutoMapper;
using DGPCE.Sigemad.Application.Contracts.Persistence;
using DGPCE.Sigemad.Application.Contracts.RegistrosActualizacion;
using DGPCE.Sigemad.Application.Exceptions;
using DGPCE.Sigemad.Application.Features.Evoluciones.Commands.ManageEvoluciones;
using DGPCE.Sigemad.Application.Features.Parametros.Commands;
using DGPCE.Sigemad.Application.Features.Parametros.Manage;
using DGPCE.Sigemad.Application.Features.Registros.Command.CreateRegistros;
using DGPCE.Sigemad.Application.Mappings;
using DGPCE.Sigemad.Application.Specifications;
using DGPCE.Sigemad.Domain.Modelos;
using Microsoft.Extensions.Logging;
using Moq;
using System.Linq.Expressions;

namespace DGPCE.Sigemad.Application.Tests.Features.Evoluciones.Commands;


public class ManageEvolucionCommandHandlerTests
{
    private readonly Mock<ILogger<ManageParametroCommandHandler>> _loggerMock;
    private readonly Mock<IUnitOfWork> _unitOfWorkMock;
    private readonly ManageParametroCommandHandler _handler;
    private readonly Mock<IRegistroActualizacionService> _registroActualizacionServiceMock;

    public ManageEvolucionCommandHandlerTests()
    {
        _loggerMock = new Mock<ILogger<ManageParametroCommandHandler>>();
        _unitOfWorkMock = new Mock<IUnitOfWork>();
        _registroActualizacionServiceMock = new Mock<IRegistroActualizacionService>();

        var config = new MapperConfiguration(cfg =>
        {
            cfg.AddProfile<MappingProfile>();
        });
        var mapper = config.CreateMapper();

        _handler = new ManageParametroCommandHandler(
            _loggerMock.Object, 
            _unitOfWorkMock.Object, 
            mapper,
            _registroActualizacionServiceMock.Object);
    }

    [Fact]
    public async Task Handle_ValidRequest_AddsNewEvolucion()
    {
        // Arrange
        var command = new ManageParametroCommand
        {
            IdSuceso = 1,
            Registro = new CreateRegistroCommand
            {
                IdMedio = 1,
                IdEntradaSalida = 1,
                RegistroProcedenciasDestinos = new List<int> { 1, 2 }
            },
            Parametro = new CreateParametroCommand
            {
                IdEstadoIncendio = 1,
                IdFaseEmergencia = 1,
                IdPlanSituacion = 1
            }
        };

        var suceso = new Suceso { Id = 1, Borrado = false };
        var medio = new Medio { Id = 1 };
        var entradaSalida = new EntradaSalida { Id = 1 };
        var procedenciaDestino = new List<ProcedenciaDestino>
            {
                new ProcedenciaDestino { Id = 1 },
                new ProcedenciaDestino { Id = 2 }
            };
        var estadoIncendio = new EstadoIncendio { Id = 1, Obsoleto = false };
        var faseEmergencia = new FaseEmergencia { Id = 1 };
        var planSituacion = new PlanSituacion { Id = 1 };

        _unitOfWorkMock.Setup(uow => uow.Repository<Suceso>().GetByIdAsync(command.IdSuceso))
            .ReturnsAsync(suceso);
        _unitOfWorkMock.Setup(uow => uow.Repository<Medio>().GetByIdAsync(command.Registro.IdMedio.Value))
            .ReturnsAsync(medio);
        _unitOfWorkMock.Setup(uow => uow.Repository<EntradaSalida>().GetByIdAsync(command.Registro.IdEntradaSalida.Value))
            .ReturnsAsync(entradaSalida);
        _unitOfWorkMock.Setup(uow => uow.Repository<ProcedenciaDestino>().GetAsync(It.IsAny<Expression<Func<ProcedenciaDestino, bool>>>()))
            .ReturnsAsync(procedenciaDestino);
        _unitOfWorkMock.Setup(uow => uow.Repository<EstadoIncendio>().GetByIdAsync(command.Parametro.IdEstadoIncendio))
            .ReturnsAsync(estadoIncendio);
        _unitOfWorkMock.Setup(uow => uow.Repository<FaseEmergencia>().GetByIdAsync(command.Parametro.IdFaseEmergencia.Value))
            .ReturnsAsync(faseEmergencia);
        _unitOfWorkMock.Setup(uow => uow.Repository<PlanSituacion>().GetByIdAsync(command.Parametro.IdPlanSituacion.Value))
            .ReturnsAsync(planSituacion);
        _unitOfWorkMock.Setup(uow => uow.Repository<Evolucion>().AddEntity(It.IsAny<Evolucion>()))
        .Callback<Evolucion>(sr => sr.Id = 1); // Set the Id to a non-zero value
        _unitOfWorkMock.Setup(uow => uow.Complete()).ReturnsAsync(1);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.NotNull(result);
        Assert.True(result.Id > 0);
        _unitOfWorkMock.Verify(uow => uow.Repository<Evolucion>().AddEntity(It.IsAny<Evolucion>()), Times.Once);
        _unitOfWorkMock.Verify(uow => uow.Complete(), Times.Once);
    }

    [Fact]
    public async Task Handle_InvalidSucesoId_ThrowsNotFoundException()
    {
        // Arrange
        var command = new ManageParametroCommand
        {
            IdSuceso = 1
        };

        _unitOfWorkMock.Setup(uow => uow.Repository<Suceso>().GetByIdAsync(command.IdSuceso))
            .ReturnsAsync((Suceso)null);

        // Act & Assert
        await Assert.ThrowsAsync<NotFoundException>(() => _handler.Handle(command, CancellationToken.None));
    }

    [Fact]
    public async Task Handle_InvalidMedioId_ThrowsNotFoundException()
    {
        // Arrange
        var command = new ManageParametroCommand
        {
            IdSuceso = 1,
            Registro = new CreateRegistroCommand
            {
                IdMedio = 1
            }
        };

        var suceso = new Suceso { Id = 1, Borrado = false };

        _unitOfWorkMock.Setup(uow => uow.Repository<Suceso>().GetByIdAsync(command.IdSuceso))
            .ReturnsAsync(suceso);
        _unitOfWorkMock.Setup(uow => uow.Repository<Medio>().GetByIdAsync(command.Registro.IdMedio.Value))
            .ReturnsAsync((Medio)null);

        // Act & Assert
        await Assert.ThrowsAsync<NotFoundException>(() => _handler.Handle(command, CancellationToken.None));
    }

    [Fact]
    public async Task Handle_InvalidEntradaSalidaId_ThrowsNotFoundException()
    {
        // Arrange
        var command = new ManageParametroCommand
        {
            IdSuceso = 1,
            Registro = new CreateRegistroCommand
            {
                IdEntradaSalida = 1
            }
        };

        var suceso = new Suceso { Id = 1, Borrado = false };

        _unitOfWorkMock.Setup(uow => uow.Repository<Suceso>().GetByIdAsync(command.IdSuceso))
            .ReturnsAsync(suceso);
        _unitOfWorkMock.Setup(uow => uow.Repository<EntradaSalida>().GetByIdAsync(command.Registro.IdEntradaSalida.Value))
            .ReturnsAsync((EntradaSalida)null);

        // Act & Assert
        await Assert.ThrowsAsync<NotFoundException>(() => _handler.Handle(command, CancellationToken.None));
    }

    [Fact]
    public async Task Handle_InvalidProcedenciaDestinoId_ThrowsNotFoundException()
    {
        // Arrange
        var command = new ManageParametroCommand
        {
            IdSuceso = 1,
            Registro = new CreateRegistroCommand
            {
                RegistroProcedenciasDestinos = new List<int> { 1, 2 }
            }
        };

        var suceso = new Suceso { Id = 1, Borrado = false };
        var procedenciaDestino = new List<ProcedenciaDestino>
            {
                new ProcedenciaDestino { Id = 1 }
            };

        _unitOfWorkMock.Setup(uow => uow.Repository<Suceso>().GetByIdAsync(command.IdSuceso))
            .ReturnsAsync(suceso);
        _unitOfWorkMock.Setup(uow => uow.Repository<ProcedenciaDestino>().GetAsync(It.IsAny<Expression<Func<ProcedenciaDestino, bool>>>()))
            .ReturnsAsync(procedenciaDestino);

        // Act & Assert
        await Assert.ThrowsAsync<NotFoundException>(() => _handler.Handle(command, CancellationToken.None));
    }

    [Fact]
    public async Task Handle_InvalidEstadoIncendioId_ThrowsNotFoundException()
    {
        // Arrange
        var command = new ManageParametroCommand
        {
            IdSuceso = 1,
            Parametro = new CreateParametroCommand
            {
                IdEstadoIncendio = 1
            }
        };

        var suceso = new Suceso { Id = 1, Borrado = false };

        _unitOfWorkMock.Setup(uow => uow.Repository<Suceso>().GetByIdAsync(command.IdSuceso))
            .ReturnsAsync(suceso);
        _unitOfWorkMock.Setup(uow => uow.Repository<EstadoIncendio>().GetByIdAsync(command.Parametro.IdEstadoIncendio))
            .ReturnsAsync((EstadoIncendio)null);

        // Act & Assert
        await Assert.ThrowsAsync<NotFoundException>(() => _handler.Handle(command, CancellationToken.None));
    }

    [Fact]
    public async Task Handle_InvalidFaseEmergenciaId_ThrowsNotFoundException()
    {
        // Arrange
        var command = new ManageParametroCommand
        {
            IdSuceso = 1,
            Parametro = new CreateParametroCommand
            {
                IdEstadoIncendio = 1,
                IdFaseEmergencia = 1
            }
        };

        var suceso = new Suceso { Id = 1, Borrado = false };

        _unitOfWorkMock.Setup(uow => uow.Repository<Suceso>().GetByIdAsync(command.IdSuceso))
            .ReturnsAsync(suceso);
        _unitOfWorkMock.Setup(uow => uow.Repository<EstadoIncendio>().GetByIdAsync(command.Parametro.IdEstadoIncendio))
            .ReturnsAsync(new EstadoIncendio { Id = 1, Descripcion = "Activo"});
        _unitOfWorkMock.Setup(uow => uow.Repository<FaseEmergencia>().GetByIdAsync(command.Parametro.IdFaseEmergencia.Value))
            .ReturnsAsync((FaseEmergencia)null);

        // Act & Assert
        await Assert.ThrowsAsync<NotFoundException>(() => _handler.Handle(command, CancellationToken.None));
    }

    [Fact]
    public async Task Handle_InvalidPlanSituacionId_ThrowsNotFoundException()
    {
        // Arrange
        var command = new ManageParametroCommand
        {
            IdSuceso = 1,
            Parametro = new CreateParametroCommand
            {
                IdEstadoIncendio = 1,
                IdFaseEmergencia = 1,
                IdPlanSituacion = 1
            }
        };

        var suceso = new Suceso { Id = 1, Borrado = false };

        _unitOfWorkMock.Setup(uow => uow.Repository<Suceso>().GetByIdAsync(command.IdSuceso))
            .ReturnsAsync(suceso);
        _unitOfWorkMock.Setup(uow => uow.Repository<EstadoIncendio>().GetByIdAsync(command.Parametro.IdEstadoIncendio))
            .ReturnsAsync(new EstadoIncendio { Id = 1, Descripcion = "Activo" });
        _unitOfWorkMock.Setup(uow => uow.Repository<FaseEmergencia>().GetByIdAsync(command.Parametro.IdFaseEmergencia.Value))
            .ReturnsAsync(new FaseEmergencia { Id=1, IdPlanEmergencia = 1, Orden = 1, Descripcion = "Alerta y Seguimiento"});
        _unitOfWorkMock.Setup(uow => uow.Repository<PlanSituacion>().GetByIdAsync(command.Parametro.IdPlanSituacion.Value))
            .ReturnsAsync((PlanSituacion)null);

        // Act & Assert
        await Assert.ThrowsAsync<NotFoundException>(() => _handler.Handle(command, CancellationToken.None));
    }

    [Fact]
    public async Task Handle_UpdateExistingEvolucion()
    {
        // Arrange
        var command = new ManageParametroCommand
        {
            //IdEvolucion = 1,
            IdSuceso = 1,
            Registro = new CreateRegistroCommand
            {
                IdMedio = 1,
                IdEntradaSalida = 1,
                RegistroProcedenciasDestinos = new List<int> { 1, 2 }
            },
            Parametro = new CreateParametroCommand
            {
                IdEstadoIncendio = 1,
                IdFaseEmergencia = 1,
                IdPlanSituacion = 1
            }
        };

        var evolucion = new Evolucion
        {
            Id = 1,
            IdSuceso = 1
        };

        var suceso = new Suceso { Id = 1, Borrado = false };
        var medio = new Medio { Id = 1 };
        var entradaSalida = new EntradaSalida { Id = 1 };
        var procedenciaDestino = new List<ProcedenciaDestino>
            {
                new ProcedenciaDestino { Id = 1 },
                new ProcedenciaDestino { Id = 2 }
            };
        var estadoIncendio = new EstadoIncendio { Id = 1, Obsoleto = false };
        var faseEmergencia = new FaseEmergencia { Id = 1 };
        var planSituacion = new PlanSituacion { Id = 1 };

        _unitOfWorkMock.Setup(uow => uow.Repository<Evolucion>().GetByIdWithSpec(It.IsAny<ISpecification<Evolucion>>()))
            .ReturnsAsync(evolucion);
        _unitOfWorkMock.Setup(uow => uow.Repository<Suceso>().GetByIdAsync(command.IdSuceso))
            .ReturnsAsync(suceso);
        _unitOfWorkMock.Setup(uow => uow.Repository<Medio>().GetByIdAsync(command.Registro.IdMedio.Value))
            .ReturnsAsync(medio);
        _unitOfWorkMock.Setup(uow => uow.Repository<EntradaSalida>().GetByIdAsync(command.Registro.IdEntradaSalida.Value))
            .ReturnsAsync(entradaSalida);
        _unitOfWorkMock.Setup(uow => uow.Repository<ProcedenciaDestino>().GetAsync(It.IsAny<Expression<Func<ProcedenciaDestino, bool>>>()))
            .ReturnsAsync(procedenciaDestino);
        _unitOfWorkMock.Setup(uow => uow.Repository<EstadoIncendio>().GetByIdAsync(command.Parametro.IdEstadoIncendio))
            .ReturnsAsync(estadoIncendio);
        _unitOfWorkMock.Setup(uow => uow.Repository<FaseEmergencia>().GetByIdAsync(command.Parametro.IdFaseEmergencia.Value))
            .ReturnsAsync(faseEmergencia);
        _unitOfWorkMock.Setup(uow => uow.Repository<PlanSituacion>().GetByIdAsync(command.Parametro.IdPlanSituacion.Value))
            .ReturnsAsync(planSituacion);
        _unitOfWorkMock.Setup(uow => uow.Repository<Evolucion>().UpdateEntity(It.IsAny<Evolucion>()));
        _unitOfWorkMock.Setup(uow => uow.Complete()).ReturnsAsync(1);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(evolucion.Id, result.Id);
        _unitOfWorkMock.Verify(uow => uow.Repository<Evolucion>().UpdateEntity(It.IsAny<Evolucion>()), Times.Once);
        _unitOfWorkMock.Verify(uow => uow.Complete(), Times.Once);
    }

    [Fact]
    public async Task Handle_ShouldValidateRegistro()
    {
        // Arrange
        var command = new ManageParametroCommand
        {
            IdSuceso = 1,
            Registro = new CreateRegistroCommand { IdMedio = 1, IdEntradaSalida = 1 }
        };

        var medio = new Medio { Id = 1 };
        var entradaSalida = new EntradaSalida { Id = 1 };
        var procedenciaDestino = new List<ProcedenciaDestino>
            {
                new ProcedenciaDestino { Id = 1 },
                new ProcedenciaDestino { Id = 2 }
            };
        var suceso = new Suceso { Id = 1, Borrado = false };

        _unitOfWorkMock.Setup(uow => uow.Repository<Medio>().GetByIdAsync(command.Registro.IdMedio.Value))
            .ReturnsAsync(medio);
        _unitOfWorkMock.Setup(uow => uow.Repository<EntradaSalida>().GetByIdAsync(command.Registro.IdEntradaSalida.Value))
            .ReturnsAsync(entradaSalida);
        _unitOfWorkMock.Setup(uow => uow.Repository<ProcedenciaDestino>().GetAsync(It.IsAny<Expression<Func<ProcedenciaDestino, bool>>>()))
            .ReturnsAsync(procedenciaDestino);
        _unitOfWorkMock.Setup(uow => uow.Repository<Suceso>().GetByIdAsync(command.IdSuceso))
            .ReturnsAsync(suceso);
        _unitOfWorkMock.Setup(uow => uow.Repository<Evolucion>().AddEntity(It.IsAny<Evolucion>()))
        .Callback<Evolucion>(sr => sr.Id = 1); // Set the Id to a non-zero value
        _unitOfWorkMock.Setup(uow => uow.Complete()).ReturnsAsync(1);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.NotNull(result);
        _unitOfWorkMock.Verify(uow => uow.Repository<Medio>().GetByIdAsync(command.Registro.IdMedio.Value), Times.Once);
        _unitOfWorkMock.Verify(uow => uow.Repository<EntradaSalida>().GetByIdAsync(command.Registro.IdEntradaSalida.Value), Times.Once);
    }

    [Fact]
    public async Task Handle_ShouldValidateParametros()
    {
        // Arrange
        var command = new ManageParametroCommand
        {
            IdSuceso = 1,
            Parametro = new CreateParametroCommand { IdEstadoIncendio = 1, IdFaseEmergencia = 1, IdPlanSituacion = 1, IdSituacionEquivalente = 1 }
        };

        var estadoIncendio = new EstadoIncendio { Id = 1, Obsoleto = false };
        var faseEmergencia = new FaseEmergencia { Id = 1 };
        var planSituacion = new PlanSituacion { Id = 1 };
        var situacionEquivalente = new SituacionEquivalente { Id = 1, Obsoleto = false };
        var procedenciaDestino = new List<ProcedenciaDestino>
            {
                new ProcedenciaDestino { Id = 1 },
                new ProcedenciaDestino { Id = 2 }
            };
        var suceso = new Suceso { Id = 1, Borrado = false };

        _unitOfWorkMock.Setup(uow => uow.Repository<EstadoIncendio>().GetByIdAsync(command.Parametro.IdEstadoIncendio))
            .ReturnsAsync(estadoIncendio);
        _unitOfWorkMock.Setup(uow => uow.Repository<FaseEmergencia>().GetByIdAsync(command.Parametro.IdFaseEmergencia.Value))
            .ReturnsAsync(faseEmergencia);
        _unitOfWorkMock.Setup(uow => uow.Repository<PlanSituacion>().GetByIdAsync(command.Parametro.IdPlanSituacion.Value))
            .ReturnsAsync(planSituacion);
        _unitOfWorkMock.Setup(uow => uow.Repository<SituacionEquivalente>().GetByIdAsync(command.Parametro.IdSituacionEquivalente.Value))
            .ReturnsAsync(situacionEquivalente);
        _unitOfWorkMock.Setup(uow => uow.Repository<ProcedenciaDestino>().GetAsync(It.IsAny<Expression<Func<ProcedenciaDestino, bool>>>()))
            .ReturnsAsync(procedenciaDestino);
        _unitOfWorkMock.Setup(uow => uow.Repository<Suceso>().GetByIdAsync(command.IdSuceso))
            .ReturnsAsync(suceso);
        _unitOfWorkMock.Setup(uow => uow.Repository<Evolucion>().AddEntity(It.IsAny<Evolucion>()))
        .Callback<Evolucion>(sr => sr.Id = 1); // Set the Id to a non-zero value
        _unitOfWorkMock.Setup(uow => uow.Complete()).ReturnsAsync(1);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.NotNull(result);
        _unitOfWorkMock.Verify(uow => uow.Repository<EstadoIncendio>().GetByIdAsync(command.Parametro.IdEstadoIncendio), Times.Once);
        _unitOfWorkMock.Verify(uow => uow.Repository<FaseEmergencia>().GetByIdAsync(command.Parametro.IdFaseEmergencia.Value), Times.Once);
        _unitOfWorkMock.Verify(uow => uow.Repository<PlanSituacion>().GetByIdAsync(command.Parametro.IdPlanSituacion.Value), Times.Once);
        _unitOfWorkMock.Verify(uow => uow.Repository<SituacionEquivalente>().GetByIdAsync(command.Parametro.IdSituacionEquivalente.Value), Times.Once);
    }
}

