import { CommonModule } from '@angular/common';
import { Component, EventEmitter, Input, OnChanges, OnInit, Output, SimpleChanges, inject, signal } from '@angular/core';
import { FlexLayoutModule } from '@angular/flex-layout';
import { FormBuilder, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
import { MatAutocompleteModule } from '@angular/material/autocomplete';
import { MatButtonModule } from '@angular/material/button';
import { MatDialog, MatDialogModule } from '@angular/material/dialog';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatIconModule } from '@angular/material/icon';
import { MatInputModule } from '@angular/material/input';
import { MatSnackBar } from '@angular/material/snack-bar';
import { MunicipalityService } from '@services/municipality.service';
import { ProvinceService } from '@services/province.service';
import { MapCreateComponent } from '@shared/mapCreate/map-create.component';
import { DateUtils } from '@shared/utils/date-utils';
import { FECHA_MAXIMA_DATETIME, FECHA_MINIMA_DATETIME } from '@type/constants';
import { Fire } from '@type/fire.type';
import { Municipality } from '@type/municipality.type';
import { Province } from '@type/province.type';
import { FileSystemFileEntry, NgxFileDropEntry, NgxFileDropModule } from 'ngx-file-drop';
import { Feature } from 'ol';
import { Geometry } from 'ol/geom';
import { Observable, map, startWith } from 'rxjs';

@Component({
  selector: 'app-coordinacion-pma-form',
  standalone: true,
  imports: [
    CommonModule,
    ReactiveFormsModule,
    MatFormFieldModule,
    MatInputModule,
    MatButtonModule,
    MatIconModule,
    MatAutocompleteModule,
    FlexLayoutModule,
    NgxFileDropModule,
    MatDialogModule,
  ],
  templateUrl: './coordinacion-pma-form.component.html',
  styleUrls: ['./coordinacion-pma-form.component.scss']
})
export class CoordinacionPmaFormComponent implements OnInit, OnChanges {
  @Input() initialData: any = null;
  @Input() displayPart: 'first-row' | 'second-row' | 'third-row' = 'first-row';
  @Input() directionCoordinationService: any = null; // Para manejar descargas de archivos
  @Input() fire!: Fire;
  @Output() formChange = new EventEmitter<any>();
  @Output() formValidityChange = new EventEmitter<boolean>();
  
  private fb = inject(FormBuilder);
  private provinceService = inject(ProvinceService);
  private municipalityService = inject(MunicipalityService);
  private snackBar = inject(MatSnackBar);
  public matDialog = inject(MatDialog);
  
  public fechaMinimaDateTime = FECHA_MINIMA_DATETIME;
  public fechaMaximaDateTime = FECHA_MAXIMA_DATETIME;
  public file: File | null = null;
  public existingFile: any = null; // Para archivos existentes del servidor
  public pmaForm: FormGroup;
  
  // Signals para provincias y municipios
  public provinces = signal<Province[]>([]);
  public municipalities = signal<Municipality[]>([]);
  
  // Observables para autocompletado
  public provincefilteredOptions!: Observable<Province[]>;
  public municipalityfilteredOptions!: Observable<Municipality[]>;
  
  constructor() {
    // Obtener fecha actual en formato datetime-local
    const fechaActual = this.getCurrentDateTimeLocal();
    
    this.pmaForm = this.fb.group({
      fechaHoraInicio: [fechaActual, Validators.required],
      fechaHoraFin: [null],
      lugar: [null, Validators.required],
      provincia: [null, Validators.required],
      municipio: [null, Validators.required],
      observaciones: [null],
      geoPosicion: [null],
    });
  }

  /**
   * Obtiene la fecha y hora actual en formato datetime-local (YYYY-MM-DDTHH:mm)
   */
  private getCurrentDateTimeLocal(): string {
    return DateUtils.getCurrentCESTDate('YYYY-MM-DDTHH:mm');
  }

