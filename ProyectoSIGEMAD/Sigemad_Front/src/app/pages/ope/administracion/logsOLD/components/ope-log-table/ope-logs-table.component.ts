import { CommonModule } from '@angular/common';
import { Component, EventEmitter, inject, Input, OnChanges, Output, Renderer2, SimpleChanges, ViewChild } from '@angular/core';
import { OpeLog } from '../../../../../../types/ope/administracion/ope-log.type';
import { MatProgressSpinnerModule } from '@angular/material/progress-spinner';
import { MatPaginator, MatPaginatorModule } from '@angular/material/paginator';
import moment from 'moment';
import { MatTableDataSource, MatTableModule } from '@angular/material/table';
import { MatSort, MatSortModule } from '@angular/material/sort';

@Component({
  selector: 'app-ope-logs-table',
  standalone: true,
  imports: [MatProgressSpinnerModule, MatPaginatorModule, MatTableModule, CommonModule, MatSortModule],
  templateUrl: './ope-logs-table.component.html',
  styleUrl: './ope-logs-table.component.scss',
})
export class OpeLogsTableComponent implements OnChanges {
  @Input() opeLogs: OpeLog[] = [];
  @Input() isLoading: boolean = true;
  @Input() refreshFilterForm: boolean = true;

  @Output() refreshFilterFormChange = new EventEmitter<boolean>();

  public dataSource = new MatTableDataSource<OpeLog>([]);
  @ViewChild(MatPaginator) paginator!: MatPaginator;
  @ViewChild(MatSort) sort!: MatSort;

  public displayedColumns: string[] = ['fechaRegistro', 'tipoMovimiento', 'usuario'];

  ngOnChanges(changes: SimpleChanges): void {
    if (changes['opeLogs'] && this.opeLogs) {
      this.dataSource.data = this.opeLogs;
    }
  }

  ngAfterViewInit(): void {
    this.dataSource.paginator = this.paginator;
    this.dataSource.sort = this.sort;
    this.setDataSourceAttributes();
  }

  setDataSourceAttributes() {
    if (this.sort) {
      this.dataSource.sort = this.sort;
      this.dataSource.sortingDataAccessor = (item, property) => {
        switch (property) {
          case 'usuario': {
            return this.getUsuario(item) || '';
          }
          default: {
            const result = item[property as keyof OpeLog];
            return typeof result === 'string' || typeof result === 'number' ? result : '';
          }
        }
      };
      this.dataSource._updateChangeSubscription();
    }
  }

  getFechaFormateada(fecha: any) {
    if (!fecha) {
      return '';
    }
    return moment(fecha).format('DD/MM/yyyy HH:mm');
  }

  getTipoMovimiento(tipoMovimiento: string): string {
    if (!tipoMovimiento) {
      return 'Desconocido'; // O cualquier otro valor por defecto
    }

    switch (tipoMovimiento.toUpperCase()) {
      case 'I':
        return 'Creación';
      case 'U':
        return 'Actualización';
      case 'D':
        return 'Borrado';
      default:
        return tipoMovimiento; // Si el valor no coincide, lo devuelve tal cual
    }
  }

  getUsuario(opeLog: OpeLog): string {
    if (!opeLog || !opeLog.tipoMovimiento) {
      return 'Desconocido'; // O cualquier otro valor por defecto
    }

    switch (opeLog.tipoMovimiento.toUpperCase()) {
      case 'I':
        return opeLog.creadoPor || 'Desconocido';
      case 'U':
        return opeLog.modificadoPor || 'Desconocido';
      case 'D':
        return opeLog.eliminadoPor || 'Desconocido';
      default:
        return 'Desconocido'; // Si el valor no coincide, lo devuelve tal cual
    }
  }
}
