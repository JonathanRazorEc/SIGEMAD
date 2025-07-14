import { HttpClient } from '@angular/common/http';
import { Injectable, inject } from '@angular/core';
import { firstValueFrom } from 'rxjs';
import { Impact } from '../types/impact.type';

@Injectable({ providedIn: 'root' })
export class ImpactService {
  private http = inject(HttpClient);

  get() {
    const endpoint = '/impactos';

    return firstValueFrom(this.http.get<Impact[]>(endpoint).pipe((response) => response));
  }
}
