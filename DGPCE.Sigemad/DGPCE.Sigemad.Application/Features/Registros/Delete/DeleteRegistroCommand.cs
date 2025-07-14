using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DGPCE.Sigemad.Application.Features.Registros.DeleteRegistros;
public class DeleteRegistroCommand : IRequest
{
    public int IdRegistroActualizacion { get; set; }
}
