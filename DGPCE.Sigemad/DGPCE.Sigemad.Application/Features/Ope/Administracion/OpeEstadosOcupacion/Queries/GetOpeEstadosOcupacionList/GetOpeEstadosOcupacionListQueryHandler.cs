using DGPCE.Sigemad.Application.Contracts.Persistence;
using DGPCE.Sigemad.Application.Specifications.Ope.Administracion.OpeEstadosOcupacion;
using DGPCE.Sigemad.Domain.Modelos.Ope.Administracion;
using MediatR;


namespace DGPCE.Sigemad.Application.Features.Ope.Administracion.OpeEstadosOcupacion.Queries.GetOpeEstadosOcupacionList
{
    public class GetOpeEstadosOcupacionListQueryHandler : IRequestHandler<GetOpeEstadosOcupacionListQuery, IReadOnlyList<OpeEstadoOcupacion>>
    {
        private readonly IUnitOfWork _unitOfWork;

        public GetOpeEstadosOcupacionListQueryHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<IReadOnlyList<OpeEstadoOcupacion>> Handle(GetOpeEstadosOcupacionListQuery request, CancellationToken cancellationToken)
        {
            var specification = new OpeEstadosOcupacionSpecification();
            var opeEstadosOcupacion = await _unitOfWork.Repository<OpeEstadoOcupacion>().GetAllWithSpec(specification);
            return opeEstadosOcupacion;
        }

    }


}