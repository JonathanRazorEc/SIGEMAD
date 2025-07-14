import { AutonomousCommunity } from '@type/autonomous-community.type';
import { Municipality } from '@type/municipality.type';
import { Province } from '@type/province.type';

export type OpeFrontera = {
  id: number;
  nombre: string;
  idCcaa: number;
  CCAA: AutonomousCommunity;
  idProvincia: number;
  provincia: Province;
  idMunicipio: number;
  municipio: Municipality;
  carretera: string;
  PK: number;
  coordenadaUTM_X: number;
  coordenadaUTM_Y: number;
  transitoMedioVehiculos: number;
  transitoAltoVehiculos: number;

  // Campos de auditoría
  creadoPor: string;
  fechaCreacion: string;
  modificadoPor: string;
  fechaModificacion: string;
};
