using DGPCE.Sigemad.Domain.Common;

namespace DGPCE.Sigemad.Domain.Modelos;
public class Direccion : BaseDomainModel<int>
{
    public int IdRegistro { get; set; }
    public virtual Registro Registro { get; set; } = null!;

   // public int IdTipoGestionDireccion { get; set; }
    //public virtual TipoGestionDireccion TipoGestionDireccion { get; set; }

    public int IdTipoDireccionEmergencia { get; set; }
    public virtual TipoDireccionEmergencia TipoDireccionEmergencia { get; set; }

    public string AutoridadQueDirige { get; set; }
    public DateTime FechaInicio { get; set; }
    public DateTime? FechaFin { get; set; }

    public Guid? IdArchivo { get; set; }
    public Archivo? Archivo { get; set; }


}
