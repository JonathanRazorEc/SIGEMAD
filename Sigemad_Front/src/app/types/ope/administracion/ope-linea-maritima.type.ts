import { OpeFase } from './ope-fase.type';
import { OpePuerto } from './ope-puerto.type';

export type OpeLineaMaritima = {
  id: number;
  nombre: string;
  idOpePuertoOrigen: number;
  opePuertoOrigen: OpePuerto;
  idOpePuertoDestino: number;
  opePuertoDestino: OpePuerto;
  idOpeFase: number;
  opeFase: OpeFase;
  fechaValidezDesde: string;
  fechaValidezHasta: string;
  numeroRotaciones: number;
  numeroPasajeros: number;
  numeroTurismos: number;
  numeroAutocares: number;
  numeroCamiones: number;
  numeroTotalVehiculos: number;

  // Campos de auditoría
  creadoPor: string;
  fechaCreacion: string;
  modificadoPor: string;
  fechaModificacion: string;
};
