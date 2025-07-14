import { GeoPosition } from './geo-position.type';

export type Municipality = {
  id: number;
  descripcion: string;
  utmX: number;
  utmY: number;
  huso: number;
  geoPosicion: GeoPosition;
};
