using DGPCE.Sigemad.Domain.Modelos;


namespace DGPCE.Sigemad.Application.Specifications.DetallesDocumentacion;

public class DetalleDocumentacionSpecification : BaseSpecification<DetalleDocumentacion>
{
    public DetalleDocumentacionSpecification(int detalleId)
        : base(d => d.Id == detalleId)
    {
        AddInclude(d => d.DocumentacionProcedenciaDestinos);
    }
}
