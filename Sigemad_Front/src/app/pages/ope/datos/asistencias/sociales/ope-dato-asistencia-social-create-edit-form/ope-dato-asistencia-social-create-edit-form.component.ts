import { CommonModule } from '@angular/common';
import { ChangeDetectorRef, Component, computed, Inject, inject, Input, OnInit, Renderer2, signal, SimpleChanges } from '@angular/core';
import { FormControl, FormGroup, FormGroupDirective, FormsModule, ReactiveFormsModule, Validators } from '@angular/forms';
import { FlexLayoutModule } from '@angular/flex-layout';
import { MatAutocompleteModule, MatAutocompleteSelectedEvent } from '@angular/material/autocomplete';
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
import { FECHA_MAXIMA_DATEPICKER, FECHA_MINIMA_DATEPICKER } from '@type/constants';
import { OpePuertosService } from '@services/ope/administracion/ope-puertos.service';
import { MomentDateAdapter } from '@angular/material-moment-adapter';
import { MatChipListboxChange, MatChipsModule } from '@angular/material/chips';
import { OpeDatoAsistenciasSanitariasComponent } from '../../sanitarias/ope-dato-asistencias-sanitarias.component';
import { OpeDatoAsistenciasTraduccionesComponent } from '../../traducciones/ope-dato-asistencias-traducciones.component';
import { OpeDatoAsistenciaTraduccion } from '@type/ope/datos/ope-dato-asistencia-traduccion.type';
import { OpeDatoAsistenciaSanitaria } from '@type/ope/datos/ope-dato-asistencia-sanitaria.type';
import { OpeDatoAsistenciaSocial } from '@type/ope/datos/ope-dato-asistencia-social.type';
import { OpeDatoAsistenciasSocialesComponent } from '../ope-dato-asistencias-sociales.component';
import { OpeAsistenciaSocialTipo } from '@type/ope/datos/ope-asistencia-social-tipo.type';
import { OpeDatoAsistenciaSocialTarea } from '@type/ope/datos/ope-dato-asistencia-social-tarea.type';
import { OpeDatoAsistenciaSocialOrganismo } from '@type/ope/datos/ope-dato-asistencia-social-organismo.type';
import { OpeDatoAsistenciasSocialesTareasComponent } from '../tareas/ope-dato-asistencias-sociales-tareas.component';
import { OpeDatoAsistenciasSocialesOrganismosComponent } from '../organismos/ope-dato-asistencias-sociales-organismos.component';
import { OpeDatoAsistenciaSocialUsuario } from '@type/ope/datos/ope-dato-asistencia-social-usuario.type';
import { OpeDatoAsistenciasSocialesUsuariosComponent } from '../usuarios/ope-dato-asistencias-sociales-usuarios.component';
import { OpeValidator } from '@shared/validators/ope/ope-validator';
import { OpeDatosAsistenciasValidator } from '@shared/validators/ope/datos/ope-datos-asistencias-validator';

@Component({
  selector: 'ope-dato-asistencia-social-create-edit',
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
    OpeDatoAsistenciasSocialesTareasComponent,
    OpeDatoAsistenciasSocialesOrganismosComponent,
    OpeDatoAsistenciasSocialesUsuariosComponent,
  ],
  providers: [
    { provide: DateAdapter, useClass: MomentDateAdapter },
    { provide: MAT_DATE_FORMATS, useValue: FORMATO_FECHA },
  ],
  templateUrl: './ope-dato-asistencia-social-create-edit-form.component.html',
  styleUrl: './ope-dato-asistencia-social-create-edit-form.component.scss',
})
export class OpeDatoAsistenciaSocialCreateEdit implements OnInit {
  constructor(
    public dialogRef: MatDialogRef<OpeDatoAsistenciaSocialCreateEdit>,
    //private matDialog: MatDialog,
    public alertService: AlertService,

    //@Inject(MAT_DIALOG_DATA) public data: { opeDatoAsistencia: any }
    @Inject(MAT_DIALOG_DATA)
    public data: { opeDatoAsistenciaSocial: OpeDatoAsistenciaSocial; opeAsistenciasSocialesTipos: OpeAsistenciaSocialTipo[]; fecha: string }
  ) {}

  public formData!: FormGroup;

  public dataSource = new MatTableDataSource<any>([]);

  private spinner = inject(NgxSpinnerService);
  public renderer = inject(Renderer2);

  public snackBar = inject(MatSnackBar);
  public utilsService = inject(UtilsService);
  public routenav = inject(Router);

  //opeAsistenciasSocialesTipos: OpeAsistenciaSocialTipo[] = [];
  //
  opeAsistenciasSocialesTipos = signal<OpeAsistenciaSocialTipo[]>([]);
  //

