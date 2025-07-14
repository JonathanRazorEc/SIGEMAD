import { OpeOcupacion } from './ope-ocupacion.type';

export type OpePorcentajeOcupacionAreaEstacionamiento = {
  id: number;
  idOpeOcupacion: number;
  opeOcupacion: OpeOcupacion;
  porcentajeInferior: number;
  porcentajeSuperior: number;

  // Campos de auditoría
  creadoPor: string;
  fechaCreacion: string;
  modificadoPor: string;
  fechaModificacion: string;
};
