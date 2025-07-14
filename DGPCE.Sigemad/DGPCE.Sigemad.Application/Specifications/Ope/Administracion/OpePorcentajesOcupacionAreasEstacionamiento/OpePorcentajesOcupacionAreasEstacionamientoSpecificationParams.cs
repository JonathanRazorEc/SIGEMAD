namespace DGPCE.Sigemad.Application.Specifications.Ope.Administracion.OpePorcentajesOcupacionAreasEstacionamiento;

public class OpePorcentajesOcupacionAreasEstacionamientoSpecificationParams : SpecificationParams
{
    public int? Id { get; set; }
    
    public int? IdOpeOcupacion { get; set; }
   
    public int PorcentajeInferior { get; set; }

    public int PorcentajeSuperior { get; set; }

}