import { CommonModule } from '@angular/common';
import { AfterViewInit, Component, computed, ElementRef, Inject, inject, OnInit, signal, SimpleChanges, ViewChild } from '@angular/core';
import { FormControl, FormGroup, FormsModule, ReactiveFormsModule, Validators } from '@angular/forms';
import { FlexLayoutModule } from '@angular/flex-layout';
import { MatAutocomplete, MatAutocompleteModule, MatAutocompleteSelectedEvent, MatAutocompleteTrigger } from '@angular/material/autocomplete';
import { MatButtonModule } from '@angular/material/button';
import { DateAdapter, MAT_DATE_FORMATS, MatNativeDateModule, NativeDateAdapter } from '@angular/material/core';
import { MatDatepickerInputEvent, MatDatepickerModule } from '@angular/material/datepicker';
import { MAT_DIALOG_DATA, MatDialog, MatDialogModule, MatDialogRef } from '@angular/material/dialog';
import { MatExpansionModule } from '@angular/material/expansion';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatIconModule } from '@angular/material/icon';
import { MatInputModule } from '@angular/material/input';
import { MatSelectChange, MatSelectModule } from '@angular/material/select';
import { Router } from '@angular/router';
import { NgxSpinnerModule, NgxSpinnerService } from 'ngx-spinner';
import { DragDropModule } from '@angular/cdk/drag-drop';
import { AlertService } from '@shared/alert/alert.service';
import { LocalFiltrosOpeLineasMaritimas } from '@services/ope/administracion/local-filtro-ope-lineas-maritimas.service';
import { OpeLineasMaritimasService } from '@services/ope/administracion/ope-lineas-maritimas.service';
import moment from 'moment';
import { FechaValidator } from '@shared/validators/fecha-validator';
import { UtilsService } from '@shared/services/utils.service';
import { FECHA_MAXIMA_DATEPICKER, FECHA_MINIMA_DATEPICKER } from '@type/constants';
import { MomentDateAdapter } from '@angular/material-moment-adapter';
import { FORMATO_FECHA } from '@type/date-formats';
import { OpeFase } from '@type/ope/administracion/ope-fase.type';
import { OpeFasesService } from '@services/ope/administracion/ope-fases.service';
import { OpePuerto } from '@type/ope/administracion/ope-puerto.type';
import { OpePuertosService } from '@services/ope/administracion/ope-puertos.service';
import { OpeValidator } from '@shared/validators/ope/ope-validator';
import { MatSnackBar } from '@angular/material/snack-bar';
import { OpeErrorsService } from '@shared/services/ope/ope-errors.service';
import { OpeLineasMaritimasValidator } from '@shared/validators/ope/administracion/ope-lineas-maritimas-validator';

@Component({
  selector: 'ope-linea-maritima-create-edit',
  standalone: true,
  imports: [
    CommonModule,
    ReactiveFormsModule,
    FormsModule,
    MatDialogModule,
    MatFormFieldModule,
    MatInputModule,
    MatSelectModule,
    MatButtonModule,
    MatAutocompleteModule,
    MatIconModule,
    FlexLayoutModule,
    MatExpansionModule,
    MatDatepickerModule,
    MatNativeDateModule,
    NgxSpinnerModule,
    DragDropModule,
  ],
  providers: [
    { provide: DateAdapter, useClass: MomentDateAdapter },
    { provide: MAT_DATE_FORMATS, useValue: FORMATO_FECHA },
  ],
  templateUrl: './ope-linea-maritima-create-edit-form.component.html',
  styleUrl: './ope-linea-maritima-create-edit-form.component.scss',
})
export class OpeLineaMaritimaCreateEdit implements OnInit {
  constructor(
    private opeLineasMaritimasService: OpeLineasMaritimasService,
    private opePuertosService: OpePuertosService,
    private opeFasesService: OpeFasesService,
    public dialogRef: MatDialogRef<OpeLineaMaritimaCreateEdit>,
    public alertService: AlertService,

    @Inject(MAT_DIALOG_DATA) public data: { opeLineaMaritima: any }
  ) {}

  public opePuertosOrigen = signal<OpePuerto[]>([]);
  public opePuertosDestino = signal<OpePuerto[]>([]);
  public opeFases = signal<OpeFase[]>([]);

  public formData!: FormGroup;

