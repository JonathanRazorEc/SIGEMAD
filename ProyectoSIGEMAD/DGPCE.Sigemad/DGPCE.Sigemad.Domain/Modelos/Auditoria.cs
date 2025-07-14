using DGPCE.Sigemad.Domain.Common;


namespace DGPCE.Sigemad.Domain.Modelos;
public class Auditoria
{
    public Auditoria(string nombreTabla, string operacion, string valoresAntiguos = "", string valoresNuevos = "", Guid? autor = null)
    {
        NombreTabla = nombreTabla;
        Operacion = operacion;
        ValoresAntiguos = valoresAntiguos;
        ValoresNuevos = valoresNuevos;
        Autor = autor;
        Fecha = DateTime.UtcNow; // Se inicializa en el constructor
    }
    public int Id { get; set; }
    public string NombreTabla { get; set; } = string.Empty;
    public string Operacion { get; set; } = string.Empty;
    public string ValoresAntiguos { get; set; } = string.Empty;
    public string ValoresNuevos { get; set; } = string.Empty;
    public Guid? Autor { get; set; }
    public DateTime Fecha { get; set; } = DateTime.UtcNow;
}