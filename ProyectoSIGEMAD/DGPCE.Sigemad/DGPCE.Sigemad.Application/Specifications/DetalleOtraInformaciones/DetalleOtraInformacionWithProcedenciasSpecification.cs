using DGPCE.Sigemad.Domain.Modelos;

namespace DGPCE.Sigemad.Application.Specifications.DetalleOtraInformaciones;
public class DetalleOtraInformacionWithProcedenciasSpecification : BaseSpecification<DetalleOtraInformacion>
{
    public DetalleOtraInformacionWithProcedenciasSpecification(int detalleId)
        : base(d => d.Id == detalleId)
    {
        AddInclude(d => d.ProcedenciasDestinos);
    }
}

