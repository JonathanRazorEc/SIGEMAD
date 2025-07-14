import { AsyncPipe, CommonModule } from '@angular/common';
import { Component, EventEmitter, inject, Input, Output, signal, ViewChild } from '@angular/core';
import { FlexLayoutModule } from '@angular/flex-layout';
import { FormBuilder, FormControl, FormGroup, FormGroupDirective, ReactiveFormsModule, Validators } from '@angular/forms';
import { MatButtonModule } from '@angular/material/button';
import { DateAdapter, MAT_DATE_FORMATS, MatNativeDateModule, NativeDateAdapter } from '@angular/material/core';
import { MatDatepickerModule } from '@angular/material/datepicker';
import { MAT_DIALOG_DATA, MatDialog } from '@angular/material/dialog';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatGridListModule } from '@angular/material/grid-list';
import { MatIconModule } from '@angular/material/icon';
import { MatInputModule } from '@angular/material/input';
import { MatSelectModule } from '@angular/material/select';
import { MatSnackBar } from '@angular/material/snack-bar';
import { MatSort } from '@angular/material/sort';
import { MatTableDataSource, MatTableModule } from '@angular/material/table';
import moment from 'moment';
import { NgxSpinnerModule, NgxSpinnerService } from 'ngx-spinner';
import Feature from 'ol/Feature';
import { Geometry } from 'ol/geom';
import { EvolutionService } from '../../../services/evolution.service';
import { DireccionesService } from '../../../services/direcciones.service';
import { MunicipalityService } from '../../../services/municipality.service';
import { ProvinceService } from '../../../services/province.service';
import { MapCreateComponent } from '../../../shared/mapCreate/map-create.component';
import { Municipality } from '../../../types/municipality.type';
import { Province } from '../../../types/province.type';
import { SavePayloadModal } from '../../../types/save-payload-modal';
import { BaseItem } from '@type/base-item.type';
import { HttpClient } from '@angular/common/http';
import { InterventionDataService, InterventionTableItem } from '../../../services/intervention-data.service';
import { RecordsService } from '@services/records.service';
import { MatAutocompleteModule, MatAutocompleteSelectedEvent } from '@angular/material/autocomplete';
import { map, Observable, of, startWith } from 'rxjs';
import { DateUtils } from '@shared/utils/date-utils';
import { MatTooltipModule } from '@angular/material/tooltip';

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
  selector: 'app-intervention',
  standalone: true,
  imports: [
    ReactiveFormsModule,
    MatFormFieldModule,
    MatDatepickerModule,
    MatNativeDateModule,
    CommonModule,
    MatInputModule,
    FlexLayoutModule,
    MatGridListModule,
    MatButtonModule,
    MatSelectModule,
    MatTableModule,
    MatIconModule,
    NgxSpinnerModule,
    MatAutocompleteModule,
    AsyncPipe,
    MatTooltipModule,
  ],
  templateUrl: './intervention.component.html',
  styleUrl: './intervention.component.scss',
  providers: [
    { provide: DateAdapter, useClass: NativeDateAdapter },
    { provide: MAT_DATE_FORMATS, useValue: FORMATO_FECHA },
  ],
})
export class InterventionComponent {
  @ViewChild(MatSort) sort!: MatSort;
  @Output() save = new EventEmitter<SavePayloadModal>();
  @Output() changesMade = new EventEmitter<boolean>();
  @Output() hasUnsavedChanges = new EventEmitter<boolean>();
  @Input() editData: any;
  @Input() esUltimo: boolean | undefined;
  @Input() dataMaestros: any;
  @Input() fire: any;
  @Input() isNewRecord: boolean | undefined;
  @Input() registroId: number | null = null;
  @Input() showButton: boolean = true;

  data = inject(MAT_DIALOG_DATA) as { title: string; idIncendio: number };

  public polygon = signal<any>([]);

  public direcionesServices = inject(DireccionesService);
  public evolutionService = inject(EvolutionService);
  public toast = inject(MatSnackBar);

