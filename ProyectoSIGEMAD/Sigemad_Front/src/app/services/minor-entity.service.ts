import { HttpClient } from '@angular/common/http';
import { Injectable, inject } from '@angular/core';
import { firstValueFrom } from 'rxjs';
import { MinorEntity } from '../types/minor-entity.type';

@Injectable({ providedIn: 'root' })
export class MinorEntityService {
  private http = inject(HttpClient);

  get(muni_id: number | string) {
    const endpoint = `/Municipios/${muni_id}/entidades-menores`;

    return firstValueFrom(this.http.get<MinorEntity[]>(endpoint).pipe((response) => response));
  }
}
