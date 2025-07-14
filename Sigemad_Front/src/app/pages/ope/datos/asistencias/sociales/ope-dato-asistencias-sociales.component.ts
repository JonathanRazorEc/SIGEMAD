import { CommonModule } from '@angular/common';
import { Component, EventEmitter, Input, OnChanges, OnInit, Output, SimpleChanges, ViewChild, computed, effect, inject, signal } from '@angular/core';
import { FormBuilder, FormGroup, FormGroupDirective, ReactiveFormsModule, Validators } from '@angular/forms';
import { MatDatepickerModule } from '@angular/material/datepicker';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { MatSelectModule } from '@angular/material/select';
import { MatButtonModule } from '@angular/material/button';
import { MatIconModule } from '@angular/material/icon';
import { MatSort, MatSortModule } from '@angular/material/sort';
import { MatTableDataSource, MatTableModule } from '@angular/material/table';
import { NgxSpinnerModule, NgxSpinnerService } from 'ngx-spinner';
import { UtilsService } from '@shared/services/utils.service';
import { FlexLayoutModule } from '@angular/flex-layout';
import { MatSnackBar } from '@angular/material/snack-bar';
import { OpeDatoAsistenciaSocial } from '@type/ope/datos/ope-dato-asistencia-social.type';
import { OpeAsistenciaSocialTipo } from '@type/ope/datos/ope-asistencia-social-tipo.type';
import { OpeAsistenciasSocialesTiposService } from '@services/ope/datos/ope-asistencias-sociales-tipos.service';
import { MatDialog } from '@angular/material/dialog';
import { OpeDatoAsistenciaSocialCreateEdit } from './ope-dato-asistencia-social-create-edit-form/ope-dato-asistencia-social-create-edit-form.component';
import { OpeDatoAsistenciaSocialTarea } from '@type/ope/datos/ope-dato-asistencia-social-tarea.type';
import { OpeDatoAsistenciaSocialOrganismo } from '@type/ope/datos/ope-dato-asistencia-social-organismo.type';
import { OpeDatoAsistenciaSocialUsuario } from '@type/ope/datos/ope-dato-asistencia-social-usuario.type';
import { OpeValidator } from '@shared/validators/ope/ope-validator';
import { MatAutocompleteModule } from '@angular/material/autocomplete';
import { MatSortNoClearDirective } from '@shared/directive/ope/mat-sort-no-clear.directive';
import { TooltipDirective } from '@shared/directive/tooltip/tooltip.directive';
import { OpeDatosAsistenciasValidator } from '@shared/validators/ope/datos/ope-datos-asistencias-validator';

interface FormType {
  id?: number;
  idOpeDatoAsistencia?: number;
  opeAsistenciaSocialTipo: { id: number; nombre: string };
  numero: number;
  observaciones: string;
  opeDatosAsistenciasSocialesTareas?: OpeDatoAsistenciaSocialTarea[];
  opeDatosAsistenciasSocialesOrganismos?: OpeDatoAsistenciaSocialOrganismo[];
  opeDatosAsistenciasSocialesUsuarios?: OpeDatoAsistenciaSocialUsuario[];
}

@Component({
  selector: 'app-ope-dato-asistencias-sociales',
  standalone: true,
  imports: [
    CommonModule,
    ReactiveFormsModule,
    MatFormFieldModule,
    MatInputModule,
    MatSelectModule,
    MatDatepickerModule,
    MatButtonModule,
    MatTableModule,
    MatIconModule,
    NgxSpinnerModule,
    FlexLayoutModule,
    MatAutocompleteModule,
    MatSortModule,
    MatSortNoClearDirective,
    TooltipDirective,
  ],
  templateUrl: './ope-dato-asistencias-sociales.component.html',
  styleUrl: './ope-dato-asistencias-sociales.component.scss',
})
export class OpeDatoAsistenciasSocialesComponent implements OnInit, OnChanges {
  @Input() opeDatosAsistenciasSociales: OpeDatoAsistenciaSocial[] = [];
  @Output() opeDatosAsistenciasSocialesChange = new EventEmitter<any[]>();
  @Input() fecha!: string;

