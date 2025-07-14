import { HttpClient } from '@angular/common/http';
import { Injectable, inject } from '@angular/core';
import { firstValueFrom } from 'rxjs';
import { Menu } from '../types/menu.types';

@Injectable({ providedIn: 'root' })
export class MenuService {
  private http = inject(HttpClient);

  get() {
    const endpoint = '/Menus';

    return firstValueFrom(this.http.get<Menu[]>(endpoint).pipe((response) => response));
  }
}
