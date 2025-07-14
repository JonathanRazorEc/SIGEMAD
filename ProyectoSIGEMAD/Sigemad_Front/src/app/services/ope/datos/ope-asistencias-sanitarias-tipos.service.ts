import { HttpClient } from '@angular/common/http';
import { Injectable, inject } from '@angular/core';
import { firstValueFrom } from 'rxjs';
import { OpeAsistenciaSanitariaTipo } from '@type/ope/datos/ope-asistencia-sanitaria-tipo.type';

@Injectable({ providedIn: 'root' })
export class OpeAsistenciasSanitariasTiposService {
  public http = inject(HttpClient);
  public endpoint = '/ope-asistencias-sanitarias-tipos';

  get() {
    return firstValueFrom(this.http.get<OpeAsistenciaSanitariaTipo[]>(this.endpoint).pipe((response) => response));
  }
}
