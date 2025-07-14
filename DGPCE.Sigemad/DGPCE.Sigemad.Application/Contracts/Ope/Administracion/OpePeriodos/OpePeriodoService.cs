using AutoMapper;
using Azure.Core;
using DGPCE.Sigemad.Application.Contracts.Persistence;
using DGPCE.Sigemad.Application.Exceptions;
using DGPCE.Sigemad.Application.Specifications.Ope.Administracion.OpePeriodos;
using DGPCE.Sigemad.Domain.Modelos.Ope.Administracion;

namespace DGPCE.Sigemad.Application.Contracts.Ope.Administracion.OpePeriodos;
public class OpePeriodoService : IOpePeriodoService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public OpePeriodoService(
        IUnitOfWork unitOfWork,
        IMapper mapper
        )
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task ValidarRegistrosDuplicados(int? id, string nombre, DateTime FechaInicioFaseSalida, DateTime FechaFinFaseSalida,
        DateTime FechaInicioFaseRetorno, DateTime FechaFinFaseRetorno)
    {
        var specParamsBusquedaDuplicados = new OpePeriodosSpecificationParams();
        var specBusquedaDuplicados = new OpePeriodosSpecification(specParamsBusquedaDuplicados);
        var opePeriodos = await _unitOfWork.Repository<OpePeriodo>().GetAllWithSpec(specBusquedaDuplicados);

        // Filtrar duplicados
        var registrosDuplicados = opePeriodos
         .Where(x =>
             (id == null || x.Id != id.Value)
             && (
                 x.Nombre == nombre
                 ||
                 (x.FechaInicioFaseSalida <= FechaFinFaseSalida && FechaInicioFaseSalida <= x.FechaFinFaseSalida)
                 ||
                 (x.FechaInicioFaseRetorno <= FechaFinFaseRetorno && FechaInicioFaseRetorno <= x.FechaFinFaseRetorno)
             )
         )
         .ToList();

        if (registrosDuplicados.Any())
        {
            throw new BadRequestException("Ya existe un periodo con esos datos (nombre y/o fechas).");
        }
    }

}
