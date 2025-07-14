import { CommonModule } from '@angular/common';
import { Component, computed, EventEmitter, inject, Input, OnInit, Output, signal, SimpleChanges } from '@angular/core';
import { DateAdapter, MAT_DATE_FORMATS, NativeDateAdapter } from '@angular/material/core';
import { FlexLayoutModule } from '@angular/flex-layout';
import { FormBuilder, FormControl, FormGroup, FormsModule, ReactiveFormsModule, Validators } from '@angular/forms';
import { MatAutocompleteModule } from '@angular/material/autocomplete';
import { MatButtonModule } from '@angular/material/button';
import { MatDatepickerModule } from '@angular/material/datepicker';
import { MatExpansionModule, MatExpansionPanel } from '@angular/material/expansion';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatIconModule } from '@angular/material/icon';
import { MatInputModule } from '@angular/material/input';
import { MatSelectModule } from '@angular/material/select';
import { MatDialog, MatDialogModule } from '@angular/material/dialog';
import { MenuItemActiveService } from '@services/menu-item-active.service';
import { ApiResponse } from '@type/api-response.type';
import { OpeLineaMaritima } from '@type/ope/administracion/ope-linea-maritima.type';
import { FormFieldComponent } from '@shared/Inputs/field.component';
import moment from 'moment';
import { OpeLineasMaritimasService } from '@services/ope/administracion/ope-lineas-maritimas.service';
import { LocalFiltrosOpeLineasMaritimas } from '@services/ope/administracion/local-filtro-ope-lineas-maritimas.service';
import { OpeLineaMaritimaCreateEdit } from '../ope-linea-maritima-create-edit-form/ope-linea-maritima-create-edit-form.component';
import { FORMATO_FECHA } from '@type/date-formats';
import { MomentDateAdapter } from '@angular/material-moment-adapter';
import { OpeFase } from '@type/ope/administracion/ope-fase.type';
import { OpeFasesService } from '@services/ope/administracion/ope-fases.service';
import { Countries } from '@type/country.type';
import { MatSnackBar } from '@angular/material/snack-bar';
import { OpePaisesService } from '@services/ope/administracion/ope-paises.service';
import { OpePais } from '@type/ope/administracion/ope-pais.type';
import saveAs from 'file-saver';
import { OpePuerto } from '@type/ope/administracion/ope-puerto.type';
import { OpeValidator } from '@shared/validators/ope/ope-validator';
import { OpePuertosService } from '@services/ope/administracion/ope-puertos.service';
import { UtilsService } from '@shared/services/utils.service';
import { OpeUtilsService } from '@shared/services/ope/ope-utils.service';
import { IDS_TABLAS_LOGS_OPE } from '@type/ope/ope-constants';
import { MatCheckboxModule } from '@angular/material/checkbox';
import { OpeLineasMaritimasValidator } from '@shared/validators/ope/administracion/ope-lineas-maritimas-validator';

@Component({
  selector: 'app-ope-linea-maritima-filter-form',
  standalone: true,
  imports: [
    CommonModule,
    ReactiveFormsModule,
    FormsModule,
    FormFieldComponent,
    MatFormFieldModule,
    MatInputModule,
    MatSelectModule,
    MatButtonModule,
    MatAutocompleteModule,
    MatIconModule,
    FlexLayoutModule,
    MatExpansionModule,
    MatDatepickerModule,
    MatDialogModule,
    MatCheckboxModule,
  ],
  providers: [
    { provide: DateAdapter, useClass: MomentDateAdapter },
    { provide: MAT_DATE_FORMATS, useValue: FORMATO_FECHA },
  ],
  templateUrl: './ope-lineas-maritimas-filter-form.component.html',
  styleUrl: './ope-lineas-maritimas-filter-form.component.scss',
})
export class OpeLineaMaritimaFilterFormComponent implements OnInit {
  @Input() currentPage = 1;
  @Input() pageSize = 5;

  @Input() opeLineasMaritimas: ApiResponse<OpeLineaMaritima[]> | undefined;
  @Input() filtros: any;
  @Input() isLoading: boolean = true;
  @Input() refreshFilterForm: boolean = true;
  @Output() opeLineasMaritimasChange = new EventEmitter<ApiResponse<OpeLineaMaritima[]>>();
  @Output() isLoadingChange = new EventEmitter<boolean>();
  @Output() refreshFilterFormChange = new EventEmitter<boolean>();

  //@Input() opeLineasMaritimasPaises: OpePais[] = [];

  //
  @Input() opePuertosPaises: OpePais[] = [];
  //

  public filtrosOpeLineasMaritimasService = inject(LocalFiltrosOpeLineasMaritimas);
  public opeLineasMaritimasService = inject(OpeLineasMaritimasService);
  private opePuertosService = inject(OpePuertosService);
  private opeFasesService = inject(OpeFasesService);
  private opePaisesService = inject(OpePaisesService);

  private snackBar = inject(MatSnackBar);

  public menuItemActiveService = inject(MenuItemActiveService);
  private dialog = inject(MatDialog);
  public formData!: FormGroup;

