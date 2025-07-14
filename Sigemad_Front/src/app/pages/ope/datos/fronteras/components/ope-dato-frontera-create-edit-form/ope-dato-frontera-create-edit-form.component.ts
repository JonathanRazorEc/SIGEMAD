import { CommonModule } from '@angular/common';
import { ChangeDetectorRef, Component, Inject, inject, OnInit, Renderer2, signal, SimpleChanges, ViewChild } from '@angular/core';
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
import { MatSelectChange, MatSelectModule } from '@angular/material/select';
import { Router } from '@angular/router';
import { NgxSpinnerModule, NgxSpinnerService } from 'ngx-spinner';
import { MatSnackBar } from '@angular/material/snack-bar';
import { DragDropModule } from '@angular/cdk/drag-drop';
import { AlertService } from '@shared/alert/alert.service';
import { OpeDatosFronterasService } from '@services/ope/datos/ope-datos-fronteras.service';
import moment from 'moment';
import { FechaValidator } from '@shared/validators/fecha-validator';
import { FORMATO_FECHA } from '../../../../../../types/date-formats';
import { UtilsService } from '@shared/services/utils.service';
import { MatTableDataSource, MatTableModule } from '@angular/material/table';
import { OpeFrontera } from '@type/ope/administracion/ope-frontera.type';
import { OpeDatoFrontera } from '@type/ope/datos/ope-dato-frontera.type';
import { MatToolbarModule } from '@angular/material/toolbar';
import { FECHA_MAXIMA_DATEPICKER, FECHA_MAXIMA_DATETIME, FECHA_MINIMA_DATEPICKER, FECHA_MINIMA_DATETIME } from '@type/constants';
import { OpeDatosFronterasIntervalosHorariosService } from '@services/ope/datos/ope-datos-fronteras-intervalos-horarios.service';
import { OpeDatoFronteraIntervaloHorario } from '@type/ope/datos/ope-dato-frontera-intervalo-horario.type';
import { MomentDateAdapter } from '@angular/material-moment-adapter';
import { MatCheckboxModule } from '@angular/material/checkbox';
import { OpeErrorsService } from '@shared/services/ope/ope-errors.service';
import { MatPaginator, MatPaginatorModule } from '@angular/material/paginator';
import { MatSort, MatSortModule } from '@angular/material/sort';
import { MatSortNoClearDirective } from '@shared/directive/ope/mat-sort-no-clear.directive';
import { TooltipDirective } from '@shared/directive/tooltip/tooltip.directive';

@Component({
  selector: 'ope-dato-frontera-create-edit',
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
    MatCheckboxModule,
    MatSortModule,
    MatPaginatorModule,
    MatSortNoClearDirective,
    TooltipDirective,
  ],
  providers: [
    { provide: DateAdapter, useClass: MomentDateAdapter },
    { provide: MAT_DATE_FORMATS, useValue: FORMATO_FECHA },
  ],
  templateUrl: './ope-dato-frontera-create-edit-form.component.html',
  styleUrl: './ope-dato-frontera-create-edit-form.component.scss',
})
export class OpeDatoFronteraCreateEdit implements OnInit {
  constructor(
    //private filtrosOpeDatosFronterasService: LocalFiltrosOpeDatosFronteras,
    private opeDatosFronterasService: OpeDatosFronterasService,
    private opeDatosFronterasIntervalosHorariosService: OpeDatosFronterasIntervalosHorariosService,
    public dialogRef: MatDialogRef<OpeDatoFronteraCreateEdit>,
    public alertService: AlertService,
    @Inject(MAT_DIALOG_DATA) public data: { opeFrontera: OpeFrontera; opeDatoFrontera: OpeDatoFrontera },
    private cdRef: ChangeDetectorRef
  ) {}

  public opeDatosFronterasIntervalosHorarios = signal<OpeDatoFronteraIntervaloHorario[]>([]);

  public formData!: FormGroup;

