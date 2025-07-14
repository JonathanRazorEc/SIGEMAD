import { HttpClient } from '@angular/common/http';
import { Injectable, inject } from '@angular/core';
import { firstValueFrom } from 'rxjs';
import { MediaOwnership } from '../types/media-ownership.type';

@Injectable({ providedIn: 'root' })
export class MediaOwnershipService {
  private http = inject(HttpClient);

  get() {
    const endpoint = '/titular-medios';

    return firstValueFrom(this.http.get<MediaOwnership[]>(endpoint).pipe((response) => response));
  }
}
