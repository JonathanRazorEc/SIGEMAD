import { CommonModule } from '@angular/common';
import { Component, computed, Inject, inject, LOCALE_ID, OnInit, signal, SimpleChanges } from '@angular/core';
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
import { MatSnackBar } from '@angular/material/snack-bar';
import { DragDropModule } from '@angular/cdk/drag-drop';
import { AlertService } from '@shared/alert/alert.service';
import { LocalFiltrosOpeFronteras } from '@services/ope/administracion/local-filtro-ope-fronteras.service';
import { OpeFronterasService } from '@services/ope/administracion/ope-fronteras.service';
import moment from 'moment';
import { FORMATO_FECHA } from '../../../../../../types/date-formats';
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
import { COUNTRIES_ID } from '@type/constants';
import { OpeValidator } from '@shared/validators/ope/ope-validator';
import { OpeErrorsService } from '@shared/services/ope/ope-errors.service';
import { OpeFronterasValidator } from '@shared/validators/ope/administracion/ope-fronteras-validator';

@Component({
  selector: 'ope-frontera-create-edit',
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
    { provide: DateAdapter, useClass: NativeDateAdapter },
    { provide: MAT_DATE_FORMATS, useValue: FORMATO_FECHA },
  ],
  templateUrl: './ope-frontera-create-edit-form.component.html',
  styleUrl: './ope-frontera-create-edit-form.component.scss',
})
export class OpeFronteraCreateEdit implements OnInit {
  constructor(
    private filtrosOpeFronterasService: LocalFiltrosOpeFronteras,
    private opeFronterasService: OpeFronterasService,
    public dialogRef: MatDialogRef<OpeFronteraCreateEdit>,
    private matDialog: MatDialog,
    public alertService: AlertService,
    private router: Router,

    private territoryService: TerritoryService,
    private countryService: CountryService,
    private autonomousCommunityService: AutonomousCommunityService,
    private provinceService: ProvinceService,
    private municipioService: MunicipalityService,

    @Inject(MAT_DIALOG_DATA) public data: { opeFrontera: any }
  ) {}

  public filteredCountries = signal<Countries[]>([]);

  public territories = signal<Territory[]>([]);
  public autonomousCommunities = signal<AutonomousCommunity[]>([]);
  public provinces = signal<Province[]>([]);
  public municipalities = signal<Municipality[]>([]);

  public listaPaisesExtranjeros = signal<Countries[]>([]);
  public listaPaisesNacionales = signal<Countries[]>([]);

  public formData!: FormGroup;

  public today: string = new Date().toISOString().split('T')[0];

  private spinner = inject(NgxSpinnerService);

  public snackBar = inject(MatSnackBar);
  public utilsService = inject(UtilsService);
  public opeErrorsService = inject(OpeErrorsService);

