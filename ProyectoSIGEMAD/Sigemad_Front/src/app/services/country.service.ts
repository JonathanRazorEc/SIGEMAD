import { HttpClient } from '@angular/common/http';
import { Injectable, inject } from '@angular/core';
import { firstValueFrom } from 'rxjs';
import { Countries } from '../types/country.type';

@Injectable({ providedIn: 'root' })
export class CountryService {
  private http = inject(HttpClient);

  getExtranjeros() {
    const endpoint = '/paises?mostrarNacional=false';
    return firstValueFrom(this.http.get<Countries[]>(endpoint).pipe((response) => response));
  }

  getNacionales() {
    const endpoint = '/paises?mostrarNacional=true';

    return firstValueFrom(this.http.get<Countries[]>(endpoint).pipe((response) => response));
  }

  get() {
    let endpoint = '/paises';

    return firstValueFrom(this.http.get<Countries[]>(endpoint).pipe((response) => response));
  }
}
