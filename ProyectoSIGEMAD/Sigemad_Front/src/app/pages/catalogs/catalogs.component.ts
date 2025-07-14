import { Component, Inject, OnInit, Optional, ViewChild } from '@angular/core';
import { CommonModule } from '@angular/common';
import { MatListModule } from '@angular/material/list';
import { MatInputModule } from '@angular/material/input';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatCardModule } from '@angular/material/card';
import { MatPaginator, MatPaginatorModule } from '@angular/material/paginator';
import { FormsModule } from '@angular/forms';
import { CatalogService } from '@services/catalog.service';
import { MatButtonModule } from '@angular/material/button';
import { MatIconModule } from '@angular/material/icon';
import { MatSelectModule } from '@angular/material/select';
import { MatCheckboxModule } from '@angular/material/checkbox';
import { MatDialogModule, MatDialog, MAT_DIALOG_DATA, MatDialogRef } from '@angular/material/dialog';
import { ModalCreateEditComponent } from './components/modal-edit/modal-create-edit.component';
import { MatTooltipModule } from '@angular/material/tooltip';
import Swal from 'sweetalert2';
import { ActivatedRoute } from '@angular/router';
import { RowHighlightDirective } from '@shared/directive/ope/row-highlight.directive';
import moment from 'moment';
import { DateAdapter, MAT_DATE_FORMATS } from '@angular/material/core';
import { MomentDateAdapter } from '@angular/material-moment-adapter';
import { FORMATO_FECHA } from '@type/date-formats';
import { IDS_TABLAS_MAESTRAS_GRUPOS } from '@type/constants';

@Component({
  selector: 'app-catalogs',
  standalone: true,
  imports: [
    CommonModule,
    FormsModule,
    MatListModule,
    MatPaginatorModule,
    MatInputModule,
    MatFormFieldModule,
    MatCardModule,
    MatButtonModule,
    MatIconModule,
    MatSelectModule,
    MatCheckboxModule,
    MatDialogModule,
    MatTooltipModule,
    RowHighlightDirective,
  ],
  //providers: [CatalogService],
  providers: [CatalogService, { provide: DateAdapter, useClass: MomentDateAdapter }, { provide: MAT_DATE_FORMATS, useValue: FORMATO_FECHA }],
  templateUrl: './catalogs.component.html',
  styleUrl: './catalogs.component.scss',
})
export class CatalogsComponent implements OnInit {
  catalogs: any[] = [];
  selectedCatalog: any = null;
  catalogColumns: any[] = [];
  dataSource: any[] = [];
  filterText: string = '';
  displayedColumns: string[] = [];
  selectedColumn: string = '';
  filterValue: string = '';
  showDeleted: boolean = false;
  filterApplied: boolean = false;
  filteredCatalogs: any;

  @ViewChild(MatPaginator) paginator!: MatPaginator;

  //
  idTablaMaestraGrupo: number = 1;
  idTablaInicialAMostrar: number = 0;
  mostrarListadoTablas: boolean = true;
  numeroRegistrosAMostrar: number = 25;
  mostrarFiltros: boolean = true;
  mostrarExportarExcel: boolean = true;
  mostrarPaginacion: boolean = true;
  mostrarAccionesRegistros: boolean = true;
  mostrarBottonCerrar: boolean = false;
  public IDS_TABLAS_MAESTRAS_GRUPOS = IDS_TABLAS_MAESTRAS_GRUPOS;
  //

  constructor(
    private catalogService: CatalogService,
    private dialog: MatDialog,
    private route: ActivatedRoute,
    @Optional() @Inject(MAT_DIALOG_DATA) public data: any,
    @Optional() private dialogRef?: MatDialogRef<CatalogsComponent>
  ) {
    if (data) {
      this.idTablaMaestraGrupo = data.idTablaMaestraGrupo ?? this.idTablaMaestraGrupo;
      this.idTablaInicialAMostrar = data.idTablaInicialAMostrar ?? this.idTablaInicialAMostrar;
      this.mostrarListadoTablas = data.mostrarListadoTablas ?? this.mostrarListadoTablas;
      this.numeroRegistrosAMostrar = data.numeroRegistrosAMostrar ?? this.numeroRegistrosAMostrar;
      this.mostrarFiltros = data.mostrarFiltros ?? this.mostrarFiltros;
      this.mostrarExportarExcel = data.mostrarExportarExcel ?? this.mostrarExportarExcel;
      this.mostrarPaginacion = data.mostrarPaginacion ?? this.mostrarPaginacion;
      this.mostrarAccionesRegistros = data.mostrarAccionesRegistros ?? this.mostrarAccionesRegistros;
      this.mostrarBottonCerrar = data.mostrarBottonCerrar ?? this.mostrarBottonCerrar;
    }
  }

