import { CommonModule } from '@angular/common';
import { Component, inject, signal } from '@angular/core';
import { RouterOutlet } from '@angular/router';
import { OpePuntosControlCarreterasTableComponent } from './components/ope-punto-control-carretera-table/ope-puntos-control-carreteras-table.component';
import { ApiResponse } from '../../../../types/api-response.type';
import { OpePuntoControlCarreteraFilterFormComponent } from './components/ope-punto-control-carretera-filter-form/ope-puntos-control-carreteras-filter-form.component';
import { OpePuntoControlCarretera } from '../../../../types/ope/administracion/ope-punto-control-carretera.type';
import { LocalFiltrosOpePuntosControlCarreteras } from '@services/ope/administracion/local-filtro-ope-puntos-control-carreteras.service';
import { OpePuntosControlCarreterasService } from '@services/ope/administracion/ope-puntos-control-carreteras.service';

@Component({
  selector: 'app-ope-puntos-control-carreteras',
  standalone: true,
  imports: [CommonModule, RouterOutlet, OpePuntosControlCarreterasTableComponent, OpePuntoControlCarreteraFilterFormComponent],
  templateUrl: './ope-puntos-control-carreteras.component.html',
  styleUrl: './ope-puntos-control-carreteras.component.scss',
})
export class OpePuntosControlCarreterasComponent {
  public opePuntosControlCarreterasService = inject(OpePuntosControlCarreterasService);
  public filtrosOpePuntosControlCarreterasService = inject(LocalFiltrosOpePuntosControlCarreteras);

  public filtros = signal<any>({});

  public isLoading = true;
  public refreshFilterForm = true;
  public childActivated = false;

  public opePuntosControlCarreteras: ApiResponse<OpePuntoControlCarretera[]> = {
    count: 0,
    page: 1,
    pageSize: 10,
    data: [],
    pageCount: 0,
  };

  async ngOnInit() {
    //const fires = await this.fireService.get();
    //this.fires = fires;
    this.filtros.set(this.filtrosOpePuntosControlCarreterasService.getFilters());
  }
}