  async ngOnInit() {
    this.formData = new FormGroup(
      {
        nombre: new FormControl('', Validators.required),
        autonomousCommunity: new FormControl('', [Validators.required, OpeValidator.opcionValidaDeSelectPorId(() => this.CCAAFiltradas())]),
        provincia: new FormControl('', [Validators.required, OpeValidator.opcionValidaDeSelectPorId(() => this.provinciasFiltradas())]),
        municipality: new FormControl('', [Validators.required, OpeValidator.opcionValidaDeSelectPorId(() => this.municipiosFiltrados())]),
        carretera: new FormControl('', Validators.required),
        PK: new FormControl(null, [Validators.required, Validators.min(0), Validators.max(9999999), Validators.pattern(/^\d+([.,]\d{1,4})?$/)]),
        coordenadaUTM_X: new FormControl(null, [Validators.required, Validators.min(0), Validators.pattern(/^\d+$/)]),
        coordenadaUTM_Y: new FormControl(null, [Validators.required, Validators.min(0), Validators.pattern(/^\d+$/)]),
        transitoMedioVehiculos: new FormControl(null, [OpeValidator.optionalIntegerValidator()]),
        transitoAltoVehiculos: new FormControl(null, [OpeValidator.optionalIntegerValidator()]),
      },
      {
        validators: [OpeFronterasValidator.validarTransitoAltoMayorQueMedio('transitoMedioVehiculos', 'transitoAltoVehiculos')],
      }
    );

    //
    await this.loadCommunities();
    //

    if (!this.data.opeFrontera?.id) {
      this.formData.get('municipality')?.disable();
      this.formData.get('provincia')?.disable();
    }

    if (this.data.opeFrontera?.id) {
      //this.loadMunicipalities({ value: this.data.opeFrontera.idProvincia });
      //await this.loadProvinces({ value: this.data.opeFrontera.idCcaa });
      await this.loadProvinces({ option: { value: this.data.opeFrontera.idCcaa } } as MatAutocompleteSelectedEvent);
      //await this.loadMunicipios({ value: this.data.opeFrontera.idProvincia });
      await this.loadMunicipios({ option: { value: this.data.opeFrontera.idProvincia } } as MatAutocompleteSelectedEvent);
      this.formData.patchValue({
        id: this.data.opeFrontera.id,
        nombre: this.data.opeFrontera.nombre,
        autonomousCommunity: this.data.opeFrontera.idCcaa,
        provincia: this.data.opeFrontera.idProvincia,
        municipality: this.data.opeFrontera.idMunicipio,
        carretera: this.data.opeFrontera.carretera,
        PK: this.data.opeFrontera.pk,
        coordenadaUTM_X: this.data.opeFrontera.coordenadaUTM_X,
        coordenadaUTM_Y: this.data.opeFrontera.coordenadaUTM_Y,
        transitoMedioVehiculos: this.data.opeFrontera.transitoMedioVehiculos,
        transitoAltoVehiculos: this.data.opeFrontera.transitoAltoVehiculos,
      });
    }

    /*
    const countriesExtranjeros = await this.countryService.getExtranjeros();
    this.listaPaisesExtranjeros.set(countriesExtranjeros);
    const countriesNacionales = await this.countryService.getNacionales();
    this.listaPaisesNacionales.set(countriesNacionales);

    this.filteredCountries.set(countriesNacionales);

    const territories = await this.territoryService.get();
    this.territories.set(territories);
    */

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
    // Para el autocompletar de los municipios
    this.getForm('municipality')?.valueChanges.subscribe((value: string) => {
      if (typeof value === 'string') {
        this.filtroMunicipio.set(value);
      }
    });
    // FIN autocompletar

    //this.onSubmit();
  }

  ngOnChanges(changes: SimpleChanges): void {
    if ('refreshFilterForm' in changes) {
      //this.onSubmit();
      alert('aa');
    }
  }

  async onSubmit() {
    if (this.formData.valid) {
      this.spinner.show();
      const data = this.formData.value;

      //const municipio = this.municipalities().find((item) => item.id === data.municipality);

      if (this.data.opeFrontera?.id) {
        data.id = this.data.opeFrontera.id;
        await this.opeFronterasService
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
            this.snackBar.open(this.opeErrorsService.obtenerMensajeError(error, 'opeFronteras'), '', {
              duration: 3000,
              horizontalPosition: 'center',
              verticalPosition: 'bottom',
              panelClass: ['snackbar-rojo'],
            });
            console.error('Error', error);
          });
      } else {
        await this.opeFronterasService
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
            this.snackBar.open(this.opeErrorsService.obtenerMensajeError(error, 'opeFronteras'), '', {
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

  async loadCommunities(country?: any) {
    /*
    if (country === '9999') {
      alert('aa');
      this.autonomousCommunities.set([]);
      return;
    }
    */
    const autonomousCommunities = await this.autonomousCommunityService.getByCountry(COUNTRIES_ID.SPAIN.toString());
    this.autonomousCommunities.set(autonomousCommunities);
  }

  /*
  async loadProvinces(event: any) {
    const ac_id = event.value;
    const provinces = await this.provinceService.get(ac_id);
    this.provinces.set(provinces);
    //this.formData.get('provincia')?.enable();
    //this.formData.get('municipality')?.reset();
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
    //this.formData.get('municipality')?.reset();
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
