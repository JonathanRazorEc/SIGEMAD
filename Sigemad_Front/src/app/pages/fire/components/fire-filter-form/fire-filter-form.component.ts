import { CommonModule } from '@angular/common';
import { ChangeDetectorRef, Component, EventEmitter, inject, Input, NgZone, OnInit, Output, signal, SimpleChanges } from '@angular/core';

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
import moment from 'moment';
import { AutonomousCommunityService } from '../../../../services/autonomous-community.service';
import { ComparativeDateService } from '../../../../services/comparative-date.service';
import { CountryService } from '../../../../services/country.service';
import { EventStatusService } from '../../../../services/eventStatus.service';
import { FireService } from '../../../../services/fire.service';
import { LocalFiltrosIncendio } from '../../../../services/local-filtro-incendio.service';
import { MenuItemActiveService } from '../../../../services/menu-item-active.service';
import { MoveService } from '../../../../services/move.service';
import { MunicipalityService } from '../../../../services/municipality.service';
import { ProvinceService } from '../../../../services/province.service';
import { SeverityLevelService } from '../../../../services/severity-level.service';
import { SuperficiesService } from '../../../../services/superficies.service';
import { TerritoryService } from '../../../../services/territory.service';
import { FormFieldComponent } from '../../../../shared/Inputs/field.component';
import { ApiResponse } from '../../../../types/api-response.type';
import { AutonomousCommunity } from '../../../../types/autonomous-community.type';
import { ComparativeDate } from '../../../../types/comparative-date.type';
import { Countries } from '../../../../types/country.type';
import { EventStatus } from '../../../../types/eventStatus.type';
import { FireStatus } from '../../../../types/fire-status.type';
import { Fire } from '../../../../types/fire.type';
import { Move } from '../../../../types/move.type';
import { Municipality } from '../../../../types/municipality.type';
import { Province } from '../../../../types/province.type';
import { SeverityLevel } from '../../../../types/severity-level.type';
import { Territory } from '../../../../types/territory.type';
import { FireCreateEdit } from '../../../fire/components/fire-create-edit-form/fire-create-edit-form.component';
import { MasterDataEvolutionsService } from '../../../../services/master-data-evolutions.service';
import { SituationsEquivalent } from '../../../../types/situations-equivalent.type';
import { EventService } from '../../../../services/event.service';
import { Event } from '../../../../types/event.type';
import { MomentDateAdapter } from '@angular/material-moment-adapter';
import { FORMATO_FECHA } from '../../../../types/date-formats';
import { COUNTRIES_ID } from '@type/constants';
import { MatSnackBar, MatSnackBarModule } from '@angular/material/snack-bar';
import { catchError, debounceTime, filter, map, merge, Observable, of, startWith, switchMap } from 'rxjs';

// Constantes para los valores de territorio
const TERRITORY_NACIONAL = 1;
const TERRITORY_EXTRANJERO = 2;
const TERRITORY_TRANSFRONTERIZO = 3;

@Component({
  selector: 'app-fire-filter-form',
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
    MatSnackBarModule
  ],
  providers: [
    { provide: DateAdapter, useClass: MomentDateAdapter },
    { provide: MAT_DATE_FORMATS, useValue: FORMATO_FECHA },
  ],
  templateUrl: './fire-filter-form.component.html',
  styleUrl: './fire-filter-form.component.scss',
})
export class FireFilterFormComponent implements OnInit {
  // PCD

  // FIN PCD

  @Input() currentPage = 1
  @Input() pageSize = 5
  @Input() fires: ApiResponse<Fire[]> | undefined;
  @Input() filtros: any;
  @Input() isLoading: boolean = true;
  @Input() refreshFilterForm: boolean = true;
  @Output() firesChange = new EventEmitter<ApiResponse<Fire[]>>();
  @Output() isLoadingChange = new EventEmitter<boolean>();
  @Output() refreshFilterFormChange = new EventEmitter<boolean>();

  public filtrosIncendioService = inject(LocalFiltrosIncendio);

  public superficiesService = inject(SuperficiesService);
  public menuItemActiveService = inject(MenuItemActiveService);
  public territoryService = inject(TerritoryService);
  public autonomousCommunityService = inject(AutonomousCommunityService);
  public provinceService = inject(ProvinceService);
  public countryService = inject(CountryService);
  public eventStatusService = inject(EventStatusService);
  public eventTypeService = inject(EventService);
  public municipalityService = inject(MunicipalityService);
  public severityLevelService = inject(SeverityLevelService);
  public fireService = inject(FireService);
  public comparativeDateService = inject(ComparativeDateService);
  public moveService = inject(MoveService);
  private dialog = inject(MatDialog);
  public masterData = inject(MasterDataEvolutionsService);
  private snackBar = inject(MatSnackBar);

