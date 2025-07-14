using DGPCE.Sigemad.Domain.Modelos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DGPCE.Sigemad.Application.Specifications.Documentos;
public class DocumentacionWithFilteredData : BaseSpecification<Documentacion>
{
    public DocumentacionWithFilteredData(int id, List<int> idsDetallesDocumentacion)
     : base(d => d.Id == id && d.Borrado == false)
    {
        if (idsDetallesDocumentacion.Any())
        {
            AddInclude(d => d.DetallesDocumentacion.Where(dir => idsDetallesDocumentacion.Contains(dir.Id) && !dir.Borrado));
            AddInclude("DetallesDocumentacion.TipoDocumento");
            AddInclude("DetallesDocumentacion.Archivo");
            AddInclude("DetallesDocumentacion.DocumentacionProcedenciaDestinos");
            AddInclude("DetallesDocumentacion.DocumentacionProcedenciaDestinos.ProcedenciaDestino");
            
        }
    }
}
