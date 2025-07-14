import { OpeAsistenciaSocialTipo } from './ope-asistencia-social-tipo.type';
import { OpeDatoAsistenciaSocialOrganismo } from './ope-dato-asistencia-social-organismo.type';
import { OpeDatoAsistenciaSocialTarea } from './ope-dato-asistencia-social-tarea.type';
import { OpeDatoAsistenciaSocialUsuario } from './ope-dato-asistencia-social-usuario.type';

export type OpeDatoAsistenciaSocial = {
  id: number;
  idOpeDatoAsistencia: number;
  idOpeAsistenciaSocialTipo: number;
  opeAsistenciaSocialTipo: OpeAsistenciaSocialTipo;
  numero: number;
  observaciones: string;

  opeDatosAsistenciasSocialesTareasModificado?: boolean;
  opeDatosAsistenciasSocialesOrganismosModificado?: boolean;
  opeDatosAsistenciasSocialesUsuariosModificado?: boolean;

  opeDatosAsistenciasSocialesTareas: OpeDatoAsistenciaSocialTarea[];
  opeDatosAsistenciasSocialesOrganismos: OpeDatoAsistenciaSocialOrganismo[];
  opeDatosAsistenciasSocialesUsuarios: OpeDatoAsistenciaSocialUsuario[];
};
