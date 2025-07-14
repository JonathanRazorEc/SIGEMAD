import { CommonModule } from '@angular/common';
import { Component, EventEmitter, inject, Input, OnChanges, Output, Renderer2, SimpleChanges, ViewChild } from '@angular/core';
import { MatProgressSpinnerModule } from '@angular/material/progress-spinner';
import { MatPaginator, MatPaginatorModule } from '@angular/material/paginator';
import moment from 'moment';
import { MatTableDataSource, MatTableModule } from '@angular/material/table';
import { MatSort, MatSortModule } from '@angular/material/sort';
import { MatDialog } from '@angular/material/dialog';
import { Router } from '@angular/router';
import { OpePuntoControlCarreteraCreateEdit } from '../ope-punto-control-carretera-create-edit-form/ope-punto-control-carretera-create-edit-form.component';
import { TooltipDirective } from '@shared/directive/tooltip/tooltip.directive';
import { AlertService } from '@shared/alert/alert.service';
import { NgxSpinnerService } from 'ngx-spinner';
import { OpePuntosControlCarreterasService } from '@services/ope/administracion/ope-puntos-control-carreteras.service';
import { MatSnackBar } from '@angular/material/snack-bar';
import { OpePuntoControlCarretera } from '@type/ope/administracion/ope-punto-control-carretera.type';
import { RowHighlightDirective } from '@shared/directive/ope/row-highlight.directive';
import { MatSortNoClearDirective } from '@shared/directive/ope/mat-sort-no-clear.directive';
import { UtilsService } from '@shared/services/utils.service';

@Component({
  selector: 'app-ope-puntos-control-carreteras-table',
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
  templateUrl: './ope-puntos-control-carreteras-table.component.html',
  styleUrl: './ope-puntos-control-carreteras-table.component.scss',
})
export class OpePuntosControlCarreterasTableComponent implements OnChanges {
  @Input() opePuntosControlCarreteras: OpePuntoControlCarretera[] = [];
  @Input() isLoading: boolean = true;
  @Input() refreshFilterForm: boolean = true;

  @Output() refreshFilterFormChange = new EventEmitter<boolean>();

  public dataSource = new MatTableDataSource<OpePuntoControlCarretera>([]);
  @ViewChild(MatPaginator) paginator!: MatPaginator;
  @ViewChild(MatSort) sort!: MatSort;
  public router = inject(Router);
  private dialog = inject(MatDialog);

  private spinner = inject(NgxSpinnerService);
  public renderer = inject(Renderer2);
  public alertService = inject(AlertService);
  public snackBar = inject(MatSnackBar);
  public opePuntosControlCarreterasService = inject(OpePuntosControlCarreterasService);
  public routenav = inject(Router);
  public utilsService = inject(UtilsService);

  public displayedColumns: string[] = ['nombre', 'CCAA', 'provincia', 'municipio', 'transitoMedioVehiculos', 'transitoAltoVehiculos', 'opciones'];

  ngOnChanges(changes: SimpleChanges): void {
    if (changes['opePuntosControlCarreteras'] && this.opePuntosControlCarreteras) {
      this.dataSource.data = this.opePuntosControlCarreteras;
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
          case 'CCAA': {
            //return item.CCAA?.descripcion || '';
            return (item as any).ccaa?.descripcion || '';
          }
          case 'provincia': {
            return item.provincia?.descripcion || '';
          }
          case 'municipio': {
            return item.municipio?.descripcion || '';
          }
          default: {
            const result = item[property as keyof OpePuntoControlCarretera];
            return typeof result === 'string' || typeof result === 'number' ? result : '';
          }
        }
      };
      this.dataSource.sortData = this.utilsService.getSpanishCollatorSortFn(this.dataSource.sortingDataAccessor);
      this.dataSource._updateChangeSubscription();
    }
  }

  goToEdit(puntoControlCarretera: OpePuntoControlCarretera) {
    //this.router.navigate([`fire/fire-national-edit/1`]);
  }

  goToEditPuntoControlCarretera(opePuntoControlCarretera: OpePuntoControlCarretera) {}

  goModal() {
    const dialogRef = this.dialog.open(OpePuntoControlCarreteraCreateEdit, {
      width: '90vw',
      height: '90vh',
      maxWidth: 'none',
      disableClose: true,
      data: {
        title: 'Nuevo - PuntoControlCarretera',
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
    return moment(fecha).format('DD/MM/yyyy HH:mm');
  }

  goModalEdit(opePuntoControlCarretera: OpePuntoControlCarretera) {
    const dialogRef = this.dialog.open(OpePuntoControlCarreteraCreateEdit, {
      width: '65vw',
      maxWidth: 'none',
      disableClose: true,
      data: {
        title: 'Modificar - PuntoControlCarretera.',
        opePuntoControlCarretera: opePuntoControlCarretera,
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
  async deleteOpePuntoControlCarretera(idOpePuntoControlCarretera: number) {
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

          await this.opePuntosControlCarreterasService.delete(idOpePuntoControlCarretera);
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
                this.routenav.navigate(['/ope/administracion/puntos-control-carreteras']).then(() => {
                  window.location.href = '/ope/administracion/puntos-control-carreteras';
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
