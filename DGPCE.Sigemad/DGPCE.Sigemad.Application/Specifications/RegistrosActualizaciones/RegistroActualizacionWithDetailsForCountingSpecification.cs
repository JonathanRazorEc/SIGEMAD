using DGPCE.Sigemad.Domain.Enums;
using DGPCE.Sigemad.Domain.Modelos;

namespace DGPCE.Sigemad.Application.Specifications.RegistrosActualizaciones;
public class RegistroActualizacionWithDetailsForCountingSpecification : BaseSpecification<RegistroActualizacion>
{
    private static readonly int[] ApartadosAExcluir = new[] {
        (int)ApartadoRegistroEnum.DatoPrincipal
    };
    public RegistroActualizacionWithDetailsForCountingSpecification(RegistroActualizacionSpecificationParams @params)
        : base(r =>
        r.Borrado == false &&
        (!@params.Id.HasValue || r.Id == @params.Id) &&
        (!@params.IdMinimo.HasValue || r.Id > @params.IdMinimo.Value) &&
        (!@params.IdSuceso.HasValue || r.IdSuceso == @params.IdSuceso.Value) &&
        (!@params.IdTipoRegistroActualizacion.HasValue || r.IdTipoRegistroActualizacion == @params.IdTipoRegistroActualizacion.Value))
    {

        AddCriteria(r => !r.DetallesRegistro.Any() || r.DetallesRegistro.Any(d => !ApartadosAExcluir.Contains(d.IdApartadoRegistro)));
        AddOrderByDescending(r => r.FechaCreacion);
    }
}
