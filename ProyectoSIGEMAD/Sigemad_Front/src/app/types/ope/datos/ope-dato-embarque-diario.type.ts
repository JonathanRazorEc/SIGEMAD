import { OpeLineaMaritima } from '../administracion/ope-linea-maritima.type';

export type OpeDatoEmbarqueDiario = {
  id: number | null; // Para permitir que al recargar la página se pueda crear un nuevo registro
  idOpeLineaMaritima: number;
  opeLineaMaritima: OpeLineaMaritima;
  fecha: string;
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