  private fb = inject(FormBuilder);
  public matDialog = inject(MatDialog);
  private spinner = inject(NgxSpinnerService);
  private provinceService = inject(ProvinceService);
  private municipalityService = inject(MunicipalityService);
  private http = inject(HttpClient);
  private interventionDataService = inject(InterventionDataService);
  private recordsService = inject(RecordsService);

  public get interventionTableData() {
    return this.interventionDataService.interventionTableData;
  }

  public displayedColumns: string[] = [
    'tipoIntervencionMedios',
    'numeroCapacidades',
    'caracterMedios',
    'titularidadMedio',
    'fechaInicio',
    'fechaFin',
    'opciones',
  ];

  formDataIntervencion!: FormGroup;

  public isCreate = signal<number>(-1);
  public provinces = signal<Province[]>([]);
  public municipalities = signal<Municipality[]>([]);
  public caracterMedios = signal<BaseItem[]>([]);
  public tipoIntervencionMedios = signal<BaseItem[]>([]);
  public filteredIntervencionMedios: Observable<BaseItem[]> = of([]);
  public titularMedios = signal<BaseItem[]>([]);
  public dataSource = new MatTableDataSource<any>([]);
  public readonly ID_TIPO_OTROS = 92;
  public mostrarTablaDetalles = false;
  public mediosCapacidades = signal<any[]>([]);
  public columnasTablaDetalles: string[] = ['descripcion', 'tipoMedio', 'numeroMedio', 'intervinientes'];
  public datosTablaDetalles: any[] = [];
  public showForm = false;

  async resetForm() {
    this.spinner.show();
    this.formDataIntervencion.reset({
      caracterMedios: '',
      tipoIntervencionMedios: '',
      descripcion: { value: '', disabled: true },
      medioNoCatalogado: '',
      numeroCapacidades: 1,
      titularidadMedio: '',
      titular: '',
      provincia: '',
      municipio: '',
      fechaInicio: this.getCurrentDateTimeString(),
      fechaFin: '',
      observaciones: '',
    });

    if (this.esTerritorioNacional()){

    const defaultProvincia = this.provinces().find((provincia) => provincia.id === this.fire.idProvincia);
    console.log('🚀 ~ CecopiComponent ~ ngOnInit ~ this.fire:', this.fire);
    this.formDataIntervencion.get('provincia')?.setValue(defaultProvincia);

    const municipalities = await this.municipalityService.get(this.fire.idProvincia);
    this.municipalities.set(municipalities); };


    const defaultMuni = this.municipalities().find((muni) => muni.id === this.fire.idMunicipio);
    this.formDataIntervencion.get('municipio')?.setValue(defaultMuni);

    this.mostrarTablaDetalles = false;
    this.datosTablaDetalles = [];
    this.showForm = true;
    this.spinner.hide();

    // No emitimos los eventos aquí, solo mostramos el formulario
  }

