using DGPCE.Sigemad.Domain.Modelos;


namespace DGPCE.Sigemad.Application.Specifications.SuperficieFiltros;
public class SuperficieFiltrosSpecifications : BaseSpecification<SuperficieFiltro>
{
    public SuperficieFiltrosSpecifications(SuperficieFiltroParams request)
    : base(superficieFiltro =>
        (!request.IdTipoFiltro.HasValue || request.IdTipoFiltro > 0 && superficieFiltro.IdTipoFiltro == request.IdTipoFiltro) &&
      (!request.Id.HasValue || request.Id > 0 && superficieFiltro.Id == request.Id) &&
      (superficieFiltro.Borrado == false))
    {
        AddInclude(b => b.TipoFiltro);
    }
}
