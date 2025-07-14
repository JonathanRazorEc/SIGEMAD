import { CommonModule } from '@angular/common';
import { ChangeDetectorRef, Component, computed, Inject, inject, OnInit, Renderer2, signal, SimpleChanges } from '@angular/core';
import { FormControl, FormGroup, FormGroupDirective, FormsModule, ReactiveFormsModule, Validators } from '@angular/forms';
import { FlexLayoutModule } from '@angular/flex-layout';
import { MatAutocompleteModule } from '@angular/material/autocomplete';
import { MatButtonModule } from '@angular/material/button';
import { DateAdapter, MAT_DATE_FORMATS, MatNativeDateModule, NativeDateAdapter } from '@angular/material/core';
import { MatDatepickerInputEvent, MatDatepickerModule } from '@angular/material/datepicker';
import { MAT_DIALOG_DATA, MatDialog, MatDialogModule, MatDialogRef } from '@angular/material/dialog';
import { MatExpansionModule } from '@angular/material/expansion';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatIcon, MatIconModule, MatIconRegistry } from '@angular/material/icon';
import { MatInputModule } from '@angular/material/input';
import { MatSelectModule } from '@angular/material/select';
import { Router } from '@angular/router';
import { NgxSpinnerModule, NgxSpinnerService } from 'ngx-spinner';
import { MatSnackBar } from '@angular/material/snack-bar';
import { DragDropModule } from '@angular/cdk/drag-drop';
import { AlertService } from '@shared/alert/alert.service';
import { OpeDatosAsistenciasService } from '@services/ope/datos/ope-datos-asistencias.service';
import moment from 'moment';
import { FechaValidator } from '@shared/validators/fecha-validator';
import { FORMATO_FECHA } from '../../../../../../types/date-formats';
import { UtilsService } from '@shared/services/utils.service';
import { MatTableDataSource, MatTableModule } from '@angular/material/table';
import { OpePuerto } from '@type/ope/administracion/ope-puerto.type';
import { OpeDatoAsistencia } from '@type/ope/datos/ope-dato-asistencia.type';
import { MatToolbarModule } from '@angular/material/toolbar';
import { COUNTRIES_ID, FECHA_MAXIMA_DATEPICKER, FECHA_MINIMA_DATEPICKER } from '@type/constants';
import { OpePuertosService } from '@services/ope/administracion/ope-puertos.service';
import { MomentDateAdapter } from '@angular/material-moment-adapter';
import { MatChipListboxChange, MatChipsModule } from '@angular/material/chips';
import { OpeDatoAsistenciasSanitariasComponent } from '../../sanitarias/ope-dato-asistencias-sanitarias.component';
import { OpeDatoAsistenciasTraduccionesComponent } from '../../traducciones/ope-dato-asistencias-traducciones.component';
import { OpeDatoAsistenciaTraduccion } from '@type/ope/datos/ope-dato-asistencia-traduccion.type';
import { OpeDatoAsistenciaSanitaria } from '@type/ope/datos/ope-dato-asistencia-sanitaria.type';
import { OpeDatoAsistenciaSocial } from '@type/ope/datos/ope-dato-asistencia-social.type';
import { OpeDatoAsistenciasSocialesComponent } from '../../sociales/ope-dato-asistencias-sociales.component';
import { OpeValidator } from '@shared/validators/ope/ope-validator';
import { OpeErrorsService } from '@shared/services/ope/ope-errors.service';
import { OpeDatoAsistenciasTodasComponent } from '../../todas/ope-dato-asistencias-todas.component';
import { DomSanitizer } from '@angular/platform-browser';
import { OpeDatosAsistenciasValidator } from '@shared/validators/ope/datos/ope-datos-asistencias-validator';

