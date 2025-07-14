import { GeoPosition } from './geo-position.type';

export type Countries = {
  id: number;
  descripcion: string;
  x: number;
  y: number;
  geoPosicion: GeoPosition;
};
