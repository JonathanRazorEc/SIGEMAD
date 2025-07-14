import { Injectable, inject, signal } from '@angular/core';
import { firstValueFrom } from 'rxjs';
import { HttpClient } from '@angular/common/http';
import { DirectionType } from '../types/direction-type.type';
import { DateUtils } from '@shared/utils/date-utils';

// Definir la interfaz para los datos de gestión
export interface GestionData {
  direcciones: any[];
  coordinacionesCecopi: any[];
  coordinacionesPMA: any[];
}

// Interfaz para tipos de direcciones de emergencias
export interface TipoDireccionEmergencia {
  id: number;
  descripcion: string;
}

@Injectable({
  providedIn: 'root'
})
export class DirectionCoordinationService {
  private http = inject(HttpClient);

  // Signal para almacenar datos de gestión
  public dataGestion = signal<GestionData>({
    direcciones: [],
    coordinacionesCecopi: [],
    coordinacionesPMA: []
  });

  constructor() {}

  getDirectionTypes() {
    const endpoint = '/tipos-sistemas-emergencia';
    return firstValueFrom(this.http.get<DirectionType[]>(endpoint).pipe((response) => response));
  }

  getTiposDireccionesEmergencias() {
    const endpoint = '/tipos-direcciones-emergencias';
    return firstValueFrom(this.http.get<TipoDireccionEmergencia[]>(endpoint).pipe((response) => response));
  }

  /**
   * Envía los datos de dirección y coordinación al endpoint
   */
  async postDireccionCoordinacion(data: GestionData, idSuceso: number, idRegistroActualizacion: number): Promise<any> {
    const formData = new FormData();
    console.log('DATE', data)
    
    // Agregar datos básicos
    formData.append('idRegistroActualizacion', idRegistroActualizacion.toString());
    formData.append('IdSuceso', idSuceso.toString());

    // Procesar direcciones
    data.direcciones.forEach((direccion, index) => {
      formData.append(`Direcciones[${index}].IdTipoDireccionEmergencia`, direccion.tipoDireccion?.toString() || '');
      formData.append(`Direcciones[${index}].AutoridadQueDirige`, direccion.lugar || '');
      formData.append(`Direcciones[${index}].FechaInicio`, this.formatDateForAPI(direccion.fechaHoraInicio));
      formData.append(`Direcciones[${index}].FechaFin`, this.formatDateForAPI(direccion.fechaHoraFin));
      
      // Manejar archivo si existe
      if (direccion.file && direccion.file instanceof File) {
        formData.append(`Direcciones[${index}].archivo`, direccion.file);
      } else {
        formData.append(`Direcciones[${index}].archivo`, '');
      }
    });

    // Procesar coordinaciones CECOPI
    data.coordinacionesCecopi.forEach((cecopi, index) => {
      formData.append(`CoordinacionesCECOPI[${index}].FechaInicio`, this.formatDateForAPI(cecopi.fechaHoraInicio));
      formData.append(`CoordinacionesCECOPI[${index}].FechaFin`, this.formatDateForAPI(cecopi.fechaHoraFin));
      formData.append(`CoordinacionesCECOPI[${index}].IdProvincia`, cecopi.provincia?.id?.toString() || '');
      formData.append(`CoordinacionesCECOPI[${index}].IdMunicipio`, cecopi.municipio?.id?.toString() || '');
      formData.append(`CoordinacionesCECOPI[${index}].Lugar`, cecopi.lugar || '');
      formData.append(`CoordinacionesCECOPI[${index}].Observaciones`, cecopi.observaciones || '');
      formData.append(
          `CoordinacionesCECOPI[${index}].GeoPosicion`,
          cecopi.geoPosicion ? JSON.stringify(cecopi.geoPosicion) : ''
        );

      
      // Manejar archivo si existe
      if (cecopi.file && cecopi.file instanceof File) {
        formData.append(`CoordinacionesCECOPI[${index}].Archivo`, cecopi.file);
      } else {
        formData.append(`CoordinacionesCECOPI[${index}].Archivo`, '');
      }
    });

    // Procesar coordinaciones PMA
    data.coordinacionesPMA.forEach((pma, index) => {
      formData.append(`CoordinacionesPMA[${index}].FechaInicio`, this.formatDateForAPI(pma.fechaHoraInicio));
      formData.append(`CoordinacionesPMA[${index}].FechaFin`, this.formatDateForAPI(pma.fechaHoraFin));
      formData.append(`CoordinacionesPMA[${index}].IdProvincia`, pma.provincia?.id?.toString() || '');
      formData.append(`CoordinacionesPMA[${index}].IdMunicipio`, pma.municipio?.id?.toString() || '');
      formData.append(`CoordinacionesPMA[${index}].Lugar`, pma.lugar || '');
      formData.append(`CoordinacionesPMA[${index}].Observaciones`, pma.observaciones || '');
      formData.append(
        `CoordinacionesPMA[${index}].GeoPosicion`,
        pma.geoPosicion ? JSON.stringify(pma.geoPosicion) : ''
      );

      
      // Manejar archivo si existe
      if (pma.file && pma.file instanceof File) {
        formData.append(`CoordinacionesPMA[${index}].Archivo`, pma.file);
      } else {
        formData.append(`CoordinacionesPMA[${index}].Archivo`, '');
      }
    });

    const endpoint = '/registros/direccion-coordinacion';
    
    return firstValueFrom(
      this.http.post(endpoint, formData).pipe(
        (response) => response
      )
    );
  }

  /**
   * Formatea la fecha para el API (mantiene la fecha y hora exactas sin conversión de zona horaria)
   */
  private formatDateForAPI(date: string): string {
    if (!date) return '';
    
    try {
      // // Si la fecha ya tiene formato YYYY-MM-DDTHH:mm, agregar segundos y Z
      // if (/^\d{4}-\d{2}-\d{2}T\d{2}:\d{2}$/.test(dateString)) {
      //   return `${dateString}:00.000Z`;
      // }
      
      // // Si ya tiene formato completo, devolverla tal como está
      // if (dateString.includes('Z') || dateString.includes('+') || dateString.includes('-', 10)) {
      //   return dateString;
      // }
      
      // // Para otros formatos, usar el método original
      // const date = new Date(dateString);
      // return date.toISOString();
      return date;
    } catch (error) {
      console.error('Error al formatear fecha:', error);
      return date;
    }
  }

  /**
   * Descarga un archivo por su ID
   */
  async getFile(fileId: string): Promise<Blob> {
    const endpoint = `/Archivos/${fileId}/contenido`;
    return firstValueFrom(
      this.http.get(endpoint, { responseType: 'blob' }).pipe(
        (response) => response
      )
    );
  }

  clearData() {
    // Reiniciar los datos de gestión
    this.dataGestion.set({
      direcciones: [],
      coordinacionesCecopi: [],
      coordinacionesPMA: []
    });
  }
} 