
using DGPCE.Sigemad.Domain.Modelos;

namespace DGPCE.Sigemad.Application.Features.Distritos.Vms;
public class DistritoVm
{
    public int Id { get; set; }
    public string Descripcion { get; set; } = null!;
    public string? CodigoOficial { get; set; }

    public Pais Pais { get; set; } = null!;
}
