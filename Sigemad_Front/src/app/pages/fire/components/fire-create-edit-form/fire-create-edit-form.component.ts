import { CommonModule } from '@angular/common';
import { Component, Inject, inject, OnInit, signal, ViewChild } from '@angular/core';
import { AbstractControl, FormControl, FormGroup, FormsModule, ReactiveFormsModule, Validators } from '@angular/forms';
import { EventEmitter, Input, Output } from '@angular/core';

import { CountryService } from '../../../../services/country.service';
import { EventService } from '../../../../services/event.service';
import { FireService } from '../../../../services/fire.service';
import { LocalFiltrosIncendio } from '../../../../services/local-filtro-incendio.service';
import { MunicipalityService } from '../../../../services/municipality.service';
import { ProvinceService } from '../../../../services/province.service';
import { TerritoryService } from '../../../../services/territory.service';
import { Countries } from '../../../../types/country.type';
import { Event } from '../../../../types/event.type';
import { Municipality } from '../../../../types/municipality.type';
import { Province } from '../../../../types/province.type';
import { Territory } from '../../../../types/territory.type';
import { Fire } from '../../../../types/fire.type';

import { FlexLayoutModule } from '@angular/flex-layout';
import { MatAutocompleteModule } from '@angular/material/autocomplete';
import { MatButtonModule } from '@angular/material/button';
import { DateAdapter, NativeDateAdapter, MAT_DATE_FORMATS, MAT_DATE_LOCALE } from '@angular/material/core';
import { MatDatepickerModule } from '@angular/material/datepicker';
import { MAT_DIALOG_DATA, MatDialog, MatDialogModule, MatDialogRef } from '@angular/material/dialog';
import { MatExpansionModule } from '@angular/material/expansion';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatIconModule } from '@angular/material/icon';
import { MatInputModule } from '@angular/material/input';
import { MatSelectModule } from '@angular/material/select';
import { Router } from '@angular/router';
import moment from 'moment-timezone';

import { NgxSpinnerModule, NgxSpinnerService } from 'ngx-spinner';
import Feature from 'ol/Feature';
import { Geometry } from 'ol/geom';
import { EventStatusService } from '../../../../services/eventStatus.service';
import { AlertService } from '../../../../shared/alert/alert.service';
import { TooltipDirective } from '../../../../shared/directive/tooltip/tooltip.directive';
import { FormFieldComponent } from '../../../../shared/Inputs/field.component';
import { MapCreateComponent } from '../../../../shared/mapCreate/map-create.component';
import { EventStatus } from '../../../../types/eventStatus.type';
import { MatSnackBar, MatSnackBarModule } from '@angular/material/snack-bar';
import { catchError, debounceTime, Observable, of, startWith, switchMap, merge } from 'rxjs';

// PCD
import { DragDropModule } from '@angular/cdk/drag-drop';
import { FechaValidator } from '../../../../shared/validators/fecha-validator';
import { FECHA_MAXIMA_DATETIME, FECHA_MINIMA_DATETIME } from '@type/constants';
import { MatCardModule } from '@angular/material/card';
// FIN PCD

import { FireNationalFormComponent } from './components/national/fire-national-form.component';
import { FireForeignFormComponent } from './components/foreign/fire-foreign-form.component';

const FORMATO_FECHA = {
  parse: {
    dateInput: 'LL',
  },
  display: {
    dateInput: 'LL',
    monthYearLabel: 'MMM YYYY',
    dateA11yLabel: 'LL',
    monthYearA11yLabel: 'MMMM YYYY',
  },
};

@Component({
  selector: 'fire-create-edit',
  standalone: true,
  imports: [
    CommonModule,
    ReactiveFormsModule,
    FormsModule,
    MatDialogModule,
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
    NgxSpinnerModule,
    TooltipDirective,
    DragDropModule,
    MatSnackBarModule,
    MatCardModule,
    FireNationalFormComponent,
    FireForeignFormComponent
  ],
  providers: [
    { provide: DateAdapter, useClass: NativeDateAdapter },
    { provide: MAT_DATE_FORMATS, useValue: FORMATO_FECHA },
  ],
  templateUrl: './fire-create-edit-form.component.html',
  styleUrl: './fire-create-edit-form.component.scss',
})
export class FireCreateEdit implements OnInit {
  @Input() data: { fire?: any } = { fire: null };

