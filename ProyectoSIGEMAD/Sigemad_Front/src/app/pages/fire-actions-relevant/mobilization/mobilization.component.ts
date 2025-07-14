import { Component, EventEmitter, inject, Input, Output, signal, ViewChild } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FlexLayoutModule } from '@angular/flex-layout';
import {
  FormBuilder,
  FormGroup,
  FormGroupDirective,
  ReactiveFormsModule,
  Validators,
} from '@angular/forms';
import { MatButtonModule } from '@angular/material/button';
import {
  DateAdapter,
  MAT_DATE_FORMATS,
  MatNativeDateModule,
  NativeDateAdapter,
} from '@angular/material/core';
import { MatDatepickerModule } from '@angular/material/datepicker';
import { MAT_DIALOG_DATA, MatDialog } from '@angular/material/dialog';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatGridListModule } from '@angular/material/grid-list';
import { MatIconModule } from '@angular/material/icon';
import { MatInputModule } from '@angular/material/input';
import { MatSelectModule } from '@angular/material/select';
import { MatSnackBar } from '@angular/material/snack-bar';
import { MatSort } from '@angular/material/sort';
import { MatTableDataSource, MatTableModule } from '@angular/material/table';
import moment from 'moment';
import { NgxSpinnerModule, NgxSpinnerService } from 'ngx-spinner';
import { ActionsRelevantService } from '../../../services/actions-relevant.service';
import { SavePayloadModal } from '../../../types/save-payload-modal';
import { GenericMaster } from '../../../types/actions-relevant.type';
import { _isNumberValue } from '@angular/cdk/coercion';
import { Step1Component } from './step1/step1.component';
import { Step2Component } from './step2/step2.component';
import { Step3Component } from './step3/step3.component';
import { Step4Component } from './step4/step4.component';
import { Step5Component } from './step5/step5.component';
import { Step6Component } from './step6/step6.component';
import { Step7Component } from './step7/step7.component';
import { Step8Component } from './step8/step8.component';
import {
  ActuacionRelevante,
  Movilizacion,
  PasoAportacion,
  PasoDespliegue,
  PasoIntervencion,
  PasoLlegadaBase,
  PasoOfrecimiento,
  PasoSolicitud,
  PasoTramitacion,
} from '../../../types/mobilization.type';
import { TooltipDirective } from '../../../shared/directive/tooltip/tooltip.directive';

const FORMATO_FECHA = {
  parse: {
    dateInput: 'LL',
  },
  display: {
    dateInput: 'LL',
    monthYearLabel: 'MMM YYYY',
    dateA11yLabel: 'LL',
    monthYearA11yLabel: 'MMMM YYYY',
  },
};

@Component({
  selector: 'app-mobilization',
  standalone: true,
  imports: [
    ReactiveFormsModule,
    MatFormFieldModule,
    MatDatepickerModule,
    MatNativeDateModule,
    CommonModule,
    MatInputModule,
    FlexLayoutModule,
    MatGridListModule,
    MatButtonModule,
    MatSelectModule,
    MatTableModule,
    MatIconModule,
    NgxSpinnerModule,
    Step1Component,
    Step2Component,
    Step3Component,
    Step4Component,
    Step5Component,
    Step6Component,
    Step7Component,
    Step8Component,
    TooltipDirective,
  ],
  providers: [
    { provide: DateAdapter, useClass: NativeDateAdapter },
    { provide: MAT_DATE_FORMATS, useValue: FORMATO_FECHA },
  ],
  templateUrl: './mobilization.component.html',
  styleUrl: './mobilization.component.scss',
})
export class MobilizationComponent {
  @ViewChild(MatSort) sort!: MatSort;
  data = inject(MAT_DIALOG_DATA) as { title: string; idIncendio: number };
  @Output() save = new EventEmitter<SavePayloadModal>();
  @Input() editData: any;
  @Input() esUltimo: boolean | undefined;
  @Input() fire: any;
  @Input() dataMaestros: any;

  public movilizacionService = inject(ActionsRelevantService);
  public toast = inject(MatSnackBar);
  private fb = inject(FormBuilder);
  public matDialog = inject(MatDialog);
  private spinner = inject(NgxSpinnerService);
  public pasoActual = signal<number>(1); // Inicialmente en Paso 1

  displayedColumns: string[] = ['solicitante', 'situacion', 'ultimaActualizacion', 'opciones'];
  // Definir datos estáticos
  dataSource = new MatTableDataSource([
    {
      solicitante: 'Delegación del gobierno',
      situacion: 'Emergencia activada',
      ultimaActualizacion: '20/08/2024',
    },
  ]);
  formData!: FormGroup;
  editTipo = false;

  public isCreate = signal<number>(-1);
  public tiposGestion = signal<GenericMaster[]>([]);
  public tipoAdmin = signal<GenericMaster[]>([]);
  public pasoSolicitud!: PasoSolicitud;
  public pasoTramitacion!: PasoTramitacion;
  public pasoOfrecimiento!: PasoOfrecimiento;
  public pasoAportacion!: PasoAportacion;
  public pasoDespliegue!: PasoDespliegue;
  public pasoIntervencion!: PasoIntervencion;
  public pasoLlegada!: PasoLlegadaBase;
  public movilizacionSeleccionada?: Movilizacion;
  public btnGuardar = 'Nueva solicitud';
  public editar: boolean = false;

