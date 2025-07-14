using DGPCE.Sigemad.Domain.Common;

namespace DGPCE.Sigemad.Domain.Modelos;

public class Suceso: BaseDomainModel<int>
{
    public Suceso()
    {
        Incendios = new();
        Documentaciones = new();
        OtraInformaciones = new();
        SucesoRelacionados = new();
        ActuacionesRelevantes = new();
        //Evoluciones = new();
        RegistroActualizaciones = new();
    }

    public int IdTipo { get; set; }
    public virtual TipoSuceso TipoSuceso { get; set; } = null!;


    public virtual List<Incendio> Incendios { get; set; }


    // Datos del suceso
    public virtual List<Registro> Registros { get; set; }
    //public virtual List<Evolucion> Evoluciones { get; set; }
    //public virtual Evolucion Evolucion => Evoluciones.FirstOrDefault(e => e.EsFoto == false);
    //public virtual DireccionCoordinacionEmergencia DireccionCoordinacionEmergencia { get; set; }
    public virtual List<Documentacion> Documentaciones { get; set; }
    public virtual List<OtraInformacion> OtraInformaciones { get; set; }
    public virtual List<SucesoRelacionado> SucesoRelacionados { get; set; }
    public virtual List<ActuacionRelevanteDGPCE> ActuacionesRelevantes { get; set; }

    public virtual List<RegistroActualizacion> RegistroActualizaciones { get; set; }
}
