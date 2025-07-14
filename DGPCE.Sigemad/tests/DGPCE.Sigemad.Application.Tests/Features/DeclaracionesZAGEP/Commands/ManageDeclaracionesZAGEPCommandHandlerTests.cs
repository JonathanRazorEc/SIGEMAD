using AutoMapper;
using DGPCE.Sigemad.Application.Contracts.Persistence;
using DGPCE.Sigemad.Application.Contracts.RegistrosActualizacion;
using DGPCE.Sigemad.Application.Dtos.DeclaracionesZAGEP;
using DGPCE.Sigemad.Application.Exceptions;
using DGPCE.Sigemad.Application.Features.DeclaracionesZAGEP.Commands.ManageDeclaracionesZAGEP;
using DGPCE.Sigemad.Application.Specifications.ActuacionesRelevantesDGPCE;
using DGPCE.Sigemad.Domain.Modelos;
using Microsoft.Extensions.Logging;
using Moq;


namespace DGPCE.Sigemad.Application.Tests.Features.DeclaracionesZAGEP.Commands
{
    public class ManageDeclaracionesZAGEPCommandHandlerTests
    {
        private readonly Mock<ILogger<ManageDeclaracionesZAGEPCommandHandler>> _loggerMock;
        private readonly Mock<IUnitOfWork> _unitOfWorkMock;
        private readonly Mock<IMapper> _mapperMock;
        private readonly Mock<IRegistroActualizacionService> _registroActualizacionMock;

        public ManageDeclaracionesZAGEPCommandHandlerTests()
        {
            _loggerMock = new Mock<ILogger<ManageDeclaracionesZAGEPCommandHandler>>();
            _unitOfWorkMock = new Mock<IUnitOfWork>();
            _mapperMock = new Mock<IMapper>();
            _registroActualizacionMock = new Mock<IRegistroActualizacionService>();
        }

        [Fact]
        public async Task Handle_WithValidRequestAndExistingDeclaracionesZAGEP_ShouldUpdateDeclaracionesZAGEP()
        {
            // Arrange
            var command = new ManageDeclaracionesZAGEPCommand
            {
                IdRegistroActualizacion = 1,
                IdSuceso = 1,
                Detalles = new List<ManageDeclaracionZAGEPDto>
        {
            new ManageDeclaracionZAGEPDto
            {
                Id = 1,
                FechaSolicitud = new DateOnly(2022, 1, 1),
                Denominacion = "Declaracion 1 ACTUALIZADO",
                Observaciones = "Observaciones 1 ACTUALIZADO"
            }
        }
            };

            var actuacionRelevante = new ActuacionRelevanteDGPCE
            {
                Id = 1,
                IdSuceso = 1,
                DeclaracionesZAGEP = new List<DeclaracionZAGEP>
        {
            new DeclaracionZAGEP
            {
                Id = 1,
                FechaSolicitud = new DateOnly(2022, 1, 1),
                Denominacion = "Declaracion 1",
                Observaciones = "Observaciones 1"
            }
        }
            };

            _unitOfWorkMock.Setup(uow => uow.Repository<ActuacionRelevanteDGPCE>().GetByIdWithSpec(It.IsAny<ActuacionRelevanteDGPCESpecification>()))
                .ReturnsAsync(actuacionRelevante);

            _unitOfWorkMock.Setup(uow => uow.Complete())
                .ReturnsAsync(1);

            var handler = new ManageDeclaracionesZAGEPCommandHandler(_loggerMock.Object, _unitOfWorkMock.Object, _mapperMock.Object, _registroActualizacionMock.Object);

            // Act
            var response = await handler.Handle(command, CancellationToken.None);

            // Assert
            _unitOfWorkMock.Verify(uow => uow.Repository<ActuacionRelevanteDGPCE>().UpdateEntity(It.IsAny<ActuacionRelevanteDGPCE>()), Times.Once);
            _unitOfWorkMock.Verify(uow => uow.Complete(), Times.Once);
            Assert.Equal(1, response.IdActuacionRelevante);
        }


