import { CommonModule } from '@angular/common';
import { Component, inject, signal } from '@angular/core';
import { RouterOutlet } from '@angular/router';
import { OpeAreasEstacionamientoTableComponent } from './components/ope-area-estacionamiento-table/ope-areas-estacionamiento-table.component';
import { ApiResponse } from '../../../../types/api-response.type';
import { OpeAreaEstacionamientoFilterFormComponent } from './components/ope-area-estacionamiento-filter-form/ope-areas-estacionamiento-filter-form.component';
import { LocalFiltrosOpeAreasEstacionamiento } from '../../../../services/ope/administracion/local-filtro-ope-areas-estacionamiento.service';
import { OpeAreasEstacionamientoService } from '@services/ope/administracion/ope-areas-estacionamiento.service';
import { OpeAreaEstacionamiento } from '@type/ope/administracion/ope-area-estacionamiento.type';

@Component({
  selector: 'app-ope-areas-estacionamiento',
  standalone: true,
  imports: [CommonModule, RouterOutlet, OpeAreasEstacionamientoTableComponent, OpeAreaEstacionamientoFilterFormComponent],
  templateUrl: './ope-areas-estacionamiento.component.html',
  styleUrl: './ope-areas-estacionamiento.component.scss',
})
export class OpeAreasEstacionamientoComponent {
  public opeAreasEstacionamientoService = inject(OpeAreasEstacionamientoService);
  public filtrosOpeAreasEstacionamientoService = inject(LocalFiltrosOpeAreasEstacionamiento);

  public filtros = signal<any>({});

  public isLoading = true;
  public refreshFilterForm = true;
  public childActivated = false;

  public opeAreasEstacionamiento: ApiResponse<OpeAreaEstacionamiento[]> = {
    count: 0,
    page: 1,
    pageSize: 10,
    data: [],
    pageCount: 0,
  };

  async ngOnInit() {
    //const fires = await this.fireService.get();
    //this.fires = fires;
    this.filtros.set(this.filtrosOpeAreasEstacionamientoService.getFilters());
  }
}
