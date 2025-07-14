import { Component, EventEmitter, Input, OnInit, Output, inject, OnChanges, SimpleChanges, signal } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormBuilder, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { MatButtonModule } from '@angular/material/button';
import { MatIconModule } from '@angular/material/icon';
import { MatSelectModule } from '@angular/material/select';
import { MatSnackBar } from '@angular/material/snack-bar';
import { FlexLayoutModule } from '@angular/flex-layout';
import { NgxFileDropEntry, FileSystemFileEntry, NgxFileDropModule } from 'ngx-file-drop';
import { FECHA_MAXIMA_DATETIME, FECHA_MINIMA_DATETIME } from '@type/constants';
import { DirectionCoordinationService, TipoDireccionEmergencia } from '../../../../../services/direction-coordination.service';
import { Fire } from '@type/fire.type';
import { DateUtils } from '@shared/utils/date-utils';

@Component({
  selector: 'app-direccion-form',
  standalone: true,
  imports: [
    CommonModule,
    ReactiveFormsModule,
    MatFormFieldModule,
    MatInputModule,
    MatButtonModule,
    MatIconModule,
    MatSelectModule,
    FlexLayoutModule,
    NgxFileDropModule
  ],
  templateUrl: './direccion-form.component.html',
  styleUrls: ['./direccion-form.component.scss']
})
export class DireccionFormComponent implements OnInit, OnChanges {
  @Input() initialData: any = null; // Para limpiar o inicializar el formulario
  @Input() displayPart: 'first-row' | 'second-row' = 'first-row'; // Nuevo Input
  @Input() directionCoordinationService: any = null; // Para manejar descargas de archivos
  @Input() fire!: Fire;
  @Output() formChange = new EventEmitter<any>();
  @Output() formValidityChange = new EventEmitter<boolean>();
  
  private fb = inject(FormBuilder);
  private directionService = inject(DirectionCoordinationService);
  private snackBar = inject(MatSnackBar);
  
  // Variables para fechas
  public fechaMinimaDateTime = FECHA_MINIMA_DATETIME;
  public fechaMaximaDateTime = FECHA_MAXIMA_DATETIME;
  
  // Variable para el archivo
  public file: File | null = null;
  public existingFile: any = null; // Para archivos existentes del servidor
  
  // Formulario de dirección
  public direccionForm: FormGroup;
  public tiposDirecciones = signal<TipoDireccionEmergencia[]>([]);
  
  constructor() {
    // Obtener fecha actual en formato datetime-local
    const fechaActual = this.getCurrentDateTimeLocal();
    
    // Inicializar el formulario con fecha actual por defecto solo para fechaHoraInicio
    this.direccionForm = this.fb.group({
      fechaHoraInicio: [fechaActual, Validators.required],
      fechaHoraFin: [null],
      tipoDireccion: [null, Validators.required],
      lugar: [null, Validators.required]
    });
  }
  
  /**
   * Obtiene la fecha y hora actual en formato datetime-local (YYYY-MM-DDTHH:mm)
   */
  private getCurrentDateTimeLocal(): string {
    return DateUtils.getCurrentCESTDate('YYYY-MM-DDTHH:mm');
  }
  
  async ngOnInit(): Promise<void> {
    // Cargar tipos de direcciones si es la segunda fila
    if (this.displayPart === 'second-row') {
      try {
        const tipos = await this.directionService.getTiposDireccionesEmergencias();
        this.tiposDirecciones.set(tipos);
      } catch (error) {
        console.error('Error al cargar tipos de direcciones:', error);
        this.tiposDirecciones.set([]);
      }
    }
    
    this.setupFormBasedOnDisplayPart();
    this.direccionForm.valueChanges.subscribe(values => {
      this.emitFormValues();
      this.formValidityChange.emit(this.isPartValid());
    });
    this.formValidityChange.emit(this.isPartValid());

    // Inicializar el formulario
    this.initForm();

    // Agregar validador para fechaHoraInicio
    this.direccionForm.get('fechaHoraInicio')?.valueChanges.subscribe(fechaHoraInicio => {
      if (fechaHoraInicio && this.fire?.fechaInicio) {
        const fechaIncendio = new Date(this.fire.fechaInicio);
        const fechaInicioDate = new Date(fechaHoraInicio);

        console.log("Validador - Fecha incendio:", fechaIncendio);
        console.log("Validador - Fecha hora inicio:", fechaInicioDate);

        // Comparar las fechas usando getTime() para comparar timestamps
        if (fechaInicioDate.getTime() < fechaIncendio.getTime()) {
          this.direccionForm.get('fechaHoraInicio')?.setErrors({ fechaHoraInicioInvalida: true });
        } else {
          // Si la fecha es válida, limpiamos el error específico
          const errores = this.direccionForm.get('fechaHoraInicio')?.errors;
          if (errores) {
            delete errores['fechaHoraInicioInvalida'];
            // Si no quedan más errores, establecemos errors a null
            this.direccionForm.get('fechaHoraInicio')?.setErrors(Object.keys(errores).length ? errores : null);
          }
        }
      }
    });

    // Agregar validador para fechaHoraFin
    this.direccionForm.get('fechaHoraFin')?.valueChanges.subscribe(fechaHoraFin => {
      if (fechaHoraFin) {
        const fechaHoraInicio = this.direccionForm.get('fechaHoraInicio')?.value;
        if (fechaHoraInicio) {
          const fechaInicioDate = new Date(fechaHoraInicio);
          const fechaFinDate = new Date(fechaHoraFin);

          console.log("Validador - Fecha hora inicio:", fechaInicioDate);
          console.log("Validador - Fecha hora fin:", fechaFinDate);

          if (fechaFinDate.getTime() <= fechaInicioDate.getTime()) {
            this.direccionForm.get('fechaHoraFin')?.setErrors({ fechaHoraFinInvalida: true });
          } else {
            // Si la fecha es válida, limpiamos el error específico
            const errores = this.direccionForm.get('fechaHoraFin')?.errors;
            if (errores) {
              delete errores['fechaHoraFinInvalida'];
              // Si no quedan más errores, establecemos errors a null
              this.direccionForm.get('fechaHoraFin')?.setErrors(Object.keys(errores).length ? errores : null);
            }
          }
        }
      }
    });
  }
  
