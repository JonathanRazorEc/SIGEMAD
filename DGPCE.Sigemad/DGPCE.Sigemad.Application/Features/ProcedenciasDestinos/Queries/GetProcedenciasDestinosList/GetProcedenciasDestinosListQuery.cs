using DGPCE.Sigemad.Domain.Modelos;
using MediatR;

namespace DGPCE.Sigemad.Application.Features.ProcedenciasDestinos.Queries.GetProcedenciasDestinosList
{
    public class GetProcedenciasDestinosListQuery : IRequest<IReadOnlyList<ProcedenciaDestino>>
    {
    }
}
