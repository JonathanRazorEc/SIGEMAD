using DGPCE.Sigemad.Domain.Modelos.Ope.Administracion;

namespace DGPCE.Sigemad.Application.Specifications.Ope.Administracion.OpeLineasMaritimas;

public class OpeLineasMaritimasPuertoSpecification : BaseSpecification<OpeLineaMaritima>
{
    public OpeLineasMaritimasPuertoSpecification(int opePuertoId)
        : base(linea =>
            linea.Borrado != true &&
            (linea.IdOpePuertoOrigen == opePuertoId || linea.IdOpePuertoDestino == opePuertoId))
    {
    }
}