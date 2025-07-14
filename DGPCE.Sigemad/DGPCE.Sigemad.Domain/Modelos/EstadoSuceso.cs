using DGPCE.Sigemad.Domain.Common;

namespace DGPCE.Sigemad.Domain.Modelos
{
    public class EstadoSuceso : EditableCatalogModel
    {
        public int Id { get; set; }

        public string Descripcion { get; set; } = null!;
    }
}
