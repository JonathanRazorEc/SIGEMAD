namespace DGPCE.Sigemad.Domain.Modelos.Ope.Administracion;

public class OpeEstadoOcupacion
{   
    /*
    public OpePeriodoTipo()
    {
    }
    */
    public int Id { get; set; }
    public string Nombre { get; set; } = null!;
    public int PorcentajeInferior { get; set; }
    public int PorcentajeSuperior { get; set; }
    public bool Borrado { get; set; }
}
