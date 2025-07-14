import { Countries } from '@type/country.type';

export type OpePais = {
  id: number;
  idPais: number;
  pais: Countries;
  extranjero: boolean;
  opePuertos: boolean;
  rutaImagen: string;
  opeDatosAsistencias: boolean;
};
