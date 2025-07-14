import { OriginDestination } from './origin-destination.type';

export interface EvolucionIncendio {
  idSuceso: number;
  idRegistroActualizacion: number;
  parametro: ParametroRecord[];
}

export interface ParametroRecord {
  id?: number;
  idEstadoIncendio: number;
  fechaFinal: string;
  idPlanEmergencia: number;
  idFaseEmergencia: number;
  idPlanSituacion: number;
  idSituacionEquivalente: number | null;
  fechaHoraActualizacion: string;
  observaciones: string;
  prevision: string;
  idRegistro?: number;
  esModificado?: boolean;
}

// Mantenemos estas interfaces por compatibilidad con código existente
export interface DatoPrincipal {
  fechaHora: string;
  observaciones: string;
  prevision: string;
}

export interface Parametro {
  idEstadoIncendio: number;
  fechaFinal: string;
  idPlanEmergencia: number;
  idFaseEmergencia: number;
  idSituacionEquivalente: number;
  idPlanSituacion: number;
}
