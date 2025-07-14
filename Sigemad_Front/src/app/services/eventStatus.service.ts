import { HttpClient } from '@angular/common/http';
import { Injectable, inject } from '@angular/core';
import { firstValueFrom } from 'rxjs';

import { EventStatus } from '../types/eventStatus.type';

@Injectable({ providedIn: 'root' })
export class EventStatusService {
  private http = inject(HttpClient);

  get() {
    let endpoint = '/estados-sucesos';

    return firstValueFrom(this.http.get<EventStatus[]>(endpoint).pipe((response) => response));
  }
}
