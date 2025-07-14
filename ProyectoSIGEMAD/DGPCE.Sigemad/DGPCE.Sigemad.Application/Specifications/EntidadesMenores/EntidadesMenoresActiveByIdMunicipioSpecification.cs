using DGPCE.Sigemad.Domain.Modelos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DGPCE.Sigemad.Application.Specifications.EntidadesMenores;
public class EntidadesMenoresActiveByIdMunicipioSpecification : BaseSpecification<EntidadMenor>
{
    public EntidadesMenoresActiveByIdMunicipioSpecification(int idMunicipio)
        : base(i => i.IdMunicipio == idMunicipio && i.Borrado == false)
    {
        AddOrderBy(i => i.Descripcion); // Ordenación por defecto
    }
}
