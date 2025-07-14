using AutoMapper;
using DGPCE.Sigemad.Application.Contracts.Persistence;
using DGPCE.Sigemad.Application.Features.Ope.Datos.OpeDatosAsistencias.Vms;
using DGPCE.Sigemad.Application.Features.Ope.Datos.OpeDatosAsistencias.Queries.GetOpeDatosAsistenciasList;
using DGPCE.Sigemad.Application.Features.Shared;
using DGPCE.Sigemad.Application.Specifications.Ope.Datos.OpeDatosAsistencias;
using DGPCE.Sigemad.Domain.Modelos.Ope.Datos;
using MediatR;
using Microsoft.Extensions.Logging;

namespace DGPCE.Sigemad.Application.Features.Ope.Datos.OpeDatosAsistencias.Queries.GetOpeDatosAsistenciasList
{
    public class GetOpeDatosAsistenciasListQueryHandler : IRequestHandler<GetOpeDatosAsistenciasListQuery, PaginationVm<OpeDatoAsistenciaVm>>
    {
        private readonly ILogger<GetOpeDatosAsistenciasListQueryHandler> _logger;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public GetOpeDatosAsistenciasListQueryHandler(
        ILogger<GetOpeDatosAsistenciasListQueryHandler> logger,
        IUnitOfWork unitOfWork,
        IMapper mapper
        )
        {
            _logger = logger;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<PaginationVm<OpeDatoAsistenciaVm>> Handle(GetOpeDatosAsistenciasListQuery request, CancellationToken cancellationToken)
        {
            _logger.LogInformation($"{nameof(GetOpeDatosAsistenciasListQueryHandler)} - BEGIN");

            var spec = new OpeDatosAsistenciasSpecification(request);
            var opeDatosAsistencias = await _unitOfWork.Repository<OpeDatoAsistencia>()
                .GetAllWithSpec(spec);

            // Para quitar datos borrados (sanitarias, sociales, traducciones)
            foreach (var asistencia in opeDatosAsistencias)
            {
                asistencia.OpeDatosAsistenciasSociales = asistencia.OpeDatosAsistenciasSociales?
                    .Where(s => !s.Borrado)
                    .Select(s =>
                    {
                        s.OpeDatosAsistenciasSocialesTareas = s.OpeDatosAsistenciasSocialesTareas?.Where(t => !t.Borrado).ToList();
                        s.OpeDatosAsistenciasSocialesOrganismos = s.OpeDatosAsistenciasSocialesOrganismos?.Where(o => !o.Borrado).ToList();
                        s.OpeDatosAsistenciasSocialesUsuarios = s.OpeDatosAsistenciasSocialesUsuarios?.Where(u => !u.Borrado).ToList();
                        return s;
                    }).ToList();

                asistencia.OpeDatosAsistenciasTraducciones = asistencia.OpeDatosAsistenciasTraducciones?
                    .Where(t => !t.Borrado)
                    .ToList();
            }

            // Aplicar filtro numérico
            var opeDatosAsistenciasList = opeDatosAsistencias.ToList();
            opeDatosAsistencias = AplicarFiltroNumerico(request, opeDatosAsistenciasList);

            var totalOpeDatosAsistencias = opeDatosAsistencias.Count();
            var opeDatoAsistenciaVmList = _mapper.Map<List<OpeDatoAsistenciaVm>>(opeDatosAsistencias);

            var rounded = Math.Ceiling(Convert.ToDecimal(totalOpeDatosAsistencias) / Convert.ToDecimal(request.PageSize));
            var totalPages = Convert.ToInt32(rounded);

            var pagination = new PaginationVm<OpeDatoAsistenciaVm>
            {
                Count = totalOpeDatosAsistencias,
                Data = opeDatoAsistenciaVmList,
                PageCount = totalPages,
                PageIndex = request.PageIndex,
                PageSize = request.PageSize
            };

            _logger.LogInformation($"{nameof(GetOpeDatosAsistenciasListQueryHandler)} - END");
            return pagination;
        }

        private List<OpeDatoAsistencia> AplicarFiltroNumerico(GetOpeDatosAsistenciasListQuery request, List<OpeDatoAsistencia> opeDatosAsistencias)
        {
            if (string.IsNullOrEmpty(request.CriterioNumerico))
                return opeDatosAsistencias;

            switch (request.CriterioNumerico.ToLower())
            {
                case "total":
                    return FiltrarPorTotal(request, opeDatosAsistencias);
                case "sanitarias":
                    return FiltrarPorSanitarias(request, opeDatosAsistencias);
                case "sociales":
                    return FiltrarPorSociales(request, opeDatosAsistencias);
                case "traducciones":
                    return FiltrarPorTraducciones(request, opeDatosAsistencias);
                default:
                    return opeDatosAsistencias;
            }
        }

        private List<OpeDatoAsistencia> FiltrarPorTotal(GetOpeDatosAsistenciasListQuery request, List<OpeDatoAsistencia> opeDatosAsistencias)
        {
            Func<OpeDatoAsistencia, int> selector = x =>
                (x.OpeDatosAsistenciasSanitarias?.Sum(s => s.Numero) ?? 0) +
                (x.OpeDatosAsistenciasSociales?.Sum(s => s.Numero) ?? 0) +
                (x.OpeDatosAsistenciasTraducciones?.Sum(t => t.Numero) ?? 0);

            return FiltrarPorSelector(request, opeDatosAsistencias, selector);
        }

        private List<OpeDatoAsistencia> FiltrarPorSanitarias(GetOpeDatosAsistenciasListQuery request, List<OpeDatoAsistencia> opeDatosAsistencias)
        {
            Func<OpeDatoAsistencia, int> selector = x => x.OpeDatosAsistenciasSanitarias?.Sum(s => s.Numero) ?? 0;

            return FiltrarPorSelector(request, opeDatosAsistencias, selector);
        }

        private List<OpeDatoAsistencia> FiltrarPorSociales(GetOpeDatosAsistenciasListQuery request, List<OpeDatoAsistencia> opeDatosAsistencias)
        {
            Func<OpeDatoAsistencia, int> selector = x => x.OpeDatosAsistenciasSociales?.Sum(s => s.Numero) ?? 0;

            return FiltrarPorSelector(request, opeDatosAsistencias, selector);
        }

        private List<OpeDatoAsistencia> FiltrarPorTraducciones(GetOpeDatosAsistenciasListQuery request, List<OpeDatoAsistencia> opeDatosAsistencias)
        {
            Func<OpeDatoAsistencia, int> selector = x => x.OpeDatosAsistenciasTraducciones?.Sum(t => t.Numero) ?? 0;

            return FiltrarPorSelector(request, opeDatosAsistencias, selector);
        }

        private List<OpeDatoAsistencia> FiltrarPorSelector(GetOpeDatosAsistenciasListQuery request, List<OpeDatoAsistencia> opeDatosAsistencias, Func<OpeDatoAsistencia, int> selector)
        {
            switch (request.CriterioNumericoRadio?.ToLower())
            {
                case "maximo":
                    int max = opeDatosAsistencias.Max(selector);
                    return opeDatosAsistencias.Where(x => selector(x) == max).ToList();

                case "minimo":
                    int min = opeDatosAsistencias.Min(selector);
                    return opeDatosAsistencias.Where(x => selector(x) == min).ToList();

                case "cantidad":
                    if (!request.CriterioNumericoCriterioCantidadCantidad.HasValue)
                        return opeDatosAsistencias;

                    int cantidadFiltro = request.CriterioNumericoCriterioCantidadCantidad.Value;
                    switch (request.CriterioNumericoCriterioCantidad?.ToLower())
                    {
                        case "igual":
                            return opeDatosAsistencias.Where(x => selector(x) == cantidadFiltro).ToList();
                        case "mayor":
                            return opeDatosAsistencias.Where(x => selector(x) > cantidadFiltro).ToList();
                        case "menor":
                            return opeDatosAsistencias.Where(x => selector(x) < cantidadFiltro).ToList();
                        default:
                            return opeDatosAsistencias;
                    }

                default:
                    return opeDatosAsistencias;
            }
        }


    }
}
