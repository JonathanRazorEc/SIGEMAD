using DGPCE.Sigemad.Application.Contracts.Persistence;
using DGPCE.Sigemad.Application.Specifications.Ope.Administracion.OpePaises;
using DGPCE.Sigemad.Domain.Modelos.Ope.Administracion;
using MediatR;

namespace DGPCE.Sigemad.Application.Features.Ope.Administracion.OpePaises.Queries.GetOpePaisesList
{
    public class GetOpePaisesListQueryHandler : IRequestHandler<GetOpePaisesListQuery, IReadOnlyList<OpePais>>
    {
        private readonly IUnitOfWork _unitOfWork;

        public GetOpePaisesListQueryHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<IReadOnlyList<OpePais>> Handle(GetOpePaisesListQuery request, CancellationToken cancellationToken)
        {
            var specification = new OpePaisesSpecification(request);
            var opePaises = await _unitOfWork.Repository<OpePais>().GetAllWithSpec(specification);
            return opePaises;
        }
    }
}