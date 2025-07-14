using DGPCE.Sigemad.Domain.Modelos;

namespace DGPCE.Sigemad.Application.Specifications.OtrasInformaciones;
public class DetalleOtraInformacionByIdOtraInformacionSpecification : BaseSpecification<DetalleOtraInformacion>
{
    public DetalleOtraInformacionByIdOtraInformacionSpecification(int idOtraInformacion)
        : base(i => i.IdOtraInformacion == idOtraInformacion && i.Borrado == false)
    {
        AddInclude(i => i.ProcedenciasDestinos);
    }
}

