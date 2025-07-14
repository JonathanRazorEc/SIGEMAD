using DGPCE.Sigemad.Application.Contracts.Persistence;
using DGPCE.Sigemad.Application.Specifications.Ope.Datos.OpeAsistenciasSocialesSexos;
using DGPCE.Sigemad.Domain.Modelos.Ope.Datos;
using MediatR;


namespace DGPCE.Sigemad.Application.Features.Ope.Datos.OpeAsistenciasSocialesSexos.Queries.GetOpeAsistenciasSocialesSexosList
{
    public class GetOpeAsistenciasSocialesSexosListQueryHandler : IRequestHandler<GetOpeAsistenciasSocialesSexosListQuery, IReadOnlyList<OpeAsistenciaSocialSexo>>
    {
        private readonly IUnitOfWork _unitOfWork;

        public GetOpeAsistenciasSocialesSexosListQueryHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<IReadOnlyList<OpeAsistenciaSocialSexo>> Handle(GetOpeAsistenciasSocialesSexosListQuery request, CancellationToken cancellationToken)
        {
            var specification = new OpeAsistenciasSocialesSexosSpecification();
            var opeAsistenciasSocialesSexos = await _unitOfWork.Repository<OpeAsistenciaSocialSexo>().GetAllWithSpec(specification);
            return opeAsistenciasSocialesSexos;
        }

    }


}