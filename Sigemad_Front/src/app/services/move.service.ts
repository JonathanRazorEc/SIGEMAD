import { HttpClient } from '@angular/common/http';
import { Injectable, inject } from '@angular/core';
import { firstValueFrom } from 'rxjs';
import { Move } from '../types/move.type';

@Injectable({ providedIn: 'root' })
export class MoveService {
  private http = inject(HttpClient);

  get() {
    let endpoint = '/tipos-movimientos';

    return firstValueFrom(this.http.get<Move[]>(endpoint).pipe((response) => response));
  }
}
