using DGPCE.Sigemad.Domain.Common;
using DGPCE.Sigemad.Domain.Modelos.Ope.Administracion;
using DGPCE.Sigemad.Domain.Modelos.Ope.Datos;

namespace DGPCE.Sigemad.Application.Features.Ope.Datos.OpeDatosFronteras.Vms
{
    public class OpeDatoFronteraVm : BaseDomainModel<int>
    {
        public int IdOpeFrontera { get; set; }
        public DateTime Fecha { get; set; }
        public int IdOpeDatoFronteraIntervaloHorario { get; set; }
        public bool IntervaloHorarioPersonalizado { get; set; }
        public TimeSpan? InicioIntervaloHorarioPersonalizado { get; set; }
        public TimeSpan? FinIntervaloHorarioPersonalizado { get; set; }
        public int NumeroVehiculos { get; set; }
        public string Afluencia { get; set; } = null!;

        public virtual OpeFrontera OpeFrontera { get; set; } = null!;
        public virtual OpeDatoFronteraIntervaloHorario OpeDatoFronteraIntervaloHorario { get; set; } = null!;
    }
}