  constructor(
    private filtrosIncendioService: LocalFiltrosIncendio,
    private territoryService: TerritoryService,
    private provinceService: ProvinceService,
    private municipalityService: MunicipalityService,
    private eventService: EventService,
    private countryServices: CountryService,
    private fireService: FireService,
    public eventStatusService: EventStatusService,
    public dialogRef: MatDialogRef<FireCreateEdit>,
    private matDialog: MatDialog,
    public alertService: AlertService,
    private router: Router,
    private snackBar: MatSnackBar,
    @Inject(MAT_DIALOG_DATA) public dialogData: { fire?: any } = { fire: null }
  ) {
    this.data = dialogData;
  }
  originalMunicipalityId: any
  public selectedTerritory: number = 1;
  public showInputForeign: boolean = false;

  public territories = signal<Territory[]>([]);
  public provinces = signal<Province[]>([]);
  public municipalities = signal<Municipality[]>([]);
  public listClassEvent = signal<Event[]>([]);
  public countries = signal<Countries[]>([]);
  public foreignCountries = signal<Countries[]>([]);
  public listEventStatus = signal<EventStatus[]>([]);

  public length: number = 0;
  public latitude: number = 0;
  public municipalityName: string = '';

  public formData!: FormGroup;

  public today: string = new Date().toISOString().split('T')[0];

  public fechaMinimaDateTime = FECHA_MINIMA_DATETIME;
  public fechaMaximaDateTime = FECHA_MAXIMA_DATETIME;
  

  //MAP
  public coordinates = signal<any>({});
  public geometry = signal<any>([]);
  private spinner = inject(NgxSpinnerService);

  public provincefilteredOptions!: Observable<Province[]>;
  public municipalityfilteredOptions!: Observable<Municipality[]>;

  async ngOnInit() {
    this.formData = new FormGroup({
      territory: new FormControl('', Validators.required),
      classEvent: new FormControl(1, Validators.required),
      province: new FormControl('', Validators.required),
      municipality: new FormControl('', Validators.required),
      denomination: new FormControl('', Validators.required),
      

      startDateTime: new FormControl(
        this.data.fire?.fechaInicio
          ? moment(this.data.fire.fechaInicio).startOf('day').format('YYYY-MM-DDTHH:mm')
          : moment().startOf('day').format('YYYY-MM-DDTHH:mm'),
        [Validators.required, FechaValidator.validarFechaPosteriorHoy(), FechaValidator.validarFecha]
      ),

      
      eventStatus: new FormControl(1, Validators.required),
      generalNote: new FormControl(''),
      country: new FormControl(''),
      limitSpain: new FormControl(false),
      idMunicipioExtranjero: new FormControl(''),
      ubicacion: new FormControl(''),
    });

    // Si se recibe idTerritorio desde el modal, usarlo como valor por defecto
    if (this.data.fire?.idTerritorio) {
      this.formData.patchValue({
        territory: this.data.fire.idTerritorio
      });
      this.selectedTerritory = this.data.fire.idTerritorio;
    } else if (!this.data.fire?.id) {
      this.formData.patchValue({
        territory: 1,
        classEvent: 1,
        eventStatus: 1,
      });
      this.selectedTerritory = 1;
    }

    this.originalMunicipalityId = this.data.fire.idMunicipio; // Guarda el ID original
    
    this.initializeSearchProvince();
    this.initializeSearchMunicipality();

    if (this.data.fire?.id) {
      this.loadMunicipalities({ value: this.data.fire.idProvincia });

      this.formData.patchValue({
        id: this.data.fire.id,
        territory: this.data.fire.idTerritorio,
        denomination: this.data.fire.denominacion,


        startDateTime: moment(this.data.fire.fechaInicio).format('YYYY-MM-DDTHH:mm'),

        generalNote: this.data.fire.notaGeneral,
        classEvent: this.data.fire.idClaseSuceso,
        eventStatus: this.data.fire.idEstadoSuceso,
      });
      
      this.selectedTerritory = this.data.fire.idTerritorio;

      if (this.data.fire.provincia) {
        this.formData.get('province')?.setValue(this.data.fire.provincia);
      } else {
        this.formData.get('province')?.setValue(this.data.fire.idProvincia);
      }

      if (this.data.fire.municipio) {
        this.formData.get('municipality')?.setValue(this.data.fire.municipio);
      } else {
        this.formData.get('municipality')?.setValue(this.data.fire.idMunicipio);
      }

      if (this.data.fire.pais) {
        this.formData.get('country')?.setValue(this.data.fire.pais);
      } else {
        this.formData.get('country')?.setValue(this.data.fire.idPais);
      }

      this.geometry.set(this.data?.fire?.geoPosicion?.coordinates);
    }

    const territories = await this.territoryService.getForCreate();
    this.territories.set(territories);

    const provinces = await this.provinceService.get();
    this.provinces.set(provinces);

    const events = await this.eventService.get();
    this.listClassEvent.set(events);

    const countries = await this.countryServices.get();
    this.countries.set(countries);
    
    const foreignCountries = await this.countryServices.getExtranjeros();
    this.foreignCountries.set(foreignCountries);

    const listEventStatus = await this.eventStatusService.get();
    this.listEventStatus.set(listEventStatus);
    
    this.updateValidatorsBasedOnTerritory();

  }

