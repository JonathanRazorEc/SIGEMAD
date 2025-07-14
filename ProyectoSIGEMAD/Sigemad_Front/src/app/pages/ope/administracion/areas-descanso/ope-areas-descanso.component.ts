import { CommonModule } from '@angular/common';
import { Component, inject, signal } from '@angular/core';
import { RouterOutlet } from '@angular/router';
import { OpeAreasDescansoTableComponent } from './components/ope-area-descanso-table/ope-areas-descanso-table.component';
import { ApiResponse } from '../../../../types/api-response.type';
import { OpeAreaDescansoFilterFormComponent } from './components/ope-area-descanso-filter-form/ope-areas-descanso-filter-form.component';
import { LocalFiltrosOpeAreasDescanso } from '../../../../services/ope/administracion/local-filtro-ope-areas-descanso.service';
import { OpeAreaDescanso } from '../../../../types/ope/administracion/ope-area-descanso.type';
import { OpeAreasDescansoService } from '@services/ope/administracion/ope-areas-descanso.service';

@Component({
  selector: 'app-ope-areas-descanso',
  standalone: true,
  imports: [CommonModule, RouterOutlet, OpeAreasDescansoTableComponent, OpeAreaDescansoFilterFormComponent],
  templateUrl: './ope-areas-descanso.component.html',
  styleUrl: './ope-areas-descanso.component.scss',
})
export class OpeAreasDescansoComponent {
  public opeAreasDescansoService = inject(OpeAreasDescansoService);
  public filtrosOpeAreasDescansoService = inject(LocalFiltrosOpeAreasDescanso);

  public filtros = signal<any>({});

  public isLoading = true;
  public refreshFilterForm = true;
  public childActivated = false;

  public opeAreasDescanso: ApiResponse<OpeAreaDescanso[]> = {
    count: 0,
    page: 1,
    pageSize: 10,
    data: [],
    pageCount: 0,
  };

  async ngOnInit() {
    //const fires = await this.fireService.get();
    //this.fires = fires;
    this.filtros.set(this.filtrosOpeAreasDescansoService.getFilters());
  }
}