  public dataSourceDatosFronterasRelacionados = new MatTableDataSource<any>([]);
  @ViewChild(MatPaginator) paginator!: MatPaginator;
  @ViewChild(MatSort) sort!: MatSort;
  //public opeDatosFronterasRelacionados = signal<OpeDatoFrontera[]>([]);

  private spinner = inject(NgxSpinnerService);
  public renderer = inject(Renderer2);

  public snackBar = inject(MatSnackBar);
  public utilsService = inject(UtilsService);
  public opeErrorsService = inject(OpeErrorsService);
  public routenav = inject(Router);

  public fechaMinimaDateTime = FECHA_MINIMA_DATETIME;
  public fechaMaximaDateTime = FECHA_MAXIMA_DATETIME;

  public displayedColumns: string[] = ['fechaInicio', 'fechaFin', 'numeroVehiculos', 'afluencia', 'opciones'];

  public fechaMinimaDatePicker = FECHA_MINIMA_DATEPICKER;
  public fechaMaximaDatePicker = FECHA_MAXIMA_DATEPICKER;

  async ngOnInit() {
    this.formData = new FormGroup(
      {
        //opeFrontera: new FormControl('', Validators.required),
        fecha: new FormControl(moment().format('YYYY-MM-DD'), [
          Validators.required,
          FechaValidator.validarFechaPosteriorHoy(),
          FechaValidator.validarFecha,
        ]),
        opeDatoFronteraIntervaloHorario: new FormControl('', Validators.required),
        intervaloHorarioPersonalizado: new FormControl({ value: false, disabled: true }),
        inicioIntervaloHorarioPersonalizado: new FormControl({ value: '', disabled: true }, [Validators.pattern(/^([0-1]\d|2[0-3]):([0-5]\d)$/)]),
        finIntervaloHorarioPersonalizado: new FormControl({ value: '', disabled: true }, [Validators.pattern(/^([0-1]\d|2[0-3]):([0-5]\d)$/)]),
        numeroVehiculos: new FormControl(null, [Validators.required, Validators.min(0), Validators.max(9999999), Validators.pattern(/^\d+$/)]),
        afluencia: new FormControl(''),
      },
      {
        validators: [
          FechaValidator.validarHoraFinPosteriorHoraInicio('inicioIntervaloHorarioPersonalizado', 'finIntervaloHorarioPersonalizado'),
          FechaValidator.validarHoraDentroRangoIntervalo('inicioIntervaloHorarioPersonalizado', 'opeDatoFronteraIntervaloHorario', () =>
            this.opeDatosFronterasIntervalosHorarios()
          ),
          FechaValidator.validarHoraDentroRangoIntervalo('finIntervaloHorarioPersonalizado', 'opeDatoFronteraIntervaloHorario', () =>
            this.opeDatosFronterasIntervalosHorarios()
          ),
        ],
      }
    );

    if (this.data.opeDatoFrontera?.id) {
      this.formData.patchValue({
        id: this.data.opeDatoFrontera.id,
        fecha: moment(this.data.opeDatoFrontera.fecha).format('YYYY-MM-DD'),
        opeDatoFronteraIntervaloHorario: this.data.opeDatoFrontera.idOpeDatoFronteraIntervaloHorario,
        intervaloHorarioPersonalizado: this.data.opeDatoFrontera.intervaloHorarioPersonalizado,
        inicioIntervaloHorarioPersonalizado: this.data.opeDatoFrontera.inicioIntervaloHorarioPersonalizado,
        finIntervaloHorarioPersonalizado: this.data.opeDatoFrontera.finIntervaloHorarioPersonalizado,
        numeroVehiculos: this.data.opeDatoFrontera.numeroVehiculos,
        afluencia: this.data.opeDatoFrontera.afluencia,
      });

      this.getForm('intervaloHorarioPersonalizado').enable();

      if (this.data.opeDatoFrontera.intervaloHorarioPersonalizado) {
        this.formData.get('inicioIntervaloHorarioPersonalizado')?.enable();
        this.formData.get('finIntervaloHorarioPersonalizado')?.enable();
      }
    }

    const opeDatosFronterasIntervalosHorarios = await this.opeDatosFronterasIntervalosHorariosService.get();
    this.opeDatosFronterasIntervalosHorarios.set(opeDatosFronterasIntervalosHorarios);

    const { fecha } = this.formData.value;
    const opeDatosFronterasRelacionados = await this.opeDatosFronterasService.get({
      fechaInicio: moment(fecha).format('YYYY-MM-DD'),
      fechaFin: moment(fecha).format('YYYY-MM-DD'),
      idsOpeFronteras: [this.data.opeFrontera.id],
    });
    //this.opeDatosFronterasRelacionados.set(opeDatosFronterasRelacionados.data);
    this.dataSourceDatosFronterasRelacionados.data = opeDatosFronterasRelacionados.data;
    this.cdRef.detectChanges();
    this.actualizarPaginacionYOrdenacion();
  }

