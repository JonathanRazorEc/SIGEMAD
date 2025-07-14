import { CommonModule } from '@angular/common';
import { Component, EventEmitter, inject, Input, OnChanges, Output, Renderer2, SimpleChanges, ViewChild } from '@angular/core';
import { OpePeriodo } from '../../../../../../types/ope/administracion/ope-periodo.type';
import { MatProgressSpinnerModule } from '@angular/material/progress-spinner';
import { MatPaginator, MatPaginatorModule } from '@angular/material/paginator';
import moment from 'moment';
import { MatTableDataSource, MatTableModule } from '@angular/material/table';
import { MatSort, MatSortModule } from '@angular/material/sort';
import { MatDialog } from '@angular/material/dialog';
import { Router } from '@angular/router';
import { OpePeriodoCreateEdit } from '../ope-periodo-create-edit-form/ope-periodo-create-edit-form.component';
import { TooltipDirective } from '@shared/directive/tooltip/tooltip.directive';
import { AlertService } from '@shared/alert/alert.service';
import { NgxSpinnerService } from 'ngx-spinner';
import { OpePeriodosService } from '@services/ope/administracion/ope-periodos.service';
import { MatSnackBar } from '@angular/material/snack-bar';
import { RowHighlightDirective } from '@shared/directive/ope/row-highlight.directive';
import { OpeErrorsService } from '@shared/services/ope/ope-errors.service';
import { MatSortNoClearDirective } from '@shared/directive/ope/mat-sort-no-clear.directive';
import { UtilsService } from '@shared/services/utils.service';

@Component({
  selector: 'app-ope-periodos-table',
  standalone: true,
  imports: [
    MatProgressSpinnerModule,
    MatPaginatorModule,
    MatTableModule,
    CommonModule,
    TooltipDirective,
    MatSortModule,
    RowHighlightDirective,
    MatSortNoClearDirective,
  ],
  templateUrl: './ope-periodos-table.component.html',
  styleUrl: './ope-periodos-table.component.scss',
})
export class OpePeriodosTableComponent implements OnChanges {
  @Input() opePeriodos: OpePeriodo[] = [];
  @Input() isLoading: boolean = true;
  @Input() refreshFilterForm: boolean = true;

  @Output() refreshFilterFormChange = new EventEmitter<boolean>();

  public dataSource = new MatTableDataSource<OpePeriodo>([]);
  @ViewChild(MatPaginator) paginator!: MatPaginator;
  @ViewChild(MatSort) sort!: MatSort;
  public router = inject(Router);
  private dialog = inject(MatDialog);

  private spinner = inject(NgxSpinnerService);
  public renderer = inject(Renderer2);
  public alertService = inject(AlertService);
  public snackBar = inject(MatSnackBar);
  public opeErrorsService = inject(OpeErrorsService);
  public opePeriodosService = inject(OpePeriodosService);
  public routenav = inject(Router);
  public utilsService = inject(UtilsService);

  public displayedColumns: string[] = [
    'nombre',
    'opePeriodoTipo',
    'fechaInicioFaseSalida',
    'fechaFinFaseSalida',
    'fechaInicioFaseRetorno',
    'fechaFinFaseRetorno',
    'opciones',
  ];

  ngOnChanges(changes: SimpleChanges): void {
    if (changes['opePeriodos'] && this.opePeriodos) {
      this.dataSource.data = this.opePeriodos;
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
          case 'opePeriodoTipo': {
            return item.opePeriodoTipo?.nombre || '';
          }
          default: {
            const result = item[property as keyof OpePeriodo];
            return typeof result === 'string' || typeof result === 'number' ? result : '';
          }
        }
      };

      this.dataSource.sortData = this.utilsService.getSpanishCollatorSortFn(this.dataSource.sortingDataAccessor);
      this.dataSource._updateChangeSubscription();
    }
  }

  goToEdit(periodo: OpePeriodo) {
    //this.router.navigate([`fire/fire-national-edit/1`]);
  }

  goToEditPeriodo(opePeriodo: OpePeriodo) {}

  goModal() {
    const dialogRef = this.dialog.open(OpePeriodoCreateEdit, {
      width: '90vw',
      height: '90vh',
      maxWidth: 'none',
      disableClose: true,
      data: {
        title: 'Nuevo - Periodo',
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

  goModalEdit(opePeriodo: OpePeriodo) {
    const dialogRef = this.dialog.open(OpePeriodoCreateEdit, {
      width: '65vw',
      maxWidth: 'none',
      disableClose: true,
      data: {
        title: 'Modificar - Periodo.',
        opePeriodo: opePeriodo,
      },
    });

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
  }

  //
  async deleteOpePeriodo(idOpePeriodo: number) {
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

          //await this.opePeriodosService.delete(idOpePeriodo);

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
                this.routenav.navigate(['/ope/administracion/periodos']).then(() => {
                  window.location.href = '/ope/administracion/periodos';
                });
                this.spinner.hide();
              });
            // FIN PCD
          }, 2000);
          */

          // TEST
          /*
          this.spinner.hide();
          this.refreshFilterFormChange.emit(!this.refreshFilterForm);
          this.snackBar.open('Datos eliminados correctamente!', '', {
            duration: 3000,
            horizontalPosition: 'center',
            verticalPosition: 'bottom',
            panelClass: ['snackbar-verde'],
          });
          */
          // FIN TEST

          //
          try {
            await this.opePeriodosService.delete(idOpePeriodo);
            this.spinner.hide();
            this.refreshFilterFormChange.emit(!this.refreshFilterForm);
            this.snackBar.open('Datos eliminados correctamente!', '', {
              duration: 3000,
              horizontalPosition: 'center',
              verticalPosition: 'bottom',
              panelClass: ['snackbar-verde'],
            });
          } catch (error) {
            this.spinner.hide();
            this.snackBar.open(this.opeErrorsService.obtenerMensajeError(error, 'opePeriodos'), '', {
              duration: 3000,
              horizontalPosition: 'center',
              verticalPosition: 'bottom',
              panelClass: ['snackbar-rojo'],
            });
            console.error('Error', error);
          }
          //
        } else {
          this.spinner.hide();
        }
      });
  }
  //
}
