import { HttpClient } from '@angular/common/http';
import { Injectable, inject } from '@angular/core';
import { catchError, firstValueFrom, map, throwError } from 'rxjs';

@Injectable({ providedIn: 'root' })
export class FireDocumentationService {
  public http = inject(HttpClient);

  getById(id: Number) {
    //idRegistroActualizacion
    let endpoint = `/Documentaciones/?idSuceso=${id}`;

    return firstValueFrom(this.http.get<any[]>(endpoint).pipe((response) => response));
  }

  getByIdRegistro(id: Number, idRegistro: Number) {
    //idRegistroActualizacion
    let endpoint = `/Documentaciones/?idSuceso=${id}&idRegistroActualizacion=${idRegistro}`;
    return firstValueFrom(this.http.get<any[]>(endpoint).pipe((response) => response));
  }

  post(data: any) {
    const endpoint = '/Documentaciones/lista';
  
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

  getFile(id: string): Promise<Blob> {
    const endpoint = `/Archivos/${id}/contenido`;
  
    return firstValueFrom(
      this.http.get(endpoint, { responseType: 'blob' }).pipe(
        map((response: Blob) => {
          return response;
        }),
        catchError((error) => {
          return throwError(() => error.error);
        })
      )
    );
  }

  //delete
  delete(id: number) {
    const endpoint = `/Documentaciones/${id}`;

    return firstValueFrom(
      this.http.delete(endpoint).pipe((response) => response)
    );
  }
}