  public today: string = new Date().toISOString().split('T')[0];

  public snackBar = inject(MatSnackBar);
  private spinner = inject(NgxSpinnerService);

  public utilsService = inject(UtilsService);
  public fechaMinimaDatePicker = FECHA_MINIMA_DATEPICKER;
  public fechaMaximaDatePicker = FECHA_MAXIMA_DATEPICKER;

  public opeErrorsService = inject(OpeErrorsService);

  async ngOnInit() {
    this.formData = new FormGroup(
      {
        nombre: new FormControl('', Validators.required),
        opePuertoOrigen: new FormControl('', [Validators.required, OpeValidator.opcionValidaDeSelectPorId(() => this.opePuertosOrigenFiltrados())]),
        opePuertoDestino: new FormControl('', [Validators.required, OpeValidator.opcionValidaDeSelectPorId(() => this.opePuertosDestinoFiltrados())]),
        opeFase: new FormControl('', Validators.required),
        fechaValidezDesde: new FormControl(new Date(), [Validators.required, FechaValidator.validarFecha]),
        fechaValidezHasta: new FormControl(null, [FechaValidator.validarFecha]),
        numeroRotaciones: new FormControl(null, [OpeValidator.optionalIntegerValidator()]),
        numeroPasajeros: new FormControl(null, [OpeValidator.optionalIntegerValidator()]),
        numeroTurismos: new FormControl(null, [OpeValidator.optionalIntegerValidator()]),
        numeroAutocares: new FormControl(null, [OpeValidator.optionalIntegerValidator()]),
        numeroCamiones: new FormControl(null, [OpeValidator.optionalIntegerValidator()]),
        numeroTotalVehiculos: new FormControl(null, [OpeValidator.optionalIntegerValidator()]),
      },
      {
        validators: [
          FechaValidator.validarFechaFinPosteriorFechaInicio('fechaValidezDesde', 'fechaValidezHasta'),
          OpeLineasMaritimasValidator.validarNumeroPasajerosMayorOIgualAvehiculos('numeroPasajeros', 'numeroTotalVehiculos'),
          OpeLineasMaritimasValidator.opePuertosOrigenDestinoDiferentesValidator('opePuertoOrigen', 'opePuertoDestino'),
        ],
      }
    );

    //
    const { fechaValidezDesde, fechaValidezHasta } = this.formData.value;
    const opePuertos = await this.opePuertosService.get({
      fechaValidezDesde: fechaValidezDesde ? moment(fechaValidezDesde).format('YYYY-MM-DD') : null,
      fechaValidezHasta: fechaValidezHasta ? moment(fechaValidezHasta).format('YYYY-MM-DD') : null,
    });

    this.opePuertosOrigen.set(opePuertos.data);
    this.opePuertosDestino.set(opePuertos.data);
    //

    if (this.data.opeLineaMaritima?.id) {
      //this.loadMunicipalities({ value: this.data.opeLineaMaritima.idProvincia });
      //this.loadProvinces({ value: this.data.opeLineaMaritima.idCcaa });
      //this.loadMunicipios({ value: this.data.opeLineaMaritima.idProvincia });
      this.formData.patchValue({
        id: this.data.opeLineaMaritima.id,
        nombre: this.data.opeLineaMaritima.nombre,
        opePuertoOrigen: this.data.opeLineaMaritima.idOpePuertoOrigen,
        opePuertoDestino: this.data.opeLineaMaritima.idOpePuertoDestino,
        opeFase: this.data.opeLineaMaritima.idOpeFase,
        fechaValidezDesde: moment(this.data.opeLineaMaritima.fechaValidezDesde).format('YYYY-MM-DD'),
        fechaValidezHasta: this.data.opeLineaMaritima.fechaValidezHasta
          ? moment(this.data.opeLineaMaritima.fechaValidezHasta).format('YYYY-MM-DD')
          : null,
        numeroRotaciones: this.data.opeLineaMaritima.numeroRotaciones,
        numeroPasajeros: this.data.opeLineaMaritima.numeroPasajeros,
        numeroTurismos: this.data.opeLineaMaritima.numeroTurismos,
        numeroAutocares: this.data.opeLineaMaritima.numeroAutocares,
        numeroCamiones: this.data.opeLineaMaritima.numeroCamiones,
        numeroTotalVehiculos: this.data.opeLineaMaritima.numeroTotalVehiculos,
      });
    }

    //const opePuertos = await this.opePuertosService.getAll();
    /*
    const { fechaValidezDesde, fechaValidezHasta } = this.formData.value;
    const opePuertos = await this.opePuertosService.get({
      fechaValidezDesde: fechaValidezDesde ? moment(fechaValidezDesde).format('YYYY-MM-DD') : null,
      fechaValidezHasta: fechaValidezHasta ? moment(fechaValidezHasta).format('YYYY-MM-DD') : null,
    });

    this.opePuertosOrigen.set(opePuertos.data);
    this.opePuertosDestino.set(opePuertos.data);
    */

    const opeFases = await this.opeFasesService.get();
    this.opeFases.set(opeFases);

    // Para el autocompletar de los puertos
    this.getForm('opePuertoOrigen')?.valueChanges.subscribe((value: string) => {
      if (typeof value === 'string') {
        this.filtroOpePuertoOrigen.set(value);
      }
      this.onChangePuerto(null as any);
    });
    this.getForm('opePuertoDestino')?.valueChanges.subscribe((value: string) => {
      if (typeof value === 'string') {
        this.filtroOpePuertoDestino.set(value);
      }
      this.onChangePuerto(null as any);
    });
    // FIN autocompletar
  }

