using DGPCE.Sigemad.Application.Dtos.Utilitarios;
using MediatR;

namespace DGPCE.Sigemad.Application.Features.Utilitarios.Queries.GetTestResponseTime;
public class GetTestResponseTimeQueryHandler : IRequestHandler<GetTestResponseTimeQuery, TestResponseTimeDto>
{

    public async Task<TestResponseTimeDto> Handle(GetTestResponseTimeQuery request, CancellationToken cancellationToken)
    {
        var startTime = DateTime.Now;

        await Task.Delay(request.Milisegundos);

        var endTime = DateTime.Now;

        return new TestResponseTimeDto
        {
            BeginDatetime = startTime,
            EndDatetime = endTime,
            ElapsedMilliseconds = (endTime - startTime).TotalMilliseconds
        };
    }
}
