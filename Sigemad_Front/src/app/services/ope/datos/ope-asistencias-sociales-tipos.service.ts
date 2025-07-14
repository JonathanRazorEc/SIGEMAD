import { HttpClient } from '@angular/common/http';
import { Injectable, inject } from '@angular/core';
import { firstValueFrom } from 'rxjs';
import { OpeAsistenciaSocialTipo } from '@type/ope/datos/ope-asistencia-social-tipo.type';

@Injectable({ providedIn: 'root' })
export class OpeAsistenciasSocialesTiposService {
  public http = inject(HttpClient);
  public endpoint = '/ope-asistencias-sociales-tipos';

  get() {
    return firstValueFrom(this.http.get<OpeAsistenciaSocialTipo[]>(this.endpoint).pipe((response) => response));
  }
}
