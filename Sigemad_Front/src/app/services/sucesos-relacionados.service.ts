import { HttpClient } from '@angular/common/http';
import { Injectable, inject } from '@angular/core';
import { catchError, firstValueFrom, map, throwError } from 'rxjs';

@Injectable({ providedIn: 'root' })
export class SucesosRelacionadosService {
  private http = inject(HttpClient);
  generateUrlWitchParams({ url, params }: any) {
    return Object.keys(params).reduce((prev: any, key: any, index: any) => {
      if (!params[key]) {
        return `${prev}`;
      }
      return `${prev}&${key}=${params[key]}`;
    }, `${url}`);
  }

  getListaSuceso(query: any = '') {
    const URLBASE = '/Sucesos?Sort=desc&PageSize=15';

    const endpoint = this.generateUrlWitchParams({
      url: URLBASE,
      params: query,
    });

    return firstValueFrom(this.http.get<any[]>(endpoint).pipe((response) => response));
  }

  get(id: string | number, idSuceso: string | number) {
    const endpoint = `/sucesos-relacionados?idSuceso=${idSuceso}&idRegistroActualizacion=${id}`;
    //const endpoint = `/sucesos-relacionados?idSuceso=1&idRegistroActualizacion=${idSucesoPrincipal}`;
    return firstValueFrom(this.http.get<any[]>(endpoint).pipe((response) => response));
  }

  getRegistros(idSuceso: string | number) {
    const endpoint = `/sucesos-relacionados?idSuceso=${idSuceso}`;
    //const endpoint = `/sucesos-relacionados?idSuceso=1&idRegistroActualizacion=${idSucesoPrincipal}`;
    return firstValueFrom(this.http.get<any[]>(endpoint).pipe((response) => response));
  }

  post(body: any) {
    const endpoint = `/sucesos-relacionados/lista`;
    return firstValueFrom(
      this.http.post(endpoint, body).pipe(
        map((response) => {
          return response;
        }),
        catchError((error) => {
          return throwError(error.error);
        })
      )
    );
  }

  delete(idSucesoPrincipal: number | string) {
    const endpoint = `/sucesos-relacionados/${idSucesoPrincipal}`;
    return firstValueFrom(this.http.delete(endpoint).pipe((response) => response));
  }
}