@Component({
  selector: 'ope-dato-asistencia-create-edit',
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
    MatChipsModule,
    OpeDatoAsistenciasSanitariasComponent,
    OpeDatoAsistenciasSocialesComponent,
    OpeDatoAsistenciasTraduccionesComponent,
  ],
  providers: [
    { provide: DateAdapter, useClass: MomentDateAdapter },
    { provide: MAT_DATE_FORMATS, useValue: FORMATO_FECHA },
  ],
  templateUrl: './ope-dato-asistencia-create-edit-form.component.html',
  styleUrl: './ope-dato-asistencia-create-edit-form.component.scss',
})
export class OpeDatoAsistenciaCreateEdit implements OnInit {
  constructor(
    //private filtrosOpeDatosAsistenciasService: LocalFiltrosOpeDatosAsistencias,
    private opePuertosService: OpePuertosService,
    private opeDatosAsistenciasService: OpeDatosAsistenciasService,
    public dialogRef: MatDialogRef<OpeDatoAsistenciaCreateEdit>,
    private matDialog: MatDialog,
    public alertService: AlertService,
    private matIconRegistry: MatIconRegistry,
    private domSanitizer: DomSanitizer,
    //@Inject(MAT_DIALOG_DATA) public data: { opeDatoAsistencia: any }
    @Inject(MAT_DIALOG_DATA) public data: { opePuerto: OpePuerto; opeDatoAsistencia: OpeDatoAsistencia }
  ) {
    this.matIconRegistry.addSvgIcon(
      'icono-asistencias-sanitarias',
      this.domSanitizer.bypassSecurityTrustResourceUrl('/assets/assets/img/ope/datos/asistencias-sanitarias.svg')
    );

    this.matIconRegistry.addSvgIcon(
      'icono-asistencias-sociales',
      this.domSanitizer.bypassSecurityTrustResourceUrl('/assets/assets/img/ope/datos/asistencias-sociales.svg')
    );

    this.matIconRegistry.addSvgIcon(
      'icono-asistencias-traducciones',
      this.domSanitizer.bypassSecurityTrustResourceUrl('/assets/assets/img/ope/datos/asistencias-traducciones.svg')
    );
  }

  public opePuertos = signal<OpePuerto[]>([]);
  public formData!: FormGroup;

  public dataSource = new MatTableDataSource<any>([]);

  private spinner = inject(NgxSpinnerService);
  public renderer = inject(Renderer2);

  public snackBar = inject(MatSnackBar);
  public utilsService = inject(UtilsService);
  public opeErrorsService = inject(OpeErrorsService);
  public routenav = inject(Router);

  public fechaMinimaDatePicker = FECHA_MINIMA_DATEPICKER;
  public fechaMaximaDatePicker = FECHA_MAXIMA_DATEPICKER;

  sections = [
    { id: 1, label: 'Sanitarias', icon: 'icono-asistencias-sanitarias' },
    { id: 2, label: 'Sociales', icon: 'icono-asistencias-sociales' },
    { id: 3, label: 'Traducciones', icon: 'icono-asistencias-traducciones' },
    //{ id: 4, label: 'Todas' },
  ];

  selectedOption: MatChipListboxChange = { source: null as any, value: 0 };

  opeDatosAsistenciasSanitarias: OpeDatoAsistenciaSanitaria[] = [];
  opeDatosAsistenciasSociales: OpeDatoAsistenciaSocial[] = [];
  opeDatosAsistenciasTraducciones: OpeDatoAsistenciaTraduccion[] = [];

  async ngOnInit() {
    this.formData = new FormGroup(
      {
        opePuerto: new FormControl('', [Validators.required, OpeValidator.opcionValidaDeSelectPorId(() => this.opePuertosFiltrados())]),
        fecha: new FormControl(new Date(new Date().setDate(new Date().getDate() - 1)), [Validators.required, FechaValidator.validarFecha]),
        opeDatosAsistenciasSanitarias: new FormControl([]),
        opeDatosAsistenciasSanitariasModificado: new FormControl(false),
        opeDatosAsistenciasSociales: new FormControl([]),
        opeDatosAsistenciasSocialesModificado: new FormControl(false),
        opeDatosAsistenciasTraducciones: new FormControl([]),
        opeDatosAsistenciasTraduccionesModificado: new FormControl(false),
      },
      {
        validators: [
          OpeDatosAsistenciasValidator.validarFechaNumeroConsistente(
            {
              opeDatosAsistenciasSanitarias: [],
              opeDatosAsistenciasSociales: [
                'opeDatosAsistenciasSocialesTareas',
                'opeDatosAsistenciasSocialesOrganismos',
                'opeDatosAsistenciasSocialesUsuarios',
              ],
              opeDatosAsistenciasTraducciones: [],
            },
            'fecha'
          ),
        ],
      }
    );

    //const opePuertos = await this.opePuertosService.get();
    // Solo puertos de España
    const opePuertos = await this.opePuertosService.get({
      idPais: COUNTRIES_ID.SPAIN,
    });
    //
    this.opePuertos.set(opePuertos.data);

    if (this.data.opeDatoAsistencia?.id) {
      this.formData.patchValue({
        id: this.data.opeDatoAsistencia.id,
        opePuerto: this.data.opeDatoAsistencia.idOpePuerto,
        fecha: moment(this.data.opeDatoAsistencia.fecha).format('YYYY-MM-DD'),
        opeDatosAsistenciasSanitarias: this.data.opeDatoAsistencia.opeDatosAsistenciasSanitarias,
        opeDatosAsistenciasSociales: this.data.opeDatoAsistencia.opeDatosAsistenciasSociales,
        opeDatosAsistenciasTraducciones: this.data.opeDatoAsistencia.opeDatosAsistenciasTraducciones,
      });

      // Actualizar nº de asistencias en las pestañas
      this.actualizarNumerosPestanias();
      // FIN Actualizar nº de asistencias en las pestañas
    }

    //const opePuertos = await this.opePuertosService.get();
    //this.opePuertos.set(opePuertos.data);

    // Para el autocompletar de los puertos
    this.getForm('opePuerto')?.valueChanges.subscribe((value: string) => {
      if (typeof value === 'string') {
        this.filtroOpePuerto.set(value);
      }
    });
    // FIN autocompletar
  }

