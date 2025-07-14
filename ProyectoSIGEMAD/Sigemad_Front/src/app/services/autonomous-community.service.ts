import { HttpClient, HttpParams } from '@angular/common/http';
import { Injectable, inject } from '@angular/core';
import { firstValueFrom } from 'rxjs';

import { AutonomousCommunity } from '../types/autonomous-community.type';

@Injectable({ providedIn: 'root' })
export class AutonomousCommunityService {
  private http = inject(HttpClient);

  get() {
    const endpoint = '/ComunidadesAutonomas';

    return firstValueFrom(this.http.get<AutonomousCommunity[]>(endpoint).pipe((response) => response));
  }

  getByCountry(idCountry: string) {
    const endpoint = `/paises/${idCountry}/comunidades`;

    return firstValueFrom(this.http.get<AutonomousCommunity[]>(endpoint).pipe((response) => response));
  }

  getBySearch(busqueda?: string, IdPais?: number): Promise<AutonomousCommunity[]> {
    let params = new HttpParams();
    
    if (busqueda && busqueda.trim().length > 0) {
      params = params.set('busqueda', busqueda);
    }
    
    if (IdPais !== undefined && IdPais > 0) {
      params = params.set('IdPais', IdPais.toString());
    }
    
    return firstValueFrom(this.http.get<AutonomousCommunity[]>('/v2-paises/busqueda/comunidades', { params }).pipe((response) => response));
  }
}