  constructor(
    private opeAsistenciasSocialesTiposService: OpeAsistenciasSocialesTiposService,
    public utilsService: UtilsService,
    private dialog: MatDialog
  ) {}

  @ViewChild(MatSort) sort!: MatSort;

  public snackBar = inject(MatSnackBar);

  private fb = inject(FormBuilder);
  formData!: FormGroup;

  public opeAsistenciasSocialesTipos = signal<OpeAsistenciaSocialTipo[]>([]);
  public dataOpeDatosAsistenciasSociales = signal<FormType[]>([]);

  public isCreate = signal<number>(-1);
  public isSaving = signal<boolean>(false);

  //public dataSource = new MatTableDataSource<any>([]);
  //
  public dataSourceOpeDatosAsistenciasSociales = new MatTableDataSource<FormType>();
  private syncEffect = effect(() => {
    this.dataSourceOpeDatosAsistenciasSociales.data = this.dataOpeDatosAsistenciasSociales();
  });
  //

  public displayedColumns: string[] = [
    'opeAsistenciaSocialTipo',
    'numero',
    'numeroOpeDatosAsistenciasSocialesTareas',
    'numeroOpeDatosAsistenciasSocialesOrganismos',
    'numeroOpeDatosAsistenciasSocialesUsuarios',
    'observaciones',
    'opciones',
  ];

  ////
  filaSeleccionadaIndex: number | null = null;
  ///

  async ngOnInit() {
    this.formData = this.fb.group({
      opeAsistenciaSocialTipo: [
        '',
        [Validators.required, OpeValidator.opcionValidaDeSelectPorOption(() => this.opeAsistenciasSocialesTiposFiltrados())],
      ],
      numero: [
        null,
        [
          Validators.required,
          Validators.min(0),
          Validators.max(9999999),
          Validators.pattern(/^\d+$/),
          OpeDatosAsistenciasValidator.validarCampoCeroSiFechaFuturaDatosAsistencias(this.fecha, 'numero'),
          OpeDatosAsistenciasValidator.validarCampoNoCeroSiFechaNoFuturaDatosAsistencias(this.fecha, 'numero'),
        ],
      ],
      observaciones: ['', [Validators.maxLength(1000)]],
    });

    if (this.opeDatosAsistenciasSociales?.length > 0) {
      this.dataOpeDatosAsistenciasSociales.set(this.opeDatosAsistenciasSociales);
    }

    const opeAsistenciasSocialesTipos = await this.opeAsistenciasSocialesTiposService.get();
    this.opeAsistenciasSocialesTipos.set(opeAsistenciasSocialesTipos);

    // Para el autocompletar de los tipos de asistencias sociales
    this.getForm('opeAsistenciaSocialTipo')?.valueChanges.subscribe((value: string) => {
      if (typeof value === 'string') {
        this.filtroOpeAsistenciaSocialTipo.set(value);
      }
    });
    // FIN autocompletar
  }

  //
  ngOnChanges(changes: SimpleChanges): void {
    if (changes['fecha']) {
      this.reConfigurarFormulario();
    }
  }

  //
  ngAfterViewInit(): void {
    //this.dataSourceOpeDatosAsistenciasSanitarias.paginator = this.paginator;
    this.dataSourceOpeDatosAsistenciasSociales.sort = this.sort;
    this.setDataSourceAttributes();
  }

  setDataSourceAttributes() {
    if (this.sort) {
      this.dataSourceOpeDatosAsistenciasSociales.sort = this.sort;

      this.dataSourceOpeDatosAsistenciasSociales.sortingDataAccessor = (item, property) => {
        switch (property) {
          case 'opeAsistenciaSocialTipo': {
            return item.opeAsistenciaSocialTipo?.nombre || '';
          }
          case 'numeroOpeDatosAsistenciasSocialesTareas': {
            return this.getNumeroTotalDeListado(item.opeDatosAsistenciasSocialesTareas);
          }
          case 'numeroOpeDatosAsistenciasSocialesOrganismos': {
            return this.getNumeroTotalDeListado(item.opeDatosAsistenciasSocialesOrganismos);
          }
          case 'numeroOpeDatosAsistenciasSocialesUsuarios': {
            return this.getNumeroTotalDeListado(item.opeDatosAsistenciasSocialesUsuarios);
          }
          default: {
            const result = item[property as keyof FormType];
            return typeof result === 'string' || typeof result === 'number' ? result : '';
          }
        }
      };

      this.dataSourceOpeDatosAsistenciasSociales._updateChangeSubscription();
    }
  }
  //