  async ngOnInit() {
    this.tiposGestion.set(this.dataMaestros.tiposGestion);

    this.formData = this.fb.group({
      idTipoNotificacion: [null, Validators.required],
      idTipoNotificacionEdit: [null],
      paso1: this.fb.group({
        IdProcedenciaMedio: [null, Validators.required],
        AutoridadSolicitante: ['', Validators.required],
        FechaHoraSolicitud: [new Date(), Validators.required],
        Descripcion: [''],
        Observaciones: [''],
      }),
      paso2: this.fb.group({
        IdDestinoMedio: [null, Validators.required],
        TitularMedio: [''],
        FechaHoraTramitacion: [new Date(), Validators.required],
        PublicadoCECIS: [false],
        Descripcion2: [''],
        Observaciones2: [''],
      }),
      paso3: this.fb.group({
        TitularMedio3: ['', Validators.required],
        GestionCECOD: [false],
        FechaHoraOfrecimiento: [new Date(), Validators.required],
        Descripcion3: ['', Validators.required],
        FechaHoraDisponibilidad: [null],
        Observaciones3: [''],
      }),
      paso5: this.fb.group({
        IdCapacidad: [null, Validators.required],
        MedioNoCatalogado: [{ value: '', disabled: true }],
        IdTipoAdministracion: [null, Validators.required],
        TitularMedio5: [''],
        FechaHoraAportacion: [new Date(), Validators.required],
        Descripcion5: [''],
      }),
      paso6: this.fb.group({
        IdCapacidad: [null, Validators.required],
        MedioNoCatalogado: [''],
        FechaHoraDespliegue: [new Date(), Validators.required],
        FechaHoraInicioIntervencion: [null],
        Descripcion6: [''],
        Observaciones6: [''],
      }),
      paso7: this.fb.group({
        IdCapacidad: [null, Validators.required],
        MedioNoCatalogado: [''],
        FechaHoraInicioIntervencion: [new Date(), Validators.required],
        Observaciones7: [''],
      }),
      paso8: this.fb.group({
        IdCapacidad: [null, Validators.required],
        MedioNoCatalogado: [''],
        FechaHoraLlegada: [new Date(), Validators.required],
        Observaciones8: [''],
      }),
    });

    if (this.editData) {
      if (this.movilizacionService.dataMovilizacion().length === 0) {
        const inputMobilizaciones = this.editData.movilizacionMedios;
        const mappedMobilizaciones = {
          IdActuacionRelevante: this.editData.id,
          IdSuceso: this.editData.idSuceso,
          Movilizaciones: inputMobilizaciones.map((movilizacion: any) => ({
            Id: movilizacion.id,
            Solicitante: movilizacion.solicitante,
            Pasos: movilizacion.pasos.reduce((acc: any[], p: any) => {
              // Mapeo para Paso 1: Solicitud
              if (p.pasoMovilizacion && p.pasoMovilizacion.id === 1 && p.solicitudMedio) {
                acc.push({
                  Id: p.id,
                  TipoPaso: 1,
                  IdProcedenciaMedio: p.solicitudMedio.procedenciaMedio?.id || 0,
                  AutoridadSolicitante: p.solicitudMedio.autoridadSolicitante || '',
                  FechaHoraSolicitud: new Date(p.solicitudMedio.fechaHoraSolicitud).toISOString(),
                  Descripcion: p.solicitudMedio.descripcion || '',
                  Observaciones: p.solicitudMedio.observaciones || '',
                });
              }
              // Mapeo para Paso 2: Tramitación
              else if (p.pasoMovilizacion && p.pasoMovilizacion.id === 2 && p.tramitacionMedio) {
                acc.push({
                  Id: p.id,
                  TipoPaso: 2,
                  IdDestinoMedio: p.tramitacionMedio.destinoMedio?.id || 0,
                  TitularMedio: p.tramitacionMedio.titularMedio || '',
                  FechaHoraTramitacion: new Date(
                    p.tramitacionMedio.fechaHoraTramitacion
                  ).toISOString(),
                  PublicadoCECIS: p.tramitacionMedio.publicadoCECIS ?? false,
                  Descripcion: p.tramitacionMedio.descripcion || '',
                  Observaciones: p.tramitacionMedio.observaciones || '',
                });
              }
              // Mapeo para Paso 3: Ofrecimiento
              else if (p.pasoMovilizacion && p.pasoMovilizacion.id === 3 && p.ofrecimientoMedio) {
                acc.push({
                  Id: p.id,
                  TipoPaso: 3,
                  TitularMedio: p.ofrecimientoMedio.titularMedio || '',
                  FechaHoraOfrecimiento: new Date(
                    p.ofrecimientoMedio.fechaHoraOfrecimiento
                  ).toISOString(),
                  FechaHoraDisponibilidad: p.ofrecimientoMedio.fechaHoraDisponibilidad
                    ? new Date(p.ofrecimientoMedio.fechaHoraDisponibilidad).toISOString()
                    : null,
                  GestionCECOD: p.ofrecimientoMedio.gestionCECOD ?? false,
                  Descripcion: p.ofrecimientoMedio.descripcion || '',
                  Observaciones: p.ofrecimientoMedio.observaciones || '',
                });
              }
              // Mapeo para Paso 5: Aportación
              else if (p.pasoMovilizacion && p.pasoMovilizacion.id === 5 && p.aportacionMedio) {
                acc.push({
                  Id: p.id,
                  TipoPaso: 5,
                  IdCapacidad: p.aportacionMedio.capacidad?.id || 0,
                  MedioNoCatalogado: p.aportacionMedio.medioNoCatalogado || '',
                  IdTipoAdministracion: p.aportacionMedio.idTipoAdministracion || 0,
                  TitularMedio: p.aportacionMedio.titularMedio || '',
                  FechaHoraAportacion: new Date(
                    p.aportacionMedio.fechaHoraAportacion
                  ).toISOString(),
                  Descripcion: p.aportacionMedio.descripcion || '',
                  Observaciones: p.aportacionMedio.observaciones || '',
                });
              }
              // Paso 6: Despliegue
              else if (p.pasoMovilizacion && p.pasoMovilizacion.id === 6 && p.despliegueMedio) {
                acc.push({
                  Id: p.id,
                  TipoPaso: 6,
                  IdCapacidad: p.despliegueMedio.capacidad?.id || 0,
                  MedioNoCatalogado: p.despliegueMedio.medioNoCatalogado || '',
                  FechaHoraDespliegue: new Date(
                    p.despliegueMedio.fechaHoraDespliegue
                  ).toISOString(),
                  FechaHoraInicioIntervencion: new Date(
                    p.despliegueMedio.fechaHoraInicioIntervencion
                  ).toISOString(),
                  Observaciones: p.despliegueMedio.observaciones || '',
                });
              }
              // Paso 7: Intervención (Fin de intervención)
              else if (
                p.pasoMovilizacion &&
                p.pasoMovilizacion.id === 7 &&
                p.finIntervencionMedio
              ) {
                acc.push({
                  Id: p.id,
                  TipoPaso: 7,
                  IdCapacidad: p.finIntervencionMedio.capacidad?.id || 0,
                  MedioNoCatalogado: p.finIntervencionMedio.medioNoCatalogado || '',
                  FechaHoraInicioIntervencion: new Date(
                    p.finIntervencionMedio.fechaHoraInicioIntervencion
                  ).toISOString(),
                  Observaciones: p.finIntervencionMedio.observaciones || '',
                });
              }
              // Paso 8: Llegada a Base
              else if (p.pasoMovilizacion && p.pasoMovilizacion.id === 8 && p.llegadaBaseMedio) {
                acc.push({
                  Id: p.id,
                  TipoPaso: 8,
                  IdCapacidad: p.llegadaBaseMedio.capacidad?.id || 0,
                  MedioNoCatalogado: p.llegadaBaseMedio.medioNoCatalogado || '',
                  FechaHoraLlegada: new Date(p.llegadaBaseMedio.fechaHoraLlegada).toISOString(),
                  Observaciones: p.llegadaBaseMedio.observaciones || '',
                });
              }
              return acc;
            }, []),
          })),
        };

        this.movilizacionService.dataMovilizacion.set([mappedMobilizaciones]);
        console.log(
          '🚀 ~ MobilizationComponent ~ ngOnInit ~ this.movilizacionService.dataMovilizacion:',
          this.movilizacionService.dataMovilizacion()
        );
      }
    }
    this.spinner.hide();
  }

