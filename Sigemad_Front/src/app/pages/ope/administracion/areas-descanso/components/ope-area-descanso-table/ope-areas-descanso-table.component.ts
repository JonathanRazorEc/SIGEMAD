import { CommonModule } from '@angular/common';
import { Component, EventEmitter, inject, Input, OnChanges, Output, Renderer2, SimpleChanges, ViewChild } from '@angular/core';
import { MatProgressSpinnerModule } from '@angular/material/progress-spinner';
import { MatPaginator, MatPaginatorModule } from '@angular/material/paginator';
import moment from 'moment';
import { MatTableDataSource, MatTableModule } from '@angular/material/table';
import { MatSort, MatSortModule } from '@angular/material/sort';
import { MatDialog } from '@angular/material/dialog';
import { Router } from '@angular/router';
import { OpeAreaDescansoCreateEdit } from '../ope-area-descanso-create-edit-form/ope-area-descanso-create-edit-form.component';
import { TooltipDirective } from '@shared/directive/tooltip/tooltip.directive';
import { AlertService } from '@shared/alert/alert.service';
import { NgxSpinnerService } from 'ngx-spinner';
import { OpeAreasDescansoService } from '@services/ope/administracion/ope-areas-descanso.service';
import { MatSnackBar } from '@angular/material/snack-bar';
import { OpeAreaDescanso } from '@type/ope/administracion/ope-area-descanso.type';
import { RowHighlightDirective } from '@shared/directive/ope/row-highlight.directive';
import { MatSortNoClearDirective } from '@shared/directive/ope/mat-sort-no-clear.directive';
import { UtilsService } from '@shared/services/utils.service';

@Component({
  selector: 'app-ope-areas-descanso-table',
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
  templateUrl: './ope-areas-descanso-table.component.html',
  styleUrl: './ope-areas-descanso-table.component.scss',
})
export class OpeAreasDescansoTableComponent implements OnChanges {
  @Input() opeAreasDescanso: OpeAreaDescanso[] = [];
  @Input() isLoading: boolean = true;
  @Input() refreshFilterForm: boolean = true;

  @Output() refreshFilterFormChange = new EventEmitter<boolean>();

  public dataSource = new MatTableDataSource<OpeAreaDescanso>([]);
  @ViewChild(MatPaginator) paginator!: MatPaginator;
  @ViewChild(MatSort) sort!: MatSort;
  public router = inject(Router);
  private dialog = inject(MatDialog);

  private spinner = inject(NgxSpinnerService);
  public renderer = inject(Renderer2);
  public alertService = inject(AlertService);
  public snackBar = inject(MatSnackBar);
  public opeAreasDescansoService = inject(OpeAreasDescansoService);
  public routenav = inject(Router);
  public utilsService = inject(UtilsService);

  public displayedColumns: string[] = [
    'nombre',
    'opeAreaDescansoTipo',
    'CCAA',
    'provincia',
    'municipio',
    'capacidad',
    //'opeEstadoOcupacion',
    'opciones',
  ];

  ngOnChanges(changes: SimpleChanges): void {
    if (changes['opeAreasDescanso'] && this.opeAreasDescanso) {
      this.dataSource.data = this.opeAreasDescanso;
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
          case 'opeAreaDescansoTipo': {
            return item.opeAreaDescansoTipo?.nombre || '';
          }
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
          /*
          case 'opeEstadoOcupacion': {
            return item.opeEstadoOcupacion?.nombre || '';
          }
          */
          default: {
            const result = item[property as keyof OpeAreaDescanso];
            return typeof result === 'string' || typeof result === 'number' ? result : '';
          }
        }
      };

      this.dataSource.sortData = this.utilsService.getSpanishCollatorSortFn(this.dataSource.sortingDataAccessor);
      this.dataSource._updateChangeSubscription();
    }
  }

  goToEdit(frontera: OpeAreaDescanso) {
    //this.router.navigate([`fire/fire-national-edit/1`]);
  }

  goToEditAreaDescanso(opeAreaDescanso: OpeAreaDescanso) {}

  goModal() {
    const dialogRef = this.dialog.open(OpeAreaDescansoCreateEdit, {
      width: '90vw',
      height: '90vh',
      maxWidth: 'none',
      disableClose: true,
      data: {
        title: 'Nuevo - Área Descanso',
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

  goModalEdit(opeAreaDescanso: OpeAreaDescanso) {
    const dialogRef = this.dialog.open(OpeAreaDescansoCreateEdit, {
      width: '65vw',
      maxWidth: 'none',
      disableClose: true,
      data: {
        title: 'Modificar - Área Descanso.',
        opeAreaDescanso: opeAreaDescanso,
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
  async deleteOpeAreaDescanso(idOpeAreaDescanso: number) {
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

          await this.opeAreasDescansoService.delete(idOpeAreaDescanso);
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
                this.routenav.navigate(['/ope/administracion/areas-descanso']).then(() => {
                  window.location.href = '/ope/administracion/areas-descanso';
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
