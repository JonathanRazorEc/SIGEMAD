import { CommonModule } from '@angular/common';
import { Component, inject, signal } from '@angular/core';
import { RouterOutlet } from '@angular/router';
import { OpePeriodosTableComponent } from './components/ope-periodo-table/ope-periodos-table.component';
import { ApiResponse } from '../../../../types/api-response.type';
import { OpePeriodo } from '../../../../types/ope/administracion/ope-periodo.type';
import { OpePeriodoFilterFormComponent } from './components/ope-periodo-filter-form/ope-periodos-filter-form.component';
import { LocalFiltrosOpePeriodos } from '@services/ope/administracion/local-filtro-ope-periodos.service';
import { OpePeriodosService } from '@services/ope/administracion/ope-periodos.service';

@Component({
  selector: 'app-ope-periodos',
  standalone: true,
  imports: [CommonModule, RouterOutlet, OpePeriodosTableComponent, OpePeriodoFilterFormComponent],
  templateUrl: './ope-periodos.component.html',
  styleUrl: './ope-periodos.component.scss',
})
export class OpePeriodosComponent {
  public opePeriodosService = inject(OpePeriodosService);
  public filtrosOpePeriodosService = inject(LocalFiltrosOpePeriodos);

  public filtros = signal<any>({});

  public isLoading = true;
  public refreshFilterForm = true;
  public childActivated = false;

  public opePeriodos: ApiResponse<OpePeriodo[]> = {
    count: 0,
    page: 1,
    pageSize: 10,
    data: [],
    pageCount: 0,
  };

  async ngOnInit() {
    //const fires = await this.fireService.get();
    //this.fires = fires;
    this.filtros.set(this.filtrosOpePeriodosService.getFilters());
  }
}
