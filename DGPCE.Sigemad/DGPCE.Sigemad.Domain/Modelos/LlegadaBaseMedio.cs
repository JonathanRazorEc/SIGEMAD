﻿using DGPCE.Sigemad.Domain.Common;

namespace DGPCE.Sigemad.Domain.Modelos;
public class LlegadaBaseMedio : BaseDomainModel<int>
{
    public EjecucionPaso EjecucionPaso { get; set; }
    public int IdEjecucionPaso { get; set; }


    public Capacidad Capacidad { get; set; }
    public int IdCapacidad { get; set; }

    public string? MedioNoCatalogado { get; set; }
    public DateTime? FechaHoraLlegada { get; set; }
    public string? Observaciones { get; set; }
}
