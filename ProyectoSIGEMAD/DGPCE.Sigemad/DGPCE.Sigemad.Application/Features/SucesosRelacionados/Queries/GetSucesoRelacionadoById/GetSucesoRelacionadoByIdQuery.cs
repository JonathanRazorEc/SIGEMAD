using DGPCE.Sigemad.Application.Features.SucesosRelacionados.Vms;
using MediatR;

namespace DGPCE.Sigemad.Application.Features.SucesosRelacionados.Queries.GetSucesoRelacionadoById;
public class GetSucesoRelacionadoByIdQuery : IRequest<SucesoRelacionadoVm>
{
    public int Id { get; set; }
    public GetSucesoRelacionadoByIdQuery(int id)
    {
        Id = id;
    }
}
