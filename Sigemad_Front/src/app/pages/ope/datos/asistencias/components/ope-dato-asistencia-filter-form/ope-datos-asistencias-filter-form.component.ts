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
import { OpeDatosAsistenciasService } from '@services/ope/datos/ope-datos-asistencias.service';
import { LocalFiltrosOpeDatosAsistencias } from '@services/ope/datos/local-filtro-ope-datos-asistencias.service';
import { OpeDatoAsistenciaCreateEdit } from '../ope-dato-asistencia-create-edit-form/ope-dato-asistencia-create-edit-form.component';
import { FORMATO_FECHA } from '@type/date-formats';
import { MomentDateAdapter } from '@angular/material-moment-adapter';
import { OpeFase } from '@type/ope/administracion/ope-fase.type';
import { OpeFasesService } from '@services/ope/administracion/ope-fases.service';
import { MatSnackBar } from '@angular/material/snack-bar';
import { OpeDatoAsistencia } from '@type/ope/datos/ope-dato-asistencia.type';
import { OpePeriodosService } from '@services/ope/administracion/ope-periodos.service';
import { OpePuertosService } from '@services/ope/administracion/ope-puertos.service';
import { OpePuerto } from '@type/ope/administracion/ope-puerto.type';
import { OpePeriodo } from '@type/ope/administracion/ope-periodo.type';
import moment from 'moment';
import 'moment/locale/es';
import { COUNTRIES_ID, FECHA_MAXIMA_DATEPICKER, FECHA_MINIMA_DATEPICKER } from '@type/constants';
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
  selector: 'app-ope-dato-asistencia-filter-form',
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
  templateUrl: './ope-datos-asistencias-filter-form.component.html',
  styleUrl: './ope-datos-asistencias-filter-form.component.scss',
})
export class OpeDatoAsistenciaFilterFormComponent implements OnInit {
  @Input() currentPage = 1;
  @Input() pageSize = 5;

  @Input() opeDatosAsistencias: ApiResponse<OpeDatoAsistencia[]> | undefined;
  @Input() filtros: any;
  @Input() isLoading: boolean = true;
  @Input() refreshFilterForm: boolean = true;
  @Output() opeDatosAsistenciasChange = new EventEmitter<ApiResponse<OpeDatoAsistencia[]>>();
  @Output() isLoadingChange = new EventEmitter<boolean>();
  @Output() refreshFilterFormChange = new EventEmitter<boolean>();

  public filtrosOpeDatosAsistenciasService = inject(LocalFiltrosOpeDatosAsistencias);
  public opeDatosAsistenciasService = inject(OpeDatosAsistenciasService);
  private opePeriodosService = inject(OpePeriodosService);
  private opeFasesService = inject(OpeFasesService);
  private opePuertosService = inject(OpePuertosService);

  private snackBar = inject(MatSnackBar);

  public menuItemActiveService = inject(MenuItemActiveService);
  private dialog = inject(MatDialog);
  public formData!: FormGroup;

  myForm!: FormGroup;

  showFilters = false;

  public opePeriodos = signal<OpePeriodo[]>([]);
  public opeFases = signal<OpeFase[]>([]);
  public opePuertos = signal<OpePuerto[]>([]);

  public fechaMinimaDatePicker = FECHA_MINIMA_DATEPICKER;
  public fechaMaximaDatePicker = FECHA_MAXIMA_DATEPICKER;

  public utilsService = inject(UtilsService);

  // LOGS
  public opeUtilsService = inject(OpeUtilsService);
  public ID_TABLA_LOGS = IDS_TABLAS_LOGS_OPE.DATOSASISTENCIAS;
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
      opeFase,
      //opePuerto,
      opePuertosSeleccionados,
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
        opeFase: new FormControl(opeFase ?? ''),
        //opePuerto: new FormControl(opePuerto ?? '', [OpeValidator.opcionValidaDeSelectPorId(() => this.opePuertosFiltrados())]),
        //
        opePuertosSeleccionados: new FormControl(Array.isArray(opePuertosSeleccionados) ? opePuertosSeleccionados.map((p: any) => p.id) : []),
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
    this.menuItemActiveService.set.emit('/ope-datos-asistencias');

    //const comparativeDates = await this.comparativeDateService.get();
    //this.comparativeDates.set(comparativeDates);

    const opePeriodos = await this.opePeriodosService.get();
    this.opePeriodos.set(opePeriodos.data);

    const opeFases = await this.opeFasesService.get();
    this.opeFases.set(opeFases);