  async ngOnInit(): Promise<void> {
    try {
      const provinces = await this.provinceService.get();
      this.provinces.set(provinces);
      
      this.provincefilteredOptions = this.pmaForm.get('provincia')!.valueChanges.pipe(
        startWith(''),
        map(value => {
          if (typeof value === 'object' && value !== null) {
            return this._filterProvince(value.descripcion || '');
          }
          return this._filterProvince(value || '');
        })
      );
      
      this.municipalityfilteredOptions = this.pmaForm.get('municipio')!.valueChanges.pipe(
        startWith(''),
        map(value => {
          if (typeof value === 'object' && value !== null) {
            return this._filterMunicipality(value.descripcion || '');
          }
          return this._filterMunicipality(value || '');
        })
      );
    } catch (error) {
      console.error('Error loading provinces:', error);
    }
    
    this.setupFormBasedOnDisplayPart();
    this.pmaForm.valueChanges.subscribe(values => {
      this.emitFormValues();
      this.formValidityChange.emit(this.isPartValid());
    });
    this.formValidityChange.emit(this.isPartValid());

    // Agregar validador para fechaHoraInicio
    this.pmaForm.get('fechaHoraInicio')?.valueChanges.subscribe(fechaHoraInicio => {
      if (fechaHoraInicio && this.fire?.fechaInicio) {
        const fechaIncendio = new Date(this.fire.fechaInicio);
        const fechaInicioDate = new Date(fechaHoraInicio);

        console.log("Validador PMA - Fecha incendio:", fechaIncendio);
        console.log("Validador PMA - Fecha hora inicio:", fechaInicioDate);

        // Comparar las fechas usando getTime() para comparar timestamps
        if (fechaInicioDate.getTime() < fechaIncendio.getTime()) {
          this.pmaForm.get('fechaHoraInicio')?.setErrors({ fechaHoraInicioInvalida: true });
        } else {
          // Si la fecha es válida, limpiamos el error específico
          const errores = this.pmaForm.get('fechaHoraInicio')?.errors;
          if (errores) {
            delete errores['fechaHoraInicioInvalida'];
            // Si no quedan más errores, establecemos errors a null
            this.pmaForm.get('fechaHoraInicio')?.setErrors(Object.keys(errores).length ? errores : null);
          }
        }
      }
    });

    // Agregar validador para fechaHoraFin
    this.pmaForm.get('fechaHoraFin')?.valueChanges.subscribe(fechaHoraFin => {
      if (fechaHoraFin) {
        const fechaHoraInicio = this.pmaForm.get('fechaHoraInicio')?.value;
        if (fechaHoraInicio) {
          const fechaInicioDate = new Date(fechaHoraInicio);
          const fechaFinDate = new Date(fechaHoraFin);

          console.log("Validador PMA - Fecha hora inicio:", fechaInicioDate);
          console.log("Validador PMA - Fecha hora fin:", fechaFinDate);

          if (fechaFinDate.getTime() <= fechaInicioDate.getTime()) {
            this.pmaForm.get('fechaHoraFin')?.setErrors({ fechaHoraFinInvalida: true });
          } else {
            // Si la fecha es válida, limpiamos el error específico
            const errores = this.pmaForm.get('fechaHoraFin')?.errors;
            if (errores) {
              delete errores['fechaHoraFinInvalida'];
              // Si no quedan más errores, establecemos errors a null
              this.pmaForm.get('fechaHoraFin')?.setErrors(Object.keys(errores).length ? errores : null);
            }
          }
        }
      }
    });

    await this.setDefaultValues();
  }

  ngOnChanges(changes: SimpleChanges): void {
    if (changes['initialData']) {
      if (changes['initialData'].currentValue === null) {
        // Obtener fecha actual para el reset
        const fechaActual = this.getCurrentDateTimeLocal();
        
        this.pmaForm.reset({
          fechaHoraInicio: fechaActual,
          fechaHoraFin: null,
          lugar: null,
          provincia: null,
          municipio: null,
          observaciones: null,
          geoPosicion: null,
        });
        this.file = null;
        this.existingFile = null;
        this.emitFormValues();
        this.formValidityChange.emit(this.isPartValid());
      } else if (changes['initialData'].currentValue) {
        // Cargar datos para edición
        const data = changes['initialData'].currentValue;
        this.pmaForm.patchValue({
          fechaHoraInicio: data.fechaHoraInicio || this.getCurrentDateTimeLocal(),
          fechaHoraFin: data.fechaHoraFin || null,
          lugar: data.lugar || null,
          provincia: data.provincia || null,
          municipio: data.municipio || null,
          observaciones: data.observaciones || null,
          geoPosicion: data.geoPosicion,
        });
        
        // Cargar archivo si existe
        if (data.file) {
          this.file = data.file;
        }
        
        // Cargar archivo existente si existe
        if (data.archivo) {
          this.existingFile = data.archivo;
        }
        
        this.emitFormValues();
        this.formValidityChange.emit(this.isPartValid());
      }
    }
    if (changes['displayPart']) {
      this.setupFormBasedOnDisplayPart();
      this.formValidityChange.emit(this.isPartValid());
    }
  }

