using DGPCE.Sigemad.Application.Dtos.Common;

namespace DGPCE.Sigemad.Application.Dtos.Archivos;
public class ArchivoDto : BaseDto<Guid>
{
    public string NombreOriginal { get; set; }
    public string Tipo { get; set; }
    public string Extension { get; set; }
    public long PesoEnBytes { get; set; }
}
