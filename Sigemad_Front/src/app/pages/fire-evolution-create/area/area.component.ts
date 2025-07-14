import { Component, EventEmitter, inject, Input, Output, signal, ViewChild } from '@angular/core';

import { CommonModule } from '@angular/common';
import { ChangeDetectorRef } from '@angular/core';
import { FlexLayoutModule } from '@angular/flex-layout';
import { FormBuilder, FormGroup, FormGroupDirective, ReactiveFormsModule, Validators } from '@angular/forms';
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
import { MinorEntityService } from '../../../services/minor-entity.service';
import { MunicipalityService } from '../../../services/municipality.service';
import { ProvinceService } from '../../../services/province.service';
import { RecordsService } from '@services/records.service';
import { MapCreateComponent } from '../../../shared/mapCreate/map-create.component';
import { CoordinationAddress } from '../../../types/coordination-address';
import { MinorEntity } from '../../../types/minor-entity.type';
import { Municipality } from '../../../types/municipality.type';
import { Province } from '../../../types/province.type';
import { SavePayloadModal } from '../../../types/save-payload-modal';
import { FileSystemDirectoryEntry, FileSystemFileEntry, NgxFileDropEntry, NgxFileDropModule } from 'ngx-file-drop';
import { readFileAsArrayBuffer, readFileAsText } from '../../../shared/utils/file-utils';
import shp from 'shpjs';
import { FECHA_MAXIMA_DATETIME, FECHA_MINIMA_DATETIME } from '@type/constants';
import { AlertService } from '../../../shared/alert/alert.service';
import { MatAutocompleteModule } from '@angular/material/autocomplete';
import { Observable, map, startWith } from 'rxjs';
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
  selector: 'app-area',
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
    MapCreateComponent,
    NgxFileDropModule,
    MatAutocompleteModule,
    MatTooltipModule,
  ],
  providers: [
    { provide: DateAdapter, useClass: NativeDateAdapter },
    { provide: MAT_DATE_FORMATS, useValue: FORMATO_FECHA },
  ],
  templateUrl: './area.component.html',
  styleUrl: './area.component.scss',
})
export class AreaComponent {
  @ViewChild(MatSort) sort!: MatSort;
  data = inject(MAT_DIALOG_DATA) as { title: string; idIncendio: number };
  @Output() save = new EventEmitter<SavePayloadModal>();
  @Output() changesMade = new EventEmitter<boolean>();
  @Output() hasUnsavedChanges = new EventEmitter<boolean>();
  @Input() editData: any;
  @Input() esUltimo: boolean | undefined;
  @Input() fire: any;
  @Input() isNewRecord: boolean | undefined;
  @Input() registroId: number | null = null;
  @Input() registrosPosterioresConAreasAfectadas: boolean = false;

  public evolutionService = inject(EvolutionService);
  public toast = inject(MatSnackBar);
  private provinceService = inject(ProvinceService);
  private fb = inject(FormBuilder);
  public matDialog = inject(MatDialog);
  private spinner = inject(NgxSpinnerService);
  private municipalityService = inject(MunicipalityService);
  private minorService = inject(MinorEntityService);
  private recordsService = inject(RecordsService);
  private cdr = inject(ChangeDetectorRef);
  private alertService = inject(AlertService);
  public polygon = signal<any>([]);

  public displayedColumns: string[] = ['fechaHora', 'descripcion', 'fichero', 'procendenciaDestino', 'opciones'];

  formData!: FormGroup;

  public coordinationAddress = signal<CoordinationAddress[]>([]);
  public isCreate = signal<number>(-1);
  public provinces = signal<Province[]>([]);
  public municipalities = signal<Municipality[]>([]);
  public minors = signal<MinorEntity[]>([]);
  public dataSource = new MatTableDataSource<any>([]);
  public showForm = false;

  public fechaMinimaDateTime = FECHA_MINIMA_DATETIME;
  public fechaMaximaDateTime = FECHA_MAXIMA_DATETIME;

  selectedMunicipio: any;
  listaMunicipios: any;
  onlyView: any = null;
  defaultPolygon: any;
  index = 0;

  file: File | null = null;
  public files: NgxFileDropEntry[] = [];
  fileFlag: boolean = false;
  fileContent: string | null = null;

