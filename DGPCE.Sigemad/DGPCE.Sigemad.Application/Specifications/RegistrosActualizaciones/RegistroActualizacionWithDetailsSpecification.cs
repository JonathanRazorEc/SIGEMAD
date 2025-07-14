using DGPCE.Sigemad.Domain.Enums;
using DGPCE.Sigemad.Domain.Modelos;

namespace DGPCE.Sigemad.Application.Specifications.RegistrosActualizaciones;
public class RegistroActualizacionWithDetailsSpecification : BaseSpecification<RegistroActualizacion>
{
    private static readonly int[] ApartadosAExcluir = new[] {
        (int)ApartadoRegistroEnum.DatoPrincipal
    };

    public RegistroActualizacionWithDetailsSpecification(RegistroActualizacionSpecificationParams @params)
        : base(r =>
        r.Borrado == false &&
        (!@params.Id.HasValue || r.Id == @params.Id) &&
        (!@params.IdMinimo.HasValue || r.Id > @params.IdMinimo.Value) &&
        (!@params.IdSuceso.HasValue || r.IdSuceso == @params.IdSuceso.Value) &&
        (!@params.IdTipoRegistroActualizacion.HasValue || r.IdTipoRegistroActualizacion == @params.IdTipoRegistroActualizacion.Value))
    {
        // Para filtrar el registro principal 
        //AddCriteria(r => r.DetallesRegistro.Any(d => !ApartadosAExcluir.Contains(d.IdApartadoRegistro)));

        AddCriteria(r => !r.DetallesRegistro.Any() || r.DetallesRegistro.Any(d => !ApartadosAExcluir.Contains(d.IdApartadoRegistro)));
        AddInclude(r => r.TipoRegistroActualizacion);
        
        //Filtra los detalles 
        AddInclude(r => r.DetallesRegistro.Where(d => !ApartadosAExcluir.Contains(d.IdApartadoRegistro) && (d.IdEstadoRegistro != EstadoRegistroEnum.Eliminado && d.IdEstadoRegistro != EstadoRegistroEnum.CreadoYEliminado)));
        AddInclude("DetallesRegistro.ApartadoRegistro");

        AddOrderByDescending(r => r.FechaCreacion);

        ApplyPaging(@params);
    }
}
