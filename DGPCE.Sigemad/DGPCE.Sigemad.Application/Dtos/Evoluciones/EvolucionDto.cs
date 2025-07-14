using DGPCE.Sigemad.Application.Dtos.AreasAfectadas;
using DGPCE.Sigemad.Application.Dtos.Common;
using DGPCE.Sigemad.Application.Dtos.Impactos;
using DGPCE.Sigemad.Application.Dtos.IntervencionMedios;
using DGPCE.Sigemad.Application.Dtos.Parametros.Dtos;

namespace DGPCE.Sigemad.Application.Dtos.Evoluciones;
public class EvolucionDto: BaseDto<int>
{
    public int IdSuceso { get; set; }

    public DatoPrincipalEvolucionDto? DatoPrincipal { get; set; }
    //public ParametroDto? Parametro { get; set; }
   // public List<AreaAfectadaDto> AreaAfectadas { get; set; } = new();
    //public List<ImpactoEvolucionDto> Impactos { get; set; } = new();
    public List<IntervencionMedioDto> IntervencionMedios { get; set; } = new();
}

