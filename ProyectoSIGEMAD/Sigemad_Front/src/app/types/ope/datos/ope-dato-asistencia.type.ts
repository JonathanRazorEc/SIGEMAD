import { OpePuerto } from '../administracion/ope-puerto.type';
import { OpeDatoAsistenciaSanitaria } from './ope-dato-asistencia-sanitaria.type';
import { OpeDatoAsistenciaSocial } from './ope-dato-asistencia-social.type';
import { OpeDatoAsistenciaTraduccion } from './ope-dato-asistencia-traduccion.type';

export type OpeDatoAsistencia = {
  id: number;
  idOpePuerto: number;
  opePuerto: OpePuerto;
  fecha: string;
  opeDatosAsistenciasSanitarias: OpeDatoAsistenciaSanitaria[];
  opeDatosAsistenciasSociales: OpeDatoAsistenciaSocial[];
  opeDatosAsistenciasTraducciones: OpeDatoAsistenciaTraduccion[];

  // Campos de auditoría
  creadoPor: string;
  fechaCreacion: string;
  modificadoPor: string;
  fechaModificacion: string;
};
