import { HttpClient } from '@angular/common/http';
import { Injectable, inject } from '@angular/core';
import { firstValueFrom } from 'rxjs';
import { OpeOcupacion } from '@type/ope/administracion/ope-ocupacion.type';

@Injectable({ providedIn: 'root' })
export class OpeOcupacionesService {
  public http = inject(HttpClient);
  public endpoint = '/ope-ocupaciones';

  get() {
    return firstValueFrom(this.http.get<OpeOcupacion[]>(this.endpoint).pipe((response) => response));
  }
}
