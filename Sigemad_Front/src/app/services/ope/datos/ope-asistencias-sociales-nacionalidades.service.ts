import { HttpClient } from '@angular/common/http';
import { Injectable, inject } from '@angular/core';
import { OpeAsistenciaSocialNacionalidad } from '@type/ope/datos/ope-asistencia-social-nacionalidad.type';
import { firstValueFrom } from 'rxjs';

@Injectable({ providedIn: 'root' })
export class OpeAsistenciasSocialesNacionalidadesService {
  public http = inject(HttpClient);
  public endpoint = '/ope-asistencias-sociales-nacionalidades';

  get() {
    return firstValueFrom(this.http.get<OpeAsistenciaSocialNacionalidad[]>(this.endpoint).pipe((response) => response));
  }
}
