using DGPCE.Sigemad.Application.Dtos.Utilitarios;
using MediatR;

namespace DGPCE.Sigemad.Application.Features.Utilitarios.Queries.GetTestResponseTime;
public class GetTestResponseTimeQuery : IRequest<TestResponseTimeDto>
{
    public int Milisegundos { get; set; }
}
