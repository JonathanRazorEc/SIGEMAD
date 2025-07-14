import { CommonModule } from '@angular/common';
import { Component, computed, EventEmitter, inject, Input, OnInit, Output, signal, SimpleChanges } from '@angular/core';
import { DateAdapter, MAT_DATE_FORMATS, MAT_DATE_LOCALE, NativeDateAdapter } from '@angular/material/core';
import { FlexLayoutModule } from '@angular/flex-layout';
import { FormBuilder, FormControl, FormGroup, FormsModule, ReactiveFormsModule, Validators } from '@angular/forms';
import { MatAutocompleteModule } from '@angular/material/autocomplete';
import { MatButtonModule } from '@angular/material/button';
import { MatDatepickerInputEvent, MatDatepickerModule } from '@angular/material/datepicker';
import { MatExpansionModule, MatExpansionPanel } from '@angular/material/expansion';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatIconModule } from '@angular/material/icon';
import { MatInputModule } from '@angular/material/input';
import { MatSelectChange, MatSelectModule } from '@angular/material/select';
import { MatDialog, MatDialogModule } from '@angular/material/dialog';
import { MenuItemActiveService } from '@services/menu-item-active.service';
import { ApiResponse } from '@type/api-response.type';
import { OpeDatosFronterasService } from '@services/ope/datos/ope-datos-fronteras.service';
import { LocalFiltrosOpeDatosFronteras } from '@services/ope/datos/local-filtro-ope-datos-fronteras.service';
import { OpeDatoFronteraCreateEdit } from '../ope-dato-frontera-create-edit-form/ope-dato-frontera-create-edit-form.component';
import { FORMATO_FECHA } from '@type/date-formats';
import { MomentDateAdapter } from '@angular/material-moment-adapter';
import { MatSnackBar } from '@angular/material/snack-bar';
import { OpeDatoFrontera } from '@type/ope/datos/ope-dato-frontera.type';
import { OpePeriodosService } from '@services/ope/administracion/ope-periodos.service';
import { OpeFronterasService } from '@services/ope/administracion/ope-fronteras.service';
import { OpeFrontera } from '@type/ope/administracion/ope-frontera.type';
import { OpePeriodo } from '@type/ope/administracion/ope-periodo.type';
import moment from 'moment';
import 'moment/locale/es';
import { FECHA_MAXIMA_DATEPICKER, FECHA_MINIMA_DATEPICKER } from '@type/constants';
import { FechaValidator } from '@shared/validators/fecha-validator';
import { saveAs } from 'file-saver';
import { MatRadioChange, MatRadioModule } from '@angular/material/radio';
import { UtilsService } from '@shared/services/utils.service';
import { OpeValidator } from '@shared/validators/ope/ope-validator';
import Swal from 'sweetalert2';
import { MultiSelectAutocompleteComponent } from '@shared/components/multi-select-autocomplete/multi-select-autocomplete.component';
import { CatalogsComponent } from 'src/app/pages/catalogs/catalogs.component';
import { IDS_TABLAS_LOGS_OPE, NUMEROREGISTROSLOGS } from '@type/ope/ope-constants';
import { OpeUtilsService } from '@shared/services/ope/ope-utils.service';

@Component({
  selector: 'app-ope-dato-frontera-filter-form',
  standalone: true,
  imports: [
    CommonModule,
    ReactiveFormsModule,
    FormsModule,
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
    MatRadioModule,
    MultiSelectAutocompleteComponent,
  ],

  providers: [
    { provide: MAT_DATE_LOCALE, useValue: 'es' },
    { provide: DateAdapter, useClass: MomentDateAdapter, deps: [MAT_DATE_LOCALE] },
    { provide: MAT_DATE_FORMATS, useValue: FORMATO_FECHA },
  ],
  //
  templateUrl: './ope-datos-fronteras-filter-form.component.html',
  styleUrl: './ope-datos-fronteras-filter-form.component.scss',
})
export class OpeDatoFronteraFilterFormComponent implements OnInit {
  @Input() currentPage = 1;
  @Input() pageSize = 5;