  changeTerritory(event: any) {
    this.selectedTerritory = event.value;
    
    if (event.value == 1) {
      this.showInputForeign = false;
    }
    else if (event.value == 2) {
      this.showInputForeign = true;
    }
    
    this.updateValidatorsBasedOnTerritory();
  }
  
  updateValidatorsBasedOnTerritory() {
    const provinceControl = this.formData.get('province');
    const municipalityControl = this.formData.get('municipality');
    const countryControl = this.formData.get('country');
    const ubicationControl = this.formData.get('idMunicipioExtranjero');
    
    if (this.selectedTerritory === 1) {
      provinceControl?.setValidators([Validators.required]);
      municipalityControl?.setValidators([Validators.required]);
      
      countryControl?.clearValidators();
      ubicationControl?.clearValidators();
      
      countryControl?.setValue('');
      ubicationControl?.setValue('');
    } 
    else if (this.selectedTerritory === 2) {
      countryControl?.setValidators([Validators.required]);
      ubicationControl?.setValidators([Validators.required]);
      
      provinceControl?.clearValidators();
      municipalityControl?.clearValidators();
      
      provinceControl?.setValue('');
      municipalityControl?.setValue('');
    }
    
    provinceControl?.updateValueAndValidity();
    municipalityControl?.updateValueAndValidity();
    countryControl?.updateValueAndValidity();
    ubicationControl?.updateValueAndValidity();
  }

  async loadMunicipalities(event: any) {
    const province_id = event.value;
    const municipalities = await this.municipalityService.getBySearch('', province_id);
    this.municipalities.set(municipalities);
    this.formData.get('municipality')?.enable();
  }

  private checkExistingFire(municipalityId: number, fires: Fire[]): Fire | null {
    return fires.find((fire) => fire.idMunicipio === municipalityId && fire.estadoSuceso?.descripcion !== 'Cerrado') || null;
  }

