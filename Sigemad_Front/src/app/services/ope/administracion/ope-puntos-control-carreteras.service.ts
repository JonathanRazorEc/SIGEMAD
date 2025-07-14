import { DatePipe } from '@angular/common';
import { HttpClient } from '@angular/common/http';
import { Injectable, inject } from '@angular/core';
import { catchError, firstValueFrom, map, throwError } from 'rxjs';
import { ApiResponse } from '../../../types/api-response.type';
import { OpePuntoControlCarretera } from '@type/ope/administracion/ope-punto-control-carretera.type';

@Injectable({ providedIn: 'root' })
export class OpePuntosControlCarreterasService {
  public http = inject(HttpClient);
  public datepipe = inject(DatePipe);
  public endpoint = '/ope-puntos-control-carreteras';

  generateUrlWitchParams({ url, params }: any) {
    return Object.keys(params).reduce((prev: any, key: any, index: any) => {
      if (!params[key]) {
        return `${prev}`;
      }
      return `${prev}&${key}=${params[key]}`;
    }, `${url}`);
  }

  get(query: any = '') {
    const URLBASE = '/ope-puntos-control-carreteras?Sort=desc&PageSize=99999';

    const endpoint = this.generateUrlWitchParams({
      url: URLBASE,
      params: query,
    });
    return firstValueFrom(this.http.get<ApiResponse<OpePuntoControlCarretera[]>>(endpoint).pipe((response) => response));
  }

  post(data: any) {
    const body = {
      nombre: data.nombre,
      idCcaa: data.autonomousCommunity,
      idProvincia: data.provincia,
      idMunicipio: data.municipality,
      carretera: data.carretera,
      PK: data.PK,
      coordenadaUTM_X: data.coordenadaUTM_X,
      coordenadaUTM_Y: data.coordenadaUTM_Y,
      transitoMedioVehiculos: data.transitoMedioVehiculos,
      transitoAltoVehiculos: data.transitoAltoVehiculos,
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
      idCcaa: data.autonomousCommunity,
      idProvincia: data.provincia,
      idMunicipio: data.municipality,
      carretera: data.carretera,
      PK: data.PK,
      coordenadaUTM_X: data.coordenadaUTM_X,
      coordenadaUTM_Y: data.coordenadaUTM_Y,
      transitoMedioVehiculos: data.transitoMedioVehiculos,
      transitoAltoVehiculos: data.transitoAltoVehiculos,
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
    const endpoint = `/ope-puntos-control-carreteras/${id}`;

    return firstValueFrom(this.http.delete(endpoint).pipe((response) => response));
  }
}