  @Input() opeDatosFronteras: ApiResponse<OpeDatoFrontera[]> | undefined;
  @Input() filtros: any;
  @Input() isLoading: boolean = true;
  @Input() refreshFilterForm: boolean = true;
  @Output() opeDatosFronterasChange = new EventEmitter<ApiResponse<OpeDatoFrontera[]>>();
  @Output() isLoadingChange = new EventEmitter<boolean>();
  @Output() refreshFilterFormChange = new EventEmitter<boolean>();

  public filtrosOpeDatosFronterasService = inject(LocalFiltrosOpeDatosFronteras);
  public opeDatosFronterasService = inject(OpeDatosFronterasService);
  private opePeriodosService = inject(OpePeriodosService);
  private opeFronterasService = inject(OpeFronterasService);

  private snackBar = inject(MatSnackBar);

  public menuItemActiveService = inject(MenuItemActiveService);
  private dialog = inject(MatDialog);
  public formData!: FormGroup;

  myForm!: FormGroup;

  showFilters = false;

  public opePeriodos = signal<OpePeriodo[]>([]);
  public opeFronteras = signal<OpeFrontera[]>([]);

  public fechaMinimaDatePicker = FECHA_MINIMA_DATEPICKER;
  public fechaMaximaDatePicker = FECHA_MAXIMA_DATEPICKER;

  public utilsService = inject(UtilsService);

  // LOGS
  public opeUtilsService = inject(OpeUtilsService);
  public ID_TABLA_LOGS = IDS_TABLAS_LOGS_OPE.DATOSFRONTERAS;
  // FIN LOGS

  async ngOnInit() {
    const fb = new FormBuilder();
    this.myForm = fb.group({
      selectField: ['', Validators.required],
      inputField1: ['', Validators.required],
      inputField2: ['', Validators.required],
    });

    const {
      fechaInicio,
      fechaFin,
      opePeriodo,
      //opeFrontera,
      opeFronterasSeleccionadas,
      criterioNumerico,
      criterioNumericoRadio,
      criterioNumericoCriterioCantidad,
      criterioNumericoCriterioCantidadCantidad,
    } = this.filtros();

    this.formData = new FormGroup(
      {
        fechaInicio: new FormControl(fechaInicio ?? null, [FechaValidator.validarFecha]),
        fechaFin: new FormControl(fechaFin ?? null, [FechaValidator.validarFecha]),
        opePeriodo: new FormControl(opePeriodo ?? ''),
        //opeFrontera: new FormControl(opeFrontera ?? '', [OpeValidator.opcionValidaDeSelectPorId(() => this.opeFronterasFiltrados())]),
        //
        opeFronterasSeleccionadas: new FormControl(Array.isArray(opeFronterasSeleccionadas) ? opeFronterasSeleccionadas.map((p: any) => p.id) : []),
        //
        criterioNumerico: new FormControl(criterioNumerico ?? ''),
        criterioNumericoRadio: new FormControl({ value: criterioNumericoRadio ?? 'maximo', disabled: true }),
        criterioNumericoCriterioCantidad: new FormControl({ value: criterioNumericoCriterioCantidad ?? '', disabled: true }),
        criterioNumericoCriterioCantidadCantidad: new FormControl({ value: criterioNumericoCriterioCantidadCantidad ?? '', disabled: true }),
      },
      {
        validators: [
          FechaValidator.validarFechaFinPosteriorFechaInicio('fechaInicio', 'fechaFin', true),
          OpeValidator.requiredIfCriterioNumericoRadioEnabledConCantidad('criterioNumericoRadio', 'criterioNumericoCriterioCantidad'),
          OpeValidator.requiredIfEnabled('criterioNumericoCriterioCantidad', 'criterioNumericoCriterioCantidadCantidad'),
        ],
      }
    );

    //this.clearFormFilter();
    this.menuItemActiveService.set.emit('/ope-datos-fronteras');

    //const comparativeDates = await this.comparativeDateService.get();
    //this.comparativeDates.set(comparativeDates);

    const opePeriodos = await this.opePeriodosService.get();
    this.opePeriodos.set(opePeriodos.data);

    const opeFronteras = await this.opeFronterasService.get();
    this.opeFronteras.set(opeFronteras.data);

    this.onSubmit();
  }

