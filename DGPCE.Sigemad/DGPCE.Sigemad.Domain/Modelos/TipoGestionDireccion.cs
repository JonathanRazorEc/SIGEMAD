using DGPCE.Sigemad.Domain.Common;

namespace DGPCE.Sigemad.Domain.Modelos;
public class TipoGestionDireccion: EditableCatalogModel
{
    public int Id { get; set; }
    public string Descripcion { get; set; } = null!;
    public int Formulario { get; set; }

}
