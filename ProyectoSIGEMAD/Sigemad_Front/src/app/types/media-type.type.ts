type clasificacionMedio = {
  id: number;
  descripcion: string;
};
type titularidadMedio = {
  id: number;
  descripcion: string;
};

export type MediaType = {
  id: number;
  descripcion: string;
  clasificacionMedio: clasificacionMedio;
  titularidadMedio: titularidadMedio;
};
