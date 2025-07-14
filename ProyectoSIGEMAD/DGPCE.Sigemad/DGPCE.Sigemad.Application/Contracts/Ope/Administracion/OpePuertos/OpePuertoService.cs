using AutoMapper;
using DGPCE.Sigemad.Application.Contracts.Persistence;
using DGPCE.Sigemad.Application.Exceptions;
using DGPCE.Sigemad.Application.Specifications.Ope.Administracion.OpePuertos;
using DGPCE.Sigemad.Domain.Modelos.Ope.Administracion;

namespace DGPCE.Sigemad.Application.Contracts.Ope.Administracion.OpePuertos;
public class OpePuertoService : IOpePuertoService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public OpePuertoService(
        IUnitOfWork unitOfWork,
        IMapper mapper
        )
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task ValidarRegistrosDuplicados(int? id, string nombre, DateTime fechaValidezDesde, DateTime? fechaValidezHasta)
    {
        var specParamsBusquedaDuplicados = new OpePuertosSpecificationParams
        {
            Nombre = nombre,
        };

        var specBusquedaDuplicados = new OpePuertosSpecification(specParamsBusquedaDuplicados);
        var opePuertos = await _unitOfWork.Repository<OpePuerto>().GetAllWithSpec(specBusquedaDuplicados);

        // Filtrar duplicados
        var registrosDuplicados = opePuertos
            .Where(x =>
                (id == null || x.Id != id.Value) &&
                x.FechaValidezDesde.Date <= (fechaValidezHasta ?? DateTime.MaxValue).Date &&
                fechaValidezDesde.Date <= (x.FechaValidezHasta ?? DateTime.MaxValue).Date
            )
            .ToList();

        if (registrosDuplicados.Any())
        {
            throw new BadRequestException("Ya existe un puerto activo con ese nombre en esas fechas.");
        }
    }

}
