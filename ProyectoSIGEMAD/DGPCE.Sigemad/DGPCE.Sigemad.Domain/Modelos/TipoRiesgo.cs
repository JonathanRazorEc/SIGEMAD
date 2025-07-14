using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DGPCE.Sigemad.Domain.Modelos;
public class TipoRiesgo
{
    public int Id { get; set; }

    public string Descripcion { get; set; } = null!;

    public int? IdTipoSuceso { get; set; }

    public TipoSuceso TipoSuceso { get; set; } = null!;
}
