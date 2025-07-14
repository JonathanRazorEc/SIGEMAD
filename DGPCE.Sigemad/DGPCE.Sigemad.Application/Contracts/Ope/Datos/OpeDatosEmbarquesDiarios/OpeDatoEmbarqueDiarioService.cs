using AutoMapper;
using DGPCE.Sigemad.Application.Contracts.Persistence;
using DGPCE.Sigemad.Application.Exceptions;
using DGPCE.Sigemad.Application.Specifications.Ope.Datos.OpeDatosEmbarquesDiarios;
using DGPCE.Sigemad.Domain.Modelos.Ope.Administracion;
using DGPCE.Sigemad.Domain.Modelos.Ope.Datos;

namespace DGPCE.Sigemad.Application.Contracts.Ope.Datos.OpeDatosEmbarquesDiarios;
public class OpeDatoEmbarqueDiarioService : IOpeDatoEmbarqueDiarioService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public OpeDatoEmbarqueDiarioService(
        IUnitOfWork unitOfWork,
        IMapper mapper
        )
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }


    public async Task ValidarRegistrosDuplicados(int? id, int idOpeLineaMaritima, DateTime fecha)
    {
        var specParamsBusquedaDuplicados = new OpeDatosEmbarquesDiariosSpecificationParams
        {
            //Id = request.Id,
            //IdOpeLineaMaritima = idOpeLineaMaritima,
            IdsOpeLineasMaritimas = new List<int> { idOpeLineaMaritima },
            FechaInicio = DateOnly.FromDateTime(fecha),
            FechaFin = DateOnly.FromDateTime(fecha),
        };

        var specBusquedaDuplicados = new OpeDatosEmbarquesDiariosSpecification(specParamsBusquedaDuplicados);
        var opeDatosEmbarquesDiarios = await _unitOfWork.Repository<OpeDatoEmbarqueDiario>().GetAllWithSpec(specBusquedaDuplicados);

        // Filtrar duplicados
        var registrosDuplicados = opeDatosEmbarquesDiarios
            .Where(x => id == null || x.Id != id.Value)
            .ToList();

        if (registrosDuplicados.Any())
        {
            //_logger.LogWarning($"Ya existe otro ope dato de frontera para la frontera con la misma fecha y con el mismo intervalo horario: {request.IdOpeDatoEmbarqueDiarioIntervaloHorario}");
            throw new BadRequestException("Ya existe un dato de embarque diario para esa línea y esa fecha.");
        }
    }

}