  ngOnInit() {
    //this.loadCatalogs();
    if (!this.data) {
      this.route.paramMap.subscribe((params) => {
        const idGrupo = Number(params.get('idTablaMaestraGrupo'));
        this.idTablaMaestraGrupo = idGrupo || 1;
        this.loadCatalogs(this.idTablaMaestraGrupo);
      });
    } else {
      this.loadCatalogs(this.idTablaMaestraGrupo);
    }
  }

  loadCatalogs(IdTablaMaestraGrupo: number) {
    this.catalogService.getCatalogTables(IdTablaMaestraGrupo).subscribe({
      next: (response) => {
        this.catalogs = response;
        this.filteredCatalogs = this.catalogs;

        //
        // Aquí seleccionamos por defecto la tabla con idTablaInicialAMostrar si existe
        if (this.idTablaInicialAMostrar && this.idTablaInicialAMostrar > 0) {
          const tablaInicial = this.catalogs.find((c) => c.Id === this.idTablaInicialAMostrar);
          if (tablaInicial) {
            this.selectCatalog(tablaInicial);
          }
        }
        //
      },
      error: (err) => {
        console.error('Error loading catalogs', err);
      },
    });
  }

  selectCatalog(catalog: any) {
    this.selectedCatalog = catalog;
    this.selectedColumn = '';
    this.filterValue = '';
    this.showDeleted = false;
    this.filterApplied = false;

    if (this.paginator) {
      this.paginator.pageIndex = 0;
      this.paginator.pageSize = 25;
    }

    this.getCatalogColumns(catalog.Id);
    //this.loadCatalogItems(catalog.Id, 1, this.numeroRegistrosAMostrar);
    // Ordenamos por los logs de OPE en orden descendente
    const ordenacion = this.idTablaMaestraGrupo === IDS_TABLAS_MAESTRAS_GRUPOS.OPE_LOGS ? 'desc' : 'asc';
    const order = catalog.Id === 27 ? 'asc' : ordenacion;
    this.loadCatalogItems(catalog.Id, 1, this.numeroRegistrosAMostrar, order);
  }

  getCatalogColumns(tableId: number) {
    this.catalogService.getCatalogColumns(tableId, true).subscribe({
      next: (response) => {
        this.catalogColumns = response;
        this.displayedColumns = response.map((col: any) => col.Columna);
        this.displayedColumns.push('actions');
      },
      error: (err) => {
        console.error('Error loading catalog columns', err);
        this.catalogColumns = [];
        this.displayedColumns = [];
      },
    });
  }

  loadCatalogItems(tableId: number, page: number = 1, pageSize: number = 25, orderDirection: 'asc' | 'desc' = 'asc') {
    this.catalogService.getCatalogItems(tableId, undefined, undefined, this.showDeleted, page, pageSize, orderDirection).subscribe({
      next: (response) => {
        this.dataSource = response.items;
        if (this.paginator) {
          this.paginator.length = response.total;
        }
      },
      error: (err) => {
        console.error('Error loading catalog items', err);
        this.dataSource = [];
        this.paginator.length = 0;
      },
    });
  }

