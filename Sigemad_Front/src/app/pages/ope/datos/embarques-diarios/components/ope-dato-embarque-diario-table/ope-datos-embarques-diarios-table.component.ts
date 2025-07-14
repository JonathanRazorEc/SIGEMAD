import { CommonModule } from '@angular/common';
import { Component, effect, EventEmitter, inject, Input, OnChanges, Output, Renderer2, signal, SimpleChanges, ViewChild } from '@angular/core';
import { MatProgressSpinnerModule } from '@angular/material/progress-spinner';
import { MatPaginator, MatPaginatorModule } from '@angular/material/paginator';
import moment from 'moment';
import { MatTableDataSource, MatTableModule } from '@angular/material/table';
import { MatSort, MatSortModule } from '@angular/material/sort';
import { MatDialog } from '@angular/material/dialog';
import { Router } from '@angular/router';
import { AlertService } from '@shared/alert/alert.service';
import { NgxSpinnerService } from 'ngx-spinner';
import { MatSnackBar } from '@angular/material/snack-bar';
import { OpeDatoEmbarqueDiario } from '@type/ope/datos/ope-dato-embarque-diario.type';
import { OpeDatosEmbarquesDiariosService } from '@services/ope/datos/ope-datos-embarques-diarios.service';
import { OpeDatoEmbarqueDiarioCreateEdit } from '../ope-dato-embarque-diario-create-edit-form/ope-dato-embarque-diario-create-edit-form.component';
import { RowHighlightDirective } from '@shared/directive/ope/row-highlight.directive';
import { MatSortNoClearDirective } from '@shared/directive/ope/mat-sort-no-clear.directive';
import { TooltipDirective } from '@shared/directive/tooltip/tooltip.directive';
import { DateUtils } from '@shared/utils/date-utils';

@Component({
  selector: 'app-ope-datos-embarques-diarios-table',
  standalone: true,
  imports: [
    MatProgressSpinnerModule,
    MatPaginatorModule,
    MatTableModule,
    CommonModule,
    MatSortModule,
    RowHighlightDirective,
    MatSortNoClearDirective,
    TooltipDirective,
  ],
  templateUrl: './ope-datos-embarques-diarios-table.component.html',
  styleUrl: './ope-datos-embarques-diarios-table.component.scss',
})
export class OpeDatosEmbarquesDiariosTableComponent implements OnChanges {
  @Input() opeDatosEmbarquesDiarios: OpeDatoEmbarqueDiario[] = [];
  @Input() isLoading: boolean = true;
  @Input() refreshFilterForm: boolean = true;

  @Output() refreshFilterFormChange = new EventEmitter<boolean>();

  public dataSource = new MatTableDataSource<OpeDatoEmbarqueDiario>([]);
  @ViewChild(MatPaginator) paginator!: MatPaginator;
  @ViewChild(MatSort) sort!: MatSort;
  public router = inject(Router);
  private dialog = inject(MatDialog);

  private spinner = inject(NgxSpinnerService);
  public renderer = inject(Renderer2);
  public alertService = inject(AlertService);
  public snackBar = inject(MatSnackBar);
  public opeDatosEmbarquesDiariosService = inject(OpeDatosEmbarquesDiariosService);
  public routenav = inject(Router);

  public displayedColumns: string[] = [
    'fecha',
    'opeLineaMaritima',
    'opeFase',
    'numeroRotaciones',
    'numeroPasajeros',
    'numeroTurismos',
    'numeroAutocares',
    'numeroCamiones',
    'numeroTotalVehiculos',
    'opciones',
  ];

  ngOnChanges(changes: SimpleChanges): void {
    if (changes['opeDatosEmbarquesDiarios'] && this.opeDatosEmbarquesDiarios) {
      this.dataSource.data = this.opeDatosEmbarquesDiarios;
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
          /*
          case 'fecha': {
            // ORDENACION POR FECHA Y NOMBRE COMBINADA
            // Formateamos fecha a 'YYYYMMDD' para que quede ordenable por string
            const fecha = new Date(item.fecha);
            const year = fecha.getFullYear();
            const month = (fecha.getMonth() + 1).toString().padStart(2, '0');
            const day = fecha.getDate().toString().padStart(2, '0');
            const fechaFormateada = `${year}${month}${day}`;

            const nombre = (item.opeLineaMaritima?.nombre || '').toLowerCase();

            return fechaFormateada + nombre;
            // FIN ORDENACION POR FECHA Y NOMBRE COMBINADA
          }
          */
          case 'opeLineaMaritima': {
            return item.opeLineaMaritima?.nombre || '';
          }
          case 'opeFase': {
            return item.opeLineaMaritima?.opeFase?.nombre || '';
          }
          default: {
            const result = item[property as keyof OpeDatoEmbarqueDiario];
            return typeof result === 'string' || typeof result === 'number' ? result : '';
          }
        }
      };

