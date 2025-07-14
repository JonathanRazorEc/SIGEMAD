using DGPCE.Sigemad.Application.Features.CCAA.Vms;
using DGPCE.Sigemad.Application.Features.Municipios.Vms;
using DGPCE.Sigemad.Application.Features.Provincias.Vms;
using DGPCE.Sigemad.Domain.Modelos;

namespace DGPCE.Sigemad.Application.Features.TipoIntervencionMedios.Vms;
public class TipoIntervencionMedioVm
{
    public int Id { get; set; }
    public string Descripcion { get; set; } = null!;
    public string? TitularidadOtra { get; set; }

    public ClasificacionMedio ClasificacionMedio { get; set; } = null!;
    public TitularidadMedio TitularidadMedio { get; set; } = null!;
    public TipoEntidadTitularidadMedio TipoEntidadTitularidadMedio { get; set; } = null!;
    public ComunidadesAutonomasSinProvinciasVm TitularidadAutonomica { get; set; } = null!;
    public ComunidadesAutonomasSinProvinciasVm TitularidadAutonomicaMunicipal { get; set; } = null!;
    public ProvinciaSinMunicipiosConIdComunidadVm TitularidadProvinciaMunicipal { get; set; } = null!;
    public MunicipioConIdProvincia TitularidadMunicipal { get; set; } = null!;
    public Pais TitularidadPais { get; set; } = null!;
}