  private setupFormBasedOnDisplayPart(): void {
    const fechaHoraInicioCtrl = this.pmaForm.get('fechaHoraInicio');
    const fechaHoraFinCtrl = this.pmaForm.get('fechaHoraFin');
    const lugarCtrl = this.pmaForm.get('lugar');
    const provinciaCtrl = this.pmaForm.get('provincia');
    const municipioCtrl = this.pmaForm.get('municipio');
    const observacionesCtrl = this.pmaForm.get('observaciones');

    // Limpiar todos los validadores
    fechaHoraInicioCtrl?.clearValidators();
    fechaHoraFinCtrl?.clearValidators();
    lugarCtrl?.clearValidators();
    provinciaCtrl?.clearValidators();
    municipioCtrl?.clearValidators();
    observacionesCtrl?.clearValidators();

    // Aplicar validadores según la parte
    if (this.displayPart === 'first-row') {
      fechaHoraInicioCtrl?.setValidators([Validators.required]);
    } else if (this.displayPart === 'second-row') {
      lugarCtrl?.setValidators([Validators.required]);
      provinciaCtrl?.setValidators([Validators.required]);
      municipioCtrl?.setValidators([Validators.required]);
    }
    // third-row no tiene validadores (observaciones es opcional)

    // Actualizar validadores
    fechaHoraInicioCtrl?.updateValueAndValidity({ emitEvent: false });
    fechaHoraFinCtrl?.updateValueAndValidity({ emitEvent: false });
    lugarCtrl?.updateValueAndValidity({ emitEvent: false });
    provinciaCtrl?.updateValueAndValidity({ emitEvent: false });
    municipioCtrl?.updateValueAndValidity({ emitEvent: false });
    observacionesCtrl?.updateValueAndValidity({ emitEvent: false });
  }

  private isPartValid(): boolean {
    if (this.displayPart === 'first-row') {
      return this.pmaForm.get('fechaHoraInicio')?.valid ?? false;
    } else if (this.displayPart === 'second-row') {
      return !!(this.pmaForm.get('lugar')?.valid && this.pmaForm.get('provincia')?.valid && this.pmaForm.get('municipio')?.valid);
    } else if (this.displayPart === 'third-row') {
      return true; // Las observaciones son opcionales
    }
    return this.pmaForm.valid;
  }

  private emitFormValues(): void {
    const formValues = this.pmaForm.value;
    const relevantValues: any = {};

    if (this.displayPart === 'first-row') {
      relevantValues.fechaHoraInicio = formValues.fechaHoraInicio;
      relevantValues.fechaHoraFin = formValues.fechaHoraFin;
      relevantValues.file = this.file ? { name: this.file.name, size: this.file.size, type: this.file.type } : null;
    } else if (this.displayPart === 'second-row') {
      relevantValues.lugar = formValues.lugar;
      relevantValues.provincia = formValues.provincia;
      relevantValues.municipio = formValues.municipio;
    } else if (this.displayPart === 'third-row') {
      relevantValues.observaciones = formValues.observaciones;
    }

    relevantValues.geoPosicion = formValues.geoPosicion;
    
    this.formChange.emit(relevantValues);
  }
  
  // Métodos para manejo de archivos
  dropped(files: NgxFileDropEntry[]): void {
    if (this.displayPart !== 'first-row') return; // El drop solo afecta a la primera parte
    for (const droppedFile of files) {
      if (droppedFile.fileEntry.isFile) {
        const fileEntry = droppedFile.fileEntry as FileSystemFileEntry;
        fileEntry.file((file: File) => {
          this.file = file;
          this.emitFormValues();
        });
      }
    }
  }

  fileOver(event: any): void {
    console.log('🚀 ~ CoordinacionPmaFormComponent ~ fileOver ~ event:', event);
  }

  fileLeave(event: any): void {
    console.log('🚀 ~ CoordinacionPmaFormComponent ~ fileLeave ~ event:', event);
  }
  
  // Métodos para autocompletado
  private _filterProvince(value: string): Province[] {
    if (!value) return this.provinces();
    
    const filterValue = value.toLowerCase();
    return this.provinces().filter(province => province.descripcion.toLowerCase().includes(filterValue));
  }

  private _filterMunicipality(value: string): Municipality[] {
    if (!value) return this.municipalities();
    
    const filterValue = value.toLowerCase();
    return this.municipalities().filter(municipality => municipality.descripcion.toLowerCase().includes(filterValue));
  }

  // Métodos para visualización en autocomplete
  displayProvince = (province: Province): string => {
    return province && province.descripcion ? province.descripcion : '';
  }