      // ORDENACIÓN POR DEFECTO (fecha descendente + nombre ascendente)
      const collatorES = new Intl.Collator('es', { sensitivity: 'base' });
      this.dataSource.sortData = (data, sort) => {
        return data.sort((a, b) => {
          if (sort.active === 'fecha') {
            //const fechaA = new Date(a.fecha).getTime();
            //const fechaB = new Date(b.fecha).getTime();
            //
            const fechaA = DateUtils.truncarFecha(a.fecha);
            const fechaB = DateUtils.truncarFecha(b.fecha);
            //

            // Compara fechas respetando la dirección elegida
            let result = fechaA < fechaB ? -1 : fechaA > fechaB ? 1 : 0;

            if (result === 0) {
              // Si la fecha es igual, compara nombres SIEMPRE en orden ascendente
              const nombreA = (a.opeLineaMaritima?.nombre || '').toLowerCase();
              const nombreB = (b.opeLineaMaritima?.nombre || '').toLowerCase();
              //result = nombreA.localeCompare(nombreB); // No aplicar inversión aquí
              //
              result = collatorES.compare(nombreA, nombreB);
              //
            } else {
              // Aplicar la dirección solo sobre la comparación de fecha
              result = sort.direction === 'asc' ? result : -result;
            }

            return result;
          }

          // Orden normal para otras columnas
          const valueA = this.dataSource.sortingDataAccessor(a, sort.active);
          const valueB = this.dataSource.sortingDataAccessor(b, sort.active);
          //let result = valueA < valueB ? -1 : valueA > valueB ? 1 : 0;
          //
          let result: number;

          if (typeof valueA === 'string' && typeof valueB === 'string') {
            result = collatorES.compare(valueA, valueB);
          } else {
            result = valueA < valueB ? -1 : valueA > valueB ? 1 : 0;
          }
          //
          return sort.direction === 'asc' ? result : -result;
        });
      };

      setTimeout(() => {
        this.sort.active = 'fecha';
        this.sort.direction = 'desc';
        this.sort.sortChange.emit({ active: this.sort.active, direction: this.sort.direction });
      });
      // FIN ORDENACIÓN POR DEFECTO

      this.dataSource._updateChangeSubscription();
    }
  }

  /*
  goToEdit(frontera: OpeDatoEmbarqueDiario) {
    //this.router.navigate([`fire/fire-national-edit/1`]);
  }
  */

  goToEditDatoEmbarqueDiario(opeDatoEmbarqueDiario: OpeDatoEmbarqueDiario) {}

  goModal() {
    const dialogRef = this.dialog.open(OpeDatoEmbarqueDiarioCreateEdit, {
      width: '90vw',
      height: '90vh',
      maxWidth: 'none',
      disableClose: true,
      data: {
        title: 'Nuevo - DatoEmbarqueDiario',
      },
    });

    dialogRef.afterClosed().subscribe((result) => {
      if (result) {
        console.log('Modal result:', result);
      }
    });
  }

  getFechaFormateada(fecha: any) {
    if (!fecha) {
      return '';
    }
    return moment(fecha).format('DD/MM/yyyy');
  }

  goModalEdit(opeDatoEmbarqueDiario: OpeDatoEmbarqueDiario) {
    const dialogRef = this.dialog.open(OpeDatoEmbarqueDiarioCreateEdit, {
      width: '65vw',
      maxWidth: 'none',
      disableClose: true,
      data: {
        title: 'Modificar - Dato de embarque diario.',
        opeDatoEmbarqueDiario: opeDatoEmbarqueDiario,
      },
    });

    dialogRef.afterClosed().subscribe((result) => {
      if (result?.refresh) {
        this.refreshFilterFormChange.emit(!this.refreshFilterForm);

        if (result?.borrado) {
          //const mensaje = result?.borrado ? 'Datos eliminados correctamente!' : 'Datos ingresados correctamente!';
          const mensaje = 'Datos eliminados correctamente!';

          this.snackBar.open(mensaje, '', {
            duration: 3000,
            horizontalPosition: 'center',
            verticalPosition: 'bottom',
            panelClass: ['snackbar-verde'],
          });
        }
      }
    });
  }

  async deleteOpeDatoEmbarqueDiario(idOpeDatoEmbarqueDiario: number) {
    this.alertService
      .showAlert({
        title: '¿Estás seguro de eliminar el registro?',
        showCancelButton: true,
        cancelButtonColor: '#d33',
        confirmButtonText: '¡Sí, eliminar!',
        cancelButtonText: 'Cancelar',
        customClass: {
          title: 'sweetAlert-fsize20',
        },
      })

      .then(async (result) => {
        if (result.isConfirmed) {
          this.spinner.show();
          const toolbar = document.querySelector('mat-toolbar');
          this.renderer.setStyle(toolbar, 'z-index', '1');

          await this.opeDatosEmbarquesDiariosService.delete(idOpeDatoEmbarqueDiario);
          /*
          setTimeout(() => {
            //PCD
            this.snackBar
              .open('Datos eliminados correctamente!', '', {
                duration: 3000,
                horizontalPosition: 'center',
                verticalPosition: 'bottom',
                panelClass: ['snackbar-verde'],
              })
              .afterDismissed()
              .subscribe(() => {
                this.routenav.navigate(['/ope/datos/embarques-diarios']).then(() => {
                  window.location.href = '/ope/datos/embarques-diarios';
                });
                this.spinner.hide();
              });
            // FIN PCD
          }, 2000);
          */
          // TEST
          this.spinner.hide();
          this.refreshFilterFormChange.emit(!this.refreshFilterForm);
          this.snackBar.open('Datos eliminados correctamente!', '', {
            duration: 3000,
            horizontalPosition: 'center',
            verticalPosition: 'bottom',
            panelClass: ['snackbar-verde'],
          });
          // FIN TEST
        } else {
          this.spinner.hide();
        }
      });
  }
}
