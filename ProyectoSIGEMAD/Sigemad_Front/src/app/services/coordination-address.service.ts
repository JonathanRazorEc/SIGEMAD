import { HttpClient } from '@angular/common/http';
import { Injectable, inject, signal } from '@angular/core';
import { catchError, firstValueFrom, map, throwError } from 'rxjs';

interface FormTypeCecopi {
  id?: string;
  idIncendio?: number;
  fechaInicio: Date;
  fechaFin: Date;
  provincia: { id: number; descripcion: string };
  municipio: { id: number; descripcion: string };
  lugar?: string;
  observaciones?: string;
  geoPosicion?: any;
}

interface FormTypeAddress {
  id?: string;
  autoridadQueDirige: string;
  idIncendio?: number;
  fechaInicio: Date;
  fechaFin: Date;
  tipoDireccionEmergencia: { id: number; descripcion: string };
  geoPosicion?: any;
  observaciones?: any;
}

interface FormTypePma {
  id?: string;
  autoridadQueDirige: string;
  idIncendio?: number;
  fechaInicio: Date;
  fechaFin: Date;
  tipoDireccionEmergencia: number;
  provincia: { id: number; descripcion: string };
  municipio: { id: number; descripcion: string };
  lugar?: string;
  geoPosicion?: any;
  observaciones?: any;
}

@Injectable({ providedIn: 'root' })
export class CoordinationAddressService {
  private http = inject(HttpClient);
  public dataCecopi = signal<FormTypeCecopi[]>([]);
  public dataCoordinationAddress = signal<FormTypeAddress[]>([]);
  public dataPma = signal<FormTypePma[]>([]);

  clearData(): void {
    this.dataCecopi.set([]);
    this.dataCoordinationAddress.set([]);
    this.dataPma.set([]);
  }

  postAddress(body: any) {
    const endpoint = `/direcciones-coordinaciones-emergencias/direcciones`;

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

  postCecopi(body: any) {
    const endpoint = `/direcciones-coordinaciones-emergencias/coordinaciones-cecopi`;

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

  postPma(body: any) {
    const endpoint = `/direcciones-coordinaciones-emergencias/coordinaciones-pma`;

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

  getById(id: Number) {
    let endpoint = `/direcciones-coordinaciones-emergencias?idSuceso=${id}`;
    return firstValueFrom(this.http.get<any[]>(endpoint).pipe((response) => response));
  }

  getByIdRegistro(id: Number, registro: Number) {
    let endpoint = `/direcciones-coordinaciones-emergencias?idSuceso=${id}&idRegistroActualizacion=${registro}`;
    return firstValueFrom(this.http.get<any[]>(endpoint).pipe((response) => response));
  }

  delete(id: number) {
    const endpoint = `/direcciones-coordinaciones-emergencias/${id}`;

    return firstValueFrom(this.http.delete(endpoint).pipe((response) => response));
  }
}
