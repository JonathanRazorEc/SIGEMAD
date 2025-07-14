import { CommonModule } from '@angular/common';
import { Component, inject, signal } from '@angular/core';
import { RouterOutlet } from '@angular/router';
import { ApiResponse } from '../../../../types/api-response.type';
import { OpeDatosEmbarquesDiariosService } from '@services/ope/datos/ope-datos-embarques-diarios.service';
import { LocalFiltrosOpeDatosEmbarquesDiarios } from '@services/ope/datos/local-filtro-ope-datos-embarques-diarios.service';
import { OpeDatosEmbarquesDiariosTableComponent } from './components/ope-dato-embarque-diario-table/ope-datos-embarques-diarios-table.component';
import { OpeDatoEmbarqueDiarioFilterFormComponent } from './components/ope-dato-embarque-diario-filter-form/ope-datos-embarques-diarios-filter-form.component';
import { OpeDatoEmbarqueDiario } from '@type/ope/datos/ope-dato-embarque-diario.type';

@Component({
  selector: 'app-ope-datos-embarques-diarios',
  standalone: true,
  imports: [CommonModule, RouterOutlet, OpeDatosEmbarquesDiariosTableComponent, OpeDatoEmbarqueDiarioFilterFormComponent],
  templateUrl: './ope-datos-embarques-diarios.component.html',
  styleUrl: './ope-datos-embarques-diarios.component.scss',
})
export class OpeDatosEmbarquesDiariosComponent {
  public opeDatosEmbarquesDiariosService = inject(OpeDatosEmbarquesDiariosService);
  public filtrosOpeDatosEmbarquesDiariosService = inject(LocalFiltrosOpeDatosEmbarquesDiarios);

  public filtros = signal<any>({});

  public isLoading = true;
  public refreshFilterForm = true;
  public childActivated = false;

  public opeDatosEmbarquesDiarios: ApiResponse<OpeDatoEmbarqueDiario[]> = {
    count: 0,
    page: 1,
    pageSize: 10,
    data: [],
    pageCount: 0,
  };

  async ngOnInit() {
    //const fires = await this.fireService.get();
    //this.fires = fires;
    this.filtros.set(this.filtrosOpeDatosEmbarquesDiariosService.getFilters());
  }
}
