export interface InterventionMedium {
  idMediosCapacidad: number;
  numeroIntervinientes: number;
}

export interface Intervention {
  id: number | null;
  idCaracterMedio: number;
  descripcion: string;
  medioNoCatalogado: string;
  numeroCapacidades: number;
  idTitularidadMedio: number;
  titular: string;
  fechaHoraInicio: string;
  fechaHoraFin: string;
  idProvincia: number;
  idMunicipio: number;
  observaciones: string;
  detalleIntervencionMedios: InterventionMedium[];
}

export interface InterventionUpdate {
  IdRegistroActualizacion: number;
  IdSuceso: number;
  Intervenciones: Intervention[];
} 