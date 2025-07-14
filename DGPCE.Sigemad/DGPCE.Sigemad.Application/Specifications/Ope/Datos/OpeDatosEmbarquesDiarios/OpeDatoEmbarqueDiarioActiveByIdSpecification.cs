using DGPCE.Sigemad.Domain.Modelos.Ope.Datos;

namespace DGPCE.Sigemad.Application.Specifications.Ope.Datos.OpeDatosEmbarquesDiarios;
public class OpeDatoEmbarqueDiarioActiveByIdSpecification : BaseSpecification<OpeDatoEmbarqueDiario>
{
    public OpeDatoEmbarqueDiarioActiveByIdSpecification(int id)
        : base(i => i.Id == id && i.Borrado == false)
    {

    }
}
