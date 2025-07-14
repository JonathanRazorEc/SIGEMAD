import { Countries } from '@type/country.type';
import { OpeAsistenciaSocialEdad } from './ope-asistencia-social-edad.type';
import { OpeAsistenciaSocialNacionalidad } from './ope-asistencia-social-nacionalidad.type';
import { OpeAsistenciaSocialSexo } from './ope-asistencia-social-sexo.type';

export type OpeDatoAsistenciaSocialUsuario = {
  id: number;
  idOpeDatoAsistenciaSocial: number;
  idOpeAsistenciaSocialEdad: number;
  opeAsistenciaSocialEdad: OpeAsistenciaSocialEdad;

  idOpeAsistenciaSocialSexo: number;
  opeAsistenciaSocialSexo: OpeAsistenciaSocialSexo;

  idOpeAsistenciaSocialNacionalidad: number;
  opeAsistenciaSocialNacionalidad: OpeAsistenciaSocialNacionalidad;

  idPaisResidencia: number;
  paisResidencia: Countries;

  numero: number;
  observaciones: string;
};
