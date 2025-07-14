
using DGPCE.Sigemad.Domain.Modelos;

namespace DGPCE.Sigemad.Application.Specifications.OtrasInformaciones;
public class OtraInformacionWithDetalle : BaseSpecification<OtraInformacion>
{
    public OtraInformacionWithDetalle(OtraInformacionParams @params)
    : base(d =>
    (!@params.Id.HasValue || d.Id == @params.Id) &&
    (!@params.IdSuceso.HasValue || d.IdSuceso == @params.IdSuceso) &&
     d.Borrado == false)
    {
        AddInclude(d => d.DetallesOtraInformacion);
        AddInclude("DetallesOtraInformacion.Medio");
        AddInclude("DetallesOtraInformacion.ProcedenciasDestinos");
        AddInclude("DetallesOtraInformacion.ProcedenciasDestinos.ProcedenciaDestino");

    }
}
