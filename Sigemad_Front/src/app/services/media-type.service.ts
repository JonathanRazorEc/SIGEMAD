import { HttpClient } from '@angular/common/http';
import { Injectable, inject } from '@angular/core';
import { firstValueFrom } from 'rxjs';
import { MediaType } from '../types/media-type.type';

@Injectable({ providedIn: 'root' })
export class MediaTypeService {
  private http = inject(HttpClient);

  get() {
    const endpoint = '/tipo-intervencion-medios';

    return firstValueFrom(this.http.get<MediaType[]>(endpoint).pipe((response) => response));
  }
}
