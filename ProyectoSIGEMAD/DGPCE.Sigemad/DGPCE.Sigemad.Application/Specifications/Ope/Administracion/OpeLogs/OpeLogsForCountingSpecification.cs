using DGPCE.Sigemad.Domain.Modelos.Ope.Administracion;

namespace DGPCE.Sigemad.Application.Specifications.Ope.Administracion.OpeLogs;

public class OpeLogsForCountingSpecification : BaseSpecification<OpeLog>
{
    public OpeLogsForCountingSpecification(OpeLogsSpecificationParams request)
        : base(opeLog =>
        (!request.Id.HasValue || opeLog.Id == request.Id)
        // && opeLog.Borrado != true
        )
    {

    }
}