  async onSubmit() {
    if (this.formData.valid) {
      this.spinner.show();
      const data = this.formData.value;

      if (this.data.opeDatoAsistencia?.id) {
        data.id = this.data.opeDatoAsistencia.id;
        await this.opeDatosAsistenciasService
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
            this.closeModal({ refresh: true, esGuardado: true });
            this.spinner.hide();
            //});
            // FIN PCD
          })
          .catch((error) => {
            this.spinner.hide();
            this.snackBar.open(this.opeErrorsService.obtenerMensajeError(error, 'opeDatosAsistencias'), '', {
              duration: 3000,
              horizontalPosition: 'center',
              verticalPosition: 'bottom',
              panelClass: ['snackbar-rojo'],
            });
            console.error('Error', error);
          });
      } else {
        await this.opeDatosAsistenciasService
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
            this.closeModal({ refresh: true, esGuardado: true });
            this.spinner.hide();
            //});
          })
          .catch((error) => {
            this.spinner.hide();
            this.snackBar.open(this.opeErrorsService.obtenerMensajeError(error, 'opeDatosAsistencias'), '', {
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

  /////
  closeModal(params?: any) {
    if (params?.esGuardado) {
      this.dialogRef.close(params);
      return;
    }

    const opeDatosAsistenciasSanitariasModificado = this.formData.get('opeDatosAsistenciasSanitariasModificado')?.value;
    const opeDatosAsistenciasSocialesModificado = this.formData.get('opeDatosAsistenciasSocialesModificado')?.value;
    const opeDatosAsistenciasTraduccionesModificado = this.formData.get('opeDatosAsistenciasTraduccionesModificado')?.value;

    if (
      opeDatosAsistenciasSanitariasModificado ||
      opeDatosAsistenciasSocialesModificado ||
      opeDatosAsistenciasTraduccionesModificado ||
      this.formData.dirty
    ) {
      this.alertService

        .showAlert({
          title: 'Tienes cambios sin guardar.<br>¿Estás seguro de que quieres salir?',
          showCancelButton: true,
          cancelButtonColor: '#d33',
          confirmButtonText: '¡Sí, salir!',
          cancelButtonText: 'Cancelar',
          customClass: {
            title: 'sweetAlert-fsize20',
          },
        })
        .then(async (result) => {
          if (result.isConfirmed) {
            this.dialogRef.close(params);
          }
        });
    } else {
      this.dialogRef.close(params);
    }
  }

  ////

  getForm(atributo: string): any {
    return this.formData.controls[atributo];
  }

  getFechaFormateada(date: any) {
    return moment(date).format('DD/MM/YY');
  }

  async deleteOpeDatoAsistencia(idOpeDatoAsistencia: number) {
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

          await this.opeDatosAsistenciasService.delete(idOpeDatoAsistencia);
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
                this.routenav.navigate(['/ope/datos/asistencias']).then(() => {
                  window.location.href = '/ope/datos/asistencias';
                });
                this.spinner.hide();
              });
            // FIN PCD
          }, 2000);
        } else {
          this.spinner.hide();
        }
      });
  }

  onSelectionChange(event: MatChipListboxChange): void {
    this.selectedOption = event;
  }

  trackByFn(index: number, item: any): number {
    return item.id;
  }

  actualizarOpeDatosAsistenciasSanitarias(lista: OpeDatoAsistenciaSanitaria[]) {
    this.formData.patchValue({
      opeDatosAsistenciasSanitarias: lista,
      opeDatosAsistenciasSanitariasModificado: true,
    });

    if (!this.data.opeDatoAsistencia) {
      this.data.opeDatoAsistencia = {} as OpeDatoAsistencia;
    }
    this.data.opeDatoAsistencia.opeDatosAsistenciasSanitarias = lista;

    // Actualizar nº de asistencias en las pestañas
    this.actualizarNumerosPestanias();
    // FIN Actualizar nº de asistencias en las pestañas
  }

  actualizarOpeDatosAsistenciasSociales(lista: OpeDatoAsistenciaSocial[]) {
    this.formData.patchValue({
      opeDatosAsistenciasSociales: lista,
      opeDatosAsistenciasSocialesModificado: true,
    });

    if (!this.data.opeDatoAsistencia) {
      this.data.opeDatoAsistencia = {} as OpeDatoAsistencia;
    }
    this.data.opeDatoAsistencia.opeDatosAsistenciasSociales = lista;

    // Actualizar nº de asistencias en las pestañas
    this.actualizarNumerosPestanias();
    // FIN Actualizar nº de asistencias en las pestañas
  }

  actualizarOpeDatosAsistenciasTraducciones(lista: OpeDatoAsistenciaTraduccion[]) {
    this.formData.patchValue({
      opeDatosAsistenciasTraducciones: lista,
      opeDatosAsistenciasTraduccionesModificado: true,
    });

    if (!this.data.opeDatoAsistencia) {
      this.data.opeDatoAsistencia = {} as OpeDatoAsistencia;
    }

    this.data.opeDatoAsistencia.opeDatosAsistenciasTraducciones = lista;

    // Actualizar nº de asistencias en las pestañas
    this.actualizarNumerosPestanias();
    // FIN Actualizar nº de asistencias en las pestañas
  }

  // Para el autocompletar de los puertos
  filtroOpePuerto = signal('');

  // Para input con objeto como valor
  displayFn = (id: number | null): string => {
    if (id == null) return '';
    const match = this.opePuertos().find((item) => item.id === id);
    return match ? match.nombre : '';
  };

  // Computado para filtrar lista corta
  opePuertosFiltrados = computed(() => {
    return this.utilsService.filtrarPorTexto(this.opePuertos(), this.filtroOpePuerto(), false);
  });
  // Fin autocompletar

  actualizarNumerosPestanias() {
    this.sections = [
      {
        id: 1,
        label: `Sanitarias${
          this.getNumeroTotalDeListado(this.data.opeDatoAsistencia.opeDatosAsistenciasSanitarias) > 0
            ? ' (' + this.getNumeroTotalDeListado(this.data.opeDatoAsistencia.opeDatosAsistenciasSanitarias) + ')'
            : ''
        }`,
        icon: 'icono-asistencias-sanitarias',
      },
      {
        id: 2,
        label: `Sociales${
          this.getNumeroTotalDeListado(this.data.opeDatoAsistencia.opeDatosAsistenciasSociales) > 0
            ? ' (' + this.getNumeroTotalDeListado(this.data.opeDatoAsistencia.opeDatosAsistenciasSociales) + ')'
            : ''
        }`,
        icon: 'icono-asistencias-sociales',
      },
      {
        id: 3,
        label: `Traducciones${
          this.getNumeroTotalDeListado(this.data.opeDatoAsistencia.opeDatosAsistenciasTraducciones) > 0
            ? ' (' + this.getNumeroTotalDeListado(this.data.opeDatoAsistencia.opeDatosAsistenciasTraducciones) + ')'
            : ''
        }`,
        icon: 'icono-asistencias-traducciones',
      },
    ];
  }

  getNumeroTotalDeListado(array: any[] | undefined): number {
    if (!array) return 0;
    return array.reduce((acc, item) => acc + (item.numero || 0), 0);
  }

  getFechaFormateadaConHorasMinutosYSegundos = (fecha: any) => {
    if (!fecha) {
      return '';
    }
    return moment(fecha).format('DD/MM/YYYY HH:mm:ss');
  };
}
