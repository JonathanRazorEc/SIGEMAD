
using DGPCE.Sigemad.Domain.Modelos;

namespace DGPCE.Sigemad.Application.Specifications.Documentos;
internal class DocumentacionSpecification : BaseSpecification<Documentacion>
{
    public DocumentacionSpecification(int id)
        : base(d => d.Id == id && d.Borrado == false)
    {
        AddInclude(d => d.DetallesDocumentacion);
        AddInclude("DetallesDocumentacion.DocumentacionProcedenciaDestinos.ProcedenciaDestino");
        AddInclude("DetallesDocumentacion.TipoDocumento");
        AddInclude("DetallesDocumentacion.Archivo");
    }
}