  async onSubmit() {
    if (this.formData.valid) {
      this.spinner.show();
      const data = this.formData.value;

      //const municipio = this.municipalities().find((item) => item.id === data.municipality);
      if (this.data.opeLineaMaritima?.id) {
        data.id = this.data.opeLineaMaritima.id;
        await this.opeLineasMaritimasService
          .update(data)
          .then((response) => {
            // PCD
            /*
            this.snackBar
              .open('Datos modificados correctamente!', '', {
                duration: 3000,
                horizontalPosition: 'center',
                verticalPosition: 'bottom',
                panelClass: ['snackbar-verde'],
              })
              .afterDismissed()
              .subscribe(() => {
              */
            this.closeModal({ refresh: true });
            this.spinner.hide();
            //});
            // FIN PCD
          })
          .catch((error) => {
            this.spinner.hide();
            this.snackBar.open(this.opeErrorsService.obtenerMensajeError(error, 'opeLineasMaritimas'), '', {
              duration: 3000,
              horizontalPosition: 'center',
              verticalPosition: 'bottom',
              panelClass: ['snackbar-rojo'],
            });
            console.error('Error', error);
          });
      } else {
        await this.opeLineasMaritimasService
          .post(data)
          .then((response) => {
            /*
            this.snackBar
              .open('Datos creados correctamente!', '', {
                duration: 3000,
                horizontalPosition: 'center',
                verticalPosition: 'bottom',
                panelClass: ['snackbar-verde'],
              })
              .afterDismissed()
              .subscribe(() => {
              */
            this.closeModal({ refresh: true });
            this.spinner.hide();
            //});
          })
          .catch((error) => {
            this.spinner.hide();
            this.snackBar.open(this.opeErrorsService.obtenerMensajeError(error, 'opeLineasMaritimas'), '', {
              duration: 3000,
              horizontalPosition: 'center',
              verticalPosition: 'bottom',
              panelClass: ['snackbar-rojo'],
            });
            console.error('Error', error);
          });
      }
    } else {
      this.formData.markAllAsTouched();
    }
  }

  closeModal(params?: any) {
    this.dialogRef.close(params);
  }

  getForm(atributo: string): any {
    return this.formData.controls[atributo];
  }

  onChangeNumeroVehiculos(event: Event) {
    const numeroTurismos = parseInt(this.getForm('numeroTurismos').value, 10) || 0;
    const numeroAutocares = parseInt(this.getForm('numeroAutocares').value, 10) || 0;
    const numeroCamiones = parseInt(this.getForm('numeroCamiones').value, 10) || 0;

    const totalVehiculos = numeroTurismos + numeroAutocares + numeroCamiones;

    this.formData.patchValue({
      numeroTotalVehiculos: totalVehiculos,
    });
  }