  actualizarPaginacionYOrdenacion(): void {
    this.dataSourceDatosFronterasRelacionados.paginator = this.paginator;
    this.dataSourceDatosFronterasRelacionados.sort = this.sort;
    this.setDataSourceAttributes();
  }

  setDataSourceAttributes() {
    if (this.sort) {
      this.dataSourceDatosFronterasRelacionados.sort = this.sort;

      this.dataSourceDatosFronterasRelacionados.sortingDataAccessor = (item, property) => {
        switch (property) {
          case 'fechaInicio': {
            return this.getFechaConHoraIntervaloFormateada(item, 'inicioIntervaloHorario') || '';
          }
          case 'fechaFin': {
            return this.getFechaConHoraIntervaloFormateada(item, 'finIntervaloHorario') || '';
          }

          default: {
            const result = item[property as keyof OpeDatoFrontera];
            return typeof result === 'string' || typeof result === 'number' ? result : '';
          }
        }
      };

      this.dataSourceDatosFronterasRelacionados._updateChangeSubscription();
    }
  }

  async onSubmit() {
    if (this.formData.valid) {
      this.spinner.show();
      const data = this.formData.value;

      //
      data.opeFrontera = this.data.opeFrontera?.id;
      //

      if (this.data.opeDatoFrontera?.id) {
        data.id = this.data.opeDatoFrontera.id;
        await this.opeDatosFronterasService
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
            this.snackBar.open(this.opeErrorsService.obtenerMensajeError(error, 'opeDatosFronteras'), '', {
              duration: 3000,
              horizontalPosition: 'center',
              verticalPosition: 'bottom',
              panelClass: ['snackbar-rojo'],
            });
            console.error('Error', error);
          });
      } else {
        await this.opeDatosFronterasService
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
            this.snackBar.open(this.opeErrorsService.obtenerMensajeError(error, 'opeDatosFronteras'), '', {
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

  /*
  getFechaConHoraIntervaloFormateada(date: any, horaIntervalo: string) {
    //return moment(date).format('DD/MM/YY');
    const [hora, minuto] = horaIntervalo.split(':').map(Number); // Separa y convierte en números
    return moment(date).set({ hour: hora, minute: minuto, second: 0 }).format('DD/MM/YYYY HH:mm');
  }
  */

  //
  getFechaConHoraIntervaloFormateada(opeDatoFrontera: any, tipo: 'inicioIntervaloHorario' | 'finIntervaloHorario'): string {
    const fecha = moment(opeDatoFrontera.fecha);
    let hora: string;
    if (opeDatoFrontera.intervaloHorarioPersonalizado) {
      // Si el intervalo es personalizado, usa la hora y minuto del campo correspondiente ('inicio' o 'fin')
      const intervaloPersonalizado =
        tipo === 'inicioIntervaloHorario' ? opeDatoFrontera.inicioIntervaloHorarioPersonalizado : opeDatoFrontera.finIntervaloHorarioPersonalizado;
      hora = moment(intervaloPersonalizado, 'HH:mm').format('HH:mm');
    } else {
      // Si no es personalizado, usa la hora que viene del intervalo predeterminado ('inicio' o 'fin')
      const intervalo =
        tipo === 'inicioIntervaloHorario'
          ? opeDatoFrontera.opeDatoFronteraIntervaloHorario?.inicio
          : opeDatoFrontera.opeDatoFronteraIntervaloHorario?.fin;
      hora = moment(intervalo, 'HH:mm').format('HH:mm');
    }

    // Formatea la fecha con la hora
    return fecha.format('DD/MM/YYYY') + ' ' + hora;
  }

  //

  getHoraIntervaloHorarioFormateada(date: string): string {
    if (!date) {
      return '';
    }
    return moment(date, 'HH:mm:ss.SSSSSSS').format('HH:mm');
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
    const { fecha } = this.formData.value;
    const opeDatosFronterasRelacionados = await this.opeDatosFronterasService.get({
      fechaInicio: moment(fecha).format('YYYY-MM-DD'),
      fechaFin: moment(fecha).format('YYYY-MM-DD'),
      //idOpeFrontera: this.data.opeFrontera.id,
      idsOpeFronteras: [this.data.opeFrontera.id],
    });
    //this.opeDatosFronterasRelacionados.set(opeDatosFronterasRelacionados.data);
    this.dataSourceDatosFronterasRelacionados.data = opeDatosFronterasRelacionados.data;
    this.cdRef.detectChanges();
    this.actualizarPaginacionYOrdenacion();
  }
  //

  /*
  async onFechaHoraChange(event: Event): Promise<void> {
    const input = event.target as HTMLInputElement;
    const selectedDateTime = input.value;

    const { between, fecha, opeDatoFronteraIntervaloHorario } = this.formData.value;
    const opeDatosFronterasRelacionados = await this.opeDatosFronterasService.get({
      IdComparativoFecha: between,
      fecha: moment(fecha).format('YYYY-MM-DD'),
      opeDatoFronteraIntervaloHorario: opeDatoFronteraIntervaloHorario,
    });
    this.opeDatosFronterasRelacionados.set(opeDatosFronterasRelacionados.data);
  }
  */

  editarDatoFronteraRelacionado(opeDatoFrontera: OpeDatoFrontera) {
    if (opeDatoFrontera.id) {
      // Actualizamos `this.data` solo si es necesario
      this.data = this.data || {}; // Si `this.data` está vacío, lo inicializamos como un objeto vacío
      this.data.opeDatoFrontera = { ...opeDatoFrontera }; // Asignamos el objeto directamente a `opeDatoFrontera`

      // Actualizamos los valores en el formulario
      this.formData.patchValue({
        id: opeDatoFrontera.id,
        fecha: moment(opeDatoFrontera.fecha).format('YYYY-MM-DD'),
        opeDatoFronteraIntervaloHorario: this.data.opeDatoFrontera.idOpeDatoFronteraIntervaloHorario,
        intervaloHorarioPersonalizado: this.data.opeDatoFrontera.intervaloHorarioPersonalizado,
        inicioIntervaloHorarioPersonalizado: this.data.opeDatoFrontera.inicioIntervaloHorarioPersonalizado,
        finIntervaloHorarioPersonalizado: this.data.opeDatoFrontera.finIntervaloHorarioPersonalizado,
        numeroVehiculos: opeDatoFrontera.numeroVehiculos,
        afluencia: opeDatoFrontera.afluencia,
      });

      this.getForm('intervaloHorarioPersonalizado').enable();

      if (this.data.opeDatoFrontera.intervaloHorarioPersonalizado) {
        this.formData.get('inicioIntervaloHorarioPersonalizado')?.enable();
        this.formData.get('finIntervaloHorarioPersonalizado')?.enable();
      } else {
        this.formData.get('inicioIntervaloHorarioPersonalizado')?.disable();
        this.formData.get('finIntervaloHorarioPersonalizado')?.disable();
      }
    }
  }

  async deleteOpeDatoFrontera(idOpeDatoFrontera: number) {
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

          await this.opeDatosFronterasService.delete(idOpeDatoFrontera);
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
                this.routenav.navigate(['/ope/datos/fronteras']).then(() => {
                  window.location.href = '/ope/datos/fronteras';
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

  async onChangeNumeroVehiculos(event: Event): Promise<void> {
    const inputElement = event.target as HTMLInputElement;
    const numeroVehiculos = inputElement.value ? Number(inputElement.value) : null;

    let afluencia = '';

    if (numeroVehiculos !== null) {
      if (numeroVehiculos < this.data.opeFrontera?.transitoMedioVehiculos) {
        afluencia = 'Baja';
      } else if (numeroVehiculos >= this.data.opeFrontera?.transitoMedioVehiculos && numeroVehiculos < this.data.opeFrontera?.transitoAltoVehiculos) {
        afluencia = 'Media';
      } else if (numeroVehiculos >= this.data.opeFrontera?.transitoAltoVehiculos) {
        afluencia = 'Alta';
      }
    }

    this.formData.patchValue({
      afluencia: afluencia,
    });
  }

  //
  /*
  onChangeIntervaloHorarioPersonalizado() {
    const personalizado = this.formData.get('intervaloHorarioPersonalizado')?.value;

    if (personalizado) {
      this.formData.get('inicioIntervaloHorarioPersonalizado')?.enable();
      this.formData.get('finIntervaloHorarioPersonalizado')?.enable();
    } else {
      this.formData.get('inicioIntervaloHorarioPersonalizado')?.setValue('');
      this.formData.get('finIntervaloHorarioPersonalizado')?.setValue('');
      this.formData.get('inicioIntervaloHorarioPersonalizado')?.disable();
      this.formData.get('finIntervaloHorarioPersonalizado')?.disable();
    }
  }
   */

  onChangeIntervaloHorarioPersonalizado() {
    const intervaloHorarioPersonalizado = this.formData.get('intervaloHorarioPersonalizado')?.value;

    const inicioIntervaloHorarioPersonalizado = this.formData.get('inicioIntervaloHorarioPersonalizado');
    const finIntervaloHorarioPersonalizado = this.formData.get('finIntervaloHorarioPersonalizado');

    if (intervaloHorarioPersonalizado) {
      inicioIntervaloHorarioPersonalizado?.enable();
      finIntervaloHorarioPersonalizado?.enable();

      inicioIntervaloHorarioPersonalizado?.setValidators([Validators.required, Validators.pattern(/^([0-1]\d|2[0-3]):([0-5]\d)$/)]);
      finIntervaloHorarioPersonalizado?.setValidators([Validators.required, Validators.pattern(/^([0-1]\d|2[0-3]):([0-5]\d)$/)]);
    } else {
      inicioIntervaloHorarioPersonalizado?.setValue('');
      finIntervaloHorarioPersonalizado?.setValue('');

      inicioIntervaloHorarioPersonalizado?.disable();
      finIntervaloHorarioPersonalizado?.disable();

      inicioIntervaloHorarioPersonalizado?.clearValidators();
      finIntervaloHorarioPersonalizado?.clearValidators();
    }

    inicioIntervaloHorarioPersonalizado?.updateValueAndValidity();
    finIntervaloHorarioPersonalizado?.updateValueAndValidity();
  }
  //

  onChangeIntervaloHorario(event: MatSelectChange) {
    const selectedOption = event.source.selected as any;
    const textoSeleccionado = selectedOption?.viewValue || null;

    if (!textoSeleccionado) {
      this.getForm('intervaloHorarioPersonalizado')?.disable();
    } else {
      this.getForm('intervaloHorarioPersonalizado')?.enable();
    }
  }

  getFechaFormateadaConHorasMinutosYSegundos = (fecha: any) => {
    if (!fecha) {
      return '';
    }
    return moment(fecha).format('DD/MM/YYYY HH:mm:ss');
  };
}
