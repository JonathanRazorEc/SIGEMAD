export type FireDetail = {
  fechaHora: string | Date;
  id: number | string;
  origen: string;
  registro: string;
  tecnico: string;
  tipoRegistro: {
    id: number | string;
    nombre: string;
  };
  esUltimoRegistro: boolean;
};

export interface FireDetailResponse {
  count: number;
  page: number;
  pageSize: number;
  data: FireDetail[];
  pageCount: number;
}