  async ngOnInit() {
    this.spinner.show();

    // Mostrar si es un nuevo registro o edición
    console.log('🚀 ~ InterventionComponent ~ ¿Es nuevo registro?:', this.isNewRecord);
    console.log('🚀 ~ InterventionComponent ~ registroId:', this.registroId);

    const provinces = await this.provinceService.get();
    this.provinces.set(provinces);
    this.formDataIntervencion = this.fb.group({
      caracterMedios: ['', Validators.required],
      tipoIntervencionMedios: ['', Validators.required],
      descripcion: [{ value: '', disabled: true }],
      medioNoCatalogado: [''],
      numeroCapacidades: [1, [Validators.required, Validators.min(1)]],
      titularidadMedio: ['', Validators.required],
      titular: [''],
      provincia: [''],
      municipio: [''],
      fechaInicio: [this.getCurrentDateTimeString(), Validators.required],
      fechaFin: [''],
      observaciones: [''],
    });

    const provinciaControl = this.formDataIntervencion.get('provincia');
   const municipioControl = this.formDataIntervencion.get('municipio');


    if (this.esTerritorioNacional()){
      provinciaControl?.setValidators(Validators.required);
   municipioControl?.setValidators(Validators.required);

    }else{
      provinciaControl?.clearValidators();
      municipioControl?.clearValidators();
    }

    const caracterMedios = await this.evolutionService.getCaracterMedios();
    this.caracterMedios.set(caracterMedios);

    const tipoIntervencionMedios = await this.evolutionService.getTipoIntervencionMedios();
    this.tipoIntervencionMedios.set(tipoIntervencionMedios);

    const titularMedios = await this.evolutionService.getTitularMedios();
    this.titularMedios.set(titularMedios);

    try {
      const registroData = await this.recordsService.getById(Number(this.registroId));
      console.log('Datos del registro:', registroData);

      // Si no hay datos de intervención o está vacío, intentamos obtener registros anteriores
      if (!registroData?.intervencionMedios || registroData.intervencionMedios.length === 0) {
        const registrosAnteriores = await this.recordsService.getRegistrosAnteriores(Number(this.fire.idSuceso), this.registroId!);
        console.log('Datos de registros anteriores:', registrosAnteriores);

        if (registrosAnteriores[0] && registrosAnteriores[0].intervencionMedios?.length > 0) {
          const datosFormateados = this.formatearDatosIntervencion(registrosAnteriores[0].intervencionMedios, true);
          this.interventionDataService.setInterventionData(datosFormateados);

          const interventionData = {
            IdRegistroActualizacion: this.registroId || 0,
            IdSuceso: this.fire.id || 0,
            Intervenciones: registrosAnteriores[0].intervencionMedios,
          };
          this.evolutionService.dataIntervention.set(interventionData);
        }
      } else {
        // Si hay datos de intervención, los usamos
        const datosFormateados = this.formatearDatosIntervencion(registroData.intervencionMedios, false);
        this.interventionDataService.setInterventionData(datosFormateados);

        const interventionData = {
          IdRegistroActualizacion: this.registroId || 0,
          IdSuceso: this.fire.id || 0,
          Intervenciones: registroData.intervencionMedios,
        };
        this.evolutionService.dataIntervention.set(interventionData);
      }
    } catch (error) {
      console.error('Error al cargar los datos:', error);
      this.toast.open('Error al cargar los datos', 'Cerrar', { duration: 3000 });
    }

    this.spinner.hide();

    this.actualizarValidacionDescripcion();
    this.actualizarValidacionMedioNoCatalogado();
    this.onFilterTipoIntervencionMedios();
  }

  private onFilterTipoIntervencionMedios() {
    this.filteredIntervencionMedios = (this.getFormIntervencion('tipoIntervencionMedios') as FormControl).valueChanges.pipe(
      startWith(''),
      map((value) => {
        const nombre = typeof value === 'string' ? value : value?.nombre;
        return this._filterTipoIntervencion(nombre || '');
      })
    );
  }

  private _filterTipoIntervencion(value: string): BaseItem[] {
    const filterValue = value.toLowerCase();

    return this.tipoIntervencionMedios().filter((option) => option.nombre.toLowerCase().includes(filterValue));
  }

  displayTipoIntervencion(option: BaseItem): string {
    return option && option.nombre ? option.nombre : '';
  }

