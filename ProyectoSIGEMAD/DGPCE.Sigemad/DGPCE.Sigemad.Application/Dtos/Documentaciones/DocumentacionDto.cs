using DGPCE.Sigemad.Application.Dtos.Common;

namespace DGPCE.Sigemad.Application.Dtos.Documentaciones;
public class DocumentacionDto: BaseDto<int>
{
    public int IdSuceso { get; set; }

    public List<ItemDocumentacionDto> Detalles { get; set; } = new();
}