  async onSubmit(formDirective: FormGroupDirective): Promise<void> {
    this.btnGuardar = 'Nueva solicitud';
    this.editTipo = false;
    console.log(
      '🚀 ~ MobilizationComponent ~ getOrCreateActuacion ~ this.movilizacionService.dataMovilizacion():',
      this.movilizacionService.dataMovilizacion()
    );
    const pasoActual = this.formData.get('idTipoNotificacion')?.value.id;
    if (pasoActual === undefined || pasoActual === null) {
      console.error('No se ha seleccionado un paso válido.');
      return;
    }

    const actuaciones = this.getOrCreateActuacion();
    const movilizaciones = actuaciones[0].Movilizaciones;
    console.log('🚀 ~ MobilizationComponent ~ onSubmit ~ movilizaciones:', movilizaciones);
    console.log('🚀 ~ MobilizationComponent ~ onSubmit ~ pasoActual:', pasoActual);
    switch (pasoActual) {
      case 1:
        if (!this.procesarPaso1()) return;
        if (this.editar) {
          this.btnGuardar = 'Guardar';
          if (this.movilizacionSeleccionada) {
            const index = this.movilizacionSeleccionada.Pasos.findIndex((p) => p.TipoPaso === 1);
            if (index !== -1) {
              this.movilizacionSeleccionada.Pasos[index] = this.pasoSolicitud;
            } else {
              this.movilizacionSeleccionada.Pasos.push(this.pasoSolicitud);
            }
          } else if (movilizaciones.length > 0) {
            const firstMov = movilizaciones[0];
            const index = firstMov.Pasos.findIndex((p) => p.TipoPaso === 1);
            if (index !== -1) {
              firstMov.Pasos[index] = this.pasoSolicitud;
            } else {
              firstMov.Pasos.push(this.pasoSolicitud);
            }
          } else {
            console.error('No se encontró una movilización para editar el paso.');
            return;
          }
        } else {
          // Si no estamos en modo edición, agregamos una nueva movilización con el paso 1
          this.agregarNuevaMovilizacion(movilizaciones, this.pasoSolicitud);
        }

        break;
      case 2:
        if (!this.procesarPaso2()) return;
        if (this.editar) {
          this.btnGuardar = 'Guardar';
          if (this.movilizacionSeleccionada) {
            const index = this.movilizacionSeleccionada.Pasos.findIndex((p) => p.TipoPaso === 2);
            if (index !== -1) {
              this.movilizacionSeleccionada.Pasos[index] = this.pasoTramitacion;
            } else {
              this.movilizacionSeleccionada.Pasos.push(this.pasoTramitacion);
            }
          } else if (movilizaciones.length > 0) {
            const firstMov = movilizaciones[0];
            const index = firstMov.Pasos.findIndex((p) => p.TipoPaso === 2);
            if (index !== -1) {
              firstMov.Pasos[index] = this.pasoTramitacion;
            } else {
              firstMov.Pasos.push(this.pasoTramitacion);
            }
          } else {
            console.error('No se encontró una movilización para editar el Paso 2.');
            return;
          }
        } else {
          if (!this.movilizacionSeleccionada) {
            console.error('No se ha seleccionado una movilización para agregar el Paso 2.');
            return;
          }
          this.movilizacionSeleccionada.Pasos.push(this.pasoTramitacion);
        }
        break;
      case 3:
        if (!this.procesarPaso3()) return;
        if (!this.movilizacionSeleccionada) {
          console.error('No se ha seleccionado una movilización para agregar el Paso 3.');
          return;
        }
        this.movilizacionSeleccionada.Pasos.push(this.pasoOfrecimiento);
        break;
      case 5:
        if (!this.procesarPaso5()) return;
        if (this.editar) {
          this.btnGuardar = 'Guardar';
          if (this.movilizacionSeleccionada) {
            const index = this.movilizacionSeleccionada.Pasos.findIndex((p) => p.TipoPaso === 5);
            if (index !== -1) {
              this.movilizacionSeleccionada.Pasos[index] = this.pasoAportacion;
            } else {
              this.movilizacionSeleccionada.Pasos.push(this.pasoAportacion);
            }
          } else if (movilizaciones.length > 0) {
            const firstMov = movilizaciones[0];
            const index = firstMov.Pasos.findIndex((p) => p.TipoPaso === 5);
            if (index !== -1) {
              firstMov.Pasos[index] = this.pasoAportacion;
            } else {
              firstMov.Pasos.push(this.pasoAportacion);
            }
          } else {
            console.error('No se encontró una movilización para editar el Paso 5.');
            return;
          }
        } else {
          if (!this.movilizacionSeleccionada) {
            console.error('No se ha seleccionado una movilización para agregar el Paso 5.');
            return;
          }
          this.movilizacionSeleccionada.Pasos.push(this.pasoAportacion);
        }
        break;
      case 6:
        if (!this.procesarPaso6()) return;
        if (this.editar) {
          this.btnGuardar = 'Guardar';
          if (this.movilizacionSeleccionada) {
            const index = this.movilizacionSeleccionada.Pasos.findIndex((p) => p.TipoPaso === 6);
            if (index !== -1) {
              this.movilizacionSeleccionada.Pasos[index] = this.pasoDespliegue;
            } else {
              this.movilizacionSeleccionada.Pasos.push(this.pasoDespliegue);
            }
          } else if (movilizaciones.length > 0) {
            const firstMov = movilizaciones[0];
            const index = firstMov.Pasos.findIndex((p) => p.TipoPaso === 6);
            if (index !== -1) {
              firstMov.Pasos[index] = this.pasoDespliegue;
            } else {
              firstMov.Pasos.push(this.pasoDespliegue);
            }
          } else {
            console.error('No se encontró una movilización para editar el Paso 6.');
            return;
          }
        } else {
          if (!this.movilizacionSeleccionada) {
            console.error('No se ha seleccionado una movilización para agregar el Paso 6.');
            return;
          }
          this.movilizacionSeleccionada.Pasos.push(this.pasoDespliegue);
        }
        break;
      case 7:
        if (!this.procesarPaso7()) return;
        if (this.editar) {
          this.btnGuardar = 'Guardar';
          if (this.movilizacionSeleccionada) {
            const index = this.movilizacionSeleccionada.Pasos.findIndex((p) => p.TipoPaso === 7);
            if (index !== -1) {
              this.movilizacionSeleccionada.Pasos[index] = this.pasoIntervencion;
            } else {
              this.movilizacionSeleccionada.Pasos.push(this.pasoIntervencion);
            }
          } else if (movilizaciones.length > 0) {
            const firstMov = movilizaciones[0];
            const index = firstMov.Pasos.findIndex((p) => p.TipoPaso === 7);
            if (index !== -1) {
              firstMov.Pasos[index] = this.pasoIntervencion;
            } else {
              firstMov.Pasos.push(this.pasoIntervencion);
            }
          } else {
            console.error('No se encontró una movilización para editar el Paso 7.');
            return;
          }
        } else {
          if (!this.movilizacionSeleccionada) {
            console.error('No se ha seleccionado una movilización para agregar el Paso 7.');
            return;
          }
          this.movilizacionSeleccionada.Pasos.push(this.pasoIntervencion);
        }
        break;
      case 8:
        if (!this.procesarPaso8()) return;
        if (this.editar) {
          this.btnGuardar = 'Guardar';
          if (this.movilizacionSeleccionada) {
            const index = this.movilizacionSeleccionada.Pasos.findIndex((p) => p.TipoPaso === 8);
            if (index !== -1) {
              this.movilizacionSeleccionada.Pasos[index] = this.pasoLlegada;
            } else {
              this.movilizacionSeleccionada.Pasos.push(this.pasoLlegada);
            }
          } else if (movilizaciones.length > 0) {
            const firstMov = movilizaciones[0];
            const index = firstMov.Pasos.findIndex((p) => p.TipoPaso === 8);
            if (index !== -1) {
              firstMov.Pasos[index] = this.pasoLlegada;
            } else {
              firstMov.Pasos.push(this.pasoLlegada);
            }
          } else {
            console.error('No se encontró una movilización para editar el Paso 8.');
            return;
          }
        } else {
          if (!this.movilizacionSeleccionada) {
            console.error('No se ha seleccionado una movilización para agregar el Paso 8.');
            return;
          }
          this.movilizacionSeleccionada.Pasos.push(this.pasoLlegada);
        }
        break;
      default:
        console.error('Paso actual desconocido.');
        return;
    }

    this.onReset(formDirective);
    console.log(
      '🚀 ~ MobilizationComponent ~ onSubmit ~ actuacionRelevante: ActuacionRelevante.IdActuacionRelevante:',
      this.editData
    );
    const actuacionRelevante: ActuacionRelevante = {
      IdActuacionRelevante: this.editData?.id ?? 0,
      IdSuceso: this.data.idIncendio,
      Movilizaciones: movilizaciones,
    };

    this.movilizacionService.dataMovilizacion.set([actuacionRelevante]);
    console.log('Datos actualizados:', this.movilizacionService.dataMovilizacion());
    this.editar = false;
  }

