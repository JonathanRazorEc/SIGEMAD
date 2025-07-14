using AutoMapper;
using DGPCE.Sigemad.Application.Contracts.Persistence;
using DGPCE.Sigemad.Application.Features.Provincias.Vms;
using DGPCE.Sigemad.Domain.Modelos;
using MediatR;

namespace DGPCE.Sigemad.Application.Features.Provincias.Queries.GetProvinciasList
{
    public class GetProvinciasListQueryHandler : IRequestHandler<GetProvinciasListQuery, IReadOnlyList<ProvinciaSinMunicipiosConIdComunidadVm>>
    {


        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public GetProvinciasListQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<IReadOnlyList<ProvinciaSinMunicipiosConIdComunidadVm>> Handle(GetProvinciasListQuery request, CancellationToken cancellationToken)
        {

            IReadOnlyList<ProvinciaSinMunicipiosConIdComunidadVm> provinces = await _unitOfWork.Repository<Provincia>().GetAsync(
                predicate: null,
                selector: p => new ProvinciaSinMunicipiosConIdComunidadVm
                {
                    Id = p.Id,
                    Descripcion = p.Descripcion,
                    Huso = p.Huso,
                    IdCcaa = p.IdCcaa,
                    GeoPosicion = p.GeoPosicion,
                    UtmX = p.UtmX,
                    UtmY = p.UtmY,
                },
                orderBy: q => q.OrderBy(p => p.Descripcion),
                disableTracking: true
                );


            return provinces;

        }
    }
}
