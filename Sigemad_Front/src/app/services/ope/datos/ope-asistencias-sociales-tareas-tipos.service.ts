import { HttpClient } from '@angular/common/http';
import { Injectable, inject } from '@angular/core';
import { firstValueFrom } from 'rxjs';
import { OpeAsistenciaSocialTareaTipo } from '@type/ope/datos/ope-asistencia-social-tarea-tipo.type';

@Injectable({ providedIn: 'root' })
export class OpeAsistenciasSocialesTareasTiposService {
  public http = inject(HttpClient);
  public endpoint = '/ope-asistencias-sociales-tareas-tipos';

  get() {
    return firstValueFrom(this.http.get<OpeAsistenciaSocialTareaTipo[]>(this.endpoint).pipe((response) => response));
  }
}
