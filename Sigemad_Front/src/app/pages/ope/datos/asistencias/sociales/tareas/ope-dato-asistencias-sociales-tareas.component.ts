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
import { OpeDatoAsistenciaSocialTarea } from '@type/ope/datos/ope-dato-asistencia-social-tarea.type';
import { OpeAsistenciaSocialTareaTipo } from '@type/ope/datos/ope-asistencia-social-tarea-tipo.type';
import { OpeAsistenciasSocialesTareasTiposService } from '@services/ope/datos/ope-asistencias-sociales-tareas-tipos.service';
import { OpeValidator } from '@shared/validators/ope/ope-validator';
import { MatAutocompleteModule } from '@angular/material/autocomplete';
import { MatSortNoClearDirective } from '@shared/directive/ope/mat-sort-no-clear.directive';
import { TooltipDirective } from '@shared/directive/tooltip/tooltip.directive';
import { OpeDatosAsistenciasValidator } from '@shared/validators/ope/datos/ope-datos-asistencias-validator';

interface FormType {
  id?: number;
  idOpeDatoAsistenciaSocial?: number;
  opeAsistenciaSocialTareaTipo: { id: number; nombre: string };
  numero: number;
  observaciones: string;
}

@Component({
  selector: 'app-ope-dato-asistencias-sociales-tareas',
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
  templateUrl: './ope-dato-asistencias-sociales-tareas.component.html',
  styleUrl: './ope-dato-asistencias-sociales-tareas.component.scss',
})
export class OpeDatoAsistenciasSocialesTareasComponent implements OnInit {
  @Input() opeDatosAsistenciasSocialesTareas: OpeDatoAsistenciaSocialTarea[] = [];
  @Output() opeDatosAsistenciasSocialesTareasChange = new EventEmitter<any[]>();
  @Input() fecha!: string;

  constructor(
    private opeAsistenciasSocialesTareasTiposService: OpeAsistenciasSocialesTareasTiposService,
    public utilsService: UtilsService
  ) {}

  @ViewChild(MatSort) sort!: MatSort;

  public snackBar = inject(MatSnackBar);

  private fb = inject(FormBuilder);
  formData!: FormGroup;

  public opeAsistenciasSocialesTareasTipos = signal<OpeAsistenciaSocialTareaTipo[]>([]);
  public dataOpeDatosAsistenciasSocialesTareas = signal<FormType[]>([]);

  public isCreate = signal<number>(-1);
  public isSaving = signal<boolean>(false);

  //public dataSource = new MatTableDataSource<any>([]);
  //
  public dataSourceOpeDatosAsistenciasSocialesTareas = new MatTableDataSource<FormType>();
  private syncEffect = effect(() => {
    const data = this.dataOpeDatosAsistenciasSocialesTareas();
    // Añadimos originalIndex a cada elemento para mantener referencia fija
    const dataWithIndex = data.map((item, index) => ({ ...item, originalIndex: index }));

    this.dataSourceOpeDatosAsistenciasSocialesTareas.data = dataWithIndex;
  });
  //

  public displayedColumns: string[] = ['opeAsistenciaSocialTareaTipo', 'numero', 'observaciones', 'opciones'];

  ////
  filaSeleccionadaIndex: number | null = null;
  ///

