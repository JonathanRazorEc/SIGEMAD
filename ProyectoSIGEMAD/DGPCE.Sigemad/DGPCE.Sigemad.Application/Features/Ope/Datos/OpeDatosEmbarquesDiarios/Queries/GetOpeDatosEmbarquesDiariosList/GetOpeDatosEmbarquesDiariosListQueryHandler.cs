using AutoMapper;
using DGPCE.Sigemad.Application.Contracts.Persistence;
using DGPCE.Sigemad.Application.Features.Ope.Datos.OpeDatosEmbarquesDiarios.Vms;
using DGPCE.Sigemad.Application.Features.Shared;
using DGPCE.Sigemad.Application.Specifications.Ope.Datos.OpeDatosEmbarquesDiarios;
using DGPCE.Sigemad.Domain.Modelos.Ope.Datos;
using MediatR;
using Microsoft.Extensions.Logging;

namespace DGPCE.Sigemad.Application.Features.Ope.Datos.OpeDatosEmbarquesDiarios.Queries.GetOpeDatosEmbarquesDiariosList
{
    public class GetOpeDatosEmbarquesDiariosListQueryHandler : IRequestHandler<GetOpeDatosEmbarquesDiariosListQuery, PaginationVm<OpeDatoEmbarqueDiarioVm>>
    {
        private readonly ILogger<GetOpeDatosEmbarquesDiariosListQueryHandler> _logger;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public GetOpeDatosEmbarquesDiariosListQueryHandler(
        ILogger<GetOpeDatosEmbarquesDiariosListQueryHandler> logger,
        IUnitOfWork unitOfWork,
        IMapper mapper
        )
        {
            _logger = logger;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<PaginationVm<OpeDatoEmbarqueDiarioVm>> Handle(GetOpeDatosEmbarquesDiariosListQuery request, CancellationToken cancellationToken)
        {
            _logger.LogInformation($"{nameof(GetOpeDatosEmbarquesDiariosListQueryHandler)} - BEGIN");

            var spec = new OpeDatosEmbarquesDiariosSpecification(request);
            var opeDatosEmbarquesDiarios = await _unitOfWork.Repository<OpeDatoEmbarqueDiario>()
            .GetAllWithSpec(spec);

            // Filtro numérico
            var opeDatosEmbarquesDiariosList = opeDatosEmbarquesDiarios.ToList();
            opeDatosEmbarquesDiarios = AplicarFiltroNumerico(request, opeDatosEmbarquesDiariosList);
            var totalOpeDatosEmbarquesDiarios = opeDatosEmbarquesDiarios.Count();
            // Fin Filtro numérico

            
            //var specCount = new OpeDatosEmbarquesDiariosForCountingSpecification(request);
            //var totalOpeDatosEmbarquesDiarios = await _unitOfWork.Repository<OpeDatoEmbarqueDiario>().CountAsync(specCount);
            var opeDatoEmbarqueDiarioVmList = _mapper.Map<List<OpeDatoEmbarqueDiarioVm>>(opeDatosEmbarquesDiarios);
            

            var rounded = Math.Ceiling(Convert.ToDecimal(totalOpeDatosEmbarquesDiarios) / Convert.ToDecimal(request.PageSize));
            var totalPages = Convert.ToInt32(rounded);

            var pagination = new PaginationVm<OpeDatoEmbarqueDiarioVm>
            {
                Count = totalOpeDatosEmbarquesDiarios,
                Data = opeDatoEmbarqueDiarioVmList,
                PageCount = totalPages,
                PageIndex = request.PageIndex,
                PageSize = request.PageSize
            };

            _logger.LogInformation($"{nameof(GetOpeDatosEmbarquesDiariosListQueryHandler)} - END");
            return pagination;
        }

        private List<OpeDatoEmbarqueDiario> AplicarFiltroNumerico(GetOpeDatosEmbarquesDiariosListQuery request, List<OpeDatoEmbarqueDiario> opeDatosEmbarquesDiarios)
        {
            if (string.IsNullOrEmpty(request.CriterioNumerico))
                return opeDatosEmbarquesDiarios;

            switch (request.CriterioNumerico.ToLower())
            {
                case "rotaciones":
                    opeDatosEmbarquesDiarios = FiltrarPorSelector(request, opeDatosEmbarquesDiarios, x => x.NumeroRotaciones);
                    break;
                case "pasajeros":
                    opeDatosEmbarquesDiarios = FiltrarPorSelector(request, opeDatosEmbarquesDiarios, x => x.NumeroPasajeros);
                    break;
                case "turismos":
                    opeDatosEmbarquesDiarios = FiltrarPorSelector(request, opeDatosEmbarquesDiarios, x => x.NumeroTurismos);
                    break;
                case "totalvehiculos":
                    opeDatosEmbarquesDiarios = FiltrarPorSelector(request, opeDatosEmbarquesDiarios, x => x.NumeroTotalVehiculos);
                    break;
            }

            return opeDatosEmbarquesDiarios;
        }

        //
        private List<OpeDatoEmbarqueDiario> FiltrarPorSelector(
            GetOpeDatosEmbarquesDiariosListQuery request,
            List<OpeDatoEmbarqueDiario> opeDatosEmbarquesDiarios,
            Func<OpeDatoEmbarqueDiario, int> selector)
        {
            switch (request.CriterioNumericoRadio)
            {
                case "maximo":
                    int max = opeDatosEmbarquesDiarios.Max(selector);
                    return opeDatosEmbarquesDiarios.Where(x => selector(x) == max).ToList();

                case "minimo":
                    int min = opeDatosEmbarquesDiarios.Min(selector);
                    return opeDatosEmbarquesDiarios.Where(x => selector(x) == min).ToList();

                case "cantidad":
                    return FiltrarPorCantidad(request, opeDatosEmbarquesDiarios, selector);
            }

            return opeDatosEmbarquesDiarios;
        }


        private List<OpeDatoEmbarqueDiario> FiltrarPorCantidad(GetOpeDatosEmbarquesDiariosListQuery request, List<OpeDatoEmbarqueDiario> opeDatosEmbarquesDiarios, Func<OpeDatoEmbarqueDiario, int> selector)
        {
            // Verificamos si criterioNumericoCriterioCantidadCantidad no es null
            if (!request.CriterioNumericoCriterioCantidadCantidad.HasValue)
            {
                // Si es null, devolvemos los datos sin aplicar filtro
                return opeDatosEmbarquesDiarios;
            }

            // Asignamos el valor de criterioNumericoCriterioCantidadCantidad, que es un valor no nulo
            int cantidadFiltro = request.CriterioNumericoCriterioCantidadCantidad.Value;

            switch (request.CriterioNumericoCriterioCantidad)
            {
                case "igual":
                    return opeDatosEmbarquesDiarios.Where(x => selector(x) == cantidadFiltro).ToList();

                case "mayor":
                    return opeDatosEmbarquesDiarios.Where(x => selector(x) > cantidadFiltro).ToList();

                case "menor":
                    return opeDatosEmbarquesDiarios.Where(x => selector(x) < cantidadFiltro).ToList();

                default:
                    return opeDatosEmbarquesDiarios;
            }
        }

    }

}