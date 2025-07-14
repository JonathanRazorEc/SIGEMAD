using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DGPCE.Sigemad.Application.Specifications.MunicipiosExtranjeros;
public class MunicipiosExtranjerosBySearchSpecificationParams
{
    public int? IdDistrito { get; set; }

    public int? IdPais { get; set; }
    public string? busqueda { get; set; }
}
