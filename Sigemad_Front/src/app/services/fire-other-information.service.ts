import { DatePipe } from '@angular/common';
import { HttpClient } from '@angular/common/http';
import { Injectable, inject } from '@angular/core';
import { catchError, firstValueFrom, map, throwError } from 'rxjs';

@Injectable({ providedIn: 'root' })
export class FireOtherInformationService {
  public http = inject(HttpClient);
  public datepipe = inject(DatePipe);

  getById(id: Number) {
    let endpoint = `/otras-informaciones/?idSuceso=${id}`;
    return firstValueFrom(this.http.get<any[]>(endpoint).pipe((response) => response));
  }

  getByIdRegistro(id: Number, registro: Number) {
    let endpoint = `/otras-informaciones/?idSuceso=${id}&idRegistroActualizacion=${registro}`;
    return firstValueFrom(this.http.get<any[]>(endpoint).pipe((response) => response));
  }

  post(data: any) {
    const endpoint = '/otras-informaciones/lista';

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
  //delete
  delete(id: number) {
    const endpoint = `/otras-informaciones/${id}`;

    return firstValueFrom(
      this.http.delete(endpoint).pipe((response) => response)
    );
  }
}
