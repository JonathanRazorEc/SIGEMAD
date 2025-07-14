using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DGPCE.Sigemad.Domain.Modelos
{
    public class EntradaSalida
    {
        public EntradaSalida() { }
        public int Id { get; set; }

        public string Descripcion { get; set; } = null!;
    }
}