  ngOnChanges(changes: SimpleChanges): void {
    if (changes['initialData']) {
      if (changes['initialData'].currentValue === null) {
        // Obtener fecha actual para el reset
        const fechaActual = this.getCurrentDateTimeLocal();
        
        this.direccionForm.reset({
          fechaHoraInicio: fechaActual,
          fechaHoraFin: null,
          tipoDireccion: null,
          lugar: null
        });
        this.file = null;
        this.existingFile = null;
        this.emitFormValues(); // Emitir valores reseteados
        this.formValidityChange.emit(this.isPartValid());
      } else if (changes['initialData'].currentValue) {
        // Cargar datos para edición
        const data = changes['initialData'].currentValue;
        this.direccionForm.patchValue({
          fechaHoraInicio: data.fechaHoraInicio || this.getCurrentDateTimeLocal(),
          fechaHoraFin: data.fechaHoraFin || null,
          tipoDireccion: data.tipoDireccion || null,
          lugar: data.lugar || null
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
    // Ajustar validadores según la parte que se muestra
    const fechaHoraInicioCtrl = this.direccionForm.get('fechaHoraInicio');
    const fechaHoraFinCtrl = this.direccionForm.get('fechaHoraFin');
    const tipoDireccionCtrl = this.direccionForm.get('tipoDireccion');
    const lugarCtrl = this.direccionForm.get('lugar');

    if (this.displayPart === 'first-row') {
      fechaHoraInicioCtrl?.setValidators([Validators.required]);
      fechaHoraFinCtrl?.clearValidators();
      tipoDireccionCtrl?.clearValidators();
      lugarCtrl?.clearValidators();
    } else if (this.displayPart === 'second-row') {
      fechaHoraInicioCtrl?.clearValidators();
      fechaHoraFinCtrl?.clearValidators();
      tipoDireccionCtrl?.setValidators([Validators.required]);
      lugarCtrl?.setValidators([Validators.required]);
    }
    fechaHoraInicioCtrl?.updateValueAndValidity({ emitEvent: false });
    fechaHoraFinCtrl?.updateValueAndValidity({ emitEvent: false });
    tipoDireccionCtrl?.updateValueAndValidity({ emitEvent: false });
    lugarCtrl?.updateValueAndValidity({ emitEvent: false });
  }

  private isPartValid(): boolean {
    if (this.displayPart === 'first-row') {
      return this.direccionForm.get('fechaHoraInicio')?.valid ?? false;
    } else if (this.displayPart === 'second-row') {
      return !!(this.direccionForm.get('tipoDireccion')?.valid && this.direccionForm.get('lugar')?.valid);
    }
    return this.direccionForm.valid; // Fallback por si acaso
  }

  private emitFormValues(): void {
    const formValues = this.direccionForm.value;
    const relevantValues: any = {};

    if (this.displayPart === 'first-row') {
      relevantValues.fechaHoraInicio = formValues.fechaHoraInicio;
      relevantValues.fechaHoraFin = formValues.fechaHoraFin;
      relevantValues.file = this.file ? { name: this.file.name, size: this.file.size, type: this.file.type } : null;
    } else if (this.displayPart === 'second-row') {
      relevantValues.tipoDireccion = formValues.tipoDireccion;
      relevantValues.lugar = formValues.lugar;
    }
    
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
    console.log('🚀 ~ DireccionFormComponent ~ fileOver ~ event:', event);
  }

  fileLeave(event: any): void {
    console.log('🚀 ~ DireccionFormComponent ~ fileLeave ~ event:', event);
  }
  
  // Método para verificar si el formulario es válido
  public isFormValid(): boolean {
    return this.isPartValid(); // La validez general depende de la parte mostrada
  }
  
  // Método para obtener los valores del formulario incluyendo el archivo
  public getFormValues(): any {
    const formValues = this.direccionForm.value;
    if (this.displayPart === 'first-row') {
      return {
        fechaHoraInicio: formValues.fechaHoraInicio,
        fechaHoraFin: formValues.fechaHoraFin,
        file: this.file ? { name: this.file.name, size: this.file.size, type: this.file.type } : null
      };
    }
    return { 
      tipoDireccion: formValues.tipoDireccion,
      lugar: formValues.lugar 
    }; 
  }

  public markAsTouched(): void {
    if (this.displayPart === 'first-row') {
      this.direccionForm.get('fechaHoraInicio')?.markAsTouched();
      this.direccionForm.get('fechaHoraFin')?.markAsTouched();
      // No hay control de formulario para el fichero, se maneja visualmente
    } else if (this.displayPart === 'second-row') {
      this.direccionForm.get('tipoDireccion')?.markAsTouched();
      this.direccionForm.get('lugar')?.markAsTouched();
    }
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
    if (this.direccionForm) {
      return;
    }
    
    // Inicialización del formulario (mantén el código existente de inicialización)
  }
} 