  ngOnChanges(changes: SimpleChanges): void {
    if ('refreshFilterForm' in changes) {
      this.onSubmit();
    }
  }

  async onSubmit() {
    if (!this.formData) {
      return;
    }
    if (!this.formData.valid) {
      return;
    }

    this.opeDatosFronterasChange.emit({
      count: 0,
      page: this.currentPage,
      pageSize: this.pageSize,
      data: [],
      pageCount: 0,
    });
    this.isLoading = true;
    this.isLoadingChange.emit(true);

    const {
      fechaInicio,
      fechaFin,
      opePeriodo,
      //opeFrontera,
      opeFronterasSeleccionadas,
      criterioNumerico,
      criterioNumericoRadio,
      criterioNumericoCriterioCantidad,
      criterioNumericoCriterioCantidadCantidad,
    } = this.formData.value;

    let criterioNumericoCriterioCantidadMapped = undefined;
    let criterioNumericoCriterioCantidadCantidadMapped = undefined;

    if (criterioNumericoRadio === 'cantidad' || criterioNumericoRadio === 'mayor' || criterioNumericoRadio === 'menor') {
      criterioNumericoCriterioCantidadMapped = criterioNumericoCriterioCantidad;
      switch (criterioNumericoCriterioCantidad) {
        case 'Igual a':
          criterioNumericoCriterioCantidadMapped = 'igual';
          break;
        case 'Mayor que':
          criterioNumericoCriterioCantidadMapped = 'mayor';
          break;
        case 'Menor que':
          criterioNumericoCriterioCantidadMapped = 'menor';
          break;
      }
      criterioNumericoCriterioCantidadCantidadMapped = criterioNumericoCriterioCantidadCantidad;
    }

    const opeDatosFronteras = await this.opeDatosFronterasService.get({
      fechaInicio: fechaInicio ? moment(fechaInicio).format('YYYY-MM-DD') : null,
      fechaFin: fechaFin ? moment(fechaFin).format('YYYY-MM-DD') : null,
      IdOpePeriodo: opePeriodo,
      //IdOpeFrontera: opeFrontera,
      IdsOpeFronteras: Array.isArray(opeFronterasSeleccionadas) ? opeFronterasSeleccionadas : [],
      criterioNumerico: criterioNumerico,
      criterioNumericoRadio: criterioNumericoRadio,
      criterioNumericoCriterioCantidad: criterioNumericoCriterioCantidadMapped,
      criterioNumericoCriterioCantidadCantidad: criterioNumericoCriterioCantidadCantidadMapped,
    });

    // Verificar que la respuesta tenga el campo `count`
    if (!opeDatosFronteras || typeof opeDatosFronteras.count !== 'number') {
      console.error('La respuesta del backend no contiene el campo `count`:', opeDatosFronteras);
      throw new Error('Respuesta inválida del backend');
    }

    this.filtrosOpeDatosFronterasService.setFilters(this.formData.value);
    this.opeDatosFronteras = opeDatosFronteras;
    this.opeDatosFronterasChange.emit(this.opeDatosFronteras);
    this.isLoadingChange.emit(false);
    this.isLoading = false;
  }

  async clearFormFilter() {
    this.formData.reset();
    this.formData.patchValue({
      opePeriodo: '',
      opeFrontera: '',
      criterioNumerico: '',
    });

    const criterioNumericoRadio = this.getForm('criterioNumericoRadio');
    const criterioNumericoCriterioCantidad = this.getForm('criterioNumericoCriterioCantidad');
    const criterioNumericoCriterioCantidadCantidad = this.getForm('criterioNumericoCriterioCantidadCantidad');

    criterioNumericoRadio.enable({ emitEvent: false });
    criterioNumericoRadio.setValue('maximo', { emitEvent: false });
    criterioNumericoRadio.disable({ emitEvent: false });
    criterioNumericoCriterioCantidad.disable();
    criterioNumericoCriterioCantidadCantidad.disable();
  }

