using DGPCE.Sigemad.Domain.Modelos;


namespace DGPCE.Sigemad.Application.Specifications.Registros;
public class ComprobacionRegistrosPosterioresSpecifications : BaseSpecification<Registro>
{
    public ComprobacionRegistrosPosterioresSpecifications(
   ComprobacionRegistrosPosterioresParams request )
    : base(e => e.IdSuceso == request.IdSuceso && e.Borrado == false
                && e.FechaCreacion > request.FechaCreacion && e.Id != request.Id)
    {
         AddCriteria(e => e.AreaAfectadas.Any());
    }
}
