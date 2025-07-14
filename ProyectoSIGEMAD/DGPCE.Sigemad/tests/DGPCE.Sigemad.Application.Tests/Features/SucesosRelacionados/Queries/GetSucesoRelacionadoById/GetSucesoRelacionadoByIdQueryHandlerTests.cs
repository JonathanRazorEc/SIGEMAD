using AutoMapper;
using DGPCE.Sigemad.Application.Contracts.Persistence;
using DGPCE.Sigemad.Application.Exceptions;
using DGPCE.Sigemad.Application.Features.SucesosRelacionados.Queries.GetSucesoRelacionadoById;
using DGPCE.Sigemad.Application.Specifications;
using DGPCE.Sigemad.Domain.Enums;
using DGPCE.Sigemad.Domain.Modelos;
using Microsoft.Extensions.Logging;
using Moq;

namespace DGPCE.Sigemad.Application.Tests.Features.SucesosRelacionados.Queries.GetSucesoRelacionadoById;
public class GetSucesoRelacionadoByIdQueryHandlerTests
{
    private readonly Mock<ILogger<GetSucesoRelacionadoByIdQueryHandler>> _loggerMock;
    private readonly Mock<IUnitOfWork> _unitOfWorkMock;
    private readonly Mock<IMapper> _mapperMock;
    private readonly GetSucesoRelacionadoByIdQueryHandler _handler;

    public GetSucesoRelacionadoByIdQueryHandlerTests()
    {
        _loggerMock = new Mock<ILogger<GetSucesoRelacionadoByIdQueryHandler>>();
        _unitOfWorkMock = new Mock<IUnitOfWork>();
        _mapperMock = new Mock<IMapper>();
        _handler = new GetSucesoRelacionadoByIdQueryHandler(_loggerMock.Object, _unitOfWorkMock.Object, _mapperMock.Object);
    }

    [Fact]
    public async Task Handle_SucesoRelacionadoExists_ReturnsSucesoRelacionadoVm()
    {
        // Arrange
        var sucesoRelacionado = new SucesoRelacionado
        {
            Id = 1,
            IdSucesoPrincipal = 1,
            FechaCreacion = DateTime.Now,
            FechaModificacion = DateTime.Now,
            DetalleSucesoRelacionados = new List<DetalleSucesoRelacionado>
                {
                    new DetalleSucesoRelacionado
                    {
                        IdSucesoAsociado = 2,
                        Borrado = false,
                        SucesoAsociado = new Suceso
                        {
                            Id = 2,
                            IdTipo = (int)TipoSucesoEnum.IncendioForestal,
                            FechaCreacion = DateTime.Now,
                            FechaModificacion = DateTime.Now,
                            TipoSuceso = new TipoSuceso { Descripcion = "Incendio Forestal" },
                            Incendios = new List<Incendio>
                            {
                                new Incendio
                                {
                                    EstadoSuceso = new EstadoSuceso { Descripcion = "Activo" },
                                    Denominacion = "Incendio en el bosque"
                                }
                            }
                        }
                    }
                }
        };

        _unitOfWorkMock.Setup(uow => uow.Repository<SucesoRelacionado>().GetByIdWithSpec(It.IsAny<ISpecification<SucesoRelacionado>>()))
            .ReturnsAsync(sucesoRelacionado);

        // Act
        var result = await _handler.Handle(new GetSucesoRelacionadoByIdQuery(1), CancellationToken.None);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(sucesoRelacionado.Id, result.Id);
        Assert.Equal(sucesoRelacionado.IdSucesoPrincipal, result.IdSuceso);
        Assert.Single(result.SucesosAsociados);
        Assert.Equal("Incendio Forestal", result.SucesosAsociados.First().TipoSuceso);
        Assert.Equal("Activo", result.SucesosAsociados.First().Estado);
        Assert.Equal("Incendio en el bosque", result.SucesosAsociados.First().Denominacion);
    }

    [Fact]
    public async Task Handle_SucesoRelacionadoDoesNotExist_ThrowsNotFoundException()
    {
        // Arrange
        _unitOfWorkMock.Setup(uow => uow.Repository<SucesoRelacionado>().GetByIdWithSpec(It.IsAny<ISpecification<SucesoRelacionado>>()))
            .ReturnsAsync((SucesoRelacionado)null);

        // Act & Assert
        await Assert.ThrowsAsync<NotFoundException>(() => _handler.Handle(new GetSucesoRelacionadoByIdQuery(1), CancellationToken.None));
    }
}
