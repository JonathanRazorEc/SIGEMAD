using DGPCE.Sigemad.Application.Dtos.Parametros.Dtos;
using DGPCE.Sigemad.Application.Features.Parametros.Manage;
using MediatR;


namespace DGPCE.Sigemad.Application.Features.Parametros.Commands
{
    public class ManageParametroCommand : IRequest<ManageParametroResponse>
    {
        public int IdSuceso { get; set; }
        public int? IdRegistroActualizacion { get; set; }
        public List<CreateOrUpdateParametroDto>? Parametro { get; set; } = new();
    }
}
