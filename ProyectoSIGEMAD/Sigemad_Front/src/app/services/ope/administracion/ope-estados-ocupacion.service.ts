import { HttpClient } from '@angular/common/http';
import { Injectable, inject } from '@angular/core';
import { firstValueFrom } from 'rxjs';
import { OpeEstadoOcupacion } from '@type/ope/administracion/ope-estado-ocupacion.type';

@Injectable({ providedIn: 'root' })
export class OpeEstadosOcupacionService {
  public http = inject(HttpClient);
  public endpoint = '/ope-estados-ocupacion';

  get() {
    return firstValueFrom(this.http.get<OpeEstadoOcupacion[]>(this.endpoint).pipe((response) => response));
  }
}
