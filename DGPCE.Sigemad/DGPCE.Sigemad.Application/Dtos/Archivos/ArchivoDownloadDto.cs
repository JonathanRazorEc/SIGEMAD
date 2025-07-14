namespace DGPCE.Sigemad.Application.Dtos.Archivos;
public class ArchivoDownloadDto
{
    public Stream FileStream { get; set; }
    public string ContentType { get; set; }
    public string FileName { get; set; }
}
