export interface Consequences {
  id: number;
  idImpactoClasificado: number;
  nuclear: boolean;
  valorAD: number;
  numero: number;
  observaciones: string;
  fecha: string;
  fechaHora: string;
  fechaHoraInicio: string;
  fechaHoraFin: string;
  alteracionInterrupcion: string;
  causa: string;
  numeroGraves: number;
  idTipoDanio: { id: number; descripcion: string };
  numeroUsuarios: number;
  numeroIntervinientes: number;
  numeroServicios: number;
  numeroLocalidades: number;
}
