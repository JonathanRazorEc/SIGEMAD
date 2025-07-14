using DGPCE.Sigemad.Domain.Common;
using DGPCE.Sigemad.Domain.Modelos.Ope.Administracion;

namespace DGPCE.Sigemad.Application.Features.Ope.Datos.OpeLineasMaritimas.Vms
{
    public class OpeLineaMaritimaVm : BaseDomainModel<int>
    {
        public string Nombre { get; set; } = null!;
        public int IdOpePuertoOrigen { get; set; }
        public int IdOpePuertoDestino { get; set; }
        public int IdOpeFase { get; set; }
        public DateTime FechaValidezDesde { get; set; }
        public DateTime? FechaValidezHasta { get; set; }
        public int? NumeroRotaciones { get; set; }
        public int? NumeroPasajeros { get; set; }
        public int? NumeroTurismos { get; set; }
        public int? NumeroAutocares { get; set; }
        public int? NumeroCamiones { get; set; }
        public int? NumeroTotalVehiculos { get; set; }


        public virtual OpePuerto OpePuertoOrigen { get; set; } = null!;
        public virtual OpePuerto OpePuertoDestino { get; set; } = null!;
        public virtual OpeFase OpeFase { get; set; } = null!;
    }
}