  private getOrCreateActuacion(): ActuacionRelevante[] {
    let actuaciones = this.movilizacionService.dataMovilizacion();

    console.log('🚀 ~ MobilizationComponent ~ getOrCreateActuacion ~ actuaciones:', actuaciones);
    if (!actuaciones.length) {
      actuaciones = [
        {
          IdActuacionRelevante: this.editData?.id ?? 0,
          IdSuceso: this.data.idIncendio,
          Movilizaciones: [],
        },
      ];
    }
    return actuaciones;
  }

  private agregarNuevaMovilizacion(movilizaciones: Movilizacion[], paso: PasoSolicitud): void {
    const nuevaMovilizacion: Movilizacion = {
      Id: 0,
      Solicitante: paso?.AutoridadSolicitante || 'Solicitud de movilización',
      Pasos: [paso],
    };
    movilizaciones.push(nuevaMovilizacion);
  }

  onReset(formDirective: FormGroupDirective): void {
    const defaultFormValues = {
      idTipoNotificacion: null,
      paso1: {
        IdProcedenciaMedio: null,
        AutoridadSolicitante: '',
        FechaHoraSolicitud: new Date(),
        Descripcion: '',
        Observaciones: '',
      },
      paso2: {
        IdDestinoMedio: null,
        TitularMedio: '',
        FechaHoraTramitacion: new Date(),
        PublicadoCECIS: false,
        Descripcion2: '',
        Observaciones2: '',
      },
      paso3: {
        TitularMedio3: '',
        GestionCECOD: false,
        FechaHoraOfrecimiento: new Date(),
        Descripcion3: '',
        FechaHoraDisponibilidad: null,
        Observaciones3: '',
      },
    };

    this.formData.reset(defaultFormValues);
    formDirective.resetForm(defaultFormValues);
    this.loadTipo(0);
  }

