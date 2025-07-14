using AutoMapper;
using DGPCE.Sigemad.Application.Contracts.Persistence;
using DGPCE.Sigemad.Application.Features.ImpactosEvoluciones.Commands.CreateImpactoEvoluciones;
using DGPCE.Sigemad.Application.Features.ValidacionesImpacto.Queries.GetCamposImpactosById;
using DGPCE.Sigemad.Application.Mappings;
using Microsoft.Extensions.Logging;
using Moq;

namespace DGPCE.Sigemad.Application.Tests.Features.ValidacionesImpactos.Queries;
public class GetCamposImpactosByIdQueryHandlerTest
{
    private readonly Mock<ILogger<CreateImpactoEvolucionCommandHandler>> _loggerMock;
    private readonly Mock<IUnitOfWork> _unitOfWorkMock;
    private readonly IMapper _mapper;

    public GetCamposImpactosByIdQueryHandlerTest()
    {
        _loggerMock = new Mock<ILogger<CreateImpactoEvolucionCommandHandler>>();
        _unitOfWorkMock = new Mock<IUnitOfWork>();
        var mapperConfig = new MapperConfiguration(cfg =>
        {
            cfg.AddProfile<MappingProfile>();
        });
        _mapper = mapperConfig.CreateMapper();
    }

    //TODO: Add tests
}