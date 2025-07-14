import { CommonModule } from '@angular/common';
import { Component, EventEmitter, inject, Input, OnDestroy, OnInit, Output, signal, ViewChild } from '@angular/core';

import { DateAdapter, MAT_DATE_FORMATS } from '@angular/material/core';

import { FlexLayoutModule } from '@angular/flex-layout';
import { FormBuilder, FormGroup, FormsModule, ReactiveFormsModule, Validators } from '@angular/forms';
import { MatAutocompleteModule } from '@angular/material/autocomplete';
import { MatButtonModule } from '@angular/material/button';
import { MatCheckboxModule } from '@angular/material/checkbox';
import { MatDatepickerModule } from '@angular/material/datepicker';
import { MatExpansionModule } from '@angular/material/expansion';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatIconModule } from '@angular/material/icon';
import { MatInputModule } from '@angular/material/input';
import { MatSelectModule } from '@angular/material/select';

import { MatDialogModule } from '@angular/material/dialog';
import { MomentDateAdapter } from '@angular/material-moment-adapter';
import moment from 'moment';
import { AutonomousCommunityService } from '../../../../services/autonomous-community.service';
import { ComparativeDateService } from '../../../../services/comparative-date.service';
import { CountryService } from '../../../../services/country.service';
import { EventStatusService } from '../../../../services/eventStatus.service';
import { FireService } from '../../../../services/fire.service';
import { LocalFiltrosIncendio } from '../../../../services/local-filtro-incendio.service';
import { MasterDataEvolutionsService } from '../../../../services/master-data-evolutions.service';
import { MenuItemActiveService } from '../../../../services/menu-item-active.service';
import { MoveService } from '../../../../services/move.service';
import { MunicipalityService } from '../../../../services/municipality.service';
import { ProvinceService } from '../../../../services/province.service';
import { SeverityLevelService } from '../../../../services/severity-level.service';
import { SuperficiesService } from '../../../../services/superficies.service';
import { TerritoryService } from '../../../../services/territory.service';
import { FormFieldComponent } from '../../../../shared/Inputs/field.component';
import { ComparativeDate } from '../../../../types/comparative-date.type';
import { Countries } from '../../../../types/country.type';
import { EventStatus } from '../../../../types/eventStatus.type';
import { FireStatus } from '../../../../types/fire-status.type';
import { Move } from '../../../../types/move.type';
import { SeverityLevel } from '../../../../types/severity-level.type';

import { MatSort } from '@angular/material/sort';
import { MatTableDataSource, MatTableModule } from '@angular/material/table';
import { NgxSpinnerModule, NgxSpinnerService } from 'ngx-spinner';
import { EventService } from '../../../../services/event.service';
import { SucesosRelacionadosService } from '../../../../services/sucesos-relacionados.service';

import { AlertService } from '../../../../shared/alert/alert.service';
import { MatSnackBar } from '@angular/material/snack-bar';
import { FORMATO_FECHA } from '../../../../types/date-formats';
import { COUNTRIES_ID } from '@type/constants';
import { Subject, finalize, takeUntil } from 'rxjs';

interface FireEvent {
  id: number;
  idSuceso: number;
  denominacion?: string;
  descripcion?: string;
  fechaInicio?: Date;
  selected?: boolean;
}

interface FormState {
  name: string;
  claseSuceco: number;
  territory: number;
  country: number;
  CCAA: any;
  province: any;
  minicipality: any;
  move: number;
  between: number;
  fechaInicio: Date;
  fechaFin: Date;
}

interface SucesosRelacionadosResponse {
  data: {
    sucesosAsociados?: FireEvent[];
    [key: string]: any;
  };
}

