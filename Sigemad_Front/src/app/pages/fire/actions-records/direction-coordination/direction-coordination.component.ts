import { Component, OnInit, ViewChild, inject, signal, Input, Output, EventEmitter } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormBuilder, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { MatSelectModule } from '@angular/material/select';
import { MatButtonModule } from '@angular/material/button';
import { MatIconModule } from '@angular/material/icon';
import { MatSnackBar } from '@angular/material/snack-bar';
import { MatTableModule } from '@angular/material/table';
import { MatTooltipModule } from '@angular/material/tooltip';
import { FlexLayoutModule } from '@angular/flex-layout';
import { NgxSpinnerModule, NgxSpinnerService } from 'ngx-spinner';
import { NgxFileDropEntry, FileSystemFileEntry, NgxFileDropModule } from 'ngx-file-drop';

// Importar servicios y tipos
import { DirectionCoordinationService, GestionData } from '../../../../services/direction-coordination.service';
import { RecordsService } from '../../../../services/records.service';
import { DirectionType } from '../../../../types/direction-type.type';
import { FECHA_MAXIMA_DATETIME, FECHA_MINIMA_DATETIME } from '@type/constants';

// Importar componentes hijos
import { DireccionFormComponent } from './direccion-form/direccion-form.component';
import { CoordinacionCecopiFormComponent } from './coordinacion-cecopi-form/coordinacion-cecopi-form.component';
import { CoordinacionPmaFormComponent } from './coordinacion-pma-form/coordinacion-pma-form.component';
import { DateUtils } from '@shared/utils/date-utils';

@Component({
  selector: 'app-direction-coordination',
  standalone: true,
  imports: [
    CommonModule,
    ReactiveFormsModule,
    MatFormFieldModule,
    MatInputModule,
    MatSelectModule,
    MatButtonModule,
    MatIconModule,
    MatTableModule,
    MatTooltipModule,
    FlexLayoutModule,
    NgxSpinnerModule,
    NgxFileDropModule,
    DireccionFormComponent,
    CoordinacionCecopiFormComponent,
    CoordinacionPmaFormComponent
  ],
  templateUrl: './direction-coordination.component.html',
  styleUrls: ['./direction-coordination.component.scss']
})
export class DirectionCoordinationComponent implements OnInit {
  @Input() fire: any;
  @Input() editData: any;
  @Input() esUltimo: boolean | undefined;
  @Input() estadoIncendio: any;
  @Input() registroId: number | null = null;

  @Output() save = new EventEmitter<any>();
  @Output() changesMade = new EventEmitter<boolean>();
  @Output() hasUnsavedChanges = new EventEmitter<boolean>();

  @ViewChild('direccionFormFirstRow') direccionFormFirstRowRef!: DireccionFormComponent;
  @ViewChild('direccionFormSecondRow') direccionFormSecondRowRef!: DireccionFormComponent;
  @ViewChild('coordinacionCecopiFormFirstRow') coordinacionCecopiFormFirstRowRef!: CoordinacionCecopiFormComponent;
  @ViewChild('coordinacionCecopiFormSecondRow') coordinacionCecopiFormSecondRowRef!: CoordinacionCecopiFormComponent;
  @ViewChild('coordinacionCecopiFormThirdRow') coordinacionCecopiFormThirdRowRef!: CoordinacionCecopiFormComponent;
  @ViewChild('coordinacionPmaFormFirstRow') coordinacionPmaFormFirstRowRef!: CoordinacionPmaFormComponent;
  @ViewChild('coordinacionPmaFormSecondRow') coordinacionPmaFormSecondRowRef!: CoordinacionPmaFormComponent;
  @ViewChild('coordinacionPmaFormThirdRow') coordinacionPmaFormThirdRowRef!: CoordinacionPmaFormComponent;

  private fb = inject(FormBuilder);
  private spinner = inject(NgxSpinnerService);
  private toast = inject(MatSnackBar);
  public directionService = inject(DirectionCoordinationService);
  public recordsService = inject(RecordsService);

  // Signals
  public isDataReady = signal<boolean>(false);
  public directionTypes = signal<DirectionType[]>([
    { id: 1, descripcion: 'Dirección' },
    { id: 2, descripcion: 'Coordinación CECOPI' },
    { id: 3, descripcion: 'Coordinación PMA' }
  ]);
  public showForm = signal<boolean>(false);

  // Propiedades para la tabla
  public displayedColumns: string[] = ['tipo', 'fechaInicio', 'fechaFin', 'direccion', 'autoridadDirige', 'acciones'];
  public dataSource = signal<any[]>([]);

  // Signal para tipos de direcciones de emergencias
  public tiposDireccionesEmergencias = signal<any[]>([]);

  // Variables para fechas
  public fechaMinimaDateTime = FECHA_MINIMA_DATETIME;
  public fechaMaximaDateTime = FECHA_MAXIMA_DATETIME;

  // Variable para archivo
  public file: File | null = null;

  // Signal para datos del formulario de dirección
  public direccionFormData = signal<any>({
    fechaHoraInicio: null,
    fechaHoraFin: null,
    lugar: null,
    file: null
  });

  // Formulario principal
  public formData: FormGroup;
  private direccionFirstRowData: any = {};
  private direccionSecondRowData: any = {};
  private isDireccionFirstRowValid: boolean = false;
  private isDireccionSecondRowValid: boolean = false;
  public initialDireccionData = signal<null>(null); // Para resetear los hijos

  // Variables para datos de coordinación CECOPI
  private coordinacionCecopiFirstRowData: any = {};
  private coordinacionCecopiSecondRowData: any = {};
  private coordinacionCecopiThirdRowData: any = {};
  private isCoordinacionCecopiFirstRowValid: boolean = false;
  private isCoordinacionCecopiSecondRowValid: boolean = false;
  private isCoordinacionCecopiThirdRowValid: boolean = true; // Las observaciones son opcionales

  // Variables para datos de coordinación PMA
  private coordinacionPmaFirstRowData: any = {};
  private coordinacionPmaSecondRowData: any = {};
  private coordinacionPmaThirdRowData: any = {};
  private isCoordinacionPmaFirstRowValid: boolean = false;
  private isCoordinacionPmaSecondRowValid: boolean = false;
  private isCoordinacionPmaThirdRowValid: boolean = true; // Las observaciones son opcionales

