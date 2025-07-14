
using DGPCE.Sigemad.Application.Constants;
using DGPCE.Sigemad.Domain.Enums;
using DGPCE.Sigemad.Domain.Modelos;


namespace DGPCE.Sigemad.Application.Specifications.Incendios;

public class IncendiosSpecification : BaseSpecification<Incendio>
{
    public IncendiosSpecification(IncendiosSpecificationParams request)
        : base(incendio =>
        (string.IsNullOrEmpty(request.Search) || incendio.Denominacion.Contains(request.Search)) &&
        (!request.Id.HasValue || incendio.Id == request.Id) &&
        (!request.IdTerritorio.HasValue || incendio.IdTerritorio == request.IdTerritorio) &&
        (!request.IdPais.HasValue || incendio.IdPais == request.IdPais) &&
        (!request.IdCcaa.HasValue || incendio.Provincia.IdCcaa == request.IdCcaa) &&
        (!request.IdProvincia.HasValue || incendio.IdProvincia == request.IdProvincia) &&
        (!request.IdMunicipio.HasValue || incendio.IdMunicipio == request.IdMunicipio) &&
        (!request.IdClaseSuceso.HasValue || incendio.IdClaseSuceso == request.IdClaseSuceso) &&
        (!request.IdEstadoSuceso.HasValue || incendio.IdEstadoSuceso == request.IdEstadoSuceso) &&
        (!request.IdSuceso.HasValue || incendio.IdSuceso == request.IdSuceso) &&
        (incendio.Borrado != true)
        )
    {

        if (request.IdTipoFiltro.HasValue && request.valorFiltro.HasValue)
        {
            AddCriteria(i =>
                (
                    // Caso especial: Filtro Igual y valor 0
                    request.IdTipoFiltro.Value == FiltrosComparacion.Igual && request.valorFiltro.Value == 0
                    ? (
                        // Sucesos sin registros
                        !i.Suceso.Registros.Any() ||


                        i.Suceso.Registros
                            .OrderByDescending(r => r.AreaAfectadas.Count > 0 && !r.Borrado)
                            .Take(1)
                            .Any(r => r.AreaAfectadas.Where(a => !a.Borrado).Sum(aa => aa.SuperficieAfectadaHectarea) == request.valorFiltro.Value)
                    )
                    : (
                        // Otros casos de comparación
                        i.Suceso.Registros
                            .Where(r => r.AreaAfectadas.Count > 0 && !r.Borrado)
                            .OrderByDescending(r => r.FechaCreacion)
                            .Take(1)
                            .Any(r =>
                                request.IdTipoFiltro == FiltrosComparacion.MayorQue
                                    ? r.AreaAfectadas.Where(a => !a.Borrado).Sum(aa => aa.SuperficieAfectadaHectarea) > request.valorFiltro.Value
                                    : request.IdTipoFiltro == FiltrosComparacion.MenorQue
                                    ? r.AreaAfectadas.Where(a => !a.Borrado).Sum(aa => aa.SuperficieAfectadaHectarea) < request.valorFiltro.Value
                                    : request.IdTipoFiltro == FiltrosComparacion.MayorIgualQue
                                    ? r.AreaAfectadas.Where(a => !a.Borrado).Sum(aa => aa.SuperficieAfectadaHectarea) >= request.valorFiltro.Value
                                    : request.IdTipoFiltro == FiltrosComparacion.MenorIgualQue
                                    ? r.AreaAfectadas.Where(a => !a.Borrado).Sum(aa => aa.SuperficieAfectadaHectarea) <= request.valorFiltro.Value
                                    : request.IdTipoFiltro == FiltrosComparacion.Igual
                                    ? r.AreaAfectadas.Where(a => !a.Borrado).Sum(aa => aa.SuperficieAfectadaHectarea) == request.valorFiltro.Value
                                    : false
                            )
                    )
                )
            );

        }


        if (request.IdEstadoIncendio.HasValue)
        {
            AddInclude(i => i.Suceso.Registros);

            AddCriteria(i => i.Suceso.Registros
             .Where(e => e.Borrado == false)
             .SelectMany(e => e.Parametros)
             .OrderByDescending(p => p.FechaCreacion)
             .Take(1) // Solo tomar 1 registro
                .Any(parametro => parametro.IdEstadoIncendio == request.IdEstadoIncendio.Value));

        }

        if (request.IdSituacionEquivalente.HasValue)
        {
            AddInclude(i => i.Suceso.Registros);

            AddCriteria(i => i.Suceso.Registros
             .Where(e => e.Borrado == false)
             .SelectMany(e => e.Parametros)
             .OrderByDescending(p => p.FechaCreacion)
             .Take(1) // Solo tomar 1 registro
                .Any(parametro => parametro.IdSituacionEquivalente == request.IdSituacionEquivalente.Value));

        }

        // ──────────────────────────────────────────────────────────────
        //  Filtro específico para la búsqueda Transfronteriza
        // ──────────────────────────────────────────────────────────────
        if (request.IdTerritorio == (int)TipoTerritorio.Transfronterizo)
        {
            AddCriteria(i =>
                i.Suceso.Registros
                 .Where(ev => ev.Borrado == false)
                 .SelectMany(ev => ev.IntervencionMedios)
                 .Any(m =>
                      m.Borrado == false &&
                      m.IdCaracterMedio == 4));
        }



        AddInclude("Suceso.Registros.Parametros.SituacionEquivalente");
        AddInclude("Suceso.Registros.Parametros.EstadoIncendio");

        AddInclude(i => i.Suceso.RegistroActualizaciones);
        AddOrderByDescending(i => i.Suceso.RegistroActualizaciones.OrderByDescending(ra => ra.FechaCreacion).FirstOrDefault().FechaCreacion);

        if (request.busquedaSucesos != null && (bool)request.busquedaSucesos)
        {
            AddInclude(i => i.Suceso.TipoSuceso);
        }
        else
        {
            AddInclude(i => i.Territorio);
            AddInclude(i => i.Municipio);
            AddInclude(i => i.Provincia);
            AddInclude(i => i.ClaseSuceso);
            AddInclude(i => i.Suceso);
            AddInclude(i => i.Pais);
            AddInclude(i => i.MunicipioExtranjero);
            AddInclude(i => i.Distrito);
        }

        AddInclude(i => i.EstadoSuceso);

        if (request.IdMovimiento == MovimientoTipos.Registro && request.IdComparativoFecha.HasValue)
        {
            switch (request.IdComparativoFecha.Value)
            {
                case ComparacionTipos.IgualA:

                    //AddCriteria(i => i.Suceso.Evoluciones
                    //.Where(e => e.EsFoto == false && e.Borrado == false && e.Registro.FechaHoraEvolucion != null)
                    //.Select(e => e.Registro.FechaHoraEvolucion)
                    //.OrderByDescending(fecha => fecha) // Ordena para tomar la más reciente
                    //.FirstOrDefault() != null &&
                    //DateOnly.FromDateTime(i.Suceso.Evoluciones
                    //    .Where(e => e.EsFoto == false && e.Borrado == false && e.Registro.FechaHoraEvolucion != null)
                    //    .Select(e => e.Registro.FechaHoraEvolucion)
                    //    .OrderByDescending(fecha => fecha)
                    //    .FirstOrDefault().Value) == request.FechaInicio);

                    AddCriteria(i => i.Suceso.Registros
                        .Where(e =>  e.Borrado == false) // Filtrar evoluciones válidas
                        .Where(r => r.FechaHoraEvolucion != null && r.Borrado == false) // Excluir registros borrados lógicamente
                        .OrderByDescending(r => r.FechaCreacion) // Ordenar por fecha más reciente
                        .Take(1) // Solo tomar el más reciente
                        .Any(r => DateOnly.FromDateTime(r.FechaHoraEvolucion.Value) == request.FechaInicio));

                    break;

                case ComparacionTipos.MayorQue:

                    //AddCriteria(i => i.Suceso.Evoluciones
                    //.Where(e => e.EsFoto == false && e.Borrado == false && e.Registro.FechaHoraEvolucion != null)
                    //.Select(e => e.Registro.FechaHoraEvolucion)
                    //.OrderByDescending(fecha => fecha) // Ordena para tomar la más reciente
                    //.FirstOrDefault() != null &&
                    //DateOnly.FromDateTime(i.Suceso.Evoluciones
                    //    .Where(e => e.EsFoto == false && e.Borrado == false && e.Registro.FechaHoraEvolucion != null)
                    //    .Select(e => e.Registro.FechaHoraEvolucion)
                    //    .OrderByDescending(fecha => fecha)
                    //    .FirstOrDefault().Value) > request.FechaInicio);

                    AddCriteria(i => i.Suceso.Registros
                        .Where(e => e.Borrado == false) // Filtrar evoluciones válidas
                        .Where(r => r.FechaHoraEvolucion != null && r.Borrado == false) // Excluir registros borrados lógicamente
                        .OrderByDescending(r => r.FechaCreacion) // Ordenar por fecha más reciente
                        .Take(1) // Solo tomar el más reciente
                        .Any(r => DateOnly.FromDateTime(r.FechaHoraEvolucion.Value) > request.FechaInicio));

                    break;

                case ComparacionTipos.MenorQue:
                    //AddCriteria(i => i.Suceso.Evoluciones
                    //.Where(e => e.EsFoto == false && e.Borrado == false && e.Registro.FechaHoraEvolucion != null)
                    //.Select(e => e.Registro.FechaHoraEvolucion)
                    //.OrderByDescending(fecha => fecha) // Ordena para tomar la más reciente
                    //.FirstOrDefault() != null &&
                    //DateOnly.FromDateTime(i.Suceso.Evoluciones
                    //    .Where(e => e.EsFoto == false && e.Borrado == false && e.Registro.FechaHoraEvolucion != null)
                    //    .Select(e => e.Registro.FechaHoraEvolucion)
                    //    .OrderByDescending(fecha => fecha)
                    //    .FirstOrDefault().Value) < request.FechaInicio);

                    //AddCriteria(incendio => DateOnly.FromDateTime(incendio.FechaCreacion) < request.FechaInicio);

                    AddCriteria(i => i.Suceso.Registros
                        .Where(e => e.Borrado == false) // Filtrar evoluciones válidas
                        .Where(r => r.FechaHoraEvolucion != null && r.Borrado == false) // Excluir registros borrados lógicamente
                        .OrderByDescending(r => r.FechaCreacion) // Ordenar por fecha más reciente
                        .Take(1) // Solo tomar el más reciente
                        .Any(r => DateOnly.FromDateTime(r.FechaHoraEvolucion.Value) < request.FechaInicio));

                    break;
                case ComparacionTipos.Entre:
                    if (request.FechaInicio.HasValue && request.FechaFin.HasValue)
                    {
                        //AddCriteria(i => i.Suceso.Evoluciones
                        //.Where(e => e.EsFoto == false && e.Borrado == false && e.Registro.FechaHoraEvolucion != null)
                        //.Select(e => e.Registro.FechaHoraEvolucion)
                        //.OrderByDescending(fecha => fecha) // Ordena para tomar la más reciente
                        //.FirstOrDefault() != null &&

                        //    (
                        //    DateOnly.FromDateTime(i.Suceso.Evoluciones
                        //    .Where(e => e.EsFoto == false && e.Borrado == false && e.Registro.FechaHoraEvolucion != null)
                        //    .Select(e => e.Registro.FechaHoraEvolucion)
                        //    .OrderByDescending(fecha => fecha)
                        //    .FirstOrDefault().Value) >= request.FechaInicio &&

                        //    DateOnly.FromDateTime(i.Suceso.Evoluciones
                        //    .Where(e => e.EsFoto == false && e.Borrado == false && e.Registro.FechaHoraEvolucion != null)
                        //    .Select(e => e.Registro.FechaHoraEvolucion)
                        //    .OrderByDescending(fecha => fecha)
                        //    .FirstOrDefault().Value) <= request.FechaInicio
                        //    )
                        //);

                        //AddCriteria(incendio => DateOnly.FromDateTime(incendio.FechaCreacion) >= request.FechaInicio && DateOnly.FromDateTime(incendio.FechaCreacion) <= request.FechaFin);

                        AddCriteria(i => i.Suceso.Registros
                        .Where(e => e.Borrado == false) // Filtrar evoluciones válidas
                        .Where(r => r.FechaHoraEvolucion != null && r.Borrado == false) // Excluir registros borrados lógicamente
                        .OrderByDescending(r => r.FechaCreacion) // Ordenar por fecha más reciente
                        .Take(1) // Solo tomar el más reciente
                        .Any(r => 
                            DateOnly.FromDateTime(r.FechaHoraEvolucion.Value) >= request.FechaInicio &&
                            DateOnly.FromDateTime(r.FechaHoraEvolucion.Value) <= request.FechaFin
                            )
                        );
                    }
                    else
                    {
                        throw new ArgumentException("Las fechas de inicio y fin deben ser proporcionadas para la comparación 'Entre'");
                    }
                    break;
                case ComparacionTipos.NoEntre:
                    if (request.FechaInicio.HasValue && request.FechaFin.HasValue)
                    {
                        //AddCriteria(i => i.Suceso.Evoluciones
                        //.Where(e => e.EsFoto == false && e.Borrado == false && e.Registro.FechaHoraEvolucion != null)
                        //.Select(e => e.Registro.FechaHoraEvolucion)
                        //.OrderByDescending(fecha => fecha) // Ordena para tomar la más reciente
                        //.FirstOrDefault() != null &&

                        //    (
                        //    DateOnly.FromDateTime(i.Suceso.Evoluciones
                        //    .Where(e => e.EsFoto == false && e.Borrado == false && e.Registro.FechaHoraEvolucion != null)
                        //    .Select(e => e.Registro.FechaHoraEvolucion)
                        //    .OrderByDescending(fecha => fecha)
                        //    .FirstOrDefault().Value) < request.FechaInicio ||

                        //    DateOnly.FromDateTime(i.Suceso.Evoluciones
                        //    .Where(e => e.EsFoto == false && e.Borrado == false && e.Registro.FechaHoraEvolucion != null)
                        //    .Select(e => e.Registro.FechaHoraEvolucion)
                        //    .OrderByDescending(fecha => fecha)
                        //    .FirstOrDefault().Value) > request.FechaInicio
                        //    )
                        //);

                        //AddCriteria(incendio => DateOnly.FromDateTime(incendio.FechaCreacion) < request.FechaInicio || DateOnly.FromDateTime(incendio.FechaCreacion) > request.FechaFin);

                        AddCriteria(i => i.Suceso.Registros
                        .Where(e =>e.Borrado == false) // Filtrar evoluciones válidas
                        .Where(r => r.FechaHoraEvolucion != null && r.Borrado == false) // Excluir registros borrados lógicamente
                        .OrderByDescending(r => r.FechaCreacion) // Ordenar por fecha más reciente
                        .Take(1) // Solo tomar el más reciente
                        .Any(r =>
                            DateOnly.FromDateTime(r.FechaHoraEvolucion.Value) < request.FechaInicio &&
                            DateOnly.FromDateTime(r.FechaHoraEvolucion.Value) > request.FechaFin
                            )
                        );
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
        else if (request.IdMovimiento == MovimientoTipos.InicioSuceso && request.IdComparativoFecha.HasValue)
        {
            switch (request.IdComparativoFecha.Value)
            {
                case ComparacionTipos.IgualA:
                    AddCriteria(incendio => DateOnly.FromDateTime(incendio.FechaInicio.UtcDateTime) == request.FechaInicio);
                    break;
                case ComparacionTipos.MayorQue:
                    AddCriteria(incendio => DateOnly.FromDateTime(incendio.FechaInicio.UtcDateTime) > request.FechaInicio);
                    break;
                case ComparacionTipos.MenorQue:
                    AddCriteria(incendio => DateOnly.FromDateTime(incendio.FechaInicio.UtcDateTime) < request.FechaInicio);
                    break;
                case ComparacionTipos.Entre:
                    if (request.FechaInicio.HasValue && request.FechaFin.HasValue)
                    {
                        AddCriteria(incendio => DateOnly.FromDateTime(incendio.FechaInicio.UtcDateTime) >= request.FechaInicio && DateOnly.FromDateTime(incendio.FechaInicio.UtcDateTime) <= request.FechaFin);
                    }
                    else
                    {
                        throw new ArgumentException("Las fechas de inicio y fin deben ser proporcionadas para la comparación 'Entre'");
                    }
                    break;
                case ComparacionTipos.NoEntre:
                    if (request.FechaInicio.HasValue && request.FechaFin.HasValue)
                    {
                        AddCriteria(incendio => DateOnly.FromDateTime(incendio.FechaInicio.UtcDateTime) < request.FechaInicio || DateOnly.FromDateTime(incendio.FechaInicio.UtcDateTime) > request.FechaFin);
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
        else if (request.IdMovimiento == MovimientoTipos.Modificacion && request.IdComparativoFecha.HasValue)
        {
            switch (request.IdComparativoFecha.Value)
            {
                case ComparacionTipos.IgualA:
                    AddCriteria(incendio => DateOnly.FromDateTime(incendio.FechaModificacion.Value) == request.FechaInicio);
                    break;
                case ComparacionTipos.MayorQue:
                    AddCriteria(incendio => DateOnly.FromDateTime(incendio.FechaModificacion.Value) > request.FechaInicio);
                    break;
                case ComparacionTipos.MenorQue:
                    AddCriteria(incendio => DateOnly.FromDateTime(incendio.FechaModificacion.Value) < request.FechaInicio);
                    break;
                case ComparacionTipos.Entre:
                    if (request.FechaInicio.HasValue && request.FechaFin.HasValue)
                    {
                        AddCriteria(incendio => DateOnly.FromDateTime(incendio.FechaModificacion.Value) >= request.FechaInicio && DateOnly.FromDateTime(incendio.FechaModificacion.Value) <= request.FechaFin);
                    }
                    else
                    {
                        throw new ArgumentException("Las fechas de inicio y fin deben ser proporcionadas para la comparación 'Entre'");
                    }
                    break;
                case ComparacionTipos.NoEntre:
                    if (request.FechaInicio.HasValue && request.FechaFin.HasValue)
                    {
                        AddCriteria(incendio => DateOnly.FromDateTime(incendio.FechaModificacion.Value) < request.FechaInicio || DateOnly.FromDateTime(incendio.FechaModificacion.Value) > request.FechaFin);
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




        ApplyPaging(request);

        // Aplicar la ordenación
        if (!string.IsNullOrEmpty(request.Sort?.ToLower()))
        {
            switch (request.Sort)
            {
                case "fechainicioasc":
                    AddOrderBy(i => i.FechaInicio);
                    break;
                case "fechaIniciodesc":
                    AddOrderByDescending(i => i.FechaInicio);
                    break;
                case "denominacionasc":
                    AddOrderBy(i => i.Denominacion);
                    break;
                case "denominaciondesc":
                    AddOrderByDescending(i => i.Denominacion);
                    break;
                case "estadosucesoasc":
                    AddOrderBy(i => i.IdEstadoSuceso);
                    break;
                case "estadosucesodesc":
                    AddOrderByDescending(i => i.IdEstadoSuceso);
                    break;
                default:
                    AddOrderBy(i => i.FechaInicio); // Orden por defecto
                    break;
            }
        }
    }
}
