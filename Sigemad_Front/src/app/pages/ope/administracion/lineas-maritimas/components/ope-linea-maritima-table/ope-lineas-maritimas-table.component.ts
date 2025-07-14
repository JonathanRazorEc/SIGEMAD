import { CommonModule } from '@angular/common';
import { Component, EventEmitter, inject, Input, OnChanges, Output, Renderer2, SimpleChanges, ViewChild } from '@angular/core';
import { OpeLineaMaritima } from '../../../../../../types/ope/administracion/ope-linea-maritima.type';
import { MatProgressSpinnerModule } from '@angular/material/progress-spinner';
import { MatPaginator, MatPaginatorModule } from '@angular/material/paginator';
import moment from 'moment';
import { MatTableDataSource, MatTableModule } from '@angular/material/table';
import { MatSort, MatSortModule } from '@angular/material/sort';
import { MatDialog } from '@angular/material/dialog';
import { Router } from '@angular/router';
import { OpeLineaMaritimaCreateEdit } from '../ope-linea-maritima-create-edit-form/ope-linea-maritima-create-edit-form.component';
import { TooltipDirective } from '@shared/directive/tooltip/tooltip.directive';
import { AlertService } from '@shared/alert/alert.service';
import { NgxSpinnerService } from 'ngx-spinner';
import { OpeLineasMaritimasService } from '@services/ope/administracion/ope-lineas-maritimas.service';
import { MatSnackBar } from '@angular/material/snack-bar';
import { RowHighlightDirective } from '@shared/directive/ope/row-highlight.directive';
import { OpeErrorsService } from '@shared/services/ope/ope-errors.service';
import { MatCheckbox } from '@angular/material/checkbox';
import { OpePais } from '@type/ope/administracion/ope-pais.type';
import { MatSortNoClearDirective } from '@shared/directive/ope/mat-sort-no-clear.directive';
import { UtilsService } from '@shared/services/utils.service';

@Component({
  selector: 'app-ope-lineas-maritimas-table',
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
  templateUrl: './ope-lineas-maritimas-table.component.html',
  styleUrl: './ope-lineas-maritimas-table.component.scss',
})
export class OpeLineasMaritimasTableComponent implements OnChanges {
  @Input() opeLineasMaritimas: OpeLineaMaritima[] = [];
  @Input() isLoading: boolean = true;
  @Input() refreshFilterForm: boolean = true;

  //
  @Input() opePuertosPaises: OpePais[] = [];
  //

  @Output() refreshFilterFormChange = new EventEmitter<boolean>();

  public dataSource = new MatTableDataSource<OpeLineaMaritima>([]);
  @ViewChild(MatPaginator) paginator!: MatPaginator;
  @ViewChild(MatSort) sort!: MatSort;
  public router = inject(Router);
  private dialog = inject(MatDialog);

  private spinner = inject(NgxSpinnerService);
  public renderer = inject(Renderer2);
  public alertService = inject(AlertService);
  public snackBar = inject(MatSnackBar);
  public opeErrorsService = inject(OpeErrorsService);
  public opeLineasMaritimasService = inject(OpeLineasMaritimasService);
  public routenav = inject(Router);
  public utilsService = inject(UtilsService);

  public displayedColumns: string[] = [
    'nombre',
    'operativo',
    'paisOrigen',
    'opePuertoOrigen',
    'paisDestino',
    'opePuertoDestino',
    'opeFase',
    'fechaValidezDesde',
    'fechaValidezHasta',
    'opciones',
  ];

  ngOnChanges(changes: SimpleChanges): void {
    if (changes['opeLineasMaritimas'] && this.opeLineasMaritimas) {
      this.dataSource.data = this.opeLineasMaritimas;
    }

    if (changes['opePuertosPaises']) {
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
          case 'opePuertoOrigen': {
            return item.opePuertoOrigen?.nombre || '';
          }
          case 'opePuertoDestino': {
            return item.opePuertoDestino?.nombre || '';
          }
          case 'opeFase': {
            return item.opeFase?.nombre || '';
          }
          case 'paisOrigen': {
            return this.getUrlBandera(item.opePuertoOrigen.idPais) || '';
          }
          case 'paisDestino': {
            return this.getUrlBandera(item.opePuertoDestino.idPais) || '';
          }
          default: {
            const result = item[property as keyof OpeLineaMaritima];
            return typeof result === 'string' || typeof result === 'number' ? result : '';
          }
        }
      };
      this.dataSource.sortData = this.utilsService.getSpanishCollatorSortFn(this.dataSource.sortingDataAccessor);
      this.dataSource._updateChangeSubscription();
    }
  }

  goToEdit(lineaMaritima: OpeLineaMaritima) {
    //this.router.navigate([`fire/fire-national-edit/1`]);
  }

  goToEditLineaMaritima(opeLineaMaritima: OpeLineaMaritima) {}

  goModal() {
    const dialogRef = this.dialog.open(OpeLineaMaritimaCreateEdit, {
      width: '90vw',
      height: '90vh',
      maxWidth: 'none',
      disableClose: true,
      data: {
        title: 'Nuevo - Linea Maritima',
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

  goModalEdit(opeLineaMaritima: OpeLineaMaritima) {
    const dialogRef = this.dialog.open(OpeLineaMaritimaCreateEdit, {
      width: '65vw',
      maxWidth: 'none',
      disableClose: true,
      autoFocus: false,
      data: {
        title: 'Modificar - Linea Maritima.',
        opeLineaMaritima: opeLineaMaritima,
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
  async deleteOpeLineaMaritima(idOpeLineaMaritima: number) {
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

          //await this.opeLineasMaritimasService.delete(idOpeLineaMaritima);
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
                this.routenav.navigate(['/ope/administracion/lineas-maritimas']).then(() => {
                  window.location.href = '/ope/administracion/lineas-maritimas';
                });
                this.spinner.hide();
              });
            // FIN PCD
          }, 2000);
          */
          //
          try {
            await this.opeLineasMaritimasService.delete(idOpeLineaMaritima);
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
            this.snackBar.open(this.opeErrorsService.obtenerMensajeError(error, 'opeLineasMaritimas'), '', {
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

  esOperativo(opeLineaMaritima: OpeLineaMaritima): boolean {
    const hoy = new Date();
    hoy.setHours(0, 0, 0, 0);

    const desde = opeLineaMaritima.fechaValidezDesde ? new Date(opeLineaMaritima.fechaValidezDesde) : null;
    const hasta = opeLineaMaritima.fechaValidezHasta ? new Date(opeLineaMaritima.fechaValidezHasta) : null;

    if (desde) desde.setHours(0, 0, 0, 0);
    if (hasta) hasta.setHours(0, 0, 0, 0);

    return (!desde || desde <= hoy) && (!hasta || hasta >= hoy);
  }
}
