import { Component, OnInit, inject, signal, Input, Output, EventEmitter, ChangeDetectorRef, NO_ERRORS_SCHEMA, OnDestroy } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormBuilder, FormGroup, ReactiveFormsModule, Validators, AbstractControl, ValidationErrors } from '@angular/forms';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { MatSelectModule } from '@angular/material/select';
import { MatButtonModule } from '@angular/material/button';
import { MatIconModule } from '@angular/material/icon';
import { MatSnackBar } from '@angular/material/snack-bar';
import { MatRowDef, MatTableModule } from '@angular/material/table';
import { MatTooltipModule } from '@angular/material/tooltip';
import { FlexLayoutModule } from '@angular/flex-layout';
import { NgxSpinnerModule, NgxSpinnerService } from 'ngx-spinner';
import { NgxFileDropEntry, FileSystemFileEntry, NgxFileDropModule } from 'ngx-file-drop';

// Importar servicios y tipos
import { SpecialPlansActivationService, SpecialPlansData, ActivacionPlan } from '../../../../services/special-plans-activation.service';
import { RecordsService } from '../../../../services/records.service';
import { AlertService } from '../../../../shared/alert/alert.service';
import { FECHA_MAXIMA_DATETIME, FECHA_MINIMA_DATETIME } from '@type/constants';
import { DateUtils } from '@shared/utils/date-utils';

@Component({
  selector: 'app-special-plans-activation',
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
    MatRowDef,
  ],
  templateUrl: './special-plans-activation.component.html',
  styleUrls: ['./special-plans-activation.component.scss'],
})
export class SpecialPlansActivationComponent implements OnInit, OnDestroy {
  @Input() fire: any;
  @Input() editData: any;
  @Input() esUltimo: boolean | undefined;
  @Input() isNewRecord: boolean | undefined;
  @Input() registroId: number | null = null;
  @Input() estadoIncendio: any;

  @Output() save = new EventEmitter<any>();
  @Output() changesMade = new EventEmitter<boolean>();
  @Output() unsavedChanges = new EventEmitter<boolean>();

  private fb = inject(FormBuilder);
  private spinner = inject(NgxSpinnerService);
  private toast = inject(MatSnackBar);
  public specialPlansService = inject(SpecialPlansActivationService);
  public recordsService = inject(RecordsService);
  public alertService = inject(AlertService);

  // Signals
  public isDataReady = signal<boolean>(false);
  public showForm = signal<boolean>(false);
  public isLoadingPlanes = signal<boolean>(false);
  public allUsedPlanes = signal<Map<number, any>>(new Map()); // Mapa para mantener todos los planes usados

  // Propiedades para la tabla
  public displayedColumns: string[] = ['tipoPlan', 'planEmergencia', 'fechaInicio', 'fechaFin', 'autoridad', 'acciones'];
  public dataSource = signal<any[]>([]);

  // Signals para datos dummy
  public tiposPlanes = signal<any[]>([]);
  public planesEmergencia = signal<any[]>([]);

  // Variables para fechas
  public fechaMinimaDateTime = FECHA_MINIMA_DATETIME;
  public fechaMaximaDateTime = FECHA_MAXIMA_DATETIME;

  // Variable para archivo
  public file: File | null = null;
  public existingFile: any = null; // Para archivos existentes del servidor

  // Formulario principal
  public formData: FormGroup = this.fb.group({
    idTipoPlan: [null, Validators.required],
    idPlanEmergencia: [null, Validators.required],
    fechaInicio: ['', [Validators.required, this.validarFechaInicioConHora.bind(this)]],
    fechaFin: ['', [this.validarFechaFinConHora.bind(this)]],
    autoridad: [null, Validators.required],
    observaciones: [null],
  });

  // Propiedades para edición
  public isEditing = signal<boolean>(false);
  public editingRecord = signal<any>(null);
  public editingIndex = signal<number>(-1);

  // Variable para mantener datos originales del servidor
  private originalServerData: SpecialPlansData = { activacionPlanes: [] };

  // Signal para forzar re-renderización de la tabla
  public forceTableRerender = signal<boolean>(true);

  constructor(private changeDetectorRef: ChangeDetectorRef) {
    this.createForm();
  }