  reConfigurarFormulario(): void {
    // Guardamos los valores actuales antes de reconfigurar
    const currentValues = this.formData ? this.formData.value : {};

    // Reconfiguramos el formulario con los nuevos validadores y configuración
    this.formData = this.fb.group({
      opeAsistenciaSocialTipo: [currentValues.opeAsistenciaSocialTipo ?? '', Validators.required],
      numero: [
        currentValues.numero ?? null, // Usamos el valor actual si existe
        [
          Validators.required,
          Validators.min(0),
          Validators.max(9999999),
          Validators.pattern(/^\d+$/),
          OpeDatosAsistenciasValidator.validarCampoCeroSiFechaFuturaDatosAsistencias(this.fecha, 'numero'),
        ],
      ],
      observaciones: [currentValues.observaciones ?? '', [Validators.maxLength(1000)]],
    });

    // Después de reconfigurar, asignamos los valores anteriores para no perderlos
    this.formData.patchValue(currentValues);
  }
  //

  onSubmit(formDirective: FormGroupDirective): void {
    if (this.formData.valid) {
      const data = this.formData.value;

      // VALIDAR DUPLICADOS
      const yaExiste = OpeDatosAsistenciasValidator.existeTipoDuplicadoEnLista(
        this.dataOpeDatosAsistenciasSociales(),
        (item) => item.opeAsistenciaSocialTipo.id,
        data.opeAsistenciaSocialTipo.id,
        this.isCreate()
      );

      if (yaExiste) {
        this.snackBar.open('El tipo de asistencia social ya existe en la lista', '', {
          duration: 3000,
          horizontalPosition: 'center',
          verticalPosition: 'bottom',
          panelClass: ['snackbar-rojo'],
        });
        return;
      }
      // FIN VALIDAR DUPLICADOS

      if (this.isCreate() == -1) {
        this.dataOpeDatosAsistenciasSociales.set([data, ...this.dataOpeDatosAsistenciasSociales()]);
      } else {
        this.editarItem(this.isCreate());
      }

      this.opeDatosAsistenciasSocialesChange.emit(
        this.dataOpeDatosAsistenciasSociales().map((item) => ({
          id: item.id,
          idOpeDatoAsistencia: item.idOpeDatoAsistencia,
          idOpeAsistenciaSocialTipo: item.opeAsistenciaSocialTipo.id,
          opeAsistenciaSocialTipo: item.opeAsistenciaSocialTipo,
          numero: item.numero,
          observaciones: item.observaciones,
        }))
      );

      formDirective.resetForm();
      this.formData.reset({
        opeAsistenciaSocialTipo: '',
      });
    } else {
      this.formData.markAllAsTouched();
    }
  }

  seleccionarItem(index: number) {
    this.isCreate.set(index);

    const opeAsistenciaSocialTipoSelected = () =>
      this.opeAsistenciasSocialesTipos().find(
        (opeAsistenciaSocialTipo) => opeAsistenciaSocialTipo.id === Number(this.dataOpeDatosAsistenciasSociales()[index].opeAsistenciaSocialTipo.id)
      );

    this.formData.patchValue({
      ...this.dataOpeDatosAsistenciasSociales()[index],
      opeAsistenciaSocialTipo: opeAsistenciaSocialTipoSelected(),
    });

    //this.formData.patchValue(this.dataOtherInformation()[index]);

    //
    //this.filaSeleccionadaIndex = index;
    //
  }

  editarItem(index: number) {
    const dataEditada = this.formData.value;
    this.dataOpeDatosAsistenciasSociales.update((data) => {
      data[index] = { ...data[index], ...dataEditada };
      return [...data];
    });
    this.isCreate.set(-1);
    this.formData.reset();
  }