  onSubmitIntervencion(formDirective: FormGroupDirective): void {
    if (this.formDataIntervencion.valid) {
      const formValue = this.formDataIntervencion.getRawValue();

      const newItem: InterventionTableItem = {
        ...formValue,
        caracterMedios: formValue.caracterMedios,
        tipoIntervencionMedios: formValue.tipoIntervencionMedios,
        titularidadMedio: formValue.titularidadMedio,
        provincia:this.esTerritorioNacional()? formValue.provincia:null,
        municipio: this.esTerritorioNacional() ?formValue.municipio:null,
        fechaInicio: formValue.fechaInicio ? DateUtils.fromCestToUtc(formValue.fechaInicio) : null,
        fechaFin: formValue.fechaFin ? DateUtils.fromCestToUtc(formValue.fechaFin) : null,
        detalleIntervencionMedios: [...this.datosTablaDetalles],
      };

      if (this.isCreate() === -1) {
        newItem.geoPosicion = {
          type: 'Polygon',
          coordinates: [this.polygon()],
        };

        this.interventionDataService.addInterventionItem(newItem);
      } else {
        const currentData = this.interventionTableData();
        newItem.id = currentData[this.isCreate()].id;
        newItem.geoPosicion = currentData[this.isCreate()].geoPosicion;

        this.interventionDataService.updateInterventionItem(newItem, this.isCreate());

        this.isCreate.set(-1);
      }

      this.actualizarDataIntervention();

      // Emitimos los eventos aquí, después de agregar o editar el registro
      // Esto habilitará el botón Guardar en el componente padre
      this.hasUnsavedChanges.emit(false); // No hay cambios sin guardar en el formulario
      this.changesMade.emit(true); // Pero sí hay cambios en los datos

      formDirective.resetForm();
      this.formDataIntervencion.reset({
        fechaInicio: new Date(),
        numeroCapacidades: 1,
      });
      this.datosTablaDetalles = [];
      this.mostrarTablaDetalles = false;
      
    if (this.esTerritorioNacional()){
      const defaultProvincia = this.provinces().find((provincia) => provincia.id === this.fire.idProvincia);
      this.formDataIntervencion.get('provincia')?.setValue(defaultProvincia);
      this.loadMunicipalities(defaultProvincia).then(() => {
        const defaultMuni = this.municipalities().find((muni) => muni.id === this.fire.idMunicipio);
        this.formDataIntervencion.get('municipio')?.setValue(defaultMuni);
      });
    }

      this.showForm = false;
    } else {
      this.formDataIntervencion.markAllAsTouched();
    }
  }

  async sendDataToEndpoint() {
    this.actualizarDataIntervention();

    if (this.interventionTableData().length > 0 && !this.editData) {
      this.save.emit({ save: true, delete: false, close: false, update: false });
    } else if (this.editData) {
      this.save.emit({ save: false, delete: false, close: false, update: true });
    }
  }

  async loadMunicipalities(event: any) {
    const province_id = event?.value?.id ?? event.id;
    const municipalities = await this.municipalityService.get(province_id);
    this.municipalities.set(municipalities);
    this.formDataIntervencion.get('municipio')?.enable();
  }

  onChangeMunicipio(event: any) {
    this.polygon.set([]);
  }

  openModalMap() {
    if (!this.formDataIntervencion.value.municipio) {
      return;
    }
    const municipioSelected = this.municipalities().find((item) => item.id == this.formDataIntervencion.value.municipio.id);

    if (!municipioSelected) {
      return;
    }

    const dialogRef = this.matDialog.open(MapCreateComponent, {
      width: '780px',
      maxWidth: '780px',
      height: '780px',
      maxHeight: '780px',
      disableClose: true,
      data: {
        municipio: municipioSelected,
        listaMunicipios: this.municipalities(),
        defaultPolygon: this.polygon(),
      },
    });

    dialogRef.componentInstance.save.subscribe((features: Feature<Geometry>[]) => {
      this.polygon.set(features);
    });
  }

  // editarItemCecopi(index: number) {
  //   const dataEditada = this.formDataIntervencion.value;
  //   const currentData = this.interventionTableData();
  //   const updatedItem = { ...currentData[index], ...dataEditada };

  //   this.interventionDataService.updateInterventionItem(updatedItem);

  //   this.isCreate.set(-1);
  //   this.formDataIntervencion.reset();
  // }

  eliminarItemCecopi(index: number) {
    const currentData = this.interventionTableData();
    const itemId = currentData[index]?.id;

    if (itemId) {
      this.interventionDataService.removeInterventionItem(itemId);
    } else {
      this.interventionDataService.interventionTableData.update((data) => {
        data.splice(index, 1);
        return [...data];
      });
    }

    if (this.isCreate() === index) {
      this.isCreate.set(-1);
      this.formDataIntervencion.reset();
    }

    this.actualizarDataIntervention();

    // Emitimos los eventos aquí, después de eliminar un registro
    // Esto habilitará el botón Guardar en el componente padre
    this.hasUnsavedChanges.emit(false); // No hay cambios sin guardar en el formulario
    this.changesMade.emit(true); // Pero sí hay cambios en los datos
  }