  sections = [
    { id: 1, label: 'Tareas' },
    { id: 2, label: 'Organismos y entidades' },
    { id: 3, label: 'Usuarios, Afectados y Demandantes' },
  ];

  selectedOption: MatChipListboxChange = { source: null as any, value: 0 };

  opeDatosAsistenciasSocialesTareas: OpeDatoAsistenciaSocialTarea[] = [];
  opeDatosAsistenciasSocialesOrganismos: OpeDatoAsistenciaSocialOrganismo[] = [];
  opeDatosAsistenciasSocialesUsuarios: OpeDatoAsistenciaSocialUsuario[] = [];

  //opeDatoAsistenciaSocialModificado = false;

  async ngOnInit() {
    this.formData = new FormGroup({
      opeAsistenciaSocialTipo: new FormControl('', [
        Validators.required,
        OpeValidator.opcionValidaDeSelectPorId(() => this.opeAsistenciasSocialesTiposFiltrados()),
      ]),
      numero: new FormControl(null, [
        Validators.required,
        Validators.min(0),
        Validators.max(9999999),
        Validators.pattern(/^\d+$/),
        OpeDatosAsistenciasValidator.validarCampoCeroSiFechaFuturaDatosAsistencias(this.data.fecha, 'numero'),
        OpeDatosAsistenciasValidator.validarCampoNoCeroSiFechaNoFuturaDatosAsistencias(this.data.fecha, 'numero'),
      ]),
      observaciones: new FormControl('', [Validators.maxLength(1000)]),
      opeDatoAsistenciaSocialModificado: new FormControl(false),
      opeDatosAsistenciasSocialesTareas: new FormControl([]),
      opeDatosAsistenciasSocialesTareasModificado: new FormControl(false),
      opeDatosAsistenciasSocialesOrganismos: new FormControl([]),
      opeDatosAsistenciasSocialesOrganismosModificado: new FormControl(false),
      opeDatosAsistenciasSocialesUsuarios: new FormControl([]),
      opeDatosAsistenciasSocialesUsuariosModificado: new FormControl(false),
    });

    if (this.data.opeDatoAsistenciaSocial) {
      this.formData.patchValue({
        id: this.data.opeDatoAsistenciaSocial.id ? this.data.opeDatoAsistenciaSocial.id : null,
        //opeAsistenciaSocialTipo: this.data.opeDatoAsistenciaSocial.idOpeAsistenciaSocialTipo,
        opeAsistenciaSocialTipo: this.data.opeDatoAsistenciaSocial.opeAsistenciaSocialTipo.id,
        numero: this.data.opeDatoAsistenciaSocial.numero,
        observaciones: this.data.opeDatoAsistenciaSocial.observaciones,
        opeDatosAsistenciasSocialesTareas: this.data.opeDatoAsistenciaSocial.opeDatosAsistenciasSocialesTareas,
        opeDatosAsistenciasSocialesOrganismos: this.data.opeDatoAsistenciaSocial.opeDatosAsistenciasSocialesOrganismos,
        opeDatosAsistenciasSocialesUsuarios: this.data.opeDatoAsistenciaSocial.opeDatosAsistenciasSocialesUsuarios,
      });

      // Actualizar nº de asistencias en las pestañas
      this.actualizarNumerosPestanias();
      // FIN Actualizar nº de asistencias en las pestañas
    }

    //this.opeAsistenciasSocialesTipos = this.data.opeAsistenciasSocialesTipos;
    //
    this.opeAsistenciasSocialesTipos.set(this.data.opeAsistenciasSocialesTipos);
    //

    // Para el autocompletar de los tipos de asistencias sociales
    this.getForm('opeAsistenciaSocialTipo')?.valueChanges.subscribe((value: string) => {
      if (typeof value === 'string') {
        this.filtroOpeAsistenciaSocialTipo.set(value);
      }
    });
    // FIN autocompletar
  }

  /*
  async onSubmit() {
    if (this.formData.valid) {
      this.spinner.show();
      const data = this.formData.value;

      if (this.data.opeDatoAsistenciaSocial?.id) {
        data.id = this.data.opeDatoAsistenciaSocial.id;
        await this.opeDatosAsistenciasService
          .update(data)
          .then((response) => {
            // PCD
            this.snackBar
              .open('Datos modificados correctamente!', '', {
                duration: 3000,
                horizontalPosition: 'center',
                verticalPosition: 'bottom',
                panelClass: ['snackbar-verde'],
              })
              .afterDismissed()
              .subscribe(() => {
                this.closeModal({ refresh: true });
                this.spinner.hide();
              });
            // FIN PCD
          })
          .catch((error) => {
            console.error('Error', error);
          });
      } else {
        await this.opeDatosAsistenciasService
          .post(data)
          .then((response) => {
            this.snackBar
              .open('Datos creados correctamente!', '', {
                duration: 3000,
                horizontalPosition: 'center',
                verticalPosition: 'bottom',
                panelClass: ['snackbar-verde'],
              })
              .afterDismissed()
              .subscribe(() => {
                this.closeModal({ refresh: true });
                //
                //this.data.opeDatoAsistencia = response as OpeDatoAsistencia;
                //
                this.spinner.hide();
              });
          })
          .catch((error) => {
            console.log(error);
          });
      }
    } else {
      this.formData.markAllAsTouched();
    }
  }
  */

