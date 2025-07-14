using DGPCE.Sigemad.Application.Dtos.CoordinacionCecopis;
using DGPCE.Sigemad.Application.Dtos.CoordinacionesPMA;
using DGPCE.Sigemad.Application.Dtos.Direcciones;
using MediatR;

namespace DGPCE.Sigemad.Application.Features.DireccionCoordinacionEmergencias.Commands;
public class CreateOrUpdateDireccionCoordinacionCommand : IRequest<CreateOrUpdateDireccionResponse>
{
    public int? IdRegistroActualizacion { get; set; }
    public int IdSuceso { get; set; }
    public List<CreateOrUpdateDireccionDto> Direcciones { get; set; } = new();
    public List<CreateOrUpdateCoordinacionPmaDto> CoordinacionesPMA { get; set; } = new();
    public List<CreateOrUpdateCoordinacionCecopiDto> CoordinacionesCECOPI { get; set; } = new();
}