  async seleccionarItemCecopi(index: number) {
    this.spinner.show();
    this.isCreate.set(index);
    const data = this.interventionTableData()[index];

    // Formatear la fecha de inicio para el input datetime-local si existe
    const fechaInicioFormatted = data.fechaInicio ? DateUtils.fromUtcToCest(data.fechaInicio) : null;
    const fechaFinFormatted = data.fechaFin ? DateUtils.fromUtcToCest(data.fechaFin) : null;

    this.formDataIntervencion.patchValue({
      caracterMedios: data.caracterMedios,
      tipoIntervencionMedios: data.tipoIntervencionMedios,
      descripcion: data.descripcion,
      medioNoCatalogado: data.medioNoCatalogado,
      numeroCapacidades: data.numeroCapacidades,
      titularidadMedio: data.titularidadMedio,
      titular: data.titular,
      provincia: data.provincia,
      municipio: data.municipio,
      fechaInicio: fechaInicioFormatted,
      fechaFin: fechaFinFormatted,
      observaciones: data.observaciones,
    });

    let tipoIntervencionSeleccionado = null;
    if (data.tipoIntervencionMedios) {
      tipoIntervencionSeleccionado = this.tipoIntervencionMedios().find((t) => t.id === data.tipoIntervencionMedios?.id);

      if (!tipoIntervencionSeleccionado && data.tipoIntervencionMedios.nombre) {
        tipoIntervencionSeleccionado = this.tipoIntervencionMedios().find((t) => t.nombre === data.tipoIntervencionMedios?.nombre);
      }
      this.showForm = true;
      console.log('Tipo intervención seleccionado:', tipoIntervencionSeleccionado);
    }

    this.formDataIntervencion.patchValue({
      ...data,
      id: data.id || 0,
      caracterMedios: this.caracterMedios().find((c) => c.id === data.caracterMedios?.id) || null,
      tipoIntervencionMedios: tipoIntervencionSeleccionado,
      titularidadMedio: this.titularMedios().find((t) => t.id === data.titularidadMedio?.id) || null,
      provincia: this.provinces().find((p) => p.id === data.provincia?.id) || null,
    });

    if (tipoIntervencionSeleccionado) {
      await this.handleTipoIntervencionChange({ value: tipoIntervencionSeleccionado });
    }

    if (data.fechaFin) {
      this.formDataIntervencion.get('fechaFin')?.setValue(fechaFinFormatted);
    }

    if (data.fechaInicio) {
      this.formDataIntervencion.get('fechaInicio')?.setValue(fechaInicioFormatted);
    }
      
    if (this.esTerritorioNacional()){    
      if (data.provincia) {
        await this.loadMunicipalities(data.provincia);
        const selectedMunicipio = this.municipalities().find((m) => m.id === data.municipio?.id);
        this.formDataIntervencion.get('municipio')?.setValue(selectedMunicipio || null);
      } else {
        this.municipalities.set([]);
        this.formDataIntervencion.get('municipio')?.setValue(null);
        this.formDataIntervencion.get('municipio')?.disable();
      }
    }

    if (data.detalleIntervencionMedios && data.detalleIntervencionMedios.length > 0) {
      this.datosTablaDetalles = [...data.detalleIntervencionMedios];
      this.mostrarTablaDetalles = true;
    }

    this.actualizarValidacionDescripcion();
    this.actualizarValidacionMedioNoCatalogado();

    if (data.geoPosicion) {
      this.polygon.set(data.geoPosicion.coordinates[0]);
    } else {
      this.polygon.set([]);
    }
    
    // Marcamos el formulario como "pristine" después de cargar los datos
    this.formDataIntervencion.markAsPristine();
    
    // No emitimos los eventos aquí, solo mostramos el formulario con los datos del registro
    this.spinner.hide();
  }

  getFormatdate(date: any) {
    if (!date) return '-'; // Retornar guion si la fecha es nula o indefinida
    // return moment(date).format('DD/MM/YYYY HH:mm');
    return DateUtils.fromUtcToCest(date, 'DD/MM/YYYY HH:mm');
  }

  getFormIntervencion(atributo: string): any {
    return this.formDataIntervencion.controls[atributo];
  }

  trackByFn(index: number, item: any): number {
    return item.id;
  }

  closeModal() {
    this.save.emit({ save: false, delete: false, close: true, update: false });
  }

  delete() {
    this.save.emit({ save: true, delete: false, close: false, update: false });
  }