  private procesarPaso1(): boolean {
    const pasoValido =
      (this.formData.get('idTipoNotificacion')?.valid ?? false) &&
      (this.formData.get('paso1.IdProcedenciaMedio')?.valid ?? false) &&
      (this.formData.get('paso1.AutoridadSolicitante')?.valid ?? false) &&
      (this.formData.get('paso1.FechaHoraSolicitud')?.valid ?? false);

    if (!pasoValido) {
      this.formData.markAllAsTouched();
      return false;
    }

    this.pasoSolicitud = {
      Id: 0,
      TipoPaso: 1,
      IdProcedenciaMedio: this.formData.value.paso1.IdProcedenciaMedio?.id ?? 0,
      AutoridadSolicitante: this.formData.value.paso1.AutoridadSolicitante,
      FechaHoraSolicitud: new Date(this.formData.value.paso1.FechaHoraSolicitud).toISOString(),
      Descripcion: this.formData.value.paso1.Descripcion || '',
      Observaciones: this.formData.value.paso1.Observaciones || '',
    };

    return true;
  }

  private procesarPaso2(): boolean {
    const pasoValido =
      (this.formData.get('paso2.IdDestinoMedio')?.valid ?? false) &&
      (this.formData.get('paso2.FechaHoraTramitacion')?.valid ?? false);

    if (!pasoValido) {
      this.formData.markAllAsTouched();
      return false;
    }

    this.pasoTramitacion = {
      Id: 0,
      TipoPaso: 2,
      IdDestinoMedio: this.formData.value.paso2.IdDestinoMedio?.id ?? 0,
      TitularMedio: this.formData.value.paso2.TitularMedio || '',
      FechaHoraTramitacion: new Date(this.formData.value.paso2.FechaHoraTramitacion).toISOString(),
      PublicadoCECIS: this.formData.value.paso2.PublicadoCECIS ?? false,
      Descripcion: this.formData.value.paso2.Descripcion2 || '',
      Observaciones: this.formData.value.paso2.Observaciones2 || '',
    };

    return true;
  }

  private procesarPaso3(): boolean {
    const pasoValido =
      (this.formData.get('paso3.TitularMedio3')?.valid ?? false) &&
      (this.formData.get('paso3.FechaHoraOfrecimiento')?.valid ?? false);

    if (!pasoValido) {
      this.formData.markAllAsTouched();
      return false;
    }

    this.pasoOfrecimiento = {
      Id: 0,
      TipoPaso: 3,
      TitularMedio: this.formData.value.paso3.TitularMedio3 || '',
      FechaHoraOfrecimiento: new Date(
        this.formData.value.paso3.FechaHoraOfrecimiento
      ).toISOString(),
      FechaHoraDisponibilidad: this.formData.value.paso3.FechaHoraDisponibilidad
        ? new Date(this.formData.value.paso3.FechaHoraDisponibilidad).toISOString()
        : '',
      GestionCECOD: this.formData.value.paso3.GestionCECOD ?? false,
      Descripcion: this.formData.value.paso3.Descripcion3 || '',
      Observaciones: this.formData.value.paso3.Observaciones3 || '',
    };

    return true;
  }

