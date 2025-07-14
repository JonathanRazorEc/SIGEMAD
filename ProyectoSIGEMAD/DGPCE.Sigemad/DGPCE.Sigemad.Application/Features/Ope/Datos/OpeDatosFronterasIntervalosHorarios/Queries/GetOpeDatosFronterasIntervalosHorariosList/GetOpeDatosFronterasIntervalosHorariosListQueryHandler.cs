using DGPCE.Sigemad.Application.Contracts.Persistence;
using DGPCE.Sigemad.Application.Specifications.Ope.Datos.OpeDatosFronterasIntervalosHorarios;
using DGPCE.Sigemad.Domain.Modelos.Ope.Datos;
using MediatR;


namespace DGPCE.Sigemad.Application.Features.Ope.Datos.OpeDatosFronterasIntervalosHorarios.Queries.GetOpeDatosFronterasIntervalosHorariosList
{
    public class GetOpeDatosFronterasIntervalosHorariosListQueryHandler : IRequestHandler<GetOpeDatosFronterasIntervalosHorariosListQuery, IReadOnlyList<OpeDatoFronteraIntervaloHorario>>
    {
        private readonly IUnitOfWork _unitOfWork;

        public GetOpeDatosFronterasIntervalosHorariosListQueryHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<IReadOnlyList<OpeDatoFronteraIntervaloHorario>> Handle(GetOpeDatosFronterasIntervalosHorariosListQuery request, CancellationToken cancellationToken)
        {
            var specification = new OpeDatosFronterasIntervalosHorariosSpecification();
            var opeDatosFronterasIntervalosHorarios = await _unitOfWork.Repository<OpeDatoFronteraIntervaloHorario>().GetAllWithSpec(specification);
            return opeDatosFronterasIntervalosHorarios;
        }

    }
}