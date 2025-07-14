import { HttpClient, HttpParams } from '@angular/common/http';
import { Injectable, inject } from '@angular/core';
import { firstValueFrom } from 'rxjs';

import { Municipality } from '../types/municipality.type';

@Injectable({ providedIn: 'root' })
export class MunicipalityService {
  private http = inject(HttpClient);

  get(province_id: number | string) {
    const endpoint = `/Municipios/${province_id}`;

    return firstValueFrom(this.http.get<Municipality[]>(endpoint).pipe((response) => response));
  }

  getBySearch(searchText: string = '', idProvincia?: number) {
    let params = new HttpParams();
    
    if (searchText) {
      params = params.set('busqueda', searchText);
    }
    
    if (idProvincia !== undefined && idProvincia > 0) {
      params = params.set('idProvincia', idProvincia.toString());
    }
    
    return firstValueFrom(this.http.get<Municipality[]>('/v2-Municipios/Busqueda', { params }).pipe((response) => response));
  }
}