  myForm!: FormGroup;

  showFilters = false;

  public showDateEnd = signal<boolean>(true);

  public opePuertosOrigen = signal<OpePuerto[]>([]);
  public opePuertosDestino = signal<OpePuerto[]>([]);

  public filteredCountriesOpePuertos = signal<Countries[]>([]);
  public listaPaisesExtranjerosOpePuertos = signal<Countries[]>([]);
  public listaPaisesNacionalesOpePuertos = signal<Countries[]>([]);
  public opeFases = signal<OpeFase[]>([]);

  //
  //public opePuertosPaises: OpePais[] = [];
  //

  public utilsService = inject(UtilsService);

  // LOGS
  public opeUtilsService = inject(OpeUtilsService);
  public ID_TABLA_LOGS = IDS_TABLAS_LOGS_OPE.LINEASMARITIMAS;
  // FIN LOGS

  async ngOnInit() {
    const fb = new FormBuilder();
    this.myForm = fb.group({
      selectField: ['', Validators.required],
      inputField1: ['', Validators.required],
      inputField2: ['', Validators.required],
    });

    const { nombre, opePuertoOrigen, opePuertoDestino, countryOrigen, countryDestino, opeFase, operativo } = this.filtros();

    this.formData = new FormGroup(
      {
        nombre: new FormControl(nombre ?? ''),
        opePuertoOrigen: new FormControl(opePuertoOrigen ?? '', [OpeValidator.opcionValidaDeSelectPorId(() => this.opePuertosOrigenFiltrados())]),
        opePuertoDestino: new FormControl(opePuertoDestino ?? '', [OpeValidator.opcionValidaDeSelectPorId(() => this.opePuertosOrigenFiltrados())]),
        countryOrigen: new FormControl(countryOrigen ?? ''),
        countryDestino: new FormControl(countryDestino ?? ''),
        opeFase: new FormControl(opeFase ?? ''),
        operativo: new FormControl(operativo ?? true),
      },
      {
        validators: [OpeLineasMaritimasValidator.opePuertosOrigenDestinoDiferentesValidator('opePuertoOrigen', 'opePuertoDestino')],
      }
    );

    //this.clearFormFilter();
    this.menuItemActiveService.set.emit('/ope-lineas-maritimas');

    //const opePuertos = await this.opePuertosService.getAll();
    const { fechaValidezDesde, fechaValidezHasta } = this.formData.value;
    const opePuertos = await this.opePuertosService.get({
      fechaValidezDesde: fechaValidezDesde ? moment(fechaValidezDesde).format('YYYY-MM-DD') : null,
      fechaValidezHasta: fechaValidezHasta ? moment(fechaValidezHasta).format('YYYY-MM-DD') : null,
    });

    this.opePuertosOrigen.set(opePuertos.data);
    this.opePuertosDestino.set(opePuertos.data);

    //
    //await this.cargarOpePuertosPaises();
    //
    const opeFases = await this.opeFasesService.get();
    this.opeFases.set(opeFases);

    // Para el autocompletar de los puertos
    this.getForm('opePuertoOrigen')?.valueChanges.subscribe((value: string) => {
      if (typeof value === 'string') {
        this.filtroOpePuertoOrigen.set(value);
      }
    });
    this.getForm('opePuertoDestino')?.valueChanges.subscribe((value: string) => {
      if (typeof value === 'string') {
        this.filtroOpePuertoDestino.set(value);
      }
    });
    // FIN autocompletar

    this.onSubmit();
  }

  ngOnChanges(changes: SimpleChanges): void {
    if (changes['opePuertosPaises'] && this.opePuertosPaises?.length > 0) {
      const countriesExtranjerosOpePuertos = this.opePuertosPaises.filter((p) => p.extranjero).map((p) => p.pais);
      this.listaPaisesExtranjerosOpePuertos.set(countriesExtranjerosOpePuertos);
      const countriesNacionalesOpePuertos = this.opePuertosPaises.filter((p) => !p.extranjero).map((p) => p.pais);
      this.listaPaisesNacionalesOpePuertos.set(countriesNacionalesOpePuertos);
      this.filteredCountriesOpePuertos.set([...this.listaPaisesNacionalesOpePuertos(), ...this.listaPaisesExtranjerosOpePuertos()]);
    }

    if ('refreshFilterForm' in changes) {
      this.onSubmit();
    }
  }

  /*
  toggleAccordion(panel: MatExpansionPanel) {
    panel.toggle();
  }
  */

