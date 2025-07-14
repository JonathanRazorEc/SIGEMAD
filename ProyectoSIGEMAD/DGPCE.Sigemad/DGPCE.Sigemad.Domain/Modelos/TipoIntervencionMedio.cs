

namespace DGPCE.Sigemad.Domain.Modelos;
public class TipoIntervencionMedio
{
    public int Id { get; set; }
    public string Descripcion { get; set; } = null!;
    public int? IdClasificacion { get; set; }
    public int? IdTitularidad { get; set; }
    public int? IdTitularidadEstatal { get; set; }
    public int? IdTitularidadAutonomica { get; set; }
    public int? IdTitularidadAutonomicaMunicipal { get; set; }
    public int? IdTitularidadProvinciaMunicipal { get; set; }
    public int? IdTitularidadMunicipal { get; set; }
    public int? IdTitularidadPais { get; set; }
    public string? TitularidadOtra { get; set; }

    public ClasificacionMedio ClasificacionMedio { get; set; } = null!;
    public TitularidadMedio TitularidadMedio { get; set; } = null!;
    public TipoEntidadTitularidadMedio TipoEntidadTitularidadMedio { get; set; } = null!;
    public Ccaa TitularidadAutonomica { get; set; } = null!;
    public Ccaa TitularidadAutonomicaMunicipal { get; set; } = null!;
    public Provincia TitularidadProvinciaMunicipal { get; set; } = null!;
    public Municipio TitularidadMunicipal { get; set; } = null!;
    public Pais TitularidadPais { get; set; } = null!;
}
