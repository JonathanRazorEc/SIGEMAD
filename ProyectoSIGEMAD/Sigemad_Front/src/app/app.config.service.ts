import { Injectable } from '@angular/core';

@Injectable({ providedIn: 'root' })
export class ConfigService {
  private config: any;
  private defaultBaseUrl: string;

  constructor() {
    this.config = (window as any)['runtimeConfig'] || {};
    const host = window.location.hostname;
    // Si estamos en localhost o en develop.sigemad.duckdns.org usamos el baseUrl de develop
    if (host === 'localhost' || host === '127.0.0.1' || host === 'develop.sigemad.duckdns.org') {
      //this.defaultBaseUrl = 'https://develop.sigemad.duckdns.org';
      this.defaultBaseUrl = 'https://localhost:7185';
    } else {
      this.defaultBaseUrl = window.location.origin;
    }
  }

  get urlGeoserver(): string {
    return this.config?.urlGeoserver || '/geoserver';
  }

  get apiUrl(): string {
    return this.config?.apiUrl || '/api/v1';
  }

  get baseUrl(): string {
    // Si en runtimeConfig viene un baseUrl personalizado, lo usamos
    return this.config?.baseUrl || this.defaultBaseUrl;
  }

  get fullApiUrl(): string {
    return `${this.baseUrl}${this.apiUrl}`;
  }

  // Método para obtener cualquier configuración genérica
  getConfig(key: string): any {
    return this.config?.[key];
  }
}
