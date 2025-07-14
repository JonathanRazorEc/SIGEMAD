using DGPCE.Sigemad.Domain.Modelos;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DGPCE.Sigemad.Application.Features.EstadosIncendio.Queries.GetEstadosIncendioList
{
    public class GetEstadosIncendioListQuery : IRequest<IReadOnlyList<EstadoIncendio>>
    {
    }
}
