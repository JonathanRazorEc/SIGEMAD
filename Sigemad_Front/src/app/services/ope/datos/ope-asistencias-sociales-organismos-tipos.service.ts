import { HttpClient } from '@angular/common/http';
import { Injectable, inject } from '@angular/core';
import { firstValueFrom } from 'rxjs';
import { OpeAsistenciaSocialOrganismoTipo } from '@type/ope/datos/ope-asistencia-social-organismo-tipo.type';

@Injectable({ providedIn: 'root' })
export class OpeAsistenciasSocialesOrganismosTiposService {
  public http = inject(HttpClient);
  public endpoint = '/ope-asistencias-sociales-organismos-tipos';

  get() {
    return firstValueFrom(this.http.get<OpeAsistenciaSocialOrganismoTipo[]>(this.endpoint).pipe((response) => response));
  }
}
