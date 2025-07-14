using DGPCE.Sigemad.Application.Dtos.Common;
using DGPCE.Sigemad.Application.Dtos.CoordinacionCecopis;
using DGPCE.Sigemad.Application.Dtos.CoordinacionesPMA;
using DGPCE.Sigemad.Application.Dtos.Direcciones;

namespace DGPCE.Sigemad.Application.Dtos.DireccionCoordinaciones;
public class DireccionCoordinacionEmergenciaDto : BaseDto<int>
{
    public int IdSuceso { get; set; }
    public List<DireccionDto> Direcciones { get; set; } = new();
    public List<CoordinacionCecopiDto> CoordinacionesCecopi { get; set; } = new();
    public List<CoordinacionPMADto> CoordinacionesPMA { get; set; } = new();
}
