import { HttpClient } from '@angular/common/http';
import { Injectable, inject } from '@angular/core';
import { firstValueFrom } from 'rxjs';
import { OpePeriodoTipo } from '@type/ope/administracion/ope-periodo-tipo.type';

@Injectable({ providedIn: 'root' })
export class OpePeriodosTiposService {
  public http = inject(HttpClient);
  public endpoint = '/ope-periodos-tipos';

  get() {
    return firstValueFrom(this.http.get<OpePeriodoTipo[]>(this.endpoint).pipe((response) => response));
  }
}