@Component({
  selector: 'app-fire-related-event-form',
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
    MatTableModule,
    MatIconModule,
    FlexLayoutModule,
    MatExpansionModule,
    MatDatepickerModule,
    MatDialogModule,
    MatCheckboxModule,
    NgxSpinnerModule,
  ],
  providers: [
    { provide: DateAdapter, useClass: MomentDateAdapter },
    { provide: MAT_DATE_FORMATS, useValue: FORMATO_FECHA },
  ],
  templateUrl: './fire-related-event-form.component.html',
  styleUrl: './fire-related-event-form.component.scss',
})
export class FireRelatedEventForm implements OnInit, OnDestroy {
  @Input() fire!: FireEvent;
  @Input() fireDetail: any;
  @Output() closeModal = new EventEmitter<void>();
  @Output() hasRecords = new EventEmitter<boolean>();

  @ViewChild(MatSort) sort!: MatSort;

  private destroy$ = new Subject<void>();
  private fb = inject(FormBuilder);
  private spinner = inject(NgxSpinnerService);
  private alertService = inject(AlertService);
  private snackBar = inject(MatSnackBar);

  // Servicios
  private autonomousCommunityService = inject(AutonomousCommunityService);
  private municipioService = inject(MunicipalityService);
  private filtrosIncendioService = inject(LocalFiltrosIncendio);
  private menuItemActiveService = inject(MenuItemActiveService);
  private superficiesService = inject(SuperficiesService);
  private territoryService = inject(TerritoryService);
  private masterData = inject(MasterDataEvolutionsService);
  private severityLevelService = inject(SeverityLevelService);
  private fireService = inject(FireService);
  private eventStatusService = inject(EventStatusService);
  private moveService = inject(MoveService);
  private comparativeDateService = inject(ComparativeDateService);
  private sucesosRelacionadosService = inject(SucesosRelacionadosService);
  private provinceService = inject(ProvinceService);
  private countryService = inject(CountryService);
  private eventService = inject(EventService);

  // Señales de estado
  public dataFindedEvents = signal<FireEvent[]>([]);
  public dataFindedRelationsEvents = signal<FireEvent[]>([]);
  public comparativeDates = signal<ComparativeDate[]>([]);
  public listaSucesosRelacionados = signal<SucesosRelacionadosResponse>({ data: {} });
  public listaSucesos = signal<{data: FireEvent[]}>({data: []});
  public fireStatus = signal<FireStatus[]>([]);
  public severityLevels = signal<SeverityLevel[]>([]);
  public filteredCountries = signal<Countries[]>([]);
  public listaPaisesExtranjeros = signal<Countries[]>([]);
  public listaPaisesNacionales = signal<Countries[]>([]);
  public superficiesFiltro = signal<any[]>([]);
  public eventStatus = signal<EventStatus[]>([]);
  public moves = signal<Move[]>([]);
  public showDateEnd = signal<boolean>(true);
  public listadoClaseSuceso = signal<any[]>([]);
  public listadoTerritorio = signal<any[]>([]);
  public listadoPaises = signal<any[]>([]);
  public listadoCCAA = signal<any[]>([]);
  public listadoProvincia = signal<any[]>([]);
  public listadoMunicipio = signal<any[]>([]);
  public isSaving = signal<boolean>(false);

  // Tabla
  public dataSource = new MatTableDataSource<FireEvent>([]);
  public displayedColumns: string[] = ['fecha', 'eventType', 'status', 'denominacion', 'opciones'];
  public displayedColumnsRelations: string[] = ['fecha', 'eventType', 'status', 'denominacion', 'opciones'];

  // Formularios
  public formData!: FormGroup;
  public myForm!: FormGroup;

  ngOnInit(): void {
    this.initForms();
    this.loadInitialData();
    this.menuItemActiveService.set.emit('/fire');
  }

  ngOnDestroy(): void {
    this.destroy$.next();
    this.destroy$.complete();
  }

