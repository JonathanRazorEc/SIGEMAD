import { CommonModule } from '@angular/common';
import { Component } from '@angular/core';
import { MatChipListboxChange, MatChipsModule } from '@angular/material/chips';
import { MatTableDataSource, MatTableModule } from '@angular/material/table';
import { RouterOutlet } from '@angular/router';
import { OpeFasesService } from '@services/ope/administracion/ope-fases.service';
import { TablaListadoComponent } from './listado/ope-log-listado.component';
import { OpePeriodosTiposService } from '@services/ope/administracion/ope-periodos-tipos.service';
import { OpeEstadosOcupacionService } from '@services/ope/administracion/ope-estados-ocupacion.service';
import { OpeAreasDescansoTiposService } from '@services/ope/administracion/ope-areas-descanso-tipos.service';
import { OpeAsistenciasSanitariasTiposService } from '@services/ope/datos/ope-asistencias-sanitarias-tipos.service';
import { OpeAsistenciasSocialesEdadesService } from '@services/ope/datos/ope-asistencias-sociales-edades.service';
import { OpeAsistenciasSocialesNacionalidadesService } from '@services/ope/datos/ope-asistencias-sociales-nacionalidades.service';
import { OpeAsistenciasSocialesOrganismosTiposService } from '@services/ope/datos/ope-asistencias-sociales-organismos-tipos.service';
import { OpeAsistenciasSocialesSexosService } from '@services/ope/datos/ope-asistencias-sociales-sexos.service';
import { OpeAsistenciasSocialesTareasTiposService } from '@services/ope/datos/ope-asistencias-sociales-tareas-tipos.service';
import { OpeAsistenciasSocialesTiposService } from '@services/ope/datos/ope-asistencias-sociales-tipos.service';
import { OpeLogsService } from '@services/ope/administracion/ope-logs.service';

@Component({
  selector: 'app-ope-logs',
  standalone: true,
  imports: [CommonModule, RouterOutlet, MatChipsModule, MatTableModule, TablaListadoComponent],
  templateUrl: './ope-logs.component.html',
  styleUrls: ['./ope-logs.component.scss'],
})
export class OpeLogsComponent {
  readonly sectionLabels = [{ label: 'Fronteras', servicio: 'fronteras' }];

  readonly sections = this.sectionLabels.map((s, index) => ({
    id: index + 1,
    ...s,
  }));

  selectedOption = this.sections[0];
  displayedColumns: string[] = [];
  dataSource: MatTableDataSource<any> = new MatTableDataSource<any>([]);

  constructor(private opeLogsService: OpeLogsService) {}

  ngOnInit() {
    this.loadData(this.selectedOption.servicio);
  }

  onSelectionChange(event: MatChipListboxChange) {
    const selected = this.sections.find((s) => s.id === event.value);
    if (selected) {
      this.selectedOption = selected;
      this.loadData(selected.servicio);
    }
  }

  loadData(servicio: string) {
    switch (servicio) {
      case 'fronteras':
        this.opeLogsService.get().then((response) => {
          this.dataSource.data = response.data;
          this.displayedColumns = this.obtenerColumnas(response.data);
        });
        break;

      default:
        this.dataSource.data = [];
        this.displayedColumns = [];
        break;
    }
  }

  obtenerColumnas(data: any[]): string[] {
    if (data.length > 0) {
      return Object.keys(data[0]).filter((key) => key !== 'id');
    }
    return [];
  }

  trackByFn(index: number, item: any): number {
    return item.id;
  }
}
