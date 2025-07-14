import { HttpClient } from '@angular/common/http';
import { Injectable, inject } from '@angular/core';
import { firstValueFrom } from 'rxjs';

@Injectable({ providedIn: 'root' })
export class SuperficiesService {
  private http = inject(HttpClient);

  getSuperficiesFiltro() {
    let endpoint = `/superficies-filtros`;

    return firstValueFrom(this.http.get<any[]>(endpoint).pipe((response) => response));
  }
}
