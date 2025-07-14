using DGPCE.Sigemad.Domain.Common;
using DGPCE.Sigemad.Domain.Modelos.Ope.Administracion;

namespace DGPCE.Sigemad.Domain.Modelos.Ope.Datos;
public class OpeDatoEmbarqueDiario : BaseDomainModel<int>
{
    public int IdOpeLineaMaritima { get; set; }
    public DateTime Fecha { get; set; }
    public int NumeroRotaciones { get; set; }
    public int NumeroPasajeros { get; set; }
    public int NumeroTurismos { get; set; }
    public int NumeroAutocares { get; set; }
    public int NumeroCamiones { get; set; }
    public int NumeroTotalVehiculos { get; set; }

    public virtual OpeLineaMaritima OpeLineaMaritima { get; set; }
}
