using DGPCE.Sigemad.Domain.Modelos.Ope.Datos;

namespace DGPCE.Sigemad.Application.Specifications.Ope.Administracion.OpeLineasMaritimas;

public class OpeDatosEmbarquesDiariosLineaMaritimaSpecification : BaseSpecification<OpeDatoEmbarqueDiario>
{
    public OpeDatosEmbarquesDiariosLineaMaritimaSpecification(int opeLineaMaritimaId)
        : base(datoEmbarqueDiario =>
            datoEmbarqueDiario.Borrado != true &&
            datoEmbarqueDiario.IdOpeLineaMaritima == opeLineaMaritimaId)
    {
    }
}