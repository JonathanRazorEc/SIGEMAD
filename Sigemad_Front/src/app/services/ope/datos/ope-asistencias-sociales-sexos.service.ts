import { HttpClient } from '@angular/common/http';
import { Injectable, inject } from '@angular/core';
import { OpeAsistenciaSocialSexo } from '@type/ope/datos/ope-asistencia-social-sexo.type';
import { firstValueFrom } from 'rxjs';

@Injectable({ providedIn: 'root' })
export class OpeAsistenciasSocialesSexosService {
  public http = inject(HttpClient);
  public endpoint = '/ope-asistencias-sociales-sexos';

  get() {
    return firstValueFrom(this.http.get<OpeAsistenciaSocialSexo[]>(this.endpoint).pipe((response) => response));
  }
}
