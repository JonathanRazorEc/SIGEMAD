export interface User {
  userId?: number;
  nombre: string;
  apellidos: string;
  email: string;
  username: string;
  password?: string;
  telefono?: string;
  fechaCreacion?: Date;
  fechaActualizacion?: Date;
  activo?: boolean;
  roles?: string[];
}