  // Propiedades para edición
  public isEditing = signal<boolean>(false);
  public editingRecord = signal<any>(null);
  public editingIndex = signal<number>(-1);
  public editingType = signal<number>(0); // 1=dirección, 2=cecopi, 3=pma
  public editingRealIndex = signal<number>(-1); // Índice real en el array correspondiente

  constructor() {
    this.formData = this.fb.group({
      tipoGestion: [null, Validators.required]
    });

    // Observar cambios en el tipo de gestión
    this.formData.get('tipoGestion')?.valueChanges.subscribe(tipoId => {
      console.log('Tipo de gestión seleccionado:', tipoId);
      this.actualizarValidadoresPorTipo(tipoId);
    });
  }

  // Actualizar validadores según el tipo seleccionado
  actualizarValidadoresPorTipo(tipoId: number) {
    const fechaInicioControl = this.formData.get('fechaHoraInicio');
    const fechaFinControl = this.formData.get('fechaHoraFin');
    const lugarControl = this.formData.get('lugar');
    
    if (tipoId === 1) { // Dirección
      fechaInicioControl?.setValidators([Validators.required]);
      fechaFinControl?.setValidators([Validators.required]);
      lugarControl?.setValidators([Validators.required]);
    } else {
      fechaInicioControl?.clearValidators();
      fechaFinControl?.clearValidators();
      lugarControl?.clearValidators();
    }
    
    fechaInicioControl?.updateValueAndValidity();
    fechaFinControl?.updateValueAndValidity();
    lugarControl?.updateValueAndValidity();
  }

  async ngOnInit(): Promise<void> {
    this.spinner.show();

    console.log('🚀 ~ DirectionCoordinationComponent ~ ¿Es nuevo registro?:', this.editData);
    
    try {
      // Cargar tipos de direcciones de emergencias
      const tiposDirecciones = await this.directionService.getTiposDireccionesEmergencias();
      this.tiposDireccionesEmergencias.set(tiposDirecciones);
      
      try {
        const registroData = await this.recordsService.getById(Number(this.registroId));
        console.log("🚀 ~ DirectionCoordinationComponent ~ ngOnInit ~ registroData:", registroData);

        // Si no hay datos o están vacíos, intentamos obtener registros anteriores
        if ((!registroData?.direcciones || registroData.direcciones.length === 0) &&
            (!registroData?.coordinacionesCecopi || registroData.coordinacionesCecopi.length === 0) &&
            (!registroData?.coordinacionesPMA || registroData.coordinacionesPMA.length === 0)) {
          if (this.registroId) {
           
            const registrosAnteriores = await this.recordsService.getRegistrosAnteriores(this.fire.idSuceso, this.registroId);
            console.log("🚀 ~ DirectionCoordinationComponent ~ ngOnInit ~ registrosAnteriores:", registrosAnteriores);

            if (registrosAnteriores[0]) {
              // Transformar y cargar los datos en el formato interno
              const gestionData: GestionData = {
                direcciones: [],
                coordinacionesCecopi: [],
                coordinacionesPMA: []
              };

              // Procesar direcciones
              if (registrosAnteriores[0].direcciones && registrosAnteriores[0].direcciones.length > 0) {
                gestionData.direcciones = registrosAnteriores[0].direcciones.map((direccion: any) => ({
                  id: 0,
                  fechaHoraInicio: direccion.fechaInicio ? direccion.fechaInicio.substring(0, 16) : '',
                  fechaHoraFin: direccion.fechaFin ? direccion.fechaFin.substring(0, 16) : '',
                  tipoDireccion: direccion.tipoDireccionEmergencia?.id,
                  lugar: direccion.autoridadQueDirige,
                  file: null,
                  archivo: direccion.archivo,
                  tipoGestion: 1,
                  esModificado: direccion.esModificado,
                }));
              }

              // Procesar coordinaciones CECOPI
              if (registrosAnteriores[0].coordinacionesCecopi && registrosAnteriores[0].coordinacionesCecopi.length > 0) {
                gestionData.coordinacionesCecopi = registrosAnteriores[0].coordinacionesCecopi.map((cecopi: any) => ({
                  id: 0,
                  fechaHoraInicio: cecopi.fechaInicio ? cecopi.fechaInicio.substring(0, 16) : '',
                  fechaHoraFin: cecopi.fechaFin ? cecopi.fechaFin.substring(0, 16) : '',
                  provincia: {
                    id: cecopi.provincia?.id,
                    descripcion: cecopi.provincia?.descripcion
                  },
                  municipio: {
                    id: cecopi.municipio?.id,
                    descripcion: cecopi.municipio?.descripcion
                  },
                  lugar: cecopi.lugar,
                  observaciones: cecopi.observaciones,
                  file: null,
                  archivo: cecopi.archivo,
                  tipoGestion: 2,
                  esModificado: cecopi.esModificado,
                  geoPosicion: cecopi.geoPosicion,
                }));
              }

              // Procesar coordinaciones PMA
              if (registrosAnteriores[0].coordinacionesPMA && registrosAnteriores[0].coordinacionesPMA.length > 0) {
                gestionData.coordinacionesPMA = registrosAnteriores[0].coordinacionesPMA.map((pma: any) => ({
                  id: 0,
                  fechaHoraInicio: pma.fechaInicio ? pma.fechaInicio.substring(0, 16) : '',
                  fechaHoraFin: pma.fechaFin ? pma.fechaFin.substring(0, 16) : '',
                  provincia: {
                    id: pma.provincia?.id,
                    descripcion: pma.provincia?.descripcion
                  },
                  municipio: {
                    id: pma.municipio?.id,
                    descripcion: pma.municipio?.descripcion
                  },
                  lugar: pma.lugar,
                  observaciones: pma.observaciones,
                  file: null,
                  archivo: pma.archivo,
                  tipoGestion: 3,
                  esModificado: pma.esModificado,
                  geoPosicion: pma.geoPosicion,
                }));
              }

              this.directionService.dataGestion.set(gestionData);
            }
          }
        } else {
          // Si hay datos, los usamos directamente
          const gestionData: GestionData = {
            direcciones: [],
            coordinacionesCecopi: [],
            coordinacionesPMA: []
          };

          // Procesar direcciones
          if (registroData.direcciones && registroData.direcciones.length > 0) {
            gestionData.direcciones = registroData.direcciones.map((direccion: any) => ({
              id: direccion.id,
              fechaHoraInicio: direccion.fechaInicio ? direccion.fechaInicio.substring(0, 16) : '',
              fechaHoraFin: direccion.fechaFin ? direccion.fechaFin.substring(0, 16) : '',
              tipoDireccion: direccion.tipoDireccionEmergencia?.id,
              lugar: direccion.autoridadQueDirige,
              file: null,
              archivo: direccion.archivo,
              tipoGestion: 1,
              esModificado: direccion.esModificado,
            }));
          }

          // Procesar coordinaciones CECOPI
          if (registroData.coordinacionesCecopi && registroData.coordinacionesCecopi.length > 0) {
            gestionData.coordinacionesCecopi = registroData.coordinacionesCecopi.map((cecopi: any) => ({
              id: cecopi.id,
              fechaHoraInicio: cecopi.fechaInicio ? cecopi.fechaInicio.substring(0, 16) : '',
              fechaHoraFin: cecopi.fechaFin ? cecopi.fechaFin.substring(0, 16) : '',
              provincia: {
                id: cecopi.provincia?.id,
                descripcion: cecopi.provincia?.descripcion
              },
              municipio: {
                id: cecopi.municipio?.id,
                descripcion: cecopi.municipio?.descripcion
              },
              lugar: cecopi.lugar,
              observaciones: cecopi.observaciones,
              file: null,
              archivo: cecopi.archivo,
              tipoGestion: 2,
              esModificado: cecopi.esModificado,
              geoPosicion: cecopi.geoPosicion,
            }));
          }

          // Procesar coordinaciones PMA
          if (registroData.coordinacionesPMA && registroData.coordinacionesPMA.length > 0) {
            gestionData.coordinacionesPMA = registroData.coordinacionesPMA.map((pma: any) => ({
              id: pma.id,
              fechaHoraInicio: pma.fechaInicio ? pma.fechaInicio.substring(0, 16) : '',
              fechaHoraFin: pma.fechaFin ? pma.fechaFin.substring(0, 16) : '',
              provincia: {
                id: pma.provincia?.id,
                descripcion: pma.provincia?.descripcion
              },
              municipio: {
                id: pma.municipio?.id,
                descripcion: pma.municipio?.descripcion
              },
              lugar: pma.lugar,
              observaciones: pma.observaciones,
              file: null,
              archivo: pma.archivo,
              tipoGestion: 3,
              esModificado: pma.esModificado,
              geoPosicion: pma.geoPosicion,
            }));
          }

          this.directionService.dataGestion.set(gestionData);
        }
        
        this.updateTableData();
        this.isDataReady.set(true);
      } catch (error) {
        console.error('Error al cargar datos del registro:', error);
        this.toast.open('Error al cargar datos del registro', 'Cerrar', {
          duration: 3000,
        });
      }
      
    } catch (error) {
      console.error('Error al cargar datos:', error);
      this.toast.open('Error al cargar datos', 'Cerrar', {
        duration: 3000,
      });
    } finally {
      this.spinner.hide();
    }
  }

