export type Menu = {
  id: number;
  nombre: string;
  icono: string;
  colorRgb: string;
  subItems: Menu[];
  ruta: string;
  isOpen?: boolean;
};