  // Nuevas propiedades para autocompletado
  public provincefilteredOptions!: Observable<Province[]>;
  public municipalityfilteredOptions!: Observable<Municipality[]>;

  private getCurrentDateTimeString(): string {
    return DateUtils.getCurrentCESTDate();
  }

  async ngOnInit() {
    this.spinner.show();
    this.selectedMunicipio = null;

    // Mostrar si es un nuevo registro o edición
    console.log('🚀 ~ AreaComponent ~ ¿Es nuevo registro?:', this.isNewRecord);
    console.log('🚀 ~ AreaComponent ~ registroId:', this.registroId);

    const provinces = await this.provinceService.get();
    this.provinces.set(provinces);

    this.formData = this.fb.group({
      fechaHora: [this.getCurrentDateTimeString(), Validators.required],
      provincia: [null, Validators.required],
      municipio: [null, Validators.required],
      entidadMenor: [null],
      observaciones: [''],
      fichero: ['', Validators.required],
      superficieAfectadaHectarea: [null],
    });
    this.formData.get('fichero')?.disable();
    this.formData.get('entidadMenor')?.disable();

    // Inicializar autocompletado de provincia
    this.provincefilteredOptions = this.formData.get('provincia')!.valueChanges.pipe(
      startWith(''),
      map((value) => this._filterProvinces(value))
    );

    // Inicializar autocompletado de municipio
    this.municipalityfilteredOptions = this.formData.get('municipio')!.valueChanges.pipe(
      startWith(''),
      map((value) => this._filterMunicipalities(value))
    );

    // Establecer valor inicial de provincia - Buscar el objeto completo, no solo el id
    const provinciaObj = provinces.find((p) => p.id === this.fire.provincia.id);
    if (provinciaObj) {
      this.formData.get('provincia')?.setValue(provinciaObj);
    } else {
      console.error('No se encontró la provincia con ID:', this.fire.provincia.id);
    }

    const municipalities = await this.municipalityService.get(this.fire.provincia.id);
    this.municipalities.set(municipalities);

    const municipioId = this.fire.municipio.id;
    const municipioObj = municipalities.find((m) => m.id === municipioId);

    if (municipioObj) {
      this.formData.get('municipio')?.setValue(municipioObj);
      this.selectedMunicipio = municipioObj;

      const minor = await this.minorService.get(municipioId);
      this.minors.set(minor);
      this.formData.get('entidadMenor')?.enable();
    } else {
      console.error('No se encontró el municipio con ID:', municipioId);
    }

    console.log('🚀 ~ AreaComponent ~ ngOnInit ~ this.editData:', this.editData);

    try {
      const registroData = await this.recordsService.getById(Number(this.registroId));
      console.log('Datos del registro:', registroData);

      // Si no hay datos de áreas afectadas o está vacío, intentamos obtener registros anteriores
      if (!registroData?.areaAfectadas || registroData.areaAfectadas.length === 0) {
        const registrosAnteriores = await this.recordsService.getRegistrosAnteriores(Number(this.fire.idSuceso), this.registroId!);
        console.log('Datos de registros anteriores:', registrosAnteriores);

        if (registrosAnteriores[0] && registrosAnteriores[0].areaAfectadas?.length > 0) {
          this.evolutionService.dataAffectedArea.set(registrosAnteriores[0].areaAfectadas);
        }
      } else {
        // Si hay datos de áreas afectadas, los usamos
        this.evolutionService.dataAffectedArea.set(registroData.areaAfectadas);
      }
    } catch (error) {
      console.error('Error al cargar los datos:', error);
      this.toast.open('Error al cargar los datos', 'Cerrar', { duration: 3000 });
    }

    this.listaMunicipios = this.municipalities();
    this.onlyView = true;
    this.defaultPolygon = this.polygon();
    this.spinner.hide();

    if (this.evolutionService.dataAffectedArea().length === 0) {
      const ficheroControl = this.formData.get('fichero');
      if (ficheroControl) {
        const currentValidators = ficheroControl.validator;
        ficheroControl.clearValidators();
        ficheroControl.updateValueAndValidity();

        const formDirective = new FormGroupDirective([], []);
        formDirective.form = this.formData;

        setTimeout(() => {
          this.onSubmit(formDirective);
          if (currentValidators) {
            ficheroControl.setValidators(currentValidators);
            ficheroControl.updateValueAndValidity();
          }
        }, 100);
      }
    }
  }

