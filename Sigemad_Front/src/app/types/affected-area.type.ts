export interface AffectedArea {
  id: number;
  fechaHora: string;
  provincia: { id: number; descripcion: string };
  municipio: { id: number; descripcion: string };
  entidadMenor: { id: number; descripcion: string };
  observaciones: string;
  geoPosicion?: any;
  superficieAfectadaHectarea?: number;
}
