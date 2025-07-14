import { Province } from './province.type';

export type AutonomousCommunity = {
  id: number;
  descripcion: string;
  provincia: Province[];
};
