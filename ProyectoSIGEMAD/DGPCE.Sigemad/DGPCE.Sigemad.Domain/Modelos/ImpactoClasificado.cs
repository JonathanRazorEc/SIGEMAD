using DGPCE.Sigemad.Domain.Common;

namespace DGPCE.Sigemad.Domain.Modelos
{
    public class ImpactoClasificado : EditableCatalogModel
    {
        public ImpactoClasificado(){}

        public int Id { get; set; }
        public int IdTipoImpacto { get; set; }
        public int IdCategoriaImpacto { get; set; }
        public string Descripcion { get; set; }
        public int IdClaseImpacto { get; set; }
        public bool RelevanciaGeneral { get; set; }
        public bool Nuclear { get; set; }
        public int? IdTipoActuacion { get; set; }
        public int? ValorAD { get; set; }

        public virtual TipoImpacto TipoImpacto { get; set; }
        public virtual CategoriaImpacto CategoriaImpacto { get; set; }
        public virtual ClaseImpacto ClaseImpacto { get; set; }
        public virtual TipoActuacion? TipoActuacion { get; set; }

    }
}
