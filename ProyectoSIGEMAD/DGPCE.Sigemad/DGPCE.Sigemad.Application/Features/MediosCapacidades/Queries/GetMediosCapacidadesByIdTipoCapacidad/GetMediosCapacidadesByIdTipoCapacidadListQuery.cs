using DGPCE.Sigemad.Application.Dtos.IntervencionMedios;
using MediatR;

namespace DGPCE.Sigemad.Application.Features.MediosCapacidades.Queries.GetMediosCapacidadesByIdTipoCapacidad;
public class GetMediosCapacidadesByIdTipoCapacidadListQuery : IRequest<IReadOnlyList<MediosCapacidadDto>>
{
    public int idTipoCapacidad { get; set; }


    public GetMediosCapacidadesByIdTipoCapacidadListQuery(int id)
    {
        idTipoCapacidad = id;
    }

}

