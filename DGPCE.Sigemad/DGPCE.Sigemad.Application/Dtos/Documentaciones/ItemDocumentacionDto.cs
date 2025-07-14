using DGPCE.Sigemad.Application.Dtos.Archivos;
using DGPCE.Sigemad.Application.Dtos.ProcedenciasDestinos;
using DGPCE.Sigemad.Domain.Modelos;

namespace DGPCE.Sigemad.Application.Dtos.Documentaciones;
public class ItemDocumentacionDto
{
    public int Id { get; set; }
    public DateTime FechaHora { get; set; }
    public DateTime FechaHoraSolicitud { get; set; }
    public TipoDocumento TipoDocumento { get; set; }
    public string Descripcion { get; set; }
    public ArchivoDto? Archivo { get; set; }
    public List<ProcedenciaDto> ProcedenciaDestinos { get; set; }

    public bool EsEliminable { get; set; }

}