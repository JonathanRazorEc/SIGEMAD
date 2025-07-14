import { DatePipe } from '@angular/common';
import { HttpClient } from '@angular/common/http';
import { Injectable, inject } from '@angular/core';
import { catchError, firstValueFrom, map, throwError } from 'rxjs';
import { ApiResponse } from '../../../types/api-response.type';
import { OpePorcentajeOcupacionAreaEstacionamiento } from '../../../types/ope/administracion/ope-porcentaje-ocupacion-area-estacionamiento.type';

@Injectable({ providedIn: 'root' })
export class OpePorcentajesOcupacionAreasEstacionamientoService {
  public http = inject(HttpClient);
  public datepipe = inject(DatePipe);
  public endpoint = '/ope-porcentajes-ocupacion-areas-estacionamiento';

  generateUrlWitchParams({ url, params }: any) {
    return Object.keys(params).reduce((prev: any, key: any, index: any) => {
      if (!params[key]) {
        return `${prev}`;
      }
      return `${prev}&${key}=${params[key]}`;
    }, `${url}`);
  }

  get(query: any = '') {
    const URLBASE = '/ope-porcentajes-ocupacion-areas-estacionamiento?Sort=desc&PageSize=999';

    const endpoint = this.generateUrlWitchParams({
      url: URLBASE,
      params: query,
    });
    return firstValueFrom(this.http.get<ApiResponse<OpePorcentajeOcupacionAreaEstacionamiento[]>>(endpoint).pipe((response) => response));
  }

  post(data: any) {
    const body = {
      idOpeOcupacion: data.opeOcupacion,
      porcentajeInferior: data.porcentajeInferior,
      porcentajeSuperior: data.porcentajeSuperior,
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
      idOpeOcupacion: data.opeOcupacion,
      porcentajeInferior: data.porcentajeInferior,
      porcentajeSuperior: data.porcentajeSuperior,
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
    const endpoint = `/ope-porcentajes-ocupacion-areas-estacionamiento/${id}`;

    return firstValueFrom(this.http.delete(endpoint).pipe((response) => response));
  }
}
