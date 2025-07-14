import { CommonModule } from '@angular/common';
import { Component, computed, Inject, inject, OnInit, signal, SimpleChanges } from '@angular/core';
import { FormControl, FormGroup, FormsModule, ReactiveFormsModule, Validators } from '@angular/forms';
import { FlexLayoutModule } from '@angular/flex-layout';
import { MatAutocompleteModule, MatAutocompleteSelectedEvent } from '@angular/material/autocomplete';
import { MatButtonModule } from '@angular/material/button';
import { DateAdapter, MAT_DATE_FORMATS, MatNativeDateModule, NativeDateAdapter } from '@angular/material/core';
import { MatDatepickerModule } from '@angular/material/datepicker';
import { MAT_DIALOG_DATA, MatDialog, MatDialogModule, MatDialogRef } from '@angular/material/dialog';
import { MatExpansionModule } from '@angular/material/expansion';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatIconModule } from '@angular/material/icon';
import { MatInputModule } from '@angular/material/input';
import { MatSelectModule } from '@angular/material/select';
import { Router } from '@angular/router';
import { NgxSpinnerModule, NgxSpinnerService } from 'ngx-spinner';
import { DragDropModule } from '@angular/cdk/drag-drop';
import { AlertService } from '@shared/alert/alert.service';
import { LocalFiltrosOpePuertos } from '@services/ope/administracion/local-filtro-ope-puertos.service';
import { OpePuertosService } from '@services/ope/administracion/ope-puertos.service';
import moment from 'moment';
import { FechaValidator } from '@shared/validators/fecha-validator';
import { UtilsService } from '@shared/services/utils.service';
import { ProvinceService } from '@services/province.service';
import { MunicipalityService } from '@services/municipality.service';
import { CountryService } from '@services/country.service';
import { Province } from '@type/province.type';
import { Municipality } from '@type/municipality.type';
import { Countries } from '@type/country.type';
import { TerritoryService } from '@services/territory.service';
import { AutonomousCommunityService } from '@services/autonomous-community.service';
import { Territory } from '@type/territory.type';
import { AutonomousCommunity } from '@type/autonomous-community.type';
import { COUNTRIES_ID, FECHA_MAXIMA_DATEPICKER, FECHA_MINIMA_DATEPICKER } from '@type/constants';
import { MomentDateAdapter } from '@angular/material-moment-adapter';
import { FORMATO_FECHA } from '@type/date-formats';
import { OpeFase } from '@type/ope/administracion/ope-fase.type';
import { OpeFasesService } from '@services/ope/administracion/ope-fases.service';
import { OpeUtilsService } from '@shared/services/ope/ope-utils.service';
import { OpePaisesService } from '@services/ope/administracion/ope-paises.service';
import { OpeErrorsService } from '@shared/services/ope/ope-errors.service';
import { MatSnackBar } from '@angular/material/snack-bar';
import { OpeValidator } from '@shared/validators/ope/ope-validator';
import { CCAA_MEDITERRANEO_ID } from '@type/ope/ope-constants';

@Component({
  selector: 'ope-puerto-create-edit',
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
  ],
  providers: [
    { provide: DateAdapter, useClass: MomentDateAdapter },
    { provide: MAT_DATE_FORMATS, useValue: FORMATO_FECHA },
  ],
  templateUrl: './ope-puerto-create-edit-form.component.html',
  styleUrl: './ope-puerto-create-edit-form.component.scss',
})
export class OpePuertoCreateEdit implements OnInit {
  constructor(
    private opePuertosService: OpePuertosService,
    private opeFasesService: OpeFasesService,
    public dialogRef: MatDialogRef<OpePuertoCreateEdit>,
    public alertService: AlertService,

    private territoryService: TerritoryService,
    private opePaisesService: OpePaisesService,
    private autonomousCommunityService: AutonomousCommunityService,
    private provinceService: ProvinceService,
    private municipioService: MunicipalityService,

    @Inject(MAT_DIALOG_DATA) public data: { opePuerto: any }
  ) {}

  public filteredCountriesOpePuertos = signal<Countries[]>([]);

  public territories = signal<Territory[]>([]);
  public autonomousCommunities = signal<AutonomousCommunity[]>([]);
  public provinces = signal<Province[]>([]);
  public municipalities = signal<Municipality[]>([]);

  public listaPaisesExtranjerosOpePuertos = signal<Countries[]>([]);
  public listaPaisesNacionalesOpePuertos = signal<Countries[]>([]);

  public opeFases = signal<OpeFase[]>([]);

  public formData!: FormGroup;

  public today: string = new Date().toISOString().split('T')[0];

  private spinner = inject(NgxSpinnerService);

