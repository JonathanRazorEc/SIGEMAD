import { CommonModule } from '@angular/common';
import { Component } from '@angular/core';
import { MatChipListboxChange, MatChipsModule } from '@angular/material/chips';
import { MatTableDataSource, MatTableModule } from '@angular/material/table';
import { RouterOutlet } from '@angular/router';
import { OpeFasesService } from '@services/ope/administracion/ope-fases.service';
import { TablaListadoComponent } from './listado/ope-catalogo-listado.component';
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
import { OpeDatoFronteraIntervaloHorario } from '@type/ope/datos/ope-dato-frontera-intervalo-horario.type';
import { OpeDatosFronterasIntervalosHorariosService } from '@services/ope/datos/ope-datos-fronteras-intervalos-horarios.service';

@Component({
  selector: 'app-ope-catalogos',
  standalone: true,
  imports: [CommonModule, RouterOutlet, MatChipsModule, MatTableModule, TablaListadoComponent],
  templateUrl: './ope-catalogos.component.html',
  styleUrls: ['./ope-catalogos.component.scss'],
})
export class OpeCatalogosComponent {
  readonly sectionLabels = [
    { label: 'Fases', servicio: 'fases' },
    { label: 'Tipos de periodos', servicio: 'tiposPeriodos' },
    { label: 'Estados de ocupación', servicio: 'ocupacion' },
    { label: 'Tipos área descanso', servicio: 'descanso' },
    { label: 'Tipos de asistencia sanitaria', servicio: 'sanitaria' },
    { label: 'Tipos de asistencia social', servicio: 'asistenciaSocial' },
    { label: 'Tipos de tareas asist. social', servicio: 'tareasSocial' },
    { label: 'Tipos organismo asist. social', servicio: 'organismosSocial' },
    { label: 'Nacionalidades asist. social', servicio: 'nacionalidades' },
    { label: 'Sexos asistencia social', servicio: 'sexos' },
    { label: 'Edades asistencia social', servicio: 'edades' },
    { label: 'Intervalos horarios dato frontera', servicio: 'intervalosHorariosDatosFronteras' },
  ];

  readonly sections = this.sectionLabels.map((s, index) => ({
    id: index + 1,
    ...s,
  }));

  selectedOption = this.sections[0];
  displayedColumns: string[] = [];
  dataSource: MatTableDataSource<any> = new MatTableDataSource<any>([]); // Especificamos el tipo aquí

  constructor(
    private opeFasesService: OpeFasesService,
    private opePeriodosTiposService: OpePeriodosTiposService,
    private opeEstadosOcupacionService: OpeEstadosOcupacionService,
    private opeAreasDescansoTiposService: OpeAreasDescansoTiposService,
    private opeAsistenciasSanitariasTiposService: OpeAsistenciasSanitariasTiposService,
    private opeAsistenciasSocialesTiposService: OpeAsistenciasSocialesTiposService,
    private opeAsistenciasSocialesTareasTiposService: OpeAsistenciasSocialesTareasTiposService,
    private opeAsistenciasSocialesOrganismosTiposService: OpeAsistenciasSocialesOrganismosTiposService,
    private opeAsistenciasSocialesNacionalidadesService: OpeAsistenciasSocialesNacionalidadesService,
    private opeAsistenciasSocialesSexosService: OpeAsistenciasSocialesSexosService,
    private opeAsistenciasSocialesEdadesService: OpeAsistenciasSocialesEdadesService,
    private opeDatosFronterasIntervalosHorariosService: OpeDatosFronterasIntervalosHorariosService
  ) {}

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
      case 'fases':
        this.opeFasesService.get().then((data) => {
          this.dataSource.data = data;
          this.displayedColumns = this.obtenerColumnas(data);
        });
        break;

      case 'tiposPeriodos':
        this.opePeriodosTiposService.get().then((data) => {
          this.dataSource.data = data;
          this.displayedColumns = this.obtenerColumnas(data);
        });
        break;

      case 'ocupacion':
        this.opeEstadosOcupacionService.get().then((data) => {
          this.dataSource.data = data;
          this.displayedColumns = this.obtenerColumnas(data);
        });
        break;

      case 'descanso':
        this.opeAreasDescansoTiposService.get().then((data) => {
          this.dataSource.data = data;
          this.displayedColumns = this.obtenerColumnas(data);
        });
        break;

      case 'sanitaria':
        this.opeAsistenciasSanitariasTiposService.get().then((data) => {
          this.dataSource.data = data;
          this.displayedColumns = this.obtenerColumnas(data);
        });
        break;

      case 'asistenciaSocial':
        this.opeAsistenciasSocialesTiposService.get().then((data) => {
          this.dataSource.data = data;
          this.displayedColumns = this.obtenerColumnas(data);
        });
        break;

      case 'tareasSocial':
        this.opeAsistenciasSocialesTareasTiposService.get().then((data) => {
          this.dataSource.data = data;
          this.displayedColumns = this.obtenerColumnas(data);
        });
        break;

      case 'organismosSocial':
        this.opeAsistenciasSocialesOrganismosTiposService.get().then((data) => {
          this.dataSource.data = data;
          this.displayedColumns = this.obtenerColumnas(data);
        });
        break;

      case 'nacionalidades':
        this.opeAsistenciasSocialesNacionalidadesService.get().then((data) => {
          this.dataSource.data = data;
          this.displayedColumns = this.obtenerColumnas(data);
        });
        break;

      case 'sexos':
        this.opeAsistenciasSocialesSexosService.get().then((data) => {
          this.dataSource.data = data;
          this.displayedColumns = this.obtenerColumnas(data);
        });
        break;

      case 'edades':
        this.opeAsistenciasSocialesEdadesService.get().then((data) => {
          this.dataSource.data = data;
          this.displayedColumns = this.obtenerColumnas(data);
        });
        break;

      case 'intervalosHorariosDatosFronteras':
        this.opeDatosFronterasIntervalosHorariosService.get().then((data) => {
          this.dataSource.data = data;
          this.displayedColumns = this.obtenerColumnas(data);
        });
        break;

      default:
        this.dataSource.data = []; // Aseguramos que no haya datos si no se selecciona un servicio
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
