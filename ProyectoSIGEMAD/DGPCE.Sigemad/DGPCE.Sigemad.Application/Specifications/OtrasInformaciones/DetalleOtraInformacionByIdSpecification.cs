using DGPCE.Sigemad.Domain.Modelos;

namespace DGPCE.Sigemad.Application.Specifications.OtrasInformaciones;
public class DetalleOtraInformacionByIdSpecification : BaseSpecification<DetalleOtraInformacion>
{
    public DetalleOtraInformacionByIdSpecification(int idDetalle)
        : base(i => i.Id == idDetalle && i.Borrado == false)
    {
        AddInclude(i => i.ProcedenciasDestinos);
    }
}
