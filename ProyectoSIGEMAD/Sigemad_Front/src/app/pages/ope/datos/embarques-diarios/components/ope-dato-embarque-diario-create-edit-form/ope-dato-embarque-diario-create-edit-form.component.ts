import { CommonModule } from '@angular/common';
import {
  AfterViewInit,
  ChangeDetectorRef,
  Component,
  computed,
  ElementRef,
  Inject,
  inject,
  OnInit,
  Renderer2,
  signal,
  SimpleChanges,
  ViewChild,
} from '@angular/core';
import { FormControl, FormGroup, FormGroupDirective, FormsModule, ReactiveFormsModule, Validators } from '@angular/forms';
import { FlexLayoutModule } from '@angular/flex-layout';
import { MatAutocompleteModule } from '@angular/material/autocomplete';
import { MatButtonModule } from '@angular/material/button';
import { DateAdapter, MAT_DATE_FORMATS, MatNativeDateModule, NativeDateAdapter } from '@angular/material/core';
import { MatDatepickerInputEvent, MatDatepickerModule } from '@angular/material/datepicker';
import { MAT_DIALOG_DATA, MatDialog, MatDialogModule, MatDialogRef } from '@angular/material/dialog';
import { MatExpansionModule } from '@angular/material/expansion';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatIconModule } from '@angular/material/icon';
import { MatInputModule } from '@angular/material/input';
import { MatSelectModule } from '@angular/material/select';
import { Router } from '@angular/router';
import { NgxSpinnerModule, NgxSpinnerService } from 'ngx-spinner';
import { DragDropModule } from '@angular/cdk/drag-drop';
import { AlertService } from '@shared/alert/alert.service';
import { OpeDatosEmbarquesDiariosService } from '@services/ope/datos/ope-datos-embarques-diarios.service';
import moment from 'moment';
import { FechaValidator } from '@shared/validators/fecha-validator';
import { FORMATO_FECHA } from '../../../../../../types/date-formats';
import { UtilsService } from '@shared/services/utils.service';
import { MatTableDataSource, MatTableModule } from '@angular/material/table';
import { OpeLineaMaritima } from '@type/ope/administracion/ope-linea-maritima.type';
import { OpeDatoEmbarqueDiario } from '@type/ope/datos/ope-dato-embarque-diario.type';
import { MatToolbarModule } from '@angular/material/toolbar';
import { FECHA_MAXIMA_DATEPICKER, FECHA_MINIMA_DATEPICKER } from '@type/constants';
import { OpeLineasMaritimasService } from '@services/ope/administracion/ope-lineas-maritimas.service';
import { MomentDateAdapter } from '@angular/material-moment-adapter';
import { OpeValidator } from '@shared/validators/ope/ope-validator';
import { MatSnackBar } from '@angular/material/snack-bar';
import { OpeErrorsService } from '@shared/services/ope/ope-errors.service';
import { MatPaginator, MatPaginatorModule } from '@angular/material/paginator';
import { MatSort, MatSortModule, Sort } from '@angular/material/sort';
import { SubmitOnlyErrorStateMatcher } from '@shared/utils/submit-only-error-state-matcher';
import { MatSortNoClearDirective } from '@shared/directive/ope/mat-sort-no-clear.directive';
import { TooltipDirective } from '@shared/directive/tooltip/tooltip.directive';
import { OpeDatosEmbarquesDiariosValidator } from '@shared/validators/ope/datos/ope-datos-embarques-diarios-validator';
import { OpeLineasMaritimasValidator } from '@shared/validators/ope/administracion/ope-lineas-maritimas-validator';
import { DateUtils } from '@shared/utils/date-utils';

@Component({
  selector: 'ope-dato-embarque-diario-create-edit',
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
    MatTableModule,
    MatToolbarModule,
    MatSortModule,
    MatPaginatorModule,
    MatSortNoClearDirective,
    TooltipDirective,
  ],
  providers: [
    { provide: DateAdapter, useClass: MomentDateAdapter },
    { provide: MAT_DATE_FORMATS, useValue: FORMATO_FECHA },
  ],
  templateUrl: './ope-dato-embarque-diario-create-edit-form.component.html',
  styleUrl: './ope-dato-embarque-diario-create-edit-form.component.scss',
})
export class OpeDatoEmbarqueDiarioCreateEdit implements OnInit {
  @ViewChild('formulario', { static: false }) formularioRef!: ElementRef;
  @ViewChild(FormGroupDirective) formGroupDirective!: FormGroupDirective;

