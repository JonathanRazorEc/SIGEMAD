import { HttpClient } from '@angular/common/http';
import { Injectable, inject } from '@angular/core';
import { firstValueFrom } from 'rxjs';
import { OpeDatoFronteraIntervaloHorario } from '@type/ope/datos/ope-dato-frontera-intervalo-horario.type';

@Injectable({ providedIn: 'root' })
export class OpeDatosFronterasIntervalosHorariosService {
  public http = inject(HttpClient);
  public endpoint = '/ope-datos-fronteras-intervalos-horarios';

  get() {
    return firstValueFrom(this.http.get<OpeDatoFronteraIntervaloHorario[]>(this.endpoint).pipe((response) => response));
  }
}
