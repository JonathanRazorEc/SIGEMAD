import { HttpClient } from '@angular/common/http';
import { Injectable, inject } from '@angular/core';
import { ApiResponse } from '@type/api-response.type';
import { OpePais } from '@type/ope/administracion/ope-pais.type';
import { firstValueFrom } from 'rxjs';

@Injectable({ providedIn: 'root' })
export class OpePaisesService {
  public http = inject(HttpClient);

  generateUrlWitchParams({ url, params }: any) {
    return Object.keys(params).reduce((prev: any, key: any, index: any) => {
      if (!params[key]) {
        return `${prev}`;
      }
      // Si es el primer parámetro, se agrega ? al principio
      if (index === 0) {
        return `${prev}?${key}=${params[key]}`;
      }
      return `${prev}&${key}=${params[key]}`;
    }, `${url}`);
  }

  get(query: any = '') {
    const URLBASE = '/ope-paises';

    const endpoint = this.generateUrlWitchParams({
      url: URLBASE,
      params: query,
    });
    //return firstValueFrom(this.http.get<ApiResponse<OpePais[]>>(endpoint).pipe((response) => response));

    return firstValueFrom(this.http.get<OpePais[]>(endpoint));
  }
}
