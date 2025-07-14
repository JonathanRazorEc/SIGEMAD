using DGPCE.Sigemad.Application.Specifications.Documentos;
using DGPCE.Sigemad.Domain.Modelos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DGPCE.Sigemad.Application.Specifications.OtrasInformaciones;
public class GetOtraInformacionWithParams : BaseSpecification<OtraInformacion>
{
    public GetOtraInformacionWithParams(OtraInformacionParams @params)
   : base(d =>
    (!@params.Id.HasValue || d.Id == @params.Id) &&
    (!@params.IdSuceso.HasValue || d.IdSuceso == @params.IdSuceso) &&
     d.Borrado == false)
    {
        AddInclude(i => i.DetallesOtraInformacion.Where(d => !d.Borrado));
        AddInclude("DetallesOtraInformacion.Medio");
        AddInclude("DetallesOtraInformacion.ProcedenciasDestinos");
        AddInclude("DetallesOtraInformacion.ProcedenciasDestinos.ProcedenciaDestino");

    }
}
