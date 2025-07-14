using DGPCE.Sigemad.Domain.Common;

namespace DGPCE.Sigemad.Domain.Modelos.Ope.Datos;
public class OpeDatoAsistenciaSocialUsuario : BaseDomainModel<int>
{
    public int IdOpeDatoAsistenciaSocial { get; set; }
    public int? IdOpeAsistenciaSocialEdad { get; set; }
    public int? IdOpeAsistenciaSocialSexo { get; set; }
    public int? IdOpeAsistenciaSocialNacionalidad { get; set; }
    public int? IdPaisResidencia { get; set; }
    public int Numero { get; set; }
    public string? Observaciones { get; set; }


    public virtual OpeDatoAsistenciaSocial OpeDatoAsistenciaSocial { get; set; } = null!;
    public virtual OpeAsistenciaSocialEdad OpeAsistenciaSocialEdad { get; set; } = null!;
    public virtual OpeAsistenciaSocialSexo OpeAsistenciaSocialSexo { get; set; } = null!;
    public virtual OpeAsistenciaSocialNacionalidad OpeAsistenciaSocialNacionalidad { get; set; } = null!;
    public virtual Pais PaisResidencia { get; set; } = null!;
}
