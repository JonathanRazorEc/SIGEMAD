import { Injectable } from '@angular/core';

@Injectable({ providedIn: 'root' })
export class ConfigLoaderService {
  load(): Promise<void> {
    return fetch('/assets/config.json')
      .then((res) => {
        if (!res.ok) throw new Error('Error al cargar config.json');
        return res.json();
      })
      .then((config) => {
        (window as any)['runtimeConfig'] = config;
      })
      .catch((err) => {
        (window as any)['runtimeConfig'] = {}; 
      });
  }
}
