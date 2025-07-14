using DGPCE.Sigemad.Domain.Common;

namespace DGPCE.Sigemad.Domain.Modelos;
public class Registro : BaseDomainModel<int>
{
    public Registro()
    {
        ProcedenciaDestinos = new();
    }

    public DateTime? FechaHoraEvolucion { get; set; }

    //public int IdEvolucion { get; set; }
    //public virtual Evolucion Evolucion { get; set; } = null!;

    public int IdSuceso { get; set; }
    public virtual Suceso Suceso { get; set; } = null!;
    public int? IdMedio { get; set; }
    public virtual Medio Medio { get; set; } = null!;

    public int? IdEntradaSalida { get; set; }
    public virtual EntradaSalida EntradaSalida { get; set; } = null!;

    public virtual List<RegistroProcedenciaDestino> ProcedenciaDestinos { get; set; } = new();

    //Datos evolutivos
    public List<AreaAfectada> AreaAfectadas { get; set; } = new();

    public List<Parametro> Parametros { get; set; } = new();

    public virtual List<TipoImpactoEvolucion> TipoImpactosEvoluciones { get; set; } = new();
    public List<IntervencionMedio> IntervencionMedios { get; set; } = new();


    //Direccion y Coordinacion
    public List<Direccion> Direcciones { get; set; } = new();
    public List<CoordinacionCecopi> CoordinacionesCecopi { get; set; } = new();
    public List<CoordinacionPMA> CoordinacionesPMA { get; set; } = new();

    //Actuaciones relevantes y Direccion Coordinacion
    public virtual List<ActivacionPlanEmergencia> ActivacionPlanEmergencias { get; set; } = new();


    //Actuaciones Relevantes
    public virtual List<ActivacionSistema> ActivacionSistemas { get; set; } = new();


    public virtual AspNetUser CreadoPorNavigation { get; set; } = null!;
    public virtual AspNetUser ModificadoPorNavigation { get; set; } = null!;
    public virtual AspNetUser EliminadoPorNavigation { get; set; } = null!;
}

