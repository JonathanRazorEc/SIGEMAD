import { CommonModule } from '@angular/common';
import { Component, inject, signal } from '@angular/core';
import { RouterOutlet } from '@angular/router';
import { OpeFronterasTableComponent } from './components/ope-frontera-table/ope-fronteras-table.component';
import { ApiResponse } from '../../../../types/api-response.type';
import { OpeFronteraFilterFormComponent } from './components/ope-frontera-filter-form/ope-fronteras-filter-form.component';
import { LocalFiltrosOpeFronteras } from '../../../../services/ope/administracion/local-filtro-ope-fronteras.service';
import { OpeFronterasService } from '@services/ope/administracion/ope-fronteras.service';
import { OpeFrontera } from '@type/ope/administracion/ope-frontera.type';

@Component({
  selector: 'app-ope-fronteras',
  standalone: true,
  imports: [CommonModule, RouterOutlet, OpeFronterasTableComponent, OpeFronteraFilterFormComponent],
  templateUrl: './ope-fronteras.component.html',
  styleUrl: './ope-fronteras.component.scss',
})
export class OpeFronterasComponent {
  public opeFronterasService = inject(OpeFronterasService);
  public filtrosOpeFronterasService = inject(LocalFiltrosOpeFronteras);

  public filtros = signal<any>({});

  public isLoading = true;
  public refreshFilterForm = true;
  public childActivated = false;

  public opeFronteras: ApiResponse<OpeFrontera[]> = {
    count: 0,
    page: 1,
    pageSize: 10,
    data: [],
    pageCount: 0,
  };

  async ngOnInit() {
    //const fires = await this.fireService.get();
    //this.fires = fires;
    this.filtros.set(this.filtrosOpeFronterasService.getFilters());
  }
}
