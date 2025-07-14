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
import { OpeDatoAsistenciaSocialOrganismo } from '@type/ope/datos/ope-dato-asistencia-social-organismo.type';
import { OpeAsistenciaSocialOrganismoTipo } from '@type/ope/datos/ope-asistencia-social-organismo-tipo.type';
import { OpeAsistenciasSocialesOrganismosTiposService } from '@services/ope/datos/ope-asistencias-sociales-organismos-tipos.service';
import { OpeValidator } from '@shared/validators/ope/ope-validator';
import { MatAutocompleteModule } from '@angular/material/autocomplete';
import { MatSortNoClearDirective } from '@shared/directive/ope/mat-sort-no-clear.directive';
import { TooltipDirective } from '@shared/directive/tooltip/tooltip.directive';
import { OpeDatosAsistenciasValidator } from '@shared/validators/ope/datos/ope-datos-asistencias-validator';

interface FormType {
  id?: number;
  idOpeDatoAsistenciaSocial?: number;
  opeAsistenciaSocialOrganismoTipo: { id: number; nombre: string };
  numero: number;
  observaciones: string;
}

@Component({
  selector: 'app-ope-dato-asistencias-sociales-organismos',
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
  templateUrl: './ope-dato-asistencias-sociales-organismos.component.html',
  styleUrl: './ope-dato-asistencias-sociales-organismos.component.scss',
})
export class OpeDatoAsistenciasSocialesOrganismosComponent implements OnInit {
  @Input() opeDatosAsistenciasSocialesOrganismos: OpeDatoAsistenciaSocialOrganismo[] = [];
  @Output() opeDatosAsistenciasSocialesOrganismosChange = new EventEmitter<any[]>();
  @Input() fecha!: string;

  constructor(
    private opeAsistenciasSocialesOrganismosTiposService: OpeAsistenciasSocialesOrganismosTiposService,
    public utilsService: UtilsService
  ) {}

  @ViewChild(MatSort) sort!: MatSort;

  public snackBar = inject(MatSnackBar);

  private fb = inject(FormBuilder);
  formData!: FormGroup;

  public opeAsistenciasSocialesOrganismosTipos = signal<OpeAsistenciaSocialOrganismoTipo[]>([]);
  public dataOpeDatosAsistenciasSocialesOrganismos = signal<FormType[]>([]);

  public isCreate = signal<number>(-1);
  public isSaving = signal<boolean>(false);

  //public dataSource = new MatTableDataSource<any>([]);
  //
  public dataSourceOpeDatosAsistenciasSocialesOrganismos = new MatTableDataSource<FormType>();
  private syncEffect = effect(() => {
    const data = this.dataOpeDatosAsistenciasSocialesOrganismos();
    // Añadimos originalIndex a cada elemento para mantener referencia fija
    const dataWithIndex = data.map((item, index) => ({ ...item, originalIndex: index }));

    this.dataSourceOpeDatosAsistenciasSocialesOrganismos.data = dataWithIndex;
  });
  //

  public displayedColumns: string[] = ['opeAsistenciaSocialOrganismoTipo', 'numero', 'observaciones', 'opciones'];

  ////
  filaSeleccionadaIndex: number | null = null;
  ///