  getForm(atributo: string): any {
    return this.formData.controls[atributo];
  }

  goModal(opeFrontera: OpeFrontera) {
    const dialogRef = this.dialog.open(OpeDatoFronteraCreateEdit, {
      width: '65vw',
      maxWidth: 'none',
      disableClose: true,
      data: {
        opeFrontera: opeFrontera,
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

  async onChangeOpePeriodo(event: MatSelectChange) {
    const selectedIdOpePeriodo = event.value;
    const selectedOpePeriodo = this.opePeriodos().find((p) => p.id === selectedIdOpePeriodo);

    if (selectedOpePeriodo) {
      this.formData.patchValue({
        fechaInicio: selectedOpePeriodo.fechaInicioFaseSalida,
        fechaFin: selectedOpePeriodo.fechaFinFaseRetorno,
      });
    } else {
      this.formData.patchValue({
        fechaInicio: null,
        fechaFin: null,
      });
    }
  }

  //
  async onFechaChange(event: MatDatepickerInputEvent<Date>): Promise<void> {
    if (!event.value) return; // Evita errores si la fecha está vacía
    this.procesarCambioFecha();
  }

  async onFechaChangeManual(event: Event): Promise<void> {
    const input = event.target as HTMLInputElement;
    const parsedDate = moment(input.value, 'DD/MM/YYYY', true);

    if (parsedDate.isValid()) {
      this.procesarCambioFecha();
    }
  }

  async procesarCambioFecha(): Promise<void> {
    const fechaInicio: Date | null = this.formData.get('fechaInicio')?.value;
    const fechaFin: Date | null = this.formData.get('fechaFin')?.value;

    const params: any = {};

    params.fechaInicio = fechaInicio ? moment(fechaInicio).format('YYYY-MM-DD') : null;
    params.fechaFin = fechaFin ? moment(fechaFin).format('YYYY-MM-DD') : null;

    if (fechaInicio && fechaFin) {
      const fechaInicioMoment = moment(fechaInicio);
      const fechaFinMoment = moment(fechaFin);

      const opePeriodo = this.opePeriodos().find((opePeriodo) => {
        const fechaInicioFaseSalida = moment(opePeriodo.fechaInicioFaseSalida);
        const fechaFinFaseRetorno = moment(opePeriodo.fechaFinFaseRetorno);

        return fechaInicioMoment.isSame(fechaInicioFaseSalida, 'day') && fechaFinMoment.isSame(fechaFinFaseRetorno, 'day');
      });

      if (opePeriodo) {
        this.formData.patchValue({
          opePeriodo: opePeriodo.id,
        });
      } else {
        this.formData.patchValue({
          opePeriodo: '',
        });
      }
    } else {
      this.formData.patchValue({
        opePeriodo: '',
      });
    }
  }

  onChangeCriterioNumerico(event: MatSelectChange) {
    const selectedValueCriterioNumerico = event.value;

    const criterioNumericoRadio = this.getForm('criterioNumericoRadio');
    const criterioNumericoCriterioCantidad = this.getForm('criterioNumericoCriterioCantidad');
    const criterioNumericoCriterioCantidadCantidad = this.getForm('criterioNumericoCriterioCantidadCantidad');

    if (selectedValueCriterioNumerico) {
      criterioNumericoRadio.enable();

      //
      if (criterioNumericoRadio.value === 'cantidad') {
        criterioNumericoCriterioCantidad.enable();
        criterioNumericoCriterioCantidadCantidad.enable();
      } else {
        criterioNumericoCriterioCantidad.disable();
        criterioNumericoCriterioCantidadCantidad.disable();
      }
      //
    } else {
      criterioNumericoRadio.disable();
      criterioNumericoCriterioCantidad.disable();
      criterioNumericoCriterioCantidadCantidad.disable();
    }
  }

  onChangeCriterioNumericoRadio(event: MatRadioChange): void {
    const selectedValue = event.value;
    const criterioNumericoCriterioCantidad = this.getForm('criterioNumericoCriterioCantidad');
    const criterioNumericoCriterioCantidadCantidad = this.getForm('criterioNumericoCriterioCantidadCantidad');
    if (selectedValue === 'cantidad') {
      criterioNumericoCriterioCantidad.enable();
      criterioNumericoCriterioCantidadCantidad.enable();
    } else {
      criterioNumericoCriterioCantidad.disable();
      criterioNumericoCriterioCantidadCantidad.disable();
    }
  }

  /*
  formatearNombreFrontera(nombre: string): string {
    if (!nombre || typeof nombre !== 'string') {
      return '';
    }
    const nombreFormateado = nombre.trim().replace(/\s+/g, '');
    return `total${nombreFormateado}`;
  }
  */ exportToExcel() {
    const f = this.formData.getRawValue();

    // Normalizar el criterioNumericoCriterioCantidad
    let criterioNumericoCriterioCantidadMapped: string | undefined = undefined;
    if (f.criterioNumericoRadio === 'cantidad' || f.criterioNumericoRadio === 'mayor' || f.criterioNumericoRadio === 'menor') {
      switch (f.criterioNumericoCriterioCantidad) {
        case 'Igual a':
          criterioNumericoCriterioCantidadMapped = 'igual';
          break;
        case 'Mayor que':
          criterioNumericoCriterioCantidadMapped = 'mayor';
          break;
        case 'Menor que':
          criterioNumericoCriterioCantidadMapped = 'menor';
          break;
        default:
          criterioNumericoCriterioCantidadMapped = f.criterioNumericoCriterioCantidad;
      }
    }

    const filtros: any = {
      fechaInicio: f.fechaInicio ? moment(f.fechaInicio).format('YYYY-MM-DD') : undefined,
      fechaFin: f.fechaFin ? moment(f.fechaFin).format('YYYY-MM-DD') : undefined,
      IdOpePeriodo: f.opePeriodo || undefined,
      IdOpeFase: f.opeFase || undefined,
      IdsOpePuertos: f.opePuertosSeleccionados?.length ? f.opePuertosSeleccionados : undefined,
      criterioNumerico: f.criterioNumerico || undefined,
      criterioNumericoRadio: f.criterioNumericoRadio || undefined,
    };

    // Solo para cantidad, mayor y menor mandamos estos dos campos
    if (f.criterioNumericoRadio === 'cantidad') {
      filtros.criterioNumericoRadio = 'cantidad';
      filtros.criterioNumericoCriterioCantidad = criterioNumericoCriterioCantidadMapped;
      filtros.criterioNumericoCriterioCantidadCantidad = f.criterioNumericoCriterioCantidadCantidad || undefined;
    }

    if (['mayor', 'menor'].includes(criterioNumericoCriterioCantidadMapped ?? '')) {
      filtros.criterioNumericoRadio = criterioNumericoCriterioCantidadMapped;
      filtros.criterioNumericoCriterioCantidadCantidad = f.criterioNumericoCriterioCantidadCantidad || undefined;
    }

    this.isLoading = true;
    this.isLoadingChange.emit(true);

    this.opeDatosFronterasService.exportarExcel(filtros).subscribe({
      next: (blob) => {
        const fileName = `OpeDatosFronteras_${moment().format('DD-MM-YYYY_HH-mm')}.xlsx`;
        console.log('Filtros exportados:', filtros);
        saveAs(blob, fileName);
        this.isLoading = false;
        this.isLoadingChange.emit(false);
      },
      error: (err) => {
        console.error('Error al exportar Excel:', err);
        this.snackBar.open('Error al exportar a Excel', '', {
          duration: 3000,
          panelClass: ['snackbar-rojo'],
        });
        this.isLoading = false;
        this.isLoadingChange.emit(false);
      },
    });
  }
}
