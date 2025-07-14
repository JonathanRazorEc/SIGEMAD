using DGPCE.Sigemad.Application.Constants;
using DGPCE.Sigemad.Domain.Modelos;

namespace DGPCE.Sigemad.Application.Specifications.Sucesos;
public class SucesosSpecification : BaseSpecification<Suceso>
{
    public SucesosSpecification(SucesosSpecificationParams @params, List<int> idsRelacionados = null)
    : base(suceso =>
    (suceso.Borrado != true) &&
    suceso.Incendios.Any(incendio =>
        (string.IsNullOrEmpty(@params.Denominacion) || incendio.Denominacion.ToLower().Contains(@params.Denominacion.ToLower())) &&
        (!@params.IdClaseSuceso.HasValue || incendio.IdClaseSuceso == @params.IdClaseSuceso) &&
        (!@params.IdTerritorio.HasValue || incendio.IdTerritorio == @params.IdTerritorio) &&
        (!@params.IdPais.HasValue || incendio.IdPais == @params.IdPais) &&
        (!@params.IdCcaa.HasValue || incendio.Provincia.IdCcaa == @params.IdCcaa) &&
        (!@params.IdProvincia.HasValue || incendio.IdProvincia == @params.IdProvincia) &&
        (!@params.IdMunicipio.HasValue || incendio.IdMunicipio == @params.IdMunicipio) &&
    (incendio.Borrado != true)
    )
    )
    {

        if (@params.IdMovimiento == MovimientoTipos.Registro && @params.IdComparativoFecha.HasValue)
        {
            switch (@params.IdComparativoFecha.Value)
            {
                case ComparacionTipos.IgualA:
                    AddCriteria(suceso => DateOnly.FromDateTime(suceso.FechaCreacion) == @params.FechaInicio);
                    break;
                case ComparacionTipos.MayorQue:
                    AddCriteria(suceso => DateOnly.FromDateTime(suceso.FechaCreacion) > @params.FechaInicio);
                    break;
                case ComparacionTipos.MenorQue:
                    AddCriteria(suceso => DateOnly.FromDateTime(suceso.FechaCreacion) < @params.FechaInicio);
                    break;
                case ComparacionTipos.Entre:
                    if (@params.FechaInicio.HasValue && @params.FechaFin.HasValue)
                    {
                        AddCriteria(suceso => DateOnly.FromDateTime(suceso.FechaCreacion) >= @params.FechaInicio && DateOnly.FromDateTime(suceso.FechaCreacion) <= @params.FechaFin);
                    }
                    else
                    {
                        throw new ArgumentException("Las fechas de inicio y fin deben ser proporcionadas para la comparación 'Entre'");
                    }
                    break;
                case ComparacionTipos.NoEntre:
                    if (@params.FechaInicio.HasValue && @params.FechaFin.HasValue)
                    {
                        AddCriteria(suceso => DateOnly.FromDateTime(suceso.FechaCreacion) < @params.FechaInicio || DateOnly.FromDateTime(suceso.FechaCreacion) > @params.FechaFin);
                    }
                    else
                    {
                        throw new ArgumentException("Las fechas de inicio y fin deben ser proporcionadas para la comparación 'No Entre'");
                    }
                    break;
                default:
                    throw new ArgumentException("Operador de comparar fechas no válido");
            }
        }
        else if (@params.IdMovimiento == MovimientoTipos.InicioSuceso && @params.IdComparativoFecha.HasValue)
        {
            switch (@params.IdComparativoFecha.Value)
            {
                case ComparacionTipos.IgualA:
                    AddCriteria(suceso => suceso.Incendios.Any(incendio => DateOnly.FromDateTime(incendio.FechaInicio.UtcDateTime) == @params.FechaInicio));
                    break;
                case ComparacionTipos.MayorQue:
                    AddCriteria(suceso => suceso.Incendios.Any(incendio => DateOnly.FromDateTime(incendio.FechaInicio.UtcDateTime) > @params.FechaInicio));
                    //AddCriteria(suceso => DateOnly.FromDateTime(suceso.FechaInicio) > @params.FechaInicio);
                    break;
                case ComparacionTipos.MenorQue:
                    AddCriteria(suceso => suceso.Incendios.Any(incendio => DateOnly.FromDateTime(incendio.FechaInicio.UtcDateTime) < @params.FechaInicio));
                    //AddCriteria(incendio => DateOnly.FromDateTime(incendio.FechaInicio.UtcDateTime) < @params.FechaInicio);
                    break;
                case ComparacionTipos.Entre:
                    if (@params.FechaInicio.HasValue && @params.FechaFin.HasValue)
                    {
                        AddCriteria(suceso => suceso.Incendios.Any(incendio =>
                            DateOnly.FromDateTime(incendio.FechaInicio.UtcDateTime) >= @params.FechaInicio &&
                            DateOnly.FromDateTime(incendio.FechaInicio.UtcDateTime) <= @params.FechaFin
                            ));
                        //AddCriteria(incendio => DateOnly.FromDateTime(incendio.FechaInicio.UtcDateTime) >= @params.FechaInicio && DateOnly.FromDateTime(incendio.FechaInicio.UtcDateTime) <= @params.FechaFin);
                    }
                    else
                    {
                        throw new ArgumentException("Las fechas de inicio y fin deben ser proporcionadas para la comparación 'Entre'");
                    }
                    break;
                case ComparacionTipos.NoEntre:
                    if (@params.FechaInicio.HasValue && @params.FechaFin.HasValue)
                    {
                        AddCriteria(suceso => suceso.Incendios.Any(incendio =>
                            DateOnly.FromDateTime(incendio.FechaInicio.UtcDateTime) < @params.FechaInicio ||
                            DateOnly.FromDateTime(incendio.FechaInicio.UtcDateTime) > @params.FechaFin
                            ));

                        //AddCriteria(incendio => DateOnly.FromDateTime(incendio.FechaInicio.UtcDateTime) < @params.FechaInicio || DateOnly.FromDateTime(incendio.FechaInicio.UtcDateTime) > @params.FechaFin);
                    }
                    else
                    {
                        throw new ArgumentException("Las fechas de inicio y fin deben ser proporcionadas para la comparación 'No Entre'");
                    }
                    break;
                default:
                    throw new ArgumentException("Operador de comparar fechas no válido");
            }
        }
        else if (@params.IdMovimiento == MovimientoTipos.Modificacion && @params.IdComparativoFecha.HasValue)
        {
            switch (@params.IdComparativoFecha.Value)
            {
                case ComparacionTipos.IgualA:
                    AddCriteria(suceso => suceso.Incendios.Any(incendio => DateOnly.FromDateTime(incendio.FechaModificacion.Value) == @params.FechaInicio));
                    //AddCriteria(suceso => DateOnly.FromDateTime(suceso.FechaModificacion.Value) == @params.FechaInicio);
                    break;
                case ComparacionTipos.MayorQue:
                    AddCriteria(suceso => suceso.Incendios.Any(incendio => DateOnly.FromDateTime(incendio.FechaModificacion.Value) > @params.FechaInicio));
                    //AddCriteria(suceso => DateOnly.FromDateTime(suceso.FechaModificacion.Value) > @params.FechaInicio);
                    break;
                case ComparacionTipos.MenorQue:
                    AddCriteria(suceso => suceso.Incendios.Any(incendio => DateOnly.FromDateTime(incendio.FechaModificacion.Value) < @params.FechaInicio));
                    //AddCriteria(suceso => DateOnly.FromDateTime(suceso.FechaModificacion.Value) < @params.FechaInicio);
                    break;
                case ComparacionTipos.Entre:
                    if (@params.FechaInicio.HasValue && @params.FechaFin.HasValue)
                    {
                        AddCriteria(suceso => suceso.Incendios.Any(incendio =>
                            DateOnly.FromDateTime(incendio.FechaModificacion.Value) >= @params.FechaInicio &&
                            DateOnly.FromDateTime(incendio.FechaModificacion.Value) <= @params.FechaFin
                            ));
                        //AddCriteria(suceso => DateOnly.FromDateTime(suceso.FechaModificacion.Value) >= @params.FechaInicio && DateOnly.FromDateTime(suceso.FechaModificacion.Value) <= @params.FechaFin);
                    }
                    else
                    {
                        throw new ArgumentException("Las fechas de inicio y fin deben ser proporcionadas para la comparación 'Entre'");
                    }
                    break;
                case ComparacionTipos.NoEntre:
                    if (@params.FechaInicio.HasValue && @params.FechaFin.HasValue)
                    {
                        AddCriteria(suceso => suceso.Incendios.Any(incendio =>
                            DateOnly.FromDateTime(incendio.FechaModificacion.Value) < @params.FechaInicio ||
                            DateOnly.FromDateTime(incendio.FechaModificacion.Value) > @params.FechaFin
                            ));
                        //AddCriteria(incendio => DateOnly.FromDateTime(incendio.FechaModificacion.Value) < @params.FechaInicio || DateOnly.FromDateTime(incendio.FechaModificacion.Value) > @params.FechaFin);
                    }
                    else
                    {
                        throw new ArgumentException("Las fechas de inicio y fin deben ser proporcionadas para la comparación 'No Entre'");
                    }
                    break;
                default:
                    throw new ArgumentException("Operador de comparar fechas no válido");
            }
        }

        if (@params.IdSuceso.HasValue)
        {
            AddCriteria(suceso => suceso.Id != @params.IdSuceso &&
            !idsRelacionados.Contains(suceso.Id));
        }

        ApplyPaging(@params);

        AddInclude(suceso => suceso.TipoSuceso);
        AddInclude(suceso => suceso.Incendios);

        AddInclude("Incendios.EstadoSuceso");
    }
}
