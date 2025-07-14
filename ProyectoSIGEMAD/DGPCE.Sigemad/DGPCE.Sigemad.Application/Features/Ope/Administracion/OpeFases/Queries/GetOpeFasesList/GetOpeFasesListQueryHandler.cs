using DGPCE.Sigemad.Application.Contracts.Persistence;
using DGPCE.Sigemad.Application.Specifications.Ope.Administracion.OpeFases;
using DGPCE.Sigemad.Domain.Modelos.Ope.Administracion;
using MediatR;


namespace DGPCE.Sigemad.Application.Features.Ope.Administracion.OpeFases.Queries.GetOpeFasesList
{
    public class GetOpeFasesListQueryHandler : IRequestHandler<GetOpeFasesListQuery, IReadOnlyList<OpeFase>>
    {
        private readonly IUnitOfWork _unitOfWork;

        public GetOpeFasesListQueryHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<IReadOnlyList<OpeFase>> Handle(GetOpeFasesListQuery request, CancellationToken cancellationToken)
        {
            var specification = new OpeFasesSpecification();
            var opeFases = await _unitOfWork.Repository<OpeFase>().GetAllWithSpec(specification);
            return opeFases;
        }

    }


}