using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DGPCE.Sigemad.Domain.Modelos
{
    public class AspNetRole
    {

        public string Id { get; set; } = default!;
        public string Name { get; set; } = default!;
        // opcionalmente si lo necesitas:
        public string? NormalizedName { get; set; }
        public string? ConcurrencyStamp { get; set; }

    }
}
