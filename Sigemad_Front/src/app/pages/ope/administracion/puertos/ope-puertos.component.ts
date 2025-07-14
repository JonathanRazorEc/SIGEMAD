import { CommonModule } from '@angular/common';
import { Component, inject, signal } from '@angular/core';
import { RouterOutlet } from '@angular/router';
import { OpePuertosTableComponent } from './components/ope-puerto-table/ope-puertos-table.component';
import { ApiResponse } from '../../../../types/api-response.type';
import { OpePuertoFilterFormComponent } from './components/ope-puerto-filter-form/ope-puertos-filter-form.component';
import { LocalFiltrosOpePuertos } from '../../../../services/ope/administracion/local-filtro-ope-puertos.service';
import { OpePuerto } from '../../../../types/ope/administracion/ope-puerto.type';
import { OpePuertosService } from '../../../../services/ope/administracion/ope-puertos.service';
import { OpePais } from '@type/ope/administracion/ope-pais.type';
import { OpePaisesService } from '@services/ope/administracion/ope-paises.service';

@Component({
  selector: 'app-ope-puertos',
  standalone: true,
  imports: [CommonModule, RouterOutlet, OpePuertosTableComponent, OpePuertoFilterFormComponent],
  templateUrl: './ope-puertos.component.html',
  styleUrl: './ope-puertos.component.scss',
})
export class OpePuertosComponent {
  public opePuertosService = inject(OpePuertosService);
  private opePaisesService = inject(OpePaisesService);
  public filtrosOpePuertosService = inject(LocalFiltrosOpePuertos);

  public filtros = signal<any>({});

  public isLoading = true;
  public refreshFilterForm = true;
  public childActivated = false;

  public opePuertos: ApiResponse<OpePuerto[]> = {
    count: 0,
    page: 1,
    pageSize: 10,
    data: [],
    pageCount: 0,
  };

  //
  public opePuertosPaises: OpePais[] = [];
  //

  async ngOnInit() {
    //const fires = await this.fireService.get();
    //this.fires = fires;
    this.filtros.set(this.filtrosOpePuertosService.getFilters());

    //
    await this.cargarOpePuertosPaises();
    //
  }

  async cargarOpePuertosPaises() {
    const opePuertosPaises = await this.opePaisesService.get({
      opePuertos: true,
    });
    this.opePuertosPaises = opePuertosPaises;
  }
}