  // Métodos para autocompletado
  private _filterProvinces(value: any): Province[] {
    if (!value) return this.provinces();

    const filterValue = typeof value === 'string' ? value.toLowerCase() : value.descripcion.toLowerCase();
    return this.provinces().filter((province) => province.descripcion.toLowerCase().includes(filterValue));
  }

  private _filterMunicipalities(value: any): Municipality[] {
    if (!value) return this.municipalities();

    const filterValue = typeof value === 'string' ? value.toLowerCase() : value.descripcion.toLowerCase();
    return this.municipalities().filter((municipality) => municipality.descripcion.toLowerCase().includes(filterValue));
  }

  // Método para visualizar la provincia en el autocomplete
  displayProvince = (province: Province): string => {
    return province && province.descripcion ? province.descripcion : '';
  };

  // Método para visualizar el municipio en el autocomplete
  displayMunicipality = (municipality: Municipality): string => {
    return municipality && municipality.descripcion ? municipality.descripcion : '';
  };

  // Nueva versión del método que carga los municipios al cambiar la provincia
  async onProvinceChange(event: any) {
    this.spinner.show();
    const provinciaSeleccionada = event.option.value;
    const province_id = provinciaSeleccionada.id;

    const municipalities = await this.municipalityService.get(province_id);
    this.municipalities.set(municipalities);

    // Limpiar la selección de municipio
    this.formData.get('municipio')?.setValue(null);
    this.formData.get('entidadMenor')?.setValue(null);
    this.formData.get('entidadMenor')?.disable();

    this.spinner.hide();
  }

  // Nueva versión del método que carga entidades menores al cambiar el municipio
  async onMunicipalityChange(event: any) {
    this.spinner.show();
    const municipioSeleccionado = event.option.value;
    this.selectedMunicipio = municipioSeleccionado;

    const muni_id = municipioSeleccionado.id;
    const minor = await this.minorService.get(muni_id);
    this.minors.set(minor);
    this.formData.get('entidadMenor')?.enable();

    // Registrar el municipio seleccionado
    console.log('🚀 ~ AreaComponent ~ onChangeMunicipio ~ this.selectedMunicipio:', this.selectedMunicipio);

    this.spinner.hide();
  }

  // Método de compatibilidad con el código anterior
  async loadMunicipalities(event: any) {
    // Esta función se mantiene para compatibilidad pero redirige a la nueva implementación
    if (event && event.value) {
      await this.onProvinceChange({ option: { value: { id: event.value } } });
    }
  }

  // Método de compatibilidad con el código anterior
  async loadMinor(event: any) {
    // Esta función se mantiene para compatibilidad pero ahora se llama desde onMunicipalityChange
    if (event && event.value) {
      const muni_id = event.value.id || event.value;
      const minor = await this.minorService.get(muni_id);
      this.minors.set(minor);
      this.formData.get('entidadMenor')?.enable();
    }
  }

  // Restauramos el método para compatibilidad
  onChangeMunicipio(event: any) {
    const municipioValue = event.value.id || event.value;
    this.selectedMunicipio = this.listaMunicipios.find((item: any) => item.id == municipioValue);
    this.polygon.set([]);

    // Si hay un valor válido, llamamos a loadMinor
    if (this.selectedMunicipio) {
      this.loadMinor({ value: this.selectedMunicipio });
    }
  }

