using DGPCE.Sigemad.Domain.Common;

namespace DGPCE.Sigemad.Domain.Modelos;

public class Archivo: BaseDomainModel<Guid>
{
    public string NombreOriginal { get; set; }
    public string RutaDeAlmacenamiento { get; set; }
    public string NombreUnico { get; set; }
    public string Tipo { get; set; }
    public string Extension { get; set; }
    public long PesoEnBytes { get; set; }
}
