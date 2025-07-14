using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DGPCE.Sigemad.Application.Features.ActuacionesRelevantes.Commands.DeleteActuacionRelevante;
public class DeleteActuacionRelevanteCommand : IRequest
{
    public int Id { get; set; }
}
