using DGPCE.Sigemad.Application.Dtos.Common;

namespace DGPCE.Sigemad.Application.Dtos.OtraInformaciones;
public class OtraInformacionDto : BaseDto<int>
{
    public int IdSuceso { get; set; }

    public List<DetalleOtraInformacionDto> Lista { get; set; } = new();
}