  private initForms(): void {
    this.myForm = this.fb.group({
      selectField: ['', Validators.required],
      inputField1: ['', Validators.required],
      inputField2: ['', Validators.required],
    });

    this.formData = this.fb.group({
      name: [''],
      claseSuceco: [1],
      territory: [1],
      country: [COUNTRIES_ID.SPAIN],
      CCAA: [''],
      province: [''],
      minicipality: [''],
      move: [1],
      between: [1],
      fechaInicio: [moment().subtract(4, 'days').toDate()],
      fechaFin: [moment().toDate()],
    });
  }

  private async loadInitialData(): Promise<void> {
    this.spinner.show();
    
    try {
      await Promise.all([
        this.loadMasterData(),
        this.loadCountryData(),
        this.loadSeverityAndStatusData(),
        this.loadRelatedEventsData()
      ]);
      
      await this.loadCommunities();
      await this.onSubmit();
    } catch (error) {
      console.error('Error loading initial data', error);
      this.showErrorMessage('Ha ocurrido un error al cargar los datos iniciales');
    } finally {
      this.spinner.hide();
    }
  }

  private async loadMasterData(): Promise<void> {
    const [
      estadoSuceso,
      territories,
      superficiesFiltro,
      moves,
      comparativeDates
    ] = await Promise.all([
      this.eventService.get(),
      this.territoryService.get(),
      this.superficiesService.getSuperficiesFiltro(),
      this.moveService.get(),
      this.comparativeDateService.get()
    ]);
    
    this.listadoClaseSuceso.set(estadoSuceso);
    this.listadoTerritorio.set(territories);
    this.superficiesFiltro.set(superficiesFiltro);
    this.moves.set(moves);
    this.comparativeDates.set(comparativeDates);
  }

  private async loadCountryData(): Promise<void> {
    const [countriesExtranjeros, countriesNacionales] = await Promise.all([
      this.countryService.getExtranjeros(),
      this.countryService.getNacionales()
    ]);
    
    this.listaPaisesExtranjeros.set(countriesExtranjeros);
    this.listaPaisesNacionales.set(countriesNacionales);
    this.filteredCountries.set(countriesNacionales);
  }

  private async loadSeverityAndStatusData(): Promise<void> {
    const [fireStatus, severityLevels, eventStatus] = await Promise.all([
      this.masterData.getFireStatus(),
      this.severityLevelService.get(),
      this.eventStatusService.get()
    ]);
    
    this.fireStatus.set(fireStatus);
    this.severityLevels.set(severityLevels);
    this.eventStatus.set(eventStatus);
  }

  private async loadRelatedEventsData(): Promise<void> {
    if (this.fireDetail) {
      const listadoSucesosRelacionados = await this.sucesosRelacionadosService.get(this.fireDetail.id, this.fire.idSuceso);
      this.listaSucesosRelacionados.set({ data: listadoSucesosRelacionados });
    } else {
      const listadoSucesosRelacionados = await this.sucesosRelacionadosService.getRegistros(this.fire.idSuceso);
      this.listaSucesosRelacionados.set({ data: listadoSucesosRelacionados });
    }
    this.updateHasRecords();
  }

  async changeTerritory(event: any): Promise<void> {
    const territoryValue = event.value;
    const countryValue = territoryValue == 1 ? COUNTRIES_ID.SPAIN : '';
    
    this.formData.patchValue({
      country: countryValue,
      autonomousCommunity: '',
      province: '',
      municipality: '',
    });
    
    await this.loadCommunities(territoryValue.id == 1 ? COUNTRIES_ID.SPAIN : '9999');
    
    if (territoryValue == 1) {
      this.filteredCountries.set(this.listaPaisesNacionales());
    } else if (territoryValue == 2) {
      this.filteredCountries.set(this.listaPaisesExtranjeros());
    } else if (territoryValue == 3) {
      this.filteredCountries.set([]);
    }
  }

  async loadCommunities(country?: any): Promise<void> {
    if (country === '9999') {
      this.listadoCCAA.set([]);
      return;
    }
    
    const actualCountry = country ?? this.formData.value.country;
    const autonomousCommunities = await this.autonomousCommunityService.getByCountry(actualCountry);
    this.listadoCCAA.set(autonomousCommunities);
  }

