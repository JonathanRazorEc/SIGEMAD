using AutoMapper;
using DGPCE.Sigemad.Application.Contracts.Persistence;
using DGPCE.Sigemad.Application.Exceptions;
using DGPCE.Sigemad.Application.Specifications.Ope.Administracion.OpeLineasMaritimas;
using DGPCE.Sigemad.Domain.Modelos.Ope.Administracion;

namespace DGPCE.Sigemad.Application.Contracts.Ope.Administracion.OpeLineasMaritimas;
public class OpeLineaMaritimaService : IOpeLineaMaritimaService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public OpeLineaMaritimaService(
        IUnitOfWork unitOfWork,
        IMapper mapper
        )
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task ValidarRegistrosDuplicados(int? id, int idOpePuertoOrigen, int idOpePuertoDestino, DateTime fechaValidezDesde, DateTime? fechaValidezHasta)
    {
        var specParamsBusquedaDuplicados = new OpeLineasMaritimasSpecificationParams
        {
            IdOpePuertoOrigen = idOpePuertoOrigen,
            IdOpePuertoDestino = idOpePuertoDestino,
        };

        var specBusquedaDuplicados = new OpeLineasMaritimasSpecification(specParamsBusquedaDuplicados);
        var opeLineasMaritimas = await _unitOfWork.Repository<OpeLineaMaritima>().GetAllWithSpec(specBusquedaDuplicados);

        // Filtrar duplicados
        var registrosDuplicados = opeLineasMaritimas
            .Where(x =>
                (id == null || x.Id != id.Value) &&
                x.FechaValidezDesde.Date <= (fechaValidezHasta ?? DateTime.MaxValue).Date &&
                fechaValidezDesde.Date <= (x.FechaValidezHasta ?? DateTime.MaxValue).Date
            )
            .ToList();

        if (registrosDuplicados.Any())
        {
            //_logger.LogWarning($"Ya existe otro ope dato de frontera para la frontera con la misma fecha y con el mismo intervalo horario: {request.IdOpeLineaMaritimaIntervaloHorario}");
            throw new BadRequestException("Ya existe una línea marítima activa para esas fechas.");
        }
    }

}
