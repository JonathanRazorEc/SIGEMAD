using DGPCE.Sigemad.Domain.Modelos;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DGPCE.Sigemad.Application.Features.NivelesGravedad.Queries.GetNivelesGravedadList
{
    public class GetNivelesGravedadListQuery: IRequest<IReadOnlyList<NivelGravedad>>
    {
    }
}
