import { CommonModule } from '@angular/common';
import { Component, inject, signal } from '@angular/core';
import { RouterOutlet } from '@angular/router';
import { OpePorcentajesOcupacionAreasEstacionamientoTableComponent } from './components/ope-porcent-ocup-area-est-table/ope-porcentajes-ocupacion-areas-estacionamiento-table.component';
import { ApiResponse } from '../../../../types/api-response.type';
import { OpePorcentajeOcupacionAreaEstacionamientoFilterFormComponent } from './components/ope-porcent-ocup-area-est-filter-form/ope-porcentajes-ocupacion-areas-estacionamiento-filter-form.component';
import { LocalFiltrosOpePorcentajesOcupacionAreasEstacionamiento } from '../../../../services/ope/administracion/local-filtro-ope-porcentajes-ocupacion-areas-estacionamiento.service';
import { OpePorcentajeOcupacionAreaEstacionamiento } from '../../../../types/ope/administracion/ope-porcentaje-ocupacion-area-estacionamiento.type';
import { OpePorcentajesOcupacionAreasEstacionamientoService } from '../../../../services/ope/administracion/ope-porcentajes-ocupacion-areas-estacionamiento.service';

@Component({
  selector: 'app-ope-porcentajes-ocupacion-areas-estacionamiento',
  standalone: true,
  imports: [
    CommonModule,
    RouterOutlet,
    OpePorcentajesOcupacionAreasEstacionamientoTableComponent,
    OpePorcentajeOcupacionAreaEstacionamientoFilterFormComponent,
  ],
  templateUrl: './ope-porcentajes-ocupacion-areas-estacionamiento.component.html',
  styleUrl: './ope-porcentajes-ocupacion-areas-estacionamiento.component.scss',
})
export class OpePorcentajesOcupacionAreasEstacionamientoComponent {
  public opePorcentajesOcupacionAreasEstacionamientoService = inject(OpePorcentajesOcupacionAreasEstacionamientoService);
  public filtrosOpePorcentajesOcupacionAreasEstacionamientoService = inject(LocalFiltrosOpePorcentajesOcupacionAreasEstacionamiento);

  public filtros = signal<any>({});

  public isLoading = true;
  public refreshFilterForm = true;
  public childActivated = false;

  public opePorcentajesOcupacionAreasEstacionamiento: ApiResponse<OpePorcentajeOcupacionAreaEstacionamiento[]> = {
    count: 0,
    page: 1,
    pageSize: 10,
    data: [],
    pageCount: 0,
  };

  async ngOnInit() {
    //const fires = await this.fireService.get();
    //this.fires = fires;
    this.filtros.set(this.filtrosOpePorcentajesOcupacionAreasEstacionamientoService.getFilters());
  }
}
