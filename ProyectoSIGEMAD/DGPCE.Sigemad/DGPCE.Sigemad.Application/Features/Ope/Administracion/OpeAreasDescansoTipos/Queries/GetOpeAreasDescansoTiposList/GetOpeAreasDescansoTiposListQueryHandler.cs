using DGPCE.Sigemad.Application.Contracts.Persistence;
using DGPCE.Sigemad.Application.Specifications.Ope.Administracion.OpeAreasDescansoTipos;
using DGPCE.Sigemad.Domain.Modelos.Ope.Administracion;
using MediatR;


namespace DGPCE.Sigemad.Application.Features.Ope.Administracion.OpeAreasDescansoTipos.Queries.GetOpeAreasDescansoTiposList
{
    public class GetOpeAreasDescansoTiposListQueryHandler : IRequestHandler<GetOpeAreasDescansoTiposListQuery, IReadOnlyList<OpeAreaDescansoTipo>>
    {
        private readonly IUnitOfWork _unitOfWork;

        public GetOpeAreasDescansoTiposListQueryHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<IReadOnlyList<OpeAreaDescansoTipo>> Handle(GetOpeAreasDescansoTiposListQuery request, CancellationToken cancellationToken)
        {
            var specification = new OpeAreasDescansoTiposSpecification();
            var opeAreasDescansoTipos = await _unitOfWork.Repository<OpeAreaDescansoTipo>().GetAllWithSpec(specification);
            return opeAreasDescansoTipos;
        }

    }


}