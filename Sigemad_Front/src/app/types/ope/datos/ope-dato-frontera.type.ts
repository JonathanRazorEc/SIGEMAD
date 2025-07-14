import { OpeFrontera } from '../administracion/ope-frontera.type';
import { OpeDatoFronteraIntervaloHorario } from './ope-dato-frontera-intervalo-horario.type';

export type OpeDatoFrontera = {
  id: number;
  idOpeFrontera: number;
  opeFrontera: OpeFrontera;
  fecha: string;
  idOpeDatoFronteraIntervaloHorario: number;
  opeDatoFronteraIntervaloHorario: OpeDatoFronteraIntervaloHorario;
  intervaloHorarioPersonalizado: boolean;
  inicioIntervaloHorarioPersonalizado: string;
  finIntervaloHorarioPersonalizado: string;
  numeroVehiculos: number;
  afluencia: string;

  // Campos de auditoría
  creadoPor: string;
  fechaCreacion: string;
  modificadoPor: string;
  fechaModificacion: string;
};
