import { HttpClient } from '@angular/common/http';
import { Injectable, inject, signal } from '@angular/core';
import { catchError, firstValueFrom, map, throwError } from 'rxjs';

import { AffectedArea } from '../types/affected-area.type';
import { InterventionUpdate } from '../types/intervention-update.type';
import { EvolucionIncendio, ParametroRecord } from '../types/evolution-record.type';
import { Evolution } from '../types/evolution.type';

@Injectable({ providedIn: 'root' })
export class EvolutionService {
  private http = inject(HttpClient);
  public dataRecords = signal<EvolucionIncendio>({
    idSuceso: 0,
    idRegistroActualizacion: 0,
    parametro: []
  });
  public dataAffectedArea = signal<AffectedArea[]>([]);
  public dataIntervention = signal<InterventionUpdate | null>(null);
  public dataConse = signal<any[]>([]);
  public dataConseFormatted: any = null;

  get(fire_id: any) {
    const endpoint = `/evoluciones?idSuceso=${fire_id}`;

    return firstValueFrom(this.http.get<Evolution[]>(endpoint).pipe((response) => response));
  }

  post(data: any) {
    const endpoint = `/Evoluciones`;

    const body = {
      idTecnico: '550E683E-0458-43E8-A6E6-20887DC2BDDD',
      idIncendio: data.fire_id,
      fechaHoraEvolucion: data.startDateTime,
      idEntradaSalida: data.inputOutput,
      idMedio: data.media,
      idTipoRegistro: 1,
      idEntidadMenor: data.areasAffected?.[0]?.minorEntity,
      resumen: true, //?
      observaciones: data.observations_1,
      idEstadoIncendio: data.status,
      superficieAfectadaHectarea: data.affectedSurface,
      fechaFinal: data.end_date,
      idProvinciaAfectada: data.areasAffected?.[0]?.province_1,
      idMunicipioAfectado: data.areasAffected?.[0]?.municipality_1,
      geoPosicionAreaAfectada: data.areasAffected?.[0]?.geoPosicion || {},
      evolucionProcedenciaDestinos: data.originDestination,
    };

    return firstValueFrom(
      this.http.post(endpoint, body).pipe(
        map((response) => {
          return response;
        }),
        catchError((error) => {
          return throwError(error.error);
        })
      )
    );
  }

  update(body: any) {
    const endpoint = `/Evoluciones`;

    return firstValueFrom(
      this.http.put(endpoint, body).pipe(
        map((response) => {
          return response;
        }),
        catchError((error) => {
          return throwError(error.error);
        })
      )
    );
  }

  clearData(): void {
    this.dataRecords.set({
      idSuceso: 0,
      idRegistroActualizacion: 0,
      parametro: []
    });
    this.dataAffectedArea.set([]);
    this.dataIntervention.set(null);
    this.dataConse.set([]);
    this.dataConseFormatted = null;
  }

  postData(body: any) {
    const endpoint = `/registros/Parametros`;

    return firstValueFrom(
      this.http.post(endpoint, body).pipe(
        map((response) => {
          return response;
        }),
        catchError((error) => {
          return throwError(error.error);
        })
      )
    );
  }

  postAreas(body: any) {
    const endpoint = `/registros/areas-afectadas`;

    return firstValueFrom(
      this.http.post(endpoint, body).pipe(
        map((response) => {
          return response;
        }),
        catchError((error) => {
          return throwError(error.error);
        })
      )
    );
  }

  postIntervencion(body: any) {
    const endpoint = `/registros/intervenciones`;

    return firstValueFrom(
      this.http.post(endpoint, body).pipe(
        map((response) => {
          return response;
        }),
        catchError((error) => {
          return throwError(error.error);
        })
      )
    );
  }

  postConse(body?: any) {
    const endpoint = `/registros/impactos`;
    
    // Si no se proporciona un body, utilizar dataConseFormatted
    const dataToSend = body || this.dataConseFormatted;
    
    if (!dataToSend) {
      return Promise.reject('No hay datos para enviar');
    }

    return firstValueFrom(
      this.http.post(endpoint, dataToSend).pipe(
        map((response) => {
          return response;
        }),
        catchError((error) => {
          return throwError(error.error);
        })
      )
    );
  }

  getById(id: Number) {
    let endpoint = `/evoluciones?idSuceso=${id}`;
    return firstValueFrom(this.http.get<any[]>(endpoint).pipe((response) => response));
  }

  getByIdRegistro(id: Number, registro: Number) {
    let endpoint = `/evoluciones?idSuceso=${id}&idRegistroActualizacion=${registro}`;
    return firstValueFrom(this.http.get<any[]>(endpoint).pipe((response) => response));
  }

  delete(id: number) {
    const endpoint = `/Evoluciones/${id}`;

    return firstValueFrom(this.http.delete(endpoint).pipe((response) => response));
  }

  deleteConse(id: number) {
    const endpoint = `/registros/${id}`;
    return firstValueFrom(this.http.delete(endpoint).pipe((response) => response));
  }


  getCaracterMedios() {
    const endpoint = `/caracter-medios`;
    return firstValueFrom(this.http.get<any[]>(endpoint).pipe((response) => response));
  }

  getTipoIntervencionMedios() {
    const endpoint = `/movilizaciones-medios/capacidades`;
    return firstValueFrom(this.http.get<any[]>(endpoint).pipe((response) => response));
  }

  getTitularMedios() {
    const endpoint = `/titular-medios`;
    return firstValueFrom(this.http.get<any[]>(endpoint).pipe((response) => response));
  }

  getMediosCapacidades(tipoIntervencionId: number) {
    const endpoint = `/Medios-capacidades/${tipoIntervencionId}`;
    return firstValueFrom(
      this.http.get<any[]>(endpoint).pipe(
        map((response) => response || []),
        catchError((error) => {
          console.error('Error al obtener medios-capacidades:', error);
          return Promise.resolve([]);
        })
      )
    );
  }
}
