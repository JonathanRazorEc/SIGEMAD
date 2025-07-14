using DGPCE.Sigemad.Domain.Modelos.Ope.Administracion;
using Microsoft.EntityFrameworkCore;

namespace DGPCE.Sigemad.Application.Specifications.Ope.Administracion.OpeLineasMaritimas;

public class OpeLineasMaritimasSpecification : BaseSpecification<OpeLineaMaritima>
{
    public OpeLineasMaritimasSpecification(OpeLineasMaritimasSpecificationParams request)
        : base(opeLineaMaritima =>
        (string.IsNullOrEmpty(request.Nombre) || opeLineaMaritima.Nombre.Contains(request.Nombre)) &&
        (!request.Id.HasValue || opeLineaMaritima.Id == request.Id) &&

          //
          (!request.IdOpeFase.HasValue || opeLineaMaritima.IdOpeFase == request.IdOpeFase) &&
        //
        // Añadir filtro de puertos origen y destino si vienen definidos
        (!request.IdOpePuertoOrigen.HasValue || opeLineaMaritima.IdOpePuertoOrigen == request.IdOpePuertoOrigen) &&
        (!request.IdOpePuertoDestino.HasValue || opeLineaMaritima.IdOpePuertoDestino == request.IdOpePuertoDestino) &&
        //
        // Añadir filtro de país origen y destino si vienen definidos
        (!request.IdPaisOrigen.HasValue || opeLineaMaritima.OpePuertoOrigen.IdPais == request.IdPaisOrigen) &&
        (!request.IdPaisDestino.HasValue || opeLineaMaritima.OpePuertoDestino.IdPais == request.IdPaisDestino) &&
        //
         (
            // Si hay al menos una de las dos fechas, se aplica el filtro por solapamiento
            request.FechaValidezDesde.HasValue || request.FechaValidezHasta.HasValue
        ?
        (
                    DateOnly.FromDateTime(opeLineaMaritima.FechaValidezDesde) <= (request.FechaValidezHasta ?? DateOnly.MaxValue) &&
        (
                        !opeLineaMaritima.FechaValidezHasta.HasValue ||
                        DateOnly.FromDateTime(opeLineaMaritima.FechaValidezHasta.Value) >= (request.FechaValidezDesde ?? DateOnly.MinValue)
                    )
                )
                : true // Si no hay ninguna fecha, se incluyen todos

        //
        ) &&
        //

        opeLineaMaritima.Borrado != true
        )
    {

        AddInclude(i => i.OpePuertoOrigen);
        AddInclude(i => i.OpePuertoDestino);
        AddInclude(i => i.OpeFase);

        ApplyPaging(request);

        // Aplicar la ordenación
        if (!string.IsNullOrEmpty(request.Sort?.ToLower()))
        {
            switch (request.Sort)
            {
                case "fechainicioasc":
                    AddOrderBy(i => i.FechaValidezDesde);
                    break;
                case "fechaIniciodesc":
                    AddOrderByDescending(i => i.FechaValidezDesde);
                    break;
                case "denominacionasc":
                    AddOrderBy(i => i.Nombre);
                    break;
                case "denominaciondesc":
                    AddOrderByDescending(i => i.Nombre);
                    break;
                default:
                    AddOrderBy(i => i.Nombre); // Orden por defecto
                    break;
            }
        }
    }
}
