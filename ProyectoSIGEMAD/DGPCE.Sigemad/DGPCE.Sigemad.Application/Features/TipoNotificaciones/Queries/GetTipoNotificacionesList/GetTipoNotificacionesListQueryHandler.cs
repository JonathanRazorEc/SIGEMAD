using DGPCE.Sigemad.Application.Contracts.Persistence;
using DGPCE.Sigemad.Domain.Modelos;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DGPCE.Sigemad.Application.Features.TipoNotificaciones.Queries.GetTipoNotificacionesList;
public class GetTipoNotificacionesListQueryHandler : IRequestHandler<GetTipoNotificacionesListQuery, IReadOnlyList<TipoNotificacion>>
{
    private readonly IUnitOfWork _unitOfWork;

    public GetTipoNotificacionesListQueryHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }


    public async Task<IReadOnlyList<TipoNotificacion>> Handle(GetTipoNotificacionesListQuery request, CancellationToken cancellationToken)
    {
        var tipoNotificacion = await _unitOfWork.Repository<TipoNotificacion>().GetAllNoTrackingAsync();
        return tipoNotificacion;
    }
}
