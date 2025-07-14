export interface EmergenciaNacional {
  idSuceso: number;
  idRegistroActualizacion: any;
  emergenciaNacional: Declaracion;
}

export interface Declaracion {
  autoridad: string;
  descripcionSolicitud: string;
  fechaHoraSolicitud: string;
  fechaHoraDeclaracion: string;
  descripcionDeclaracion: string;
  fechaHoraDireccion: string;
  observaciones: string;
}

export interface Zagep {
  id: number;
  fechaSolicitud: string;
  denominacion: string;
  observaciones: string;
}

export interface Cecod {
  id: number;
  fechaInicio: string;
  fechaFin: string;
  lugar: string;
  convocados: string;
  participantes: string;
  observaciones: string;
}

export interface Notificaciones {
  id: number;
  idTipoNotificacion: GenericMaster;
  fechaHoraNotificacion: string;
  organosNotificados: string;
  ucpm: string;
  organismoInternacional: string;
  otrosPaises: string;
  observaciones: string;
}

export interface Planes {
  idRegistroActualizacion: any;
  id: number;
  idTipoPlan: GenericMaster;
  nombrePlan: string;
  nombrePlanPersonalizado: string;
  fechaInicio: string;
  fechaFin: string;
  autoridad: string;
  observaciones: string;
  file?: any;
}

export interface ActivacionSistemas {
  id: number;
  idTipoSistemaEmergencia: GenericMaster;
  fechaHoraActivacion: string;
  fechaHoraActualizacion: string;
  autoridad: string;
  descripcionSolicitud: string;
  observaciones: string;
  idModoActivacion: GenericMaster;
  fechaActivacion: string;
  codigo: string;
  nombre: string;
  urlAcceso: string;
  fechaHoraPeticion: string;
  fechaAceptacion: string;
  peticiones: string;
  mediosCapacidades: string;
}

export interface GenericMaster {
  id: number;
  descripcion: string;
  nombre: string;
}

export interface TipoCapacidad {
  id: number;
  nombre: string;
}

export interface Capacidad {
  id: number;
  nombre: string;
  tipoCapacidad: TipoCapacidad;
}

