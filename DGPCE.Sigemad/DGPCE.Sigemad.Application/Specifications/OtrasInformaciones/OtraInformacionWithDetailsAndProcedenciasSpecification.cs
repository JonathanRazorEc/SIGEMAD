using DGPCE.Sigemad.Domain.Modelos;

namespace DGPCE.Sigemad.Application.Specifications.OtrasInformaciones;
public class OtraInformacionWithDetailsAndProcedenciasSpecification: BaseSpecification<OtraInformacion>
{
    public OtraInformacionWithDetailsAndProcedenciasSpecification(int id, List<int> idsOtrasInformaciones)
    : base(oi => oi.Id == id && !oi.Borrado) // Filtro principal
    {
        if (idsOtrasInformaciones.Any())
        {
            // Relación principal: Detalles
            AddInclude(d => d.DetallesOtraInformacion.Where(dir => idsOtrasInformaciones.Contains(dir.Id) && !dir.Borrado));
            AddInclude("DetallesOtraInformacion.Medio");
            AddInclude("DetallesOtraInformacion.ProcedenciasDestinos");
            AddInclude("DetallesOtraInformacion.ProcedenciasDestinos.ProcedenciaDestino");
        }   
    }
}