  public superficiesFiltro = signal<any[]>([]);
  public territories = signal<any[]>([]);
  public autonomousCommunities = signal<AutonomousCommunity[]>([]);
  public provinces = signal<Province[]>([]);
  //public countries = signal<Countries[]>([]);
  public listaPaisesExtranjeros = signal<Countries[]>([]);
  public listaPaisesNacionales = signal<Countries[]>([]);

  public eventStatus = signal<EventStatus[]>([]);
  public municipalities = signal<Municipality[]>([]);
  public fireStatus = signal<FireStatus[]>([]);
  public situationsEquivalent = signal<SituationsEquivalent[]>([]);

  public eventTypes = signal<Event[]>([]);

  public showDateEnd = signal<boolean>(true);

  public moves = signal<Move[]>([]);
  public comparativeDates = signal<ComparativeDate[]>([]);

  public filteredCountries = signal<Countries[]>([]);
  public formData!: FormGroup;

  ProvincesfilteredOptions: Observable<Province[]> = of<Province[]>([]);
  autonomousCommunityfilteredOptions: Observable<AutonomousCommunity[]> = of<AutonomousCommunity[]>([]);
  private ngZone = inject(NgZone);
  private cdr = inject(ChangeDetectorRef);

  myForm!: FormGroup;

  showFilters = false;

  isForeignSelected = false;

  async ngOnInit() {
    const countriesExtranjeros = await this.countryService.getExtranjeros();
    const countriesNacionales = await this.countryService.getNacionales();

    this.listaPaisesExtranjeros.set(countriesExtranjeros);
    this.listaPaisesNacionales.set(countriesNacionales);

    await this.loadForm(); // ✅ Ahora se ejecuta con países ya disponibles

    // Resto de datos auxiliares
    this.menuItemActiveService.set.emit('/fire');

    // Función reutilizable para validar arrays
    const safeArray = <T>(data: any): T[] => Array.isArray(data) ? data : [];

    const superficies = await this.superficiesService.getSuperficiesFiltro();
    this.superficiesFiltro.set(safeArray(superficies));

    const territorios = await this.territoryService.get();
    this.territories.set(safeArray(territorios));

    const fireStatus = await this.masterData.getFireStatus();
    this.fireStatus.set(safeArray(fireStatus));

    const situations = await this.masterData.getSituationEquivalent();
    this.situationsEquivalent.set(safeArray(situations));

    const eventStatus = await this.eventStatusService.get();
    this.eventStatus.set(safeArray(eventStatus));

    const moves = await this.moveService.get();
    this.moves.set(safeArray(moves));

    const dates = await this.comparativeDateService.get();
    this.comparativeDates.set(safeArray(dates));

    const types = await this.eventTypeService.get();
    this.eventTypes.set(safeArray(types));


    await this.changeTerritory({ value: this.formData.get('territory')?.value });

    this.initializeSearchProvince();
    this.initializeSearchAutonomousCommunity();
  }

