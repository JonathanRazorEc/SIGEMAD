import { DatePipe } from '@angular/common';
import { HttpClient } from '@angular/common/http';
import { Injectable, inject } from '@angular/core';
import { catchError, firstValueFrom, map, throwError } from 'rxjs';

@Injectable({ providedIn: 'root' })
export class OpeDatosAsistenciasSanitariasService {
  public http = inject(HttpClient);
  public datepipe = inject(DatePipe);

  getByIdOpeDatoAsistencia(id: Number) {
    let endpoint = `/ope-datos-asistencias-sanitarias/?idOpeDatoAsistencia=${id}`;
    return firstValueFrom(this.http.get<any[]>(endpoint).pipe((response) => response));
  }

  post(data: any) {
    const endpoint = '/ope-datos-asistencias-sanitariass/lista';

    return firstValueFrom(
      this.http.post(endpoint, data).pipe(
        map((response) => {
          return response;
        }),
        catchError((error) => {
          return throwError(error.error);
        })
      )
    );
  }
}
