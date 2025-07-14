import { CommonModule } from '@angular/common';
import { Component, EventEmitter, Input, OnInit, Output, ViewChild, computed, effect, inject, signal } from '@angular/core';
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
import { OpeDatoAsistenciaSocialUsuario } from '@type/ope/datos/ope-dato-asistencia-social-usuario.type';
import { OpeAsistenciaSocialNacionalidad } from '@type/ope/datos/ope-asistencia-social-nacionalidad.type';
import { OpeAsistenciasSocialesNacionalidadesService } from '@services/ope/datos/ope-asistencias-sociales-nacionalidades.service';
import { OpeAsistenciaSocialEdad } from '@type/ope/datos/ope-asistencia-social-edad.type';
import { OpeAsistenciaSocialSexo } from '@type/ope/datos/ope-asistencia-social-sexo.type';
import { OpeAsistenciasSocialesSexosService } from '@services/ope/datos/ope-asistencias-sociales-sexos.service';
import { OpeAsistenciasSocialesEdadesService } from '@services/ope/datos/ope-asistencias-sociales-edades.service';
import { Countries } from '@type/country.type';
import { OpePaisesService } from '@services/ope/administracion/ope-paises.service';
import { OpeValidator } from '@shared/validators/ope/ope-validator';
import { MatAutocompleteModule } from '@angular/material/autocomplete';
import { MatSortNoClearDirective } from '@shared/directive/ope/mat-sort-no-clear.directive';
import { TooltipDirective } from '@shared/directive/tooltip/tooltip.directive';
import { OpeDatosAsistenciasValidator } from '@shared/validators/ope/datos/ope-datos-asistencias-validator';

interface FormType {
  id?: number;
  idOpeDatoAsistenciaSocial?: number;
  opeAsistenciaSocialEdad: { id: number; nombre: string };
  opeAsistenciaSocialSexo: { id: number; nombre: string };
  opeAsistenciaSocialNacionalidad: { id: number; nombre: string };
  paisResidencia: { id: number; descripcion: string };
  numero: number;
  observaciones: string;
}

@Component({
  selector: 'app-ope-dato-asistencias-sociales-usuarios',
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
  templateUrl: './ope-dato-asistencias-sociales-usuarios.component.html',
  styleUrl: './ope-dato-asistencias-sociales-usuarios.component.scss',
})
export class OpeDatoAsistenciasSocialesUsuariosComponent implements OnInit {
  @Input() opeDatosAsistenciasSocialesUsuarios: OpeDatoAsistenciaSocialUsuario[] = [];
  @Output() opeDatosAsistenciasSocialesUsuariosChange = new EventEmitter<any[]>();
  @Input() fecha!: string;

  constructor(
    private opeAsistenciasSocialesEdadesService: OpeAsistenciasSocialesEdadesService,
    private opeAsistenciasSocialesSexosService: OpeAsistenciasSocialesSexosService,
    private opeAsistenciasSocialesNacionalidadesService: OpeAsistenciasSocialesNacionalidadesService,
    //private countryService: CountryService,
    private opePaisesService: OpePaisesService,
    public utilsService: UtilsService
  ) {}

  @ViewChild(MatSort) sort!: MatSort;

  public snackBar = inject(MatSnackBar);

  private fb = inject(FormBuilder);
  formData!: FormGroup;

  public opeAsistenciasSocialesEdades = signal<OpeAsistenciaSocialEdad[]>([]);
  public opeAsistenciasSocialesSexos = signal<OpeAsistenciaSocialSexo[]>([]);
  public opeAsistenciasSocialesNacionalidades = signal<OpeAsistenciaSocialNacionalidad[]>([]);
  public paisesResidenciaOpe = signal<Countries[]>([]);
  public dataOpeDatosAsistenciasSocialesUsuarios = signal<FormType[]>([]);

  public isCreate = signal<number>(-1);
  public isSaving = signal<boolean>(false);

