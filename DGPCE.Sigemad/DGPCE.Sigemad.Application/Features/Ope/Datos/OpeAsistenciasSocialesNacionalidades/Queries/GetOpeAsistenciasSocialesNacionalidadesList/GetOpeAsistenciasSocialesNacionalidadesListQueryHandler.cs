using DGPCE.Sigemad.Application.Contracts.Persistence;
using DGPCE.Sigemad.Application.Specifications.Ope.Datos.OpeAsistenciasSocialesNacionalidades;
using DGPCE.Sigemad.Domain.Modelos.Ope.Datos;
using MediatR;


namespace DGPCE.Sigemad.Application.Features.Ope.Datos.OpeAsistenciasSocialesNacionalidades.Queries.GetOpeAsistenciasSocialesNacionalidadesList
{
    public class GetOpeAsistenciasSocialesNacionalidadesListQueryHandler : IRequestHandler<GetOpeAsistenciasSocialesNacionalidadesListQuery, IReadOnlyList<OpeAsistenciaSocialNacionalidad>>
    {
        private readonly IUnitOfWork _unitOfWork;

        public GetOpeAsistenciasSocialesNacionalidadesListQueryHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<IReadOnlyList<OpeAsistenciaSocialNacionalidad>> Handle(GetOpeAsistenciasSocialesNacionalidadesListQuery request, CancellationToken cancellationToken)
        {
            var specification = new OpeAsistenciasSocialesNacionalidadesSpecification();
            var opeAsistenciasSocialesNacionalidades = await _unitOfWork.Repository<OpeAsistenciaSocialNacionalidad>().GetAllWithSpec(specification);
            return opeAsistenciasSocialesNacionalidades;
        }

    }


}