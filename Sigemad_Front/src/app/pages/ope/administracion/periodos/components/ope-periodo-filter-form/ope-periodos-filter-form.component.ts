import { CommonModule } from '@angular/common';
import { Component, EventEmitter, inject, Input, OnInit, Output, signal, SimpleChanges } from '@angular/core';

import { DateAdapter, MAT_DATE_FORMATS, NativeDateAdapter } from '@angular/material/core';

import { FlexLayoutModule } from '@angular/flex-layout';
import { FormBuilder, FormControl, FormGroup, FormsModule, ReactiveFormsModule, Validators } from '@angular/forms';
import { MatAutocompleteModule } from '@angular/material/autocomplete';
import { MatButtonModule } from '@angular/material/button';
import { MatDatepickerModule } from '@angular/material/datepicker';
import { MatExpansionModule, MatExpansionPanel } from '@angular/material/expansion';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatIconModule } from '@angular/material/icon';
import { MatInputModule } from '@angular/material/input';
import { MatSelectModule } from '@angular/material/select';
import { MatDialog, MatDialogModule } from '@angular/material/dialog';
import { MenuItemActiveService } from '@services/menu-item-active.service';
import { ApiResponse } from '../../../../../../types/api-response.type';
import { OpePeriodo } from '../../../../../../types/ope/administracion/ope-periodo.type';
import { FormFieldComponent } from '@shared/Inputs/field.component';
import moment from 'moment';
import { OpePeriodosService } from '@services/ope/administracion/ope-periodos.service';
import { LocalFiltrosOpePeriodos } from '@services/ope/administracion/local-filtro-ope-periodos.service';
import { OpePeriodoCreateEdit } from '../ope-periodo-create-edit-form/ope-periodo-create-edit-form.component';
import { ComparativeDateService } from '@services/comparative-date.service';
import { ComparativeDate } from '../../../../../../types/comparative-date.type';
import { FORMATO_FECHA } from '../../../../../../types/date-formats';
import { MomentDateAdapter } from '@angular/material-moment-adapter';
import { MatSnackBar, MatSnackBarModule } from '@angular/material/snack-bar';
import saveAs from 'file-saver';
import { CatalogsComponent } from 'src/app/pages/catalogs/catalogs.component';
import { IDS_TABLAS_LOGS_OPE, NUMEROREGISTROSLOGS } from '@type/ope/ope-constants';
import { OpeUtilsService } from '@shared/services/ope/ope-utils.service';

@Component({
  selector: 'app-ope-periodo-filter-form',
  standalone: true,
  imports: [
    CommonModule,
    ReactiveFormsModule,
    FormsModule,
    FormFieldComponent,
    MatFormFieldModule,
    MatInputModule,
    MatSelectModule,
    MatButtonModule,
    MatAutocompleteModule,
    MatIconModule,
    FlexLayoutModule,
    MatExpansionModule,
    MatDatepickerModule,
    MatDialogModule,
    MatSnackBarModule,
  ],
  providers: [
    { provide: DateAdapter, useClass: MomentDateAdapter },
    { provide: MAT_DATE_FORMATS, useValue: FORMATO_FECHA },
  ],
  templateUrl: './ope-periodos-filter-form.component.html',
  styleUrl: './ope-periodos-filter-form.component.scss',
})
export class OpePeriodoFilterFormComponent implements OnInit {
  @Input() opePeriodos: ApiResponse<OpePeriodo[]> | undefined;
  @Input() filtros: any;
  @Input() isLoading: boolean = true;
  @Input() refreshFilterForm: boolean = true;
  @Output() opePeriodosChange = new EventEmitter<ApiResponse<OpePeriodo[]>>();
  @Output() isLoadingChange = new EventEmitter<boolean>();
  @Output() refreshFilterFormChange = new EventEmitter<boolean>();

  public filtrosOpePeriodosService = inject(LocalFiltrosOpePeriodos);
  public opePeriodosService = inject(OpePeriodosService);
  public comparativeDateService = inject(ComparativeDateService);

  private snackBar = inject(MatSnackBar);

  public comparativeDates = signal<ComparativeDate[]>([]);

  public menuItemActiveService = inject(MenuItemActiveService);
  private dialog = inject(MatDialog);
  public formData!: FormGroup;

  myForm!: FormGroup;

  showFilters = false;

  public showDateEnd = signal<boolean>(true);

  // LOGS
  public opeUtilsService = inject(OpeUtilsService);
  public ID_TABLA_LOGS = IDS_TABLAS_LOGS_OPE.PERIODOS;
  // FIN LOGS

