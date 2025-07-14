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
import { FormFieldComponent } from '@shared/Inputs/field.component';
import { OpeDatosEmbarquesDiariosService } from '@services/ope/datos/ope-datos-embarques-diarios.service';
import { LocalFiltrosOpeDatosEmbarquesDiarios } from '@services/ope/datos/local-filtro-ope-datos-embarques-diarios.service';
import { OpeDatoEmbarqueDiarioCreateEdit } from '../ope-dato-embarque-diario-create-edit-form/ope-dato-embarque-diario-create-edit-form.component';
import { FORMATO_FECHA } from '@type/date-formats';
import { MomentDateAdapter } from '@angular/material-moment-adapter';
import { OpeFase } from '@type/ope/administracion/ope-fase.type';
import { OpeFasesService } from '@services/ope/administracion/ope-fases.service';
import { MatSnackBar } from '@angular/material/snack-bar';
import { OpeDatoEmbarqueDiario } from '@type/ope/datos/ope-dato-embarque-diario.type';
import { OpePeriodosService } from '@services/ope/administracion/ope-periodos.service';
import { OpeLineasMaritimasService } from '@services/ope/administracion/ope-lineas-maritimas.service';
import { OpeLineaMaritima } from '@type/ope/administracion/ope-linea-maritima.type';
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
import { OpeUtilsService } from '@shared/services/ope/ope-utils.service';
import { IDS_TABLAS_LOGS_OPE } from '@type/ope/ope-constants';

@Component({
  selector: 'app-ope-dato-embarque-diario-filter-form',
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
  /*
  providers: [
    { provide: DateAdapter, useClass: MomentDateAdapter },
    { provide: MAT_DATE_FORMATS, useValue: FORMATO_FECHA },
  ],
  */
  //
  providers: [
    { provide: MAT_DATE_LOCALE, useValue: 'es' },
    { provide: DateAdapter, useClass: MomentDateAdapter, deps: [MAT_DATE_LOCALE] },
    { provide: MAT_DATE_FORMATS, useValue: FORMATO_FECHA },
  ],
  //
  templateUrl: './ope-datos-embarques-diarios-filter-form.component.html',
  styleUrl: './ope-datos-embarques-diarios-filter-form.component.scss',
})
export class OpeDatoEmbarqueDiarioFilterFormComponent implements OnInit {
  @Input() currentPage = 1;
  @Input() pageSize = 5;

  @Input() opeDatosEmbarquesDiarios: ApiResponse<OpeDatoEmbarqueDiario[]> | undefined;
  @Input() filtros: any;
  @Input() isLoading: boolean = true;
  @Input() refreshFilterForm: boolean = true;
  @Output() opeDatosEmbarquesDiariosChange = new EventEmitter<ApiResponse<OpeDatoEmbarqueDiario[]>>();
  @Output() isLoadingChange = new EventEmitter<boolean>();
  @Output() refreshFilterFormChange = new EventEmitter<boolean>();

  public filtrosOpeDatosEmbarquesDiariosService = inject(LocalFiltrosOpeDatosEmbarquesDiarios);
  public opeDatosEmbarquesDiariosService = inject(OpeDatosEmbarquesDiariosService);
  private opePeriodosService = inject(OpePeriodosService);
  private opeFasesService = inject(OpeFasesService);
  private opeLineasMaritimasService = inject(OpeLineasMaritimasService);

  private snackBar = inject(MatSnackBar);

  public menuItemActiveService = inject(MenuItemActiveService);
  private dialog = inject(MatDialog);
  public formData!: FormGroup;

  myForm!: FormGroup;

  showFilters = false;

  public opePeriodos = signal<OpePeriodo[]>([]);
  public opeFases = signal<OpeFase[]>([]);
  public opeLineasMaritimas = signal<OpeLineaMaritima[]>([]);

  public fechaMinimaDatePicker = FECHA_MINIMA_DATEPICKER;
  public fechaMaximaDatePicker = FECHA_MAXIMA_DATEPICKER;

  public utilsService = inject(UtilsService);

  // LOGS
  public opeUtilsService = inject(OpeUtilsService);
  public ID_TABLA_LOGS = IDS_TABLAS_LOGS_OPE.DATOSEMBARQUESDIARIOS;
  // FIN LOGS

