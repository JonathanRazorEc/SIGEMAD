import { CommonModule } from '@angular/common';
import { Component, EventEmitter, inject, Input, OnChanges, Output, Renderer2, SimpleChanges, ViewChild } from '@angular/core';
import { OpePorcentajeOcupacionAreaEstacionamiento } from '../../../../../../types/ope/administracion/ope-porcentaje-ocupacion-area-estacionamiento.type';
import { MatProgressSpinnerModule } from '@angular/material/progress-spinner';
import { MatPaginator, MatPaginatorModule } from '@angular/material/paginator';
import { MatTableDataSource, MatTableModule } from '@angular/material/table';
import { MatSort, MatSortModule } from '@angular/material/sort';
import { MatDialog } from '@angular/material/dialog';
import { Router } from '@angular/router';
import { OpePorcentajeOcupacionAreaEstacionamientoCreateEdit } from '../ope-porcent-ocup-area-est-create-edit-form/ope-porcentaje-ocupacion-area-estacionamiento-create-edit-form.component';
import { TooltipDirective } from '@shared/directive/tooltip/tooltip.directive';
import { AlertService } from '@shared/alert/alert.service';
import { NgxSpinnerService } from 'ngx-spinner';
import { OpePorcentajesOcupacionAreasEstacionamientoService } from '@services/ope/administracion/ope-porcentajes-ocupacion-areas-estacionamiento.service';
import { MatSnackBar } from '@angular/material/snack-bar';
import { RowHighlightDirective } from '@shared/directive/ope/row-highlight.directive';
import { MatSortNoClearDirective } from '@shared/directive/ope/mat-sort-no-clear.directive';
import { UtilsService } from '@shared/services/utils.service';

@Component({
  selector: 'app-ope-porcentajes-ocupacion-areas-estacionamiento-table',
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
  templateUrl: './ope-porcentajes-ocupacion-areas-estacionamiento-table.component.html',
  styleUrl: './ope-porcentajes-ocupacion-areas-estacionamiento-table.component.scss',
})
export class OpePorcentajesOcupacionAreasEstacionamientoTableComponent implements OnChanges {
  @Input() opePorcentajesOcupacionAreasEstacionamiento: OpePorcentajeOcupacionAreaEstacionamiento[] = [];
  @Input() isLoading: boolean = true;
  @Input() refreshFilterForm: boolean = true;

  @Output() refreshFilterFormChange = new EventEmitter<boolean>();

  public dataSource = new MatTableDataSource<OpePorcentajeOcupacionAreaEstacionamiento>([]);
  @ViewChild(MatPaginator) paginator!: MatPaginator;
  @ViewChild(MatSort) sort!: MatSort;
  public router = inject(Router);
  private dialog = inject(MatDialog);

  private spinner = inject(NgxSpinnerService);
  public renderer = inject(Renderer2);
  public alertService = inject(AlertService);
  public snackBar = inject(MatSnackBar);
  public opePorcentajesOcupacionAreasEstacionamientoService = inject(OpePorcentajesOcupacionAreasEstacionamientoService);
  public routenav = inject(Router);
  public utilsService = inject(UtilsService);

  public displayedColumns: string[] = ['opeOcupacion', 'porcentajeInferior', 'porcentajeSuperior', 'opciones'];

  ngOnChanges(changes: SimpleChanges): void {
    if (changes['opePorcentajesOcupacionAreasEstacionamiento'] && this.opePorcentajesOcupacionAreasEstacionamiento) {
      this.dataSource.data = this.opePorcentajesOcupacionAreasEstacionamiento;
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
          case 'opeOcupacion': {
            return item.opeOcupacion?.nombre || '';
          }
          default: {
            const result = item[property as keyof OpePorcentajeOcupacionAreaEstacionamiento];
            return typeof result === 'string' || typeof result === 'number' ? result : '';
          }
        }
      };
      this.dataSource.sortData = this.utilsService.getSpanishCollatorSortFn(this.dataSource.sortingDataAccessor);
      this.dataSource._updateChangeSubscription();
    }
  }

  goToEdit(opePorcentajeOcupacionAreaEstacionamiento: OpePorcentajeOcupacionAreaEstacionamiento) {
    //this.router.navigate([`fire/fire-national-edit/1`]);
  }

  goToEditOpePorcentajeOcupacionAreaEstacionamiento(opePorcentajeOcupacionAreaEstacionamiento: OpePorcentajeOcupacionAreaEstacionamiento) {}

  goModal() {
    const dialogRef = this.dialog.open(OpePorcentajeOcupacionAreaEstacionamientoCreateEdit, {
      width: '90vw',
      height: '90vh',
      maxWidth: 'none',
      disableClose: true,
      data: {
        title: 'Nuevo - Porcentaje ocupación área estacionamiento',
      },
    });

    dialogRef.afterClosed().subscribe((result) => {
      if (result) {
        console.log('Modal result:', result);
      }
    });
  }

  goModalEdit(opePorcentajeOcupacionAreaEstacionamiento: OpePorcentajeOcupacionAreaEstacionamiento) {
    const dialogRef = this.dialog.open(OpePorcentajeOcupacionAreaEstacionamientoCreateEdit, {
      width: '65vw',
      maxWidth: 'none',
      disableClose: true,
      data: {
        title: 'Modificar - Porcentaje ocupación área estacionamiento.',
        opePorcentajeOcupacionAreaEstacionamiento: opePorcentajeOcupacionAreaEstacionamiento,
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
  async deleteOpePorcentajeOcupacionAreaEstacionamiento(idOpePorcentajeOcupacionAreaEstacionamiento: number) {
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

          await this.opePorcentajesOcupacionAreasEstacionamientoService.delete(idOpePorcentajeOcupacionAreaEstacionamiento);
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
                this.routenav.navigate(['/ope/administracion/porcentajes-ocupacion-areas-estacionamiento']).then(() => {
                  window.location.href = '/ope/administracion/porcentajes-ocupacion-areas-estacionamiento';
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
  //
}
