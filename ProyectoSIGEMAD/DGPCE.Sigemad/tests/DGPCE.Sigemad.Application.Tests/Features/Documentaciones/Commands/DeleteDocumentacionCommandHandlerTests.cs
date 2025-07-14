using DGPCE.Sigemad.Application.Contracts.Persistence;
using DGPCE.Sigemad.Application.Exceptions;
using DGPCE.Sigemad.Application.Features.Documentaciones.Commands.DeleteDocumentaciones;
using DGPCE.Sigemad.Domain.Modelos;
using MediatR;
using Microsoft.Extensions.Logging;
using Moq;

namespace DGPCE.Sigemad.Application.Tests.Features.Documentaciones.Commands;

public class DeleteDocumentacionCommandHandlerTests
{
    private readonly Mock<ILogger<DeleteDocumentacionCommandHandler>> _loggerMock;
    private readonly Mock<IUnitOfWork> _unitOfWorkMock;
    private readonly DeleteDocumentacionCommandHandler _handler;

    public DeleteDocumentacionCommandHandlerTests()
    {
        _loggerMock = new Mock<ILogger<DeleteDocumentacionCommandHandler>>();
        _unitOfWorkMock = new Mock<IUnitOfWork>();
        _handler = new DeleteDocumentacionCommandHandler(_loggerMock.Object, _unitOfWorkMock.Object);
    }

    [Fact]
    public async Task Handle_DocumentacionExists_DeletesDocumentacion()
    {
        // Arrange
        var documentacionId = 1;
        var documentacion = new Documentacion { Id = documentacionId, Borrado = false };

        _unitOfWorkMock.Setup(uow => uow.Repository<Documentacion>().GetByIdAsync(documentacionId))
            .ReturnsAsync(documentacion);
        _unitOfWorkMock.Setup(uow => uow.Complete()).ReturnsAsync(1);

        var command = new DeleteDocumentacionCommand { Id = documentacionId };

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.Equal(Unit.Value, result);
        _unitOfWorkMock.Verify(uow => uow.Repository<Documentacion>().DeleteEntity(documentacion), Times.Once);
        _unitOfWorkMock.Verify(uow => uow.Complete(), Times.Once);
    }

    [Fact]
    public async Task Handle_DocumentacionNotFound_ThrowsNotFoundException()
    {
        // Arrange
        var documentacionId = 1;

        _unitOfWorkMock.Setup(uow => uow.Repository<Documentacion>().GetByIdAsync(documentacionId))
            .ReturnsAsync((Documentacion)null);

        var command = new DeleteDocumentacionCommand { Id = documentacionId };

        // Act & Assert
        await Assert.ThrowsAsync<NotFoundException>(() => _handler.Handle(command, CancellationToken.None));
    }

    [Fact]
    public async Task Handle_DocumentacionBorrado_ThrowsNotFoundException()
    {
        // Arrange
        var documentacionId = 1;
        var documentacion = new Documentacion { Id = documentacionId, Borrado = true };

        _unitOfWorkMock.Setup(uow => uow.Repository<Documentacion>().GetByIdAsync(documentacionId))
            .ReturnsAsync(documentacion);

        var command = new DeleteDocumentacionCommand { Id = documentacionId };

        // Act & Assert
        await Assert.ThrowsAsync<NotFoundException>(() => _handler.Handle(command, CancellationToken.None));
    }

    [Fact]
    public async Task Handle_CompleteFails_ThrowsException()
    {
        // Arrange
        var documentacionId = 1;
        var documentacion = new Documentacion { Id = documentacionId, Borrado = false };

        _unitOfWorkMock.Setup(uow => uow.Repository<Documentacion>().GetByIdAsync(documentacionId))
            .ReturnsAsync(documentacion);
        _unitOfWorkMock.Setup(uow => uow.Complete()).ReturnsAsync(0);

        var command = new DeleteDocumentacionCommand { Id = documentacionId };

        // Act & Assert
        await Assert.ThrowsAsync<Exception>(() => _handler.Handle(command, CancellationToken.None));
    }
}
