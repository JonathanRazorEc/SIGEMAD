import { DatePipe } from '@angular/common';
import { HttpClient } from '@angular/common/http';
import { Injectable, inject } from '@angular/core';
import { catchError, firstValueFrom, map, Observable, throwError } from 'rxjs';
import { ApiResponse } from '../../../types/api-response.type';
import { OpeDatoFrontera } from '@type/ope/datos/ope-dato-frontera.type';

@Injectable({ providedIn: 'root' })
export class OpeDatosFronterasService {
  public http = inject(HttpClient);
  public datepipe = inject(DatePipe);
  public endpoint = '/ope-datos-fronteras';

  /*
  generateUrlWitchParams({ url, params }: any) {
    return Object.keys(params).reduce((prev: any, key: any, index: any) => {
      if (!params[key]) {
        return `${prev}`;
      }
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
    const URLBASE = '/ope-datos-fronteras?Sort=desc&PageSize=99999';

    const endpoint = this.generateUrlWitchParams({
      url: URLBASE,
      params: query,
    });
    return firstValueFrom(this.http.get<ApiResponse<OpeDatoFrontera[]>>(endpoint).pipe((response) => response));
  }

  post(data: any) {
    const body = {
      //idOpeFrontera: data.opeFrontera,
      idOpeFrontera: data.opeFrontera,
      fecha: this.datepipe.transform(data.fecha, 'yyyy-MM-dd HH:mm:ss'),
      idOpeDatoFronteraIntervaloHorario: data.opeDatoFronteraIntervaloHorario,
      intervaloHorarioPersonalizado: data.intervaloHorarioPersonalizado,
      inicioIntervaloHorarioPersonalizado: data.inicioIntervaloHorarioPersonalizado,
      finIntervaloHorarioPersonalizado: data.finIntervaloHorarioPersonalizado,
      numeroVehiculos: data.numeroVehiculos,
      afluencia: data.afluencia,
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
      idOpeFrontera: data.opeFrontera,
      fecha: this.datepipe.transform(data.fecha, 'yyyy-MM-dd HH:mm:ss'),
      idOpeDatoFronteraIntervaloHorario: data.opeDatoFronteraIntervaloHorario,
      intervaloHorarioPersonalizado: data.intervaloHorarioPersonalizado,
      inicioIntervaloHorarioPersonalizado: data.inicioIntervaloHorarioPersonalizado,
      finIntervaloHorarioPersonalizado: data.finIntervaloHorarioPersonalizado,
      numeroVehiculos: data.numeroVehiculos,
      afluencia: data.afluencia,
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
    const endpoint = `/ope-datos-fronteras/${id}`;

    return firstValueFrom(this.http.delete(endpoint).pipe((response) => response));
  }

  // 🟢 Exportar Excel con filtros
  exportarExcel(filtros: any) {
    const params = this.buildParams(filtros);
    return this.http.get(`${this.endpoint}/exportExcel`, {
      params,
      responseType: 'blob',
    });
  }

  private buildParams(filtros: any): { [key: string]: string | string[] } {
    const params: { [key: string]: string | string[] } = {};
    Object.entries(filtros).forEach(([key, value]) => {
      if (value !== null && value !== undefined && value !== '') {
        if (Array.isArray(value)) {
          params[key] = value.map((v) => v.toString());
        } else {
          params[key] = value.toString();
        }
      }
    });
    return params;
  }
}