  async onSubmit() {
    if (this.formData.valid) {
      this.spinner.show();
      const formValues = this.formData.value;
      
      let provinceId = null;
      let municipalityId = null;
      let countryId = null;
      
      if (this.selectedTerritory === 1) {
        provinceId = typeof formValues.province === 'object' ? formValues.province.id : formValues.province;
        municipalityId = typeof formValues.municipality === 'object' ? formValues.municipality.id : formValues.municipality;
      } else if (this.selectedTerritory === 2) {
        countryId = typeof formValues.country === 'object' ? formValues.country.id : formValues.country;
        
        if (formValues.limitSpain) {
          if (formValues.provinciaLimitrofe) {
            provinceId = typeof formValues.provinciaLimitrofe === 'object' ? formValues.provinciaLimitrofe.id : formValues.provinciaLimitrofe;
          }
          
          if (formValues.municipioLimitrofe) {
           
            municipalityId = typeof formValues.municipioLimitrofe === 'object' ? formValues.municipioLimitrofe.id : formValues.municipioLimitrofe;
          }
        }
      }
      console.log("🚀 ~ FireCreateEdit ~ onSubmit ~ formValues:", formValues)
      const data: {
        id?: number;
        territory: any;
        classEvent: any;
        eventStatus: any;
        startDateTime: any;
        denomination: any;
        generalNote: any;
        province: any;
        municipality: any;
        country: any;
        ubication: any;
        idMunicipioExtranjero: any;
        limitSpain: any;
        isLimitrofe?: boolean;
        geoposition: { type: string; coordinates: any };
      } = {
        territory: formValues.territory,
        classEvent: formValues.classEvent,
        eventStatus: formValues.eventStatus,

        startDateTime: formValues.startDateTime
        ? new Date(formValues.startDateTime).toISOString()
        : null,



        denomination: formValues.denomination,
        generalNote: formValues.generalNote,
        province: provinceId,
        municipality: municipalityId,
        country: countryId,
        ubication:typeof formValues.idMunicipioExtranjero === 'object' ? formValues.idMunicipioExtranjero.descripcion : 
                  (this.selectedTerritory != 1) ? formValues.idMunicipioExtranjero: null,
        limitSpain: formValues.limitSpain,
        idMunicipioExtranjero: formValues.idMunicipioExtranjero.id,
        geoposition: {
          type: 'Point',
          coordinates: this.geometry()[0] ?? [],
        }
      };
      
      if (this.selectedTerritory === 2) {
        data.isLimitrofe = formValues.limitSpain;
      }
      
      if (this.data.fire?.id) {
        data.id = this.data.fire.id;
      }
      
      if (this.selectedTerritory === 1) {
        const municipality = typeof formValues.municipality === 'object' 
          ? formValues.municipality 
          : this.municipalities().find((item) => item.id === formValues.municipality);

        try {
          const existingFires = await this.fireService.get({},1,15);
          const activeFire = this.checkExistingFire(municipalityId, existingFires.data);

          if (activeFire && !this.data.fire?.id) {
            this.spinner.hide();
            const formattedDate = moment(activeFire.fechaInicio).format('DD/MM/YYYY');
            this.alertService
              .showAlert({
                title: 'Ya existe un incendio sin cerrar en el municipio',
                html: `
                <div style="text-align: center;">
                  <p><strong>Denominación:</strong> ${activeFire.denominacion}</p>
                  <p><strong>Seguimiento:</strong> ${activeFire.estadoSuceso.descripcion}</p>
                  <p><strong>Fecha de inicio:</strong> ${formattedDate}</p>
                </div>
                <p>¿Desea continuar o cancelar?</p>
              `,
                icon: 'warning',
                showCancelButton: true,
                cancelButtonColor: '#d33',
                confirmButtonText: 'Continuar',
                cancelButtonText: 'Cancelar',
                customClass: {
                  title: 'sweetAlert-fsize20',
                },
              })
              .then(async (result) => {
                if (result.isConfirmed) {
                  this.spinner.show();
                  this.saveFireData(data);
                } else {
                  this.spinner.hide();
                  return;
                }
              });
          } else {
            this.saveFireData(data);
          }
        } catch (error) {
          this.spinner.hide();
          this.snackBar.open('Error al guardar el registro', 'Cerrar', {
            duration: 4000,
            horizontalPosition: 'center',
            verticalPosition: 'bottom',
            panelClass: ['snackbar-rojo'],
          });          
        }
      } else {
        this.saveFireData(data);
      }
    } else {
      this.formData.markAllAsTouched();
    }
  }
  
  async saveFireData(data: any) {
    console.log("🚀 ~ FireCreateEdit ~ saveFireData ~ data:", data)
    this.spinner.show();
    
    try {
      if (this.data.fire?.id) {
        const response = await this.fireService.update(data) as any;
        this.spinner.hide();
        this.dialogRef.close({ refresh: true });
        if (response && response.id) {
          this.redirectToMonitoringPanel(response);
        }
      } else {
        const response = await this.fireService.post(data) as any;
        this.spinner.hide();
        this.dialogRef.close({ refresh: true });
        this.redirectToMonitoringPanel(response);
      }
    } catch (error: any) {
      console.log(error);
      this.spinner.hide();
      
      let msg = this.data.fire?.id ? 'Error al modificar el incendio' : 'Error al crear el incendio';
      
      if (error?.Details) {
        try {
          const detalles = JSON.parse(error.Details);
          msg = Object.keys(detalles).map(key => {
            const mensajes = Array.isArray(detalles[key]) ? detalles[key].join(' ') : detalles[key];
            return `${key} : ${mensajes}`;
          }).join('\n');
        } catch (e) {
          console.error('Error al procesar detalles del error:', e);
        }
      }
      
      this.snackBar.open(msg, '', {
        duration: 4000,
        horizontalPosition: 'center',
        verticalPosition: 'bottom',
        panelClass: ['snackbar-rojo'],
      });
    }
  }


setMunicipalityId(event: any, op: any) {
  if (typeof event === 'object' && event.value) {
    if (event.value.id !== this.originalMunicipalityId) {
      const selectedItem = op.find((item: any) => item.id === event.value);
      this.formData.patchValue({
        denomination: selectedItem?.descripcion || '',
      });

    }
  } else if (typeof event === 'object') {
    if (event.id !== this.originalMunicipalityId) {
      this.formData.patchValue({
        denomination: event.descripcion,
      });
        this.originalMunicipalityId = 0
    }
  }

  this.geometry.set([]);
}