  async onSubmit() {}

  closeModal(params?: any) {
    //this.dialogRef.close(params);
    this.dialogRef.close({
      params,
      opeDatoAsistenciaSocialModificado: this.formData.get('opeDatoAsistenciaSocialModificado')?.value,
      opeDatosAsistenciasSocialesTareasModificado: this.formData.get('opeDatosAsistenciasSocialesTareasModificado')?.value,
      opeDatosAsistenciasSocialesOrganismosModificado: this.formData.get('opeDatosAsistenciasSocialesOrganismosModificado')?.value,
      opeDatosAsistenciasSocialesUsuariosModificado: this.formData.get('opeDatosAsistenciasSocialesUsuariosModificado')?.value,
    });
  }

  getForm(atributo: string): any {
    return this.formData.controls[atributo];
  }

  /*
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
  */

  onSelectionChange(event: MatChipListboxChange): void {
    this.selectedOption = event;
  }

  trackByFn(index: number, item: any): number {
    return item.id;
  }

  actualizarOpeDatosAsistenciasSocialesTareas(lista: OpeDatoAsistenciaSocialTarea[]) {
    this.formData.patchValue({
      opeDatosAsistenciasSocialesTareas: lista,
      opeDatosAsistenciasSocialesTareasModificado: true,
    });

    if (!this.data.opeDatoAsistenciaSocial) {
      this.data.opeDatoAsistenciaSocial = {} as OpeDatoAsistenciaSocial;
    }
    //this.data.opeDatoAsistenciaSocial.opeDatosAsistenciasSocialesTareas = lista;

    if (!this.data.opeDatoAsistenciaSocial.opeDatosAsistenciasSocialesTareas) {
      this.data.opeDatoAsistenciaSocial.opeDatosAsistenciasSocialesTareas = [];
    }

    this.data.opeDatoAsistenciaSocial.opeDatosAsistenciasSocialesTareas.splice(
      0,
      this.data.opeDatoAsistenciaSocial.opeDatosAsistenciasSocialesTareas.length,
      ...lista
    );

    this.data.opeDatoAsistenciaSocial.opeDatosAsistenciasSocialesTareasModificado = true;

    // Actualizar nº de asistencias en las pestañas
    this.actualizarNumerosPestanias();
    // FIN Actualizar nº de asistencias en las pestañas
  }

  actualizarOpeDatosAsistenciasSocialesOrganismos(lista: OpeDatoAsistenciaSocialOrganismo[]) {
    this.formData.patchValue({
      opeDatosAsistenciasSocialesOrganismos: lista,
      opeDatosAsistenciasSocialesOrganismosModificado: true,
    });

    if (!this.data.opeDatoAsistenciaSocial) {
      this.data.opeDatoAsistenciaSocial = {} as OpeDatoAsistenciaSocial;
    }
    //this.data.opeDatoAsistenciaSocial.opeDatosAsistenciasSocialesOrganismos = lista;
    if (!this.data.opeDatoAsistenciaSocial.opeDatosAsistenciasSocialesOrganismos) {
      this.data.opeDatoAsistenciaSocial.opeDatosAsistenciasSocialesOrganismos = [];
    }

    this.data.opeDatoAsistenciaSocial.opeDatosAsistenciasSocialesOrganismos.splice(
      0,
      this.data.opeDatoAsistenciaSocial.opeDatosAsistenciasSocialesOrganismos.length,
      ...lista
    );

    this.data.opeDatoAsistenciaSocial.opeDatosAsistenciasSocialesOrganismosModificado = true;

    // Actualizar nº de asistencias en las pestañas
    this.actualizarNumerosPestanias();
    // FIN Actualizar nº de asistencias en las pestañas
  }