  public utilsService = inject(UtilsService);
  public opeUtilsService = inject(OpeUtilsService);
  public fechaMinimaDatePicker = FECHA_MINIMA_DATEPICKER;
  public fechaMaximaDatePicker = FECHA_MAXIMA_DATEPICKER;

  public snackBar = inject(MatSnackBar);
  public opeErrorsService = inject(OpeErrorsService);

  async ngOnInit() {
    this.formData = new FormGroup(
      {
        nombre: new FormControl('', Validators.required),
        opeFase: new FormControl('', Validators.required),
        territory: new FormControl(1, [Validators.required]),
        country: new FormControl(COUNTRIES_ID.SPAIN, [Validators.required]),
        autonomousCommunity: new FormControl('', [Validators.required, OpeValidator.opcionValidaDeSelectPorId(() => this.CCAAFiltradas())]),
        provincia: new FormControl('', [Validators.required, OpeValidator.opcionValidaDeSelectPorId(() => this.provinciasFiltradas())]),
        municipality: new FormControl('', [Validators.required, OpeValidator.opcionValidaDeSelectPorId(() => this.municipiosFiltrados())]),
        coordenadaUTM_X: new FormControl(null, [Validators.required, Validators.min(0), Validators.pattern(/^\d+$/)]),
        coordenadaUTM_Y: new FormControl(null, [Validators.required, Validators.min(0), Validators.pattern(/^\d+$/)]),
        fechaValidezDesde: new FormControl(new Date(), [Validators.required, FechaValidator.validarFecha]),
        fechaValidezHasta: new FormControl(null, [FechaValidator.validarFecha]),
        capacidad: new FormControl(null, [Validators.min(0), Validators.max(9999999), Validators.pattern(/^\d+$/)]),
      },
      {
        validators: [FechaValidator.validarFechaFinPosteriorFechaInicio('fechaValidezDesde', 'fechaValidezHasta')],
      }
    );

    //
    await this.loadCommunities(this.data.opePuerto?.idPais);
    //

    if (!this.data.opePuerto?.id) {
      this.formData.get('municipality')?.disable();
      this.formData.get('provincia')?.disable();
    }

    if (this.data.opePuerto?.id) {
      //this.loadMunicipalities({ value: this.data.opePuerto.idProvincia });
      if (this.data.opePuerto.idCcaa) {
        //await this.loadProvinces({ value: this.data.opePuerto.idCcaa });
        await this.loadProvinces({ option: { value: this.data.opePuerto.idCcaa } } as MatAutocompleteSelectedEvent);
      }
      if (this.data.opePuerto.idProvincia) {
        //await this.loadMunicipios({ value: this.data.opePuerto.idProvincia });
        await this.loadMunicipios({ option: { value: this.data.opePuerto.idProvincia } } as MatAutocompleteSelectedEvent);
      }

      this.formData.patchValue({
        id: this.data.opePuerto.id,
        nombre: this.data.opePuerto.nombre,
        opeFase: this.data.opePuerto.idOpeFase,
        territory: this.data.opePuerto.idPais == COUNTRIES_ID.SPAIN ? 1 : this.data.opePuerto.idPais == 9999 ? 3 : 2,
        country: this.data.opePuerto.idPais,
        autonomousCommunity: this.data.opePuerto.idCcaa,
        provincia: this.data.opePuerto.idProvincia,
        municipality: this.data.opePuerto.idMunicipio,
        coordenadaUTM_X: this.data.opePuerto.coordenadaUTM_X,
        coordenadaUTM_Y: this.data.opePuerto.coordenadaUTM_Y,
        fechaValidezDesde: moment(this.data.opePuerto.fechaValidezDesde).format('YYYY-MM-DD'),
        fechaValidezHasta: this.data.opePuerto.fechaValidezHasta ? moment(this.data.opePuerto.fechaValidezHasta).format('YYYY-MM-DD') : null,
        capacidad: this.data.opePuerto.capacidad,
      });
    }

    const opeFases = await this.opeFasesService.get();
    this.opeFases.set(opeFases);

    // Obtener los países de la tabla OPE_Paises
    const opePuertosPaises = await this.opePaisesService.get({
      opePuertos: true,
    });
    if (opePuertosPaises != null && opePuertosPaises.length > 0) {
      const countriesExtranjerosOpePuertos = opePuertosPaises.filter((p) => p.extranjero).map((p) => p.pais);
      this.listaPaisesExtranjerosOpePuertos.set(countriesExtranjerosOpePuertos);
      const countriesNacionalesOpePuertos = opePuertosPaises.filter((p) => !p.extranjero).map((p) => p.pais);
      this.listaPaisesNacionalesOpePuertos.set(countriesNacionalesOpePuertos);

      const countryValue = this.formData.get('country')?.value;
      if (countryValue && countryValue == COUNTRIES_ID.SPAIN) {
        this.filteredCountriesOpePuertos.set(countriesNacionalesOpePuertos);
      } else {
        this.filteredCountriesOpePuertos.set(countriesExtranjerosOpePuertos);
      }
    }

    const territories = await this.territoryService.get();
    const filteredTerritories = territories.filter((t) => t.descripcion !== 'Transfronterizo');
    this.territories.set(filteredTerritories);
    //

    //await this.loadCommunities();

    // Para el autocompletar de las CCAA
    this.getForm('autonomousCommunity')?.valueChanges.subscribe((value: string) => {
      if (typeof value === 'string') {
        this.filtroCCAA.set(value);
      }
    });
    // FIN autocompletar
    // Para el autocompletar de las provincias
    this.getForm('provincia')?.valueChanges.subscribe((value: string) => {
      if (typeof value === 'string') {
        this.filtroProvincia.set(value);
      }
    });
    // FIN autocompletar
    // Para el autocompletar de los municipios
    this.getForm('municipality')?.valueChanges.subscribe((value: string) => {
      if (typeof value === 'string') {
        this.filtroMunicipio.set(value);
      }
    });
    // FIN autocompletar
  }

