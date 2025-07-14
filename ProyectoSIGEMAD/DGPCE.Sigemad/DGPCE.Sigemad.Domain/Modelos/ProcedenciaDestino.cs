
using DGPCE.Sigemad.Domain.Common;

namespace DGPCE.Sigemad.Domain.Modelos
{
    public class ProcedenciaDestino : EditableCatalogModel
    {
        public ProcedenciaDestino() {}

        public int Id { get; set; }

        public string Descripcion { get; set; }
    }
}
