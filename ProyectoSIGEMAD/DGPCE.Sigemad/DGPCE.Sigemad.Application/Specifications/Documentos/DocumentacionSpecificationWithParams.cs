using DGPCE.Sigemad.Domain.Modelos;

namespace DGPCE.Sigemad.Application.Specifications.Documentos;
public class DocumentacionSpecificationWithParams : BaseSpecification<Documentacion>
{
    public DocumentacionSpecificationWithParams(DocumentacionParams @params)
    : base(d =>
        (!@params.Id.HasValue || d.Id == @params.Id) &&
        (!@params.IdSuceso.HasValue || d.IdSuceso == @params.IdSuceso) &&
         d.Borrado == false)
    {
        AddInclude(d => d.DetallesDocumentacion);
        AddInclude("DetallesDocumentacion.TipoDocumento");
        AddInclude("DetallesDocumentacion.Archivo");
        AddInclude("DetallesDocumentacion.DocumentacionProcedenciaDestinos");
        
    }
}
