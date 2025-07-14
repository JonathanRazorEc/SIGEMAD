using DGPCE.Sigemad.Application.Contracts.Persistence;
using DGPCE.Sigemad.Application.Specifications.Ope.Administracion.OpePeriodosTipos;
using DGPCE.Sigemad.Domain.Modelos.Ope.Administracion;
using MediatR;


namespace DGPCE.Sigemad.Application.Features.Ope.Datos.OpePeriodosTipos.Queries.GetOpePeriodosTiposList
{
    public class GetOpePeriodosTiposListQueryHandler : IRequestHandler<GetOpePeriodosTiposListQuery, IReadOnlyList<OpePeriodoTipo>>
    {
        private readonly IUnitOfWork _unitOfWork;

        public GetOpePeriodosTiposListQueryHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<IReadOnlyList<OpePeriodoTipo>> Handle(GetOpePeriodosTiposListQuery request, CancellationToken cancellationToken)
        {
            var specification = new OpePeriodosTiposSpecification();
            var opePeriodosTipos = await _unitOfWork.Repository<OpePeriodoTipo>().GetAllWithSpec(specification);
            return opePeriodosTipos;
        }

    }


}