import { DatePipe } from '@angular/common';
import { HttpClient, HttpParams } from '@angular/common/http';
import { Injectable, inject } from '@angular/core';
import { catchError, firstValueFrom, map, throwError } from 'rxjs';

import { ApiResponse } from '../types/api-response.type';
import { FireDetail, FireDetailResponse } from '../types/fire-detail.type';
import { Fire } from '../types/fire.type';

@Injectable({ providedIn: 'root' })
export class FireService {
  public http = inject(HttpClient);
  public datepipe = inject(DatePipe);
  public endpoint = '/Incendios';

  generateUrlWitchParams({ url, params }: any) {
    return Object.keys(params).reduce((prev: any, key: any, index: any) => {
      if (!params[key]) {
        return `${prev}`;
      }
      return `${prev}&${key}=${params[key]}`;
    }, `${url}`);
  }

  getOnMainPage(query: any = {}, page: number, pageSize: number) {
    const URLBASE = '/Incendios?Sort=desc';
  
    const pageIndex = page === 0 ? 0 : page - 1;

    // Construir la URL con los parámetros de paginación
    const endpoint = this.generateUrlWitchParams({
      url: `${URLBASE}&PageIndex=${pageIndex}&PageSize=${pageSize}`,
      params: query,
    });
  
    // Realizar la solicitud HTTP y devolver los datos como una promesa
    return firstValueFrom(
      this.http.get<ApiResponse<Fire[]>>(endpoint).pipe(
        catchError((error) => {
          console.error('Error al obtener datos:', error);
          return throwError(() => new Error('Error al cargar los datos'));
        })
      )
    );
  }

  getById(id: number) {
    let endpoint = `/Incendios/${id}`;

    return firstValueFrom(this.http.get<Fire>(endpoint).pipe((response) => response));
  }

  details(fire_id: number, page: number = 0, pageSize: number = 5) {
    const pageIndex = page ;
    const endpoint = `/sucesos/registros?IdSuceso=${fire_id}&PageSize=${pageSize}&PageIndex=${pageIndex}`;
    return firstValueFrom(this.http.get<FireDetailResponse>(endpoint).pipe((response) => response));
  }

  get(query: any = {}, page = 0, pageSize = 5) {
    
    const pageIndex = page === 0 ? 0 : page - 1;

    const params = new HttpParams({ fromObject: {
      ...query,
      pageIndex,
      PageSize: pageSize,
      Sort:     'desc'
    }});
  
    return firstValueFrom(
      this.http.get<ApiResponse<Fire[]>>('/Incendios', { params }).pipe(
        catchError(err => throwError(() => new Error('Error al cargar los datos')))
      )
    );
  }

  post(data: any) {
    console.log("🚀 ~ FireService ~ post ~ data:", data)
    console.log("🚀 ~ FireService ~ post ~ data:", data)
    const body = {
      IdTerritorio: data.territory ? data.territory : 1,
      idClaseSuceso: data.classEvent,
      idEstadoSuceso: data.eventStatus,
      //fechaInicio: this.datepipe.transform(data.startDate, 'yyyy-MM-dd' + ' ' + data.startTime),
      // PCD
      // fechaInicio: this.datepipe.transform(data.startDateTime, 'yyyy-MM-dd  HH:mm:ss'),
      fechaInicio: data.startDateTime,
      // FIN PCD

      denominacion: data.denomination,
      notaGeneral: data.generalNote,
      IdProvincia: data.province,
      IdMunicipio: data.municipality,
      idPais: data.country,
      ubicacion: data.ubication,
      esLimitrofe: data.isLimitrofe,
      GeoPosicion: data.geoposition,
      idMunicipioExtranjero: data.idMunicipioExtranjero,
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
      IdTerritorio: data.territory,
      idClaseSuceso: data.classEvent,
      idEstadoSuceso: data.eventStatus,
      //fechaInicio: this.datepipe.transform(data.startDate, 'yyyy-MM-dd h:mm:ss'),
      // PCD
      // fechaInicio: this.datepipe.transform(data.startDateTime, 'yyyy-MM-dd HH:mm:ss'),
      fechaInicio: data.startDateTime,
      // FIN PCD

      denominacion: data.denomination,
      notaGeneral: data.generalNote,
      IdProvincia: data.province,
      IdMunicipio: data.municipality,
      idPais: data.country,
      ubicacion: data.ubication,
      esLimitrofe: data.isLimitrofe,
      GeoPosicion: data.geoposition,
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
    const endpoint = `/Incendios/${id}`;

    return firstValueFrom(this.http.delete(endpoint).pipe((response) => response));
  }
}
