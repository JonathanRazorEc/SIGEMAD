import { OpeAsistenciaSocialOrganismoTipo } from './ope-asistencia-social-organismo-tipo.type';

export type OpeDatoAsistenciaSocialOrganismo = {
  id: number;
  idOpeDatoAsistenciaSocial: number;
  idOpeAsistenciaSocialOrganismoTipo: number;
  opeAsistenciaSocialOrganismoTipo: OpeAsistenciaSocialOrganismoTipo;
  numero: number;
  observaciones: string;
};
