
using DGPCE.Sigemad.Domain.Common;

namespace DGPCE.Sigemad.Domain.Modelos
{
    public class EstadoIncendio : EditableCatalogModel
    {

        public EstadoIncendio() { }

        public int Id { get; set; }

        public string? Descripcion { get; set; }
    }
}
