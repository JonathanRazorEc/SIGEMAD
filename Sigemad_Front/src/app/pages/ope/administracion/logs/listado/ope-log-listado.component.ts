import { Component, inject, Input, OnInit, ViewChild } from '@angular/core';
import { MatTableDataSource, MatTableModule } from '@angular/material/table';
import { MatPaginator, MatPaginatorModule } from '@angular/material/paginator';
import { MatSort, MatSortModule } from '@angular/material/sort';
import { CommonModule } from '@angular/common';
import { OpeLog } from '@type/ope/administracion/ope-log.type';
import { MatDialog } from '@angular/material/dialog';
import { OpeLogView } from '../ope-log-view/ope-log-view.component';

@Component({
  selector: 'app-ope-log-listado',
  standalone: true,
  templateUrl: './ope-log-listado.component.html',
  styleUrls: ['./ope-log-listado.component.scss'],
  imports: [CommonModule, MatTableModule, MatPaginatorModule, MatSortModule],
})
export class TablaListadoComponent implements OnInit {
  //@Input() displayedColumns: string[] = [];

  @Input() set displayedColumns(columns: string[]) {
    // Nos aseguramos de no duplicarla si ya está
    if (columns && !columns.includes('opciones')) {
      this._displayedColumns = [...columns, 'opciones'];
    } else {
      this._displayedColumns = columns;
    }
  }

  get displayedColumns(): string[] {
    return this._displayedColumns;
  }

  private _displayedColumns: string[] = [];

  private dialog = inject(MatDialog);
  @Input() dataSource: MatTableDataSource<any> = new MatTableDataSource<any>();

  @ViewChild(MatPaginator) paginator!: MatPaginator;
  @ViewChild(MatSort) sort!: MatSort;

  ngOnInit(): void {
    // Si necesitas hacer algo con los datos cuando llegan por @Input, puedes hacerlo aquí
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
          /*
          case 'opePuertoOrigen': {
            return item.opePuertoOrigen?.nombre || '';
          }
          case 'opePuertoDestino': {
            return item.opePuertoDestino?.nombre || '';
          }
          case 'opeFase': {
            return item.opeFase?.nombre || '';
          }
          */
          default: {
            const result = item[property as keyof OpeLog];
            return typeof result === 'string' || typeof result === 'number' ? result : '';
          }
        }
      };
      this.dataSource._updateChangeSubscription();
    }
  }

  formatters: { [key: string]: (value: any, element?: any) => string } = {
    tipoMovimiento: (value: string) => {
      switch (value) {
        case 'I':
          return 'INSERTAR';
        case 'D':
          return 'BORRAR';
        case 'U':
          return 'ACTUALIZAR';
        default:
          return value;
      }
    },
    // Otros campos con su formateo si quieres
    // ejemplo: fecha: (value) => moment(value).format('DD/MM/YYYY'),
  };

  goModalView(opeLog: OpeLog) {
    const dialogRef = this.dialog.open(OpeLogView, {
      width: '75vw',
      maxWidth: 'none',
      disableClose: true,
      data: {
        title: 'Ver - Log.',
        opeLog: opeLog,
      },
    });

    /*
    dialogRef.afterClosed().subscribe((result) => {
      if (result?.refresh) {
        this.refreshFilterFormChange.emit(!this.refreshFilterForm);
        this.snackBar.open('Datos ingresados correctamente!', '', {
          duration: 3000,
          horizontalPosition: 'center',
          verticalPosition: 'bottom',
          panelClass: ['snackbar-verde'],
        });
      }
    });
    */
  }
}
