using DGPCE.Sigemad.Domain.Modelos.Ope.Datos;

namespace DGPCE.Sigemad.Application.Specifications.Ope.Datos.OpeDatosFronterasIntervalosHorarios;

public class OpeDatosFronterasIntervalosHorariosSpecification : BaseSpecification<OpeDatoFronteraIntervaloHorario>
{
    public OpeDatosFronterasIntervalosHorariosSpecification()
       : base(opeDatoFronteraIntervaloHorario => opeDatoFronteraIntervaloHorario.Borrado != true)
    {

    }

    public OpeDatosFronterasIntervalosHorariosSpecification(OpeDatosFronterasIntervalosHorariosSpecificationParams request)
    : base(opeDatoFronteraIntervaloHorario =>
    (!request.Id.HasValue || opeDatoFronteraIntervaloHorario.Id == request.Id) &&
       opeDatoFronteraIntervaloHorario.Borrado != true)
    {

    }
}
