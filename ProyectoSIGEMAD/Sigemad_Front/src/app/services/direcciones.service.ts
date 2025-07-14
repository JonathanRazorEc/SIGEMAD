import { HttpClient } from '@angular/common/http';
import { Injectable, inject } from '@angular/core';
import { firstValueFrom } from 'rxjs';

@Injectable({ providedIn: 'root' })
export class DireccionesService {
  private http = inject(HttpClient);

  getAllDirecciones() {
    let endpoint = `/tipos-direcciones-emergencias`;

    return firstValueFrom(this.http.get<any[]>(endpoint).pipe((response) => response));
  }
}
