using DGPCE.Sigemad.Domain.Modelos.Ope.Datos;

namespace DGPCE.Sigemad.Application.Specifications.Ope.Datos.OpeDatosEmbarquesDiarios;

public class OpeDatosEmbarquesDiariosForCountingSpecification : BaseSpecification<OpeDatoEmbarqueDiario>
{
    public OpeDatosEmbarquesDiariosForCountingSpecification(OpeDatosEmbarquesDiariosSpecificationParams request)
        : base(opeDatoEmbarqueDiario =>
        (!request.Id.HasValue || opeDatoEmbarqueDiario.Id == request.Id) &&
        opeDatoEmbarqueDiario.Borrado != true
        )
    {

    }
}
