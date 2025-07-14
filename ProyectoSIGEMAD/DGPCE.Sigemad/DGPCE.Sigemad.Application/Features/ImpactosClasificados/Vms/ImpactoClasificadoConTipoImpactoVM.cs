using DGPCE.Sigemad.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DGPCE.Sigemad.Application.Features.ImpactosClasificados.Vms;
public class ImpactoClasificadoConTipoImpactoVM
{
    public int Id { get; set; }
    public string Descripcion { get; set; }

    public virtual TipoImpactoVm TipoImpacto { get; set; }
}
