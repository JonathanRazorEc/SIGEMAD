import { HttpClient } from '@angular/common/http';
import { Injectable, inject } from '@angular/core';
import { firstValueFrom } from 'rxjs';
import { ComparativeDate } from '../types/comparative-date.type';

@Injectable({ providedIn: 'root' })
export class ComparativeDateService {
  private http = inject(HttpClient);
  private readonly baseUrl = '';

  get() {
    return firstValueFrom(
      this.http.get<ComparativeDate[]>(`${this.baseUrl}/comparativa-fechas`)
    );
  }
}