  public mostrarCampoMedioNoCatalogado(): boolean {
    const tipoIntervencionMedios = this.getFormIntervencion('tipoIntervencionMedios').value;
    return tipoIntervencionMedios && tipoIntervencionMedios.id === this.ID_TIPO_OTROS;
  }

  public tieneIntervencionMedioSeleccionado(): boolean {
    const tipoIntervencionMedios = this.getFormIntervencion('tipoIntervencionMedios').value;
    return !!tipoIntervencionMedios && !!tipoIntervencionMedios.id;
  }

  private actualizarValidacionDescripcion(): void {
    const descripcionControl = this.getFormIntervencion('descripcion');
    if (this.tieneIntervencionMedioSeleccionado() && !this.mostrarCampoMedioNoCatalogado()) {
      descripcionControl.setValidators([Validators.required]);
    } else {
      descripcionControl.clearValidators();
    }
    descripcionControl.updateValueAndValidity();
  }

  private actualizarValidacionMedioNoCatalogado(): void {
    const medioNoCatalogadoControl = this.getFormIntervencion('medioNoCatalogado');
    if (this.mostrarCampoMedioNoCatalogado()) {
      medioNoCatalogadoControl.setValidators([Validators.required]);
    } else {
      medioNoCatalogadoControl.clearValidators();
      medioNoCatalogadoControl.setValue('');
    }
    medioNoCatalogadoControl.updateValueAndValidity();
  }

  actualizarIntervinientes(index: number, valor: number): void {
    if (this.datosTablaDetalles[index]) {
      this.datosTablaDetalles[index].intervinientes = valor;
    }
  }

  onTipoIntervencionChange(event: MatAutocompleteSelectedEvent): void {
    this.handleTipoIntervencionChange(event.option);
  }

  async handleTipoIntervencionChange(event: any): Promise<void> {
    console.log(event);
    if (!event || !event.value) {
      // Si no hay valor seleccionado, deshabilitar el campo descripción
      const descripcionControl = this.getFormIntervencion('descripcion');
      descripcionControl.disable();
      return;
    }

    this.spinner.show();
    const tipoSeleccionado = event.value;

    try {
      const descripcionControl = this.getFormIntervencion('descripcion');
      const medioNoCatalogadoControl = this.getFormIntervencion('medioNoCatalogado');
      const titularidadControl = this.getFormIntervencion('titularidadMedio');
      const titularControl = this.getFormIntervencion('titular');

      if (tipoSeleccionado && tipoSeleccionado.id) {
        try {
          const response = await this.evolutionService.getMediosCapacidades(tipoSeleccionado.tipoCapacidad.id);
          if (response && response.length > 0) {
            console.log('🚀 ~ InterventionComponent ~ handleTipoIntervencionChange ~ response:', response);
            this.mediosCapacidades.set(response);

            this.datosTablaDetalles = response.map((item: any) => ({
              descripcion: item.descripcion || 'Sin descripción',
              tipoMedio: item.tipoMedio?.nombre || 'Sin tipo',
              numeroMedio: item.numeroMedio || 0,
              intervinientes: item.numeroMedio || 0,
              id: item.id || 0,
            }));

            this.mostrarTablaDetalles = true;
          } else {
            this.mediosCapacidades.set([]);
            this.datosTablaDetalles = [];
            this.mostrarTablaDetalles = false;
          }
        } catch (error) {
          console.error('Error al obtener medios-capacidades:', error);
          this.mediosCapacidades.set([]);
          this.datosTablaDetalles = [];
          this.mostrarTablaDetalles = false;
          this.spinner.hide();
        }

        // Configurar campo descripción basado en el tipo seleccionado
        if (tipoSeleccionado.id === this.ID_TIPO_OTROS) {
          // Para el tipo "Otros"
          descripcionControl.clearValidators();
          descripcionControl.setValue('');
          descripcionControl.disable();

          medioNoCatalogadoControl.setValidators([Validators.required]);
          medioNoCatalogadoControl.updateValueAndValidity();

          // Deshabilitar dimensionamiento de capacidad para "Otros"
          this.mostrarTablaDetalles = false;
          this.datosTablaDetalles = [];
        } else {
          // Para otros tipos
          descripcionControl.enable();
          descripcionControl.setValidators([Validators.required]);
          descripcionControl.setValue(tipoSeleccionado.descripcion || '');

          medioNoCatalogadoControl.clearValidators();
          medioNoCatalogadoControl.setValue('');
          medioNoCatalogadoControl.updateValueAndValidity();
        }

        descripcionControl.updateValueAndValidity();

        // Configurar titularidad y titular basados en el tipo seleccionado
        if (tipoSeleccionado.id !== this.ID_TIPO_OTROS) {
          const tipoAdmin = tipoSeleccionado?.entidad?.organismo?.administracion?.tipoAdministracion;
          if (tipoAdmin && tipoAdmin.id) {
            const titularidadEncontrada = this.titularMedios().find((t) => t.id === tipoAdmin.id);

            if (titularidadEncontrada) {
              titularidadControl.setValue(titularidadEncontrada);
            } else {
              titularidadControl.setValue('');
            }
          } else {
            titularidadControl.setValue('');
          }

          if (tipoSeleccionado.clasificacionMedio && tipoSeleccionado.clasificacionMedio.descripcion) {
            titularControl.setValue(tipoSeleccionado.clasificacionMedio.descripcion);
          } else {
            titularControl.setValue(tipoSeleccionado?.entidad?.nombre || '');
          }
        } else {
          titularControl.setValue('');
          titularidadControl.setValue('');
        }

        titularidadControl.updateValueAndValidity();
        titularControl.updateValueAndValidity();

        // Actualizar las validaciones generales
        this.actualizarValidacionDescripcion();
        this.actualizarValidacionMedioNoCatalogado();
      }
    } catch (error) {
      console.error('Error general en handleTipoIntervencionChange:', error);
    } finally {
      this.spinner.hide();
    }
  }