  private procesarPaso5(): boolean {
    let pasoValido =
      (this.formData.get('paso5.IdCapacidad')?.valid ?? false) &&
      (this.formData.get('paso5.FechaHoraAportacion')?.valid ?? false);

    const capacidadValue = this.formData.get('paso5.IdCapacidad')?.value;
    const capacidadId = capacidadValue?.id;
    console.log(
      '🚀 ~ MobilizationComponent ~ procesarPaso5 ~ this.formData.value:',
      this.formData.value
    );
    if (capacidadId === 92) {
      pasoValido = pasoValido && (this.formData.get('paso5.MedioNoCatalogado')?.valid ?? false);
    }

    if (!pasoValido) {
      this.formData.markAllAsTouched();
      return false;
    }

    this.pasoAportacion = {
      Id: 0,
      TipoPaso: 5,
      IdCapacidad: this.formData.value.paso5.IdCapacidad?.id ?? 0,

      MedioNoCatalogado: this.formData.value.paso5.MedioNoCatalogado || '',
      IdTipoAdministracion: 1,
      TitularMedio: this.formData.value.paso5.TitularMedio5 || '',
      FechaHoraAportacion: this.formData.value.paso5.FechaHoraAportacion
        ? new Date(this.formData.value.paso5.FechaHoraAportacion).toISOString()
        : '',
      Descripcion: this.formData.value.paso5.Descripcion5 || '',
    };

    console.log(
      '🚀 ~ MobilizationComponent ~ procesarPaso5 ~ this.pasoAportacion:',
      this.pasoAportacion
    );

    return true;
  }

  private procesarPaso7(): boolean {
    const controlCapacidad = this.formData.get('paso7.IdCapacidad');
    const controlCapacidadFecha = this.formData.get('paso7.FechaHoraInicioIntervencion');

    const pasoValido =
      (controlCapacidad?.value.id ?? false) && (controlCapacidadFecha?.valid ?? false);
    console.log('🚀 ~ MobilizationComponent ~ procesarPaso7 ~ pasoValido:', pasoValido);
    if (!pasoValido) {
      this.formData.markAllAsTouched();
      return false;
    }

    this.pasoIntervencion = {
      Id: 0,
      TipoPaso: 7,
      IdCapacidad: controlCapacidad?.value.id ?? 0,
      MedioNoCatalogado: this.formData.value.paso7.MedioNoCatalogado || '',
      FechaHoraInicioIntervencion: new Date(
        this.formData.value.paso7.FechaHoraInicioIntervencion
      ).toISOString(),
      Observaciones: this.formData.value.paso7.Observaciones7 || '',
      Descripcion: '',
    };

    return true;
  }

  private procesarPaso8(): boolean {
    const controlCapacidad = this.formData.get('paso8.IdCapacidad');
    const controlCapacidadFecha = this.formData.get('paso8.FechaHoraLlegada');
    const pasoValido =
      (controlCapacidad?.value.id ?? false) && (controlCapacidadFecha?.valid ?? false);

    if (!pasoValido) {
      this.formData.markAllAsTouched();
      return false;
    }

    this.pasoLlegada = {
      Id: 0,
      TipoPaso: 8,
      IdCapacidad: controlCapacidad?.value.id ?? 0,
      MedioNoCatalogado: this.formData.value.paso8.MedioNoCatalogado || '',
      FechaHoraLlegada: new Date(this.formData.value.paso8.FechaHoraLlegada).toISOString(),
      Observaciones: this.formData.value.paso8.Observaciones8 || '',
      Descripcion: '',
    };

    return true;
  }

  private procesarPaso6(): boolean {
    const controlCapacidad = this.formData.get('paso6.IdCapacidad');
    const controlCapacidadFecha = this.formData.get('paso6.FechaHoraDespliegue');

    const pasoValido =
      (controlCapacidad?.value.id ?? false) && (controlCapacidadFecha?.valid ?? false);
    console.log('🚀 ~ MobilizationComponent ~ procesarPaso6 ~ pasoValido:', pasoValido);

    if (!pasoValido) {
      this.formData.markAllAsTouched();
      return false;
    }
    console.log(
      '🚀 ~ MobilizationComponent ~ procesarPaso6 ~ this.formData.value:',
      this.formData.value
    );
    this.pasoDespliegue = {
      Id: 0,
      TipoPaso: 6,
      IdCapacidad: controlCapacidad?.value.id ?? 0,
      MedioNoCatalogado: this.formData.value.paso6.MedioNoCatalogado || '',
      FechaHoraDespliegue: new Date(this.formData.value.paso6.FechaHoraDespliegue).toISOString(),
      FechaHoraInicioIntervencion: this.formData.value.paso6.FechaHoraInicioIntervencion
        ? new Date(this.formData.value.paso6.FechaHoraInicioIntervencion).toISOString()
        : '',
      Descripcion: this.formData.value.paso6.Descripcion6 || '',
      Observaciones: this.formData.value.paso6.Observaciones6 || '',
    };

    console.log(
      '🚀 ~ MobilizationComponent ~ procesarPaso6 ~  this.pasoDespliegue:',
      this.pasoDespliegue
    );
    return true;
  }

  async sendDataToEndpoint() {
    if (this.movilizacionService.dataMovilizacion().length > 0 && !this.editData) {
      this.save.emit({ save: true, delete: false, close: false, update: false });
    } else {
      if (this.editData) {
        this.save.emit({ save: false, delete: false, close: false, update: true });
      }
    }
  }

  cargarPaso(movilizacion: Movilizacion) {
    this.btnGuardar = 'Guardar';
    this.editTipo = false;
    const controlTipoAdmin = this.formData.get('idTipoNotificacion');
    controlTipoAdmin?.setValue({ id: null });
    this.movilizacionSeleccionada = movilizacion;
    const pasoActual = this.getMaxTipoPaso(movilizacion);
    this.loadTipo(pasoActual);
  }

  getMaxTipoPaso(movilizacion: Movilizacion): number {
    if (!movilizacion || !movilizacion.Pasos || movilizacion.Pasos.length === 0) {
      return 0;
    }
    return movilizacion.Pasos.reduce((max, paso) => (paso.TipoPaso > max ? paso.TipoPaso : max), 0);
  }

