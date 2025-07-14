using DGPCE.Sigemad.Application.Contracts.Persistence;
using DGPCE.Sigemad.Application.Specifications.Ope.Datos.OpeAsistenciasSocialesOrganismosTipos;
using DGPCE.Sigemad.Domain.Modelos.Ope.Datos;
using MediatR;


namespace DGPCE.Sigemad.Application.Features.Ope.Datos.OpeAsistenciasSocialesOrganismosTipos.Queries.GetOpeAsistenciasSocialesOrganismosTiposList
{
    public class GetOpeAsistenciasSocialesOrgsTiposListQueryHandler : IRequestHandler<GetOpeAsistenciasSocialesOrganismosTiposListQuery, IReadOnlyList<OpeAsistenciaSocialOrganismoTipo>>
    {
        private readonly IUnitOfWork _unitOfWork;

        public GetOpeAsistenciasSocialesOrgsTiposListQueryHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<IReadOnlyList<OpeAsistenciaSocialOrganismoTipo>> Handle(GetOpeAsistenciasSocialesOrganismosTiposListQuery request, CancellationToken cancellationToken)
        {
            var specification = new OpeAsistenciasSocialesOrganismosTiposSpecification();
            var opeAsistenciasSocialesOrganismosTipos = await _unitOfWork.Repository<OpeAsistenciaSocialOrganismoTipo>().GetAllWithSpec(specification);
            return opeAsistenciasSocialesOrganismosTipos;
        }

    }


}