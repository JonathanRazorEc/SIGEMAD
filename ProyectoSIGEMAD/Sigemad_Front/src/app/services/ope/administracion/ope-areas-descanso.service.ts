import { DatePipe } from '@angular/common';
import { HttpClient } from '@angular/common/http';
import { Injectable, inject } from '@angular/core';
import { catchError, firstValueFrom, map, throwError } from 'rxjs';
import { ApiResponse } from '../../../types/api-response.type';
import { OpeAreaDescanso } from '@type/ope/administracion/ope-area-descanso.type';

@Injectable({ providedIn: 'root' })
export class OpeAreasDescansoService {
  public http = inject(HttpClient);
  public datepipe = inject(DatePipe);
  public endpoint = '/ope-areas-descanso';

  generateUrlWitchParams({ url, params }: any) {
    return Object.keys(params).reduce((prev: any, key: any, index: any) => {
      if (!params[key]) {
        return `${prev}`;
      }
      return `${prev}&${key}=${params[key]}`;
    }, `${url}`);
  }

  get(query: any = '') {
    const URLBASE = '/ope-areas-descanso?Sort=desc&PageSize=99999';

    const endpoint = this.generateUrlWitchParams({
      url: URLBASE,
      params: query,
    });
    return firstValueFrom(this.http.get<ApiResponse<OpeAreaDescanso[]>>(endpoint).pipe((response) => response));
  }

  post(data: any) {
    const body = {
      nombre: data.nombre,
      idOpeAreaDescansoTipo: data.opeAreaDescansoTipo,
      idCcaa: data.autonomousCommunity,
      idProvincia: data.provincia,
      idMunicipio: data.municipality,
      carretera: data.carretera,
      PK: data.PK,
      coordenadaUTM_X: data.coordenadaUTM_X,
      coordenadaUTM_Y: data.coordenadaUTM_Y,
      capacidad: data.capacidad,
      //idOpeEstadoOcupacion: data.opeEstadoOcupacion,
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
      idOpeAreaDescansoTipo: data.opeAreaDescansoTipo,
      idCcaa: data.autonomousCommunity,
      idProvincia: data.provincia,
      idMunicipio: data.municipality,
      carretera: data.carretera,
      PK: data.PK,
      coordenadaUTM_X: data.coordenadaUTM_X,
      coordenadaUTM_Y: data.coordenadaUTM_Y,
      capacidad: data.capacidad,
      //idOpeEstadoOcupacion: data.opeEstadoOcupacion,
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
    const endpoint = `/ope-areas-descanso/${id}`;

    return firstValueFrom(this.http.delete(endpoint).pipe((response) => response));
  }
}
