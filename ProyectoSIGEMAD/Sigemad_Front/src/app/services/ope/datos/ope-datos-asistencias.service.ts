import { DatePipe } from '@angular/common';
import { HttpClient } from '@angular/common/http';
import { Injectable, inject } from '@angular/core';
import { catchError, firstValueFrom, map, Observable, throwError } from 'rxjs';
import { ApiResponse } from '../../../types/api-response.type';
import { OpeDatoAsistencia } from '@type/ope/datos/ope-dato-asistencia.type';

@Injectable({ providedIn: 'root' })
export class OpeDatosAsistenciasService {
  public http = inject(HttpClient);
  public datepipe = inject(DatePipe);
  public endpoint = '/ope-datos-asistencias';

  /*
  generateUrlWitchParams({ url, params }: any) {
    return Object.keys(params).reduce((prev: any, key: any) => {
      if (params[key] === null || params[key] === undefined) return prev;
      return `${prev}&${key}=${params[key]}`;
    }, `${url}`);
  }
  */
  //
  generateUrlWitchParams({ url, params }: any) {
    const queryString = Object.keys(params)
      .filter((key) => params[key] !== null && params[key] !== undefined)
      .map((key) => {
        const value = params[key];
        if (Array.isArray(value)) {
          return value.map((v: any) => `${encodeURIComponent(key)}=${encodeURIComponent(v)}`).join('&');
        } else {
          return `${encodeURIComponent(key)}=${encodeURIComponent(value)}`;
        }
      })
      .join('&');

    return `${url}${queryString ? '&' + queryString : ''}`;
  }
  //

  get(query: any = '') {
    const base = `${this.endpoint}?Sort=desc&PageSize=99999`;
    const finalUrl = this.generateUrlWitchParams({ url: base, params: query });
    return firstValueFrom(this.http.get<ApiResponse<OpeDatoAsistencia[]>>(finalUrl).pipe((res) => res));
  }

  post(data: any) {
    const body = {
      idOpePuerto: data.opePuerto,
      fecha: this.datepipe.transform(data.fecha, 'yyyy-MM-dd HH:mm:ss'),
      //opeDatosAsistenciasTraducciones: data.opeDatosAsistenciasTraducciones,
      opeDatosAsistenciasSanitariasModificado: data.opeDatosAsistenciasSanitariasModificado,
      opeDatosAsistenciasSanitarias: data.opeDatosAsistenciasSanitarias,
      opeDatosAsistenciasSocialesModificado: data.opeDatosAsistenciasSocialesModificado,
      opeDatosAsistenciasSociales: data.opeDatosAsistenciasSociales,
      opeDatosAsistenciasTraduccionesModificado: data.opeDatosAsistenciasTraduccionesModificado,
      opeDatosAsistenciasTraducciones: data.opeDatosAsistenciasTraducciones,
    };
    return firstValueFrom(
      this.http.post(this.endpoint, body).pipe(
        map((response) => {
          return response;
        }),
        catchError((error) => {
          return throwError(error.error);
        })
      )
    );
  }

  update(data: any) {
    const body = {
      id: data.id,
      idOpePuerto: data.opePuerto,
      fecha: this.datepipe.transform(data.fecha, 'yyyy-MM-dd HH:mm:ss'),
      opeDatosAsistenciasSanitariasModificado: data.opeDatosAsistenciasSanitariasModificado,
      opeDatosAsistenciasSanitarias: data.opeDatosAsistenciasSanitarias,
      opeDatosAsistenciasSocialesModificado: data.opeDatosAsistenciasSocialesModificado,
      opeDatosAsistenciasSociales: data.opeDatosAsistenciasSociales,
      opeDatosAsistenciasTraduccionesModificado: data.opeDatosAsistenciasTraduccionesModificado,
      opeDatosAsistenciasTraducciones: data.opeDatosAsistenciasTraducciones,
    };

    return firstValueFrom(
      this.http.put(this.endpoint, body).pipe(
        map((response) => {
          return response;
        }),
        catchError((error) => {
          return throwError(error.error);
        })
      )
    );
  }

  delete(id: number) {
    const endpoint = `/ope-datos-asistencias/${id}`;

    return firstValueFrom(this.http.delete(endpoint).pipe((response) => response));
  }

  //EXCEL

  exportarExcel(filtros: {
    fechaInicio?: string;
    fechaFin?: string;
    IdOpePeriodo?: number;
    IdOpeFase?: number;
    IdsOpePuertos?: number[];
    criterioNumerico?: string;
    criterioNumericoRadio?: string;
    criterioNumericoCriterioCantidad?: string;
    criterioNumericoCriterioCantidadCantidad?: number;
  }): Observable<Blob> {
    // Construye params de forma dinámica
    const params: any = {};
    Object.entries(filtros).forEach(([k, v]) => {
      if (v != null && v !== '' && !(Array.isArray(v) && v.length === 0)) {
        params[k] = v;
      }
    });

    return this.http.get(`${this.endpoint}/exportExcel`, {
      params,
      responseType: 'blob',
    });
  }
}
