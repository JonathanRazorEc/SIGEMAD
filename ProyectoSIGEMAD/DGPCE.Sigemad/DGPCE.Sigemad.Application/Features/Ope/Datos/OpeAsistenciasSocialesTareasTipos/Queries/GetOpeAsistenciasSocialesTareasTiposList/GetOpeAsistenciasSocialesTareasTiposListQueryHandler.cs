using DGPCE.Sigemad.Application.Contracts.Persistence;
using DGPCE.Sigemad.Application.Specifications.Ope.Datos.OpeAsistenciasSocialesTareasTipos;
using DGPCE.Sigemad.Domain.Modelos.Ope.Datos;
using MediatR;


namespace DGPCE.Sigemad.Application.Features.Ope.Datos.OpeAsistenciasSocialesTareasTipos.Queries.GetOpeAsistenciasSocialesTareasTiposList
{
    public class GetOpeAsistenciasSocialesTareasTiposListQueryHandler : IRequestHandler<GetOpeAsistenciasSocialesTareasTiposListQuery, IReadOnlyList<OpeAsistenciaSocialTareaTipo>>
    {
        private readonly IUnitOfWork _unitOfWork;

        public GetOpeAsistenciasSocialesTareasTiposListQueryHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<IReadOnlyList<OpeAsistenciaSocialTareaTipo>> Handle(GetOpeAsistenciasSocialesTareasTiposListQuery request, CancellationToken cancellationToken)
        {
            var specification = new OpeAsistenciasSocialesTareasTiposSpecification();
            var opeAsistenciasSocialesTareasTipos = await _unitOfWork.Repository<OpeAsistenciaSocialTareaTipo>().GetAllWithSpec(specification);
            return opeAsistenciasSocialesTareasTipos;
        }

    }


}