  async ngOnInit() {
    this.formData = this.fb.group({
      opeAsistenciaSocialTareaTipo: [
        '',
        [Validators.required, OpeValidator.opcionValidaDeSelectPorOption(() => this.opeAsistenciasSocialesTareasTiposFiltrados())],
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

    if (this.opeDatosAsistenciasSocialesTareas?.length > 0) {
      this.dataOpeDatosAsistenciasSocialesTareas.set(this.opeDatosAsistenciasSocialesTareas);
    }

    const opeAsistenciasSocialesTareasTipos = await this.opeAsistenciasSocialesTareasTiposService.get();
    this.opeAsistenciasSocialesTareasTipos.set(opeAsistenciasSocialesTareasTipos);

    // Para el autocompletar de los tipos de asistencias sociales tareas
    this.getForm('opeAsistenciaSocialTareaTipo')?.valueChanges.subscribe((value: string) => {
      if (typeof value === 'string') {
        this.filtroOpeAsistenciaSocialTareaTipo.set(value);
      }
    });
    // FIN autocompletar
  }

  //
  ngAfterViewInit(): void {
    //this.dataSourceOpeDatosAsistenciasSanitarias.paginator = this.paginator;
    this.dataSourceOpeDatosAsistenciasSocialesTareas.sort = this.sort;
    this.setDataSourceAttributes();
  }

  setDataSourceAttributes() {
    if (this.sort) {
      this.dataSourceOpeDatosAsistenciasSocialesTareas.sort = this.sort;

      this.dataSourceOpeDatosAsistenciasSocialesTareas.sortingDataAccessor = (item, property) => {
        switch (property) {
          case 'opeAsistenciaSocialTareaTipo': {
            return item.opeAsistenciaSocialTareaTipo?.nombre || '';
          }
          default: {
            const result = item[property as keyof FormType];
            return typeof result === 'string' || typeof result === 'number' ? result : '';
          }
        }
      };

      this.dataSourceOpeDatosAsistenciasSocialesTareas._updateChangeSubscription();
    }
  }
  //

  onSubmit(formDirective: FormGroupDirective): void {
    if (this.formData.valid) {
      const data = this.formData.value;

      // VALIDAR DUPLICADOS
      const yaExiste = OpeDatosAsistenciasValidator.existeTipoDuplicadoEnLista(
        this.dataOpeDatosAsistenciasSocialesTareas(),
        (item) => item.opeAsistenciaSocialTareaTipo.id,
        data.opeAsistenciaSocialTareaTipo.id,
        this.isCreate()
      );

      if (yaExiste) {
        this.snackBar.open('El tipo de asistencia social - tarea ya existe en la lista', '', {
          duration: 3000,
          horizontalPosition: 'center',
          verticalPosition: 'bottom',
          panelClass: ['snackbar-rojo'],
        });
        return;
      }
      // FIN VALIDAR DUPLICADOS

      if (this.isCreate() == -1) {
        this.dataOpeDatosAsistenciasSocialesTareas.set([data, ...this.dataOpeDatosAsistenciasSocialesTareas()]);
      } else {
        this.editarItem(this.isCreate());
      }

      this.opeDatosAsistenciasSocialesTareasChange.emit(
        this.dataOpeDatosAsistenciasSocialesTareas().map((item) => ({
          id: item.id,
          idOpeDatoAsistenciaSocial: item.idOpeDatoAsistenciaSocial,
          idOpeAsistenciaSocialTareaTipo: item.opeAsistenciaSocialTareaTipo.id,
          opeAsistenciaSocialTareaTipo: item.opeAsistenciaSocialTareaTipo,
          numero: item.numero,
          observaciones: item.observaciones,
        }))
      );

      formDirective.resetForm();
      this.formData.reset({
        opeAsistenciaSocialTareaTipo: '',
      });
      this.filaSeleccionadaIndex = null;
    } else {
      this.formData.markAllAsTouched();
    }
  }

  seleccionarItem(index: number) {
    this.isCreate.set(index);

    const opeAsistenciaSocialTareaTipoSelected = () =>
      this.opeAsistenciasSocialesTareasTipos().find(
        (opeAsistenciaSocialTareaTipo) =>
          opeAsistenciaSocialTareaTipo.id === Number(this.dataOpeDatosAsistenciasSocialesTareas()[index].opeAsistenciaSocialTareaTipo.id)
      );

    this.formData.patchValue({
      ...this.dataOpeDatosAsistenciasSocialesTareas()[index],
      opeAsistenciaSocialTareaTipo: opeAsistenciaSocialTareaTipoSelected(),
    });

    //this.formData.patchValue(this.dataOtherInformation()[index]);

    //
    this.filaSeleccionadaIndex = index;
    //
  }

  editarItem(index: number) {
    const dataEditada = this.formData.value;
    this.dataOpeDatosAsistenciasSocialesTareas.update((data) => {
      data[index] = { ...data[index], ...dataEditada };
      return [...data];
    });
    this.isCreate.set(-1);
    this.formData.reset();
  }

  eliminarItem(index: number) {
    this.dataOpeDatosAsistenciasSocialesTareas.update((data) => {
      data.splice(index, 1);
      return [...data];
    });
    this.opeDatosAsistenciasSocialesTareasChange.emit(
      this.dataOpeDatosAsistenciasSocialesTareas().map((item) => ({
        id: item.id,
        idOpeDatoAsistenciaSocial: item.idOpeDatoAsistenciaSocial,
        idOpeAsistenciaSocialTareaTipo: item.opeAsistenciaSocialTareaTipo.id,
        opeAsistenciaSocialTareaTipo: item.opeAsistenciaSocialTareaTipo,
        numero: item.numero,
        observaciones: item.observaciones,
      }))
    );
  }

  // Para el autocompletar de los tipos de asistencias sociales tareas
  getForm(atributo: string): any {
    return this.formData.controls[atributo];
  }

  filtroOpeAsistenciaSocialTareaTipo = signal('');

  // Para input con objeto como valor
  displayFnOpeAsistenciaSocialTareaTipo = (option: { id: number; nombre: string } | null): string => {
    return option ? option.nombre : '';
  };

  // Computado para filtrar lista corta
  opeAsistenciasSocialesTareasTiposFiltrados = computed(() => {
    return this.utilsService.filtrarPorTexto(this.opeAsistenciasSocialesTareasTipos(), this.filtroOpeAsistenciaSocialTareaTipo(), false);
  });

  // Fin autocompletar
}