  constructor(
    //private filtrosOpeDatosEmbarquesDiariosService: LocalFiltrosOpeDatosEmbarquesDiarios,
    private opeLineasMaritimasService: OpeLineasMaritimasService,
    private opeDatosEmbarquesDiariosService: OpeDatosEmbarquesDiariosService,
    public dialogRef: MatDialogRef<OpeDatoEmbarqueDiarioCreateEdit>,
    public alertService: AlertService,

    //@Inject(MAT_DIALOG_DATA) public data: { opeDatoEmbarqueDiario: any }
    @Inject(MAT_DIALOG_DATA) public data: { opeLineaMaritima: OpeLineaMaritima; opeDatoEmbarqueDiario: OpeDatoEmbarqueDiario },
    private renderer: Renderer2,
    private cdRef: ChangeDetectorRef
  ) {
    // Para validar el formulario solo al enviar
    this.submitOnlyErrorStateMatcher = new SubmitOnlyErrorStateMatcher(() => this.isSubmitted);
    // FIN Para validar el formulario solo al enviar
  }

  public opeLineasMaritimas = signal<OpeLineaMaritima[]>([]);
  public formData!: FormGroup;

  public dataSourceDatosEmbarquesDiariosRelacionados = new MatTableDataSource<any>([]);
  @ViewChild(MatPaginator) paginator!: MatPaginator;
  @ViewChild(MatSort) sort!: MatSort;
  //public opeDatosEmbarquesDiariosRelacionados = signal<OpeDatoEmbarqueDiario[]>([]);

  private spinner = inject(NgxSpinnerService);
  //public renderer = inject(Renderer2);

  public utilsService = inject(UtilsService);
  public opeErrorsService = inject(OpeErrorsService);
  public routenav = inject(Router);

  public fechaMinimaDatePicker = FECHA_MINIMA_DATEPICKER;
  public fechaMaximaDatePicker = FECHA_MAXIMA_DATEPICKER;

  public snackBar = inject(MatSnackBar);

  public displayedColumns: string[] = [
    'fecha',
    'opeLineaMaritima',
    'opeFase',
    'numeroRotaciones',
    'numeroPasajeros',
    'numeroTurismos',
    'numeroAutocares',
    'numeroCamiones',
    'numeroTotalVehiculos',
    'opciones',
  ];

  private hayCambios: boolean = false;

  // Para validar el formulario solo al enviar
  submitOnlyErrorStateMatcher: SubmitOnlyErrorStateMatcher;
  isSubmitted = false;
  // FIN Para validar el formulario solo al enviar

