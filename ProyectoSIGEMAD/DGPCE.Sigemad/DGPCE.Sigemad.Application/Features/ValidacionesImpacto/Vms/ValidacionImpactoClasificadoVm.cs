namespace DGPCE.Sigemad.Application.Features.ValidacionesImpacto.Vms;
public class ValidacionImpactoClasificadoVm
{
    public int Id { get; set; }
    public int IdImpactoClasificado { get; set; }
    public string Campo { get; set; }
    public string TipoCampo { get; set; }
    public bool EsObligatorio { get; set; }
    public string Etiqueta { get; set; } = string.Empty;
    public int Orden { get; set; } 
    //public List<OptionVm> Options { get; set; } = new List<OptionVm>();
}

//public class OptionVm
//{
//    public int Id { get; set; }
//    public string Description { get; set; } = string.Empty;
//}