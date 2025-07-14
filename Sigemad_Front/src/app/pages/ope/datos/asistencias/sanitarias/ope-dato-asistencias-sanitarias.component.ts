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
import { OpeDatoAsistenciaSanitaria } from '@type/ope/datos/ope-dato-asistencia-sanitaria.type';
import { OpeAsistenciaSanitariaTipo } from '@type/ope/datos/ope-asistencia-sanitaria-tipo.type';
import { OpeAsistenciasSanitariasTiposService } from '@services/ope/datos/ope-asistencias-sanitarias-tipos.service';
import { OpeValidator } from '@shared/validators/ope/ope-validator';
import { MatAutocompleteModule } from '@angular/material/autocomplete';
import { MatSortNoClearDirective } from '@shared/directive/ope/mat-sort-no-clear.directive';
import { TooltipDirective } from '@shared/directive/tooltip/tooltip.directive';
import { OpeDatosAsistenciasValidator } from '@shared/validators/ope/datos/ope-datos-asistencias-validator';

interface FormType {
  id?: number;
  idOpeDatoAsistencia?: number;
  opeAsistenciaSanitariaTipo: { id: number; nombre: string };
  numero: number;
  observaciones: string;
}

@Component({
  selector: 'app-ope-dato-asistencias-sanitarias',
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
  templateUrl: './ope-dato-asistencias-sanitarias.component.html',
  styleUrl: './ope-dato-asistencias-sanitarias.component.scss',
})
export class OpeDatoAsistenciasSanitariasComponent implements OnInit, OnChanges {
  @Input() opeDatosAsistenciasSanitarias: OpeDatoAsistenciaSanitaria[] = [];
  @Output() opeDatosAsistenciasSanitariasChange = new EventEmitter<any[]>();
  @Input() fecha!: string;

  constructor(
    private opeAsistenciasSanitariasTiposService: OpeAsistenciasSanitariasTiposService,
    public utilsService: UtilsService
  ) {}

  @ViewChild(MatSort) sort!: MatSort;

  public snackBar = inject(MatSnackBar);

  private fb = inject(FormBuilder);
  formData!: FormGroup;

  public opeAsistenciasSanitariasTipos = signal<OpeAsistenciaSanitariaTipo[]>([]);
  public dataOpeDatosAsistenciasSanitarias = signal<FormType[]>([]);

  public isCreate = signal<number>(-1);
  public isSaving = signal<boolean>(false);

  //public dataSource = new MatTableDataSource<any>([]);
  //
  public dataSourceOpeDatosAsistenciasSanitarias = new MatTableDataSource<FormType>();
  private syncEffect = effect(() => {
    const data = this.dataOpeDatosAsistenciasSanitarias();
    // Añadimos originalIndex a cada elemento para mantener referencia fija
    const dataWithIndex = data.map((item, index) => ({ ...item, originalIndex: index }));

    this.dataSourceOpeDatosAsistenciasSanitarias.data = dataWithIndex;
  });
  //

  public displayedColumns: string[] = ['opeAsistenciaSanitariaTipo', 'numero', 'observaciones', 'opciones'];

  ////
  filaSeleccionadaIndex: number | null = null;
  ///

