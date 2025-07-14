import { HttpClient } from '@angular/common/http';
import { Injectable, inject } from '@angular/core';
import { firstValueFrom } from 'rxjs';

@Injectable({ providedIn: 'root' })
export class ImpactGroupService {
  private http = inject(HttpClient);

  get() {
    const endpoint = '/grupos-impactos';
    return firstValueFrom(this.http.get(endpoint).pipe((response) => response));
  }
}
