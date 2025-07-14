using DGPCE.Sigemad.Application.Dtos.Common;
using DGPCE.Sigemad.Application.Dtos.Sucesos;

namespace DGPCE.Sigemad.Application.Features.SucesosRelacionados.Vms;
public class SucesoRelacionadoVm: BaseDto<int>
{
    public int IdSuceso { get; set; }
    public List<SucesoGridDto> SucesosAsociados { get; set; } = new();
}