  getAllMovilizaciones(): Movilizacion[] {
    return (
      this.movilizacionService
        .dataMovilizacion()
        ?.flatMap((actuacion) => actuacion.Movilizaciones) || []
    );
  }

  editarItem(index: number) {
    const dataEditada = this.formData.value;
    this.movilizacionService.dataMovilizacion.update((data) => {
      data[index] = { ...data[index], ...dataEditada };
      return [...data];
    });
    this.isCreate.set(-1);
    this.formData.reset();
  }

  eliminarItem(index: number): void {
    this.editTipo = false;
    this.btnGuardar = 'Nueva solicitud';
    const actuaciones = this.movilizacionService.dataMovilizacion();

    if (actuaciones && actuaciones.length > 0) {
      const actuacion = actuaciones[0];
      const movilizaciones = actuacion.Movilizaciones;

      if (index >= 0 && index < movilizaciones.length) {
        const idToDelete = movilizaciones[index].Id;
        console.log('Eliminando la movilización con id:', idToDelete);
        movilizaciones.splice(index, 1);
        actuacion.Movilizaciones = movilizaciones;
        this.movilizacionService.dataMovilizacion.set([actuacion]);
        this.loadTipo(0);
        const controlTipoAdmin = this.formData.get('idTipoNotificacion');
        controlTipoAdmin?.setValue({ id: null });
      } else {
        console.error('Índice fuera del rango de movilizaciones');
      }
    } else {
      console.error('No se encontró ninguna actuación en dataMovilizacion');
    }
  }

  async seleccionarItem(index: number) {
    this.isCreate.set(index);
    const data = this.movilizacionService.dataMovilizacion()[index];
  }

  get hasMovilizaciones(): boolean {
    const data = this.movilizacionService.dataMovilizacion();
    return data && data.length > 0 && data[0].Movilizaciones && data[0].Movilizaciones.length > 0;
  }

  getFormatdate(date: any) {
    if (date) {
      return moment(date).format('DD/MM/YYYY');
    } else {
      return 'Sin fecha selecionada.';
    }
  }

  getTipoNotificacion(value: any) {
    var tipo: any;

    if (_isNumberValue(value)) {
      tipo = this.tiposGestion().find((tipo) => tipo.id === value) || null;
    } else {
      tipo = this.tiposGestion().find((tipo) => tipo.id === value.id) || null;
    }

    return tipo.descripcion;
  }

  async loadTipo(id?: any) {
    this.spinner.show();
    id === 8
      ? this.formData.get('idTipoNotificacion')?.disable()
      : this.formData.get('idTipoNotificacion')?.enable();
    const tipos = await ((await id)
      ? this.movilizacionService.getTipoGestion(id)
      : this.movilizacionService.getTipoGestion());
    this.tiposGestion.set(tipos);

    this.spinner.hide();
    return;
  }

  hasTipoPaso1OrLastStep8(actuaciones: ActuacionRelevante[]): boolean {
    return actuaciones.some((actuacion) =>
      actuacion.Movilizaciones.some((movilizacion) => {
        const pasos = movilizacion.Pasos;
        if (!pasos || pasos.length === 0) {
          return false;
        }
        if (pasos.length === 1) {
          return pasos[0].TipoPaso === 1;
        } else {
          return pasos[pasos.length - 1].TipoPaso === 8;
        }
      })
    );
  }

  getForm(atributo: string): any {
    return this.formData.controls[atributo];
  }

  trackByFn(index: number, item: any): number {
    return item.id;
  }

  closeModal() {
    this.save.emit({ save: false, delete: false, close: true, update: false });
  }

  delete() {
    this.save.emit({ save: false, delete: true, close: false, update: false });
  }

  isInteger(value: any): boolean {
    return Number.isInteger(value);
  }

  getFormGroup(controlName: string): FormGroup {
    return this.formData.get(controlName) as FormGroup;
  }