  /**
   * Actualiza los datos de la tabla combinando todos los tipos de gestión
   */
  private updateTableData(): void {
    const data = this.directionService.dataGestion();
    const tableData: any[] = [];

    // Agregar direcciones
    data.direcciones.forEach((direccion, index) => {
      // Buscar la descripción del tipo de dirección
      const tipoDireccion = this.tiposDireccionesEmergencias().find(t => t.id === direccion.tipoDireccion);
      
      tableData.push({
        id: `direccion-${index}`,
        tipo: 'Dirección',
        tipoId: 1,
        realIndex: index, // Índice real en el array de direcciones
        fechaInicio: direccion.fechaHoraInicio,
        fechaFin: direccion.fechaHoraFin,
        direccion: tipoDireccion ? tipoDireccion.descripcion : '-',
        autoridadDirige: direccion.lugar || '-',
        tipoDireccion: direccion.tipoDireccion,
        originalData: direccion
      });
    });

    // Agregar coordinaciones CECOPI
    data.coordinacionesCecopi.forEach((cecopi, index) => {
      tableData.push({
        id: `cecopi-${index}`,
        tipo: 'Coordinación CECOPI',
        tipoId: 2,
        realIndex: index, // Índice real en el array de coordinaciones CECOPI
        fechaInicio: cecopi.fechaHoraInicio,
        fechaFin: cecopi.fechaHoraFin,
        direccion: '-',
        autoridadDirige: cecopi.lugar || '-',
        provincia: cecopi.provincia?.descripcion || '-',
        municipio: cecopi.municipio?.descripcion || '-',
        observaciones: cecopi.observaciones || '-',
        geoPosicion: cecopi.geoPosicion,
        originalData: cecopi,
      });
    });

    // Agregar coordinaciones PMA
    data.coordinacionesPMA.forEach((pma, index) => {
      tableData.push({
        id: `pma-${index}`,
        tipo: 'Coordinación PMA',
        tipoId: 3,
        realIndex: index, // Índice real en el array de coordinaciones PMA
        fechaInicio: pma.fechaHoraInicio,
        fechaFin: pma.fechaHoraFin,
        direccion: '-',
        autoridadDirige: pma.lugar || '-',
        provincia: pma.provincia?.descripcion || '-',
        municipio: pma.municipio?.descripcion || '-',
        observaciones: pma.observaciones || '-',
        geoPosicion: pma.geoPosicion,
        originalData: pma,
      });
    });

    this.dataSource.set(tableData);
  }

  /**
   * Formatea la fecha para mostrar en la tabla
   */
  public formatDate(date: string): string {
    if (!date) return '-';
  
    return DateUtils.fromUtcToCest(date, 'DD-MM-yyyy HH:mm')!;
  }

