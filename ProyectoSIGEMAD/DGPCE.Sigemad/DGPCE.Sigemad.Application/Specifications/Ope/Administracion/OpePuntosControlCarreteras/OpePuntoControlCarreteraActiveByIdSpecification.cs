using DGPCE.Sigemad.Domain.Modelos.Ope.Administracion;

namespace DGPCE.Sigemad.Application.Specifications.Ope.Administracion.OpePuntosControlCarreteras;
public class OpePuntoControlCarreteraActiveByIdSpecification : BaseSpecification<OpePuntoControlCarretera>
{
    public OpePuntoControlCarreteraActiveByIdSpecification(int id)
        : base(i => i.Id == id && i.Borrado == false)
    {

    }
}
