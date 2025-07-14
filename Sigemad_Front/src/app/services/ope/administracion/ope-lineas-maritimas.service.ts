import { DatePipe } from '@angular/common';
import { HttpClient } from '@angular/common/http';
import { Injectable, inject } from '@angular/core';
import { catchError, firstValueFrom, map, throwError } from 'rxjs';
import { ApiResponse } from '../../../types/api-response.type';
import { OpeLineaMaritima } from '../../../types/ope/administracion/ope-linea-maritima.type';
import { Observable } from 'rxjs';

@Injectable({ providedIn: 'root' })
export class OpeLineasMaritimasService {
  public http = inject(HttpClient);
  public datepipe = inject(DatePipe);
  public endpoint = '/ope-lineas-maritimas';

  generateUrlWitchParams({ url, params }: any) {
    return Object.keys(params).reduce((prev: any, key: any) => {
      const value = params[key];
      if (value === undefined || value === null || value === '') return prev;

      const separator = prev.includes('?') ? '&' : '?';
      return `${prev}${separator}${encodeURIComponent(key)}=${encodeURIComponent(value)}`;
    }, url);
  }

  get(query: any = '') {
    const URLBASE = `${this.endpoint}?Sort=desc&PageSize=99999`;

    const endpoint = this.generateUrlWitchParams({
      url: URLBASE,
      params: query,
    });
    return firstValueFrom(this.http.get<ApiResponse<OpeLineaMaritima[]>>(endpoint));
  }

  post(data: any) {
    const fechaValidezHasta = data.fechaValidezHasta ? new Date(data.fechaValidezHasta) : null;
    const fechaValidezHastaFormateada =
      fechaValidezHasta && !isNaN(fechaValidezHasta.getTime()) ? this.datepipe.transform(fechaValidezHasta, 'yyyy-MM-dd HH:mm:ss') : null;

    const body = {
      nombre: data.nombre,
      idOpePuertoOrigen: data.opePuertoOrigen,
      idOpePuertoDestino: data.opePuertoDestino,
      idOpeFase: data.opeFase,
      fechaValidezDesde: this.datepipe.transform(data.fechaValidezDesde, 'yyyy-MM-dd HH:mm:ss'),
      fechaValidezHasta: fechaValidezHastaFormateada,
      numeroRotaciones: data.numeroRotaciones,
      numeroPasajeros: data.numeroPasajeros,
      numeroTurismos: data.numeroTurismos,
      numeroAutocares: data.numeroAutocares,
      numeroCamiones: data.numeroCamiones,
      numeroTotalVehiculos: data.numeroTotalVehiculos,
    };

    return firstValueFrom(
      this.http.post(this.endpoint, body).pipe(
        map((response) => response),
        catchError((error) => throwError(() => error.error))
      )
    );
  }

  update(data: any) {
    const fechaValidezHasta = data.fechaValidezHasta ? new Date(data.fechaValidezHasta) : null;
    const fechaValidezHastaFormateada =
      fechaValidezHasta && !isNaN(fechaValidezHasta.getTime()) ? this.datepipe.transform(fechaValidezHasta, 'yyyy-MM-dd HH:mm:ss') : null;

    const body = {
      id: data.id,
      nombre: data.nombre,
      idOpePuertoOrigen: data.opePuertoOrigen,
      idOpePuertoDestino: data.opePuertoDestino,
      idOpeFase: data.opeFase,
      fechaValidezDesde: this.datepipe.transform(data.fechaValidezDesde, 'yyyy-MM-dd HH:mm:ss'),
      fechaValidezHasta: fechaValidezHastaFormateada,
      numeroRotaciones: data.numeroRotaciones,
      numeroPasajeros: data.numeroPasajeros,
      numeroTurismos: data.numeroTurismos,
      numeroAutocares: data.numeroAutocares,
      numeroCamiones: data.numeroCamiones,
      numeroTotalVehiculos: data.numeroTotalVehiculos,
    };

    return firstValueFrom(
      this.http.put(this.endpoint, body).pipe(
        map((response) => response),
        catchError((error) => throwError(() => error.error))
      )
    );
  }

  delete(id: number) {
    const endpoint = `${this.endpoint}/${id}`;
    return firstValueFrom(this.http.delete(endpoint));
  }

  exportarExcel(filtros: {
    nombre?: string;
    fechaInicioFaseSalida?: string;
    fechaFinFaseSalida?: string;
    fechaInicioFaseRetorno?: string;
    fechaFinFaseRetorno?: string;
    idPuertoOrigen?: number;
    idPuertoDestino?: number;
    idPaisOrigen?: number;
    idPaisDestino?: number;
    idOpeFase?: number;
  }): Observable<Blob> {
    const params: any = {};
    for (const key in filtros) {
      const value = filtros[key as keyof typeof filtros];
      if (value !== undefined && value !== null && value !== '') {
        params[key] = value;
      }
    }

    const queryParams = new URLSearchParams(params).toString();
    const url = `${this.endpoint}/exportExcel?${queryParams}`;

    return this.http.get(url, { responseType: 'blob' });
  }
}