  async loadForm(): Promise<void> {
    const savedFilters = localStorage.getItem('fire-filters');
    let initialFilters = this.filtros();

    if (savedFilters) {
      try {
        const parsed = JSON.parse(savedFilters);
        parsed.country = parsed.country !== '' ? Number(parsed.country) : COUNTRIES_ID.SPAIN;
        initialFilters = parsed;
      } catch (e) {
        console.warn('No se pudo parsear filtros guardados', e);
      }
    }

    const countriesNacionales = this.listaPaisesNacionales();
    const countriesExtranjeros = this.listaPaisesExtranjeros();

    const allCountries = [...countriesNacionales, ...countriesExtranjeros];
    const matchedCountry = allCountries.find(p => p.id === initialFilters.country);
    const esExtranjero = matchedCountry ? countriesExtranjeros.some(p => p.id === matchedCountry.id) : false;
    const listaCorrecta = esExtranjero ? countriesExtranjeros : countriesNacionales;
    this.filteredCountries.set(listaCorrecta);

    if (!matchedCountry) {
      initialFilters.country = COUNTRIES_ID.SPAIN;
    }

    // Crear el formulario
    this.formData = new FormGroup({
      name: new FormControl(initialFilters.name ?? ''),
      territory: new FormControl(initialFilters.territory ?? 1),
      autonomousCommunity: new FormControl(initialFilters.autonomousCommunity ?? ''),
      province: new FormControl(initialFilters.province ?? ''),
      country: new FormControl(initialFilters.country ?? COUNTRIES_ID.SPAIN),
      municipality: new FormControl(initialFilters.municipality ?? ''),
      fireStatus: new FormControl(initialFilters.fireStatus ?? ''),
      episode: new FormControl(initialFilters.episode ?? ''),
      situationEquivalent: new FormControl(initialFilters.situationEquivalent ?? ''),
      affectedArea: new FormControl(initialFilters.affectedArea ?? ''),
      move: new FormControl(initialFilters.move ?? 4),
      between: new FormControl(initialFilters.between ?? 1),
      eventStatus: new FormControl(initialFilters.eventStatus ?? ''),
      CCAA: new FormControl(initialFilters.CCAA ?? ''),
      provincia: new FormControl(initialFilters.provincia ?? ''),
      fechaInicio: new FormControl(initialFilters.fechaInicio ? moment(initialFilters.fechaInicio).toDate() : moment().subtract(4, 'days').toDate()),
      fechaFin: new FormControl(initialFilters.fechaFin ? moment(initialFilters.fechaFin).toDate() : moment().toDate()),
      eventTypes: new FormControl(initialFilters.eventTypes ?? 1),
    });

    // Después de crear el form
    this.ngZone.runOutsideAngular(() => {
      requestAnimationFrame(() => {
        this.ngZone.run(() => {
          const current = this.formData.get('country')?.value;
          this.formData.get('country')?.setValue(current);
          this.cdr.detectChanges(); // fuerza redibujado si fuera necesario
        });
      });
    });


    await this.loadCommunities(initialFilters.country);
  }


  initializeSearchProvince() {
    this.provinces = signal<Province[]>([]);

    this.ProvincesfilteredOptions = merge(
      this.formData.get('provincia')?.valueChanges ?? of<string>(''),
      this.formData.get('CCAA')?.valueChanges ?? of<any>(null)
    ).pipe(
      startWith(''),
      debounceTime(500),
      switchMap(async () => {
        const provinciaValue = this.formData.get('provincia')?.value;
        const idCCAA = this.formData.get('CCAA')?.value?.id;

        if (typeof provinciaValue === 'string' && provinciaValue.trim().length >= 3 || !provinciaValue) {
          const provinces = await this.provinceService.getBySearch(provinciaValue, idCCAA);
          this.provinces.set(provinces);
          return provinces;
        } else if (idCCAA) {
          const provinces = await this.provinceService.getBySearch('', idCCAA);
          this.provinces.set(provinces);
          return provinces;
        } else {
          const provinces = await this.provinceService.getBySearch();
          this.provinces.set(provinces);
          return provinces;
        }
      }),
      catchError(() => of<Province[]>([]))
    );

    // 🔥 CARGA INICIAL: si ya existe provincia y/o CCAA, cargar sus provincias
    const currentCCAA = this.formData.get('CCAA')?.value;
    const currentProvince = this.formData.get('provincia')?.value;

    if (currentProvince && typeof currentProvince === 'object' && currentProvince.comunidadAutonoma?.id) {
      this.provinceService.getBySearch('', currentProvince.comunidadAutonoma.id).then(provinces => {
        this.provinces.set(provinces);
      });
    } else if (currentCCAA && typeof currentCCAA === 'object') {
      this.provinceService.getBySearch('', currentCCAA.id).then(provinces => {
        this.provinces.set(provinces);
      });
    }

    // 🔁 Listeners dinámicos posteriores
    this.formData.get('CCAA')?.valueChanges.subscribe(async selectedCommunity => {
      const selectedProvince = this.formData.get('provincia')?.value;
      if (selectedCommunity && typeof selectedCommunity === 'object') {
        const idCCAA = selectedCommunity.id;
        const provinces = await this.provinceService.getBySearch('', idCCAA);
        this.provinces.set(provinces);

        if (
          selectedProvince &&
          typeof selectedProvince === 'object' &&
          selectedCommunity.id !== selectedProvince.comunidadAutonoma.id
        ) {
          this.formData.get('provincia')?.setValue(null);
        }
      } else if (!selectedCommunity) {
        const provinces = await this.provinceService.getBySearch();
        this.provinces.set(provinces);
      }
    });

    this.formData.get('provincia')?.valueChanges.subscribe(async value => {
      const idCCAA = this.formData.get('CCAA')?.value?.id;
      if (!value) {
        const provinces = await this.provinceService.getBySearch('', idCCAA);
        this.provinces.set(provinces);
      }
    });
  }

