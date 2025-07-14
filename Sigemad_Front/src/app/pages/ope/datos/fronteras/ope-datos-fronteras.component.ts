import { CommonModule } from '@angular/common';
import { Component, inject, signal } from '@angular/core';
import { RouterOutlet } from '@angular/router';
import { ApiResponse } from '../../../../types/api-response.type';
import { OpeDatosFronterasService } from '@services/ope/datos/ope-datos-fronteras.service';
import { LocalFiltrosOpeDatosFronteras } from '@services/ope/datos/local-filtro-ope-datos-fronteras.service';
import { OpeDatosFronterasTableComponent } from './components/ope-dato-frontera-table/ope-datos-fronteras-table.component';
import { OpeDatoFrontera } from '@type/ope/datos/ope-dato-frontera.type';
import { OpeDatoFronteraFilterFormComponent } from './components/ope-dato-frontera-filter-form/ope-datos-fronteras-filter-form.component';

@Component({
  selector: 'app-ope-datos-fronteras',
  standalone: true,
  imports: [CommonModule, RouterOutlet, OpeDatosFronterasTableComponent, OpeDatoFronteraFilterFormComponent],
  templateUrl: './ope-datos-fronteras.component.html',
  styleUrl: './ope-datos-fronteras.component.scss',
})
export class OpeDatosFronterasComponent {
  public opeDatosFronterasService = inject(OpeDatosFronterasService);
  public filtrosOpeDatosFronterasService = inject(LocalFiltrosOpeDatosFronteras);

  public filtros = signal<any>({});

  public isLoading = true;
  public refreshFilterForm = true;
  public childActivated = false;

  public opeDatosFronteras: ApiResponse<OpeDatoFrontera[]> = {
    count: 0,
    page: 1,
    pageSize: 10,
    data: [],
    pageCount: 0,
  };

  async ngOnInit() {
    //const fires = await this.fireService.get();
    //this.fires = fires;
    this.filtros.set(this.filtrosOpeDatosFronterasService.getFilters());
  }
}