  //public dataSource = new MatTableDataSource<any>([]);
  //
  public dataSourceOpeDatosAsistenciasSocialesUsuarios = new MatTableDataSource<FormType>();
  private syncEffect = effect(() => {
    const data = this.dataOpeDatosAsistenciasSocialesUsuarios();
    // Añadimos originalIndex a cada elemento para mantener referencia fija
    const dataWithIndex = data.map((item, index) => ({ ...item, originalIndex: index }));

    this.dataSourceOpeDatosAsistenciasSocialesUsuarios.data = dataWithIndex;
  });
  //

  public displayedColumns: string[] = [
    'opeAsistenciaSocialEdad',
    'opeAsistenciaSocialSexo',
    'opeAsistenciaSocialNacionalidad',
    'paisResidencia',
    'numero',
    'observaciones',
    'opciones',
  ];

  ////
  filaSeleccionadaIndex: number | null = null;
  ///

  async ngOnInit() {
    this.formData = this.fb.group({
      opeAsistenciaSocialEdad: [
        '',
        [Validators.required, OpeValidator.opcionValidaDeSelectPorOption(() => this.opeAsistenciasSocialesEdadesFiltradas())],
      ],
      opeAsistenciaSocialSexo: ['', Validators.required],
      opeAsistenciaSocialNacionalidad: [
        '',
        [Validators.required, OpeValidator.opcionValidaDeSelectPorOption(() => this.opeAsistenciasSocialesNacionalidadesFiltradas())],
      ],
      paisResidencia: ['', [Validators.required, OpeValidator.opcionValidaDeSelectPorOption(() => this.paisesResidenciaFiltrados())]],
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

    if (this.opeDatosAsistenciasSocialesUsuarios?.length > 0) {
      this.dataOpeDatosAsistenciasSocialesUsuarios.set(this.opeDatosAsistenciasSocialesUsuarios);
    }

    const opeAsistenciasSocialesEdades = await this.opeAsistenciasSocialesEdadesService.get();
    this.opeAsistenciasSocialesEdades.set(opeAsistenciasSocialesEdades);

    const opeAsistenciasSocialesSexos = await this.opeAsistenciasSocialesSexosService.get();
    this.opeAsistenciasSocialesSexos.set(opeAsistenciasSocialesSexos);

    const opeAsistenciasSocialesNacionalidades = await this.opeAsistenciasSocialesNacionalidadesService.get();
    this.opeAsistenciasSocialesNacionalidades.set(opeAsistenciasSocialesNacionalidades);

    //const paisesResidencia = await this.countryService.get();
    //this.paisesResidencia.set(paisesResidencia);

    const opeDatosAsistenciasSocialesUsuariosPaises = await this.opePaisesService.get({
      opeDatosAsistencias: true,
    });
    if (opeDatosAsistenciasSocialesUsuariosPaises != null && opeDatosAsistenciasSocialesUsuariosPaises.length > 0) {
      this.paisesResidenciaOpe.set(opeDatosAsistenciasSocialesUsuariosPaises.map((p) => p.pais));
    }

    // Para el autocompletar de las edades de asistencias sociales usuarios
    this.getForm('opeAsistenciaSocialEdad')?.valueChanges.subscribe((value: string) => {
      if (typeof value === 'string') {
        this.filtroOpeAsistenciaSocialEdad.set(value);
      }
    });
    // FIN autocompletar de las edades de asistencias sociales usuarios

    // Para el autocompletar de las nacionalidades de asistencias sociales usuarios
    this.getForm('opeAsistenciaSocialNacionalidad')?.valueChanges.subscribe((value: string) => {
      if (typeof value === 'string') {
        this.filtroOpeAsistenciaSocialNacionalidad.set(value);
      }
    });
    // FIN autocompletar de las nacionalidades de asistencias sociales usuarios

    // Para el autocompletar de los países de residencia de asistencias sociales usuarios
    this.getForm('paisResidencia')?.valueChanges.subscribe((value: string) => {
      if (typeof value === 'string') {
        this.filtroPaisResidencia.set(value);
      }
    });
    // FIN autocompletar de los países de residencia de asistencias sociales usuarios
  }

  //
  ngAfterViewInit(): void {
    //this.dataSourceOpeDatosAsistenciasSanitarias.paginator = this.paginator;
    this.dataSourceOpeDatosAsistenciasSocialesUsuarios.sort = this.sort;
    this.setDataSourceAttributes();
  }

  setDataSourceAttributes() {
    if (this.sort) {
      this.dataSourceOpeDatosAsistenciasSocialesUsuarios.sort = this.sort;

      this.dataSourceOpeDatosAsistenciasSocialesUsuarios.sortingDataAccessor = (item, property) => {
        switch (property) {
          case 'opeAsistenciaSocialEdad': {
            return item.opeAsistenciaSocialEdad?.nombre || '';
          }
          case 'opeAsistenciaSocialNacionalidad': {
            return item.opeAsistenciaSocialNacionalidad?.nombre || '';
          }
          case 'opeAsistenciaSocialSexo': {
            return item.opeAsistenciaSocialSexo?.nombre || '';
          }
          case 'paisResidencia': {
            return item.paisResidencia?.descripcion || '';
          }
          default: {
            const result = item[property as keyof FormType];
            return typeof result === 'string' || typeof result === 'number' ? result : '';
          }
        }
      };

      this.dataSourceOpeDatosAsistenciasSocialesUsuarios._updateChangeSubscription();
    }
  }
  //

  onSubmit(formDirective: FormGroupDirective): void {
    if (this.formData.valid) {
      const data = this.formData.value;
      if (this.isCreate() == -1) {
        this.dataOpeDatosAsistenciasSocialesUsuarios.set([data, ...this.dataOpeDatosAsistenciasSocialesUsuarios()]);
      } else {
        this.editarItem(this.isCreate());
      }

      this.opeDatosAsistenciasSocialesUsuariosChange.emit(
        this.dataOpeDatosAsistenciasSocialesUsuarios().map((item) => ({
          id: item.id,
          idOpeDatoAsistenciaSocial: item.idOpeDatoAsistenciaSocial,

          idOpeAsistenciaSocialEdad: item.opeAsistenciaSocialEdad?.id,
          opeAsistenciaSocialEdad: item.opeAsistenciaSocialEdad,

          idOpeAsistenciaSocialSexo: item.opeAsistenciaSocialSexo?.id,
          opeAsistenciaSocialSexo: item.opeAsistenciaSocialSexo,

          idOpeAsistenciaSocialNacionalidad: item.opeAsistenciaSocialNacionalidad?.id,
          opeAsistenciaSocialNacionalidad: item.opeAsistenciaSocialNacionalidad,

          idPaisResidencia: item.paisResidencia?.id,
          paisResidencia: item.paisResidencia,

          numero: item.numero,
          observaciones: item.observaciones,
        }))
      );

      formDirective.resetForm();
      this.formData.reset({
        opeAsistenciaSocialEdad: '',
        opeAsistenciaSocialNacionalidad: '',
        paisResidencia: '',
      });
      this.filaSeleccionadaIndex = null;
    } else {
      this.formData.markAllAsTouched();
    }
  }

  seleccionarItem(index: number) {
    this.isCreate.set(index);

    const opeAsistenciaSocialEdadSelected = () =>
      this.opeAsistenciasSocialesEdades().find(
        (opeAsistenciaSocialEdad) =>
          opeAsistenciaSocialEdad.id === Number(this.dataOpeDatosAsistenciasSocialesUsuarios()[index].opeAsistenciaSocialEdad.id)
      );

    const opeAsistenciaSocialSexoSelected = () =>
      this.opeAsistenciasSocialesSexos().find(
        (opeAsistenciaSocialSexo) =>
          opeAsistenciaSocialSexo.id === Number(this.dataOpeDatosAsistenciasSocialesUsuarios()[index].opeAsistenciaSocialSexo.id)
      );

    const opeAsistenciaSocialNacionalidadSelected = () =>
      this.opeAsistenciasSocialesNacionalidades().find(
        (opeAsistenciaSocialNacionalidad) =>
          opeAsistenciaSocialNacionalidad.id === Number(this.dataOpeDatosAsistenciasSocialesUsuarios()[index].opeAsistenciaSocialNacionalidad.id)
      );

    const paisResidenciaOpeSelected = () =>
      this.paisesResidenciaOpe().find(
        (paisResidenciaOpe) => paisResidenciaOpe.id === Number(this.dataOpeDatosAsistenciasSocialesUsuarios()[index].paisResidencia.id)
      );

    this.formData.patchValue({
      ...this.dataOpeDatosAsistenciasSocialesUsuarios()[index],
      opeAsistenciaSocialEdad: opeAsistenciaSocialEdadSelected(),
      opeAsistenciaSocialSexo: opeAsistenciaSocialSexoSelected(),
      opeAsistenciaSocialNacionalidad: opeAsistenciaSocialNacionalidadSelected(),
      paisResidencia: paisResidenciaOpeSelected(),
    });

    //this.formData.patchValue(this.dataOtherInformation()[index]);

    //
    this.filaSeleccionadaIndex = index;
    //
  }

  editarItem(index: number) {
    const dataEditada = this.formData.value;
    this.dataOpeDatosAsistenciasSocialesUsuarios.update((data) => {
      data[index] = { ...data[index], ...dataEditada };
      return [...data];
    });
    this.isCreate.set(-1);
    this.formData.reset();
  }

  eliminarItem(index: number) {
    this.dataOpeDatosAsistenciasSocialesUsuarios.update((data) => {
      data.splice(index, 1);
      return [...data];
    });
    this.opeDatosAsistenciasSocialesUsuariosChange.emit(
      this.dataOpeDatosAsistenciasSocialesUsuarios().map((item) => ({
        id: item.id,
        idOpeDatoAsistenciaSocial: item.idOpeDatoAsistenciaSocial,

        idOpeAsistenciaSocialEdad: item.opeAsistenciaSocialEdad?.id,
        opeAsistenciaSocialEdad: item.opeAsistenciaSocialEdad,

        idOpeAsistenciaSocialSexo: item.opeAsistenciaSocialSexo?.id,
        opeAsistenciaSocialSexo: item.opeAsistenciaSocialSexo,

        idOpeAsistenciaSocialNacionalidad: item.opeAsistenciaSocialNacionalidad?.id,
        opeAsistenciaSocialNacionalidad: item.opeAsistenciaSocialNacionalidad,

        idPaisResidencia: item.paisResidencia?.id,
        paisResidencia: item.paisResidencia,

        numero: item.numero,
        observaciones: item.observaciones,
      }))
    );
  }

  getForm(atributo: string): any {
    return this.formData.controls[atributo];
  }

  // Para el autocompletar de las edades de asistencias sociales usuarios
  filtroOpeAsistenciaSocialEdad = signal('');

  // Para input con objeto como valor
  displayFnOpeAsistenciaSocialEdad = (option: { id: number; nombre: string } | null): string => {
    return option ? option.nombre : '';
  };

  // Computado para filtrar lista corta
  opeAsistenciasSocialesEdadesFiltradas = computed(() => {
    return this.utilsService.filtrarPorTexto(this.opeAsistenciasSocialesEdades(), this.filtroOpeAsistenciaSocialEdad(), true);
  });
  // Fin autocompletar de las edades de asistencias sociales usuarios

  // Para el autocompletar de las nacionalidades de asistencias sociales usuarios
  filtroOpeAsistenciaSocialNacionalidad = signal('');

  // Para input con objeto como valor
  displayFnOpeAsistenciaSocialNacionalidad = (option: { id: number; nombre: string } | null): string => {
    return option ? option.nombre : '';
  };

  // Computado para filtrar lista corta
  opeAsistenciasSocialesNacionalidadesFiltradas = computed(() => {
    return this.utilsService.filtrarPorTexto(this.opeAsistenciasSocialesNacionalidades(), this.filtroOpeAsistenciaSocialNacionalidad(), true);
  });
  // Fin autocompletar de las nacionalidades de asistencias sociales usuarios

  // Para el autocompletar de los países de residencia de asistencias sociales usuarios
  filtroPaisResidencia = signal('');

  // Para input con objeto como valor
  displayFnPaisResidencia = (option: { id: number; descripcion: string } | null): string => {
    return option ? option.descripcion : '';
  };

  // Computado para filtrar lista corta
  paisesResidenciaFiltrados = computed(() => {
    return this.utilsService.filtrarPorTexto(this.paisesResidenciaOpe(), this.filtroPaisResidencia(), true, 'descripcion');
  });
  // Fin autocompletar de los países de residencia de asistencias sociales usuarios
}
