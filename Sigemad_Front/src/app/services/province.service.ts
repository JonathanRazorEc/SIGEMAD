import { HttpClient, HttpParams } from '@angular/common/http';
import { Injectable, inject } from '@angular/core';
import { firstValueFrom } from 'rxjs';

import { Province } from '../types/province.type';

@Injectable({ providedIn: 'root' })
export class ProvinceService {
  private http = inject(HttpClient);

  get(ac_id: number = 0) {
    let endpoint = '/Provincias';

    if (ac_id != 0) {
      endpoint = `/Provincias/${ac_id}`;
    }

    return firstValueFrom(this.http.get<Province[]>(endpoint).pipe((response) => response));
  }

  getBySearch(busqueda?: string, idCCaa?: number) {
    let params = new HttpParams();
    
    if (busqueda && busqueda.trim().length > 0) {
      params = params.set('busqueda', busqueda);
    }
    
    if (idCCaa !== undefined && idCCaa > 0) {
      params = params.set('idCCaa', idCCaa.toString());
    }
    
    return firstValueFrom(this.http.get<Province[]>('/v2-Provincias/Busqueda', { params }).pipe((response) => response));
  }

}
