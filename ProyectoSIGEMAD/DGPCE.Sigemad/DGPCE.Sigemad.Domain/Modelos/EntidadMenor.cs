
using DGPCE.Sigemad.Domain.Common;
using NetTopologySuite.Geometries;

namespace DGPCE.Sigemad.Domain.Modelos
{
    public class EntidadMenor : BaseDomainModel<int>
    {
        public int? IdMunicipio { get; set; }
        public string Descripcion { get; set; } = null!;

        public int? UtmX { get; set; }
        public int? UtmY { get; set; }
        public int? Huso { get; set; }

        public Geometry? GeoPosicion { get; set; }

        public virtual Municipio Municipio { get; set; } = null!;
    }
}
