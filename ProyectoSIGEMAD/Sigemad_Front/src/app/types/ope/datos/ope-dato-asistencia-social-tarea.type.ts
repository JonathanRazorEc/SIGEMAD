import { OpeAsistenciaSocialTareaTipo } from './ope-asistencia-social-tarea-tipo.type';

export type OpeDatoAsistenciaSocialTarea = {
  id: number;
  idOpeDatoAsistenciaSocial: number;
  idOpeAsistenciaSocialTareaTipo: number;
  opeAsistenciaSocialTareaTipo: OpeAsistenciaSocialTareaTipo;
  numero: number;
  observaciones: string;
};
