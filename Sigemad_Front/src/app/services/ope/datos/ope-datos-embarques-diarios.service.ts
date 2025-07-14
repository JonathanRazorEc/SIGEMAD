import { DatePipe } from '@angular/common';
import { HttpClient } from '@angular/common/http';
import { Injectable, inject } from '@angular/core';
import { catchError, firstValueFrom, map, Observable, throwError } from 'rxjs';
import { ApiResponse } from '../../../types/api-response.type';
import { OpeDatoEmbarqueDiario } from '@type/ope/datos/ope-dato-embarque-diario.type';

@Injectable({ providedIn: 'root' })
export class OpeDatosEmbarquesDiariosService {
  public http = inject(HttpClient);
  public datepipe = inject(DatePipe);
  public endpoint = '/ope-datos-embarques-diarios';

  /*
  generateUrlWitchParams({ url, params }: any) {
    return Object.keys(params).reduce((prev: any, key: any) => {
      const value = params[key];
      if (value === null || value === undefined || value === '') return prev;

      const separator = prev.includes('?') ? '&' : '?';
      return `${prev}${separator}${encodeURIComponent(key)}=${encodeURIComponent(value)}`;
    }, url);
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
    return firstValueFrom(this.http.get<ApiResponse<OpeDatoEmbarqueDiario[]>>(finalUrl).pipe((res) => res));
  }

  post(data: any) {
    const body = {
      idOpeLineaMaritima: data.opeLineaMaritima,
      fecha: this.datepipe.transform(data.fecha, 'yyyy-MM-dd HH:mm:ss'),
      numeroRotaciones: data.numeroRotaciones,
      numeroPasajeros: data.numeroPasajeros,
      numeroTurismos: data.numeroTurismos,
      numeroAutocares: data.numeroAutocares,
      numeroCamiones: data.numeroCamiones,
      numeroTotalVehiculos: data.numeroTotalVehiculos,
    };

    return firstValueFrom(
      this.http.post(this.endpoint, body).pipe(
        map((res) => res),
        catchError((err) => throwError(() => err.error))
      )
    );
  }

  update(data: any) {
    const body = {
      id: data.id,
      idOpeLineaMaritima: data.opeLineaMaritima,
      fecha: this.datepipe.transform(data.fecha, 'yyyy-MM-dd HH:mm:ss'),
      numeroRotaciones: data.numeroRotaciones,
      numeroPasajeros: data.numeroPasajeros,
      numeroTurismos: data.numeroTurismos,
      numeroAutocares: data.numeroAutocares,
      numeroCamiones: data.numeroCamiones,
      numeroTotalVehiculos: data.numeroTotalVehiculos,
    };

    return firstValueFrom(
      this.http.put(this.endpoint, body).pipe(
        map((res) => res),
        catchError((err) => throwError(() => err.error))
      )
    );
  }

  delete(id: number) {
    const url = `${this.endpoint}/${id}`;
    return firstValueFrom(this.http.delete(url));
  }

  exportarExcel(filtros: {
    estadoEliminado: 'activo' | 'eliminado' | 'todos';
    fechaDesde?: string;
    fechaHasta?: string;
    idLinea?: number;
    fase?: string;
    periodo?: string;
    criterioNumerico?: string;
    criterioNumericoRadio?: string;
    criterioNumericoCondicion?: string;
    criterioNumericoCantidad?: number;
  }): Observable<Blob> {
    const params: any = {};

    for (const key in filtros) {
      const value = filtros[key as keyof typeof filtros];
      if (value !== undefined && value !== null && value !== '') {
        params[key] = value;
      }
    }

    return this.http.get(`${this.endpoint}/exportExcel`, {
      params,
      responseType: 'blob',
    });
  }
}
