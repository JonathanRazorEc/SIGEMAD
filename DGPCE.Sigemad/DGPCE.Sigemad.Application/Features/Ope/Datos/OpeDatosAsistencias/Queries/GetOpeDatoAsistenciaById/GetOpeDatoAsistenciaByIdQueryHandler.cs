using AutoMapper;
using DGPCE.Sigemad.Application.Contracts.Persistence;
using DGPCE.Sigemad.Application.Exceptions;
using DGPCE.Sigemad.Application.Specifications.Ope.Datos.OpeDatosAsistencias;
using DGPCE.Sigemad.Domain.Modelos.Ope.Datos;
using MediatR;
using Microsoft.Extensions.Logging;


namespace DGPCE.Sigemad.Application.Features.Ope.Datos.OpeDatosAsistencias.Queries.GetOpeDatoAsistenciaById
{
    public class GetOpeDatoAsistenciaByIdQueryHandler : IRequestHandler<GetOpeDatoAsistenciaByIdQuery, OpeDatoAsistencia>
    {

        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly ILogger<GetOpeDatoAsistenciaByIdQueryHandler> _logger;

        public GetOpeDatoAsistenciaByIdQueryHandler(
            IUnitOfWork unitOfWork, IMapper mapper,
            ILogger<GetOpeDatoAsistenciaByIdQueryHandler> logger)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<OpeDatoAsistencia> Handle(GetOpeDatoAsistenciaByIdQuery request, CancellationToken cancellationToken)
        {
            var opeDatoAsistenciaParams = new OpeDatosAsistenciasSpecificationParams
            {
                Id = request.Id,
            };

            var spec = new OpeDatosAsistenciasSpecification(opeDatoAsistenciaParams);
            var opeDatoAsistencia = await _unitOfWork.Repository<OpeDatoAsistencia>().GetByIdWithSpec(spec);

            if (opeDatoAsistencia == null)
            {
                _logger.LogWarning($"No se encontro dato de asistencia de OPE con id: {request.Id}");
                throw new NotFoundException(nameof(OpeDatoAsistencia), request.Id);
            }

            return opeDatoAsistencia;
        }
    }
}
