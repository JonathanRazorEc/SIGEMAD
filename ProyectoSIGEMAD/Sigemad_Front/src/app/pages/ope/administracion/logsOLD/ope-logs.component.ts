import { CommonModule } from '@angular/common';
import { Component, inject, signal } from '@angular/core';
import { RouterOutlet } from '@angular/router';
import { OpeLogsTableComponent } from './components/ope-log-table/ope-logs-table.component';
import { ApiResponse } from '../../../../types/api-response.type';
import { OpeLog } from '../../../../types/ope/administracion/ope-log.type';
import { OpeLogFilterFormComponent } from './components/ope-log-filter-form/ope-logs-filter-form.component';
import { LocalFiltrosOpeLogs } from '@services/ope/administracion/local-filtro-ope-logs.service';
import { OpeLogsService } from '@services/ope/administracion/ope-logs.service';

@Component({
  selector: 'app-ope-logs',
  standalone: true,
  imports: [CommonModule, RouterOutlet, OpeLogsTableComponent, OpeLogFilterFormComponent],
  templateUrl: './ope-logs.component.html',
  styleUrl: './ope-logs.component.scss',
})
export class OpeLogsComponent {
  public opeLogsService = inject(OpeLogsService);
  public filtrosOpeLogsService = inject(LocalFiltrosOpeLogs);

  public filtros = signal<any>({});

  public isLoading = true;
  public refreshFilterForm = true;
  public childActivated = false;

  public opeLogs: ApiResponse<OpeLog[]> = {
    count: 0,
    page: 1,
    pageSize: 10,
    data: [],
    pageCount: 0,
  };

  async ngOnInit() {
    //const fires = await this.fireService.get();
    //this.fires = fires;
    this.filtros.set(this.filtrosOpeLogsService.getFilters());
  }
}