  redirectToMonitoringPanel(response: any) {
    this.router.navigate([`/fire/fire-national-edit/${response.id}`]);
  }

  openModalMap() {
    if (!this.formData.value.municipality) {
      return;
    }
    const municipioSelected = typeof this.formData.value.municipality === 'object' 
      ? this.formData.value.municipality 
      : this.municipalities().find((item) => item.id == this.formData.value.municipality);

    if (!municipioSelected) {
      return;
    }

    const dialogRef = this.matDialog.open(MapCreateComponent, {
      width: '100%',
      maxWidth: '75vw',
      height: '100%',
      maxHeight: '80vh',
      disableClose: true,
      data: {
        municipio: municipioSelected,
        centroideMunicipio: true,
        onlyView: false,
        listaMunicipios: this.municipalities(),
        defaultPolygon: this.geometry(),
        close: true,
        showSearchCoordinates: true,
      },
    });

    dialogRef.componentInstance.save.subscribe((features: Feature<Geometry>[]) => {
      this.geometry.set(features);
    });
  }

  closeModal(params?: any) {
    this.dialogRef.close(params);
  }

  getForm(atributo: string): any {
    return this.formData.controls[atributo];
  }

  initializeSearchMunicipality() {
    this.municipalityfilteredOptions = merge(
      this.formData.get('municipality')?.valueChanges ?? of<string>(''),
      this.formData.get('province')?.valueChanges ?? of<any>(null)
    ).pipe(
      startWith(''),
      debounceTime(500),
      switchMap(async () => {
        const municipalityValue = this.formData.get('municipality')?.value;
        const province = this.formData.get('province')?.value;
        const idProvincia = province && typeof province === 'object' ? province.id : province;
        
        if (typeof municipalityValue === 'string' && municipalityValue.trim().length >= 3) {
          return await this.municipalityService.getBySearch(municipalityValue, idProvincia);
        } else if (idProvincia) {
          return await this.municipalityService.getBySearch('', idProvincia);
        } else {
          return await this.municipalityService.getBySearch();
        }
      }),
      catchError(() => of<Municipality[]>([]))
    );

    this.formData.get('municipality')?.valueChanges.subscribe(selectedMunicipality => {
      if (selectedMunicipality && typeof selectedMunicipality === 'object') {
        this.municipalityName = selectedMunicipality.descripcion;
        this.setMunicipalityId(selectedMunicipality, []);
        
        if (selectedMunicipality.provincia) {
          const currentProvince = this.formData.get('province')?.value;
          const currentProvinceId = typeof currentProvince === 'object' ? currentProvince.id : currentProvince;
          
          if (!currentProvinceId || currentProvinceId !== selectedMunicipality.provincia.id) {
            this.formData.get('province')?.setValue(selectedMunicipality.provincia);
          }
        }
      }
    });
  }

  displayMunicipality = (municipality: Municipality): string => {
    return municipality && municipality.descripcion ? municipality.descripcion : '';
  }

  initializeSearchProvince() {
    this.provincefilteredOptions = (this.formData.get('province')?.valueChanges ?? of<string>('')).pipe(
      startWith(''),
      debounceTime(500),
      switchMap(async value => {
        if (typeof value === 'string' && value.trim().length >= 3) {
          return await this.provinceService.getBySearch(value);
        } else if (!value) {
          return await this.provinceService.getBySearch();
        } else {
          return [];
        }
      }),
      catchError(() => of<Province[]>([]))
    );

    let isProvinceUpdateFromMunicipality = false;

    this.formData.get('municipality')?.valueChanges.subscribe(selectedMunicipality => {
      if (selectedMunicipality && typeof selectedMunicipality === 'object' && selectedMunicipality.provincia) {
        isProvinceUpdateFromMunicipality = true;
      }
    });

    this.formData.get('province')?.valueChanges.subscribe(selectedProvince => {
      if (selectedProvince && typeof selectedProvince === 'object') {
        this.formData.get('municipality')?.enable();
        
        if (!isProvinceUpdateFromMunicipality) {
          this.formData.get('municipality')?.setValue('');
        }
        
        isProvinceUpdateFromMunicipality = false;
      } else if (!selectedProvince) {
        this.formData.get('municipality')?.setValue('');
        isProvinceUpdateFromMunicipality = false;
      }
    });
  }

  displayProvince = (province: Province): string => {
    return province && province.descripcion ? province.descripcion : '';
  }

  handleMapOpen() {
    this.openModalMap();
  }
}