  toggleTablaDetalles(): void {
    this.mostrarTablaDetalles = !this.mostrarTablaDetalles;
  }

  private actualizarDataIntervention(): void {
    const interventionData = {
      IdSuceso: this.fire.id || 1,
      Intervenciones: this.interventionTableData().map((item) => {
        console.log('🚀 ~ InterventionComponent ~ ID del elemento:', item.id);

        // Formatear fechas preservando la hora local exacta
        let fechaHoraInicio = '';
        if (item.fechaInicio) {
          if (typeof item.fechaInicio === 'string') {
            // Ya es string, solo asegurarnos que tenga formato ISO completo
            fechaHoraInicio = (item.fechaInicio as string).includes('Z') ? (item.fechaInicio as string) : (item.fechaInicio as string) + ':00.000Z';
          } else {
            // Es un objeto Date, formatear preservando la hora local exacta
            const d = item.fechaInicio as Date;
            fechaHoraInicio = `${d.getFullYear()}-${String(d.getMonth() + 1).padStart(2, '0')}-${String(d.getDate()).padStart(2, '0')}T${String(d.getHours()).padStart(2, '0')}:${String(d.getMinutes()).padStart(2, '0')}:00.000Z`;
          }
        } else {
          fechaHoraInicio = new Date().toISOString();
        }

        let fechaHoraFin = '';
        if (item.fechaFin) {
          if (typeof item.fechaFin === 'string') {
            fechaHoraFin = (item.fechaFin as string).includes('Z') ? (item.fechaFin as string) : (item.fechaFin as string) + ':00.000Z';
          } else {
            const d = item.fechaFin as Date;
            fechaHoraFin = `${d.getFullYear()}-${String(d.getMonth() + 1).padStart(2, '0')}-${String(d.getDate()).padStart(2, '0')}T${String(d.getHours()).padStart(2, '0')}:${String(d.getMinutes()).padStart(2, '0')}:00.000Z`;
          }
        }

        return {
          id: item.id || 0,
          idCaracterMedio: item.caracterMedios?.id || 0,
          descripcion: item.descripcion || '',
          medioNoCatalogado: item.medioNoCatalogado || '',
          numeroCapacidades: item.numeroCapacidades || 0,
          idTitularidadMedio: item.titularidadMedio?.id || 0,
          titular: item.titular || '',
          fechaHoraInicio: fechaHoraInicio,
          fechaHoraFin: fechaHoraFin,
          idProvincia: item.provincia?.id || null,
          idMunicipio: item.municipio?.id || null,
          observaciones: item.observaciones || '',
          idCapacidad: item.tipoIntervencionMedios?.id || 0,
          detalleIntervencionMedios: (item.detalleIntervencionMedios || []).map((detalle) => {
            console.log('🚀 ~ InterventionComponent ~ Intervenciones:this.interventionTableData ~ detalle:', detalle);
            return {
              idMediosCapacidad: detalle.id || 0,
              numeroIntervinientes: Number(detalle.intervinientes) || 0,
            };
          }),
        };
      }),
    };

    const interventionDataFormatted: any = interventionData;

    console.info('Datos transformados para enviar al backend:', interventionDataFormatted);

    this.evolutionService.dataIntervention.set(interventionDataFormatted);
    console.log('🚀 this.evolutionService.dataIntervention:', this.evolutionService.dataIntervention());
  }

