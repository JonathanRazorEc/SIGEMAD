using Azure.Core;
using DGPCE.Sigemad.Domain.Modelos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DGPCE.Sigemad.Application.Specifications.Sucesos;
public class SucesoWithRegistrosSpecifications : BaseSpecification<Suceso>
{
    public SucesoWithRegistrosSpecifications(int idSuceso)
     : base(suceso =>
     (suceso.Id == idSuceso)  && (suceso.Borrado == false))
    {
        AddInclude(s => s.Registros);
    }

}
