
using DGPCE.Sigemad.Domain.Common;

namespace DGPCE.Sigemad.Domain.Modelos
{
    public class Distrito : BaseDomainModel<int>
    {      
        public int IdPais { get; set; }
        public string Descripcion { get; set; }  = null!;
        public string? CodigoOficial { get; set; }
        public virtual Pais Pais { get; set; } = null!;


    }
}