  /**
   * Elimina un registro de la tabla
   */
  public deleteRecord(element: any): void {
    const data = this.directionService.dataGestion();
    const realIndex = element.realIndex;
    
    if (element.tipoId === 1 && realIndex >= 0 && realIndex < data.direcciones.length) {
      data.direcciones.splice(realIndex, 1);
    } else if (element.tipoId === 2 && realIndex >= 0 && realIndex < data.coordinacionesCecopi.length) {
      data.coordinacionesCecopi.splice(realIndex, 1);
    } else if (element.tipoId === 3 && realIndex >= 0 && realIndex < data.coordinacionesPMA.length) {
      data.coordinacionesPMA.splice(realIndex, 1);
    } else {
      console.error('Error: No se pudo eliminar el registro', { tipoId: element.tipoId, realIndex, data });
      this.toast.open('Error al eliminar el registro', 'Cerrar', { duration: 3000 });
      return;
    }

    this.directionService.dataGestion.set(data);
    this.updateTableData();
    this.toast.open('Registro eliminado correctamente', 'Cerrar', { duration: 2000 });
    this.hasUnsavedChanges.emit(false);
    this.changesMade.emit(true);
  }

  /**
   * Edita un registro de la tabla
   */
  public editRecord(element: any): void {
    console.log('ELEMENT', element)

    // element.fechaHoraInicio = DateUtils.fromUtcToCest(element.fechaInicio),
    // element.fechaHoraFin = DateUtils.fromUtcToCest(element.fechaFin),
    element.originalData.fechaHoraInicio = DateUtils.fromUtcToCest(element.fechaInicio),
    element.originalData.fechaHoraFin = DateUtils.fromUtcToCest(element.fechaFin),

    // Establecer el estado de edición
    this.isEditing.set(true);
    this.editingRecord.set(element);
    this.editingType.set(element.tipoId);
    this.editingRealIndex.set(element.realIndex);
    
    // Mostrar el formulario
    this.showForm.set(true);
    
    // Establecer el tipo de gestión
    this.formData.patchValue({
      tipoGestion: element.tipoId
    });
    
    // Cargar los datos según el tipo
    if (element.tipoId === 1) { // Dirección
      // Preparar datos para el formulario de dirección
      const direccionData = {
        fechaHoraInicio: element.fechaInicio,
        fechaHoraFin: element.fechaFin,
        tipoDireccion: element.originalData.tipoDireccion,
        lugar: element.originalData.lugar,
        file: element.originalData.file
      };
      
      // Establecer los datos en las variables del componente
      this.direccionFirstRowData = {
        fechaHoraInicio: direccionData.fechaHoraInicio,
        fechaHoraFin: direccionData.fechaHoraFin,
        file: direccionData.file,
        archivo: element.originalData.archivo // Para archivos existentes
      };
      
      this.direccionSecondRowData = {
        tipoDireccion: direccionData.tipoDireccion,
        lugar: direccionData.lugar
      };
      
      // Establecer como válidos
      this.isDireccionFirstRowValid = true;
      this.isDireccionSecondRowValid = true;
      
    } else if (element.tipoId === 2) { // Coordinación CECOPI
      // Distribuir datos correctamente según la estructura del formulario:
      // Primera fila: fechas y archivo
      this.coordinacionCecopiFirstRowData = {
        fechaHoraInicio: element.originalData.fechaHoraInicio,
        fechaHoraFin: element.originalData.fechaHoraFin,
        file: element.originalData.file,
        archivo: element.originalData.archivo, // Para archivos existentes
        geoPosicion: element.originalData.geoPosicion,
      };
      // Segunda fila: lugar, provincia, municipio
      this.coordinacionCecopiSecondRowData = {
        lugar: element.originalData.lugar,
        provincia: element.originalData.provincia,
        municipio: element.originalData.municipio,
        geoPosicion: element.originalData.geoPosicion,
      };
      // Tercera fila: observaciones
      this.coordinacionCecopiThirdRowData = {
        observaciones: element.originalData.observaciones,
        geoPosicion: element.originalData.geoPosicion,
      };
      
      this.isCoordinacionCecopiFirstRowValid = true;
      this.isCoordinacionCecopiSecondRowValid = true;
      this.isCoordinacionCecopiThirdRowValid = true;
      
    } else if (element.tipoId === 3) { // Coordinación PMA
      // Distribuir datos correctamente según la estructura del formulario:
      // Primera fila: fechas y archivo
      this.coordinacionPmaFirstRowData = {
        fechaHoraInicio: element.originalData.fechaHoraInicio,
        fechaHoraFin: element.originalData.fechaHoraFin,
        file: element.originalData.file,
        archivo: element.originalData.archivo, // Para archivos existentes
        geoPosicion: element.originalData.geoPosicion,
      };
      // Segunda fila: lugar, provincia, municipio
      this.coordinacionPmaSecondRowData = {
        lugar: element.originalData.lugar,
        provincia: element.originalData.provincia,
        municipio: element.originalData.municipio,
        geoPosicion: element.originalData.geoPosicion,
      };
      // Tercera fila: observaciones
      this.coordinacionPmaThirdRowData = {
        observaciones: element.originalData.observaciones,
        geoPosicion: element.originalData.geoPosicion,
      };
      
      this.isCoordinacionPmaFirstRowValid = true;
      this.isCoordinacionPmaSecondRowValid = true;
      this.isCoordinacionPmaThirdRowValid = true;
    }
    
    // Trigger para que los componentes hijos se actualicen
    this.initialDireccionData.set(element.originalData);
    
    // Log de depuración para verificar distribución correcta de datos
    console.log('🔄 Datos distribuidos para edición:', {
      tipoId: element.tipoId,
      cecopiFirstRow: this.coordinacionCecopiFirstRowData,
      cecopiSecondRow: this.coordinacionCecopiSecondRowData,
      cecopiThirdRow: this.coordinacionCecopiThirdRowData,
      pmaFirstRow: this.coordinacionPmaFirstRowData,
      pmaSecondRow: this.coordinacionPmaSecondRowData,
      pmaThirdRow: this.coordinacionPmaThirdRowData,
      direccionFirstRow: this.direccionFirstRowData,
      direccionSecondRow: this.direccionSecondRowData
    });
    
    this.toast.open('Registro cargado para edición', 'Cerrar', { duration: 2000 });

    // No emitimos hasUnsavedChanges ni changesMade para que el botón Guardar no se habilite
  }

  // Método de utilidad para obtener controles del formulario
  getForm(atributo: string): any {
    return this.formData.controls[atributo];
  }

  // Método para mostrar el formulario
  showFormSection(): void {
    this.showForm.set(true);
    this.initialDireccionData.set(null); // Para triggerear OnChanges en los hijos
  }

