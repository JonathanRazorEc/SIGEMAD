import { DatePipe } from '@angular/common';
import { HttpClient } from '@angular/common/http';
import { Injectable, inject } from '@angular/core';
import { catchError, firstValueFrom, map, Observable, throwError } from 'rxjs';
import { ApiResponse } from '../../../types/api-response.type';
import { OpePuerto } from '../../../types/ope/administracion/ope-puerto.type';

@Injectable({ providedIn: 'root' })
export class OpePuertosService {
  public http = inject(HttpClient);
  public datepipe = inject(DatePipe);
  public endpoint = '/ope-puertos';

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

    return firstValueFrom(this.http.get<ApiResponse<OpePuerto[]>>(endpoint));
  }

  getAllEntreFechas(query: any = '') {
    const URLBASE = `${this.endpoint}?Sort=desc&PageSize=99999`;

    const endpoint = this.generateUrlWitchParams({
      url: URLBASE,
      params: query,
    });

    return firstValueFrom(this.http.get<ApiResponse<OpePuerto[]>>(endpoint));
  }

  post(data: any) {
    const fechaValidezHasta = data.fechaValidezHasta ? new Date(data.fechaValidezHasta) : null;
    const fechaValidezHastaFormateada =
      fechaValidezHasta && !isNaN(fechaValidezHasta.getTime()) ? this.datepipe.transform(fechaValidezHasta, 'yyyy-MM-dd HH:mm:ss') : null;

    const body = {
      nombre: data.nombre,
      idOpeFase: data.opeFase,
      idPais: data.country,
      idCcaa: data.autonomousCommunity,
      idProvincia: data.provincia,
      idMunicipio: data.municipality,
      coordenadaUTM_X: data.coordenadaUTM_X,
      coordenadaUTM_Y: data.coordenadaUTM_Y,
      fechaValidezDesde: this.datepipe.transform(data.fechaValidezDesde, 'yyyy-MM-dd HH:mm:ss'),
      fechaValidezHasta: fechaValidezHastaFormateada,
      capacidad: data.capacidad,
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
      idOpeFase: data.opeFase,
      idPais: data.country,
      idCcaa: data.autonomousCommunity,
      idProvincia: data.provincia,
      idMunicipio: data.municipality,
      coordenadaUTM_X: data.coordenadaUTM_X,
      coordenadaUTM_Y: data.coordenadaUTM_Y,
      fechaValidezDesde: this.datepipe.transform(data.fechaValidezDesde, 'yyyy-MM-dd HH:mm:ss'),
      fechaValidezHasta: fechaValidezHastaFormateada,
      capacidad: data.capacidad,
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

  ///excel

  exportarExcel(filtros: {
    estadoEliminado: 'activo' | 'eliminado' | 'todos';
    nombre?: string;
    idPais?: number;
    idOpeFase?: number;
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
