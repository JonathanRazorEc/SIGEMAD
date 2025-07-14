using DGPCE.Sigemad.Application.Dtos.Evoluciones;
using MediatR;

namespace DGPCE.Sigemad.Application.Features.Evoluciones.Queries.GetEvolucion;
public class GetEvolucionQuery : IRequest<EvolucionDto>
{
    public int? IdRegistroActualizacion { get; set; }
    public int IdSuceso { get; set; }
}
