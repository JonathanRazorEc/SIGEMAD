import { CommonModule } from '@angular/common';
import { Component, EventEmitter, inject, Input, OnChanges, Output, Renderer2, SimpleChanges, ViewChild } from '@angular/core';
import { OpePuerto } from '../../../../../../types/ope/administracion/ope-puerto.type';
import { MatProgressSpinnerModule } from '@angular/material/progress-spinner';
import { MatPaginator, MatPaginatorModule } from '@angular/material/paginator';
import moment from 'moment';
import { MatTableDataSource, MatTableModule } from '@angular/material/table';
import { MatSort, MatSortModule } from '@angular/material/sort';
import { MatDialog } from '@angular/material/dialog';
import { Router } from '@angular/router';
import { OpePuertoCreateEdit } from '../ope-puerto-create-edit-form/ope-puerto-create-edit-form.component';
import { TooltipDirective } from '@shared/directive/tooltip/tooltip.directive';
import { AlertService } from '@shared/alert/alert.service';
import { NgxSpinnerService } from 'ngx-spinner';
import { OpePuertosService } from '@services/ope/administracion/ope-puertos.service';
import { MatSnackBar } from '@angular/material/snack-bar';
import { COUNTRIES_ID, DEFAULT_PAGESIZE, DEFAULT_PAGESIZE_OPTIONS } from '@type/constants';
import { OpePais } from '@type/ope/administracion/ope-pais.type';
import { OpeErrorsService } from '@shared/services/ope/ope-errors.service';
import { RowHighlightDirective } from '@shared/directive/ope/row-highlight.directive';
import { MatCheckbox } from '@angular/material/checkbox';
import { MatSortNoClearDirective } from '@shared/directive/ope/mat-sort-no-clear.directive';
import { UtilsService } from '@shared/services/utils.service';

@Component({
  selector: 'app-ope-puertos-table',
  standalone: true,
  imports: [
    MatProgressSpinnerModule,
    MatPaginatorModule,
    MatTableModule,
    CommonModule,
    TooltipDirective,
    MatSortModule,
    RowHighlightDirective,
    MatCheckbox,
    MatSortNoClearDirective,
  ],
  templateUrl: './ope-puertos-table.component.html',
  styleUrl: './ope-puertos-table.component.scss',
})
export class OpePuertosTableComponent implements OnChanges {
  @Input() opePuertos: OpePuerto[] = [];
  @Input() isLoading: boolean = true;
  @Input() refreshFilterForm: boolean = true;

  public pageSize = DEFAULT_PAGESIZE;
  public pageSizeOptions = DEFAULT_PAGESIZE_OPTIONS;

  //
  @Input() opePuertosPaises: OpePais[] = [];
  //

  @Output() refreshFilterFormChange = new EventEmitter<boolean>();

  public dataSource = new MatTableDataSource<OpePuerto>([]);
  @ViewChild(MatPaginator) paginator!: MatPaginator;
  @ViewChild(MatSort) sort!: MatSort;
  public router = inject(Router);
  private dialog = inject(MatDialog);

  private spinner = inject(NgxSpinnerService);
  public renderer = inject(Renderer2);
  public alertService = inject(AlertService);
  public snackBar = inject(MatSnackBar);
  public opeErrorsService = inject(OpeErrorsService);
  public opePuertosService = inject(OpePuertosService);
  public routenav = inject(Router);
  public utilsService = inject(UtilsService);

  public displayedColumns: string[] = ['pais', 'operativo', 'nombre', 'opeFase', 'fechaValidezDesde', 'fechaValidezHasta', 'capacidad', 'opciones'];

  ngOnChanges(changes: SimpleChanges): void {
    if (changes['opePuertos'] && this.opePuertos) {
      this.dataSource.data = this.opePuertos;
    }

    if (changes['opePuertosPaises']) {
      //console.log('opePuertosPaises actualizado222:', this.opePuertosPaises);
      // No se necesita hacer nada aquí, solos recibir los datos
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
          case 'operativo': {
            return this.esOperativo(item) ? 1 : 0; // Ordenar por operativo (true primero)
          }
          case 'opeFase': {
            return item.opeFase?.nombre || '';
          }
          case 'pais': {
            return this.getUrlBandera(item.idPais) || '';
          }
          default: {
            const result = item[property as keyof OpePuerto];
            return typeof result === 'string' || typeof result === 'number' ? result : '';
          }
        }
      };
      this.dataSource.sortData = this.utilsService.getSpanishCollatorSortFn(this.dataSource.sortingDataAccessor);
      this.dataSource._updateChangeSubscription();
    }
  }

  goToEdit(puerto: OpePuerto) {
    //this.router.navigate([`fire/fire-national-edit/1`]);
  }

  goToEditPuerto(opePuerto: OpePuerto) {}

  goModal() {
    const dialogRef = this.dialog.open(OpePuertoCreateEdit, {
      width: '90vw',
      height: '90vh',
      maxWidth: 'none',
      disableClose: true,
      data: {
        title: 'Nuevo - Puerto',
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

  goModalEdit(opePuerto: OpePuerto) {
    const dialogRef = this.dialog.open(OpePuertoCreateEdit, {
      width: '65vw',
      maxWidth: 'none',
      disableClose: true,
      data: {
        title: 'Modificar - Puerto.',
        opePuerto: opePuerto,
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
  async deleteOpePuerto(idOpePuerto: number) {
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

          //await this.opePuertosService.delete(idOpePuerto);
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
                this.routenav.navigate(['/ope/administracion/puertos']).then(() => {
                  window.location.href = '/ope/administracion/puertos';
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
            await this.opePuertosService.delete(idOpePuerto);
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
            this.snackBar.open(this.opeErrorsService.obtenerMensajeError(error, 'opePuertos'), '', {
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

  /*
  getUrlBandera(idPais: number) {
    if (!idPais) {
      return '';
    }

    const urlBase = '/assets/assets/img/ope/administracion';
    switch (idPais) {
      case COUNTRIES_ID.SPAIN: {
        return urlBase + '/' + 'bandera-espana.png';
      }
      case COUNTRIES_ID.MOROCCO: {
        return urlBase + '/' + 'bandera-marruecos.png';
      }
      case COUNTRIES_ID.ALGERIA: {
        return urlBase + '/' + 'bandera-argelia.png';
      }
      default: {
        return urlBase + '/' + '';
      }
    }
  }
  */

  getUrlBandera(idPais: number) {
    if (!idPais) {
      return '';
    }

    const pais = this.opePuertosPaises.find((p) => p.idPais === idPais);
    if (!pais) {
      return '';
    }

    return pais.rutaImagen;
  }

  esOperativo(opePuerto: OpePuerto): boolean {
    const hoy = new Date();
    hoy.setHours(0, 0, 0, 0);

    const desde = opePuerto.fechaValidezDesde ? new Date(opePuerto.fechaValidezDesde) : null;
    const hasta = opePuerto.fechaValidezHasta ? new Date(opePuerto.fechaValidezHasta) : null;

    if (desde) desde.setHours(0, 0, 0, 0);
    if (hasta) hasta.setHours(0, 0, 0, 0);

    return (!desde || desde <= hoy) && (!hasta || hasta >= hoy);
  }
}