  initializeSearchAutonomousCommunity() {
    this.autonomousCommunityfilteredOptions = (this.formData.get('CCAA')?.valueChanges ?? of<string>('')).pipe(
      startWith(''),
      debounceTime(500),
      switchMap(async value => {
        if (typeof value === 'string' && value.trim().length >= 3) {
          return await this.autonomousCommunityService.getBySearch(value);
        } else if (!value) {
          return await this.autonomousCommunityService.getBySearch();
        } else {
          return [];
        }
      }),
      catchError(() => of<AutonomousCommunity[]>([]))
    );

    this.formData.get('provincia')?.valueChanges.subscribe(selectedProvince => {
      if (selectedProvince && typeof selectedProvince === 'object') {
        this.formData.get('CCAA')?.setValue(selectedProvince.comunidadAutonoma);
      }
    });

  }

  getTitle(option: Province | AutonomousCommunity): string {
    return option ? option.descripcion : '';
  }
  ngOnChanges(changes: SimpleChanges): void {
    if ('refreshFilterForm' in changes) {
      this.onSubmit();
    }
  }

  toggleAccordion(panel: MatExpansionPanel) {
    panel.toggle();
  }

  // NO SE USA
  /*
  getCountryByTerritory(country: any, territory: any) {
    if (territory == 1) {
      return country;
    }
    if (territory == 2) {
      if (country == this.COUNTRIES_ID.SPAIN) {
        return null;
      }
    }
  }
  */

  async changeTerritory(event: any) {
    const currentCountry = this.formData.get('country')?.value;

    // Solo reasigna si el país no se ha definido previamente
    if (!currentCountry) {
      this.formData.patchValue({
        country: event.value == TERRITORY_NACIONAL ? COUNTRIES_ID.SPAIN : '',
      });
    }

    // Siempre limpia campos dependientes
    this.formData.patchValue({
      autonomousCommunity: '',
      province: '',
      municipality: '',
    });

    // Borrar país si es transfronterizo o extranjero
    if (event.value == TERRITORY_EXTRANJERO || event.value == TERRITORY_TRANSFRONTERIZO) {
      this.formData.get('country')?.setValue('');
    }

    // Cargar países y comunidades
    this.loadCommunities(currentCountry || (event.value == TERRITORY_NACIONAL ? COUNTRIES_ID.SPAIN : '9999'));

    if (event.value == TERRITORY_NACIONAL) {
      // Nacional: forzar país a España
      this.formData.get('country')?.setValue(COUNTRIES_ID.SPAIN);
      this.filteredCountries.set(this.listaPaisesNacionales());
      this.enableSelect(true);
    } else if (event.value == TERRITORY_EXTRANJERO) {
      this.filteredCountries.set(this.listaPaisesExtranjeros());
      this.formData.get('CCAA')?.disable();
      this.formData.get('provincia')?.disable();
      this.formData.get('country')?.enable();
      this.formData.get('CCAA')?.setValue('');
      this.formData.get('provincia')?.setValue('');
    } else if (event.value == TERRITORY_TRANSFRONTERIZO) {
      this.filteredCountries.set([]);
      this.enableSelect(false);
    } else {
      this.enableSelect(true);
    }

  }

  enableSelect( enable: boolean, ) {
    if (enable) {
      this.formData.get('CCAA')?.enable();
      this.formData.get('provincia')?.enable();
      this.formData.get('country')?.enable();
    } else {
      this.formData.get('CCAA')?.setValue('');
      this.formData.get('provincia')?.setValue('');
      this.formData.get('country')?.setValue('');
      this.formData.get('CCAA')?.disable();
      this.formData.get('provincia')?.disable();
      this.formData.get('country')?.disable();
    }
  }

  async loadCommunities(country?: any): Promise<void> {
    const selectedCountry = country ?? this.formData.get('country')?.value;

    if (!selectedCountry || selectedCountry === '9999') {
      this.autonomousCommunities.set([]);
      return;
    }

    // Determinar si es extranjero o nacional
    const esExtranjero = this.listaPaisesExtranjeros().some(p => p.id === selectedCountry);
    const lista = esExtranjero ? this.listaPaisesExtranjeros() : this.listaPaisesNacionales();

    this.filteredCountries.set(lista); // Asegura que el <mat-select> funcione visualmente

    // Cargar comunidades autónomas desde el servicio
    const comunidades = await this.autonomousCommunityService.getByCountry(selectedCountry);
    this.autonomousCommunities.set(comunidades);
  }


