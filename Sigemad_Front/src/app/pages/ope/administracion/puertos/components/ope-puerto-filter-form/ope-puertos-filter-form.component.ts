import { CommonModule } from '@angular/common';
import { Component, EventEmitter, inject, Input, OnInit, Output, signal, SimpleChanges } from '@angular/core';
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
import { OpePuerto } from '@type/ope/administracion/ope-puerto.type';
import { FormFieldComponent } from '@shared/Inputs/field.component';
import moment from 'moment';
import { OpePuertosService } from '@services/ope/administracion/ope-puertos.service';
import { LocalFiltrosOpePuertos } from '@services/ope/administracion/local-filtro-ope-puertos.service';
import { OpePuertoCreateEdit } from '../ope-puerto-create-edit-form/ope-puerto-create-edit-form.component';
import { FORMATO_FECHA } from '@type/date-formats';
import { MomentDateAdapter } from '@angular/material-moment-adapter';
import { COUNTRIES_ID } from '@type/constants';
import { OpeFase } from '@type/ope/administracion/ope-fase.type';
import { OpeFasesService } from '@services/ope/administracion/ope-fases.service';
import { Countries } from '@type/country.type';
import { MatSnackBar } from '@angular/material/snack-bar';
import { OpePaisesService } from '@services/ope/administracion/ope-paises.service';
import { OpePais } from '@type/ope/administracion/ope-pais.type';
import saveAs from 'file-saver';
import { MatCheckboxModule } from '@angular/material/checkbox';
import { OpeUtilsService } from '@shared/services/ope/ope-utils.service';
import { IDS_TABLAS_LOGS_OPE } from '@type/ope/ope-constants';

@Component({
  selector: 'app-ope-puerto-filter-form',
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
  templateUrl: './ope-puertos-filter-form.component.html',
  styleUrl: './ope-puertos-filter-form.component.scss',
})
export class OpePuertoFilterFormComponent implements OnInit {
  @Input() currentPage = 1;
  @Input() pageSize = 5;

  @Input() opePuertos: ApiResponse<OpePuerto[]> | undefined;
  @Input() filtros: any;
  @Input() isLoading: boolean = true;
  @Input() refreshFilterForm: boolean = true;
  @Output() opePuertosChange = new EventEmitter<ApiResponse<OpePuerto[]>>();
  @Output() isLoadingChange = new EventEmitter<boolean>();
  @Output() refreshFilterFormChange = new EventEmitter<boolean>();

  //
  @Input() opePuertosPaises: OpePais[] = [];
  //

  public filtrosOpePuertosService = inject(LocalFiltrosOpePuertos);
  public opePuertosService = inject(OpePuertosService);
  private opeFasesService = inject(OpeFasesService);
  //private countryService = inject(CountryService);
  private opePaisesService = inject(OpePaisesService);
  //private opeUtilsService = inject(OpeUtilsService);

  private snackBar = inject(MatSnackBar);

  public menuItemActiveService = inject(MenuItemActiveService);
  private dialog = inject(MatDialog);
  public formData!: FormGroup;

  myForm!: FormGroup;

  showFilters = false;

  public showDateEnd = signal<boolean>(true);

  public filteredCountriesOpePuertos = signal<Countries[]>([]);
  //public territories = signal<Territory[]>([]);
  public listaPaisesExtranjerosOpePuertos = signal<Countries[]>([]);
  public listaPaisesNacionalesOpePuertos = signal<Countries[]>([]);
  public opeFases = signal<OpeFase[]>([]);

  // LOGS
  public opeUtilsService = inject(OpeUtilsService);
  public ID_TABLA_LOGS = IDS_TABLAS_LOGS_OPE.PUERTOS;
  // FIN LOGS