  async onSubmit() {
    if (!this.formData) {
      return;
    }

    if (!this.formData.valid) {
      return;
    }

    this.opeLineasMaritimasChange.emit({
      count: 0,
      page: this.currentPage,
      pageSize: this.pageSize,
      data: [],
      pageCount: 0,
    });
    this.isLoading = true;
    this.isLoadingChange.emit(true);

    const { nombre, opePuertoOrigen, opePuertoDestino, countryOrigen, countryDestino, opeFase, operativo } = this.formData.value;

    const opeLineasMaritimas = await this.opeLineasMaritimasService.get(
      {
        nombre: nombre,
        IdOpePuertoOrigen: opePuertoOrigen,
        IdOpePuertoDestino: opePuertoDestino,
        IdPaisOrigen: countryOrigen,
        IdPaisDestino: countryDestino,
        IdOpeFase: opeFase,
        FechaValidezDesde: operativo ? moment().format('YYYY-MM-DD') : null, // Si operativo es true, se filtra desde hoy
        FechaValidezHasta: operativo ? moment().format('YYYY-MM-DD') : null, // Si operativo es true, se filtra hasta hoy
      }
      //this.currentPage,
      //this.pageSize
    );

    // Verificar que la respuesta tenga el campo `count`
    if (!opeLineasMaritimas || typeof opeLineasMaritimas.count !== 'number') {
      console.error('La respuesta del backend no contiene el campo `count`:', opeLineasMaritimas);
      throw new Error('Respuesta inválida del backend');
    }

    this.filtrosOpeLineasMaritimasService.setFilters(this.formData.value);
    this.opeLineasMaritimas = opeLineasMaritimas;
    this.opeLineasMaritimasChange.emit(this.opeLineasMaritimas);
    this.isLoadingChange.emit(false);
    this.isLoading = false;
  }

  clearFormFilter() {
    this.formData.reset();
    this.formData.patchValue({
      nombre: '',
      country: '',
      opeFase: '',
    });
  }

  getForm(atributo: string): any {
    return this.formData.controls[atributo];
  }

  goModal() {
    const dialogRef = this.dialog.open(OpeLineaMaritimaCreateEdit, {
      width: '65vw',
      maxWidth: 'none',
      disableClose: true,
      autoFocus: false,
      data: {
        title: 'Nuevo - Datos Puerto',
        fire: {},
      },
    });

    dialogRef.afterClosed().subscribe((result) => {
      if (result) {
        console.log('Modal result:', result);
        this.snackBar.open('Datos ingresados correctamente!', '', {
          duration: 3000,
          horizontalPosition: 'center',
          verticalPosition: 'bottom',
          panelClass: ['snackbar-verde'],
        });
        this.onSubmit();
      }
    });
  }

  exportToExcel(): void {
    this.isLoading = true;
    this.isLoadingChange.emit(true);

    const v = this.formData.value;
    const filtros = {
      nombre: v.nombre || undefined,
      idPuertoOrigen: v.opePuertoOrigen || undefined,
      idPuertoDestino: v.opePuertoDestino || undefined,
      idPaisOrigen: v.countryOrigen || undefined,
      idPaisDestino: v.countryDestino || undefined,
      idOpeFase: v.opeFase || undefined,
    };

    this.opeLineasMaritimasService.exportarExcel(filtros).subscribe({
      next: (blob) => {
        const fileName = `OpeLineasMaritimas_${moment().format('DD-MM-YYYY_HH-mm')}.xlsx`;
        saveAs(blob, fileName);
        this.isLoading = false;
        this.isLoadingChange.emit(false);
      },
      error: (err) => {
        console.error('Error al exportar a Excel', err);
        this.isLoading = false;
        this.isLoadingChange.emit(false);
      },
    });
  }

  /*
  async cargarOpePuertosPaises() {
    const opePuertosPaises = await this.opePaisesService.get({
      opePuertos: true,
    });
    this.opePuertosPaises = opePuertosPaises;

    //
    const countriesExtranjerosOpePuertos = this.opePuertosPaises.filter((p) => p.extranjero).map((p) => p.pais);
    this.listaPaisesExtranjerosOpePuertos.set(countriesExtranjerosOpePuertos);
    const countriesNacionalesOpePuertos = this.opePuertosPaises.filter((p) => !p.extranjero).map((p) => p.pais);
    this.listaPaisesNacionalesOpePuertos.set(countriesNacionalesOpePuertos);
    this.filteredCountriesOpePuertos.set([...this.listaPaisesNacionalesOpePuertos(), ...this.listaPaisesExtranjerosOpePuertos()]);
    //
  }
  */

  // Para el autocompletar de los puertos
  filtroOpePuertoOrigen = signal('');
  filtroOpePuertoDestino = signal('');

  // Para input con objeto como valor
  displayFnOrigen = (id: number | null): string => {
    if (id == null) return '';
    const match = this.opePuertosOrigen().find((item) => item.id === id);
    return match ? match.nombre : '';
  };
  displayFnDestino = (id: number | null): string => {
    if (id == null) return '';
    const match = this.opePuertosDestino().find((item) => item.id === id);
    return match ? match.nombre : '';
  };

  // Computado para filtrar lista corta
  opePuertosOrigenFiltrados = computed(() => {
    return this.utilsService.filtrarPorTexto(this.opePuertosOrigen(), this.filtroOpePuertoOrigen(), false);
  });
  opePuertosDestinoFiltrados = computed(() => {
    return this.utilsService.filtrarPorTexto(this.opePuertosDestino(), this.filtroOpePuertoDestino(), false);
  });
  // Fin autocompletar
}
