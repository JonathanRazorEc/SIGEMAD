using MediatR;

namespace DGPCE.Sigemad.Application.Features.OtrasInformaciones.Commands.DeleteOtraInformacion;
public class DeleteOtraInformationQuery : IRequest
{
    public int Id { get; set; }
    public DeleteOtraInformationQuery(int id)
    {
        Id = id;
    }
}
