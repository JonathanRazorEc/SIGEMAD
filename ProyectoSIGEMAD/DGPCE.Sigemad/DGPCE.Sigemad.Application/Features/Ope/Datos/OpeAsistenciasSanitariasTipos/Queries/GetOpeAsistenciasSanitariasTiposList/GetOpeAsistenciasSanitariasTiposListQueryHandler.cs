using DGPCE.Sigemad.Application.Contracts.Persistence;
using DGPCE.Sigemad.Application.Specifications.Ope.Datos.OpeAsistenciasSanitariasTipos;
using DGPCE.Sigemad.Domain.Modelos.Ope.Datos;
using MediatR;


namespace DGPCE.Sigemad.Application.Features.Ope.Datos.OpeAsistenciasSanitariasTipos.Queries.GetOpeAsistenciasSanitariasTiposList
{
    public class GetOpeAsistenciasSanitariasTiposListQueryHandler : IRequestHandler<GetOpeAsistenciasSanitariasTiposListQuery, IReadOnlyList<OpeAsistenciaSanitariaTipo>>
    {
        private readonly IUnitOfWork _unitOfWork;

        public GetOpeAsistenciasSanitariasTiposListQueryHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<IReadOnlyList<OpeAsistenciaSanitariaTipo>> Handle(GetOpeAsistenciasSanitariasTiposListQuery request, CancellationToken cancellationToken)
        {
            var specification = new OpeAsistenciasSanitariasTiposSpecification();
            var opeAsistenciasSanitariasTipos = await _unitOfWork.Repository<OpeAsistenciaSanitariaTipo>().GetAllWithSpec(specification);
            return opeAsistenciasSanitariasTipos;
        }

    }


}