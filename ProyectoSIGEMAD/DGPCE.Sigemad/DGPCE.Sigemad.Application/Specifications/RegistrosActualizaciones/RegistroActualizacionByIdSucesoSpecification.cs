using DGPCE.Sigemad.Domain.Modelos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DGPCE.Sigemad.Application.Specifications.RegistrosActualizaciones
{
    public class RegistroActualizacionByIdSucesoSpecification : BaseSpecification<RegistroActualizacion>
    {
        public RegistroActualizacionByIdSucesoSpecification(int idSuceso, int? tipoRegistro = null)
            : base(x => x.IdSuceso == idSuceso && (!tipoRegistro.HasValue || x.IdTipoRegistroActualizacion == tipoRegistro))
        {
            AddInclude(x => x.Suceso);
            AddOrderByDescending(x => x.FechaCreacion);
        }
    }

}
