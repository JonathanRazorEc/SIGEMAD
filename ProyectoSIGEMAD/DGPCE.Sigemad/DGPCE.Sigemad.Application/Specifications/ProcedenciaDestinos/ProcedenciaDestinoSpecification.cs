
using DGPCE.Sigemad.Domain.Modelos;


namespace DGPCE.Sigemad.Application.Specifications.ProcedenciaDestinos;
public class ProcedenciaDestinoSpecification : BaseSpecification<ProcedenciaDestino>
{
    public ProcedenciaDestinoSpecification()
    {
        AddOrderBy(pd => pd.Descripcion ?? "");
    }
}