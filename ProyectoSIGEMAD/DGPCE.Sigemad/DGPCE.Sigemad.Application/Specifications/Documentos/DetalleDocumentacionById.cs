using DGPCE.Sigemad.Domain.Modelos;

namespace DGPCE.Sigemad.Application.Specifications.Documentos;
public class DetalleDocumentacionById : BaseSpecification<Documentacion>
{
    public DetalleDocumentacionById(DocumentacionParams @params)
       : base(d =>
        (!@params.Id.HasValue || d.Id == @params.Id) &&
        (!@params.IdSuceso.HasValue || d.IdSuceso == @params.IdSuceso) &&
         d.Borrado == false)
    {
        AddInclude(i => i.DetallesDocumentacion.Where(d => !d.Borrado));
        AddInclude("DetallesDocumentacion.DocumentacionProcedenciaDestinos.ProcedenciaDestino");
        AddInclude("DetallesDocumentacion.TipoDocumento");
        AddInclude("DetallesDocumentacion.Archivo");

    }
}
