import { HttpClient } from '@angular/common/http';
import { Injectable, inject } from '@angular/core';
import { firstValueFrom } from 'rxjs';
import { OpeAreaDescansoTipo } from '@type/ope/administracion/ope-area-descanso-tipo';

@Injectable({ providedIn: 'root' })
export class OpeAreasDescansoTiposService {
  public http = inject(HttpClient);
  public endpoint = '/ope-areas-descanso-tipos';

  get() {
    return firstValueFrom(this.http.get<OpeAreaDescansoTipo[]>(this.endpoint).pipe((response) => response));
  }
}
