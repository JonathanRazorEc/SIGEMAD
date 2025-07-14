
using DGPCE.Sigemad.Application.Dtos.OtraInformaciones;
using MediatR;

namespace DGPCE.Sigemad.Application.Features.OtrasInformaciones.Queries.GetOtraInformacion;
public class GetOtraInformacionQuery : IRequest<OtraInformacionDto>
{
    public int? IdRegistroActualizacion { get; set; }
    public int IdSuceso { get; set; }
}
