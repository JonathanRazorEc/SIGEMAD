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
import { LocalFiltrosOpeAreasDescanso } from '@services/ope/administracion/local-filtro-ope-areas-descanso.service';
import { OpeAreasDescansoService } from '@services/ope/administracion/ope-areas-descanso.service';
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
import { OpeAreaDescansoTipo } from '@type/ope/administracion/ope-area-descanso-tipo';
import { OpeAreasDescansoTiposService } from '@services/ope/administracion/ope-areas-descanso-tipos.service';
//import { OpeEstadoOcupacion } from '@type/ope/administracion/ope-estado-ocupacion.type';
//import { OpeEstadosOcupacionService } from '@services/ope/administracion/ope-estados-ocupacion.service';
import { OpeValidator } from '@shared/validators/ope/ope-validator';
import moment from 'moment';

@Component({
  selector: 'ope-area-descanso-create-edit',
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
  templateUrl: './ope-area-descanso-create-edit-form.component.html',
  styleUrl: './ope-area-descanso-create-edit-form.component.scss',
})
export class OpeAreaDescansoCreateEdit implements OnInit {
  constructor(
    private opeAreasDescansoService: OpeAreasDescansoService,
    public dialogRef: MatDialogRef<OpeAreaDescansoCreateEdit>,
    public alertService: AlertService,

    private autonomousCommunityService: AutonomousCommunityService,
    private provinceService: ProvinceService,
    private municipioService: MunicipalityService,

    private opeAreasDescansoTiposService: OpeAreasDescansoTiposService,
    //private opeEstadosOcupacionService: OpeEstadosOcupacionService,

    @Inject(MAT_DIALOG_DATA) public data: { opeAreaDescanso: any }
  ) {}

  public filteredCountries = signal<Countries[]>([]);

  public territories = signal<Territory[]>([]);
  public autonomousCommunities = signal<AutonomousCommunity[]>([]);
  public provinces = signal<Province[]>([]);
  public municipalities = signal<Municipality[]>([]);

  public listaPaisesExtranjeros = signal<Countries[]>([]);
  public listaPaisesNacionales = signal<Countries[]>([]);

  public opeAreasDescansoTipos = signal<OpeAreaDescansoTipo[]>([]);
  //public opeEstadosOcupacion = signal<OpeEstadoOcupacion[]>([]);

  public formData!: FormGroup;

  public today: string = new Date().toISOString().split('T')[0];

  private spinner = inject(NgxSpinnerService);

  //PCD
  public utilsService = inject(UtilsService);
  // FIN PCD

  async ngOnInit() {
    this.formData = new FormGroup({
      nombre: new FormControl('', Validators.required),
      opeAreaDescansoTipo: new FormControl('', Validators.required),
      autonomousCommunity: new FormControl('', [Validators.required, OpeValidator.opcionValidaDeSelectPorId(() => this.CCAAFiltradas())]),
      //CCAA: new FormControl(''),
      //province: new FormControl(''),
      provincia: new FormControl('', [Validators.required, OpeValidator.opcionValidaDeSelectPorId(() => this.provinciasFiltradas())]),
      municipality: new FormControl('', [Validators.required, OpeValidator.opcionValidaDeSelectPorId(() => this.municipiosFiltrados())]),
      carretera: new FormControl('', Validators.required),
      PK: new FormControl(null, [Validators.required, Validators.min(0), Validators.max(9999999), Validators.pattern(/^\d+([.,]\d{1,4})?$/)]),
      coordenadaUTM_X: new FormControl(null, [Validators.required, Validators.min(0), Validators.pattern(/^\d+$/)]),
      coordenadaUTM_Y: new FormControl(null, [Validators.required, Validators.min(0), Validators.pattern(/^\d+$/)]),
      capacidad: new FormControl(null, [Validators.min(0), Validators.max(9999999), Validators.pattern(/^\d+$/)]),
      //opeEstadoOcupacion: new FormControl('', Validators.required),
    });

    //
    await this.loadCommunities();
    //

    if (!this.data.opeAreaDescanso?.id) {
      this.formData.get('municipality')?.disable();
      this.formData.get('provincia')?.disable();
    }

    if (this.data.opeAreaDescanso?.id) {
      //this.loadMunicipalities({ value: this.data.opeAreaDescanso.idProvincia });
      //await this.loadProvinces({ value: this.data.opeAreaDescanso.idCcaa });
      await this.loadProvinces({ option: { value: this.data.opeAreaDescanso.idCcaa } } as MatAutocompleteSelectedEvent);
      //await this.loadMunicipios({ value: this.data.opeAreaDescanso.idProvincia });
      await this.loadMunicipios({ option: { value: this.data.opeAreaDescanso.idProvincia } } as MatAutocompleteSelectedEvent);
      this.formData.patchValue({
        id: this.data.opeAreaDescanso.id,
        nombre: this.data.opeAreaDescanso.nombre,
        opeAreaDescansoTipo: this.data.opeAreaDescanso.idOpeAreaDescansoTipo,
        autonomousCommunity: this.data.opeAreaDescanso.idCcaa,
        provincia: this.data.opeAreaDescanso.idProvincia,
        municipality: this.data.opeAreaDescanso.idMunicipio,
        carretera: this.data.opeAreaDescanso.carretera,
        PK: this.data.opeAreaDescanso.pk,
        coordenadaUTM_X: this.data.opeAreaDescanso.coordenadaUTM_X,
        coordenadaUTM_Y: this.data.opeAreaDescanso.coordenadaUTM_Y,
        capacidad: this.data.opeAreaDescanso.capacidad,
        //opeEstadoOcupacion: this.data.opeAreaDescanso.idOpeEstadoOcupacion,
      });
    }

    const opeAreasDescansoTipos = await this.opeAreasDescansoTiposService.get();
    this.opeAreasDescansoTipos.set(opeAreasDescansoTipos);

    //const opeEstadosOcupacion = await this.opeEstadosOcupacionService.get();
    //this.opeEstadosOcupacion.set(opeEstadosOcupacion);

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

    //this.onSubmit();
  }

  async onSubmit() {
    if (this.formData.valid) {
      this.spinner.show();
      const data = this.formData.value;

      //const municipio = this.municipalities().find((item) => item.id === data.municipality);

      if (this.data.opeAreaDescanso?.id) {
        data.id = this.data.opeAreaDescanso.id;
        await this.opeAreasDescansoService
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
            console.error('Error', error);
          });
      } else {
        await this.opeAreasDescansoService
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
            console.log(error);
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