  async onSubmit() {
    if (this.formData.valid) {
      this.spinner.show();
      const data = this.formData.value;

      //const municipio = this.municipalities().find((item) => item.id === data.municipality);
      if (this.data.opePuerto?.id) {
        data.id = this.data.opePuerto.id;
        await this.opePuertosService
          .update(data)
          .then((response) => {
            // PCD
            /*
            this.snackBar
              .open('Datos modificados correctamente!', '', {
                duration: 3000,
                horizontalPosition: 'center',
                verticalPosition: 'bottom',
                panelClass: ['snackbar-verde'],
              })
              .afterDismissed()
              .subscribe(() => {
              */
            this.closeModal({ refresh: true });
            this.spinner.hide();
            //});
            // FIN PCD
          })
          .catch((error) => {
            this.spinner.hide();
            this.snackBar.open(this.opeErrorsService.obtenerMensajeError(error, 'opePuertos'), '', {
              duration: 3000,
              horizontalPosition: 'center',
              verticalPosition: 'bottom',
              panelClass: ['snackbar-rojo'],
            });
            console.error('Error', error);
          });
      } else {
        await this.opePuertosService
          .post(data)
          .then((response) => {
            /*
            this.snackBar
              .open('Datos creados correctamente!', '', {
                duration: 3000,
                horizontalPosition: 'center',
                verticalPosition: 'bottom',
                panelClass: ['snackbar-verde'],
              })
              .afterDismissed()
              .subscribe(() => {
              */
            this.closeModal({ refresh: true });
            this.spinner.hide();
            //});
          })
          .catch((error) => {
            this.spinner.hide();
            this.snackBar.open(this.opeErrorsService.obtenerMensajeError(error, 'opePuertos'), '', {
              duration: 3000,
              horizontalPosition: 'center',
              verticalPosition: 'bottom',
              panelClass: ['snackbar-rojo'],
            });
            console.error('Error', error);
          });
      }
    } else {
      this.formData.markAllAsTouched();
    }
  }

  closeModal(params?: any) {
    this.dialogRef.close(params);
  }

  getForm(atributo: string): any {
    return this.formData.controls[atributo];
  }

  // para meter en uns servicio común
  async changeTerritory(event: any) {
    this.formData.patchValue({
      country: event.value == 1 ? COUNTRIES_ID.SPAIN : '',
      autonomousCommunity: '',
      provincia: '',
      municipality: '',
    });
    this.loadCommunities(event.value == 1 ? COUNTRIES_ID.SPAIN : '9999');
    if (event.value == 1) {
      this.filteredCountriesOpePuertos.set(this.listaPaisesNacionalesOpePuertos());
    }
    if (event.value == 2) {
      this.filteredCountriesOpePuertos.set(this.listaPaisesExtranjerosOpePuertos());
      this.formData.get('autonomousCommunity')?.disable();
    }
    if (event.value == 3) {
      this.filteredCountriesOpePuertos.set([]);
      this.formData.get('autonomousCommunity')?.disable();
    }
  }

  /*
  async loadCommunities(country?: any) {
    if (country === '9999' || this.formData.value.country !== COUNTRIES_ID.SPAIN) {
      this.autonomousCommunities.set([]);
      this.formData.get('autonomousCommunity')?.disable();
      this.formData.get('provincia')?.disable();
      this.formData.get('municipality')?.disable();
      return;
    }

    const autonomousCommunities = await this.autonomousCommunityService.getByCountry(country ?? this.formData.value.country);
    this.autonomousCommunities.set(autonomousCommunities);
    this.formData.get('autonomousCommunity')?.enable();
  }
  */