  async ngOnInit() {
    const fb = new FormBuilder();
    this.myForm = fb.group({
      selectField: ['', Validators.required],
      inputField1: ['', Validators.required],
      inputField2: ['', Validators.required],
    });

    const { nombre } = this.filtros();

    this.formData = new FormGroup({
      nombre: new FormControl(nombre ?? ''),
      /*
      between: new FormControl(between ?? 1),
      fechaInicioFaseSalida: new FormControl(fechaInicioFaseSalida ?? moment().subtract(4, 'days').toDate()),
      fechaFinFaseSalida: new FormControl(fechaFinFaseSalida ?? moment().toDate()),
      fechaInicioFaseRetorno: new FormControl(fechaInicioFaseRetorno ?? moment().subtract(4, 'days').toDate()),
      fechaFinFaseRetorno: new FormControl(fechaFinFaseRetorno ?? moment().toDate()),
      */
    });

    //this.clearFormFilter();
    this.menuItemActiveService.set.emit('/ope-periodos');

    const comparativeDates = await this.comparativeDateService.get();
    this.comparativeDates.set(comparativeDates);

    this.onSubmit();
  }

  ngOnChanges(changes: SimpleChanges): void {
    if ('refreshFilterForm' in changes) {
      this.onSubmit();
    }
  }

  /*
  toggleAccordion(panel: MatExpansionPanel) {
    panel.toggle();
  }
  */

  async onSubmit() {
    if (!this.formData) {
      return;
    }

    this.opePeriodosChange.emit({
      count: 0,
      page: 1,
      pageSize: 10,
      data: [],
      pageCount: 0,
    });
    this.isLoading = true;
    this.isLoadingChange.emit(true);

    const { nombre } = this.formData.value;

    const opePeriodos = await this.opePeriodosService.get({
      /*
      IdComparativoFecha: between,
      fechaInicioFaseSalida: moment(fechaInicioFaseSalida).format('YYYY-MM-DD'),
      fechaFinFaseSalida: moment(fechaFinFaseSalida).format('YYYY-MM-DD'),
      fechaInicioFaseRetorno: moment(fechaInicioFaseRetorno).format('YYYY-MM-DD'),
      fechaFinFaseRetorno: moment(fechaFinFaseRetorno).format('YYYY-MM-DD'),
      */
      nombre: nombre,
    });
    this.filtrosOpePeriodosService.setFilters(this.formData.value);
    this.opePeriodos = opePeriodos;
    this.opePeriodosChange.emit(this.opePeriodos);
    this.isLoadingChange.emit(false);
    this.isLoading = false;
  }

  clearFormFilter() {
    this.formData.reset();
    this.formData.patchValue({
      /*
      between: 1,
      fechaInicioFaseSalida: moment().subtract(4, 'days').toDate(),
      fechaFinFaseSalida: moment().toDate(),
      fechaInicioFaseRetorno: moment().subtract(4, 'days').toDate(),
      fechaFinFaseRetorno: moment().toDate(),
      */
      nombre: '',
    });
  }

  getForm(atributo: string): any {
    return this.formData.controls[atributo];
  }

  goModal() {
    const dialogRef = this.dialog.open(OpePeriodoCreateEdit, {
      width: '65vw',
      maxWidth: 'none',
      disableClose: true,
      data: {
        title: 'Nuevo - Datos Periodo',
        fire: {},
      },
    });

    dialogRef.afterClosed().subscribe((result) => {
      if (result) {
        console.log('Modal result:', result);
        this.snackBar.open('Datos ingresados correctamente!', '', {
          duration: 3000,
          horizontalPosition: 'center',
          verticalPosition: 'bottom',
          panelClass: ['snackbar-verde'],
        });
        this.onSubmit();
      }
    });
  }

  changeBetween(event: any) {
    this.showDateEnd.set(event.value === 1 || event.value === 5 ? true : false);
  }

  //excel

  exportToExcel(): void {
    const estado = 'todos'; // o si quieres basarlo en algún control, sustitúyelo aquí
    const v = this.formData.value;

    // función helper para formatear fecha a 'YYYY-MM-DD'
    const fmt = (d: Date | null) => (d ? moment(d).format('YYYY-MM-DD') : undefined);

    // Llamada directa al servicio
    this.opePeriodosService
      .exportarExcel({
        estadoEliminado: estado,
        nombre: v.nombre,
        idOpePeriodoTipo: v.idOpePeriodoTipo,
        fechaInicioFaseSalida: fmt(v.fechaInicioFaseSalida),
        fechaFinFaseSalida: fmt(v.fechaFinFaseSalida),
        fechaInicioFaseRetorno: fmt(v.fechaInicioFaseRetorno),
        fechaFinFaseRetorno: fmt(v.fechaFinFaseRetorno),
      })
      .subscribe({
        next: (blob) => {
          const fileName = `OpePeriodos_${estado}_${moment().format('DD-MM-YYYY_HH-mm')}.xlsx`;
          saveAs(blob, fileName);
        },
        error: (err) => {
          console.error('Error al exportar a Excel', err);
        },
      });
  }
}
