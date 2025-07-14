namespace DGPCE.Sigemad.Application.Dtos.Common;
public class FileDto
{
    public string FileName { get; set; }
    public string ContentType { get; set; }
    public string Extension { get; set; }
    public long Length { get; set; }
    public byte[] Content { get; set; }
}