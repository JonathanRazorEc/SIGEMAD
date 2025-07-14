import { HttpClient } from '@angular/common/http';
import { Injectable, inject } from '@angular/core';
import { firstValueFrom } from 'rxjs';

@Injectable({ providedIn: 'root' })
export class PlanesService {
  private http = inject(HttpClient);

  getAllPlanes() {
    let endpoint = `/tipos-planes`;

    return firstValueFrom(this.http.get<any[]>(endpoint).pipe((response) => response));
  }
}
