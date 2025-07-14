namespace DGPCE.Sigemad.Application.Specifications.RegistrosActualizaciones;
public class RegistroActualizacionSpecificationParams : SpecificationParams
{
    public int? Id { get; set; }
    public int? IdMinimo { get; set; }
    public int? IdSuceso { get; set; }
    public int? IdTipoRegistroActualizacion { get; set; }
}