  async ngOnInit(): Promise<void> {
    this.spinner.show();
    console.log('🚀 ~ SpecialPlansActivationComponent ~ ngOnInit ~ this.fire:', this.fire);
    try {
      // Cargar tipos de planes
      const [tiposPlanes] = await Promise.all([this.specialPlansService.getTiposPlanes(this.fire?.suceso?.idTipo)]);

      this.tiposPlanes.set(tiposPlanes);

      // Inicializar datos originales con estructura vacía por defecto
      if (!this.originalServerData || this.originalServerData.activacionPlanes.length === 0) {
        this.originalServerData = { activacionPlanes: [] };
      }

      // Cargar datos existentes
      try {
        const registroData = await this.recordsService.getById(Number(this.registroId));
        console.log('Datos del registro:', registroData);

        // Si no hay datos de planes especiales o está vacío, intentamos obtener registros anteriores
        if (!registroData?.activacionPlanEmergencias || registroData.activacionPlanEmergencias.length === 0) {
          const registrosAnteriores = await this.recordsService.getRegistrosAnteriores(Number(this.fire.idSuceso), this.registroId!);
          console.log('Datos de registros anteriores:', registrosAnteriores);

          if (registrosAnteriores[0] && registrosAnteriores[0].activacionPlanEmergencias?.length > 0) {
            const specialPlansData: SpecialPlansData = {
              activacionPlanes: registrosAnteriores[0].activacionPlanEmergencias.map((plan: any) => ({
                id: 0,
                idTipoPlan: plan.tipoPlan?.id,
                idPlanEmergencia: plan.planEmergencia?.id,
                tipoPlan: plan.tipoPlan,
                planEmergencia: plan.planEmergencia,
                fechaInicio: plan.fechaHoraInicio || plan.fechaInicio || '',
                fechaFin: plan.fechaHoraFin || plan.fechaFin || null,
                autoridad: plan.autoridad,
                observaciones: plan.observaciones,
                file: null,
                archivo: plan.archivo,
                fileAction: plan.archivo ? 'keep_existing' : 'none',
                esModificado: plan.esModificado,
              })),
            };

            this.specialPlansService.dataActivacionPlanes.set(specialPlansData);
            this.originalServerData = JSON.parse(JSON.stringify(specialPlansData));
          }
        } else {
          // Si hay datos de planes especiales, los usamos
          const specialPlansData: SpecialPlansData = {
            activacionPlanes: registroData.activacionPlanEmergencias.map((plan: any) => ({
              id: plan.id,
              idTipoPlan: plan.tipoPlan?.id,
              idPlanEmergencia: plan.planEmergencia?.id,
              tipoPlan: plan.tipoPlan,
              planEmergencia: plan.planEmergencia,
              fechaInicio: plan.fechaHoraInicio || plan.fechaInicio || '',
              fechaFin: plan.fechaHoraFin || plan.fechaFin || null,
              autoridad: plan.autoridad,
              observaciones: plan.observaciones,
              file: null,
              archivo: plan.archivo,
              fileAction: plan.archivo ? 'keep_existing' : 'none',
              esModificado: plan.esModificado,
            })),
          };

          this.specialPlansService.dataActivacionPlanes.set(specialPlansData);
          this.originalServerData = JSON.parse(JSON.stringify(specialPlansData));
        }
      } catch (error) {
        console.error('Error al cargar los datos:', error);
        this.toast.open('Error al cargar los datos', 'Cerrar', { duration: 3000 });
      }

      this.updateTableData();
      this.isDataReady.set(true);
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
   * Actualiza los datos de la tabla
   */
  private updateTableData(): void {
    const data = this.specialPlansService.dataActivacionPlanes();
    console.log('DATA_PLANES', data);
    const tableData: any[] = [];
    const currentUsedPlanes = new Map(this.allUsedPlanes());

    console.log('Datos originales para la tabla:', data);

    // Agregar activaciones de planes
    data.activacionPlanes.forEach((plan, index) => {
      // Buscar las descripciones de los selects
      const tipoPlan = this.tiposPlanes().find((t) => t.id === plan.idTipoPlan);

      // Intentar obtener el plan de emergencia del mapa de planes usados
      let planEmergenciaDescripcion = '-';

      // Si el plan viene con el objeto completo (del endpoint)
      if (plan.planEmergencia?.descripcion) {
        planEmergenciaDescripcion = plan.planEmergencia.descripcion;
        // Guardar en el mapa de planes usados
        currentUsedPlanes.set(plan.idPlanEmergencia, plan.planEmergencia);
      }
      // Si no, buscar en el mapa de planes usados
      else if (currentUsedPlanes.has(plan.idPlanEmergencia)) {
        planEmergenciaDescripcion = currentUsedPlanes.get(plan.idPlanEmergencia).descripcion;
      }
      // Si no, buscar en los planes actuales
      else {
        const planEmergencia = this.planesEmergencia().find((p) => p.id === plan.idPlanEmergencia);
        if (planEmergencia) {
          planEmergenciaDescripcion = planEmergencia.descripcion;
          // Guardar en el mapa de planes usados
          currentUsedPlanes.set(plan.idPlanEmergencia, planEmergencia);
        }
      }
      

      // Convertir fechas UTC a local para mostrar
      const fechaInicio = plan.fechaInicio ? DateUtils.fromUtcToCest(plan.fechaInicio) : null;
      const fechaFin = plan.fechaFin ? DateUtils.fromUtcToCest(plan.fechaFin) : null;

      tableData.push({
        id: `plan-${index}`,
        realIndex: index,
        tipoPlan: tipoPlan ? tipoPlan.descripcion : plan.tipoPlan?.descripcion || '-',
        planEmergencia: planEmergenciaDescripcion,
        fechaInicio: plan.fechaInicio,
        fechaFin: plan.fechaFin,
        autoridad: plan.autoridad || '-',
        observaciones: plan.observaciones || '-',
        originalData: plan,
      });
    });

    // Actualizar el mapa de planes usados
    this.allUsedPlanes.set(currentUsedPlanes);
    console.log('Planes usados actualizados:', currentUsedPlanes);

    console.log('Datos finales para la tabla:', tableData);
    this.dataSource.set(tableData);
  }

  // Método auxiliar para convertir fechas UTC a local
  private convertUTCToLocal(utcDate: string): string {
    // Crear fecha UTC
    const fechaUTC = new Date(utcDate);

    // Convertir a fecha local
    const fechaLocal = new Date(
      fechaUTC.getUTCFullYear(),
      fechaUTC.getUTCMonth(),
      fechaUTC.getUTCDate(),
      fechaUTC.getUTCHours(),
      fechaUTC.getUTCMinutes()
    );

    console.log('Conversión de fecha:', {
      utcOriginal: utcDate,
      fechaUTC: fechaUTC.toISOString(),
      fechaLocal: fechaLocal.toLocaleString(),
    });

    return fechaLocal.toLocaleString('es-CO', {
      year: 'numeric',
      month: '2-digit',
      day: '2-digit',
      hour: '2-digit',
      minute: '2-digit',
      hour12: true,
    });
  }

  /**
   * TrackBy function para la tabla
   */
  public trackByFn(index: number, item: any): any {
    return item.id || index;
  }

  /**
   * Elimina un registro de la tabla
   */
  public async deleteRecord(element: any): Promise<void> {
    try {
      // Mostrar diálogo de confirmación
      const result = await this.alertService.showAlert({
        title: 'Confirmar eliminación',
        text: `¿Está seguro de que desea eliminar el plan "${element.tipoPlan}" - "${element.planEmergencia}"?`,
        icon: 'warning',
        showCancelButton: true,
        confirmButtonText: 'Sí, eliminar',
        cancelButtonText: 'Cancelar',
        confirmButtonColor: '#dc3545', // Color rojo para indicar peligro
      });

      // Si el usuario cancela, no hacer nada
      if (!result.isConfirmed) {
        return;
      }

      // Eliminar del dataSource
      const currentDataSource = this.dataSource();
      const newDataSource = currentDataSource.filter((_, index) => index !== element.realIndex);

      // Actualizar dataSource
      this.dataSource.set(newDataSource);

      // También actualizar el servicio para mantener consistencia
      const serviceData = this.specialPlansService.dataActivacionPlanes();
      const newArray = serviceData.activacionPlanes.filter((_, index) => index !== element.realIndex);
      const newServiceData: SpecialPlansData = { activacionPlanes: newArray };
      this.specialPlansService.dataActivacionPlanes.set(newServiceData);
      this.originalServerData = JSON.parse(JSON.stringify(newServiceData));

      // Forzar re-renderización completa
      this.forceCompleteTableRerender();

      // Mostrar mensaje de éxito
      this.toast.open('Registro eliminado correctamente', 'Cerrar', {
        duration: 2000,
        panelClass: ['snackbar-verde'],
      });

      // Emitir evento de cambios realizados después de eliminar
      this.unsavedChanges.emit(false);
      this.changesMade.emit(true);
    } catch (error) {
      console.error('Error al eliminar el registro:', error);
      this.toast.open('Error al eliminar el registro', 'Cerrar', {
        duration: 3000,
        panelClass: ['snackbar-error'],
      });
    }
  }

  /**
   * Edita un registro de la tabla
   */
  public async editRecord(element: any): Promise<void> {
    // Establecer el estado de edición
    this.isEditing.set(true);
    this.editingRecord.set(element);
    this.editingIndex.set(element.realIndex);

    // Mostrar el formulario
    this.showForm.set(true);

    console.log('Editando registro:', element.originalData);

    // Convertir fechas UTC a formato local para el input datetime-local
    const fechaInicioLocal = element.originalData.fechaInicio ? DateUtils.fromUtcToCest(element.originalData.fechaInicio) : '';

    const fechaFinLocal = element.originalData.fechaFin ? DateUtils.fromUtcToCest(element.originalData.fechaFin) : null;

    // Cargar los datos en el formulario
    this.formData.patchValue({
      idTipoPlan: element.originalData.idTipoPlan,
      idPlanEmergencia: element.originalData.idPlanEmergencia,
      fechaInicio: fechaInicioLocal,
      fechaFin: fechaFinLocal,
      autoridad: element.originalData.autoridad,
      observaciones: element.originalData.observaciones,
    });

    console.log('Fechas cargadas en el formulario:', {
      fechaInicioOriginal: element.originalData.fechaInicio,
      fechaInicioLocal: fechaInicioLocal,
      fechaFinOriginal: element.originalData.fechaFin,
      fechaFinLocal: fechaFinLocal,
    });

    // Cargar planes de emergencia para el tipo seleccionado
    if (element.originalData.idTipoPlan) {
      try {
        // Obtener idCcaa desde la provincia del incendio
        const idCcaa = this.fire?.provincia?.idCcaa;

        const idTipoSuceso = this.fire?.suceso?.idTipo || null;

        // Cargar planes de emergencia para el tipo seleccionado
        const planes = await this.specialPlansService.getPlanesEmergenciaPorTipo(element.originalData.idTipoPlan, idCcaa, idTipoSuceso,false);
        this.planesEmergencia.set(planes);

        this.toast.open(`${planes.length} planes de emergencia cargados`, 'Cerrar', {
          duration: 2000,
          panelClass: ['snackbar-verde'],
        });
      } catch (error) {
        console.error('Error al cargar planes de emergencia:', error);
        this.planesEmergencia.set([]);
        this.toast.open('Error al cargar planes de emergencia', 'Cerrar', {
          duration: 3000,
          panelClass: ['snackbar-error'],
        });
      }
    }

    // Manejar archivos existentes vs nuevos archivos
    if (element.originalData.archivo) {
      // Si hay un archivo existente del servidor
      this.existingFile = element.originalData.archivo;
      this.file = null; // Limpiar archivo nuevo
    } else if (element.originalData.file) {
      // Si hay un archivo nuevo
      this.file = element.originalData.file;
      this.existingFile = null; // Limpiar archivo existente
    } else {
      // No hay archivos
      this.file = null;
      this.existingFile = null;
    }

    this.toast.open('Registro cargado para edición', 'Cerrar', { duration: 2000 });

    // No emitir eventos de cambios al seleccionar un registro para editar
    // this.unsavedChanges.emit(true);
    // this.changesMade.emit(true);
  }

  // Método de utilidad para obtener controles del formulario
  getForm(atributo: string): any {
    return this.formData.controls[atributo];
  }

  // Método para mostrar el formulario
  showFormSection(): void {
    // Restablecer el formulario con fecha y hora actual
    this.formData.patchValue({
      fechaInicio: DateUtils.getCurrentCESTDate(),
      fechaFin: null, // Sin valor por defecto
    });

    this.showForm.set(true);

    // No emitir eventos de cambios al mostrar el formulario
    // this.changesMade.emit(true);
    // this.unsavedChanges.emit(true);
  }

  // Métodos para manejo de archivos
  dropped(files: NgxFileDropEntry[]): void {
    for (const droppedFile of files) {
      if (droppedFile.fileEntry.isFile) {
        const fileEntry = droppedFile.fileEntry as FileSystemFileEntry;

        fileEntry.file((file: File) => {
          this.file = file;
          this.existingFile = null; // Limpiar archivo existente cuando se selecciona uno nuevo
        });
      }
    }
  }

  fileOver(event: any): void {
    // Archivo sobre la zona de drop
  }

  fileLeave(event: any): void {
    // Archivo sale de la zona de drop
  }

  // Método para selección de archivo desde input
  onFileSelected(event: any): void {
    const input = event.target as HTMLInputElement;
    if (input.files && input.files.length > 0) {
      this.file = input.files[0];
      this.existingFile = null; // Limpiar archivo existente cuando se selecciona uno nuevo
    }
  }

  // Método para guardar los datos
  onSubmit(): void {
    if (!this.formData.valid) {
      this.formData.markAllAsTouched();

      // Mostrar mensaje específico según el error
      if (this.formData.get('fechaInicio')?.hasError('fechaInicioInvalida')) {
        this.toast.open('La fecha de inicio debe ser igual o posterior a la fecha del incendio', 'Cerrar', { duration: 3000 });
        return;
      }

      if (this.formData.get('fechaFin')?.hasError('fechaFinInvalida')) {
        this.toast.open('La fecha de fin debe ser posterior a la fecha de inicio', 'Cerrar', { duration: 3000 });
        return;
      }

      this.toast.open('Por favor complete todos los campos requeridos', 'Cerrar', { duration: 3000 });
      return;
    }

    const formValues = this.formData.value;

    const planEmergencia = this.specialPlansService.dataActivacionPlanes().activacionPlanes
      .filter((item) => item.idPlanEmergencia === formValues.idPlanEmergencia).length;

    if (planEmergencia > 1) {
      this.toast.open('El plan seleccionado ya está registrado.', 'Cerrar', { duration: 3000 });
      return;
    }

    // Validación adicional de fechas
    const fechaIncendioLocal = new Date(this.fire.fechaInicio);
    const fechaIncendio = new Date(
      Date.UTC(
        fechaIncendioLocal.getFullYear(),
        fechaIncendioLocal.getMonth(),
        fechaIncendioLocal.getDate(),
        fechaIncendioLocal.getHours(),
        fechaIncendioLocal.getMinutes()
      )
    );

    // Convertir la fecha local a UTC
    const fechaInicioLocal = new Date(formValues.fechaInicio);
    const fechaInicio = new Date(
      Date.UTC(
        fechaInicioLocal.getFullYear(),
        fechaInicioLocal.getMonth(),
        fechaInicioLocal.getDate(),
        fechaInicioLocal.getHours(),
        fechaInicioLocal.getMinutes()
      )
    );

    // Para fecha fin
    let fechaFin = null;
    if (formValues.fechaFin) {
      const fechaFinLocal = new Date(formValues.fechaFin);
      fechaFin = new Date(
        Date.UTC(fechaFinLocal.getFullYear(), fechaFinLocal.getMonth(), fechaFinLocal.getDate(), fechaFinLocal.getHours(), fechaFinLocal.getMinutes())
      );
    }

    // Comparar las fechas usando getTime() para comparar timestamps
    if (fechaInicio.getTime() < fechaIncendio.getTime()) {
      this.toast.open('La fecha y hora de inicio debe ser igual o posterior a la fecha y hora del incendio', 'Cerrar', { duration: 3000 });
      return;
    }

    // Validar que la fecha fin sea posterior a la fecha inicio
    if (fechaFin && fechaFin.getTime() <= fechaInicio.getTime()) {
      this.toast.open('La fecha y hora de fin debe ser posterior a la fecha y hora de inicio', 'Cerrar', { duration: 3000 });
      return;
    }

    // Determinar el estado del archivo
    let fileAction: 'none' | 'new' | 'keep_existing' = 'none';

    if (this.file) {
      fileAction = 'new';
    } else if (this.existingFile) {
      fileAction = 'keep_existing';
    }

    const planData: ActivacionPlan = {
      idTipoPlan: formValues.idTipoPlan,
      idPlanEmergencia: formValues.idPlanEmergencia,
      fechaInicio: DateUtils.fromCestToUtc(formValues.fechaInicio)!,
      fechaFin: fechaFin && DateUtils.fromCestToUtc(formValues.fechaFin),
      autoridad: formValues.autoridad,
      observaciones: formValues.observaciones,
      file: this.file,
      fileAction: fileAction,
    };

    console.log('📁 Estado del archivo:', {
      hasFile: !!this.file,
      hasExistingFile: !!this.existingFile,
      fileAction: fileAction,
      fileName: this.file?.name || 'N/A',
      existingFileName: this.existingFile?.nombreOriginal || 'N/A',
    });

    const currentData: SpecialPlansData = {
      activacionPlanes: [...this.specialPlansService.dataActivacionPlanes().activacionPlanes],
    };

    if (this.isEditing()) {
      // Modo edición: actualizar registro existente
      const editingIndex = this.editingIndex();

      console.log('🔄 Editando registro:', { editingIndex, planData, currentData });

      if (editingIndex >= 0 && editingIndex < currentData.activacionPlanes.length) {
        // Conservar archivo existente si no se cambió
        const originalData = currentData.activacionPlanes[editingIndex];
        if (!planData.file && originalData.archivo) {
          planData.archivo = originalData.archivo;
          // Si se mantiene archivo existente, asegurar que fileAction sea correcto
          planData.fileAction = 'keep_existing';
        }
        currentData.activacionPlanes[editingIndex] = planData;
      } else {
        console.error('Error: Índice de edición inválido', { editingIndex, currentData });
        this.toast.open('Error al actualizar el registro', 'Cerrar', { duration: 3000 });
        return;
      }

      this.toast.open('Registro actualizado correctamente', 'Cerrar', { duration: 2000 });
    } else {

      // Validar duplicidad de tipo de plan para el mismo idPlanEmergencia
      const yaExiste = currentData.activacionPlanes.some(p =>
        p.idTipoPlan === planData.idTipoPlan &&
        p.idPlanEmergencia === planData.idPlanEmergencia
      );

      if (yaExiste && !this.isEditing()) {
        this.toast.open('Ya existe un plan del mismo tipo para esta emergencia', 'Cerrar', { duration: 3000 });
        return;
      }

      // Modo creación: agregar nuevo registro
      currentData.activacionPlanes.push(planData);
      // this.toast.open('Datos guardados correctamente', 'Cerrar', { duration: 2000 });
    }
    console.log('🚀 ~ SpecialPlansActivationComponent ~ onSubmit ~ currentData.activacionPlanes:', currentData.activacionPlanes);

    this.specialPlansService.dataActivacionPlanes.set(currentData);
    console.log(
      '🚀 ~ SpecialPlansActivationComponent ~ onSubmit ~ this.specialPlansService.dataActivacionPlanes:',
      this.specialPlansService.dataActivacionPlanes()
    );
    this.updateTableData();
    this.save.emit({ success: true, data: currentData });
    
    // Emitir evento de cambios realizados después de guardar
    this.changesMade.emit(true);
    this.unsavedChanges.emit(false);

    // Resetear formulario y estado de edición
    this.resetForm();
    // Ocultar el formulario después de guardar
    this.showForm.set(false);
  }

  /**
   * Resetea el formulario y el estado de edición
   */
  private resetForm(): void {
    this.formData.reset();

    // Restablecer fechas por defecto después del reset
    this.formData.patchValue({
      fechaInicio: DateUtils.getCurrentCESTDate(),
      fechaFin: null, // Sin valor por defecto
    });

    this.file = null;
    this.existingFile = null; // Limpiar archivo existente también

    // Resetear estado de edición
    this.isEditing.set(false);
    this.editingRecord.set(null);
    this.editingIndex.set(-1);

    // No emitir eventos de cambios al resetear el formulario
    // this.unsavedChanges.emit(false);
    // this.changesMade.emit(true);
  }

  /**
   * Cancela la edición y oculta el formulario
   */
  public cancelEdit(): void {
    this.resetForm();
    this.showForm.set(false);

    // Restaurar datos originales si hay cambios no guardados
    const currentData = this.specialPlansService.dataActivacionPlanes();
    const originalData = this.originalServerData;

    if (JSON.stringify(currentData) !== JSON.stringify(originalData)) {
      const dataToRestore = JSON.parse(JSON.stringify(originalData));
      this.specialPlansService.dataActivacionPlanes.set(dataToRestore);
      this.updateTableData();
    }

    this.changesMade.emit(false);
    this.unsavedChanges.emit(false);
  }

  /**
   * Método público para guardar todos los datos de activación de planes
   * Este método será llamado desde el componente padre cuando se haga clic en "Guardar"
   */
  public async saveAllData(idSuceso: number, idRegistroActualizacion: number): Promise<any> {
    try {
      const currentData = this.specialPlansService.dataActivacionPlanes();

      // Verificar si hay datos para guardar
      if (currentData.activacionPlanes.length === 0) {
        console.log('No hay datos de activación de planes para guardar');
        return { success: true, message: 'No hay datos para guardar' };
      }

      console.log('Enviando datos de activación de planes:', currentData);

      // Verificar archivos específicamente
      console.log('🔍 Verificando archivos en los datos:');
      currentData.activacionPlanes.forEach((plan, i) => {
        if (plan.file) console.log(`📁 Plan ${i} - Archivo:`, plan.file.name, plan.file.size, 'bytes');
      });

      const response = await this.specialPlansService.postActivacionPlanes(currentData, idSuceso, idRegistroActualizacion);

      console.log('Respuesta del servidor:', response);

      this.toast.open('Datos de activación de planes guardados correctamente', 'Cerrar', {
        duration: 3000,
        panelClass: ['snackbar-verde'],
      });

      // Actualizar datos originales después de guardado exitoso
      this.originalServerData = JSON.parse(JSON.stringify(currentData));

      return { success: true, data: response };
    } catch (error) {
      console.error('Error al guardar datos de activación de planes:', error);

      this.toast.open('Error al guardar datos de activación de planes', 'Cerrar', {
        duration: 5000,
        panelClass: ['snackbar-error'],
      });

      throw error;
    }
  }

  /**
   * Verifica si hay datos pendientes de guardar
   */
  public hasDataToSave(): boolean {
    const currentData = this.specialPlansService.dataActivacionPlanes();
    return currentData.activacionPlanes.length > 0;
  }

  /**
   * Verifica si hay cambios no guardados comparando con los datos originales del servidor
   */
  public hasUnsavedChanges(): boolean {
    const currentData = this.specialPlansService.dataActivacionPlanes();
    return JSON.stringify(currentData) !== JSON.stringify(this.originalServerData);
  }

  /**
   * Método público para limpiar todas las variables y datos del componente
   * Este método será llamado desde el componente padre cuando se cierre el modal
   */
  public clearAllData(): void {
    // Limpiar completamente los datos del servicio en lugar de restaurar
    this.specialPlansService.clearData();

    // Resetear formulario principal
    this.formData.reset();

    // Resetear estado de edición
    this.isEditing.set(false);
    this.editingRecord.set(null);
    this.editingIndex.set(-1);

    // Ocultar formulario
    this.showForm.set(false);

    // Limpiar archivo
    this.file = null;
    this.existingFile = null;

    // Resetear datos originales
    this.originalServerData = { activacionPlanes: [] };

    // Actualizar tabla con datos vacíos
    this.updateTableData();

    console.log('Datos de activación de planes limpiados correctamente');
  }

  /**
   * Maneja el cambio del select Tipo de Plan
   */
  public async onTipoPlanChange(idTipoPlan: number): Promise<void> {
    console.log('🚀 ~ Tipo de plan seleccionado:', idTipoPlan);

    if (idTipoPlan) {
      // Mostrar estado de carga
      this.isLoadingPlanes.set(true);
      this.spinner.show();

      // Limpiar selección actual del plan de emergencia
      this.formData.patchValue({ idPlanEmergencia: null });

      try {
        // Obtener idCcaa desde la provincia del incendio
        const idCcaa = this.fire?.provincia?.idCcaa;
        console.log('🚀 ~ IdCcaa obtenido:', idCcaa);

        const idTipoSuceso = this.fire?.suceso?.idTipo || null;

        // Cargar planes de emergencia para el tipo seleccionado
        const planes = await this.specialPlansService.getPlanesEmergenciaPorTipo(idTipoPlan, idCcaa, idTipoSuceso,false);

        // Mantener los planes existentes en el mapa
        const currentUsedPlanes = new Map(this.allUsedPlanes());
        planes.forEach((plan) => {
          if (!currentUsedPlanes.has(plan.id)) {
            currentUsedPlanes.set(plan.id, plan);
          }
        });
        this.allUsedPlanes.set(currentUsedPlanes);

        this.planesEmergencia.set(planes);
        console.log('🚀 ~ Planes de emergencia cargados:', planes);
      } catch (error) {
        console.error('Error al cargar planes de emergencia:', error);
        this.planesEmergencia.set([]);
        this.toast.open('Error al cargar planes de emergencia', 'Cerrar', {
          duration: 3000,
          panelClass: ['snackbar-error'],
        });
      } finally {
        this.isLoadingPlanes.set(false);
        this.spinner.hide();
      }
    } else {
      // Si no hay tipo seleccionado, limpiar planes
      this.planesEmergencia.set([]);
      this.formData.patchValue({ idPlanEmergencia: null });
      this.isLoadingPlanes.set(false);
    }
  }

  /**
   * Descarga un archivo existente del servidor
   */
  public async downloadExistingFile(): Promise<void> {
    if (this.existingFile) {
      try {
        console.log('🚀 ~ downloadExistingFile ~ existingFile:', this.existingFile);

        const blob = await this.specialPlansService.getFile(this.existingFile.id);

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

        this.toast.open(`Archivo ${this.existingFile.nombreOriginal} descargado correctamente`, 'Cerrar', {
          duration: 2000,
        });
      } catch (error) {
        console.error('Error al descargar el archivo:', error);
        this.toast.open('Error al descargar el archivo', 'Cerrar', {
          duration: 3000,
        });
      }
    } else {
      this.toast.open('No hay archivo existente para descargar', 'Cerrar', { duration: 3000 });
    }
  }

  /**
   * Fuerza re-renderización completa de la tabla
   */
  private forceCompleteTableRerender(): void {
    // Ocultar tabla temporalmente
    this.forceTableRerender.set(false);
    this.changeDetectorRef.detectChanges();

    // Volver a mostrar la tabla
    setTimeout(() => {
      this.forceTableRerender.set(true);
      this.changeDetectorRef.markForCheck();
      this.changeDetectorRef.detectChanges();

      // Segunda ronda de detección
      setTimeout(() => {
        this.changeDetectorRef.detectChanges();
      }, 50);
    }, 50);
  }

  private createForm(): void {
    // Suscribirse a cambios en fechaInicio para validar fechaFin
    this.formData.get('fechaInicio')?.valueChanges.subscribe(() => {
      this.formData.get('fechaFin')?.updateValueAndValidity();
    });
  }

  private validarFechaInicioConHora(control: AbstractControl): ValidationErrors | null {
    if (!control.value) return null;

    // Convertir la fecha del incendio a UTC
    const fechaIncendioLocal = new Date(this.fire.fechaInicio);
    const fechaIncendio = new Date(
      Date.UTC(
        fechaIncendioLocal.getFullYear(),
        fechaIncendioLocal.getMonth(),
        fechaIncendioLocal.getDate(),
        fechaIncendioLocal.getHours(),
        fechaIncendioLocal.getMinutes()
      )
    );
    console.log('🚀 ~ validarFechaInicioConHora ~ fechaIncendio UTC:', fechaIncendio.toISOString());

    // Convertir la fecha local del input a UTC
    const fechaInicioLocal = new Date(control.value);
    const fechaInicio = new Date(
      Date.UTC(
        fechaInicioLocal.getFullYear(),
        fechaInicioLocal.getMonth(),
        fechaInicioLocal.getDate(),
        fechaInicioLocal.getHours(),
        fechaInicioLocal.getMinutes()
      )
    );
    console.log('🚀 ~ validarFechaInicioConHora ~ fechaInicio UTC:', fechaInicio.toISOString());
    console.log(
      '🚀 ~ validarFechaInicioConHora ~ comparación:',
      'fechaInicio:',
      fechaInicio.getTime(),
      'fechaIncendio:',
      fechaIncendio.getTime(),
      'diferencia:',
      fechaInicio.getTime() - fechaIncendio.getTime()
    );

    if (fechaInicio.getTime() < fechaIncendio.getTime()) {
      console.log('🚀 ~ validarFechaInicioConHora ~ FECHA INVÁLIDA');
      return { fechaInicioInvalida: true };
    }

    return null;
  }

  private validarFechaFinConHora(control: AbstractControl): ValidationErrors | null {
    if (!control.value) return null;

    const fechaInicio = this.formData?.get('fechaInicio')?.value;
    if (!fechaInicio) return null;

    // Convertir ambas fechas locales a UTC para comparación
    const fechaInicioLocal = new Date(fechaInicio);
    const fechaInicioUTC = new Date(
      Date.UTC(
        fechaInicioLocal.getFullYear(),
        fechaInicioLocal.getMonth(),
        fechaInicioLocal.getDate(),
        fechaInicioLocal.getHours(),
        fechaInicioLocal.getMinutes()
      )
    );

    const fechaFinLocal = new Date(control.value);
    const fechaFinUTC = new Date(
      Date.UTC(fechaFinLocal.getFullYear(), fechaFinLocal.getMonth(), fechaFinLocal.getDate(), fechaFinLocal.getHours(), fechaFinLocal.getMinutes())
    );

    if (fechaFinUTC.getTime() <= fechaInicioUTC.getTime()) {
      return { fechaFinInvalida: true };
    }

    return null;
  }

  public getFormattedDate(date?: string): string {
    return date ? DateUtils.fromUtcToCest(date, 'DD/MM/yyy HH:mm')! : '-';
  }

  /**
   * Método que se ejecuta cuando se destruye el componente
   */
  ngOnDestroy(): void {
    console.log('SpecialPlansActivationComponent - ngOnDestroy');
    // No limpiar datos del servicio aquí, ya que podría ser por cambio de pestaña
    // La limpieza se hace explícitamente en el método clearAllData() llamado desde closeModal
  }
}
