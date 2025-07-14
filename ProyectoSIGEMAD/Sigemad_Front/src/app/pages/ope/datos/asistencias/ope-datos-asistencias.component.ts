import { CommonModule } from '@angular/common';
import { Component, inject, signal } from '@angular/core';
import { RouterOutlet } from '@angular/router';
import { ApiResponse } from '../../../../types/api-response.type';
import { OpeDatosAsistenciasService } from '@services/ope/datos/ope-datos-asistencias.service';
import { LocalFiltrosOpeDatosAsistencias } from '@services/ope/datos/local-filtro-ope-datos-asistencias.service';
import { OpeDatosAsistenciasTableComponent } from './components/ope-dato-asistencia-table/ope-datos-asistencias-table.component';
import { OpeDatoAsistenciaFilterFormComponent } from './components/ope-dato-asistencia-filter-form/ope-datos-asistencias-filter-form.component';
import { OpeDatoAsistencia } from '@type/ope/datos/ope-dato-asistencia.type';

@Component({
  selector: 'app-ope-datos-asistencias',
  standalone: true,
  imports: [CommonModule, RouterOutlet, OpeDatosAsistenciasTableComponent, OpeDatoAsistenciaFilterFormComponent],
  templateUrl: './ope-datos-asistencias.component.html',
  styleUrl: './ope-datos-asistencias.component.scss',
})
export class OpeDatosAsistenciasComponent {
  public opeDatosAsistenciasService = inject(OpeDatosAsistenciasService);
  public filtrosOpeDatosAsistenciasService = inject(LocalFiltrosOpeDatosAsistencias);

  public filtros = signal<any>({});

  public isLoading = true;
  public refreshFilterForm = true;
  public childActivated = false;

  public opeDatosAsistencias: ApiResponse<OpeDatoAsistencia[]> = {
    count: 0,
    page: 1,
    pageSize: 10,
    data: [],
    pageCount: 0,
  };

  async ngOnInit() {
    //const fires = await this.fireService.get();
    //this.fires = fires;
    this.filtros.set(this.filtrosOpeDatosAsistenciasService.getFilters());
  }
}
