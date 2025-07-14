import { HttpClient } from '@angular/common/http';
import { Injectable, inject } from '@angular/core';
import { Menu } from '@type/menu.types';
import { firstValueFrom } from 'rxjs';

@Injectable({ providedIn: 'root' })
export class OpeMenuService {
  private http = inject(HttpClient);

  get() {
    const endpoint = '/ope-menus';

    return firstValueFrom(this.http.get<Menu[]>(endpoint).pipe((response) => response));
  }
}
