using DGPCE.Sigemad.Application.Contracts.Persistence;
using DGPCE.Sigemad.Application.Features.ImpactosClasificados.Queries.GetTiposImpactosList;
using DGPCE.Sigemad.Domain.Modelos;
using Moq;

namespace DGPCE.Sigemad.Application.Tests.Features.ImpactosClasificados.Queries;
public class GetTiposImpactosListQueryHandlerTest
{
    private readonly Mock<IUnitOfWork> _unitOfWorkMock;
    private readonly GetTiposImpactosListQueryHandler _handler;

    public GetTiposImpactosListQueryHandlerTest()
    {
        _unitOfWorkMock = new Mock<IUnitOfWork>();
        _handler = new GetTiposImpactosListQueryHandler(_unitOfWorkMock.Object);
    }

    [Fact]
    public async Task Handle_ValidQuery_ReturnsListOfTiposImpactos()
    {
        // Arrange
        var request = new GetTiposImpactosListQuery();

        var impactoClasificadoList = new List<ImpactoClasificado>
        {
            new ImpactoClasificado { TipoImpacto = "Consecuencia" },
            new ImpactoClasificado { TipoImpacto = "Actuación" }
        };

        _unitOfWorkMock
            .Setup(uow => uow.Repository<ImpactoClasificado>().GetAllAsync())
            .ReturnsAsync(impactoClasificadoList);


        // Act
        var result = await _handler.Handle(request, CancellationToken.None);

        var expectedTiposImpactos = new List<string> { "Consecuencia", "Actuación" };
        // Assert
        Assert.Equal(expectedTiposImpactos, result);
    }

    [Fact]
    public async Task Handle_ValidQuery_ReturnsEmptyListOfTiposImpactos()
    {
        // Arrange
        var query = new GetTiposImpactosListQuery();
        var impactoClasificadoList = new List<ImpactoClasificado>();

        _unitOfWorkMock.Setup(uow => uow.Repository<ImpactoClasificado>().GetAllAsync())
            .ReturnsAsync(impactoClasificadoList);

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);
        // Assert
        Assert.Empty(result);
    }

    [Fact]
    public async Task Handle_ValidQuery_ReturnsDistinctListOfTiposImpactos()
    {
        // Arrange
        var query = new GetTiposImpactosListQuery();

        var impactoClasificadoList = new List<ImpactoClasificado>
        {
            new ImpactoClasificado { TipoImpacto = "Consecuencia" },
            new ImpactoClasificado { TipoImpacto = "Actuación" },
            new ImpactoClasificado { TipoImpacto = "Consecuencia" } // Duplicado            
        };

        var expectedTiposImpactos = new List<string> { "Consecuencia", "Actuación" };

        _unitOfWorkMock.Setup(uow => uow.Repository<ImpactoClasificado>().GetAllAsync())
            .ReturnsAsync(impactoClasificadoList);

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);
        // Assert
        Assert.Equal(expectedTiposImpactos, result);
    }


}
