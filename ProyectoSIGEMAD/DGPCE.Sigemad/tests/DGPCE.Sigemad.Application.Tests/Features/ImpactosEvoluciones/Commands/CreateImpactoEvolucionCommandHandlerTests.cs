using AutoMapper;
using DGPCE.Sigemad.Application.Contracts.Persistence;
using DGPCE.Sigemad.Application.Exceptions;
using DGPCE.Sigemad.Application.Features.ImpactosEvoluciones.Commands.CreateImpactoEvoluciones;
using DGPCE.Sigemad.Application.Mappings;
using DGPCE.Sigemad.Application.Specifications;
using DGPCE.Sigemad.Domain.Modelos;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using Moq;

namespace DGPCE.Sigemad.Application.Tests.Features.ImpactosEvoluciones.Commands;

public class CreateImpactoEvolucionCommandHandlerTests
{
    private readonly Mock<ILogger<CreateImpactoEvolucionCommandHandler>> _loggerMock;
    private readonly Mock<IUnitOfWork> _unitOfWorkMock;
    private readonly IMapper _mapper;

    public CreateImpactoEvolucionCommandHandlerTests()
    {
        _loggerMock = new Mock<ILogger<CreateImpactoEvolucionCommandHandler>>();
        _unitOfWorkMock = new Mock<IUnitOfWork>();

        var mapperConfig = new MapperConfiguration(cfg =>
        {
            cfg.AddProfile<MappingProfile>();
        });
        _mapper = mapperConfig.CreateMapper();
    }

    [Fact]
    public async Task Handle_GivenValidRequest_ShouldCreateImpactoEvolucion()
    {
        // Arrange
        var request = new CreateImpactoEvolucionCommand
        {
            IdEvolucion = 1,
            IdImpactoClasificado = 2,
            Nuclear = true,
            Numero = 3,
        };

        var evolucion = new Evolucion { Id = 1 };
        var impactoClasificado = new ImpactoClasificado { Id = 2 };
        var impactoEvolucion = new ImpactoEvolucion {
            Id = 1,
            IdEvolucion = 1,
            IdImpactoClasificado = 2,
            Numero = 3,
        };


        _unitOfWorkMock.Setup(u => u.Repository<Evolucion>().GetByIdWithSpec(It.IsAny<ISpecification<Evolucion>>()))
            .ReturnsAsync(evolucion);

        _unitOfWorkMock.Setup(u => u.Repository<ImpactoClasificado>().GetByIdAsync(request.IdImpactoClasificado))
            .ReturnsAsync(impactoClasificado);

        _unitOfWorkMock.Setup(u => u.Repository<ImpactoEvolucion>().AddEntity(It.IsAny<ImpactoEvolucion>()))
            .Callback<ImpactoEvolucion>(impactoEvolucion =>
            {
                impactoEvolucion.Id = 1;
            });

        _unitOfWorkMock.Setup(u => u.Complete())
            .ReturnsAsync(1);

        var handler = new CreateImpactoEvolucionCommandHandler(
            _loggerMock.Object,
            _unitOfWorkMock.Object,
            _mapper
            );


        // Act
        var result = await handler.Handle(request, CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result.Id.Should().Be(impactoEvolucion.Id);
        _unitOfWorkMock.Verify(u => u.Repository<ImpactoEvolucion>().AddEntity(It.IsAny<ImpactoEvolucion>()), Times.Once);
        _unitOfWorkMock.Verify(u => u.Complete(), Times.Once);
    }

    [Fact]
    public async Task Handle_GivenInvalidEvolucionId_ShouldThrowNotFoundException()
    {
        // Arrange
        var request = new CreateImpactoEvolucionCommand
        {
            IdEvolucion = 99, // No existe en la base de datos
            IdImpactoClasificado = 2,
        };

        _unitOfWorkMock.Setup(u => u.Repository<Evolucion>().GetByIdAsync(request.IdEvolucion))
            .ReturnsAsync((Evolucion)null);

        var handler = new CreateImpactoEvolucionCommandHandler(
            _loggerMock.Object,
            _unitOfWorkMock.Object,
            _mapper
        );

        // Act
        Func<Task> action = async () => await handler.Handle(request, CancellationToken.None);

        // Assert
        await action.Should().ThrowAsync<NotFoundException>()
            .WithMessage($"*{nameof(Evolucion)}*");

    }

    [Fact]
    public async Task Handle_GivenInvalidImpactoClasificadoId_ShouldThrowNotFoundException()
    {
        // Arrange
        var request = new CreateImpactoEvolucionCommand
        {
            IdEvolucion = 1,
            IdImpactoClasificado = 99, // No existe en la base de datos
        };

        var evolucion = new Evolucion { Id = 1 };

        _unitOfWorkMock.Setup(u => u.Repository<Evolucion>().GetByIdWithSpec(It.IsAny<ISpecification<Evolucion>>()))
            .ReturnsAsync(evolucion);

        _unitOfWorkMock.Setup(u => u.Repository<ImpactoClasificado>().GetByIdAsync(request.IdImpactoClasificado))
            .ReturnsAsync((ImpactoClasificado)null);

        var handler = new CreateImpactoEvolucionCommandHandler(
            _loggerMock.Object,
            _unitOfWorkMock.Object,
            _mapper
        );

        // Act
        Func<Task> action = async () => await handler.Handle(request, CancellationToken.None);

        // Assert
        await action.Should().ThrowAsync<NotFoundException>()
            .WithMessage($"*{nameof(ImpactoClasificado)}*");
    }


}
