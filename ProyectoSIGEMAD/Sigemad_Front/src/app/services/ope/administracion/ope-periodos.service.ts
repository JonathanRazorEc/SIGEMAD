import { DatePipe } from '@angular/common';
import { HttpClient, HttpParams } from '@angular/common/http';
import { Injectable, inject } from '@angular/core';
import { catchError, firstValueFrom, map, Observable, throwError } from 'rxjs';
import { ApiResponse } from '../../../types/api-response.type';
import { OpePeriodo } from '../../../types/ope/administracion/ope-periodo.type';

@Injectable({ providedIn: 'root' })
export class OpePeriodosService {
  public http = inject(HttpClient);
  public datepipe = inject(DatePipe);
  public endpoint = '/ope-periodos';

  generateUrlWitchParams({ url, params }: any) {
    return Object.keys(params).reduce((prev: any, key: any, index: any) => {
      if (!params[key]) {
        return `${prev}`;
      }
      return `${prev}&${key}=${params[key]}`;
    }, `${url}`);
  }

  get(query: any = '') {
    const URLBASE = '/ope-periodos?Sort=desc&PageSize=99999';

    const endpoint = this.generateUrlWitchParams({
      url: URLBASE,
      params: query,
    });
    return firstValueFrom(this.http.get<ApiResponse<OpePeriodo[]>>(endpoint).pipe((response) => response));
  }

  post(data: any) {
    const body = {
      nombre: data.nombre,
      idOpePeriodoTipo: data.opePeriodoTipo,
      fechaInicioFaseSalida: this.datepipe.transform(data.fechaInicioFaseSalida, 'yyyy-MM-dd HH:mm:ss'),
      fechaFinFaseSalida: this.datepipe.transform(data.fechaFinFaseSalida, 'yyyy-MM-dd HH:mm:ss'),
      fechaInicioFaseRetorno: this.datepipe.transform(data.fechaInicioFaseRetorno, 'yyyy-MM-dd HH:mm:ss'),
      fechaFinFaseRetorno: this.datepipe.transform(data.fechaFinFaseRetorno, 'yyyy-MM-dd HH:mm:ss'),
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
      nombre: data.nombre,
      idOpePeriodoTipo: data.opePeriodoTipo,
      fechaInicioFaseSalida: this.datepipe.transform(data.fechaInicioFaseSalida, 'yyyy-MM-dd HH:mm:ss'),
      fechaFinFaseSalida: this.datepipe.transform(data.fechaFinFaseSalida, 'yyyy-MM-dd HH:mm:ss'),
      fechaInicioFaseRetorno: this.datepipe.transform(data.fechaInicioFaseRetorno, 'yyyy-MM-dd HH:mm:ss'),
      fechaFinFaseRetorno: this.datepipe.transform(data.fechaFinFaseRetorno, 'yyyy-MM-dd HH:mm:ss'),
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
    const endpoint = `/ope-periodos/${id}`;

    return firstValueFrom(this.http.delete(endpoint).pipe((response) => response));
  }

  //excel

  exportarExcel(filtros: {
    nombre?: string;
    idOpePeriodoTipo?: number;
    fechaInicioFaseSalida?: string;
    fechaFinFaseSalida?: string;
    fechaInicioFaseRetorno?: string;
    fechaFinFaseRetorno?: string;
    estadoEliminado: 'activo' | 'eliminado' | 'todos';
  }): Observable<Blob> {
    let params = new HttpParams();
    Object.entries(filtros).forEach(([k, v]) => {
      if (v != null && v !== '') params = params.set(k, v.toString());
    });
    return this.http.get(`${this.endpoint}/exportExcel`, {
      params,
      responseType: 'blob',
    });
  } //..
}