  actualizarOpeDatosAsistenciasSocialesUsuarios(lista: OpeDatoAsistenciaSocialUsuario[]) {
    this.formData.patchValue({
      opeDatosAsistenciasSocialesUsuarios: lista,
      opeDatosAsistenciasSocialesUsuariosModificado: true,
    });

    if (!this.data.opeDatoAsistenciaSocial) {
      this.data.opeDatoAsistenciaSocial = {} as OpeDatoAsistenciaSocial;
    }

    //this.data.opeDatoAsistenciaSocial.opeDatosAsistenciasSocialesUsuarios = lista;
    if (!this.data.opeDatoAsistenciaSocial.opeDatosAsistenciasSocialesUsuarios) {
      this.data.opeDatoAsistenciaSocial.opeDatosAsistenciasSocialesUsuarios = [];
    }

    this.data.opeDatoAsistenciaSocial.opeDatosAsistenciasSocialesUsuarios.splice(
      0,
      this.data.opeDatoAsistenciaSocial.opeDatosAsistenciasSocialesUsuarios.length,
      ...lista
    );

    this.data.opeDatoAsistenciaSocial.opeDatosAsistenciasSocialesUsuariosModificado = true;

    // Actualizar nº de asistencias en las pestañas
    this.actualizarNumerosPestanias();
    // FIN Actualizar nº de asistencias en las pestañas
  }

  /*
  changeTipoAsistenciaSocial(event: any) {
    const valor = (event.target as HTMLSelectElement).value;
    this.data.opeDatoAsistenciaSocial.idOpeAsistenciaSocialTipo = +valor;
    //this.opeDatoAsistenciaSocialModificado = true;
    this.formData.patchValue({
      opeDatoAsistenciaSocialModificado: true,
    });
  }
  */

  //
  changeTipoAsistenciaSocial(event: MatAutocompleteSelectedEvent) {
    const idSeleccionado = event.option.value;

    // Buscar el objeto completo en el array de tipos
    const objetoCompleto = this.data.opeAsistenciasSocialesTipos.find((item) => item.id === idSeleccionado);
    if (objetoCompleto) {
      this.data.opeDatoAsistenciaSocial.opeAsistenciaSocialTipo = objetoCompleto;
    }

    // Actualizar el formulario solo con el id
    this.formData.patchValue({
      opeAsistenciaSocialTipo: idSeleccionado,
      opeDatoAsistenciaSocialModificado: true,
    });
  }
  //

  changeNumero(event: any) {
    const valor = (event.target as HTMLInputElement).value;

    //
    this.formData.markAllAsTouched();
    this.formData.updateValueAndValidity();

    // Si el formulario no es válido, salimos
    if (this.formData.invalid) {
      return;
    }
    //

    this.data.opeDatoAsistenciaSocial.numero = +valor;
    this.formData.patchValue({
      opeDatoAsistenciaSocialModificado: true,
    });
  }

  changeObservaciones(event: any) {
    const valor = (event.target as HTMLTextAreaElement).value;
    this.data.opeDatoAsistenciaSocial.observaciones = valor;
    this.formData.patchValue({
      opeDatoAsistenciaSocialModificado: true,
    });
  }

  actualizarNumerosPestanias() {
    this.sections = [
      {
        id: 1,
        label: `Tareas${
          this.getNumeroTotalDeListado(this.data.opeDatoAsistenciaSocial.opeDatosAsistenciasSocialesTareas) > 0
            ? ' (' + this.getNumeroTotalDeListado(this.data.opeDatoAsistenciaSocial.opeDatosAsistenciasSocialesTareas) + ')'
            : ''
        }`,
      },
      {
        id: 2,
        label: `Organismos y entidades${
          this.getNumeroTotalDeListado(this.data.opeDatoAsistenciaSocial.opeDatosAsistenciasSocialesOrganismos) > 0
            ? ' (' + this.getNumeroTotalDeListado(this.data.opeDatoAsistenciaSocial.opeDatosAsistenciasSocialesOrganismos) + ')'
            : ''
        }`,
      },
      {
        id: 3,
        label: `Usuarios, Afectados y Demandantes${
          this.getNumeroTotalDeListado(this.data.opeDatoAsistenciaSocial.opeDatosAsistenciasSocialesUsuarios) > 0
            ? ' (' + this.getNumeroTotalDeListado(this.data.opeDatoAsistenciaSocial.opeDatosAsistenciasSocialesUsuarios) + ')'
            : ''
        }`,
      },
    ];
  }

  getNumeroTotalDeListado(array: any[] | undefined): number {
    if (!array) return 0;
    return array.reduce((acc, item) => acc + (item.numero || 0), 0);
  }

  // Para el autocompletar de los tipos de asistencias sociales
  filtroOpeAsistenciaSocialTipo = signal('');

  displayFnOpeAsistenciaSocialTipo = (id: number | null): string => {
    if (id == null) return '';
    const match = this.opeAsistenciasSocialesTipos().find((item) => item.id === id);
    return match ? match.nombre : '';
  };

  // Computado para filtrar lista corta
  opeAsistenciasSocialesTiposFiltrados = computed(() => {
    return this.utilsService.filtrarPorTexto(this.opeAsistenciasSocialesTipos(), this.filtroOpeAsistenciaSocialTipo(), false);
  });
  // Fin autocompletar
}
