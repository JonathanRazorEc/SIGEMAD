using DGPCE.Sigemad.Application.Contracts.Persistence;
using DGPCE.Sigemad.Application.Features.ImpactosClasificados.Queries.GetGruposImpactosList;
using DGPCE.Sigemad.Application.Features.ImpactosClasificados.Vms;
using DGPCE.Sigemad.Application.Specifications;
using DGPCE.Sigemad.Domain.Modelos;
using Moq;
using System.Linq.Expressions;

namespace DGPCE.Sigemad.Application.Tests.Features.ImpactosClasificados.Queries;
public class GetGruposImpactosListQueryHandlerTest
{
    private readonly Mock<IUnitOfWork> _unitOfWorkMock;
    private readonly GetGruposImpactosListQueryHandler _handler;

    public GetGruposImpactosListQueryHandlerTest()
    {
        _unitOfWorkMock = new Mock<IUnitOfWork>();
        _handler = new GetGruposImpactosListQueryHandler(_unitOfWorkMock.Object);
    }

    [Fact]
    public async Task Handle_ValidQuery_ReturnsListOfGruposImpactos()
    {
        // Arrange
        var request = new GetGruposImpactosListQuery();

        var impactoClasificadoList = new List<ImpactoClasificado>
        {
            new ImpactoClasificado { GrupoImpacto = "Personas" },
            new ImpactoClasificado { GrupoImpacto = "Servicios básicos" },
            new ImpactoClasificado { GrupoImpacto = "Daños" },
            new ImpactoClasificado { GrupoImpacto = "Medio natural y otros" }
        };

        var expectedGruposImpactos = new List<string> { "Personas", "Servicios básicos", "Daños", "Medio natural y otros" };

        _unitOfWorkMock.Setup(uow => uow.Repository<ImpactoClasificado>().GetAllWithSpec(It.IsAny<ISpecification<ImpactoClasificado>>()))
            .ReturnsAsync(impactoClasificadoList);

        _unitOfWorkMock.Setup(u => u.Repository<ImpactoClasificado>().GetAsync(
            It.IsAny<Expression<Func<ImpactoClasificado, string>>>(),
            null, // predicate
            null, // orderBy
            null, // includes
            true, // disableTracking
            true  // distinct
        )).ReturnsAsync(expectedGruposImpactos);

        // Act
        var result = await _handler.Handle(request, CancellationToken.None);

        
        // Assert
        Assert.Equal(expectedGruposImpactos, result);
    }

    [Fact]
    public async Task Handle_ValidQuery_ReturnsEmptyListOfGruposImpactos()
    {
        // Arrange
        var query = new GetGruposImpactosListQuery();
        var impactoClasificadoList = new List<ImpactoClasificado>();

        _unitOfWorkMock.Setup(uow => uow.Repository<ImpactoClasificado>().GetAllAsync())
            .ReturnsAsync(impactoClasificadoList);

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);
        // Assert
        Assert.Empty(result);
    }

    [Fact]
    public async Task Handle_ValidQuery_ReturnsDistinctListOfGruposImpactos()
    {
        // Arrange
        var query = new GetGruposImpactosListQuery();

        var impactoClasificadoList = new List<ImpactoClasificado>
        {
            new ImpactoClasificado { GrupoImpacto = "Personas" },
            new ImpactoClasificado { GrupoImpacto = "Servicios básicos" },
            new ImpactoClasificado { GrupoImpacto = "Daños" },
            new ImpactoClasificado { GrupoImpacto = "Medio natural y otros" },
            new ImpactoClasificado { GrupoImpacto = "Servicios básicos" } // Duplicado            
        };

        var expectedGruposImpactos = new List<string> { "Personas", "Servicios básicos", "Daños", "Medio natural y otros" };

        _unitOfWorkMock.Setup(uow => uow.Repository<ImpactoClasificado>().GetAllWithSpec(It.IsAny<ISpecification<ImpactoClasificado>>()))
            .ReturnsAsync(impactoClasificadoList);

        _unitOfWorkMock.Setup(u => u.Repository<ImpactoClasificado>().GetAsync(
            It.IsAny<Expression<Func<ImpactoClasificado, string>>>(),
            null, // predicate
            null, // orderBy
            null, // includes
            true, // disableTracking
            true  // distinct
        )).ReturnsAsync(expectedGruposImpactos);

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);
        // Assert
        Assert.Equal(expectedGruposImpactos, result);
    }


}
