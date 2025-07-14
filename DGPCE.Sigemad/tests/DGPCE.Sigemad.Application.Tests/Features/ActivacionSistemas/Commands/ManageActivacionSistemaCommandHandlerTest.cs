using AutoMapper;
using DGPCE.Sigemad.Application.Contracts.Persistence;
using DGPCE.Sigemad.Application.Contracts.RegistrosActualizacion;
using DGPCE.Sigemad.Application.Dtos.ActivacionSistema;
using DGPCE.Sigemad.Application.Exceptions;
using DGPCE.Sigemad.Application.Features.ActivacionesSistemas.Manage;
using DGPCE.Sigemad.Application.Specifications;
using DGPCE.Sigemad.Domain.Modelos;
using Microsoft.Extensions.Logging;
using Moq;
using System.Linq.Expressions;


namespace DGPCE.Sigemad.Application.Tests.Features.ActivacionSistemas.Commands;
public class ManageActivacionSistemaCommandHandlerTest
{
    private readonly Mock<ILogger<ManageActivacionSistemaCommandHandler>> _loggerMock;
    private readonly Mock<IUnitOfWork> _unitOfWorkMock;
    private readonly Mock<IMapper> _mapperMock;
    private readonly ManageActivacionSistemaCommandHandler _handler;
    private readonly Mock<IRegistroActualizacionService> _registroActualizacionMock;

    public ManageActivacionSistemaCommandHandlerTest()
    {
        _loggerMock = new Mock<ILogger<ManageActivacionSistemaCommandHandler>>();
        _unitOfWorkMock = new Mock<IUnitOfWork>();
        _mapperMock = new Mock<IMapper>();
        _registroActualizacionMock = new Mock<IRegistroActualizacionService>();

        _handler = new ManageActivacionSistemaCommandHandler(
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
        var command = new ManageActivacionSistemaCommand
        {
            IdSuceso = 1,
            Detalles = new List<ManageActivacionSistemaDto>
            {
                new ManageActivacionSistemaDto
                {
                    IdTipoSistemaEmergencia = 1,
                    IdModoActivacion = 1,
                    Autoridad = "Autoridad"
                }
            }
        };

        var activacionSistemapDto = command.Detalles.First();
        var activacionSistema = new ActivacionSistema
        {
            Id = 0,
            IdTipoSistemaEmergencia = activacionSistemapDto.IdTipoSistemaEmergencia,
            IdModoActivacion = activacionSistemapDto.IdModoActivacion,
            Autoridad = activacionSistemapDto.Autoridad
        };

        _unitOfWorkMock.Setup(uow => uow.Repository<Suceso>().GetByIdAsync(It.IsAny<int>()))
            .ReturnsAsync(new Suceso { Id = 1 });

        _unitOfWorkMock.Setup(uow => uow.Repository<TipoSistemaEmergencia>().GetAsync(It.IsAny<Expression<Func<TipoSistemaEmergencia, bool>>>()))
            .ReturnsAsync(new List<TipoSistemaEmergencia> { new TipoSistemaEmergencia { Id = 1 } });

        _unitOfWorkMock.Setup(uow => uow.Repository<ModoActivacion>().GetAsync(It.IsAny<Expression<Func<ModoActivacion, bool>>>()))
            .ReturnsAsync(new List<ModoActivacion> { new ModoActivacion { Id = 1 } });

        _mapperMock.Setup(m => m.Map<ActivacionSistema>(It.IsAny<ManageActivacionSistemaDto>()))
       .Returns(activacionSistema);

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
        var command = new ManageActivacionSistemaCommand
        {
            IdSuceso = 1,
            Detalles = new List<ManageActivacionSistemaDto>
            {
                new ManageActivacionSistemaDto
                {
                    IdTipoSistemaEmergencia = 1,
                    IdModoActivacion = 1,
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
        var command = new ManageActivacionSistemaCommand
        {
            IdRegistroActualizacion = 1,
            IdSuceso = 1,
            Detalles = new List<ManageActivacionSistemaDto>
            {
                new ManageActivacionSistemaDto
                {
                    IdTipoSistemaEmergencia = 1,
                    IdModoActivacion = 1,
                    Autoridad = "Autoridad"
                }
            }
        };

        var actuacionRelevante = new ActuacionRelevanteDGPCE { Id = 1, IdSuceso = 1 };

        _unitOfWorkMock.Setup(uow => uow.Repository<ActuacionRelevanteDGPCE>().GetByIdWithSpec(It.IsAny<ISpecification<ActuacionRelevanteDGPCE>>()))
            .ReturnsAsync(actuacionRelevante);

        var activacionSistemapDto = command.Detalles.First();
        var activacionSistema = new ActivacionSistema
        {
            Id = 0,
            IdTipoSistemaEmergencia = activacionSistemapDto.IdTipoSistemaEmergencia,
            IdModoActivacion = activacionSistemapDto.IdModoActivacion,
            Autoridad = activacionSistemapDto.Autoridad
        };

        _unitOfWorkMock.Setup(uow => uow.Repository<Suceso>().GetByIdAsync(It.IsAny<int>()))
            .ReturnsAsync(new Suceso { Id = 1 });

        _unitOfWorkMock.Setup(uow => uow.Repository<TipoSistemaEmergencia>().GetAsync(It.IsAny<Expression<Func<TipoSistemaEmergencia, bool>>>()))
            .ReturnsAsync(new List<TipoSistemaEmergencia> { new TipoSistemaEmergencia { Id = 1 } });

        _unitOfWorkMock.Setup(uow => uow.Repository<ModoActivacion>().GetAsync(It.IsAny<Expression<Func<ModoActivacion, bool>>>()))
            .ReturnsAsync(new List<ModoActivacion> { new ModoActivacion { Id = 1 } });

        _mapperMock.Setup(m => m.Map<ActivacionSistema>(It.IsAny<ManageActivacionSistemaDto>()))
       .Returns(activacionSistema);

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
        var command = new ManageActivacionSistemaCommand
        {
            IdRegistroActualizacion = 1,
            IdSuceso = 1,
            Detalles = new List<ManageActivacionSistemaDto>
            {
                new ManageActivacionSistemaDto
                {
                    IdTipoSistemaEmergencia = 1,
                    IdModoActivacion = 1,
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
