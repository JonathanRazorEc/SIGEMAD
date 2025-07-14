using DGPCE.Sigemad.Application.Features.SucesosRelacionados.Vms;
using MediatR;

namespace DGPCE.Sigemad.Application.Features.SucesosRelacionados.Queries.GetSucesoRelacionado;
public class GetSucesoRelacionadoQuery : IRequest<SucesoRelacionadoVm>
{
    public int? IdRegistroActualizacion { get; set; }
    public int IdSuceso { get; set; }
}
