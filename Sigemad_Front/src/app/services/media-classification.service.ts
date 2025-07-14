import { HttpClient } from '@angular/common/http';
import { Injectable, inject } from '@angular/core';
import { firstValueFrom } from 'rxjs';
import { MediaClassification } from '../types/media-classification.type';

@Injectable({ providedIn: 'root' })
export class MediaClassificationService {
  private http = inject(HttpClient);

  get() {
    const endpoint = '/clasificacion-medios';

    return firstValueFrom(this.http.get<MediaClassification[]>(endpoint).pipe((response) => response));
  }
}