  // Métodos para manejo de archivos
  dropped(files: NgxFileDropEntry[]): void {
    for (const droppedFile of files) {
      if (droppedFile.fileEntry.isFile) {
        const fileEntry = droppedFile.fileEntry as FileSystemFileEntry;
        
        fileEntry.file((file: File) => {
          this.file = file;
          this.direccionFormData.update(data => ({ ...data, file: this.file }));
          console.log('🚀 ~ DirectionCoordinationComponent ~ dropped ~ file:', file);
        });
      }
    }
  }

  fileOver(event: any): void {
    console.log('🚀 ~ DirectionCoordinationComponent ~ fileOver ~ event:', event);
  }

  fileLeave(event: any): void {
    console.log('🚀 ~ DirectionCoordinationComponent ~ fileLeave ~ event:', event);
  }

  // Manejadores para la primera instancia del form hijo
  onDireccionFirstRowFormChange(data: any): void {
    this.direccionFirstRowData = { ...this.direccionFirstRowData, ...data };
    
    // Si hay un archivo, obtener la referencia real del componente hijo
    if (data.file && this.direccionFormFirstRowRef) {
      this.direccionFirstRowData.file = this.direccionFormFirstRowRef.file;
    }
  }
  onDireccionFirstRowValidityChange(isValid: boolean): void {
    this.isDireccionFirstRowValid = isValid;
  }

  // Manejadores para la segunda instancia del form hijo
  onDireccionSecondRowFormChange(data: any): void {
    this.direccionSecondRowData = { ...this.direccionSecondRowData, ...data };
  }
  onDireccionSecondRowValidityChange(isValid: boolean): void {
    this.isDireccionSecondRowValid = isValid;
  }

  // Métodos para manejar cambios en el formulario de coordinación CECOPI
  // Primera fila
  onCoordinacionCecopiFirstRowFormChange(data: any): void {
    this.coordinacionCecopiFirstRowData = { ...this.coordinacionCecopiFirstRowData, ...data };
    
    // Si hay un archivo, obtener la referencia real del componente hijo
    if (data.file && this.coordinacionCecopiFormFirstRowRef) {
      this.coordinacionCecopiFirstRowData.file = this.coordinacionCecopiFormFirstRowRef.file; // El objeto File real
    }
  }

  onCoordinacionCecopiFirstRowValidityChange(isValid: boolean): void {
    this.isCoordinacionCecopiFirstRowValid = isValid;
  }

  // Segunda fila
  onCoordinacionCecopiSecondRowFormChange(data: any): void {
    this.coordinacionCecopiSecondRowData = { ...this.coordinacionCecopiSecondRowData, ...data };
    
    // Si hay un archivo, obtener la referencia real del componente hijo
    if (data.file && this.coordinacionCecopiFormSecondRowRef) {
      this.coordinacionCecopiSecondRowData.file = this.coordinacionCecopiFormSecondRowRef.file; // El objeto File real
    }
  }

  onCoordinacionCecopiSecondRowValidityChange(isValid: boolean): void {
    this.isCoordinacionCecopiSecondRowValid = isValid;
  }

  // Tercera fila
  onCoordinacionCecopiThirdRowFormChange(data: any): void {
    this.coordinacionCecopiThirdRowData = { ...this.coordinacionCecopiThirdRowData, ...data };
    
    // Si hay un archivo, obtener la referencia real del componente hijo
    if (data.file && this.coordinacionCecopiFormThirdRowRef) {
      this.coordinacionCecopiThirdRowData.file = this.coordinacionCecopiFormThirdRowRef.file; // El objeto File real
    }
  }

  onCoordinacionCecopiThirdRowValidityChange(isValid: boolean): void {
    this.isCoordinacionCecopiThirdRowValid = isValid;
  }

  // Métodos para manejar cambios en el formulario de coordinación PMA
  // Primera fila
  onCoordinacionPmaFirstRowFormChange(data: any): void {
    this.coordinacionPmaFirstRowData = { ...this.coordinacionPmaFirstRowData, ...data };
    
    // Si hay un archivo, obtener la referencia real del componente hijo
    if (data.file && this.coordinacionPmaFormFirstRowRef) {
      this.coordinacionPmaFirstRowData.file = this.coordinacionPmaFormFirstRowRef.file; // El objeto File real
    }
  }

  onCoordinacionPmaFirstRowValidityChange(isValid: boolean): void {
    this.isCoordinacionPmaFirstRowValid = isValid;
  }

  // Segunda fila
  onCoordinacionPmaSecondRowFormChange(data: any): void {
    this.coordinacionPmaSecondRowData = { ...this.coordinacionPmaSecondRowData, ...data };
    
    // Si hay un archivo, obtener la referencia real del componente hijo
    if (data.file && this.coordinacionPmaFormSecondRowRef) {
      this.coordinacionPmaSecondRowData.file = this.coordinacionPmaFormSecondRowRef.file; // El objeto File real
    }
  }

  onCoordinacionPmaSecondRowValidityChange(isValid: boolean): void {
    this.isCoordinacionPmaSecondRowValid = isValid;
  }

  // Tercera fila
  onCoordinacionPmaThirdRowFormChange(data: any): void {
    this.coordinacionPmaThirdRowData = { ...this.coordinacionPmaThirdRowData, ...data };
    
    // Si hay un archivo, obtener la referencia real del componente hijo
    if (data.file && this.coordinacionPmaFormThirdRowRef) {
      this.coordinacionPmaThirdRowData.file = this.coordinacionPmaFormThirdRowRef.file; // El objeto File real
    }
  }

  onCoordinacionPmaThirdRowValidityChange(isValid: boolean): void {
    this.isCoordinacionPmaThirdRowValid = isValid;
  }

  // Métodos para datos iniciales de componentes hijos
  getInitialDireccionData() {
    return this.initialDireccionData();
  }
  
  initialCoordinacionCecopiData() {
    return null;
  }

  initialCoordinacionPmaData() {
    return null;
  }

