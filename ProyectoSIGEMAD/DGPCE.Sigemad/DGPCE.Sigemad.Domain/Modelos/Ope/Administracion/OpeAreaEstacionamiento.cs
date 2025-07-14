using DGPCE.Sigemad.Domain.Common;

namespace DGPCE.Sigemad.Domain.Modelos.Ope.Administracion;

public class OpeAreaEstacionamiento : BaseDomainModel<int>
{
    public string Nombre { get; set; } = null!;
    public int IdCcaa { get; set; }
    public int IdProvincia { get; set; }
    public int IdMunicipio { get; set; }
    public string Carretera { get; set; } = null!;
    public decimal PK { get; set; }
    public int CoordenadaUTM_X { get; set; }
    public int CoordenadaUTM_Y { get; set; }
    public bool InstalacionPortuaria { get; set; }
    public int? IdOpePuerto { get; set; }
    public int Capacidad { get; set; }

    public virtual Ccaa Ccaa { get; set; } = null!;
    public virtual Provincia Provincia { get; set; } = null!;
    public virtual Municipio Municipio { get; set; } = null!;
    public virtual OpePuerto OpePuerto { get; set; } = null!;

}