  async ngOnInit() {
    const fb = new FormBuilder();
    this.myForm = fb.group({
      selectField: ['', Validators.required],
      inputField1: ['', Validators.required],
      inputField2: ['', Validators.required],
    });

    const { nombre, country, opeFase, operativo } = this.filtros();

    this.formData = new FormGroup({
      nombre: new FormControl(nombre ?? ''),
      country: new FormControl(country ?? ''),
      opeFase: new FormControl(opeFase ?? ''),
      operativo: new FormControl(operativo ?? true),
    });

    //this.clearFormFilter();
    this.menuItemActiveService.set.emit('/ope-puertos');

    //const comparativeDates = await this.comparativeDateService.get();
    //this.comparativeDates.set(comparativeDates);

    const opeFases = await this.opeFasesService.get();
    this.opeFases.set(opeFases);

    /*
    const countriesExtranjeros = await this.countryService.getExtranjeros();
    //this.listaPaisesExtranjeros.set(countriesExtranjeros);

    //
    const paisesExtranjerosOPE = this.opeUtilsService.getPaisesExtranjerosOPE();
    this.listaPaisesExtranjeros.set(countriesExtranjeros.filter((pais) => paisesExtranjerosOPE.some((filtro) => filtro.id === pais.id)));
    //
    const countriesNacionales = await this.countryService.getNacionales();
    this.listaPaisesNacionales.set(countriesNacionales);

    //this.filteredCountries.set(countriesNacionales);
    //
    */

    //
    /*
    if (this.opePuertosPaises != null && this.opePuertosPaises.length > 0) {
      const countriesExtranjerosOpePuertos = this.opePuertosPaises.filter((p) => p.extranjero).map((p) => p.pais);
      this.listaPaisesExtranjerosOpePuertos.set(countriesExtranjerosOpePuertos);
      const countriesNacionalesOpePuertos = this.opePuertosPaises.filter((p) => !p.extranjero).map((p) => p.pais);
      this.listaPaisesNacionalesOpePuertos.set(countriesNacionalesOpePuertos);
    }
      */
    //

    /*
    const countryValue = this.formData.get('country')?.value;
    if (countryValue && countryValue == COUNTRIES_ID.SPAIN) {
      this.filteredCountries.set(countriesNacionales);
    } else {
      this.filteredCountries.set(countriesExtranjeros);
    }
      */
    //this.filteredCountriesOpePuertos.set([...this.listaPaisesNacionalesOpePuertos(), ...this.listaPaisesExtranjerosOpePuertos()]);
    //

    //const territories = await this.territoryService.get();
    //this.territories.set(territories);

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

    this.opePuertosChange.emit({
      count: 0,
      page: this.currentPage,
      pageSize: this.pageSize,
      data: [],
      pageCount: 0,
    });
    this.isLoading = true;
    this.isLoadingChange.emit(true);

    const { nombre, country, opeFase, operativo } = this.formData.value;

    const opePuertos = await this.opePuertosService.get(
      {
        nombre: nombre,
        IdPais: country,
        IdOpeFase: opeFase,
        FechaValidezDesde: operativo ? moment().format('YYYY-MM-DD') : null, // Si operativo es true, se filtra desde hoy
        FechaValidezHasta: operativo ? moment().format('YYYY-MM-DD') : null, // Si operativo es true, se filtra hasta hoy
      }
      //this.currentPage,
      //this.pageSize
    );

    // Verificar que la respuesta tenga el campo `count`
    if (!opePuertos || typeof opePuertos.count !== 'number') {
      console.error('La respuesta del backend no contiene el campo `count`:', opePuertos);
      throw new Error('Respuesta inválida del backend');
    }

    this.filtrosOpePuertosService.setFilters(this.formData.value);
    this.opePuertos = opePuertos;
    this.opePuertosChange.emit(this.opePuertos);
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
    const dialogRef = this.dialog.open(OpePuertoCreateEdit, {
      width: '65vw',
      maxWidth: 'none',
      disableClose: true,
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

  /*
  async changeTerritory(event: any) {
    this.formData.patchValue({
      country: event.value == 1 ? COUNTRIES_ID.SPAIN : '',
    });
    if (event.value == 1) {
      this.filteredCountriesOpePuertos.set(this.listaPaisesNacionalesOpePuertos());
      this.formData.get('country')?.enable();
    } else if (event.value == 2) {
      this.filteredCountriesOpePuertos.set(this.listaPaisesExtranjerosOpePuertos());
      this.formData.get('country')?.enable();
    } else if (event.value == 3) {
      this.filteredCountriesOpePuertos.set([]);
      this.formData.get('country')?.disable();
    } else {
      this.filteredCountriesOpePuertos.set([]);
      this.formData.get('country')?.disable();
    }
  }
  */

  exportToExcel(): void {
    this.isLoading = true;
    this.isLoadingChange.emit(true);

    const v = this.formData.value;
    const filtros = {
      estadoEliminado: 'todos' as 'activo' | 'eliminado' | 'todos', // o calcula aquí 'activo'/'eliminado' si lo necesitas
      nombre: v.nombre || undefined,
      idPais: v.country || undefined,
      idOpeFase: v.opeFase || undefined,
    };

    this.opePuertosService.exportarExcel(filtros).subscribe({
      next: (blob) => {
        const fileName = `OpePuertos_${moment().format('DD-MM-YYYY_HH-mm')}.xlsx`;
        saveAs(blob, fileName);
        this.isLoading = false;
        this.isLoadingChange.emit(false);
      },
      error: (err) => {
        console.error('Error al exportar a Excel', err);
        this.snackBar.open('Error al exportar a Excel', '', {
          duration: 3000,
          panelClass: ['snackbar-rojo'],
        });
        this.isLoading = false;
        this.isLoadingChange.emit(false);
      },
    });
  }
}