  async onSubmit(formDirective: FormGroupDirective) {
    const dataFormulario = this.formData.value;
    console.log('🚀 ~ AreaComponent ~ onSubmit ~ dataFormulario:', dataFormulario);

    // Verificar si es un registro automático
    const esRegistroAutomatico = dataFormulario.observaciones === 'Registro automático';

    // Si es un registro automático, no necesitamos validar duplicados
    // ya que previamente verificamos que no existen áreas
    let validacionExitosa = esRegistroAutomatico ? true : await this.validarDuplicados(dataFormulario);

    if (validacionExitosa && this.formData.valid) {
      // Verificar si estamos editando un registro existente
      if (this.isCreate() >= 0) {
        // Si estamos editando, llamar al método editarItem
        await this.editarItem(this.isCreate());
      } else {
        // Si es un nuevo registro, agregar al array
        dataFormulario.fechaHora = DateUtils.fromCestToUtc(dataFormulario.fechaHora);

        if (dataFormulario.superficieAfectadaHectarea !== null && dataFormulario.superficieAfectadaHectarea !== '') {
          dataFormulario.superficieAfectadaHectarea = Number(dataFormulario.superficieAfectadaHectarea);
        }

        // Asegurarse de que el polígono esté correctamente establecido
        const geoPosicion = {
          type: 'Polygon',
          coordinates: [this.polygon()],
        };

        this.evolutionService.dataAffectedArea.update((data) => {
          return [
            ...data,
            {
              ...dataFormulario,
              geoPosicion: geoPosicion,
            },
          ];
        });

        formDirective.resetForm();
        
        // Obtenemos los objetos completos de provincia y municipio
        const provinciaObj = this.provinces().find((p) => p.id === this.fire.provincia.id);
        const municipioObj = this.municipalities().find((m) => m.id === this.fire.municipio.id);
        
        this.formData.reset({
          fechaHora: this.getCurrentDateTimeString(),
          provincia: provinciaObj || null,
          municipio: municipioObj || null,
          entidadMenor: null,
          observaciones: '',
          fichero: '',
          superficieAfectadaHectarea: null,
        });

        if (municipioObj) {
          this.selectedMunicipio = municipioObj;
        }

        this.isCreate.set(-1);
        this.showForm = false;
        this.hasUnsavedChanges.emit(false);
        
        // Solo emitir changesMade(true) si no es un registro automático
        if (!esRegistroAutomatico) {
          this.changesMade.emit(true);
        }
      }
    } else {
      this.formData.markAllAsTouched();
    }
    console.log('🚀 ~ AreaComponent ~ onSubmit ~  this.evolutionService.dataAffectedArea:', this.evolutionService.dataAffectedArea());
  }

  async validarDuplicados(nuevoRegistro: any): Promise<boolean> {
    const areasExistentes = this.evolutionService.dataAffectedArea();
    
    // Si no hay áreas existentes o solo hay una y estamos editándola, no puede haber duplicados
    if (areasExistentes.length === 0 || (areasExistentes.length === 1 && this.isCreate() === 0)) {
      return true;
    }
    
    const provinciaId = nuevoRegistro.provincia?.id || nuevoRegistro.provincia;
    const municipioId = nuevoRegistro.municipio?.id || nuevoRegistro.municipio;
    const entidadMenorId = nuevoRegistro.entidadMenor;

    const areasParaComparar = this.isCreate() >= 0 ? areasExistentes.filter((_, index) => index !== this.isCreate()) : areasExistentes;

    // Si después de filtrar no quedan áreas para comparar, no puede haber duplicados
    if (areasParaComparar.length === 0) {
      return true;
    }

    const areasDuplicadas = areasParaComparar.filter((area) => {
      const areaProvinciaId = area.provincia?.id || area.provincia;
      const areaMunicipioId = area.municipio?.id || area.municipio;
      const areaEntidadMenorId = area.entidadMenor;

      return areaProvinciaId === provinciaId && areaMunicipioId === municipioId && areaEntidadMenorId === entidadMenorId;
    });

    // if (areasDuplicadas.length > 0) {
    //   await this.alertService.showAlert({
    //     title: 'Área duplicada',
    //     text: 'No pueden incluirse varias zonas de área afectada con los mismos datos de provincia, municipio y entidad menor.',
    //     icon: 'warning',
    //     confirmButtonText: 'Entendido',
    //   });
    //   return false;
    // }

    if (entidadMenorId) {
      const areasSinEntidadMenor = areasParaComparar.filter((area) => {
        const areaProvinciaId = area.provincia?.id || area.provincia;
        const areaMunicipioId = area.municipio?.id || area.municipio;

        return areaProvinciaId === provinciaId && areaMunicipioId === municipioId && !area.entidadMenor;
      });

      if (areasSinEntidadMenor.length > 0) {
        await this.alertService.showAlert({
          title: 'Conflicto de áreas',
          text: 'No se puede incluir una zona con entidad menor cuando ya existe una zona sin entidad menor para la misma provincia y municipio.',
          icon: 'warning',
          confirmButtonText: 'Entendido',
        });
        return false;
      }
    }

    if (!entidadMenorId) {
      const areasConEntidadMenor = areasParaComparar.filter((area) => {
        const areaProvinciaId = area.provincia?.id || area.provincia;
        const areaMunicipioId = area.municipio?.id || area.municipio;

        return areaProvinciaId === provinciaId && areaMunicipioId === municipioId && area.entidadMenor;
      });

      if (areasConEntidadMenor.length > 0) {
        await this.alertService.showAlert({
          title: 'Conflicto de áreas',
          text: 'No se puede incluir una zona sin entidad menor cuando ya existen zonas con entidad menor para la misma provincia y municipio.',
          icon: 'warning',
          confirmButtonText: 'Entendido',
        });
        return false;
      }
    }

    return true;
  }

