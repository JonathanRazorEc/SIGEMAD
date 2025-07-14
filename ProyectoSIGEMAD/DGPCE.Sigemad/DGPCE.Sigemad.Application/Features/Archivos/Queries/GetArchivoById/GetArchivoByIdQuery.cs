using DGPCE.Sigemad.Application.Dtos.Archivos;
using MediatR;

namespace DGPCE.Sigemad.Application.Features.Archivos.Queries.GetArchivoById;
public class GetArchivoByIdQuery: IRequest<ArchivoDownloadDto>
{
    public Guid Id { get; set; }

    public GetArchivoByIdQuery(Guid id)
    {
        Id = id;
    }
}
