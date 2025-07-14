// En Application/Features/Auditoria/Queries/GetAuditoriaIncendio
using DGPCE.Sigemad.Application.Features.Auditoria.Vms;
using MediatR;

namespace DGPCE.Sigemad.Application.Features.Auditoria.Queries.GetAuditoriaIncendio
{
    public class GetAuditoriaIncendioQuery : IRequest<AuditoriaIncendioVm>
    {
        public int IdIncendio { get; set; }

        public GetAuditoriaIncendioQuery(int idIncendio)
        {
            IdIncendio = idIncendio;
        }
    }
}
