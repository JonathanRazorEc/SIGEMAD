using DGPCE.Sigemad.Domain.Common;


namespace DGPCE.Sigemad.Domain.Modelos;

public class Evolucion : BaseDomainModel<int>
{
    public int IdSuceso { get; set; }
    public Suceso Suceso { get; set; }
    public bool EsFoto { get; set; }



}
