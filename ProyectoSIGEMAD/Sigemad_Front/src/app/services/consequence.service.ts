import { HttpClient } from '@angular/common/http';
import { Injectable, inject } from '@angular/core';
import { firstValueFrom } from 'rxjs';
import { Impact } from '../types/impact.type';
import { environment } from 'src/environments/environment';

@Injectable({ providedIn: 'root' })
export class ConsequenceService {
  private http = inject(HttpClient);

  getGrupo(tipo: string) {
    const endpoint = `/grupos-impactos?Tipo=${tipo}`;

    return firstValueFrom(this.http.get<Impact[]>(endpoint).pipe((response) => response));
  }

  getDenominaciones(tipo: string, grupo: string) {
    const endpoint = `/impactos?Tipo=${tipo}&Grupo=${grupo}`;

    return firstValueFrom(this.http.get<Impact[]>(endpoint).pipe((response) => response));
  }

  getCamposImpacto(idImpacto: string) {
    const endpoint = `/campos-impactos/${idImpacto}`;

    return firstValueFrom(this.http.get<Impact[]>(endpoint).pipe((response) => response));
  }

  getImpactosClasificados(tipoId?: number) {
    const endpoint = tipoId 
      ? `/impactos-clasificados?idTipoImpacto=${tipoId}`
      : '/impactos-clasificados';

    return firstValueFrom(this.http.get<any[]>(endpoint).pipe((response) => response));
  }
  
  getValidacionImpactoClasificado(descripcionId: number) {
    const endpoint = `/validacion-impacto-clasificado/${descripcionId}`;

    return firstValueFrom(this.http.get<any[]>(endpoint).pipe((response) => response));
  }
}