  //
  async loadCommunities(country?: any) {
    const countryToUse = country ?? this.formData.value.country;
    if (countryToUse === '9999' || countryToUse !== COUNTRIES_ID.SPAIN) {
      this.autonomousCommunities.set([]);
      this.formData.get('autonomousCommunity')?.disable();
      this.formData.get('provincia')?.disable();
      this.formData.get('municipality')?.disable();
      return;
    }

    const autonomousCommunities = await this.autonomousCommunityService.getByCountry(countryToUse);
    //this.autonomousCommunities.set(autonomousCommunities);

    // Solo aparecen las CCAA del mediterráneo
    const idsCCAAMediterraneo = Object.values(CCAA_MEDITERRANEO_ID) as number[];
    const CCAAMediterraneo = autonomousCommunities.filter((c) => idsCCAAMediterraneo.includes(c.id));
    this.autonomousCommunities.set(CCAAMediterraneo);
    // FIN Solo aparecen las CCAA del mediterráneo

    this.formData.get('autonomousCommunity')?.enable();
  }
  //

  /*
  async loadProvinces(event: any) {
    const ac_id = event.value;
    const provinces = await this.provinceService.get(ac_id);
    this.provinces.set(provinces);
    //this.formData.get('provincia')?.enable();
    //
    //this.formData.get('provincia')?.reset();
    this.formData.get('provincia')?.setValue('');
    this.formData.get('provincia')?.enable();
    this.formData.get('municipality')?.reset();
    this.formData.get('municipality')?.disable();
    //
  }
  */

  //
  async loadProvinces(event: MatAutocompleteSelectedEvent) {
    //const ac_id = event.value;
    const ac_id = event.option.value;
    const provinces = await this.provinceService.get(ac_id);
    this.provinces.set(provinces);
    //this.formData.get('provincia')?.enable();
    //
    //this.formData.get('provincia')?.reset();
    this.formData.get('provincia')?.setValue('');
    this.formData.get('provincia')?.enable();
    this.formData.get('municipality')?.reset();
    this.formData.get('municipality')?.disable();
    //
  }
  //

  /*
  async loadMunicipios(event: any) {
    const provinciaId = event.value;
    const municipios = await this.municipioService.get(provinciaId);
    this.municipalities.set(municipios);
    this.formData.get('municipality')?.reset();
    this.formData.get('municipality')?.enable();
  }
  */

  //
  async loadMunicipios(event: MatAutocompleteSelectedEvent) {
    //const provinciaId = event.value;
    const provinciaId = event.option.value;
    const municipios = await this.municipioService.get(provinciaId);
    this.municipalities.set(municipios);
    //this.formData.get('municipality')?.reset();
    this.formData.get('municipality')?.setValue('');
    this.formData.get('municipality')?.enable();
  }
  //

  // Para el autocompletar de las CCAA
  filtroCCAA = signal('');

  // Para input con objeto como valor
  displayFnCCAA = (id: number | null): string => {
    if (id == null) return '';
    const match = this.autonomousCommunities().find((item) => item.id === id);
    return match ? match.descripcion : '';
  };

  // Computado para filtrar lista corta
  CCAAFiltradas = computed(() => {
    return this.utilsService.filtrarPorTexto(this.autonomousCommunities(), this.filtroCCAA(), true, 'descripcion');
  });
  // Fin autocompletar

  // Para el autocompletar de las provincias
  filtroProvincia = signal('');

  // Para input con objeto como valor
  displayFnProvincia = (id: number | null): string => {
    if (id == null) return '';
    const match = this.provinces().find((item) => item.id === id);
    return match ? match.descripcion : '';
  };

  // Computado para filtrar lista corta
  provinciasFiltradas = computed(() => {
    return this.utilsService.filtrarPorTexto(this.provinces(), this.filtroProvincia(), true, 'descripcion');
  });
  // Fin autocompletar

  // Para el autocompletar de los municipios
  filtroMunicipio = signal('');

  // Para input con objeto como valor
  displayFnMunicipio = (id: number | null): string => {
    if (id == null) return '';
    const match = this.municipalities().find((item) => item.id === id);
    return match ? match.descripcion : '';
  };

  // Computado para filtrar lista corta
  municipiosFiltrados = computed(() => {
    return this.utilsService.filtrarPorTexto(this.municipalities(), this.filtroMunicipio(), true, 'descripcion');
  });
  // Fin autocompletar

  getFechaFormateadaConHorasMinutosYSegundos = (fecha: any) => {
    if (!fecha) {
      return '';
    }
    return moment(fecha).format('DD/MM/YYYY HH:mm:ss');
  };
}