  async loadProvinces(event: any): Promise<void> {
    const ac_id = event.value.id;
    const provinces = await this.provinceService.get(ac_id);
    this.listadoProvincia.set(provinces);
  }

  async loadMunicipios(event: any): Promise<void> {
    const provinciaId = event.value.id;
    const municipios = await this.municipioService.get(provinciaId);
    this.listadoMunicipio.set(municipios);
  }

  changeBetween(event: any): void {
    this.showDateEnd.set(event.value === 1 || event.value === 5);
  }

  async onSubmit(): Promise<void> {
    this.spinner.show();
    
    try {
      const formValues = this.formData.value as FormState;
      const searchParams = this.buildSearchParams(formValues);
      const listadoSucesos: any = await this.sucesosRelacionadosService.getListaSuceso(searchParams);
      
      const dataFiltrada = this.filterAssociatedEvents(listadoSucesos.data);
      this.listaSucesos.set({ data: dataFiltrada });
    } catch (error) {
      console.error('Error en la búsqueda', error);
      this.showErrorMessage('Error al buscar sucesos relacionados');
    } finally {
      this.spinner.hide();
    }
  }

  private buildSearchParams(formValues: FormState): any {
    const { name, claseSuceco, territory, country, CCAA, province, minicipality, move, between, fechaInicio, fechaFin } = formValues;
    
    return {
      Denominacion: name,
      IdClaseSuceso: claseSuceco,
      IdTerritorio: territory,
      IdPais: country,
      IdSuceso: this.fire.idSuceso,
      IdCcaa: CCAA?.id,
      IdProvincia: province?.id,
      IdMunicipio: minicipality?.id,
      IdMovimiento: move,
      IdComparativoFecha: between,
      FechaInicio: moment(fechaInicio).format('YYYY-MM-DD'),
      FechaFin: moment(fechaFin).format('YYYY-MM-DD'),
      Page: 0,
      search: name,
    };
  }

  private filterAssociatedEvents(allEvents: FireEvent[]): FireEvent[] {
    const associatedEvents = this.listaSucesosRelacionados()?.data?.sucesosAsociados || [];
    
    return allEvents.filter(
      (event: FireEvent) => !associatedEvents.some(
        (associated: FireEvent) => associated.id === event.id
      )
    );
  }

  async guardarAgregar(): Promise<void> {
    this.spinner.show();
    
    if (this.isSaving()) {
      this.spinner.hide();
      return;
    }
    
    this.isSaving.set(true);
    
    try {
      const idsSucesosAsociados = this.getAssociatedEventIds();
      
      if (idsSucesosAsociados?.length === 0) {
        this.showWarningMessage('Debe introducir al menos un suceso!');
        return;
      }
      
      await this.saveAssociatedEvents(idsSucesosAsociados);
      await this.onSubmit();
      this.closeModal.emit();
      this.showSuccessMessage('Registro guardado correctamente!');
    } catch (error) {
      console.error('Error al guardar', error);
      this.showErrorAlert('Ha ocurrido un error!', 'Contacte a soporte técnico!');
    } finally {
      this.isSaving.set(false);
      this.spinner.hide();
    }
  }

  private getAssociatedEventIds(): number[] {
    const sucesosAsociados = this.listaSucesosRelacionados()?.data?.sucesosAsociados;
    return sucesosAsociados ? sucesosAsociados.map((item: FireEvent) => item.id) : [];
  }

  private async saveAssociatedEvents(idsSucesosAsociados: number[]): Promise<void> {
    const response: any = await this.sucesosRelacionadosService.post({
      idsSucesosAsociados,
      IdRegistroActualizacion: this.fireDetail?.id ?? 0,
      idSuceso: this.fire.idSuceso,
    });
    
    let responseId = 0;
    if (!this.fireDetail ) {
      responseId = response.idSucesoRelacionado;
    } else {
      responseId = this.fireDetail.id;
    }
    
    const listadoSucesosRelacionados = await this.sucesosRelacionadosService.get(responseId, this.fire.idSuceso);
    this.listaSucesosRelacionados.set({ data: listadoSucesosRelacionados || {} });
    this.updateHasRecords();
  }