  async ngOnInit() {
    this.formData = new FormGroup(
      {
        opeLineaMaritima: new FormControl('', [
          Validators.required,
          OpeValidator.opcionValidaDeSelectPorId(() => this.opeLineasMaritimasFiltradas()),
        ]),
        fecha: new FormControl(moment().subtract(1, 'days').toDate(), [Validators.required, FechaValidator.validarFecha]),
        numeroRotaciones: new FormControl(null, [
          Validators.required,
          Validators.min(0),
          Validators.max(9999999),
          Validators.pattern(/^\d+$/),
          //OpeValidator.validarCampoCeroSiFechaFutura('fecha', 'numeroRotaciones'),
        ]),
        numeroPasajeros: new FormControl(null, [
          Validators.required,
          Validators.min(0),
          Validators.max(9999999),
          Validators.pattern(/^\d+$/),
          //OpeValidator.validarCampoCeroSiFechaFutura('fecha', 'numeroPasajeros'),
        ]),
        numeroTurismos: new FormControl(null, [
          Validators.required,
          Validators.min(0),
          Validators.max(9999999),
          Validators.pattern(/^\d+$/),
          //OpeValidator.validarCampoCeroSiFechaFutura('fecha', 'numeroTurismos'),
        ]),
        numeroAutocares: new FormControl(null, [
          Validators.required,
          Validators.min(0),
          Validators.max(9999999),
          Validators.pattern(/^\d+$/),
          //OpeValidator.validarCampoCeroSiFechaFutura('fecha', 'numeroAutocares'),
        ]),
        numeroCamiones: new FormControl(null, [
          Validators.required,
          Validators.min(0),
          Validators.max(9999999),
          Validators.pattern(/^\d+$/),
          //OpeValidator.validarCampoCeroSiFechaFutura('fecha', 'numeroCamiones'),
        ]),
        numeroTotalVehiculos: new FormControl(null, [
          Validators.required,
          Validators.min(0),
          Validators.max(9999999),
          Validators.pattern(/^\d+$/),
          //OpeValidator.validarCampoCeroSiFechaFutura('fecha', 'numeroTotalVehiculos'),
        ]),
      },
      {
        validators: [
          OpeLineasMaritimasValidator.validarNumeroPasajerosMayorOIgualAvehiculos('numeroPasajeros', 'numeroTotalVehiculos'),
          OpeDatosEmbarquesDiariosValidator.validarNumeroPasajerosYVehiculosSiRotacionesCero(
            'numeroRotaciones',
            'numeroPasajeros',
            'numeroTotalVehiculos'
          ),
          OpeDatosEmbarquesDiariosValidator.validarPasajerosYVehiculosSiRotacionesMayorCero(
            'numeroRotaciones',
            'numeroPasajeros',
            'numeroTotalVehiculos'
          ),
          //
          OpeDatosEmbarquesDiariosValidator.validarCamposCeroSiFechaFutura(
            ['numeroRotaciones', 'numeroPasajeros', 'numeroTurismos', 'numeroAutocares', 'numeroCamiones', 'numeroTotalVehiculos'],
            'fecha'
          ),
          //
        ],
      }
    );

    //const opeLineasMaritimas = await this.opeLineasMaritimasService.get();
    // Solo líneas máritimas operativas
    const fechaValue = this.formData.get('fecha')?.value;
    const opeLineasMaritimas = await this.opeLineasMaritimasService.get({
      fechaValidezDesde: fechaValue ? moment(fechaValue).format('YYYY-MM-DD') : null,
      fechaValidezHasta: fechaValue ? moment(fechaValue).format('YYYY-MM-DD') : null,
    });
    //
    this.opeLineasMaritimas.set(opeLineasMaritimas.data);

    if (this.data.opeDatoEmbarqueDiario?.id) {
      this.formData.patchValue({
        id: this.data.opeDatoEmbarqueDiario.id,
        opeLineaMaritima: this.data.opeDatoEmbarqueDiario.idOpeLineaMaritima,
        fecha: moment(this.data.opeDatoEmbarqueDiario.fecha).format('YYYY-MM-DD'),
        numeroRotaciones: this.data.opeDatoEmbarqueDiario.numeroRotaciones,
        numeroPasajeros: this.data.opeDatoEmbarqueDiario.numeroPasajeros,
        numeroTurismos: this.data.opeDatoEmbarqueDiario.numeroTurismos,
        numeroAutocares: this.data.opeDatoEmbarqueDiario.numeroAutocares,
        numeroCamiones: this.data.opeDatoEmbarqueDiario.numeroCamiones,
        numeroTotalVehiculos: this.data.opeDatoEmbarqueDiario.numeroTotalVehiculos,
      });
    }

    //const opeLineasMaritimas = await this.opeLineasMaritimasService.get();
    //this.opeLineasMaritimas.set(opeLineasMaritimas.data);

    const { fecha } = this.formData.value;
    const opeDatosEmbarquesDiariosRelacionados = await this.opeDatosEmbarquesDiariosService.get({
      fechaInicio: moment(fecha).format('YYYY-MM-DD'),
      fechaFin: moment(fecha).format('YYYY-MM-DD'),
      //fecha: moment(fecha).format('YYYY-MM-DD'),
    });
    this.dataSourceDatosEmbarquesDiariosRelacionados.data = opeDatosEmbarquesDiariosRelacionados.data;
    this.cdRef.detectChanges();
    this.actualizarPaginacionYOrdenacion();

    // Para el autocompletar de las lineas maritimas
    this.getForm('opeLineaMaritima')?.valueChanges.subscribe((value: string) => {
      if (typeof value === 'string') {
        this.filtroOpeLineaMaritima.set(value);
      }
    });
    // FIN autocompletar

    this.formData.get('numeroRotaciones')?.valueChanges.subscribe(() => {
      this.formData.updateValueAndValidity();
      this.formData.get('numeroPasajeros')?.markAsTouched();
      this.formData.get('numeroTotalVehiculos')?.markAsTouched();
    });
  }