  // Método para guardar los datos
  async onSubmit(): Promise<void> {
    if (!this.formData.valid) {
      this.formData.markAllAsTouched();
      this.toast.open('Por favor seleccione un tipo de gestión', 'Cerrar', { duration: 3000 });
      return;
    }

    if (this.isEditing()) {
      const tipoGestion = this.formData.value.tipoGestion;
      let datosAGuardar: any = {};

      if (tipoGestion === 1) { // Dirección
        let formDireccionValido = true;
        if (!this.isDireccionFirstRowValid) {
          this.direccionFormFirstRowRef?.markAsTouched();
          formDireccionValido = false;
        }
        if (!this.isDireccionSecondRowValid) {
          this.direccionFormSecondRowRef?.markAsTouched();
          formDireccionValido = false;
        }

        if (!formDireccionValido) {
          this.toast.open('Por favor complete todos los campos requeridos en el formulario de Dirección', 'Cerrar', { duration: 3000 });
          return;
        }
        datosAGuardar = { ...this.direccionFirstRowData, ...this.direccionSecondRowData };
      } else if (tipoGestion === 2) {
        let formCecopiValido = true;
        if (!this.isCoordinacionCecopiFirstRowValid) {
          this.coordinacionCecopiFormFirstRowRef?.markAsTouched();
          formCecopiValido = false;
        }
        if (!this.isCoordinacionCecopiSecondRowValid) {
          this.coordinacionCecopiFormSecondRowRef?.markAsTouched();
          formCecopiValido = false;
        }
        // La tercera fila (observaciones) es opcional, no necesita validación

        if (!formCecopiValido) {
          this.toast.open('Por favor complete todos los campos requeridos en el formulario de Coordinación CECOPI', 'Cerrar', { duration: 3000 });
          return;
        }
        datosAGuardar = { ...this.coordinacionCecopiFirstRowData, ...this.coordinacionCecopiSecondRowData, ...this.coordinacionCecopiThirdRowData, tipoGestion: tipoGestion };
      } else if (tipoGestion === 3) {
        let formPmaValido = true;
        if (!this.isCoordinacionPmaFirstRowValid) {
          this.coordinacionPmaFormFirstRowRef?.markAsTouched();
          formPmaValido = false;
        }
        if (!this.isCoordinacionPmaSecondRowValid) {
          this.coordinacionPmaFormSecondRowRef?.markAsTouched();
          formPmaValido = false;
        }
        // La tercera fila (observaciones) es opcional, no necesita validación

        if (!formPmaValido) {
          this.toast.open('Por favor complete todos los campos requeridos en el formulario de Coordinación PMA', 'Cerrar', { duration: 3000 });
          return;
        }
        datosAGuardar = { ...this.coordinacionPmaFirstRowData, ...this.coordinacionPmaSecondRowData, ...this.coordinacionPmaThirdRowData, tipoGestion: tipoGestion };
      }

      const currentData: GestionData = {
        direcciones: [...this.directionService.dataGestion().direcciones],
        coordinacionesCecopi: [...this.directionService.dataGestion().coordinacionesCecopi],
        coordinacionesPMA: [...this.directionService.dataGestion().coordinacionesPMA]
      };

      // Modo edición: actualizar registro existente usando índices reales
      const editingType = this.editingType();
      const realIndex = this.editingRealIndex();
      
      console.log('🔄 Editando registro:', { editingType, realIndex, datosAGuardar, currentData });
      datosAGuardar.fechaHoraInicio = DateUtils.fromCestToUtc(datosAGuardar.fechaHoraInicio);
      datosAGuardar.fechaHoraFin = DateUtils.fromCestToUtc(datosAGuardar.fechaHoraFin);
      
      if (editingType === 1 && realIndex >= 0 && realIndex < currentData.direcciones.length) {
        // Conservar archivo existente si no se cambió
        const originalData = currentData.direcciones[realIndex];
        if (!datosAGuardar.file && originalData.archivo) {
          datosAGuardar.archivo = originalData.archivo;
        }
        currentData.direcciones[realIndex] = datosAGuardar;
      } else if (editingType === 2 && realIndex >= 0 && realIndex < currentData.coordinacionesCecopi.length) {
        // Conservar archivo existente si no se cambió
        const originalData = currentData.coordinacionesCecopi[realIndex];
        if (!datosAGuardar.file && originalData.archivo) {
          datosAGuardar.archivo = originalData.archivo;
        }
        currentData.coordinacionesCecopi[realIndex] = datosAGuardar;
      } else if (editingType === 3 && realIndex >= 0 && realIndex < currentData.coordinacionesPMA.length) {
        // Conservar archivo existente si no se cambió
        const originalData = currentData.coordinacionesPMA[realIndex];
        if (!datosAGuardar.file && originalData.archivo) {
          datosAGuardar.archivo = originalData.archivo;
        }

        currentData.coordinacionesPMA[realIndex] = datosAGuardar;
      } else {
        console.error('Error: Índice de edición inválido', { editingType, realIndex, currentData });
        this.toast.open('Error al actualizar el registro', 'Cerrar', { duration: 3000 });
        return;
      }
      
      this.directionService.dataGestion.set(currentData);
      this.updateTableData();
      this.save.emit({ success: true, data: currentData });
      
      this.toast.open('Registro actualizado correctamente', 'Cerrar', { duration: 2000 });
      
      // Resetear formulario y estado de edición
      this.resetForm();

      this.hasUnsavedChanges.emit(false);
      this.changesMade.emit(true);

      this.showForm.set(false);
    } else {
      // Si no está en modo edición, usar addRecord
      await this.addRecord();
    }
  }

  /**
   * Resetea el formulario y el estado de edición
   */
  private resetForm(): void {
    this.formData.reset({ tipoGestion: null });
    this.initialDireccionData.set(null);
    this.direccionFirstRowData = {};
    this.direccionSecondRowData = {};
    this.isDireccionFirstRowValid = false;
    this.isDireccionSecondRowValid = false;
    this.coordinacionCecopiFirstRowData = {};
    this.coordinacionCecopiSecondRowData = {};
    this.coordinacionCecopiThirdRowData = {};
    this.isCoordinacionCecopiFirstRowValid = false;
    this.isCoordinacionCecopiSecondRowValid = false;
    this.isCoordinacionCecopiThirdRowValid = true;
    this.coordinacionPmaFirstRowData = {};
    this.coordinacionPmaSecondRowData = {};
    this.coordinacionPmaThirdRowData = {};
    this.isCoordinacionPmaFirstRowValid = false;
    this.isCoordinacionPmaSecondRowValid = false;
    this.isCoordinacionPmaThirdRowValid = true;
    
    // Resetear estado de edición
    this.isEditing.set(false);
    this.editingRecord.set(null);
    this.editingIndex.set(-1);
    this.editingType.set(0);
    this.editingRealIndex.set(-1);
  }

