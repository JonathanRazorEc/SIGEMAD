using DGPCE.Sigemad.Domain;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DGPCE.Sigemad.Application.Features.TiposImpactos.Queries;
public class GetTipoImpactoListQuery : IRequest<IReadOnlyList<TipoImpacto>>
{
    public bool nuclear { get; set; }
    public GetTipoImpactoListQuery(bool nuclear)
    {
        this.nuclear = nuclear;
    }
}
