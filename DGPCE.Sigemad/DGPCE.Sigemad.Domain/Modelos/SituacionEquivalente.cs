using DGPCE.Sigemad.Domain.Common;

namespace DGPCE.Sigemad.Domain.Modelos;
public class SituacionEquivalente : EditableCatalogModel
{
    public int Id { get; set; }
    public string Descripcion { get; set; }
    public bool Obsoleto { get; set; }
    public int Prioridad { get; set; }
}