  /**
   * Cancela la edición y oculta el formulario
   */
  public cancelEdit(): void {
    this.resetForm();
    this.showForm.set(false);
    this.changesMade.emit(false);
    this.hasUnsavedChanges.emit(false);    
  }

  /**
   * Método público para guardar todos los datos de dirección y coordinación
   * Este método será llamado desde el componente padre cuando se haga clic en "Guardar"
   */
  public async saveAllData(idSuceso: number, idRegistroActualizacion: number): Promise<any> {
    try {
      const currentData = this.directionService.dataGestion();
      
      // Verificar si hay datos para guardar
      if (currentData.direcciones.length === 0 && 
          currentData.coordinacionesCecopi.length === 0 && 
          currentData.coordinacionesPMA.length === 0) {
        console.log('No hay datos de dirección y coordinación para guardar');
        return { success: true, message: 'No hay datos para guardar' };
      }
      
      // Verificar archivos específicamente
      console.log('🔍 Verificando archivos en los datos:');
      currentData.direcciones.forEach((dir, i) => {
        if (dir.file) console.log(`📁 Dirección ${i} - Archivo:`, dir.file.name, dir.file.size, 'bytes');
        // dir.fechaHoraInicio = dir.fechaHoraInicio ? DateUtils.fromCestToUtc(dir.fechaHoraInicio) : null;
        // dir.fechaHoraFin = dir.fechaHoraFin ? DateUtils.fromCestToUtc(dir.fechaHoraFin) : null;
      });
      currentData.coordinacionesCecopi.forEach((cecopi, i) => {
        // cecopi.fechaHoraInicio = cecopi.fechaHoraInicio ? DateUtils.fromCestToUtc(cecopi.fechaHoraInicio) : null;
        // cecopi.fechaHoraFin = cecopi.fechaHoraFin ? DateUtils.fromCestToUtc(cecopi.fechaHoraFin) : null;
        if (cecopi.file) console.log(`📁 CECOPI ${i} - Archivo:`, cecopi.file.name, cecopi.file.size, 'bytes');
      });
      currentData.coordinacionesPMA.forEach((pma, i) => {
        // pma.fechaHoraInicio = pma.fechaHoraInicio ? DateUtils.fromCestToUtc(pma.fechaHoraInicio) : null;
        // pma.fechaHoraFin = pma.fechaHoraFin ? DateUtils.fromCestToUtc(pma.fechaHoraFin) : null;
        if (pma.file) console.log(`📁 PMA ${i} - Archivo:`, pma.file.name, pma.file.size, 'bytes');
      });

      console.log('Enviando datos de dirección y coordinación:', currentData);
      
      const response = await this.directionService.postDireccionCoordinacion(
        currentData, 
        idSuceso, 
        idRegistroActualizacion
      );
      
      console.log('Respuesta del servidor:', response);
      
      this.toast.open('Datos de dirección y coordinación guardados correctamente', 'Cerrar', { 
        duration: 3000,
        panelClass: ['snackbar-verde']
      });
      
      return { success: true, data: response };
      
    } catch (error) {
      console.error('Error al guardar datos de dirección y coordinación:', error);
      
      this.toast.open('Error al guardar datos de dirección y coordinación', 'Cerrar', { 
        duration: 5000,
        panelClass: ['snackbar-error']
      });
      
      throw error;
    }
  }

  /**
   * Verifica si hay datos pendientes de guardar
   */
  public hasDataToSave(): boolean {
    const currentData = this.directionService.dataGestion();
    return currentData.direcciones.length > 0 || 
           currentData.coordinacionesCecopi.length > 0 || 
           currentData.coordinacionesPMA.length > 0;
  }

  /**
   * Método público para limpiar todas las variables y datos del componente
   * Este método será llamado desde el componente padre cuando se cierre el modal
   */
  public clearAllData(): void {
    console.log('Limpiando todos los datos de dirección y coordinación...');
    
    // Limpiar datos del servicio
    this.directionService.clearData();
    
    // Resetear formulario principal
    this.formData.reset({ tipoGestion: null });
    
    // Limpiar variables de dirección
    this.direccionFirstRowData = {};
    this.direccionSecondRowData = {};
    this.isDireccionFirstRowValid = false;
    this.isDireccionSecondRowValid = false;
    
    // Limpiar variables de coordinación CECOPI
    this.coordinacionCecopiFirstRowData = {};
    this.coordinacionCecopiSecondRowData = {};
    this.coordinacionCecopiThirdRowData = {};
    this.isCoordinacionCecopiFirstRowValid = false;
    this.isCoordinacionCecopiSecondRowValid = false;
    this.isCoordinacionCecopiThirdRowValid = true;
    
    // Limpiar variables de coordinación PMA
    this.coordinacionPmaFirstRowData = {};
    this.coordinacionPmaSecondRowData = {};
    this.coordinacionPmaThirdRowData = {};
    this.isCoordinacionPmaFirstRowValid = false;
    this.isCoordinacionPmaSecondRowValid = false;
    this.isCoordinacionPmaThirdRowValid = true;
    
    // Resetear estado de edición
    this.isEditing.set(false);
    this.editingRecord.set(null);
    this.editingIndex.set(-1);
    
    // Ocultar formulario
    this.showForm.set(false);
    
    // Resetear datos iniciales para componentes hijos
    this.initialDireccionData.set(null);
    
    // Limpiar archivo
    this.file = null;
    this.direccionFormData.set({
      fechaHoraInicio: null,
      fechaHoraFin: null,
      lugar: null,
      file: null
    });
    
    // Actualizar tabla (debería estar vacía)
    this.updateTableData();
    
    console.log('Datos de dirección y coordinación limpiados correctamente');
  }