  async ngOnInit() {
    const fb = new FormBuilder();
    this.myForm = fb.group({
      selectField: ['', Validators.required],
      inputField1: ['', Validators.required],
      inputField2: ['', Validators.required],
      //BUSQUEDA
      /*
      fechaDesde: [null],
      fechaHasta: [null],
      linea: [null],
      fase: [null],
      criterioNumerico: [null],
      criterioNumericoRadio: [null],
      criterioNumericoCriterioCantidad: [null],
      criterioNumericoCriterioCantidadCantidad: [null],
      */
    });

    const {
      fechaInicio,
      fechaFin,
      opePeriodo,
      opeFase,
      //opeLineaMaritima,
      opeLineasMaritimasSeleccionadas,
      criterioNumerico,
      criterioNumericoRadio,
      criterioNumericoCriterioCantidad,
      criterioNumericoCriterioCantidadCantidad,
    } = this.filtros();

    //
    const opePeriodos = await this.opePeriodosService.get();
    this.opePeriodos.set(opePeriodos.data);
    const opePeriodoPorDefecto = opePeriodo || opePeriodos.data[0]?.id || '';
    //

    this.formData = new FormGroup(
      {
        fechaInicio: new FormControl(fechaInicio ?? null, [FechaValidator.validarFecha]),
        fechaFin: new FormControl(fechaFin ?? null, [FechaValidator.validarFecha]),
        //opePeriodo: new FormControl(opePeriodo ?? ''),
        //
        opePeriodo: new FormControl(opePeriodoPorDefecto),
        //
        opeFase: new FormControl(opeFase ?? ''),
        //opeLineaMaritima: new FormControl(opeLineaMaritima ?? '', [OpeValidator.opcionValidaDeSelectPorId(() => this.opeLineasMaritimasFiltradas())]),
        opeLineasMaritimasSeleccionadas: new FormControl(
          Array.isArray(opeLineasMaritimasSeleccionadas) ? opeLineasMaritimasSeleccionadas.map((p: any) => p.id) : []
        ),
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
    this.menuItemActiveService.set.emit('/ope-datos-embarques-diarios');

    //const comparativeDates = await this.comparativeDateService.get();
    //this.comparativeDates.set(comparativeDates);

    //const opePeriodos = await this.opePeriodosService.get();
    //this.opePeriodos.set(opePeriodos.data);

    const opeFases = await this.opeFasesService.get();
    this.opeFases.set(opeFases);

    const opeLineasMaritimas = await this.opeLineasMaritimasService.get();
    this.opeLineasMaritimas.set(opeLineasMaritimas.data);

    // Para el autocompletar de las lineas maritimas
    /*
    this.getForm('opeLineaMaritima')?.valueChanges.subscribe((value: string) => {
      if (typeof value === 'string') {
        this.filtroOpeLineaMaritima.set(value);
      }
    });
    */
    // FIN autocompletar

    // Para actualizar las fechas del periodo
    if (!opePeriodo && opePeriodos.data.length > 0) {
      const eventoSimulado = { value: opePeriodoPorDefecto } as MatSelectChange;
      await this.onChangeOpePeriodo(eventoSimulado);
    }
    //

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

    this.opeDatosEmbarquesDiariosChange.emit({
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
      opeFase,
      //opeLineaMaritima,
      opeLineasMaritimasSeleccionadas,
      criterioNumerico,
      criterioNumericoRadio,
      criterioNumericoCriterioCantidad,
      criterioNumericoCriterioCantidadCantidad,
    } = this.formData.value;

    const opeDatosEmbarquesDiarios = await this.opeDatosEmbarquesDiariosService.get(
      {
        fechaInicio: fechaInicio ? moment(fechaInicio).format('YYYY-MM-DD') : null,
        fechaFin: fechaFin ? moment(fechaFin).format('YYYY-MM-DD') : null,
        IdOpePeriodo: opePeriodo,
        IdOpeFase: opeFase,
        //IdOpeLineaMaritima: opeLineaMaritima,
        IdsOpeLineasMaritimas: Array.isArray(opeLineasMaritimasSeleccionadas) ? opeLineasMaritimasSeleccionadas : [],

        criterioNumerico: criterioNumerico,
        criterioNumericoRadio: criterioNumericoRadio,
        criterioNumericoCriterioCantidad: criterioNumericoCriterioCantidad,
        criterioNumericoCriterioCantidadCantidad: criterioNumericoCriterioCantidadCantidad,
      }
      //this.currentPage,
      //this.pageSize
    );

    // Verificar que la respuesta tenga el campo `count`
    if (!opeDatosEmbarquesDiarios || typeof opeDatosEmbarquesDiarios.count !== 'number') {
      console.error('La respuesta del backend no contiene el campo `count`:', opeDatosEmbarquesDiarios);
      throw new Error('Respuesta inválida del backend');
    }

    this.filtrosOpeDatosEmbarquesDiariosService.setFilters(this.formData.value);
    this.opeDatosEmbarquesDiarios = opeDatosEmbarquesDiarios;
    this.opeDatosEmbarquesDiariosChange.emit(this.opeDatosEmbarquesDiarios);
    this.isLoadingChange.emit(false);
    this.isLoading = false;
  }

  async clearFormFilter() {
    this.formData.reset();
    this.formData.patchValue({
      opePeriodo: '',
      opeFase: '',
      opeLineaMaritima: '',
      criterioNumerico: '',
      //criterioNumericoRadioOpcion: 'maximo',
    });
    await this.actualizarLineasMaritimas();

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

  goModal() {
    const dialogRef = this.dialog.open(OpeDatoEmbarqueDiarioCreateEdit, {
      width: '65vw',
      maxWidth: 'none',
      disableClose: true,
      data: {
        title: 'Nuevo - Datos Embarque',
        fire: {},
      },
    });

    dialogRef.afterClosed().subscribe((result) => {
      /*
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
      */
      //
      if (result?.refresh) {
        console.log('Modal result:', result);
        this.onSubmit();
      }
      //
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

    await this.actualizarLineasMaritimas();
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
    await this.actualizarLineasMaritimas();
  }

  async actualizarLineasMaritimas() {
    const { fechaInicio, fechaFin, opeFase } = this.formData.value;
    const opeLineasMaritimas = await this.opeLineasMaritimasService.get({
      fechaValidezDesde: fechaInicio ? moment(fechaInicio).format('YYYY-MM-DD') : null,
      fechaValidezHasta: fechaFin ? moment(fechaFin).format('YYYY-MM-DD') : null,
      idOpeFase: opeFase,
    });
    this.opeLineasMaritimas.set(opeLineasMaritimas.data);
  }

  //
  //////////////////////excel

  exportToExcel(): void {
    this.isLoading = true;
    this.isLoadingChange.emit(true);

    const f = this.formData.value;

    const filtros = {
      estadoEliminado: 'activo' as const, // siempre exportar solo activos
      fechaDesde: f.fechaInicio ? this.formatDate(f.fechaInicio) : undefined,
      fechaHasta: f.fechaFin ? this.formatDate(f.fechaFin) : undefined,
      idLinea: f.opeLineaMaritima || undefined,
      fase: f.opeFase || undefined,
      periodo: f.opePeriodo || undefined,
      criterioNumerico: f.criterioNumerico || undefined,
      criterioNumericoRadio: f.criterioNumericoRadio || undefined,
      criterioNumericoCondicion: f.criterioNumericoCriterioCantidad || undefined,
      criterioNumericoCantidad: f.criterioNumericoCriterioCantidadCantidad || undefined,
    };

    this.opeDatosEmbarquesDiariosService.exportarExcel(filtros).subscribe({
      next: (blob) => {
        const fileName = `OpeDatosEmbarquesDiarios_${moment().format('DD-MM-YYYY_HH-mm')}.xlsx`;
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

  private formatDate(date: Date): string {
    const d = new Date(date);
    const year = d.getFullYear();
    const month = ('0' + (d.getMonth() + 1)).slice(-2);
    const day = ('0' + d.getDate()).slice(-2);
    return `${year}-${month}-${day}`;
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

  // Para el autocompletar de las lineas maritimas
  /*
  filtroOpeLineaMaritima = signal('');

  // Para input con objeto como valor
  displayFn = (id: number | null): string => {
    if (id == null) return '';
    const match = this.opeLineasMaritimas().find((item) => item.id === id);
    return match ? match.nombre : '';
  };

  // Computado para filtrar lista corta
  opeLineasMaritimasFiltradas = computed(() => {
    const texto = this.filtroOpeLineaMaritima().toLowerCase();
    return this.opeLineasMaritimas().filter((l) => l.nombre.toLowerCase().includes(texto));
  });
  */
  // Fin autocompletar
}
