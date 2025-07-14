using AutoMapper;
using DGPCE.Sigemad.Application.Contracts.Persistence;
using DGPCE.Sigemad.Application.Dtos.OtraInformaciones;
using DGPCE.Sigemad.Application.Exceptions;
using DGPCE.Sigemad.Application.Features.OtrasInformaciones.Queries.GetOtrasInformacionesList;
using DGPCE.Sigemad.Application.Specifications.OtrasInformaciones;
using DGPCE.Sigemad.Domain.Modelos;
using MediatR;
using Microsoft.Extensions.Logging;

namespace DGPCE.Sigemad.Application.Features.OtrasInformaciones.Queries.GetOtrasInformacionesById;
public class GetOtraInformacionByIdQueryHandler : IRequestHandler<GetOtraInformacionByIdQuery, OtraInformacionDto>
{
    private readonly ILogger<GetOtraInformacionByIdQueryHandler> _logger;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public GetOtraInformacionByIdQueryHandler(
        IUnitOfWork unitOfWork,
        ILogger<GetOtraInformacionByIdQueryHandler> logger,
        IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _logger = logger;
        _mapper = mapper;
    }

    public async Task<OtraInformacionDto> Handle(GetOtraInformacionByIdQuery request, CancellationToken cancellationToken)
    {
        _logger.LogInformation($"{nameof(GetOtraInformacionByIdQueryHandler)} - BEGIN");

        var spec = new OtraInformacionActiveByIdSpecification(request.Id);
        var otraInformacion = await _unitOfWork.Repository<OtraInformacion>().GetByIdWithSpec(spec);

        if (otraInformacion is null)
        {
            _logger.LogWarning($"No se encontró otra información con id: {request.Id}");
            throw new NotFoundException(nameof(OtraInformacion), request.Id);
        }

        // Filtrar ProcedenciasDestinos que no están borrados
        otraInformacion.DetallesOtraInformacion = otraInformacion.DetallesOtraInformacion
            .Select(detalle =>
            {
                detalle.ProcedenciasDestinos = detalle.ProcedenciasDestinos
                    .Where(pd => !pd.Borrado)
                    .ToList();
                return detalle;
            })
            .ToList();

        var otraInformacionDto = _mapper.Map<OtraInformacion, OtraInformacionDto>(otraInformacion);

        _logger.LogInformation($"{nameof(GetOtraInformacionByIdQueryHandler)} - END");
        return otraInformacionDto;
    }


}