  async setDefaultValues() {
    const province = this.provinces().find(item => item.id === this.fire.idProvincia);

    if (!province || this.pmaForm.get('provincia')?.value) {
      return;
    }
    
    try {
      const municipalities = await this.municipalityService.get(this.fire.idProvincia);
      this.municipalities.set(municipalities);
      const municipaly = this.municipalities().find(item => item.id === this.fire.idMunicipio);
      
      this.pmaForm.patchValue({
        provincia: province,
        municipio: municipaly,
      });
    } catch (error) {
      console.error('Error al cargar municipios:', error);
      this.municipalities.set([]);
    }
  }

  displayMunicipality = (municipality: Municipality): string => {
    return municipality && municipality.descripcion ? municipality.descripcion : '';
  }

  // Manejo de cambio de provincia
  async onProvinceChange(event: any): Promise<void> {
    const provinciaSeleccionada = event.option.value;
    const province_id = provinciaSeleccionada.id;
    
    try {
      const municipalities = await this.municipalityService.get(province_id);
      this.municipalities.set(municipalities);
      
      // Limpiar la selección de municipio
      this.pmaForm.get('municipio')?.setValue(null);
    } catch (error) {
      console.error('Error al cargar municipios:', error);
      this.municipalities.set([]);
    }
  }

  // Manejo de cambio de municipio
  onMunicipalityChange(event: any): void {
    // Aquí se puede agregar lógica adicional si es necesario
  }
  
  // Métodos públicos para el componente padre
  public isFormValid(): boolean {
    return this.isPartValid();
  }

  public getFormValues(): any {
    const formValues = this.pmaForm.value;
    if (this.displayPart === 'first-row') {
      return {
        fechaHoraInicio: formValues.fechaHoraInicio,
        fechaHoraFin: formValues.fechaHoraFin,
        file: this.file ? { name: this.file.name, size: this.file.size, type: this.file.type } : null,
        geoPosicion: formValues.geoPosicion,
      };
    } else if (this.displayPart === 'second-row') {
      return {
        lugar: formValues.lugar,
        provincia: formValues.provincia,
        municipio: formValues.municipio,
        geoPosicion: formValues.geoPosicion,
      };
    } else if (this.displayPart === 'third-row') {
      return {
        observaciones: formValues.observaciones,
        geoPosicion: formValues.geoPosicion,
      };
    }
    return {};
  }

  public markAsTouched(): void {
    this.pmaForm.markAllAsTouched();
  }
  
  public async downloadExistingFile(): Promise<void> {
    if (this.existingFile && this.directionCoordinationService) {
      try {
        console.log('🚀 ~ downloadExistingFile ~ existingFile:', this.existingFile);
        
        const blob = await this.directionCoordinationService.getFile(this.existingFile.id);

        // Crear una URL para el Blob
        const url = window.URL.createObjectURL(blob);

        // Crear un enlace temporal para la descarga
        const a = document.createElement('a');
        a.href = url;
        a.download = this.existingFile.nombreOriginal; // Nombre del archivo original
        document.body.appendChild(a);
        a.click();

        // Limpia el objeto URL después de la descarga
        window.URL.revokeObjectURL(url);
        document.body.removeChild(a);
        
        this.snackBar.open(`Archivo ${this.existingFile.nombreOriginal} descargado correctamente`, 'Cerrar', { 
          duration: 2000 
        });
      } catch (error) {
        console.error('Error al descargar el archivo:', error);
        this.snackBar.open('Error al descargar el archivo', 'Cerrar', { 
          duration: 3000 
        });
      }
    } else {
      this.snackBar.open('No hay archivo existente para descargar', 'Cerrar', { duration: 3000 });
    }
  }

  private initForm(): void {
    // Si ya existe el formulario, no lo reinicializamos
    if (this.pmaForm) {
      return;
    }
    
    // Inicialización del formulario (mantén el código existente de inicialización)
  }

  openModalMap() {
    if (!this.pmaForm.value.municipio) {
      return;
    }

    const polygon = this.pmaForm.get('geoPosicion')?.value
      ? [ this.pmaForm.get('geoPosicion')?.value.coordinates ]
      : this.fire.geoPosicion?.coordinates[0];

    const dialogRef = this.matDialog.open(MapCreateComponent, {
      width: '780px',
      maxWidth: '780px',
      height: '520px',
      maxHeight: '520px',
      disableClose: true,
      data: {
        municipio: this.pmaForm.value.municipio,
        listaMunicipios: this.municipalities(),
        defaultPolygon: polygon,
        close: true,
        showSearchCoordinates: true,
      },
    });

    dialogRef.componentInstance.save.subscribe((features: Feature<Geometry>[]) => {
      this.pmaForm.get('geoPosicion')?.setValue({
        type: 'Point',
        coordinates: features[0],
      });
    });
  }
} 