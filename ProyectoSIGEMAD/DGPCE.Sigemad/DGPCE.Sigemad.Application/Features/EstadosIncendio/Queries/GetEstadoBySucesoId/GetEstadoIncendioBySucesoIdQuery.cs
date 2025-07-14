using MediatR;

namespace DGPCE.Sigemad.Application.Features.EstadosIncendio.Queries.GetEstadoBySucesoId;

public record GetEstadoIncendioBySucesoIdQuery(int IdSuceso) : IRequest<string?>;
