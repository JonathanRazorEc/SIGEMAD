using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DGPCE.Sigemad.Application.Specifications.Municipios;
public class MunicipiosSpecificationParams
{
    public int? Id { get; set; }
    public int? IdProvincia { get; set; }

    public string? busqueda { get; set; }
}
