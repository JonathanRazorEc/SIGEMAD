using DGPCE.Sigemad.Domain.Modelos;


namespace DGPCE.Sigemad.Application.Specifications.SucesosRelacionados;

public class SucesoRelacionadoWithFilteredData : BaseSpecification<SucesoRelacionado>
{
    public SucesoRelacionadoWithFilteredData(int id, List<int> idsDetallesSucesoRelacionado)
     : base(d => d.Id == id && d.Borrado == false)
    {
        if (idsDetallesSucesoRelacionado.Any())
        {
            AddInclude(b => b.DetalleSucesoRelacionados.Where(dir => idsDetallesSucesoRelacionado.Contains(dir.IdSucesoAsociado) && !dir.Borrado)); ;
            AddInclude("DetalleSucesoRelacionados.SucesoAsociado");
            AddInclude("DetalleSucesoRelacionados.SucesoAsociado.TipoSuceso");
            AddInclude("DetalleSucesoRelacionados.SucesoAsociado.Incendios.EstadoSuceso");

        }
    }
}