  async handleSeleccionarItem(i: number): Promise<void> {
    const newLista: any = this.listaSucesos();
    newLista.data[i].selected = !newLista.data[i].selected;
    this.listaSucesos.set(newLista);
  }

  agregarItem(): void {
    this.spinner.show();
    
    try {
      const newData = this.listaSucesosRelacionados();
      const dataPush = this.listaSucesos()?.data?.filter((item: any) => item.selected);
      
      if (newData?.data?.sucesosAsociados) {
        newData.data.sucesosAsociados = [...newData.data.sucesosAsociados, ...dataPush];
      } else {
        newData.data.sucesosAsociados = [...dataPush];
      }
      
      const newDataOptions = this.listaSucesos()?.data?.filter((item: any) => !item.selected);
      
      this.listaSucesos.set({ data: newDataOptions });
      this.listaSucesosRelacionados.set({ ...newData });
      this.updateHasRecords();
    } finally {
      this.spinner.hide();
    }
  }

  async eliminarItem(i: number): Promise<void> {
    this.spinner.show();
    
    try {
      const relacionados = this.listaSucesosRelacionados();
      const sucesosAsociados = relacionados?.data?.sucesosAsociados || [];
      
      if (!sucesosAsociados.length || i >= sucesosAsociados.length) {
        return;
      }
      
      const itemToRemove = sucesosAsociados[i];

      const newListaSucesos = [
        ...this.listaSucesos().data, 
        { ...itemToRemove, selected: false }
      ];
      this.listaSucesos.set({ data: newListaSucesos });
      
      const newAsociados = sucesosAsociados.filter(
        (asociado: any) => asociado.id !== itemToRemove.id
      );
      
      this.listaSucesosRelacionados.set({
        data: {
          ...relacionados.data,
          sucesosAsociados: newAsociados,
        },
      });
      this.updateHasRecords();
    } finally {
      this.spinner.hide();
    }
  }

  clearFormFilter(): void {
    this.formData.reset();
    this.formData.patchValue({
      between: 1,
      move: 1,
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
      severityLevel: '',
      name: '',
    });
  }

  getForm(atributo: string): any {
    return this.formData.controls[atributo];
  }

  getFormatdate(date: any): string {
    return date ? moment(date).format('DD/MM/YY HH:mm') : '';
  }

  getDescripcionProcedenciaDestion(procedenciaDestino: any[]): string {
    return procedenciaDestino.map((obj) => obj.descripcion).join(', ');
  }
  
  private showSuccessMessage(message: string): void {
    this.snackBar.open(message, '', {
      duration: 3000,
      horizontalPosition: 'center',
      verticalPosition: 'bottom',
      panelClass: ['snackbar-verde'],
    });
  }
  
  private showWarningMessage(message: string): void {
    this.snackBar.open(message, '', {
      duration: 3000,
      horizontalPosition: 'center',
      verticalPosition: 'bottom',
      panelClass: ['snackbar-rojo'],
    });
  }
  
  private showErrorMessage(message: string): void {
    this.snackBar.open(message, '', {
      duration: 4000,
      horizontalPosition: 'center',
      verticalPosition: 'bottom',
      panelClass: ['snackbar-rojo'],
    });
  }
  
  private showErrorAlert(title: string, text: string): void {
    this.alertService
      .showAlert({
        title,
        text,
        icon: 'error',
      })
      .then(() => {
        this.closeModal.emit();
      });
  }

  private updateHasRecords(): void {
    const hasSucesosAsociados = (this.listaSucesosRelacionados()?.data?.sucesosAsociados || []).length > 0;
    this.hasRecords.emit(hasSucesosAsociados);
  }
}
