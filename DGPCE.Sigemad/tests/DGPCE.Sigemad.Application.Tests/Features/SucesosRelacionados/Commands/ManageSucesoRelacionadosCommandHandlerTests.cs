using AutoMapper;
using DGPCE.Sigemad.Application.Contracts.Persistence;
using DGPCE.Sigemad.Application.Contracts.RegistrosActualizacion;
using DGPCE.Sigemad.Application.Exceptions;
using DGPCE.Sigemad.Application.Features.SucesosRelacionados.Commands.ManageSucesoRelacionados;
using DGPCE.Sigemad.Application.Specifications;
using DGPCE.Sigemad.Domain.Modelos;
using Microsoft.Extensions.Logging;
using Moq;
using System.Linq.Expressions;

namespace DGPCE.Sigemad.Application.Tests.Features.SucesosRelacionados.Commands.ManageSucesoRelacionados;

public class ManageSucesoRelacionadosCommandHandlerTests
{
    private readonly Mock<ILogger<ManageSucesoRelacionadosCommandHandler>> _loggerMock;
    private readonly Mock<IUnitOfWork> _unitOfWorkMock;
    private readonly Mock<IMapper> _mapperMock;
    private readonly ManageSucesoRelacionadosCommandHandler _handler;
    private readonly Mock<IRegistroActualizacionService> _registroActualizacionMock;

    public ManageSucesoRelacionadosCommandHandlerTests()
    {
        _loggerMock = new Mock<ILogger<ManageSucesoRelacionadosCommandHandler>>();
        _unitOfWorkMock = new Mock<IUnitOfWork>();
        _mapperMock = new Mock<IMapper>();
        _registroActualizacionMock = new Mock<IRegistroActualizacionService>();

        _handler = new ManageSucesoRelacionadosCommandHandler(_loggerMock.Object, _unitOfWorkMock.Object, _mapperMock.Object, _registroActualizacionMock.Object);
    }

    [Fact]
    public async Task Handle_ValidRequest_AddsNewSucesoRelacionado()
    {
        // Arrange
        var command = new ManageSucesoRelacionadosCommand
        {
            IdSuceso = 1,
            IdsSucesosAsociados = new List<int> { 2, 3 }
        };

        var suceso = new Suceso { Id = 1, Borrado = false };
        var sucesosAsociados = new List<Suceso>
        {
            new Suceso { Id = 2 },
            new Suceso { Id = 3 }
        };

        _unitOfWorkMock.Setup(uow => uow.Repository<Suceso>().GetByIdAsync(command.IdSuceso))
            .ReturnsAsync(suceso);
        _unitOfWorkMock.Setup(uow => uow.Repository<Suceso>().GetAsync(It.IsAny<Expression<Func<Suceso, bool>>>()))
                .ReturnsAsync(sucesosAsociados);

        _unitOfWorkMock.Setup(uow => uow.Repository<SucesoRelacionado>().AddEntity(It.IsAny<SucesoRelacionado>()))
            .Callback<SucesoRelacionado>(sr => sr.Id = 1); // Set the Id to a non-zero value
        _unitOfWorkMock.Setup(uow => uow.Complete()).ReturnsAsync(1);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.NotNull(result);
        Assert.True(result.IdSucesoRelacionado > 0);
        _unitOfWorkMock.Verify(uow => uow.Repository<SucesoRelacionado>().AddEntity(It.IsAny<SucesoRelacionado>()), Times.Once);
        _unitOfWorkMock.Verify(uow => uow.Complete(), Times.Once);
    }

    [Fact]
    public async Task Handle_InvalidSucesoId_ThrowsNotFoundException()
    {
        // Arrange
        var command = new ManageSucesoRelacionadosCommand
        {
            IdSuceso = 1,
            IdsSucesosAsociados = new List<int> { 2, 3 }
        };

        _unitOfWorkMock.Setup(uow => uow.Repository<Suceso>().GetByIdAsync(command.IdSuceso))
            .ReturnsAsync((Suceso)null);

        // Act & Assert
        await Assert.ThrowsAsync<NotFoundException>(() => _handler.Handle(command, CancellationToken.None));
    }

    [Fact]
    public async Task Handle_InvalidSucesosAsociados_ThrowsNotFoundException()
    {
        // Arrange
        var command = new ManageSucesoRelacionadosCommand
        {
            IdSuceso = 1,
            IdsSucesosAsociados = new List<int> { 2, 3 }
        };

        var suceso = new Suceso { Id = 1, Borrado = false };
        var sucesosAsociados = new List<Suceso>
        {
            new Suceso { Id = 2 }
        };

        _unitOfWorkMock.Setup(uow => uow.Repository<Suceso>().GetByIdAsync(command.IdSuceso))
            .ReturnsAsync(suceso);
        _unitOfWorkMock.Setup(uow => uow.Repository<Suceso>().GetAsync(It.IsAny<Expression<Func<Suceso, bool>>>()))
                .ReturnsAsync(sucesosAsociados);

        // Act & Assert
        await Assert.ThrowsAsync<NotFoundException>(() => _handler.Handle(command, CancellationToken.None));
    }

    [Fact]
    public async Task Handle_UpdateExistingSucesoRelacionado()
    {
        // Arrange
        var command = new ManageSucesoRelacionadosCommand
        {
            IdRegistroActualizacion = 1,
            IdSuceso = 1,
            IdsSucesosAsociados = new List<int> { 2, 3 }
        };

        var sucesoRelacionado = new SucesoRelacionado
        {
            Id = 1,
            IdSucesoPrincipal = 1,
            DetalleSucesoRelacionados = new List<DetalleSucesoRelacionado>
            {
                new DetalleSucesoRelacionado { IdSucesoAsociado = 2 }
            }
        };

        var sucesosAsociados = new List<Suceso>
        {
            new Suceso { Id = 2 },
            new Suceso { Id = 3 }
        };

        _unitOfWorkMock.Setup(uow => uow.Repository<SucesoRelacionado>().GetByIdWithSpec(It.IsAny<ISpecification<SucesoRelacionado>>()))
            .ReturnsAsync(sucesoRelacionado);
        _unitOfWorkMock.Setup(uow => uow.Repository<Suceso>().GetAsync(It.IsAny<Expression<Func<Suceso, bool>>>()))
                .ReturnsAsync(sucesosAsociados);
        _unitOfWorkMock.Setup(uow => uow.Repository<SucesoRelacionado>().UpdateEntity(It.IsAny<SucesoRelacionado>()));
        _unitOfWorkMock.Setup(uow => uow.Complete()).ReturnsAsync(1);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(sucesoRelacionado.Id, result.IdSucesoRelacionado);
        _unitOfWorkMock.Verify(uow => uow.Repository<SucesoRelacionado>().UpdateEntity(It.IsAny<SucesoRelacionado>()), Times.Once);
        _unitOfWorkMock.Verify(uow => uow.Complete(), Times.Once);
    }
}
