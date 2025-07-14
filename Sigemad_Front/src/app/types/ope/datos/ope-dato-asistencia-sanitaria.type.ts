import { OpePuerto } from '../administracion/ope-puerto.type';
import { OpeAsistenciaSanitariaTipo } from './ope-asistencia-sanitaria-tipo.type';

export type OpeDatoAsistenciaSanitaria = {
  id: number;
  idOpeDatoAsistencia: number;
  idOpeAsistenciaSanitariaTipo: number;
  opeAsistenciaSanitariaTipo: OpeAsistenciaSanitariaTipo;
  numero: number;
  observaciones: string;
};
