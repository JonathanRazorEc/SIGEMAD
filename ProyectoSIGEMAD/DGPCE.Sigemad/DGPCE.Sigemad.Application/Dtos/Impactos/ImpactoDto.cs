using DGPCE.Sigemad.Application.Dtos.Municipios;
using DGPCE.Sigemad.Application.Features.Provincias.Vms;
using DGPCE.Sigemad.Domain.Modelos;
using NetTopologySuite.Geometries;

namespace DGPCE.Sigemad.Application.Dtos.Impactos;
public class ImpactoDto
{
    public int Id { get; set; }
    public string? Causa { get; set; }
    public DateTime? Fecha { get; set; }
    public DateTime? FechaHora { get; set; }
    public DateTime? FechaHoraInicio { get; set; }
    public DateTime? FechaHoraFin { get; set; }
    public int? Numero { get; set; }
    public int? NumeroGraves { get; set; }
    public int? NumeroIntervinientes { get; set; }
    public int? NumeroLocalidades { get; set; }
    public int? NumeroServicios { get; set; }
    public int? NumeroUsuarios { get; set; }
    public string? Observaciones { get; set; }

    public DateTime? ExtraFechaHora1 { get; set; }
    public DateTime? ExtraFechaHora2 { get; set; }

    public int? ExtraNumerico1 { get; set; }
    public int? ExtraNumerico2 { get; set; }
    public int? ExtraNumerico3 { get; set; }

    public string? ExtraCaracter1 { get; set; }

    public virtual TipoDanio? TipoDanio { get; set; }
    public virtual ProvinciaSinMunicipiosVm? Provincia { get; set; }
    public virtual MunicipioDto? Municipio { get; set; }

    public virtual ImpactoClasificado ImpactoClasificado { get; set; }

    public virtual AlteracionInterrupcion AlteracionInterrupcion { get; set; }

    public virtual ZonaPlanificacion? ZonaPlanificacion { get; set; }

    // AUDITORÍA REAL
    public DateTime FechaCreacion { get; set; }
    public DateTime? FechaModificacion { get; set; }

    // NUEVOS INDICADORES
    public bool EsNuevo { get; set; }
    public bool EsModificado { get; set; }


}
