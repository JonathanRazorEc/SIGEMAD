import { Injectable } from '@angular/core';

@Injectable({ providedIn: 'root' })
export class LocalFiltrosOpeDatosFronteras {
  filtros: any = {};

  getFilters() {
    return this.filtros;
  }

  setFilters(filtros: any) {
    this.filtros = filtros;
  }
}
