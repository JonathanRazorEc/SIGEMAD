import { HttpClient } from '@angular/common/http';
import { Injectable, inject } from '@angular/core';
import { catchError, firstValueFrom, map, throwError } from 'rxjs';

@Injectable({ providedIn: 'root' })
export class AreasAffectedService {
  private http = inject(HttpClient);

  getByEvolution(id: Number) {
    let endpoint = `/areas-afectadas/evolucion/${id}`;

    return firstValueFrom(this.http.get<any[]>(endpoint).pipe((response) => response));
  }

  post(body: any) {
    const endpoint = `/areas-afectadas`;

    return firstValueFrom(
      this.http.post(endpoint, body).pipe(
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
