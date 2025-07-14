using DGPCE.Sigemad.Application.Dtos.Evoluciones;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DGPCE.Sigemad.Application.Features.Registros.Queries;
public class GetRegistrosQuery : IRequest<RegistroEvolucionDto>
{
    public int IdRegistroActualizacion { get; set; }

    //public int IdSuceso { get; set; }
}