  private getCurrentDateTimeString(): string {
    return DateUtils.getCurrentCESTDate();
  }

  cancel() {
    this.showForm = false;
    this.changesMade.emit(false);
    this.hasUnsavedChanges.emit(false);
  }

  private formatearDatosIntervencion(datos: any[], esNuevo: boolean = false) {
    return datos.map((item: any) => ({
      id: esNuevo ? 0 : item.id,
      caracterMedios: {
        id: item.caracterMedio?.id,
        descripcion: item.caracterMedio?.descripcion,
        nombre: item.caracterMedio?.descripcion || '',
      },
      tipoIntervencionMedios: item.capacidad
        ? {
            id: item.capacidad?.id,
            nombre: item.capacidad?.nombre,
            descripcion: item.capacidad?.descripcion,
          }
        : item.tipoIntervencionMedios || null,
      descripcion: item.descripcion || '',
      medioNoCatalogado: item.medioNoCatalogado || false,
      numeroCapacidades: item.numeroCapacidades || 0,
      titularidadMedio: {
        id: item.titularidadMedio?.id,
        nombre: item.titularidadMedio?.nombre,
        descripcion: item.titularidadMedio?.descripcion,
      },
      titular: item.titular || '',
      fechaInicio: item.fechaHoraInicio ? item.fechaHoraInicio.substring(0, 16) : '',
      fechaFin: item.fechaHoraFin ? item.fechaHoraFin.substring(0, 16) : '',
      provincia: {
        id: item.provincia?.id,
        descripcion: item.provincia?.descripcion,
        idCcaa: item.provincia?.idCcaa || 0,
        utmX: item.provincia?.utmX || 0,
        utmY: item.provincia?.utmY || 0,
        huso: item.provincia?.huso || 0,
        codigoProvincia: item.provincia?.codigoProvincia || '',
        geoPosicion: item.provincia?.geoPosicion || null,
        AutonomousCommunity: item.provincia?.AutonomousCommunity || null,
      },
      municipio: {
        id: item.municipio?.id,
        descripcion: item.municipio?.descripcion,
        codigoMunicipio: item.municipio?.codigoMunicipio || '',
        idProvincia: item.municipio?.idProvincia || 0,
        utmX: item.municipio?.utmX || 0,
        utmY: item.municipio?.utmY || 0,
        huso: item.municipio?.huso || 0,
        geoPosicion: item.municipio?.geoPosicion || null,
      },
      observaciones: item.observaciones || '',
      detalleIntervencionMedios:
        item.detalleIntervencionMedios?.map((detalle: any) => ({
          id: detalle.mediosCapacidad?.id,
          descripcion: detalle.mediosCapacidad?.descripcion,
          tipoMedio: detalle.mediosCapacidad?.tipoMedio,
          numeroMedio: detalle.mediosCapacidad?.numeroMedio,
          intervinientes: detalle.numeroIntervinientes,
        })) || [],
      geoPosicion: item.geoPosicion || null,
      esModificado: item.esModificado,
    }));
  }

esTerritorioNacional(): boolean {
  return this.fire.idTerritorio == 1;
}

}
