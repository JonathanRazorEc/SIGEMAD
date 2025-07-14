using DGPCE.Sigemad.Application.Constants;
using MediatR;

namespace DGPCE.Sigemad.Application.Features.Archivos.Commands.CreateFile;
public class CreateFileCommand : IRequest<CreateFileResponse>
{
    public string NombreOriginal { get; set; }
    public string Extension { get; set; }
    public string Tipo { get; set; }
    public long PesoEnBytes { get; set; }
    public Stream Archivo { get; set; }
    public ContextFile Context { get; set; }
}