  async sendDataToEndpoint() {
    if (this.evolutionService.dataAffectedArea().length > 0 && !this.editData) {
      this.save.emit({ save: true, delete: false, close: false, update: false });
    } else {
      if (this.editData) {
        this.save.emit({ save: false, delete: false, close: false, update: true });
      }
    }
  }

  async editarItem(index: number) {
    const dataEditada = this.formData.value;

    if (!(await this.validarDuplicados(dataEditada))) {
      return;
    }

    if (this.registrosPosterioresConAreasAfectadas) {
      const result = await this.alertService.showAlert({
        title: 'Información importante',
        text: 'Existen registros posteriores de área afectada. Los cambios que se van a guardar no actualizarán esos registros y, si se necesita, tendrá que realizar los cambios en cada uno de ellos. ¿Desea continuar?',
        icon: 'warning',
        showCancelButton: true,
        confirmButtonText: 'Sí, continuar',
        cancelButtonText: 'Cancelar',
      });

      if (!result.isConfirmed) {
        return;
      }
    }

    if (dataEditada.superficieAfectadaHectarea !== null && dataEditada.superficieAfectadaHectarea !== '') {
      dataEditada.superficieAfectadaHectarea = Number(dataEditada.superficieAfectadaHectarea);
    }

    dataEditada.fechaHora = DateUtils.fromCestToUtc(dataEditada.fechaHora);

    // Asegurarse de que el polígono esté correctamente establecido
    const geoPosicion = {
      type: 'Polygon',
      coordinates: [this.polygon()],
    };

    this.evolutionService.dataAffectedArea.update((data) => {
      // Mantener el polígono original si no se ha modificado
      const originalGeoPosicion = data[index].geoPosicion;
      
      data[index] = { 
        ...data[index], 
        ...dataEditada,
        // Usar el nuevo polígono si existe, de lo contrario mantener el original
        geoPosicion: this.polygon().length > 0 ? geoPosicion : originalGeoPosicion
      };
      return [...data];
    });

    this.hasUnsavedChanges.emit(true);
    this.changesMade.emit(true);

    this.isCreate.set(-1);
    this.formData.reset();

    this.showForm = false;
  }

  async eliminarItem(index: number) {
    if (this.registrosPosterioresConAreasAfectadas) {
      const result = await this.alertService.showAlert({
        title: 'Información importante',
        text: 'Existen registros posteriores de área afectada. La eliminación de este registro no actualizará esos registros y, si se necesita, tendrá que realizar los cambios en cada uno de ellos. ¿Desea continuar?',
        icon: 'warning',
        showCancelButton: true,
        confirmButtonText: 'Sí, eliminar',
        cancelButtonText: 'Cancelar',
      });

      if (!result.isConfirmed) {
        return;
      }
    }

    this.index = -1;
    this.evolutionService.dataAffectedArea.update((data) => {
      data.splice(index, 1);
      return [...data];
    });

    this.hasUnsavedChanges.emit(false);
    this.changesMade.emit(true);
  }

