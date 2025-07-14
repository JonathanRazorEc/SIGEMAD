import { HttpClient } from '@angular/common/http';
import { Injectable, inject } from '@angular/core';
import { firstValueFrom } from 'rxjs';

@Injectable({ providedIn: 'root' })
export class TipoDocumentoService {
  private http = inject(HttpClient);

  get() {
    let endpoint = `/tipo-documentos`;

    return firstValueFrom(this.http.get<any[]>(endpoint).pipe((response) => response));
  }
}
