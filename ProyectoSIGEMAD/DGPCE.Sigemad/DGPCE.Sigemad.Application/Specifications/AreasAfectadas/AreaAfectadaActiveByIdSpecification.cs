using DGPCE.Sigemad.Domain.Modelos;

namespace DGPCE.Sigemad.Application.Specifications.AreasAfectadas;
public class AreaAfectadaActiveByIdSpecification : BaseSpecification<AreaAfectada>
{
    public AreaAfectadaActiveByIdSpecification(int id)
        : base(a => a.Id == id && a.Borrado == false)
    {
        AddInclude(a => a.Provincia);
        AddInclude(a => a.Municipio);
        AddInclude(a => a.EntidadMenor);
    }
}