  async seleccionarItem(index: number) {
    this.index = index;
    console.log('🚀 ~ AreaComponent ~ seleccionarItem ~ index:', this.index);
    this.isCreate.set(index);
    const data = this.evolutionService.dataAffectedArea()[index];
    this.spinner.show();

    const provinciaId = typeof data.provincia === 'object' ? data.provincia?.id : data.provincia;
    const municipioId = typeof data.municipio === 'object' ? data.municipio?.id : data.municipio;

    if (provinciaId) {
      const municipalities = await this.municipalityService.get(provinciaId);
      this.municipalities.set(municipalities);
    }

    if (data.entidadMenor?.id && municipioId) {
      const minor = await this.minorService.get(municipioId);
      this.minors.set(minor);
    }

    this.formData.get('fechaHora')?.setValue(DateUtils.fromUtcToCest(data.fechaHora));
    this.formData.get('observaciones')?.setValue(data.observaciones);
    this.formData.get('superficieAfectadaHectarea')?.setValue(data.superficieAfectadaHectarea);

    if (data.geoPosicion?.coordinates) {
      this.polygon.set(data.geoPosicion.coordinates[0]);
      this.defaultPolygon = this.polygon();
    }

    // Buscar el objeto de provincia completo y establecerlo
    if (provinciaId) {
      const provinciaObj = this.provinces().find((p) => p.id === provinciaId);
      if (provinciaObj) {
        this.formData.get('provincia')?.setValue(provinciaObj);
      } else {
        console.error('No se encontró la provincia con ID:', provinciaId);
      }
    }

    // Buscar el objeto de municipio completo y establecerlo
    if (municipioId) {
      const municipioObj = this.municipalities().find((m) => m.id === municipioId);
      if (municipioObj) {
        this.formData.get('municipio')?.setValue(municipioObj);
        this.selectedMunicipio = municipioObj;
      } else {
        console.error('No se encontró el municipio con ID:', municipioId);
      }
    }

    if (data.entidadMenor) {
      const entidadMenorId = typeof data.entidadMenor === 'object' ? data.entidadMenor.id : data.entidadMenor;
      this.formData.get('entidadMenor')?.setValue(entidadMenorId);
    }

    this.formData.get('entidadMenor')?.enable();
    this.formData.get('municipio')?.enable();

    this.showForm = true;
    this.changesMade.emit(false);
    // No emitimos hasUnsavedChanges para que el botón Guardar no se habilite
    this.spinner.hide();
  }

  getFormatdate(date: any) {
    return DateUtils.fromUtcToCest(date, 'DD/MM/YYYY HH:mm');
  }

  getForm(atributo: string): any {
    return this.formData.controls[atributo];
  }

  trackByFn(index: number, item: any): number {
    return item.id;
  }

  closeModal() {
    this.save.emit({ save: false, delete: false, close: true, update: false });
  }

  delete() {
    this.save.emit({ save: false, delete: true, close: false, update: false });
  }

  getProvincesById(id: number): string | null {
    const data = this.provinces();
    const found = data.find((item) => item.id === id);
    return found ? found.descripcion : null;
  }

  getMunicipalitiesById(id: number): string | null {
    const data = this.municipalities();
    const found = data.find((item) => item.id === id);
    return found ? found.descripcion : null;
  }

  getMinorById(id: number): string | null {
    const found = this.minors().find((m) => m.id === id);
    return found ? found.descripcion : '';
  }

  isInteger(value: any): boolean {
    return Number.isInteger(value);
  }

  onSave(features: Feature<Geometry>[]) {
    console.log('Guardando polígono:', features);
    this.polygon.set(features);
    
    // Ya no necesitamos actualizar this.editData.areaAfectadas aquí
    // porque el polígono se actualiza cuando se llama a editarItem o cuando se crea un nuevo registro
  }

  public dropped(files: NgxFileDropEntry[]) {
    this.spinner.show();
    for (const droppedFile of files) {
      if (droppedFile.fileEntry.isFile) {
        if (droppedFile.fileEntry.name.endsWith('.zip')) {
          const fileEntry = droppedFile.fileEntry as FileSystemFileEntry;
          fileEntry.file(async (file: File) => {
            this.file = file;
            this.fileFlag = true;

            const fileContent = await readFileAsArrayBuffer(file);
            const geojson = await shp(fileContent);
            //console.log(geojson);

            this.formData.patchValue({ file });
            this.onFileSelected(JSON.stringify(geojson));
          });
        } else {
          const fileEntry = droppedFile.fileEntry as FileSystemFileEntry;
          fileEntry.file(async (file: File) => {
            this.file = file;
            this.fileFlag = true;

            const fileContent = await readFileAsText(file);
            this.formData.patchValue({ file });

            this.onFileSelected(fileContent);
          });
        }
      } else {
        const fileEntry = droppedFile.fileEntry as FileSystemDirectoryEntry;
        // console.log(droppedFile.relativePath, fileEntry);
      }
    }
    this.spinner.hide();
  }

