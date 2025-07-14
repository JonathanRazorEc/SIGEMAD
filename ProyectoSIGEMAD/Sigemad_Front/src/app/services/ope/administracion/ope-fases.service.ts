import { HttpClient } from '@angular/common/http';
import { Injectable, inject } from '@angular/core';
import { firstValueFrom } from 'rxjs';
import { OpeFase } from '@type/ope/administracion/ope-fase.type';

@Injectable({ providedIn: 'root' })
export class OpeFasesService {
  public http = inject(HttpClient);
  public endpoint = '/ope-fases';

  get() {
    return firstValueFrom(this.http.get<OpeFase[]>(this.endpoint));
  }
}