  async ngOnInit() {
    this.formData = this.fb.group({
      opeAsistenciaSanitariaTipo: [
        '',
        [Validators.required, OpeValidator.opcionValidaDeSelectPorOption(() => this.opeAsistenciasSanitariasTiposFiltrados())],
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

    if (this.opeDatosAsistenciasSanitarias?.length > 0) {
      this.dataOpeDatosAsistenciasSanitarias.set(this.opeDatosAsistenciasSanitarias);
    }

    const opeAsistenciasSanitariasTipos = await this.opeAsistenciasSanitariasTiposService.get();
    this.opeAsistenciasSanitariasTipos.set(opeAsistenciasSanitariasTipos);

    // Para el autocompletar de los tipos de asistencias sanitarias
    this.getForm('opeAsistenciaSanitariaTipo')?.valueChanges.subscribe((value: string) => {
      if (typeof value === 'string') {
        this.filtroOpeAsistenciaSanitariaTipo.set(value);
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
    this.dataSourceOpeDatosAsistenciasSanitarias.sort = this.sort;
    this.setDataSourceAttributes();
  }

  setDataSourceAttributes() {
    if (this.sort) {
      this.dataSourceOpeDatosAsistenciasSanitarias.sort = this.sort;

      this.dataSourceOpeDatosAsistenciasSanitarias.sortingDataAccessor = (item, property) => {
        switch (property) {
          case 'opeAsistenciaSanitariaTipo': {
            return item.opeAsistenciaSanitariaTipo?.nombre || '';
          }
          default: {
            const result = item[property as keyof FormType];
            return typeof result === 'string' || typeof result === 'number' ? result : '';
          }
        }
      };

      this.dataSourceOpeDatosAsistenciasSanitarias._updateChangeSubscription();
    }
  }
  //

  reConfigurarFormulario(): void {
    // Guardamos los valores actuales antes de reconfigurar
    const currentValues = this.formData ? this.formData.value : {};

    // Reconfiguramos el formulario con los nuevos validadores y configuración
    this.formData = this.fb.group({
      opeAsistenciaSanitariaTipo: [currentValues.opeAsistenciaSanitariaTipo ?? '', Validators.required],
      numero: [
        currentValues.numero ?? null, // Usamos el valor actual si existe
        [
          Validators.required,
          Validators.min(0),
          Validators.max(9999999),
          Validators.pattern(/^\d+$/),
          OpeDatosAsistenciasValidator.validarCampoCeroSiFechaFuturaDatosAsistencias(this.fecha, 'numero'),
          OpeDatosAsistenciasValidator.validarCampoNoCeroSiFechaNoFuturaDatosAsistencias(this.fecha, 'numero'),
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
        this.dataOpeDatosAsistenciasSanitarias(),
        (item) => item.opeAsistenciaSanitariaTipo.id,
        data.opeAsistenciaSanitariaTipo.id,
        this.isCreate()
      );

      if (yaExiste) {
        this.snackBar.open('El tipo de asistencia sanitaria ya existe en la lista', '', {
          duration: 3000,
          horizontalPosition: 'center',
          verticalPosition: 'bottom',
          panelClass: ['snackbar-rojo'],
        });
        return;
      }
      // FIN VALIDAR DUPLICADOS

      if (this.isCreate() == -1) {
        this.dataOpeDatosAsistenciasSanitarias.set([data, ...this.dataOpeDatosAsistenciasSanitarias()]);
      } else {
        this.editarItem(this.isCreate());
      }

      this.opeDatosAsistenciasSanitariasChange.emit(
        this.dataOpeDatosAsistenciasSanitarias().map((item) => ({
          id: item.id,
          idOpeDatoAsistencia: item.idOpeDatoAsistencia,
          idOpeAsistenciaSanitariaTipo: item.opeAsistenciaSanitariaTipo.id,
          opeAsistenciaSanitariaTipo: item.opeAsistenciaSanitariaTipo,
          numero: item.numero,
          observaciones: item.observaciones,
        }))
      );

      formDirective.resetForm();
      this.formData.reset({
        opeAsistenciaSanitariaTipo: '',
      });
      this.filaSeleccionadaIndex = null;
    } else {
      this.formData.markAllAsTouched();
    }
  }

  seleccionarItem(index: number) {
    this.isCreate.set(index);

    const opeAsistenciaSanitariaTipoSelected = () =>
      this.opeAsistenciasSanitariasTipos().find(
        (opeAsistenciaSanitariaTipo) =>
          opeAsistenciaSanitariaTipo.id === Number(this.dataOpeDatosAsistenciasSanitarias()[index].opeAsistenciaSanitariaTipo.id)
      );

    this.formData.patchValue({
      ...this.dataOpeDatosAsistenciasSanitarias()[index],
      opeAsistenciaSanitariaTipo: opeAsistenciaSanitariaTipoSelected(),
    });

    //this.formData.patchValue(this.dataOtherInformation()[index]);

    //
    this.filaSeleccionadaIndex = index;
    //
  }

  editarItem(index: number) {
    const dataEditada = this.formData.value;
    this.dataOpeDatosAsistenciasSanitarias.update((data) => {
      data[index] = { ...data[index], ...dataEditada };
      return [...data];
    });
    this.isCreate.set(-1);
    this.formData.reset();
  }

  eliminarItem(index: number) {
    this.dataOpeDatosAsistenciasSanitarias.update((data) => {
      data.splice(index, 1);
      return [...data];
    });
    this.opeDatosAsistenciasSanitariasChange.emit(
      this.dataOpeDatosAsistenciasSanitarias().map((item) => ({
        id: item.id,
        idOpeDatoAsistencia: item.idOpeDatoAsistencia,
        idOpeAsistenciaSanitariaTipo: item.opeAsistenciaSanitariaTipo.id,
        opeAsistenciaSanitariaTipo: item.opeAsistenciaSanitariaTipo,
        numero: item.numero,
        observaciones: item.observaciones,
      }))
    );
  }

  // Para el autocompletar de los tipos de asistencias sanitarias
  getForm(atributo: string): any {
    return this.formData.controls[atributo];
  }

  filtroOpeAsistenciaSanitariaTipo = signal('');

  // Para input con objeto como valor
  displayFnOpeAsistenciaSanitariaTipo = (option: { id: number; nombre: string } | null): string => {
    return option ? option.nombre : '';
  };

  // Computado para filtrar lista corta
  opeAsistenciasSanitariasTiposFiltrados = computed(() => {
    return this.utilsService.filtrarPorTexto(this.opeAsistenciasSanitariasTipos(), this.filtroOpeAsistenciaSanitariaTipo(), false);
  });

  // Fin autocompletar
}
