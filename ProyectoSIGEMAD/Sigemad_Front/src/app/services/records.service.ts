import { HttpClient } from '@angular/common/http';
import { Injectable, inject } from '@angular/core';
import { catchError, firstValueFrom, map, throwError } from 'rxjs';

@Injectable({ providedIn: 'root' })
export class RecordsService {
  private http = inject(HttpClient);

  getByEvolution(id: Number) {
    let endpoint = `/registros/evolucion/${id}`;
    return firstValueFrom(this.http.get<any[]>(endpoint).pipe((response) => response));
  }

  getById(id: number) {
    const endpoint = `/registros/${id}`;
    return firstValueFrom(this.http.get<any>(endpoint).pipe(
      map((response) => {
        return response;
      }),
      catchError((error) => {
        return throwError(error.error);
      })
    ));
  }
  
  getRegistrosAnteriores(id: number, idRegistro: number) {
    const endpoint = `/registros-anteriores?idSuceso=${id}&IdRegistro=${idRegistro}`;
    return firstValueFrom(this.http.get<any>(endpoint).pipe(
      map((response) => {
        return response;
      }),
      catchError((error) => {
        return throwError(error.error);
      })
    ));
  }

  getAll(id: number) {
    const endpoint = `/registros/${id}`;
    return firstValueFrom(this.http.get<any>(endpoint).pipe(
      map((response) => {
        return response;
      }),
      catchError((error) => {
        return throwError(error.error);
      })
    ));
  }

  post(body: any) {
    const endpoint = `/registros`;

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
  
  delete(id: number) {
    const endpoint = `/registros/${id}`;
    
    return firstValueFrom(
      this.http.delete(endpoint).pipe(
        map((response) => {
          return response;
        }),
        catchError((error) => {
          return throwError(error.error);
        })
      )
    );
  }
  
  getFaseNivelSituacion(idPlan: number, idSituacion: number) {
    const endpoint
      = `/plan-situacion-emergencia/fase-nivel-situacion?idPlanEmergencia=${ idPlan }&situacionEquivalente=${ idSituacion }`;

    return firstValueFrom(this.http.get<any>(endpoint).pipe(
      map((response) => {
        return response;
      }),
      catchError((error) => {
        return throwError(error.error);
      })
    ));
  }
} 