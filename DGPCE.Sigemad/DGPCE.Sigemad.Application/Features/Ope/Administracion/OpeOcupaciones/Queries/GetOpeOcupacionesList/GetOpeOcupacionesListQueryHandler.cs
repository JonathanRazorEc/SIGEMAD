using DGPCE.Sigemad.Application.Contracts.Persistence;
using DGPCE.Sigemad.Application.Specifications.Ope.Administracion.OpeOcupaciones;
using DGPCE.Sigemad.Domain.Modelos.Ope.Administracion;
using MediatR;


namespace DGPCE.Sigemad.Application.Features.Ope.Administracion.OpeOcupaciones.Queries.GetOpeOcupacionesList
{
    public class GetOpeOcupacionesListQueryHandler : IRequestHandler<GetOpeOcupacionesListQuery, IReadOnlyList<OpeOcupacion>>
    {
        private readonly IUnitOfWork _unitOfWork;

        public GetOpeOcupacionesListQueryHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<IReadOnlyList<OpeOcupacion>> Handle(GetOpeOcupacionesListQuery request, CancellationToken cancellationToken)
        {
            var specification = new OpeOcupacionesSpecification();
            var opeOcupaciones = await _unitOfWork.Repository<OpeOcupacion>().GetAllWithSpec(specification);
            return opeOcupaciones;
        }

    }


}