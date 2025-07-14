using AutoMapper;
using DGPCE.Sigemad.Application.Contracts.Persistence;
using DGPCE.Sigemad.Application.Dtos.SituacionesEquivalentes;
using DGPCE.Sigemad.Application.Features.SituacionesEquivalentes.Queries.GetAll;
using DGPCE.Sigemad.Domain.Modelos;
using Microsoft.Extensions.Logging;
using Moq;
using System.Linq.Expressions;

namespace DGPCE.Sigemad.Application.Tests.Features.SituacionesEquivalentes.Queries.GetAll;

public class GetAllSituacionesEquivalentesQueryCommandTests
{
    private readonly Mock<ILogger<GetAllSituacionesEquivalentesQueryCommand>> _loggerMock;
    private readonly Mock<IUnitOfWork> _unitOfWorkMock;
    private readonly Mock<IMapper> _mapperMock;
    private readonly GetAllSituacionesEquivalentesQueryCommand _handler;

    public GetAllSituacionesEquivalentesQueryCommandTests()
    {
        _loggerMock = new Mock<ILogger<GetAllSituacionesEquivalentesQueryCommand>>();
        _unitOfWorkMock = new Mock<IUnitOfWork>();
        _mapperMock = new Mock<IMapper>();
        _handler = new GetAllSituacionesEquivalentesQueryCommand(_loggerMock.Object, _unitOfWorkMock.Object, _mapperMock.Object);
    }

    [Fact]
    public async Task Handle_ReturnsListOfSituacionEquivalenteDto()
    {
        // Arrange
        var situacionesEquivalentes = new List<SituacionEquivalente>
        {
            new SituacionEquivalente { Id = 1, Descripcion = "1", Obsoleto = false },
            new SituacionEquivalente { Id = 2, Descripcion = "2", Obsoleto = false }
        };

        var situacionesEquivalentesDto = new List<SituacionEquivalenteDto>
        {
            new SituacionEquivalenteDto { Id = 1, Descripcion = "1" },
            new SituacionEquivalenteDto { Id = 2, Descripcion = "2" }
        };

        _unitOfWorkMock.Setup(uow => uow.Repository<SituacionEquivalente>().GetAsync(It.IsAny<Expression<Func<SituacionEquivalente, bool>>>()))
            .ReturnsAsync(situacionesEquivalentes);

        _mapperMock.Setup(m => m.Map<IReadOnlyList<SituacionEquivalenteDto>>(situacionesEquivalentes))
            .Returns(situacionesEquivalentesDto);

        // Act
        var result = await _handler.Handle(new GetAllSituacionesEquivalentesQuery(), CancellationToken.None);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(2, result.Count);
        Assert.Equal(situacionesEquivalentesDto, result);
    }

    [Fact]
    public async Task Handle_ReturnsEmptyList_WhenNoSituacionesEquivalentesFound()
    {
        // Arrange
        var situacionesEquivalentes = new List<SituacionEquivalente>();
        var situacionesEquivalentesDto = new List<SituacionEquivalenteDto>();

        _unitOfWorkMock.Setup(uow => uow.Repository<SituacionEquivalente>().GetAsync(It.IsAny<Expression<Func<SituacionEquivalente, bool>>>()))
            .ReturnsAsync(situacionesEquivalentes);

        _mapperMock.Setup(m => m.Map<IReadOnlyList<SituacionEquivalenteDto>>(situacionesEquivalentes))
            .Returns(situacionesEquivalentesDto);

        // Act
        var result = await _handler.Handle(new GetAllSituacionesEquivalentesQuery(), CancellationToken.None);

        // Assert
        Assert.NotNull(result);
        Assert.Empty(result);
    }

    [Fact]
    public async Task Handle_ThrowsException_WhenRepositoryThrowsException()
    {
        // Arrange
        _unitOfWorkMock.Setup(uow => uow.Repository<SituacionEquivalente>().GetAsync(It.IsAny<Expression<Func<SituacionEquivalente, bool>>>()))
            .ThrowsAsync(new System.Exception("Repository error"));

        // Act & Assert
        await Assert.ThrowsAsync<System.Exception>(() => _handler.Handle(new GetAllSituacionesEquivalentesQuery(), CancellationToken.None));
    }
}
