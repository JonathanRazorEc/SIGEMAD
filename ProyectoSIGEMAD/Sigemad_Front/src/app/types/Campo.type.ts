export type Campo = {
  id: number;
  idImpactoClasificado: number;
  campo: string;
  tipoCampo: string;
  esObligatorio: boolean;
  label?: string;
  options?: { id: string; description: string }[];
  initValue?: any;
};
