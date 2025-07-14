using DGPCE.Sigemad.Application.Contracts.Persistence;
using DGPCE.Sigemad.Application.Features.EstadosIncendio.Queries.GetEstadoBySucesoId;
using DGPCE.Sigemad.Application.Specifications.Evoluciones;
using DGPCE.Sigemad.Application.Specifications.Registros;
using DGPCE.Sigemad.Domain.Modelos;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace DGPCE.Sigemad.Application.Features.EstadosIncendio.Queries;

public class GetEstadoIncendioBySucesoIdQueryHandler : IRequestHandler<GetEstadoIncendioBySucesoIdQuery, string?>
{
    private readonly IUnitOfWork _unitOfWork;

    public GetEstadoIncendioBySucesoIdQueryHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<string?> Handle(GetEstadoIncendioBySucesoIdQuery request, CancellationToken cancellationToken)
    {
        //var evolucion = await _unitOfWork.Repository<Evolucion>()
        //    .GetQuery()
        //    .Include(e => e.Parametros)
        //        .ThenInclude(p => p.EstadoIncendio)
        //    .Where(e => e.IdSuceso == request.IdSuceso && !e.Borrado)
        //    .OrderByDescending(e => e.FechaCreacion)
        //    .FirstOrDefaultAsync(cancellationToken);

        var registroParams = new RegistroSpecificationParams
        {
            IdSuceso = request.IdSuceso
        };

        var spec = new RegistroSpecification(registroParams);
        var registro = await _unitOfWork.Repository<Registro>()
        .GetFirstOrDefaultAsync(spec);

        var estadoIncendio = registro?.Parametros
            .Where(p => !p.Borrado)
            .OrderByDescending(p => p.FechaCreacion)
            .FirstOrDefault()?.EstadoIncendio?.Descripcion;

        return estadoIncendio;
    }
}
