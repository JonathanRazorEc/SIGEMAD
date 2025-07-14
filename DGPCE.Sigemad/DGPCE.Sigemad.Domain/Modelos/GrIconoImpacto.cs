using DGPCE.Sigemad.Domain.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DGPCE.Sigemad.Domain.Modelos;
public class GrIconoImpacto : EditableCatalogModel
{
    public int Id { get; set; }
    public string Descripcion { get; set; }
    public string Imagen { get; set; }

    public int IdGrupoImpacto { get; set; }

    public virtual GrupoImpacto GrupoImpacto { get; set; }
}
