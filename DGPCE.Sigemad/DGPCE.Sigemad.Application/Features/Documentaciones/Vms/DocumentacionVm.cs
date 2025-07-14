using DGPCE.Sigemad.Domain.Modelos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DGPCE.Sigemad.Application.Features.Documentaciones.Vms;
public class DocumentacionVm
{
    public int IdIncendio { get; set; }
    public DateTime FechaHora { get; set; }
    public DateTime FechaHoraSolicitud { get; set; }
    public int IdTipoDocumento { get; set; }
    public string Descripcion { get; set; }
    public Guid? IdArchivo { get; set; }

    public List<DetalleDocumentacion>? DetalleDocumentaciones { get; set; } = null;
}
