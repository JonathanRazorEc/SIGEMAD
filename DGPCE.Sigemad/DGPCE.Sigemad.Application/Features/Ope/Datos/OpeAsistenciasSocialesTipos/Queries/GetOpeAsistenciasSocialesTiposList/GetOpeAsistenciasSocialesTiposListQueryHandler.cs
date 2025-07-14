using DGPCE.Sigemad.Application.Contracts.Persistence;
using DGPCE.Sigemad.Application.Specifications.Ope.Datos.OpeAsistenciasSocialesTipos;
using DGPCE.Sigemad.Domain.Modelos.Ope.Datos;
using MediatR;


namespace DGPCE.Sigemad.Application.Features.Ope.Datos.OpeAsistenciasSocialesTipos.Queries.GetOpeAsistenciasSocialesTiposList
{
    public class GetOpeAsistenciasSocialesTiposListQueryHandler : IRequestHandler<GetOpeAsistenciasSocialesTiposListQuery, IReadOnlyList<OpeAsistenciaSocialTipo>>
    {
        private readonly IUnitOfWork _unitOfWork;

        public GetOpeAsistenciasSocialesTiposListQueryHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<IReadOnlyList<OpeAsistenciaSocialTipo>> Handle(GetOpeAsistenciasSocialesTiposListQuery request, CancellationToken cancellationToken)
        {
            var specification = new OpeAsistenciasSocialesTiposSpecification();
            var opeAsistenciasSocialesTipos = await _unitOfWork.Repository<OpeAsistenciaSocialTipo>().GetAllWithSpec(specification);
            return opeAsistenciasSocialesTipos;
        }

    }


}