        [Fact]
        public async Task Handle_WithValidRequestAndNewDeclaracionesZAGEP_ShouldAddDeclaracionesZAGEP()
        {
            // Arrange
            var command = new ManageDeclaracionesZAGEPCommand
            {
                IdSuceso = 1,
                Detalles = new List<ManageDeclaracionZAGEPDto>
        {
            new ManageDeclaracionZAGEPDto
            {
                Id = 0,
                FechaSolicitud = new DateOnly(2022, 1, 1),
                Denominacion = "Declaracion 1",
                Observaciones = "Observaciones 1"
            }
        }
            };

            var suceso = new Suceso
            {
                Id = 1,
                Borrado = false
            };

            var declaracionZagepDto = command.Detalles.First();
            var declaracionZagep = new DeclaracionZAGEP
            {
                Id = 0,
                FechaSolicitud = declaracionZagepDto.FechaSolicitud,
                Denominacion = declaracionZagepDto.Denominacion,
                Observaciones = declaracionZagepDto.Observaciones
            };

            _unitOfWorkMock.Setup(uow => uow.Repository<Suceso>().GetByIdAsync(It.IsAny<int>()))
                .ReturnsAsync(suceso);

            _mapperMock.Setup(m => m.Map<DeclaracionZAGEP>(It.IsAny<ManageDeclaracionZAGEPDto>()))
                .Returns(declaracionZagep);

            _unitOfWorkMock.Setup(uow => uow.Repository<ActuacionRelevanteDGPCE>().AddEntity(It.IsAny<ActuacionRelevanteDGPCE>()))
                .Callback<ActuacionRelevanteDGPCE>(actuacionRelevanteDGPCE =>
                {
                    actuacionRelevanteDGPCE.Id = 1;
                });

            _unitOfWorkMock.Setup(u => u.Complete())
                .ReturnsAsync(1);

            var handler = new ManageDeclaracionesZAGEPCommandHandler(_loggerMock.Object, _unitOfWorkMock.Object, _mapperMock.Object, _registroActualizacionMock.Object);

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
            var command = new ManageDeclaracionesZAGEPCommand
            {
                IdRegistroActualizacion = 1,
                IdSuceso = 1,
                Detalles = new List<ManageDeclaracionZAGEPDto>
        {
            new ManageDeclaracionZAGEPDto
            {
                Id = 1,
                FechaSolicitud = new DateOnly(2022, 1, 1),
                Denominacion = "Declaracion 1",
                Observaciones = "Observaciones 1"
            }
        }
            };

            _unitOfWorkMock.Setup(uow => uow.Repository<ActuacionRelevanteDGPCE>().GetByIdWithSpec(It.IsAny<ActuacionRelevanteDGPCESpecification>()))
                .ReturnsAsync((ActuacionRelevanteDGPCE)null);

            var handler = new ManageDeclaracionesZAGEPCommandHandler(_loggerMock.Object, _unitOfWorkMock.Object, _mapperMock.Object, _registroActualizacionMock.Object);

            // Act & Assert
            await Assert.ThrowsAsync<NotFoundException>(() => handler.Handle(command, CancellationToken.None));
        }


        [Fact]
        public async Task Handle_WithValidRequestAndExistingActuacionRelevante_ShouldDeleteDeclaracionZAGEP()
        {
            // Arrange
            var command = new ManageDeclaracionesZAGEPCommand
            {
                IdRegistroActualizacion = 1,
                IdSuceso = 1,
                Detalles = new()
            };

            var actuacionRelevante = new ActuacionRelevanteDGPCE
            {
                Id = 1,
                DeclaracionesZAGEP = new List<DeclaracionZAGEP>
        {
            new DeclaracionZAGEP
            {
                Id = 1,
                FechaSolicitud = new DateOnly(2022, 1, 1),
                Denominacion = "Declaracion 1",
                Observaciones = "Observaciones 1"
            }
        }
            };

            _unitOfWorkMock.Setup(uow => uow.Repository<ActuacionRelevanteDGPCE>().GetByIdWithSpec(It.IsAny<ActuacionRelevanteDGPCESpecification>()))
                .ReturnsAsync(actuacionRelevante);

            _unitOfWorkMock.Setup(uow => uow.Repository<DeclaracionZAGEP>().DeleteEntity(It.IsAny<DeclaracionZAGEP>()))
                .Verifiable();

            _unitOfWorkMock.Setup(u => u.Complete())
                .ReturnsAsync(1); // Asegúrate de que Complete devuelva un valor mayor que 0

            var handler = new ManageDeclaracionesZAGEPCommandHandler(_loggerMock.Object, _unitOfWorkMock.Object, _mapperMock.Object, _registroActualizacionMock.Object);

            // Act
            var response = await handler.Handle(command, CancellationToken.None);

            // Assert
            _unitOfWorkMock.Verify(uow => uow.Repository<DeclaracionZAGEP>().DeleteEntity(It.IsAny<DeclaracionZAGEP>()), Times.Once);
            _unitOfWorkMock.Verify(uow => uow.Repository<ActuacionRelevanteDGPCE>().UpdateEntity(It.IsAny<ActuacionRelevanteDGPCE>()), Times.Once);
            _unitOfWorkMock.Verify(uow => uow.Complete(), Times.Once);
            Assert.Equal(1, response.IdActuacionRelevante);
        }

    }
}
