using AutoMapper;
using DGPCE.Sigemad.Application.Contracts.Persistence;
using DGPCE.Sigemad.Application.Exceptions;
using DGPCE.Sigemad.Application.Specifications.Ope.Datos.OpeDatosFronteras;
using DGPCE.Sigemad.Application.Specifications.Ope.Datos.OpeDatosFronterasIntervalosHorarios;
using DGPCE.Sigemad.Domain.Modelos.Ope.Administracion;
using DGPCE.Sigemad.Domain.Modelos.Ope.Datos;

namespace DGPCE.Sigemad.Application.Contracts.Ope.Datos.OpeDatosFronteras;
public class OpeDatoFronteraService : IOpeDatoFronteraService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public OpeDatoFronteraService(
        IUnitOfWork unitOfWork,
        IMapper mapper
        )
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task ValidarHoraInicioMenorHoraFinIntervaloHorarioPersonalizado(TimeSpan? inicio, TimeSpan? fin)
    {
        if (inicio is null || fin is null)
        {
            return;
        }
        if (fin <= inicio)
        {
            throw new BadRequestException("La hora de fin debe ser posterior a la hora de inicio.");
        }
    }

    public async Task ValidarHoraDentroIntervaloHorario(TimeSpan? hora, int idOpeDatoFronteraIntervaloHorario)
    {
        if (hora is null)
        {
            return;
        }

        var opeDatoFronteraIntervaloHorarioParams = new OpeDatosFronterasIntervalosHorariosSpecificationParams
        {
            Id = idOpeDatoFronteraIntervaloHorario,
        };

        var specOpeDatoFronteraIntervaloHorario = new OpeDatosFronterasIntervalosHorariosSpecification(opeDatoFronteraIntervaloHorarioParams);
        var opeDatoFronteraIntervaloHorario = await _unitOfWork.Repository<OpeDatoFronteraIntervaloHorario>().GetByIdWithSpec(specOpeDatoFronteraIntervaloHorario);

        if (opeDatoFronteraIntervaloHorario == null)
        {
            //_logger.LogWarning($"No se encontro dato de frontera intervalo horario de OPE con id: {idOpeDatoFronteraIntervaloHorario}");
            throw new NotFoundException(nameof(OpeDatoFronteraIntervaloHorario), idOpeDatoFronteraIntervaloHorario);
        }

        if (hora < opeDatoFronteraIntervaloHorario.Inicio || hora > opeDatoFronteraIntervaloHorario.Fin)
        {
            throw new BadRequestException("La hora personalizada está fuera del intervalo.");
        }

    }


    //(public async Task ValidarRegistrosDuplicados(UpdateOpeDatoFronteraCommand request)
    public async Task ValidarRegistrosDuplicados(int? id, int idOpeFrontera, DateTime fecha, int idOpeDatoFronteraIntervaloHorario, TimeSpan horaInicio, TimeSpan horaFin)
    {
        var specParamsBusquedaDuplicados = new OpeDatosFronterasSpecificationParams
        {
            //Id = request.Id,
            //IdOpeFrontera = idOpeFrontera,
            IdsOpeFronteras = new List<int> { idOpeFrontera },
            //Fecha = DateOnly.FromDateTime(fecha),
            FechaInicio = DateOnly.FromDateTime(fecha),
            FechaFin = DateOnly.FromDateTime(fecha),
            IdOpeOpeDatoFronteraIntervaloHorario = idOpeDatoFronteraIntervaloHorario
        };

        var specBusquedaDuplicados = new OpeDatosFronterasSpecification(specParamsBusquedaDuplicados);
        var opeDatosFronteras = await _unitOfWork.Repository<OpeDatoFrontera>().GetAllWithSpec(specBusquedaDuplicados);

        // Filtrar duplicados
        var registrosDuplicados = opeDatosFronteras
            .Where(x => (id == null || x.Id != id.Value) &&
                (
                    !x.IntervaloHorarioPersonalizado &&
                        x.OpeDatoFronteraIntervaloHorario.Inicio == horaInicio &&
                        x.OpeDatoFronteraIntervaloHorario.Fin == horaFin
                    
                    ||
                    x.IntervaloHorarioPersonalizado &&
                        x.InicioIntervaloHorarioPersonalizado.HasValue &&
                        x.FinIntervaloHorarioPersonalizado.HasValue &&
                        x.InicioIntervaloHorarioPersonalizado.Value == horaInicio &&
                        x.FinIntervaloHorarioPersonalizado.Value == horaFin
                    
                )
            )
            .ToList();

        if (registrosDuplicados.Any())
        {
            //_logger.LogWarning($"Ya existe otro ope dato de frontera para la frontera con la misma fecha y con el mismo intervalo horario: {request.IdOpeDatoFronteraIntervaloHorario}");
            throw new BadRequestException("Ya existe un intervalo horario con esos datos de inicio / fin para esta frontera y fecha.");
        }
    }

}
