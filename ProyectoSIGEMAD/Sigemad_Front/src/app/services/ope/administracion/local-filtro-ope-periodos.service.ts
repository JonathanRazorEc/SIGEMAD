import { Injectable } from '@angular/core';

@Injectable({ providedIn: 'root' })
export class LocalFiltrosOpePeriodos {
  filtros: any = {};

  getFilters() {
    return this.filtros;
  }

  setFilters(filtros: any) {
    this.filtros = filtros;
  }
}