    //const opePuertos = await this.opePuertosService.get();
    // Solo puertos de España
    const opePuertos = await this.opePuertosService.get({
      idPais: COUNTRIES_ID.SPAIN,
    });
    //
    this.opePuertos.set(opePuertos.data);

    // Para el autocompletar de las lineas maritimas
    /*
    this.getForm('opePuerto')?.valueChanges.subscribe((value: string) => {
      if (typeof value === 'string') {
        this.filtroOpePuerto.set(value);
      }
    });
    */
    // FIN autocompletar

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

    this.opeDatosAsistenciasChange.emit({
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
      //opePuerto,
      opePuertosSeleccionados,
      criterioNumerico,
      criterioNumericoRadio,
      criterioNumericoCriterioCantidad,
      criterioNumericoCriterioCantidadCantidad,
    } = this.formData.value;

    let criterioNumericoCriterioCantidadMapped = undefined;
    let criterioNumericoCriterioCantidadCantidadMapped = undefined;

    if (criterioNumericoRadio === 'cantidad') {
      // Solo para 'cantidad' mapea la condición
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
    const opeDatosAsistencias = await this.opeDatosAsistenciasService.get({
      fechaInicio: fechaInicio ? moment(fechaInicio).format('YYYY-MM-DD') : null,
      fechaFin: fechaFin ? moment(fechaFin).format('YYYY-MM-DD') : null,
      IdOpePeriodo: opePeriodo,
      IdOpeFase: opeFase,
      IdsOpePuertos: Array.isArray(opePuertosSeleccionados) ? opePuertosSeleccionados : [],
      criterioNumerico,
      criterioNumericoRadio,
      criterioNumericoCriterioCantidadCantidad: criterioNumericoCriterioCantidadCantidadMapped,
      criterioNumericoCriterioCantidad: criterioNumericoCriterioCantidadMapped,
    });
    //this.currentPage,
    //this.pageSize

    // Verificar que la respuesta tenga el campo `count`
    if (!opeDatosAsistencias || typeof opeDatosAsistencias.count !== 'number') {
      console.error('La respuesta del backend no contiene el campo `count`:', opeDatosAsistencias);
      throw new Error('Respuesta inválida del backend');
    }

    this.filtrosOpeDatosAsistenciasService.setFilters(this.formData.value);
    this.opeDatosAsistencias = opeDatosAsistencias;
    this.opeDatosAsistenciasChange.emit(this.opeDatosAsistencias);
    this.isLoadingChange.emit(false);
    this.isLoading = false;
  }

  async clearFormFilter() {
    this.formData.reset();
    this.formData.patchValue({
      opePeriodo: '',
      opeFase: '',
      opePuerto: '',
      criterioNumerico: '',
      //criterioNumericoRadioOpcion: 'maximo',
    });
    await this.actualizarPuertos();

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
    const dialogRef = this.dialog.open(OpeDatoAsistenciaCreateEdit, {
      width: '65vw',
      maxWidth: 'none',
      disableClose: true,
      data: {
        title: 'Nuevo - Datos Embarque',
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

    await this.actualizarPuertos();
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
    await this.actualizarPuertos();
  }

  async actualizarPuertos() {
    const { fechaInicio, fechaFin, opeFase } = this.formData.value;
    const opePuertos = await this.opePuertosService.get({
      fechaValidezDesde: fechaInicio ? moment(fechaInicio).format('YYYY-MM-DD') : null,
      fechaValidezHasta: fechaFin ? moment(fechaFin).format('YYYY-MM-DD') : null,
      idOpeFase: opeFase,
    });
    this.opePuertos.set(opePuertos.data);
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

  // Para el autocompletar de las lineas maritimas
  filtroOpePuerto = signal('');

  // Para input con objeto como valor
  displayFn = (id: number | null): string => {
    if (id == null) return '';
    const match = this.opePuertos().find((item) => item.id === id);
    return match ? match.nombre : '';
  };

  // Computado para filtrar lista corta
  /*
  opePuertosFiltrados = computed(() => {
    const texto = this.filtroOpePuerto().toLowerCase();
    return this.opePuertos().filter((l) => l.nombre.toLowerCase().includes(texto));
  });
  */
  // Fin autocompletar

  //EXCEL
  exportToExcel() {
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

    this.opeDatosAsistenciasService.exportarExcel(filtros).subscribe({
      next: (blob) => {
        const fileName = `OpeDatosAsistencias_${moment().format('DD-MM-YYYY_HH-mm')}.xlsx`;
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
