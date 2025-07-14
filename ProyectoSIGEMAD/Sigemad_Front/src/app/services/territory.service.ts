import { HttpClient } from '@angular/common/http';
import { Injectable, inject } from '@angular/core';
import { firstValueFrom } from 'rxjs';

import { Territory } from '../types/territory.type';

@Injectable({ providedIn: 'root' })
export class TerritoryService {
  private http = inject(HttpClient);


get(): Promise<Territory[]> {
  const endpoint = '/territorios';
  return firstValueFrom(this.http.get<Territory[]>(endpoint));
}

  getForCreate() {
    const endpoint = '/territorios-crear';

    return firstValueFrom(this.http.get<Territory[]>(endpoint).pipe((response) => response));
  }
}
