import { HttpClient } from '@angular/common/http';
import { Injectable, inject } from '@angular/core';
import { OpeAsistenciaSocialEdad } from '@type/ope/datos/ope-asistencia-social-edad.type';
import { firstValueFrom } from 'rxjs';

@Injectable({ providedIn: 'root' })
export class OpeAsistenciasSocialesEdadesService {
  public http = inject(HttpClient);
  public endpoint = '/ope-asistencias-sociales-edades';

  get() {
    return firstValueFrom(this.http.get<OpeAsistenciaSocialEdad[]>(this.endpoint).pipe((response) => response));
  }
}