  eliminarItem(index: number) {
    this.dataOpeDatosAsistenciasSociales.update((data) => {
      data.splice(index, 1);
      return [...data];
    });
    this.opeDatosAsistenciasSocialesChange.emit(
      this.dataOpeDatosAsistenciasSociales().map((item) => ({
        id: item.id,
        idOpeDatoAsistencia: item.idOpeDatoAsistencia,
        idOpeAsistenciaSocialTipo: item.opeAsistenciaSocialTipo.id,
        opeAsistenciaSocialTipo: item.opeAsistenciaSocialTipo,
        numero: item.numero,
        observaciones: item.observaciones,
      }))
    );
  }

  editarDatoAsistenciaSocial(opeDatoAsistenciaSocial: OpeDatoAsistenciaSocial, index: number) {
    //
    this.filaSeleccionadaIndex = index;
    //

    const dialogRef = this.dialog.open(OpeDatoAsistenciaSocialCreateEdit, {
      width: '85vw',
      maxWidth: 'none',
      disableClose: true,
      data: {
        opeDatoAsistenciaSocial: opeDatoAsistenciaSocial,
        opeAsistenciasSocialesTipos: this.opeAsistenciasSocialesTipos(),
        fecha: this.fecha,
      },
    });

    dialogRef.afterClosed().subscribe((result) => {
      this.filaSeleccionadaIndex = null;
      if (result) {
        //console.log('Modal result:', result);
        const {
          params,
          opeDatoAsistenciaSocialModificado,
          opeDatosAsistenciasSocialesTareasModificado,
          opeDatosAsistenciasSocialesOrganismosModificado,
          opeDatosAsistenciasSocialesUsuariosModificado,
        } = result;

        if (
          result.opeDatoAsistenciaSocialModificado ||
          result.opeDatosAsistenciasSocialesTareasModificado ||
          result.opeDatosAsistenciasSocialesOrganismosModificado ||
          result.opeDatosAsistenciasSocialesUsuariosModificado
        ) {
          this.opeDatosAsistenciasSocialesChange.emit(
            this.dataOpeDatosAsistenciasSociales().map((item) => ({
              id: item.id,
              idOpeDatoAsistencia: item.idOpeDatoAsistencia,
              idOpeAsistenciaSocialTipo: item.opeAsistenciaSocialTipo.id,
              opeAsistenciaSocialTipo: item.opeAsistenciaSocialTipo,
              numero: item.numero,
              observaciones: item.observaciones,
              opeDatosAsistenciasSocialesTareas: item.opeDatosAsistenciasSocialesTareas,
              opeDatosAsistenciasSocialesOrganismos: item.opeDatosAsistenciasSocialesOrganismos,
              opeDatosAsistenciasSocialesUsuarios: item.opeDatosAsistenciasSocialesUsuarios,

              opeDatoAsistenciaSocialModificado,
              opeDatosAsistenciasSocialesTareasModificado,
              opeDatosAsistenciasSocialesOrganismosModificado,
              opeDatosAsistenciasSocialesUsuariosModificado,
            }))
          );
        }
      }
    });
  }

  getNumeroTotalDeListado(array: any[] | undefined): number {
    if (!array) return 0;
    return array.reduce((acc, item) => acc + (item.numero || 0), 0);
  }

  // Para el autocompletar de los tipos de asistencias sociales
  getForm(atributo: string): any {
    return this.formData.controls[atributo];
  }

  filtroOpeAsistenciaSocialTipo = signal('');

  // Para input con objeto como valor
  displayFnOpeAsistenciaSocialTipo = (option: { id: number; nombre: string } | null): string => {
    return option ? option.nombre : '';
  };

  // Computado para filtrar lista corta
  opeAsistenciasSocialesTiposFiltrados = computed(() => {
    return this.utilsService.filtrarPorTexto(this.opeAsistenciasSocialesTipos(), this.filtroOpeAsistenciaSocialTipo(), false);
  });

  // Fin autocompletar
}
