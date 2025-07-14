using DGPCE.Sigemad.Application.Contracts.Persistence;
using DGPCE.Sigemad.Application.Specifications.Ope.Datos.OpeAsistenciasSocialesEdades;
using DGPCE.Sigemad.Domain.Modelos.Ope.Datos;
using MediatR;

namespace DGPCE.Sigemad.Application.Features.Ope.Datos.OpeAsistenciasSocialesEdades.Queries.GetOpeAsistenciasSocialesEdadesList
{
    public class GetOpeAsistenciasSocialesEdadesListQueryHandler : IRequestHandler<GetOpeAsistenciasSocialesEdadesListQuery, IReadOnlyList<OpeAsistenciaSocialEdad>>
    {
        private readonly IUnitOfWork _unitOfWork;

        public GetOpeAsistenciasSocialesEdadesListQueryHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<IReadOnlyList<OpeAsistenciaSocialEdad>> Handle(GetOpeAsistenciasSocialesEdadesListQuery request, CancellationToken cancellationToken)
        {
            var specification = new OpeAsistenciasSocialesEdadesSpecification();
            var opeAsistenciasSocialesEdades = await _unitOfWork.Repository<OpeAsistenciaSocialEdad>().GetAllWithSpec(specification);
            return opeAsistenciasSocialesEdades;
        }

    }


}