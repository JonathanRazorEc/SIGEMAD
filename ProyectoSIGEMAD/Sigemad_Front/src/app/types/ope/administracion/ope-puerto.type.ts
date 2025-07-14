import { GeoPosition } from '@type/geo-position.type';
import { OpeFase } from './ope-fase.type';

export type OpePuerto = {
  id: number;
  nombre: string;
  idOpeFase: number;
  opeFase: OpeFase;
  idPais: number;
  idCcaa: number;
  idProvincia: number;
  idMunicipio: number;
  coordenadaUTM_X: number;
  coordenadaUTM_Y: number;
  fechaValidezDesde: string;
  fechaValidezHasta: string;
  capacidad: number;

  // Campos de auditoría
  creadoPor: string;
  fechaCreacion: string;
  modificadoPor: string;
  fechaModificacion: string;
};
