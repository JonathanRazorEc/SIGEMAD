import { HttpClient } from '@angular/common/http';
import { Injectable, inject } from '@angular/core';
import { firstValueFrom } from 'rxjs';

import { SeverityLevel } from '../types/severity-level.type';

@Injectable({ providedIn: 'root' })
export class SeverityLevelService {
  private http = inject(HttpClient);

  get() {
    //const endpoint = '/NivelGravedad'; ANTIGUO
    const endpoint = '/situaciones-operativas';

    return firstValueFrom(this.http.get<SeverityLevel[]>(endpoint).pipe((response) => response));
  }
}
