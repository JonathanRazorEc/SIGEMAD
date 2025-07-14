using AutoMapper;
using DGPCE.Sigemad.Application.Contracts.Persistence;
using DGPCE.Sigemad.Application.Exceptions;
using DGPCE.Sigemad.Application.Specifications.Ope.Datos.OpeDatosAsistencias;
using DGPCE.Sigemad.Domain.Modelos.Ope.Datos;

namespace DGPCE.Sigemad.Application.Contracts.Ope.Datos.OpeDatosAsistencias;
public class OpeDatoAsistenciaService : IOpeDatoAsistenciaService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public OpeDatoAsistenciaService(
        IUnitOfWork unitOfWork,
        IMapper mapper
        )
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }


    public async Task ValidarRegistrosDuplicados(int? id, int idOpePuerto, DateTime fecha)
    {
        var specParamsBusquedaDuplicados = new OpeDatosAsistenciasSpecificationParams
        {
            //Id = request.Id,
            //IdOpePuerto = idOpePuerto,
            IdsOpePuertos = new List<int> { idOpePuerto },
            FechaInicio = DateOnly.FromDateTime(fecha),
            FechaFin = DateOnly.FromDateTime(fecha),
        };

        var specBusquedaDuplicados = new OpeDatosAsistenciasSpecification(specParamsBusquedaDuplicados);
        var opeDatosAsistencias = await _unitOfWork.Repository<OpeDatoAsistencia>().GetAllWithSpec(specBusquedaDuplicados);

        // Filtrar duplicados
        var registrosDuplicados = opeDatosAsistencias
            .Where(x => id == null || x.Id != id.Value)
            .ToList();

        if (registrosDuplicados.Any())
        {
            //_logger.LogWarning($"Ya existe otro ope dato de Asistencia para la Asistencia con la misma fecha y con el mismo intervalo horario: {request.IdOpeDatoAsistenciaIntervaloHorario}");
            throw new BadRequestException("Ya existe un dato de asistencia para ese puerto y fecha.");
        }
    }

}