  async ngOnInit() {
    this.formData = this.fb.group({
      opeAsistenciaSocialOrganismoTipo: [
        '',
        [Validators.required, OpeValidator.opcionValidaDeSelectPorOption(() => this.opeAsistenciasSocialesOrganismosTiposFiltrados())],
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

    if (this.opeDatosAsistenciasSocialesOrganismos?.length > 0) {
      this.dataOpeDatosAsistenciasSocialesOrganismos.set(this.opeDatosAsistenciasSocialesOrganismos);
    }

    const opeAsistenciasSocialesOrganismosTipos = await this.opeAsistenciasSocialesOrganismosTiposService.get();
    this.opeAsistenciasSocialesOrganismosTipos.set(opeAsistenciasSocialesOrganismosTipos);

    // Para el autocompletar de los tipos de asistencias sociales organismos
    this.getForm('opeAsistenciaSocialOrganismoTipo')?.valueChanges.subscribe((value: string) => {
      if (typeof value === 'string') {
        this.filtroOpeAsistenciaSocialOrganismoTipo.set(value);
      }
    });
    // FIN autocompletar
  }

  //
  ngAfterViewInit(): void {
    //this.dataSourceOpeDatosAsistenciasSanitarias.paginator = this.paginator;
    this.dataSourceOpeDatosAsistenciasSocialesOrganismos.sort = this.sort;
    this.setDataSourceAttributes();
  }

  setDataSourceAttributes() {
    if (this.sort) {
      this.dataSourceOpeDatosAsistenciasSocialesOrganismos.sort = this.sort;

      this.dataSourceOpeDatosAsistenciasSocialesOrganismos.sortingDataAccessor = (item, property) => {
        switch (property) {
          case 'opeAsistenciaSocialOrganismoTipo': {
            return item.opeAsistenciaSocialOrganismoTipo?.nombre || '';
          }
          default: {
            const result = item[property as keyof FormType];
            return typeof result === 'string' || typeof result === 'number' ? result : '';
          }
        }
      };

      this.dataSourceOpeDatosAsistenciasSocialesOrganismos._updateChangeSubscription();
    }
  }
  //

  onSubmit(formDirective: FormGroupDirective): void {
    if (this.formData.valid) {
      const data = this.formData.value;

      // VALIDAR DUPLICADOS
      const yaExiste = OpeDatosAsistenciasValidator.existeTipoDuplicadoEnLista(
        this.dataOpeDatosAsistenciasSocialesOrganismos(),
        (item) => item.opeAsistenciaSocialOrganismoTipo.id,
        data.opeAsistenciaSocialOrganismoTipo.id,
        this.isCreate()
      );

      if (yaExiste) {
        this.snackBar.open('El tipo de asistencia social - organismo ya existe en la lista', '', {
          duration: 3000,
          horizontalPosition: 'center',
          verticalPosition: 'bottom',
          panelClass: ['snackbar-rojo'],
        });
        return;
      }
      // FIN VALIDAR DUPLICADOS

      if (this.isCreate() == -1) {
        this.dataOpeDatosAsistenciasSocialesOrganismos.set([data, ...this.dataOpeDatosAsistenciasSocialesOrganismos()]);
      } else {
        this.editarItem(this.isCreate());
      }

      this.opeDatosAsistenciasSocialesOrganismosChange.emit(
        this.dataOpeDatosAsistenciasSocialesOrganismos().map((item) => ({
          id: item.id,
          idOpeDatoAsistenciaSocial: item.idOpeDatoAsistenciaSocial,
          idOpeAsistenciaSocialOrganismoTipo: item.opeAsistenciaSocialOrganismoTipo.id,
          opeAsistenciaSocialOrganismoTipo: item.opeAsistenciaSocialOrganismoTipo,
          numero: item.numero,
          observaciones: item.observaciones,
        }))
      );

      formDirective.resetForm();
      this.formData.reset({
        opeAsistenciaSocialOrganismoTipo: '',
      });
      this.filaSeleccionadaIndex = null;
    } else {
      this.formData.markAllAsTouched();
    }
  }

  seleccionarItem(index: number) {
    this.isCreate.set(index);

    const opeAsistenciaSocialOrganismoTipoSelected = () =>
      this.opeAsistenciasSocialesOrganismosTipos().find(
        (opeAsistenciaSocialOrganismoTipo) =>
          opeAsistenciaSocialOrganismoTipo.id === Number(this.dataOpeDatosAsistenciasSocialesOrganismos()[index].opeAsistenciaSocialOrganismoTipo.id)
      );

    this.formData.patchValue({
      ...this.dataOpeDatosAsistenciasSocialesOrganismos()[index],
      opeAsistenciaSocialOrganismoTipo: opeAsistenciaSocialOrganismoTipoSelected(),
    });

    //this.formData.patchValue(this.dataOtherInformation()[index]);

    //
    this.filaSeleccionadaIndex = index;
    //
  }

  editarItem(index: number) {
    const dataEditada = this.formData.value;
    this.dataOpeDatosAsistenciasSocialesOrganismos.update((data) => {
      data[index] = { ...data[index], ...dataEditada };
      return [...data];
    });
    this.isCreate.set(-1);
    this.formData.reset();
  }

  eliminarItem(index: number) {
    this.dataOpeDatosAsistenciasSocialesOrganismos.update((data) => {
      data.splice(index, 1);
      return [...data];
    });
    this.opeDatosAsistenciasSocialesOrganismosChange.emit(
      this.dataOpeDatosAsistenciasSocialesOrganismos().map((item) => ({
        id: item.id,
        idOpeDatoAsistenciaSocial: item.idOpeDatoAsistenciaSocial,
        idOpeAsistenciaSocialOrganismoTipo: item.opeAsistenciaSocialOrganismoTipo.id,
        opeAsistenciaSocialOrganismoTipo: item.opeAsistenciaSocialOrganismoTipo,
        numero: item.numero,
        observaciones: item.observaciones,
      }))
    );
  }

  // Para el autocompletar de los tipos de asistencias sociales organismos
  getForm(atributo: string): any {
    return this.formData.controls[atributo];
  }

  filtroOpeAsistenciaSocialOrganismoTipo = signal('');

  // Para input con objeto como valor
  displayFnOpeAsistenciaSocialOrganismoTipo = (option: { id: number; nombre: string } | null): string => {
    return option ? option.nombre : '';
  };

  // Computado para filtrar lista corta
  opeAsistenciasSocialesOrganismosTiposFiltrados = computed(() => {
    return this.utilsService.filtrarPorTexto(this.opeAsistenciasSocialesOrganismosTipos(), this.filtroOpeAsistenciaSocialOrganismoTipo(), false);
  });

  // Fin autocompletar
}
