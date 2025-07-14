using DGPCE.Sigemad.Domain.Modelos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DGPCE.Sigemad.Application.Features.PlanesSituaciones.Vms;
public class PlanSituacionConFaseVm
{
    public int Id { get; set; }
    public string? Descripcion { get; set; }
    public virtual FaseEmergencia? FaseEmergencia { get; set; }
    public string? NivelSituacion { get; set; }
    public string? SituacionEquivalente { get; set; }

}
