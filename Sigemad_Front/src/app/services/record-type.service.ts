import { HttpClient } from '@angular/common/http';
import { Injectable, inject } from '@angular/core';
import { firstValueFrom } from 'rxjs';
import { RecordType } from '../types/record-type.type';

@Injectable({ providedIn: 'root' })
export class RecordTypeService {
  private http = inject(HttpClient);

  get() {
    const endpoint = '/tipos-registros';

    return firstValueFrom(this.http.get<RecordType[]>(endpoint).pipe((response) => response));
  }
}
