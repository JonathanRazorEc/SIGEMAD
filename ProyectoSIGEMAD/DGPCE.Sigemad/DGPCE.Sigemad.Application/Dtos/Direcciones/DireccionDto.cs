using DGPCE.Sigemad.Domain.Enums;
using DGPCE.Sigemad.Domain.Modelos;

namespace DGPCE.Sigemad.Application.Dtos.Direcciones;
public class DireccionDto
{
    public int Id { get; set; }
    public TipoDireccionEmergencia TipoDireccionEmergencia { get; set; }

    public int tipoFormulario = (int)TipoFormularioEnum.Direccion;

    public string AutoridadQueDirige { get; set; }
    public DateTime FechaInicio { get; set; }
    public DateTime? FechaFin { get; set; }
    public Archivo? Archivo { get; set; }
    public bool EsEliminable { get; set; }
}
