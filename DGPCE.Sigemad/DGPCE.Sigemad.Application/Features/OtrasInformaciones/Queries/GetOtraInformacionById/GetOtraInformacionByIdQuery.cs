using DGPCE.Sigemad.Application.Dtos.OtraInformaciones;
using MediatR;

namespace DGPCE.Sigemad.Application.Features.OtrasInformaciones.Queries.GetOtrasInformacionesList;
public class GetOtraInformacionByIdQuery : IRequest<OtraInformacionDto>
{
    public int Id { get; set; }

    public GetOtraInformacionByIdQuery(int id)
    {
        Id = id;
    }
}
