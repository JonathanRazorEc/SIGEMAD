import { Injectable, signal } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { firstValueFrom } from 'rxjs';
import { environment } from '../../environments/environment';
import { ActionsRelevantService } from './actions-relevant.service';
import { MasterDataEvolutionsService } from './master-data-evolutions.service';

export interface ActivacionPlan {
  id?: number;
  idTipoPlan: number;
  idPlanEmergencia: number;
  tipoPlan?: { id: number; descripcion: string }; // Objeto completo que viene del endpoint
  planEmergencia?: { id: number; descripcion: string }; // Objeto completo que viene del endpoint
  fechaInicio?: string;
  fechaFin?: string | null;
  fechaHoraInicio?: string;
  fechaHoraFin?: string | null;
  autoridad: string;
  observaciones?: string;
  file?: File | null;
  archivo?: any; // Para archivos existentes
  fileAction?: 'none' | 'new' | 'keep_existing'; // Para manejar el estado del archivo
}

export interface SpecialPlansData {
  activacionPlanes: ActivacionPlan[];
}

@Injectable({
  providedIn: 'root'
})
export class SpecialPlansActivationService {
  private apiUrl = environment.urlBase;
  
  // Signal para datos de activación de planes
  public dataActivacionPlanes = signal<SpecialPlansData>({
    activacionPlanes: []
  });

  constructor(private http: HttpClient, private actionsRelevantService: ActionsRelevantService, private masterDataEvolutionsService: MasterDataEvolutionsService) {}

  /**
   * Obtiene los tipos de plan desde el endpoint real
   */
  async getTiposPlanes(idTipoSuceso?: number): Promise<any[]> {
    try {
      return await this.actionsRelevantService.getAllPlanes(idTipoSuceso);
    } catch (error) {
      console.error('Error al cargar tipos de planes:', error);
      // Fallback a datos dummy en caso de error
      return [
        { id: 1, descripcion: 'Plan Regional' },
        { id: 2, descripcion: 'Plan Nacional' },
        { id: 3, descripcion: 'Plan Sectorial' },
        { id: 4, descripcion: 'Plan Especial' }
      ];
    }
  }

  /**
   * Obtiene los planes de emergencia por tipo de plan
   */
  async getPlanesEmergenciaPorTipo(idTipoPlan: number, idCcaa?: number,idTipoSuceso?: number, IsFullDescription: boolean = false): Promise<any[]> {
    try {
      return await this.masterDataEvolutionsService.getTypesPlansByPlan(idTipoPlan, idCcaa,idTipoSuceso,IsFullDescription);
    } catch (error) {
      console.error('Error al cargar planes de emergencia por tipo:', error);
      // Fallback a array vacío en caso de error
      return [];
    }
  }

  /**
   * Obtiene los planes de emergencia (datos dummy)
   */
  async getPlanesEmergencia(): Promise<any[]> {
    // Datos dummy para planes de emergencia
    return Promise.resolve([
      { id: 1, descripcion: 'Plan de Emergencia Regional' },
      { id: 2, descripcion: 'Plan de Emergencia Nacional' },
      { id: 3, descripcion: 'Plan de Emergencia Sectorial' },
      { id: 4, descripcion: 'Plan de Emergencia Especial' }
    ]);
  }

  /**
   * Envía los datos de activación de planes al servidor
   */
  async postActivacionPlanes(
    data: SpecialPlansData, 
    idSuceso: number, 
    idRegistroActualizacion: number
  ): Promise<any> {
    try {
      console.log('📤 Enviando datos de activación de planes:', data);

      // Crear FormData para envío
      const formData = new FormData();
      
      // Agregar datos básicos
      formData.append('IdSuceso', idSuceso.toString());
      formData.append('IdRegistroActualizacion', idRegistroActualizacion.toString());

      // Agregar cada plan de activación
      data.activacionPlanes.forEach((plan, index) => {
        const prefix = `ActivacionPlanes[${index}]`;
        
        formData.append(`${prefix}.IdTipoPlan`, plan.idTipoPlan.toString());
        formData.append(`${prefix}.IdPlanEmergencia`, plan.idPlanEmergencia.toString());
        formData.append(`${prefix}.FechaHoraInicio`, plan.fechaHoraInicio || plan.fechaInicio || '');
        if (plan.fechaHoraFin || plan.fechaFin) {
          formData.append(`${prefix}.FechaHoraFin`, plan.fechaHoraFin || plan.fechaFin || '');
        }
        formData.append(`${prefix}.Autoridad`, plan.autoridad);
        if (plan.observaciones) {
          formData.append(`${prefix}.Observaciones`, plan.observaciones);
        }
        
        // Manejar archivo según el estado
        if (plan.fileAction === 'new' && plan.file) {
          // Caso 2: Archivo seleccionado (enviar binario)
          formData.append(`${prefix}.Archivo`, plan.file, plan.file.name);
          console.log(`📎 Archivo nuevo agregado para plan ${index}:`, plan.file.name);
        } else if (plan.fileAction === 'keep_existing') {
          // Caso 3: Archivo existente sin modificar (enviar valor vacío)
          formData.append(`${prefix}.Archivo`, '');
          console.log(`📎 Mantener archivo existente para plan ${index}`);
        } else {
          // Caso 1: No archivo seleccionado (enviar null)
          formData.append(`${prefix}.Archivo`, '');
          console.log(`📎 Sin archivo para plan ${index}`);
        }

        // Agregar ID si existe (para actualizaciones)
        if (plan.id) {
          formData.append(`${prefix}.Id`, plan.id.toString());
        } else {
          formData.append(`${prefix}.Id`, '0');
        }
      });

      // Log del FormData para depuración
      console.log('📋 FormData creado para activación de planes');

      // Enviar al endpoint (ajustar la URL según sea necesario)
      const response = await firstValueFrom(
        this.http.post(`${this.apiUrl}/registros/activaciones-planes`, formData)
      );

      console.log('✅ Respuesta del servidor para activación de planes:', response);
      return response;

    } catch (error) {
      console.error('❌ Error al enviar datos de activación de planes:', error);
      throw error;
    }
  }

  /**
   * Limpia todos los datos del servicio
   */
  clearData(): void {
    this.dataActivacionPlanes.set({
      activacionPlanes: []
    });
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
} 