  onChangePuerto(event: MatAutocompleteSelectedEvent) {
    const opePuertoOrigenId = this.getForm('opePuertoOrigen').value;
    const opePuertoDestinoId = this.getForm('opePuertoDestino').value;

    const puertoOrigen = this.opePuertosOrigen().find((p) => p.id === opePuertoOrigenId);
    const puertoDestino = this.opePuertosDestino().find((p) => p.id === opePuertoDestinoId);

    //const nombreCompuesto = `${puertoOrigen?.nombre || ''}/${puertoDestino?.nombre || ''}`;
    //
    let nombreCompuesto = '';
    if (puertoOrigen && puertoDestino) {
      nombreCompuesto = `${puertoOrigen.nombre}/${puertoDestino.nombre}`;
    } else if (puertoOrigen) {
      nombreCompuesto = `${puertoOrigen.nombre}/`;
    } else if (puertoDestino) {
      nombreCompuesto = `/${puertoDestino.nombre}`;
    }
    //
    const idOpeFasePuertoOrigen = puertoOrigen?.opeFase?.id || '';

    this.formData.patchValue({
      nombre: nombreCompuesto,
      opeFase: idOpeFasePuertoOrigen,
    });
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
    const fechaValidezDesde: Date | null = this.formData.get('fechaValidezDesde')?.value;
    const fechaValidezHasta: Date | null = this.formData.get('fechaValidezHasta')?.value;

    // Creamos el objeto de parámetros
    const params: any = {};

    // Si fechaValidezDesde tiene valor, lo añadimos al filtro; si no, se pasa null
    params.fechaValidezDesde = fechaValidezDesde ? moment(fechaValidezDesde).format('YYYY-MM-DD') : null;

    // Si fechaValidezHasta tiene valor, lo añadimos al filtro; si no, se pasa null
    params.fechaValidezHasta = fechaValidezHasta ? moment(fechaValidezHasta).format('YYYY-MM-DD') : null;

    // Llamada al servicio, pasando los parámetros con null si no se proporcionan
    try {
      const opePuertos = await this.opePuertosService.get(params);

      // Actualizamos los resultados
      this.opePuertosOrigen.set(opePuertos.data);
      this.opePuertosDestino.set(opePuertos.data);

      // Por si al cambiar la fecha el puerto seleccionado ya no está operativo
      const opePuertoOrigenSeleccionadoId = this.formData.get('opePuertoOrigen')?.value;
      const opePuertoOrigenExiste = opePuertos.data.some((p) => p.id === opePuertoOrigenSeleccionadoId);
      if (!opePuertoOrigenExiste) {
        this.formData.get('opePuertoOrigen')?.reset('');
      }

      const opePuertoDestinoSeleccionadoId = this.formData.get('opePuertoDestino')?.value;
      const opePuertoDestinoExiste = opePuertos.data.some((p) => p.id === opePuertoDestinoSeleccionadoId);
      if (!opePuertoDestinoExiste) {
        this.formData.get('opePuertoDestino')?.reset('');
      }
      //

      // Actualizamos el nombre, por si desaparece un puerto que estaba seleccionado
      this.onChangePuerto(null as any);
    } catch (error) {
      console.error('Error al obtener datos:', error);
    }
  }
  //

  // Para el autocompletar de los puertos
  filtroOpePuertoOrigen = signal('');
  filtroOpePuertoDestino = signal('');

  // Para input con objeto como valor
  displayFnOrigen = (id: number | null): string => {
    if (id == null) return '';
    const match = this.opePuertosOrigen().find((item) => item.id === id);
    return match ? match.nombre : '';
  };
  displayFnDestino = (id: number | null): string => {
    if (id == null) return '';
    const match = this.opePuertosDestino().find((item) => item.id === id);
    return match ? match.nombre : '';
  };

  // Computado para filtrar lista corta
  opePuertosOrigenFiltrados = computed(() => {
    return this.utilsService.filtrarPorTexto(this.opePuertosOrigen(), this.filtroOpePuertoOrigen(), false);
  });
  opePuertosDestinoFiltrados = computed(() => {
    return this.utilsService.filtrarPorTexto(this.opePuertosDestino(), this.filtroOpePuertoDestino(), false);
  });
  // Fin autocompletar

  getFechaFormateadaConHorasMinutosYSegundos = (fecha: any) => {
    if (!fecha) {
      return '';
    }
    return moment(fecha).format('DD/MM/YYYY HH:mm:ss');
  };
}
