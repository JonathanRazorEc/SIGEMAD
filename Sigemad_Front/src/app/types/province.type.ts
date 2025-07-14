import { AutonomousCommunity } from './autonomous-community.type';
import { GeoPosition } from './geo-position.type';

export type Province = {
  id: number;
  idCcaa: number;
  descripcion: string;
  utmX: number;
  utmY: number;
  huso: number;
  geoPosicion: GeoPosition;
  AutonomousCommunity :AutonomousCommunity
};
