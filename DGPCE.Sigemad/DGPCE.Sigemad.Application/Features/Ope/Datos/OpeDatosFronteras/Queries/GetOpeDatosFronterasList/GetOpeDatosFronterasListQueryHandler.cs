using AutoMapper;
using DGPCE.Sigemad.Application.Contracts.Persistence;
using DGPCE.Sigemad.Application.Features.Ope.Datos.OpeDatosEmbarquesDiarios.Queries.GetOpeDatosEmbarquesDiariosList;
using DGPCE.Sigemad.Application.Features.Ope.Datos.OpeDatosFronteras.Queries.GetOpeDatosFronterasList;
using DGPCE.Sigemad.Application.Features.Ope.Datos.OpeDatosFronteras.Queries.GetOpeDatosFronterasList;
using DGPCE.Sigemad.Application.Features.Ope.Datos.OpeDatosFronteras.Vms;
using DGPCE.Sigemad.Application.Features.Shared;
using DGPCE.Sigemad.Application.Specifications.Ope.Datos.OpeDatosFronteras;
using DGPCE.Sigemad.Domain.Modelos.Ope.Administracion;
using DGPCE.Sigemad.Domain.Modelos.Ope.Datos;
using MediatR;
using Microsoft.Extensions.Logging;

namespace DGPCE.Sigemad.Application.Features.Ope.Datos.OpeDatosFronteras.Queries.GetOpeDatosFronterasList
{
    public class GetOpeDatosFronterasListQueryHandler : IRequestHandler<GetOpeDatosFronterasListQuery, PaginationVm<OpeDatoFronteraVm>>
    {
        private readonly ILogger<GetOpeDatosFronterasListQueryHandler> _logger;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public GetOpeDatosFronterasListQueryHandler(
        ILogger<GetOpeDatosFronterasListQueryHandler> logger,
        IUnitOfWork unitOfWork,
        IMapper mapper
        )
        {
            _logger = logger;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<PaginationVm<OpeDatoFronteraVm>> Handle(GetOpeDatosFronterasListQuery request, CancellationToken cancellationToken)
        {
            _logger.LogInformation($"{nameof(GetOpeDatosFronterasListQueryHandler)} - BEGIN");

            var spec = new OpeDatosFronterasSpecification(request);
            var opeDatosFronteras = await _unitOfWork.Repository<OpeDatoFrontera>()
            .GetAllWithSpec(spec);

            // Filtro numérico
            var opeDatosFronterasList = opeDatosFronteras.ToList();
            opeDatosFronteras = AplicarFiltroNumerico(request, opeDatosFronterasList);
            var totalOpeDatosFronteras = opeDatosFronteras.Count();
            // Fin Filtro numérico

            //var specCount = new OpeDatosFronterasForCountingSpecification(request);
            //var totalOpeDatosFronteras = await _unitOfWork.Repository<OpeDatoFrontera>().CountAsync(specCount);
            var opeDatoFronteraVmList = _mapper.Map<List<OpeDatoFronteraVm>>(opeDatosFronteras);

            var rounded = Math.Ceiling(Convert.ToDecimal(totalOpeDatosFronteras) / Convert.ToDecimal(request.PageSize));
            var totalPages = Convert.ToInt32(rounded);

            var pagination = new PaginationVm<OpeDatoFronteraVm>
            {
                Count = totalOpeDatosFronteras,
                Data = opeDatoFronteraVmList,
                PageCount = totalPages,
                PageIndex = request.PageIndex,
                PageSize = request.PageSize
            };

            _logger.LogInformation($"{nameof(GetOpeDatosFronterasListQueryHandler)} - END");
            return pagination;
        }

        // AGRUPACIÓN DE DATOS DE FRONTERA
        public class GrupoOpeDatoFrontera
        {
            public string Clave { get; set; }
            public List<OpeDatoFrontera> Valores { get; set; }
            public int TotalVehiculos { get; set; }
            public DateTime Fecha { get; set; }
            public TimeSpan HoraInicio { get; set; }
            public TimeSpan HoraFin { get; set; }
        }

        public List<GrupoOpeDatoFrontera> AgruparOpeDatosFrontera(List<OpeDatoFrontera> datos)
        {
            var grupos = datos
                .GroupBy(f =>
                {
                    var fecha = f.Fecha.Date;

                    var inicio = f.IntervaloHorarioPersonalizado
                        ? f.InicioIntervaloHorarioPersonalizado ?? TimeSpan.Zero
                        : f.OpeDatoFronteraIntervaloHorario?.Inicio ?? TimeSpan.Zero;

                    var fin = f.IntervaloHorarioPersonalizado
                        ? f.FinIntervaloHorarioPersonalizado ?? TimeSpan.Zero
                        : f.OpeDatoFronteraIntervaloHorario?.Fin ?? TimeSpan.Zero;

                    var clave = $"{fecha:yyyy-MM-dd} - {inicio:hh\\:mm} a {fin:hh\\:mm}h";
                    return (clave, fecha, inicio, fin);
                })
                .Select(g => new GrupoOpeDatoFrontera
                {
                    Clave = g.Key.clave,
                    Fecha = g.Key.fecha,
                    HoraInicio = g.Key.inicio,
                    HoraFin = g.Key.fin,
                    Valores = g.ToList(),
                    TotalVehiculos = g.Sum(x => x.NumeroVehiculos)
                })
                .ToList();

            return grupos;
        }
        // FIN AGRUPACIÓN DE DATOS DE FRONTERA

        private List<OpeDatoFrontera> AplicarFiltroNumerico(GetOpeDatosFronterasListQuery request, List<OpeDatoFrontera> opeDatosFronteras)
        {
            if (string.IsNullOrEmpty(request.CriterioNumerico))
                return opeDatosFronteras;

            var opeDatosFronterasAgrupados = AgruparOpeDatosFrontera(opeDatosFronteras);

            //
            string criterioNumerico = request.CriterioNumerico.ToLower();
            string[] partesCriterioNumerico = criterioNumerico.Split('_');

            int? idOpeFrontera = null;

            if (partesCriterioNumerico.Length == 2 && partesCriterioNumerico[0] == "total")
            {
                if (int.TryParse(partesCriterioNumerico[1], out int id))
                {
                    idOpeFrontera = id;
                }
            }
            //

            switch (partesCriterioNumerico[0])
            {
                case "total":
                    opeDatosFronteras = FiltrarPorTotal(request, opeDatosFronterasAgrupados, idOpeFrontera);
                    break;
                /*
                case "rotaciones":
                    //opeDatosFronteras = FiltrarPorRotaciones(request, opeDatosFronteras);
                    break;

                case "pasajeros":
                    //opeDatosFronteras = FiltrarPorPasajeros(request, opeDatosFronteras);
                    break;

                case "turismos":
                    //opeDatosFronteras = FiltrarPorTurismos(request, opeDatosFronteras);
                    break;
                */
            }

            return opeDatosFronteras;
        }

        /*
        private List<OpeDatoFrontera> FiltrarPorTotal(GetOpeDatosFronterasListQuery request, List<GrupoOpeDatoFrontera> opeDatosFronterasAgrupados, int? idOpeFrontera)
        {
            switch (request.CriterioNumericoRadio?.ToLower())
            {
                case "maximo":
                    return opeDatosFronterasAgrupados.OrderByDescending(g => g.TotalVehiculos)
                                 .FirstOrDefault()?.Valores ?? new List<OpeDatoFrontera>();

                case "minimo":
                    return opeDatosFronterasAgrupados.OrderBy(g => g.TotalVehiculos)
                                 .FirstOrDefault()?.Valores ?? new List<OpeDatoFrontera>();

                case "cantidad":
                    return FiltrarPorCantidad(request, opeDatosFronterasAgrupados, g => g.TotalVehiculos).SelectMany(g => g.Valores).ToList();


            }

            // Si no hay criterio claro, devuelve todos los datos planos
            return opeDatosFronterasAgrupados.SelectMany(g => g.Valores).ToList();
        }
        */

        //
        private List<OpeDatoFrontera> FiltrarPorTotal(GetOpeDatosFronterasListQuery request, List<GrupoOpeDatoFrontera> opeDatosFronterasAgrupados, int? idOpeFrontera)
        {
            // Creamos una lista auxiliar para evaluar el TotalVehiculos solo con la frontera filtrada
            var evaluables = opeDatosFronterasAgrupados
                .Select(g =>
                {
                    var valoresFiltrados = idOpeFrontera.HasValue
                        ? g.Valores.Where(v => v.IdOpeFrontera == idOpeFrontera.Value).ToList()
                        : g.Valores;

                    return new
                    {
                        GrupoOriginal = g,
                        TotalFiltrado = valoresFiltrados.Sum(v => v.NumeroVehiculos),
                        TieneDatos = valoresFiltrados.Any()
                    };
                })
                .Where(x => x.TieneDatos) // Excluimos grupos donde no haya datos de esa frontera
                .ToList();

            switch (request.CriterioNumericoRadio?.ToLower())
            {
                case "maximo":
                    return evaluables.OrderByDescending(x => x.TotalFiltrado)
                                     .FirstOrDefault()?.GrupoOriginal.Valores ?? new List<OpeDatoFrontera>();

                case "minimo":
                    return evaluables.OrderBy(x => x.TotalFiltrado)
                                     .FirstOrDefault()?.GrupoOriginal.Valores ?? new List<OpeDatoFrontera>();

                case "cantidad":
                    var gruposFiltrados = FiltrarPorCantidad(
                        request,
                        evaluables.Select(x =>
                        {
                            // Creamos una copia con TotalVehiculos filtrado, pero devolvemos los valores originales
                            return new GrupoOpeDatoFrontera
                            {
                                Valores = x.GrupoOriginal.Valores,
                                TotalVehiculos = x.TotalFiltrado
                            };
                        }).ToList(),
                        g => g.TotalVehiculos);

                    return gruposFiltrados.SelectMany(g => g.Valores).ToList();
            }

            // Si no hay criterio claro, devolvemos todos
            return opeDatosFronterasAgrupados.SelectMany(g => g.Valores).ToList();
        }


        //


        private List<GrupoOpeDatoFrontera> FiltrarPorCantidad(GetOpeDatosFronterasListQuery request, List<GrupoOpeDatoFrontera> opeDatosFronterasAgrupados, Func<GrupoOpeDatoFrontera, int> selector)
        {
            if (!request.CriterioNumericoCriterioCantidadCantidad.HasValue)
            {
                return opeDatosFronterasAgrupados;
            }

            int cantidadFiltro = request.CriterioNumericoCriterioCantidadCantidad.Value;

            switch (request.CriterioNumericoCriterioCantidad)
            {
                case "igual":
                    return opeDatosFronterasAgrupados.Where(x => selector(x) == cantidadFiltro).ToList();

                case "mayor":
                    return opeDatosFronterasAgrupados.Where(x => selector(x) > cantidadFiltro).ToList();

                case "menor":
                    return opeDatosFronterasAgrupados.Where(x => selector(x) < cantidadFiltro).ToList();

                default:
                    return opeDatosFronterasAgrupados;
            }
        }

    }
}