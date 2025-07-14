using DGPCE.Sigemad.Domain.Modelos.Ope.Datos;

namespace DGPCE.Sigemad.Application.Specifications.Ope.Datos.OpeDatosFronteras;
public class OpeDatoFronteraActiveByIdSpecification : BaseSpecification<OpeDatoFrontera>
{
    public OpeDatoFronteraActiveByIdSpecification(int id)
        : base(i => i.Id == id && i.Borrado == false)
    {
        AddInclude(i => i.OpeFrontera);
        AddInclude(i => i.OpeDatoFronteraIntervaloHorario);
    }
}
