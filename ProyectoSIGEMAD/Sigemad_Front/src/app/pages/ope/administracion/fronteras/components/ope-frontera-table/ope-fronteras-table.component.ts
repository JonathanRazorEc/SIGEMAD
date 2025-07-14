import { CommonModule } from '@angular/common';
import { Component, EventEmitter, inject, Input, OnChanges, Output, Renderer2, SimpleChanges, ViewChild } from '@angular/core';
import { MatProgressSpinnerModule } from '@angular/material/progress-spinner';
import { MatPaginator, MatPaginatorModule } from '@angular/material/paginator';
import moment from 'moment';
import { MatTableDataSource, MatTableModule } from '@angular/material/table';
import { MatSort, MatSortModule } from '@angular/material/sort';
import { MatDialog } from '@angular/material/dialog';
import { Router } from '@angular/router';
import { OpeFronteraCreateEdit } from '../ope-frontera-create-edit-form/ope-frontera-create-edit-form.component';
import { TooltipDirective } from '@shared/directive/tooltip/tooltip.directive';
import { AlertService } from '@shared/alert/alert.service';
import { NgxSpinnerService } from 'ngx-spinner';
import { OpeFronterasService } from '@services/ope/administracion/ope-fronteras.service';
import { MatSnackBar } from '@angular/material/snack-bar';
import { OpeFrontera } from '@type/ope/administracion/ope-frontera.type';
import { RowHighlightDirective } from '@shared/directive/ope/row-highlight.directive';
import { MatSortNoClearDirective } from '@shared/directive/ope/mat-sort-no-clear.directive';
import { UtilsService } from '@shared/services/utils.service';

@Component({
  selector: 'app-ope-fronteras-table',
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
  templateUrl: './ope-fronteras-table.component.html',
  styleUrl: './ope-fronteras-table.component.scss',
})
export class OpeFronterasTableComponent implements OnChanges {
  @Input() opeFronteras: OpeFrontera[] = [];
  @Input() isLoading: boolean = true;
  @Input() refreshFilterForm: boolean = true;

  @Output() refreshFilterFormChange = new EventEmitter<boolean>();

  public dataSource = new MatTableDataSource<OpeFrontera>([]);
  @ViewChild(MatPaginator) paginator!: MatPaginator;
  @ViewChild(MatSort) sort!: MatSort;
  public router = inject(Router);
  private dialog = inject(MatDialog);

  private spinner = inject(NgxSpinnerService);
  public renderer = inject(Renderer2);
  public alertService = inject(AlertService);
  public snackBar = inject(MatSnackBar);
  public opeFronterasService = inject(OpeFronterasService);
  public routenav = inject(Router);
  public utilsService = inject(UtilsService);

  public displayedColumns: string[] = ['nombre', 'CCAA', 'provincia', 'municipio', 'transitoMedioVehiculos', 'transitoAltoVehiculos', 'opciones'];

  ngOnChanges(changes: SimpleChanges): void {
    if (changes['opeFronteras'] && this.opeFronteras) {
      this.dataSource.data = this.opeFronteras;
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
        console.log('Propiedades del item:', Object.keys(item));
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
            const result = item[property as keyof OpeFrontera];
            return typeof result === 'string' || typeof result === 'number' ? result : '';
          }
        }
      };

      this.dataSource.sortData = this.utilsService.getSpanishCollatorSortFn(this.dataSource.sortingDataAccessor);
      this.dataSource._updateChangeSubscription();
    }
  }

  goToEdit(frontera: OpeFrontera) {
    //this.router.navigate([`fire/fire-national-edit/1`]);
  }

  goToEditFrontera(opeFrontera: OpeFrontera) {}

  goModal() {
    const dialogRef = this.dialog.open(OpeFronteraCreateEdit, {
      width: '90vw',
      height: '90vh',
      maxWidth: 'none',
      disableClose: true,
      data: {
        title: 'Nuevo - Frontera',
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

  goModalEdit(opeFrontera: OpeFrontera) {
    const dialogRef = this.dialog.open(OpeFronteraCreateEdit, {
      width: '65vw',
      maxWidth: 'none',
      disableClose: true,
      data: {
        title: 'Modificar - Frontera.',
        opeFrontera: opeFrontera,
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
  async deleteOpeFrontera(idOpeFrontera: number) {
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

          await this.opeFronterasService.delete(idOpeFrontera);
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
                this.routenav.navigate(['/ope/administracion/fronteras']).then(() => {
                  window.location.href = '/ope/administracion/fronteras';
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