  onFileSelected(fileContent: string) {
    this.fileContent = fileContent;
    //this.save.emit({ save: true, delete: false, close: false, update: false });
  }

  public fileOver(event: any) {
    // console.log('File over event:', event);
  }

  public fileLeave(event: any) {
    // console.log('File leave event:', event);
  }

  // Método para resetear el formulario
  resetForm() {
    // Ya no autocompleta provincia y municipio, los deja vacíos
    this.formData.reset({
      fechaHora: this.getCurrentDateTimeString(),
      provincia: null,
      municipio: null,
      entidadMenor: null,
      observaciones: '',
      fichero: '',
      superficieAfectadaHectarea: null,
    });

    // Ya no establecemos el municipio seleccionado
   this.selectedMunicipio = {};
    
    // Limpiar las entidades menores y deshabilitar el campo
    this.minors.set([]);
    this.formData.get('entidadMenor')?.disable();

    this.showForm = true;
    this.changesMade.emit(false);
    // No emitimos hasUnsavedChanges para que el botón Guardar no se habilite
  }

  cancel() {
    this.showForm = false;
    this.isCreate.set(-1);
    this.hasUnsavedChanges.emit(false);
  }

  // Método público para generar un registro automático desde el componente padre
  public async generarRegistroAutomatico() {
    try {
      // Verificar si el formulario está inicializado
      if (!this.formData) {
        console.error('El formulario no está inicializado');
        return;
      }
      
      // Verificar si el incendio es en territorio nacional
      if (!this.fire || this.fire.idTerritorio !== 1) {
        console.log('No es territorio nacional (idTerritorio:', this.fire?.idTerritorio, '), no se generará área automática');
        return;
      }
      
      // Verificar si ya existen áreas
      if (this.evolutionService.dataAffectedArea().length > 0) {
        console.log('Ya existen áreas, no se genera registro automático');
        return;
      }

      // Asegurarse de que isCreate esté configurado a -1 para crear un nuevo registro
      this.isCreate.set(-1);

      // Crear un FormGroupDirective para pasar al onSubmit
      const formDirective = new FormGroupDirective([], []);
      formDirective.form = this.formData;

      // Verificar que los datos necesarios estén cargados
      if (!this.provinces() || this.provinces().length === 0 || 
          !this.municipalities() || this.municipalities().length === 0) {
        console.error('Los datos de provincias o municipios no están cargados');
        return;
      }

      // Establecer valores en el formulario
      const provinciaObj = this.provinces().find((p) => p.id === this.fire.provincia.id);
      const municipioObj = this.municipalities().find((m) => m.id === this.fire.municipio.id);

      if (!provinciaObj || !municipioObj) {
        console.error('No se encontró la provincia o municipio del incendio');
        return;
      }

      this.formData.patchValue({
        fechaHora: this.getCurrentDateTimeString(),
        provincia: provinciaObj,
        municipio: municipioObj,
        entidadMenor: null,
        observaciones: 'Registro automático',
        superficieAfectadaHectarea: null
      });

      // Crear un polígono simple pero válido (un triángulo)
      // Usar coordenadas que formen un polígono válido pero pequeño
      const emptyPolygon = [
        [0.001, 0.001],
        [0.002, 0.001],
        [0.0015, 0.002],
        [0.001, 0.001] // Cerrar el polígono volviendo al primer punto
      ];
      this.polygon.set(emptyPolygon);

      console.log('Generando registro automático de área con datos:', {
        provincia: provinciaObj.descripcion,
        municipio: municipioObj.descripcion,
        observaciones: 'Registro automático'
      });

      // Llamar al método onSubmit para crear el registro
      await this.onSubmit(formDirective);
      
      // Asegurarse de que no queden cambios pendientes
      this.hasUnsavedChanges.emit(false);
      this.changesMade.emit(false);
      
      console.log('Registro automático de área generado correctamente, sin cambios pendientes');
    } catch (error) {
      console.error('Error al generar registro automático de área:', error);
    }
  }
}
