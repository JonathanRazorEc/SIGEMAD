using AutoMapper;
using DGPCE.Sigemad.Application.Contracts.Persistence;
using DGPCE.Sigemad.Application.Exceptions;
using DGPCE.Sigemad.Application.Specifications.Ope.Administracion.OpePorcentajesOcupacionAreasEstacionamiento;
using DGPCE.Sigemad.Domain.Modelos.Ope.Administracion;

namespace DGPCE.Sigemad.Application.Contracts.Ope.Administracion.OpePorcentsOcAE;
public class OpePorcentOcAEService : IOpePorcentOcAEService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public OpePorcentOcAEService(
        IUnitOfWork unitOfWork,
        IMapper mapper
        )
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task ValidarRegistrosDuplicados(int? id, int idOpeOcupacion)
    {
        var specParamsBusquedaDuplicados = new OpePorcentajesOcupacionAreasEstacionamientoSpecificationParams
        {
            IdOpeOcupacion = idOpeOcupacion,
        };

        var specBusquedaDuplicados = new OpePorcentajesOcupacionAreasEstacionamientoSpecification(specParamsBusquedaDuplicados);
        var opePorcentsOcAE = await _unitOfWork.Repository<OpePorcentajeOcupacionAreaEstacionamiento>().GetAllWithSpec(specBusquedaDuplicados);

        // Filtrar duplicados
        var registrosDuplicados = opePorcentsOcAE
            .Where(x =>
                id == null || x.Id != id.Value
            )
            .ToList();

        if (registrosDuplicados.Any())
        {
            throw new BadRequestException("Ya existe un porcentaje de ocupación para esa ocupación.");
        }
    }

}
