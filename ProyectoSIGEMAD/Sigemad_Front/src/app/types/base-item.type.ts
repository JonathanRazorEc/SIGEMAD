/**
 * Tipo base para items con id y descripcion
 * Utilizado para listas desplegables y selecciones
 */
export interface BaseItem {
  id: number;
  descripcion: string;
  nombre: string;
}

/**
 * Tipo base con id y descripcion como string
 * Útil para casos donde el id es un string (ejemplo: GUIDs, códigos, etc.)
 */
export interface StringIdItem {
  id: string;
  descripcion: string;
  nombre: string;
}

/**
 * Tipo base con propiedades opcionales
 * Útil para filtros y objetos parciales
 */
export interface PartialBaseItem {
  id?: number;
  descripcion?: string;
  nombre: string;
} 