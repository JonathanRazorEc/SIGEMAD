using DGPCE.Sigemad.Domain.Modelos;


namespace DGPCE.Sigemad.Application.Specifications.TipoRiesgos;
internal class TipoRiesgoByIdTipoSucesoSpecification : BaseSpecification<TipoRiesgo>
{
    public TipoRiesgoByIdTipoSucesoSpecification(int idTipoSuceso)
    : base(r => r.IdTipoSuceso == idTipoSuceso )
    {
    }
}