  actualizarPaginacionYOrdenacion(): void {
    this.dataSourceDatosEmbarquesDiariosRelacionados.paginator = this.paginator;
    this.dataSourceDatosEmbarquesDiariosRelacionados.sort = this.sort;
    this.setDataSourceAttributes();
  }

  /*
  setDataSourceAttributes() {
    if (this.sort) {
      this.dataSourceDatosEmbarquesDiariosRelacionados.sort = this.sort;

      this.dataSourceDatosEmbarquesDiariosRelacionados.sortingDataAccessor = (item, property) => {
        switch (property) {
          case 'opeLineaMaritima': {
            return item.opeLineaMaritima?.nombre || '';
          }
          case 'opeFase': {
            return item.opeLineaMaritima?.opeFase?.nombre || '';
          }

          default: {
            const result = item[property as keyof OpeDatoEmbarqueDiario];
            return typeof result === 'string' || typeof result === 'number' ? result : '';
          }
        }
      };

      this.dataSourceDatosEmbarquesDiariosRelacionados._updateChangeSubscription();
    }
  }
  */

  //
  setDataSourceAttributes() {
    if (this.sort) {
      this.dataSourceDatosEmbarquesDiariosRelacionados.sort = this.sort;

      this.dataSourceDatosEmbarquesDiariosRelacionados.sortingDataAccessor = (item, property) => {
        switch (property) {
          /*
          case 'fecha': {
            // ORDENACION POR FECHA Y NOMBRE COMBINADA
            // Formateamos fecha a 'YYYYMMDD' para que quede ordenable por string
            const fecha = new Date(item.fecha);
            const year = fecha.getFullYear();
            const month = (fecha.getMonth() + 1).toString().padStart(2, '0');
            const day = fecha.getDate().toString().padStart(2, '0');
            const fechaFormateada = `${year}${month}${day}`;

            const nombre = (item.opeLineaMaritima?.nombre || '').toLowerCase();

            return fechaFormateada + nombre;
            // FIN ORDENACION POR FECHA Y NOMBRE COMBINADA
          }
          */
          case 'opeLineaMaritima': {
            return item.opeLineaMaritima?.nombre || '';
          }
          case 'opeFase': {
            return item.opeLineaMaritima?.opeFase?.nombre || '';
          }
          default: {
            const result = item[property as keyof OpeDatoEmbarqueDiario];
            return typeof result === 'string' || typeof result === 'number' ? result : '';
          }
        }
      };

      // ORDENACIÓN POR DEFECTO (fecha descendente + nombre ascendente)
      const collatorES = new Intl.Collator('es', { sensitivity: 'base' });
      this.dataSourceDatosEmbarquesDiariosRelacionados.sortData = (data, sort) => {
        return data.sort((a, b) => {
          if (sort.active === 'fecha') {
            //const fechaA = new Date(a.fecha).getTime();
            //const fechaB = new Date(b.fecha).getTime();
            //
            const fechaA = DateUtils.truncarFecha(a.fecha);
            const fechaB = DateUtils.truncarFecha(b.fecha);
            //

            // Compara fechas respetando la dirección elegida
            let result = fechaA < fechaB ? -1 : fechaA > fechaB ? 1 : 0;

            if (result === 0) {
              // Si la fecha es igual, compara nombres SIEMPRE en orden ascendente
              const nombreA = (a.opeLineaMaritima?.nombre || '').toLowerCase();
              const nombreB = (b.opeLineaMaritima?.nombre || '').toLowerCase();
              //result = nombreA.localeCompare(nombreB); // No aplicar inversión aquí
              //
              result = collatorES.compare(nombreA, nombreB);
              //
            } else {
              // Aplicar la dirección solo sobre la comparación de fecha
              result = sort.direction === 'asc' ? result : -result;
            }

            return result;
          }

          // Orden normal para otras columnas
          const valueA = this.dataSourceDatosEmbarquesDiariosRelacionados.sortingDataAccessor(a, sort.active);
          const valueB = this.dataSourceDatosEmbarquesDiariosRelacionados.sortingDataAccessor(b, sort.active);
          //let result = valueA < valueB ? -1 : valueA > valueB ? 1 : 0;
          //
          let result: number;

          if (typeof valueA === 'string' && typeof valueB === 'string') {
            result = collatorES.compare(valueA, valueB);
          } else {
            result = valueA < valueB ? -1 : valueA > valueB ? 1 : 0;
          }
          //
          return sort.direction === 'asc' ? result : -result;
        });
      };

      setTimeout(() => {
        this.sort.active = 'fecha';
        this.sort.direction = 'desc';
        this.sort.sortChange.emit({ active: this.sort.active, direction: this.sort.direction });
      });
      // FIN ORDENACIÓN POR DEFECTO

      this.dataSourceDatosEmbarquesDiariosRelacionados._updateChangeSubscription();
    }
  }
  //