  // Método para validar todos los formularios según el tipo de gestión seleccionado
  private validateAllForms(): boolean {
    const tipoGestion = this.formData.get('tipoGestion')?.value;
    
    if (!tipoGestion) {
      this.toast.open('Debe seleccionar un tipo de gestión', 'Cerrar', { duration: 3000 });
      return false;
    }

    switch (tipoGestion) {
      case 1: // Dirección
        if (!this.isDireccionFirstRowValid || !this.isDireccionSecondRowValid) {
          this.toast.open('Por favor complete todos los campos obligatorios', 'Cerrar', { duration: 3000 });
          return false;
        }
        break;
      case 2: // CECOPI
        if (!this.isCoordinacionCecopiFirstRowValid || !this.isCoordinacionCecopiSecondRowValid) {
          this.toast.open('Por favor complete todos los campos obligatorios', 'Cerrar', { duration: 3000 });
          return false;
        }
        break;
      case 3: // PMA
        if (!this.isCoordinacionPmaFirstRowValid || !this.isCoordinacionPmaSecondRowValid) {
          this.toast.open('Por favor complete todos los campos obligatorios', 'Cerrar', { duration: 3000 });
          return false;
        }
        break;
    }

    return true;
  }

  // Método para obtener los valores del formulario según el tipo
  private getFormValues(): any {
    const tipoGestion = this.formData.get('tipoGestion')?.value;

    console.log(this.coordinacionPmaFirstRowData, this.coordinacionPmaSecondRowData, this.coordinacionPmaThirdRowData)
    
    switch (tipoGestion) {
      case 1: // Dirección
        return {
          ...this.direccionFirstRowData,
          ...this.direccionSecondRowData
        };
      case 2: // CECOPI
        const geoPosicionCecopi =
          this.coordinacionCecopiFirstRowData.geoPosicion ??
          this.coordinacionCecopiSecondRowData.geoPosicion??
          this.coordinacionCecopiThirdRowData.geoPosicion;
  
        return {
          ...this.coordinacionCecopiFirstRowData,
          ...this.coordinacionCecopiSecondRowData,
          ...this.coordinacionCecopiThirdRowData,
          geoPosicion: geoPosicionCecopi,
        };
      case 3: // PMA
        const geoPosicionPma =
          this.coordinacionPmaFirstRowData.geoPosicion ??
          this.coordinacionPmaSecondRowData.geoPosicion??
          this.coordinacionPmaThirdRowData.geoPosicion;

        return {
          ...this.coordinacionPmaFirstRowData,
          ...this.coordinacionPmaSecondRowData,
          ...this.coordinacionPmaThirdRowData,
          geoPosicion: geoPosicionPma,
        };
      default:
        return {};
    }
  }

  // Método para resetear los formularios
  private resetForms(): void {
    // Resetear el formulario principal
    this.formData.reset();
    
    // Resetear los datos de los formularios hijos
    this.direccionFirstRowData = {};
    this.direccionSecondRowData = {};
    this.coordinacionCecopiFirstRowData = {};
    this.coordinacionCecopiSecondRowData = {};
    this.coordinacionCecopiThirdRowData = {};
    this.coordinacionPmaFirstRowData = {};
    this.coordinacionPmaSecondRowData = {};
    this.coordinacionPmaThirdRowData = {};
    
    // Resetear las validaciones
    this.isDireccionFirstRowValid = false;
    this.isDireccionSecondRowValid = false;
    this.isCoordinacionCecopiFirstRowValid = false;
    this.isCoordinacionCecopiSecondRowValid = false;
    this.isCoordinacionPmaFirstRowValid = false;
    this.isCoordinacionPmaSecondRowValid = false;
  }

  // Método para cargar los registros
  private async loadRecords(): Promise<void> {
    this.updateTableData();
  }

  async addRecord(): Promise<void> {
    try {
      this.spinner.show();
      
      // Validar que todos los formularios estén completos
      if (!this.validateAllForms()) {
        this.spinner.hide();
        return;
      }

      // Obtener valores de los formularios
      const recordData = this.getFormValues();
      const tipoGestion = this.formData.get('tipoGestion')?.value;

      recordData.fechaHoraInicio = DateUtils.fromCestToUtc(recordData.fechaHoraInicio);
      recordData.fechaHoraFin = DateUtils.fromCestToUtc(recordData.fechaHoraFin);

      console.log('RECORD_DATA', recordData)

      // Obtener los datos actuales
      const currentData = this.directionService.dataGestion();

      // Agregar el nuevo registro según el tipo
      switch (tipoGestion) {
        case 1: // Dirección
          const directionRecord = !!currentData.direcciones.find(
            (item) => item.fechaHoraInicio === recordData.fechaHoraInicio
          );

          if (directionRecord) {
            this.toast.open('Ya existe un registro con el mismo tipo y fecha seleccionados.', 'Cerrar', { duration: 3000 });
            return;
          }

          currentData.direcciones.push(recordData);
          break;
        case 2: // CECOPI
          const cecopiRecord = !!currentData.direcciones.find(
            (item) => item.fechaHoraInicio === recordData.fechaHoraInicio
          );

          if (cecopiRecord) {
            this.toast.open('Ya existe un registro con el mismo tipo y fecha seleccionados.', 'Cerrar', { duration: 3000 });
            return;
          }

          currentData.coordinacionesCecopi.push(recordData);
          break;
        case 3: // PMA
          const pmaRecord = !!currentData.direcciones.find(
            (item) => item.fechaHoraInicio === recordData.fechaHoraInicio
          );

          if (pmaRecord) {
            this.toast.open('Ya existe un registro con el mismo tipo y fecha seleccionados.', 'Cerrar', { duration: 3000 });
            return;
          }

          currentData.coordinacionesPMA.push(recordData);
          break;
      }

      // Actualizar los datos en el servicio
      this.directionService.dataGestion.set(currentData);

      // Actualizar la tabla
      this.updateTableData();

      // Mostrar mensaje de éxito
      this.toast.open('Registro agregado correctamente', 'Cerrar', { duration: 3000 });

      // Ocultar el formulario y resetear el estado
      this.resetForms();
      this.showForm.set(false);

      // Emitir que se han realizado cambios
      this.changesMade.emit(true);
      this.hasUnsavedChanges.emit(false);
      
    } catch (error) {
      console.error('Error al agregar registro:', error);
      this.toast.open('Error al agregar el registro', 'Cerrar', { duration: 3000 });
    } finally {
      this.spinner.hide();
    }
  }
} 