  //EXCEL
  exportarExcel() {
    if (!this.selectedCatalog) return;

    const tableId = this.selectedCatalog.Id;

    this.catalogService.exportCatalogAsExcel(tableId, false).subscribe({
      next: (blob) => {
        const url = window.URL.createObjectURL(blob);
        const a = document.createElement('a');
        a.href = url;
        a.download = `${this.selectedCatalog.Etiqueta || 'catalogo'}_${new Date().toISOString().slice(0, 10)}.xlsx`;
        document.body.appendChild(a);
        a.click();
        document.body.removeChild(a);
        window.URL.revokeObjectURL(url);
      },
      error: (err) => {
        console.error('Error al exportar Excel', err);
        Swal.fire({
          icon: 'error',
          title: 'Error',
          text: 'Ocurrió un error al exportar el archivo Excel.',
        });
      },
    });

    // Swal.fire({
    //   title: '¿Incluir registros borrados?',
    //   text: '¿Deseas incluir también los registros marcados como borrados en el Excel exportado?',
    //   icon: 'question',
    //   showCancelButton: true,
    //   confirmButtonText: 'Sí, incluir todos',
    //   cancelButtonText: 'No, solo activos',
    //   reverseButtons: true,
    // }).then((result) => {
    //   if (result.isConfirmed || result.dismiss === Swal.DismissReason.cancel) {
    //     const includeDeleted = result.isConfirmed;

    //   }
    // });
  }

  applyFilter(page: number = 1, pageSize: number = 25) {
    if (!this.selectedCatalog) return;
    const tableId = this.selectedCatalog.Id;

    if (this.paginator) {
      this.paginator.pageIndex = 0;
    }

    const params: any = {
      showDeleted: this.showDeleted.toString(),
      page: page.toString(),
      pageSize: pageSize.toString(),
    };

    if (this.selectedColumn) params.column = this.selectedColumn;
    if (this.filterValue?.trim()) params.value = this.filterValue.trim();

    this.catalogService.getCatalogItemsWithFilter(tableId, params).subscribe({
      next: (response) => {
        this.dataSource = response.items;
        this.filterApplied = true;
        if (this.paginator) this.paginator.length = response.total;
      },
      error: (err) => {
        console.error('Error applying filter', err);
        this.filterApplied = true;
      },
    });
  }

  handlePageEvent(event: any) {
    const pageIndex = event.pageIndex + 1;
    const pageSize = event.pageSize;

    if (this.selectedColumn || this.filterValue.trim()) {
      this.applyFilter(pageIndex, pageSize);
    } else {
      this.loadCatalogItems(this.selectedCatalog.Id, pageIndex, pageSize);
    }
  }

  editItem(item: any) {
    const openEditDialog = (overrideWarning: boolean = false) => {
      const dialogRef = this.dialog.open(ModalCreateEditComponent, {
        width: '750px',
        maxWidth: '95vw',
        disableClose: true,
        data: {
          //mode: 'edit',
          //
          mode: this.selectedCatalog.Editable ? 'edit' : 'view',
          //
          overrideWarning,
          columns: this.catalogColumns,
          item: item,
          autoFocus: false,
          catalogId: this.selectedCatalog.Id,
          tablaEditable: this.selectedCatalog.Editable,
          idTablaMaestraGrupo: this.idTablaMaestraGrupo,
        },
      });

      dialogRef.afterClosed().subscribe((wasSaved) => {
        if (wasSaved) {
          this.applyFilter();
        }
      });
    };

    if (!item.Editable) {
      Swal.fire({
        icon: 'warning',
        title: 'Advertencia',
        text: 'Este registro tiene cierta lógica asociada en la aplicación, no es recomendable modificarlo.',
        showCancelButton: true,
        confirmButtonText: 'Continuar',
        cancelButtonText: 'Cancelar',
      }).then((result) => {
        if (result.isConfirmed) {
          openEditDialog(true);
        }
      });
    } else {
      openEditDialog();
    }
  }

  //formatear el boolean
  formatBoolean(val: boolean): string {
    return val ? 'Sí' : 'No';
  }

  createNew() {
    if (!this.selectedCatalog) return;

    const dialogRef = this.dialog.open(ModalCreateEditComponent, {
      width: '750px',
      maxWidth: '95vw',
      disableClose: true,
      data: {
        mode: 'create',
        columns: this.catalogColumns,
        item: null,
        catalogId: this.selectedCatalog.Id,
        tablaEditable: this.selectedCatalog.Editable,
      },
    });

    dialogRef.afterClosed().subscribe((wasSaved) => {
      if (wasSaved) {
        this.applyFilter();
      }
    });
  }

