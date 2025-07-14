using DGPCE.Sigemad.Domain.Common;

namespace DGPCE.Sigemad.Domain.Modelos;
public class ValidacionImpactoClasificado : EditableCatalogModel
{
    public ValidacionImpactoClasificado()
    {

    }

    public int Id { get; set; }
    public int IdImpactoClasificado { get; set; }
    public string Campo { get; set; }
    public string TipoCampo { get; set; }
    public bool EsObligatorio { get; set; }
    public string Etiqueta { get; set; }

    public bool Orden { get; set; }

    public virtual ImpactoClasificado ImpactoClasificado { get; set; } = null!;

}