  async loadProvinces(event: any) {
    const ac_id = event.value;
    const provinces = await this.provinceService.get(ac_id);
    this.provinces.set(provinces);
  }

  changeBetween(event: any) {
    this.showDateEnd.set(event.value === 1 || event.value === 5 ? true : false);
  }

  async onSubmit() {
    if (!this.formData) {
      return;
    }

    // Emitir datos vacíos antes de la consulta
    this.firesChange.emit({
      count: 0,
      page: this.currentPage,
      pageSize: this.pageSize,
      data: [],
      pageCount: 0,
    });

    this.isLoading = true;
    this.isLoadingChange.emit(true);

    try {
      const {
        territory,
        country,
        autonomousCommunity,
        province,
        fireStatus,
        eventStatus,
        situationEquivalent,
        affectedArea,
        move,
        between,
        fechaInicio,
        fechaFin,
        name,
        eventTypes,
      } = this.formData.value;

      console.log('onSubmit')
      // Obtener los datos del backend
      const fires = await this.fireService.getOnMainPage(
        {
          IdTerritorio: territory,
          IdPais: country,
          IdCcaa: autonomousCommunity,
          IdProvincia: province,
          IdEstadoSuceso: eventStatus,
          IdEstadoIncendio: fireStatus,
          IdSituacionEquivalente: situationEquivalent,
          IdSuperficieAfectada: affectedArea,
          IdMovimiento: move,
          IdComparativoFecha: between,
          FechaInicio: moment(fechaInicio).format('YYYY-MM-DD'),
          FechaFin: moment(fechaFin).format('YYYY-MM-DD'),
          denominacion: name,
          search: name,
          idClaseSuceso: eventTypes,
        },
        0,
        this.pageSize
      );

      // Verificar que la respuesta tenga el campo `count`
      if (!fires || typeof fires.count !== 'number') {
        console.error('La respuesta del backend no contiene el campo `count`:', fires);
        throw new Error('Respuesta inválida del backend');
      }

      // Actualizar los datos locales
      this.fires = fires;
      this.firesChange.emit(fires); // Emitir los datos actualizados

      // Guardar los filtros aplicados
      this.filtrosIncendioService.setFilters(this.formData.value);
      localStorage.setItem('fire-filters', JSON.stringify(this.formData.value));
    } catch (error) {
      console.error('Error al cargar datos:', error);
      this.snackBar.open('Error al cargar los datos', 'Cerrar', { duration: 3000 });
    } finally {
      this.isLoading = false;
      this.isLoadingChange.emit(false);
    }
  }

  clearFormFilter() {
    this.formData.reset();
    this.formData.patchValue({
      between: 1,
      move: 4,
      territory: 1,
      country: COUNTRIES_ID.SPAIN,
      fechaInicio: moment().subtract(4, 'days').toDate(),
      fechaFin: moment().toDate(),
      autonomousCommunity: '',
      province: '',
      municipality: '',
      fireStatus: '',
      episode: '',
      affectedArea: '',
      situationEquivalent: '',
      name: '',
      eventTypes: 1,
    });

    localStorage.removeItem('fire-filters');

    this.filteredCountries.set(this.listaPaisesNacionales());

    this.formData.get('CCAA')?.enable();
    this.formData.get('provincia')?.enable();
    this.formData.get('country')?.enable();
  }

  getForm(atributo: string): any {
    return this.formData.controls[atributo];
  }

  goModal() {
    // Obtener el territorio seleccionado en el filtro
    const selectedTerritory = this.formData.get('territory')?.value;

    const dialogRef = this.dialog.open(FireCreateEdit, {
      width: '45vw',
      maxWidth: 'none',
      disableClose: true,
      data: {
        title: 'Nuevo - Datos Evolución',
        fire: { idTerritorio: selectedTerritory }, // Pasar el territorio seleccionado
      },
    });

    dialogRef.afterClosed().subscribe((result) => {
      if (result) {
        console.log('Modal result:', result);
        this.snackBar
          .open('Datos ingresados correctamente!', '', {
            duration: 3000,
            horizontalPosition: 'center',
            verticalPosition: 'bottom',
            panelClass: ['snackbar-verde'],
          });
        this.onSubmit();
      }
    });
  }
}