  async onSubmit() {
    // Para validar el formulario solo al enviar
    this.isSubmitted = true;
    // FIN Para validar el formulario solo al enviar

    if (this.formData.valid) {
      this.spinner.show();
      const data = this.formData.value;

      if (this.data.opeDatoEmbarqueDiario?.id) {
        data.id = this.data.opeDatoEmbarqueDiario.id;
        await this.opeDatosEmbarquesDiariosService
          .update(data)
          .then((response: any) => {
            // Actualizamos la fecha de modificación
            this.data.opeDatoEmbarqueDiario.fechaModificacion = response.fechaModificacion;
            this.data.opeDatoEmbarqueDiario.modificadoPor = response.modificadoPor;
            // FIN Actualizamos la fecha de modificación

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
            //this.closeModal({ refresh: true });
            //this.spinner.hide();
            //});

            // Actualizar datos embarques relacionados
            const fecha = this.getForm('fecha')?.value;
            if (fecha) {
              this.procesarCambioFecha(fecha);
            }
            this.clearForm();
            this.hayCambios = true;
            this.renderer.removeClass(this.formularioRef.nativeElement, 'form-en-edicion');
            //

            // No cerrar ventana
            this.snackBar.open('Datos ingresados correctamente!', '', {
              duration: 3000,
              horizontalPosition: 'center',
              verticalPosition: 'bottom',
              panelClass: ['snackbar-verde'],
            });
            this.spinner.hide();
            // FIN No cerrar ventana
            // FIN PCD
          })
          .catch((error) => {
            this.spinner.hide();
            this.snackBar.open(this.opeErrorsService.obtenerMensajeError(error, 'opeDatosEmbarquesDiarios'), '', {
              duration: 3000,
              horizontalPosition: 'center',
              verticalPosition: 'bottom',
              panelClass: ['snackbar-rojo'],
            });
            console.error('Error', error);
          });
      } else {
        await this.opeDatosEmbarquesDiariosService
          .post(data)
          .then((response: any) => {
            // Actualizamos la fecha de creación
            this.data.opeDatoEmbarqueDiario = {
              ...data, // opcional, si quieres conservar lo enviado
              id: response.id,
              fechaCreacion: response.fechaCreacion,
              creadoPor: response.creadoPor,
            };
            // FIN Actualizamos la fecha de creación
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
            //this.closeModal({ refresh: true });
            //this.spinner.hide();
            //});

            // Actualizar datos embarques relacionados
            const fecha = this.getForm('fecha')?.value;
            if (fecha) {
              this.procesarCambioFecha(fecha);
            }
            this.clearForm();
            this.hayCambios = true;
            this.renderer.removeClass(this.formularioRef.nativeElement, 'form-en-edicion');
            //

            // No cerrar ventana
            this.snackBar.open('Datos ingresados correctamente!', '', {
              duration: 3000,
              horizontalPosition: 'center',
              verticalPosition: 'bottom',
              panelClass: ['snackbar-verde'],
            });
            this.spinner.hide();
            // FIN No cerrar ventana
          })
          .catch((error) => {
            this.spinner.hide();
            this.snackBar.open(this.opeErrorsService.obtenerMensajeError(error, 'opeDatosEmbarquesDiarios'), '', {
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

  /*
  closeModal(params?: any) {
    this.dialogRef.close(params);
  }
  */

  //
  closeModal(params?: any) {
    const paramsConRefresh = {
      ...(params || {}),
      refresh: this.hayCambios,
    };
    this.dialogRef.close(paramsConRefresh);
  }
  //

  getForm(atributo: string): any {
    return this.formData.controls[atributo];
  }

  getFechaFormateada(fecha: any) {
    if (!fecha) {
      return '';
    }
    return moment(fecha).format('DD/MM/yyyy');
  }

  async onFechaChange(event: MatDatepickerInputEvent<Date>): Promise<void> {
    if (!event.value) return; // Evita errores si la fecha está vacía
    this.procesarCambioFecha(event.value);
  }

  async onFechaChangeManual(event: Event): Promise<void> {
    const input = event.target as HTMLInputElement;
    const parsedDate = moment(input.value, 'DD/MM/YYYY', true);

    if (parsedDate.isValid()) {
      this.procesarCambioFecha(parsedDate.toDate());
    }
  }

  async procesarCambioFecha(fecha: Date): Promise<void> {
    const formattedDate = moment(fecha).format('YYYY-MM-DD');

    try {
      const opeDatosEmbarquesDiariosRelacionados = await this.opeDatosEmbarquesDiariosService.get({
        fechaInicio: formattedDate,
        fechaFin: formattedDate,
      });

      this.dataSourceDatosEmbarquesDiariosRelacionados.data = opeDatosEmbarquesDiariosRelacionados.data;
      this.cdRef.detectChanges();
      this.actualizarPaginacionYOrdenacion();
    } catch (error) {
      console.error('Error al obtener datos:', error);
    }

    // Actualizar líneas marítimas con solo líneas máritimas operativas
    const fechaValue = this.formData.get('fecha')?.value;
    const opeLineasMaritimas = await this.opeLineasMaritimasService.get({
      fechaValidezDesde: fechaValue ? moment(fechaValue).format('YYYY-MM-DD') : null,
      fechaValidezHasta: fechaValue ? moment(fechaValue).format('YYYY-MM-DD') : null,
    });
    this.opeLineasMaritimas.set(opeLineasMaritimas.data);

    // Por si al cambiar la fecha la línea martítima seleccionada ya no está operativa
    const opeLineaMaritimaSeleccionadaId = this.formData.get('opeLineaMaritima')?.value;
    const opeLineaMaritimaExiste = opeLineasMaritimas.data.some((p) => p.id === opeLineaMaritimaSeleccionadaId);
    if (!opeLineaMaritimaExiste) {
      this.formData.get('opeLineaMaritima')?.reset('');
    }
    //
  }

  editarDatoEmbarqueDiarioRelacionado(opeDatoEmbarqueDiario: OpeDatoEmbarqueDiario) {
    if (opeDatoEmbarqueDiario.id) {
      // Actualizamos `this.data` solo si es necesario
      this.data = this.data || {}; // Si `this.data` está vacío, lo inicializamos como un objeto vacío
      this.data.opeDatoEmbarqueDiario = { ...opeDatoEmbarqueDiario }; // Asignamos el objeto directamente a `opeDatoEmbarqueDiario`

      // Actualizamos los valores en el formulario
      this.formData.patchValue({
        id: opeDatoEmbarqueDiario.id,
        opeLineaMaritima: opeDatoEmbarqueDiario.idOpeLineaMaritima,
        fecha: moment(opeDatoEmbarqueDiario.fecha).format('YYYY-MM-DDTHH:mm'),
        numeroRotaciones: opeDatoEmbarqueDiario.numeroRotaciones,
        numeroPasajeros: opeDatoEmbarqueDiario.numeroPasajeros,
        numeroTurismos: opeDatoEmbarqueDiario.numeroTurismos,
        numeroAutocares: opeDatoEmbarqueDiario.numeroAutocares,
        numeroCamiones: opeDatoEmbarqueDiario.numeroCamiones,
        numeroTotalVehiculos: opeDatoEmbarqueDiario.numeroTotalVehiculos,
      });
    }

    //
    this.renderer.addClass(this.formularioRef.nativeElement, 'form-en-edicion');
    //
  }

  async deleteOpeDatoEmbarqueDiario(idOpeDatoEmbarqueDiario: number) {
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

          await this.opeDatosEmbarquesDiariosService.delete(idOpeDatoEmbarqueDiario);
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
                this.routenav.navigate(['/ope/datos/embarques-diarios']).then(() => {
                  window.location.href = '/ope/datos/embarques-diarios';
                });
                this.spinner.hide();
              });
            // FIN PCD
          }, 2000);
          */
          // TEST
          this.closeModal({ refresh: true, borrado: true });
          this.spinner.hide();
          // FIN TEST
        } else {
          this.spinner.hide();
        }
      });
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

  // Para el autocompletar de las lineas maritimas
  filtroOpeLineaMaritima = signal('');

  // Para input con objeto como valor
  displayFn = (id: number | null): string => {
    if (id == null) return '';
    const match = this.opeLineasMaritimas().find((item) => item.id === id);
    return match ? match.nombre : '';
  };

  // Computado para filtrar lista corta
  opeLineasMaritimasFiltradas = computed(() => {
    return this.utilsService.filtrarPorTexto(this.opeLineasMaritimas(), this.filtroOpeLineaMaritima(), true);
  });
  // Fin autocompletar

  onChangeNumeroRotaciones() {
    const numeroRotaciones = this.formData.get('numeroRotaciones')?.value;

    // Verifica que el valor no sea nulo antes de proceder
    if (numeroRotaciones !== null && numeroRotaciones === 0) {
      // Si el valor es 0, reseteamos otros campos
      this.formData.get('numeroPasajeros')?.setValue(0);
      this.formData.get('numeroTurismos')?.setValue(0);
      this.formData.get('numeroAutocares')?.setValue(0);
      this.formData.get('numeroCamiones')?.setValue(0);
      this.formData.get('numeroTotalVehiculos')?.setValue(0);
    }
  }

  /*
  async clearForm() {
    this.formData.reset();

    this.formData.patchValue({
      //opePeriodo: '',
      fecha: moment().subtract(1, 'days').toDate(),
    });

    // Limpia errores y estados visuales
    Object.keys(this.formData.controls).forEach((key) => {
      const control = this.formData.get(key);
      control?.setErrors(null);
    });
  }
  */

  /*
  async clearForm() {
    // Para validar el formulario solo al enviar
    this.isSubmitted = false;
    // FIN Para validar el formulario solo al enviar
    this.formGroupDirective.resetForm();

    this.formData.patchValue({
      fecha: moment().subtract(1, 'days').toDate(),
    });
  }
  */

  //
  async clearForm() {
    // Para validar el formulario solo al enviar
    this.isSubmitted = false;
    // FIN Para validar el formulario solo al enviar
    this.formGroupDirective.resetForm();

    this.formData.patchValue({
      fecha: moment().subtract(1, 'days').toDate(),
    });

    //
    // Limpia el id
    if (this.data?.opeDatoEmbarqueDiario) {
      this.data.opeDatoEmbarqueDiario.id = null;
    }
    //
  }
  //

  getFechaFormateadaConHorasMinutosYSegundos = (fecha: any) => {
    if (!fecha) {
      return '';
    }
    return moment(fecha).format('DD/MM/YYYY HH:mm:ss');
  };
}