  async editarPaso(paso: any): Promise<void> {
    this.editTipo = true;
    console.log('🚀 ~ MobilizationComponent ~ editarPaso ~ paso:', paso);
    this.btnGuardar = 'Guardar';
    this.editar = true;
    await this.loadTipo(paso.TipoPaso - 1);

    const foundTipoAdmin = this.tiposGestion().find((item) => item.id === paso.TipoPaso);
    const controlTipoAdmin = this.formData.get('idTipoNotificacionEdit');
    if (foundTipoAdmin) {
      console.log('🚀 ~ Step5Component ~ loadMedio ~ foundTipoAdmin:', foundTipoAdmin);
      controlTipoAdmin?.setValue(foundTipoAdmin);
      controlTipoAdmin?.disable();
    }

    if (paso.TipoPaso === 1) {
      const procedenciaSeleccionada = this.dataMaestros.procedencia.find(
        (proc: any) => proc.id === paso.IdProcedenciaMedio
      );
      if (!procedenciaSeleccionada) {
        return;
      }
      const controlTipoAdmin = this.formData.get('idTipoNotificacion');
      controlTipoAdmin?.setValue({ id: 1 });

      this.formData.get('paso1')?.patchValue({
        IdProcedenciaMedio: procedenciaSeleccionada,
        AutoridadSolicitante: paso.AutoridadSolicitante,
        FechaHoraSolicitud: new Date(paso.FechaHoraSolicitud),
        Descripcion: paso.Descripcion,
        Observaciones: paso.Observaciones,
      });
    } else if (paso.TipoPaso === 2) {
      if (paso.TipoPaso === 2) {
        const destinoSeleccionado = this.dataMaestros.destinos.find(
          (dest: any) => dest.id === paso.IdDestinoMedio
        );
        if (!destinoSeleccionado) {
          return;
        }

        this.formData.get('idTipoNotificacion')?.patchValue({ id: 2 });
        this.formData.get('paso2')?.patchValue({
          IdDestinoMedio: destinoSeleccionado, // IdDestinoMedio
          TitularMedio: paso.TitularMedio,
          FechaHoraTramitacion: new Date(paso.FechaHoraTramitacion),
          PublicadoCECIS: paso.PublicadoCECIS,
          Descripcion2: paso.Descripcion,
          Observaciones2: paso.Observaciones,
        });
      }
    } else if (paso.TipoPaso === 3) {
      if (paso.TipoPaso === 3) {
        this.formData.get('idTipoNotificacion')?.patchValue({ id: 3 });
        this.formData.get('paso3')?.patchValue({
          TitularMedio3: paso.TitularMedio,
          GestionCECOD: paso.GestionCECOD,
          FechaHoraOfrecimiento: new Date(paso.FechaHoraOfrecimiento),
          Descripcion3: paso.Descripcion,
          FechaHoraDisponibilidad: new Date(paso.FechaHoraDisponibilidad),
          Observaciones3: paso.Observaciones,
        });
      }
    } else if (paso.TipoPaso === 5) {
      if (paso.TipoPaso === 5) {
        const capacidadCeleccionada = this.dataMaestros.capacidades.find(
          (cap: any) => cap.id === paso.IdCapacidad
        );
        if (!capacidadCeleccionada) {
          return;
        }
        this.tipoAdmin.set(this.dataMaestros.tipoAdmin);
        const foundTipoAdmin = this.tipoAdmin().find((item) => item.id === 1);
        const controlTipoAdmin = this.formData.get('IdTipoAdministracion');
        if (foundTipoAdmin) {
          console.log('🚀 ~ Step5Component ~ loadMedio ~ foundTipoAdmin:', foundTipoAdmin);
          controlTipoAdmin?.setValue(foundTipoAdmin);
          controlTipoAdmin?.disable();
        }

        this.formData.get('idTipoNotificacion')?.patchValue({ id: 5 });
        this.formData.get('paso5')?.patchValue({
          IdCapacidad: capacidadCeleccionada,
          MedioNoCatalogado: paso.MedioNoCatalogado,
          IdTipoAdministracion: foundTipoAdmin,
          TitularMedio5: paso.TitularMedio,
          FechaHoraAportacion: paso.FechaHoraAportacion ? new Date(paso.FechaHoraAportacion) : null,
          Descripcion5: paso.Descripcion,
        });
      }
    } else if (paso.TipoPaso === 6) {
      if (paso.TipoPaso === 6) {
        const capacidadCeleccionada = this.dataMaestros.capacidades.find(
          (cap: any) => cap.id === paso.IdCapacidad
        );
        if (!capacidadCeleccionada) {
          return;
        }

        this.formData.get('idTipoNotificacion')?.patchValue({ id: 6 });
        this.formData.get('paso6')?.patchValue({
          IdCapacidad: capacidadCeleccionada,
          MedioNoCatalogado: paso.MedioNoCatalogado,
          FechaHoraDespliegue: new Date(paso.FechaHoraDespliegue),
          FechaHoraInicioIntervencion: new Date(paso.FechaHoraInicioIntervencion),
          Descripcion6: paso.Descripcion,
          Observaciones6: paso.Observaciones,
        });
      }
    } else if (paso.TipoPaso === 7) {
      if (paso.TipoPaso === 7) {
        const capacidadCeleccionada = this.dataMaestros.capacidades.find(
          (cap: any) => cap.id === paso.IdCapacidad
        );
        if (!capacidadCeleccionada) {
          return;
        }

        this.formData.get('idTipoNotificacion')?.patchValue({ id: 7 });
        this.formData.get('paso7')?.patchValue({
          IdCapacidad: capacidadCeleccionada,
          MedioNoCatalogado: paso.MedioNoCatalogado,
          FechaHoraInicioIntervencion: new Date(paso.FechaHoraInicioIntervencion),
          Observaciones7: paso.Observaciones,
        });
      }
    } else if (paso.TipoPaso === 8) {
      if (paso.TipoPaso === 8) {
        const capacidadCeleccionada = this.dataMaestros.capacidades.find(
          (cap: any) => cap.id === paso.IdCapacidad
        );
        if (!capacidadCeleccionada) {
          return;
        }

        this.formData.get('idTipoNotificacion')?.patchValue({ id: 8 });
        this.formData.get('paso8')?.patchValue({
          IdCapacidad: capacidadCeleccionada,
          MedioNoCatalogado: paso.MedioNoCatalogado,
          FechaHoraLlegada: new Date(paso.FechaHoraLlegada),
          Observaciones8: paso.Observaciones,
        });
      }
    }
  }

  obtenerAutoridadSolicitante(movilizacion: any): string | null {
    const pasoEncontrado = movilizacion.Pasos.find((paso: any) => paso.TipoPaso === 5);
    console.log(
      '🚀 ~ MobilizationComponent ~ obtenerAutoridadSolicitante ~ pasoEncontrado:',
      pasoEncontrado
    );
    if (!pasoEncontrado) {
      return 'N/A';
    }
    if (pasoEncontrado.MedioNoCatalogado) {
      return pasoEncontrado.MedioNoCatalogado;
    } else {
      const capacidadEncontrada = this.dataMaestros.capacidades.find(
        (capacidad: any) => capacidad.id === pasoEncontrado.IdCapacidad
      );
      return capacidadEncontrada ? capacidadEncontrada.nombre : 'N/A';
    }
  }

  compareTipo(tipo1: any, tipo2: any): boolean {
    return tipo1 && tipo2 ? tipo1.id === tipo2.id : tipo1 === tipo2;
  }
}