  deleteItem(item: any) {
    if (!this.selectedCatalog) return;

    const itemId = item.Id;
    //const itemName = item.Descripcion;
    // Para el caso de que el item no tenga Descripcion, usamos Nombre
    const itemName = item.Descripcion !== undefined ? item.Descripcion : item.Nombre;
    //
    const isDeleted = item.Borrado;

    const actionMessage = isDeleted ? 'recuperar' : 'eliminar';
    //const confirmationMessage = `¿Estás seguro de que deseas ${actionMessage} el registro "${itemName || ''}"?`;
    const confirmationMessage = `¿Estás seguro de que deseas ${actionMessage} el registro${itemName ? ` "${itemName}"` : ''}?`;

    Swal.fire({
      icon: 'warning',
      title: 'Confirmación',
      text: confirmationMessage,
      showCancelButton: true,
      confirmButtonText: 'Continuar',
      cancelButtonText: 'Cancelar',
    }).then((result) => {
      if (!result.isConfirmed) return;

      const tableId = this.selectedCatalog.Id;

      this.catalogService.deleteCatalogItem(tableId, itemId).subscribe({
        next: () => this.applyFilter(),
        error: (err) => {
          console.error(`Error al intentar ${actionMessage} el registro`, err);
          Swal.fire({
            icon: 'error',
            title: 'Error',
            text: `Ocurrió un error al intentar ${actionMessage} el registro.`,
          });
        },
      });
    });
  }

  onColumnChange(value: string): void {
    if (value === '') {
      this.filterApplied = false;
      this.filterValue = '';
    }
  }

  exportarCSV() {
    if (!this.selectedCatalog) return;

    const tableId = this.selectedCatalog.Id;
    this.catalogService.exportCatalogAsCsv(tableId).subscribe({
      next: (blob) => {
        const url = window.URL.createObjectURL(blob);
        const a = document.createElement('a');
        a.href = url;
        a.download = `${this.selectedCatalog.Etiqueta || 'catalogo'}_${new Date().toISOString().slice(0, 10)}.csv`;
        document.body.appendChild(a);
        a.click();
        document.body.removeChild(a);
        window.URL.revokeObjectURL(url);
      },
      error: (err) => {
        console.error('Error al exportar CSV', err);
        Swal.fire({
          icon: 'error',
          title: 'Error',
          text: 'Ocurrió un error al exportar el archivo CSV.',
        });
      },
    });
  }

  getFechaFormateadaConHorasYMinutos(fecha: any) {
    if (!fecha) {
      return '';
    }
    return moment(fecha).format('DD/MM/yyyy HH:mm');
  }

  getFechaFormateadaSinHorasNiMinutos(fecha: any) {
    if (!fecha) {
      return '';
    }
    return moment(fecha).format('DD/MM/yyyy');
  }

  getSoloHorasYMinutosFormateados(fecha: string) {
    if (!fecha) return '';
    // moment.parseZone para que no cambie zona horaria, por si acaso
    return moment.parseZone(fecha, 'HH:mm:ss').format('HH:mm');
  }

  closeModal(params?: any) {
    this.dialogRef?.close();
  }

  getTipoMovimientoDescripcion(valor: string): string {
    switch (valor) {
      case 'I':
        return 'Inserción';
      case 'U':
        return 'Modificación';
      case 'D':
        return 'Borrado';
      default:
        return valor; // o 'Desconocido'
    }
  }

  getUsuarioPorTipoMovimiento(tipo: string, element: any): string {
    switch (tipo) {
      case 'I':
        return element['CreadoPor'] || '';
      case 'U':
        return element['ModificadoPor'] || '';
      case 'D':
        return element['EliminadoPor'] || '';
      default:
        return '';
    }
  }

  getCabecera(): string {
    let cabecera = 'Listado de tablas maestras y catálogos';

    switch (this.idTablaMaestraGrupo) {
      case IDS_TABLAS_MAESTRAS_GRUPOS.OPE_CATALOGO:
        cabecera = 'Catálogo OPE';
        break;
      case IDS_TABLAS_MAESTRAS_GRUPOS.OPE_LOGS:
        cabecera = 'Logs OPE';
        break;
      case IDS_TABLAS_MAESTRAS_GRUPOS.OPE_HISTORICO_SIGE2:
        cabecera = 'Historico SIGE2';
        break;
    }

    return cabecera;
  }
}
