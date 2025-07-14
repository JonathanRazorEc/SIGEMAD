import { CommonModule } from '@angular/common';
import { Component, effect, EnvironmentInjector, EventEmitter, inject, Input, OnInit, Output, runInInjectionContext, signal } from '@angular/core';
import { FlexLayoutModule } from '@angular/flex-layout';
import { FormBuilder, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
import { MatButtonModule } from '@angular/material/button';
import { DateAdapter, MAT_DATE_FORMATS, MAT_DATE_LOCALE, MatNativeDateModule, NativeDateAdapter } from '@angular/material/core';
import { MatDatepickerModule } from '@angular/material/datepicker';
import { MAT_DIALOG_DATA, MatDialog, MatDialogRef } from '@angular/material/dialog';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatGridListModule } from '@angular/material/grid-list';
import { MatIconModule } from '@angular/material/icon';
import { MatInputModule } from '@angular/material/input';
import { MatSelectModule } from '@angular/material/select';
import { MatSnackBar } from '@angular/material/snack-bar';
import { MatSort } from '@angular/material/sort';
import { MatTableModule } from '@angular/material/table';
import moment from 'moment';
import { NgxSpinnerModule, NgxSpinnerService } from 'ngx-spinner';
import { EvolutionService } from '../../../services/evolution.service';
import { MasterDataEvolutionsService } from '../../../services/master-data-evolutions.service';
import { EvolucionIncendio, ParametroRecord } from '../../../types/evolution-record.type';
import { FireStatus } from '../../../types/fire-status.type';
import { Phases } from '../../../types/phases.type';
import { SavePayloadModal } from '../../../types/save-payload-modal';
import { SituationPlan } from '../../../types/situation-plan.type';
import { SituationsEquivalent } from '../../../types/situations-equivalent.type';
import { TypesPlans } from '../../../types/types-plans.type';
import { MomentDateAdapter } from '@angular/material-moment-adapter';
import { FORMATO_FECHA } from '../../../types/date-formats';
import { FechaValidator } from '../../../shared/validators/fecha-validator';
import { UtilsService } from '../../../shared/services/utils.service';
import { FECHA_MAXIMA_DATETIME, FECHA_MINIMA_DATETIME } from '@type/constants';
import { RecordsService } from '@services/records.service';
import { FireDetail } from '@type/fire-detail.type';
import { disable } from 'ol/rotationconstraint';
import { MatTooltipModule } from '@angular/material/tooltip';

@Component({
  selector: 'app-records',
  standalone: true,
  imports: [
    CommonModule,
    MatInputModule,
    FlexLayoutModule,
    MatGridListModule,
    MatButtonModule,
    MatButtonModule,
    MatSort,
    MatTableModule,
    MatIconModule,
    MatNativeDateModule,
    MatFormFieldModule,
    MatDatepickerModule,
    ReactiveFormsModule,
    MatSelectModule,
    NgxSpinnerModule,
    MatTooltipModule,
  ],
  templateUrl: './records.component.html',
  styleUrl: './records.component.scss',
  providers: [
    { provide: DateAdapter, useClass: MomentDateAdapter },
    { provide: MAT_DATE_FORMATS, useValue: FORMATO_FECHA },
  ],
})
export class RecordsComponent implements OnInit {
  data = inject(MAT_DIALOG_DATA) as { title: string; idIncendio: number; fireDetail?: FireDetail; valoresDefecto?: number };
  @Output() save = new EventEmitter<SavePayloadModal>();
  @Output() changesMade = new EventEmitter<boolean>();
  @Output() hasUnsavedChanges = new EventEmitter<boolean>();
  @Input() editData: any;
  @Input() esUltimo: boolean | undefined;
  @Input() estadoIncendio: any;
  @Input() fire: any;
  @Input() isNewRecord: boolean | undefined;
  @Input() registroId: number | null = null;

  private dialogRef = inject(MatDialogRef<RecordsComponent>);
  private masterData = inject(MasterDataEvolutionsService);
  private recordsService = inject(RecordsService);
  private fb = inject(FormBuilder);
  private spinner = inject(NgxSpinnerService);
  private toast = inject(MatSnackBar);
  private utilsService = inject(UtilsService);
  public evolutionService = inject(EvolutionService);

  private environmentInjector = inject(EnvironmentInjector);
  public status = signal<FireStatus[]>([]);
  public typesPlans = signal<TypesPlans[]>([]);
  public situationEquivalent = signal<SituationsEquivalent[]>([]);
  public isCreate = signal<number>(-1);
  public phases = signal<Phases[]>([]);
  public niveles = signal<SituationPlan[]>([]);
  public operativas = signal<SituationPlan[]>([]);

  public fechaMinimaDateTime = FECHA_MINIMA_DATETIME;
  public fechaMaximaDateTime = FECHA_MAXIMA_DATETIME;

  public isOperativaDisabled = true;
  public hasPreviousRecords = false;

  public formData: FormGroup;
  public editIndex: number = -1;
  public showForm: boolean = false;
  public isReadonly = false;
  public displayedColumns: string[] = ['fechaHora', 'estado', 'planEmergencia', 'opciones'];

  constructor() {
    this.formData = this.fb.group({
      datetimeUpdate: [this.getCurrentDateTimeString(), Validators.required],
      observations_1: [''],
      forecast: [''],
      status: [1],
      end_date: [{ value: null, disabled: true }],
      emergencyPlanActivated: [null],
      phases: [null],
      nivel: [null],
      operativa: [{ value: null, disabled: true }],
    });

    // Asegurar que el formulario esté oculto al inicio
    this.showForm = false;
  }

  async ngOnInit() {
    this.spinner.show();

    // Mostrar flag de nuevo registro o edición
    console.log('🚀 ~ RecordsComponent ~ ¿Es nuevo registro?:', this.isNewRecord);
    console.log('🚀 ~ RecordsComponent ~ registroId:', this.registroId);

    // Asegurar que el formulario esté oculto al inicio
    this.showForm = false;

    try {
      const status = await this.masterData.getFireStatus();
      this.status.set(status);

      const typesPlans = await this.masterData.getActivatedEmergencyPlans(this.fire.idSuceso);
      this.typesPlans.set(typesPlans);

      const situationEquivalente = await this.masterData.getSituationEquivalent();
      this.situationEquivalent.set(situationEquivalente);

      this.estadoIncendio ? this.formData.get('status')?.setValue(this.estadoIncendio) : 0;

      this.evolutionService.dataRecords.set({
        idSuceso: this.data.idIncendio,
        idRegistroActualizacion: this.registroId || 0,
        parametro: [],
      });

      try {
        let registroData;

        // Si es nuevo registro, primero intentamos obtener todos los registros
        registroData = await this.recordsService.getById(Number(this.registroId));
        console.log('Datos del registro:', registroData);

        // Si no hay parámetros, intentamos obtener registros anteriores
        if (!registroData?.parametros || registroData.parametros.length === 0) {
          const registrosAnteriores = await this.recordsService.getRegistrosAnteriores(Number(this.fire.idSuceso), this.registroId!);
          console.log('Datos de registros anteriores:', registrosAnteriores);

          if (registrosAnteriores[0] && registrosAnteriores[0].parametros.length > 0) {
            this.hasPreviousRecords = true;

            const parametros = registrosAnteriores[0].parametros.map((param: any) => ({
              id: 0,
              idEstadoIncendio: param.estadoIncendio?.id || (typeof param.estadoIncendio === 'number' ? param.estadoIncendio : null),
              fechaFinal: param.fechaFinal || '',
              idPlanEmergencia: param.planEmergencia?.id || null,
              idFaseEmergencia: param.faseEmergencia?.id || null,
              idPlanSituacion: param.planSituacion?.id || null,
              idSituacionEquivalente: param.situacionEquivalente?.id || null,
              fechaHoraActualizacion: param.fechaHoraActualizacion || '',
              observaciones: param.observaciones || '',
              prevision: param.prevision || '',
              idRegistro: param.idRegistro,
              esModificado: param.esModificado,
            }));

            this.evolutionService.dataRecords.update((current) => ({
              ...current,
              parametro: parametros,
            }));
          }
        } else {
          // Si hay parámetros, los usamos
          const parametros = registroData?.parametros.map((param: any) => ({
            id: param.id || 0,
            idEstadoIncendio: param.estadoIncendio?.id || (typeof param.estadoIncendio === 'number' ? param.estadoIncendio : null),
            fechaFinal: param.fechaFinal || '',
            idPlanEmergencia: param.planEmergencia?.id || null,
            idFaseEmergencia: param.faseEmergencia?.id || null,
            idPlanSituacion: param.planSituacion?.id || null,
            idSituacionEquivalente: param.situacionEquivalente?.id || null,
            fechaHoraActualizacion: param.fechaHoraActualizacion || '',
            observaciones: param.observaciones || '',
            prevision: param.prevision || '',
            idRegistro: param.idRegistro,
            esModificado: param.esModificado,
          }));
          console.log('🚀 ~ RecordsComponent ~ parametros ~ parametros:', parametros);

          this.evolutionService.dataRecords.update((current) => ({
            ...current,
            parametro: parametros,
          }));
        }
      } catch (error) {
        console.error('Error al cargar los datos:', error);
        this.toast.open('Error al cargar los datos', 'Cerrar', { duration: 3000 });
      }
    } catch (error) {
      console.error('Error al cargar datos:', error);
      this.toast.open('Error al cargar datos', 'Cerrar', {
        duration: 3000,
      });
    } finally {
      this.spinner.hide();
    }

    this.setupConditionalValidation();
  }

  public get showAddButton() {
    const hasOwnRecord = !!this.evolutionService.dataRecords().parametro
      .find((item) => !item.idRegistro || item.idRegistro === this.registroId || 0);

    return (!this.hasPreviousRecords && !hasOwnRecord)
      || (this.hasPreviousRecords && !hasOwnRecord)
      && !this.showForm;
  }

  async updateFormWithJson(json: any) {
    const rawDate: string = json.registro?.fechaHoraEvolucion || '';
    let dateValue: Date = new Date();
    if (rawDate) {
      dateValue = new Date(rawDate);
    }

    const rawDate2: string = json.registro?.fechaHora || '';
    let dateValue2: Date = new Date();
    if (rawDate2) {
      dateValue2 = new Date(rawDate2);
    }

    const formattedDate = moment(dateValue).format('YYYY-MM-DDTHH:mm');
    const formattedDate2 = moment(dateValue).format('YYYY-MM-DDTHH:mm');

    this.formData.patchValue({
      datetimeUpdate: formattedDate2,
      observations_1: json.datoPrincipal?.observaciones || '',
      forecast: json.datoPrincipal?.prevision || '',
      status: json.parametro?.estadoIncendio?.id || 1,
      end_date: json.parametro?.fechaFinal ? new Date(json.parametro.fechaFinal) : null,
      emergencyPlanActivated: json.parametro?.planEmergencia?.id || '',
      phases: json.parametro?.faseEmergencia?.id || '',
      nivel: json.parametro?.planSituacion?.id || '',
      operativa: json.parametro?.situacionEquivalente?.id || '',
    });

    await this.updateEndDate(this.formData.get('status')?.value);
    this.formData.patchValue({
      end_date: json.parametro?.fechaFinal ? new Date(json.parametro.fechaFinal) : null,
    });
    this.loadPhases(null, json.parametro?.planEmergencia?.id);
    this.loadLevels();

    if (json.parametro?.situacionEquivalente?.id) {
      this.formData.get('operativa')?.enable();
    }
  }

  updateEndDate(statusValue: number) {
    const endDateControl = this.formData.get('end_date');
    if (!endDateControl) return;

    if (statusValue === 2) {
      endDateControl.enable();
      endDateControl.setValidators([Validators.required]);
      endDateControl.patchValue(new Date());
    } else {
      endDateControl.disable();
      endDateControl.clearValidators();
      endDateControl.patchValue(null);
    }
    endDateControl.updateValueAndValidity();
    return true;
  }

  async sendDataToEndpoint() {
    // Verificar si hay parámetros en el array
    if (this.evolutionService.dataRecords().parametro.length === 0) {
      // Si no hay parámetros, verificar si es porque se eliminaron todos los registros existentes
      // Esta verificación se hace indirectamente: si había registros previos (hasPreviousRecords)
      // o si estamos en un registro existente (registroId > 0), entonces debemos enviar un array vacío
      if (this.hasPreviousRecords || this.registroId) {
        this.spinner.show();
        
        try {
          // Enviar array vacío para eliminar los registros guardados
          const emptyData = {
            idSuceso: this.fire.idSuceso,
            idRegistroActualizacion: this.registroId,
            parametro: []
          };
          
          await this.evolutionService.postData(emptyData);
          
          this.toast.open('Registros eliminados correctamente', 'Cerrar', {
            duration: 3000,
          });
          
          return; // Terminamos aquí, ya se ha completado la operación
        } catch (error) {
          console.error('Error al eliminar registros:', error);
          this.toast.open('Error al eliminar registros', 'Cerrar', {
            duration: 3000,
          });
          this.spinner.hide();
          return; // Terminamos aquí, hubo un error
        } finally {
          this.spinner.hide();
        }
      } else {
        // Si no había registros previos, mostrar mensaje
        this.toast.open('Debe agregar al menos un registro', 'Cerrar', {
          duration: 3000,
        });
        return;
      }
    }

    // Continuar con el flujo normal si hay registros para guardar
    this.spinner.show();

    try {
      const evolucionIncendio = this.evolutionService.dataRecords();

      const response = await this.recordsService.post(evolucionIncendio);
      console.log('Respuesta del servicio:', response);

      this.toast.open('Registros guardados correctamente', 'Cerrar', {
        duration: 3000,
      });

      this.dialogRef.close(true);
    } catch (error) {
      console.error('Error al guardar registros:', error);
      this.toast.open('Error al guardar registros', 'Cerrar', {
        duration: 3000,
      });
    } finally {
      this.spinner.hide();
    }
  }

  async loadLevels() {
    const phases_id = this.editData.parametro?.faseEmergencia?.id;
    const plan_id = this.editData.parametro?.planEmergencia?.id;
    let situationsPlans: any[] = [];
    if (plan_id) {
      this.formData.get('nivel')?.enable();
      situationsPlans = await this.masterData.getSituationsPlans(plan_id, phases_id);
    }
    this.niveles.set(situationsPlans);
    this.formData.get('nivel')?.setValue(this.editData.parametro?.planSituacion?.id);
    return true;
  }

  async loadPhases(event: any, id?: string) {
    let id_plan;

    if (!event) {
      id_plan = id;
    } else {
      id_plan = event.value;
    }

    const operativaControl = this.formData.get('operativa');

    this.spinner.show();
    if (id_plan) {
      const phases = await this.masterData.getPhases(id_plan);
      this.phases.set(phases);
      this.formData.get('phases')?.enable();
      operativaControl?.enable(); // Habilita el combo si hay plan
      this.spinner.hide();
      return phases;
    } else {
      operativaControl?.disable(); // Lo desactiva si no hay plan
      operativaControl?.reset(); // Limpia el valor seleccionado
    }
    this.spinner.hide();
    return [];
  }

  async loadSituationPlans(event: any) {
    this.spinner.show();
    const phases_id = event.value;
    const plan_id = this.formData.get('emergencyPlanActivated')?.value;
    let situationsPlans: any = [];
    if (plan_id) {
      situationsPlans = await this.masterData.getSituationsPlans(plan_id, phases_id);
    }

    this.niveles.set(situationsPlans);
    this.formData.get('nivel')?.enable();
    this.formData.get('operativa')?.enable();

    this.spinner.hide();
  }

  selectStatus(event: any) {
    const status_id = event.value;
    this.updateEndDate(status_id);
  }

  async loadSituacionEquivalente(event: any) {
    this.spinner.show();
    let arr: SituationPlan[] = [];
    const nivelSelect = this.niveles().find((situacion) => situacion.id === event.value);
    const found = this.situationEquivalent().find((situacion) => situacion.descripcion === String(nivelSelect?.situacionEquivalente));

    this.formData.get('operativa')?.setValue(found?.id);
    this.spinner.hide();
  }

  getFormatdate(date: any) {
    return moment(date).format('DD/MM/YYYY HH:mm');
  }

  private getCurrentDateTimeString(): string {
    return moment().format('YYYY-MM-DDTHH:mm');
  }

  showToast() {
    this.toast.open('Guardado correctamente', 'Cerrar', {
      duration: 3000,
      horizontalPosition: 'center',
      verticalPosition: 'bottom',
    });
  }

  getForm(atributo: string): any {
    return this.formData.controls[atributo];
  }

  closeModal() {
    this.dialogRef.close();
  }

  delete() {
    this.dialogRef.close({ deleted: true });
  }

  setupConditionalValidation() {
    const emergencyPlanControl = this.formData.get('emergencyPlanActivated');
    const phasesControl = this.formData.get('phases');
    const nivelControl = this.formData.get('nivel');
    const operativaControl = this.formData.get('operativa');

    if (!emergencyPlanControl || !phasesControl || !nivelControl || !operativaControl) return;

    if (emergencyPlanControl.value) {
      phasesControl.setValidators([Validators.required]);
      nivelControl.setValidators([Validators.required]);
      operativaControl.setValidators([Validators.required]);
    } else {
      phasesControl.clearValidators();
      nivelControl.clearValidators();
      operativaControl.clearValidators();
    }

    phasesControl.updateValueAndValidity();
    nivelControl.updateValueAndValidity();
    operativaControl.updateValueAndValidity();

    emergencyPlanControl.valueChanges.subscribe((value) => {
      if (value) {
        phasesControl.setValidators([Validators.required]);
        nivelControl.setValidators([Validators.required]);
        operativaControl.setValidators([Validators.required]);
      } else {
        phasesControl.clearValidators();
        nivelControl.clearValidators();
        operativaControl.clearValidators();
      }
      phasesControl.updateValueAndValidity();
      nivelControl.updateValueAndValidity();
      operativaControl.updateValueAndValidity();
    });
  }

  onSubmit() {
    if (this.formData.valid) {
      const formValues = this.formData.value;

      const newRecord: ParametroRecord = {
        idEstadoIncendio: formValues.status || null,
        fechaFinal: formValues.end_date ? moment(formValues.end_date).format('YYYY-MM-DDTHH:mm:ss') : '',
        idPlanEmergencia: formValues.emergencyPlanActivated || null,
        idFaseEmergencia: formValues.phases || null,
        idPlanSituacion: formValues.nivel || null,
        idSituacionEquivalente: formValues.operativa ? Number(formValues.operativa) : null,
        fechaHoraActualizacion: moment(formValues.datetimeUpdate).format('YYYY-MM-DDTHH:mm:ss'),
        observaciones: formValues.observations_1 || '',
        prevision: formValues.forecast || '',
      };

      if (this.editIndex !== -1) {
        // newRecord.esModificado = true;
        const currentData = { ...this.evolutionService.dataRecords() };
        const currentParametro = [...currentData.parametro];

        if (currentParametro[this.editIndex].id) {
          newRecord.id = currentParametro[this.editIndex].id;
        }

        currentParametro[this.editIndex] = newRecord;

        this.evolutionService.dataRecords.update((current) => ({
          ...current,
          parametro: currentParametro,
        }));

        this.editIndex = -1;
      } else {
        // Solo permitimos un registro en total
        this.evolutionService.dataRecords.update((current) => ({
          ...current,
          parametro: [newRecord],
        }));
      }

      // Emitimos los eventos aquí, después de agregar o editar el registro
      // Esto habilitará el botón Guardar en el componente padre
      this.hasUnsavedChanges.emit(false); // No hay cambios sin guardar en el formulario
      this.changesMade.emit(true); // Pero sí hay cambios en los datos

      this.resetForm();

      // Ocultamos el formulario después de agregar o editar
      this.showForm = false;

      this.toast.open('Registro agregado correctamente', 'Cerrar', {
        duration: 2000,
      });
    } else {
      this.formData.markAllAsTouched();
      this.toast.open('Por favor complete todos los campos requeridos', 'Cerrar', {
        duration: 3000,
      });
    }
  }

  closeForm() {
    this.isReadonly = false;
    this.showForm = false;
    this.formData.enable();
    this.resetForm();
  }

  seleccionarRegistro(index: number, readonly = false) {
    this.closeForm();
    this.editIndex = index;
    const record = this.evolutionService.dataRecords().parametro[index];
    console.log('seleccionarRegistro', record, readonly);

    this.showForm = true;
    this.isReadonly = readonly;

    this.formData.patchValue({
      datetimeUpdate: record.fechaHoraActualizacion ? record.fechaHoraActualizacion.substring(0, 16) : '',
      observations_1: record.observaciones || '',
      forecast: record.prevision || '',
      status: record.idEstadoIncendio || null,
      end_date: record.fechaFinal ? new Date(record.fechaFinal) : null,
      emergencyPlanActivated: record.idPlanEmergencia || null,
      phases: record.idFaseEmergencia || null,
      nivel: record.idPlanSituacion || null,
      operativa: record.idSituacionEquivalente || null,
    });

    if (readonly) {
      this.formData.disable();
    }

    this.loadDependentData(record);
    // No emitimos los eventos aquí, solo cuando se realicen cambios reales en el formulario
    
    // Marcamos el formulario como "pristine" después de cargar los datos
    this.formData.markAsPristine();
  }

  async loadDependentData(record: ParametroRecord) {
    if (record.idPlanEmergencia) {
      try {
        const phasesData = await this.masterData.getPhases(record.idPlanEmergencia);
        this.phases.set(phasesData);
      } catch (error) {
        console.error('Error al cargar fases:', error);
      }
    }

    if (record.idFaseEmergencia) {
      try {
        const situationPlansData = await this.masterData.getSituationsPlans(record.idPlanEmergencia, record.idFaseEmergencia);
        this.niveles.set(situationPlansData);
      } catch (error) {
        console.error('Error al cargar planes de situación:', error);
      }
    }

    if (record.idPlanSituacion) {
      try {
        const situacionEquivalenteData = await this.masterData.getSituationEquivalent();
        this.situationEquivalent.set(situacionEquivalenteData);
        this.formData.get('operativa')?.enable();
      } catch (error) {
        console.error('Error al cargar situación equivalente:', error);
      }
    }

    if (record.idEstadoIncendio == 2) {
      this.formData.get('end_date')?.enable();
    }
  }

  eliminarRegistro(index: number) {
    if (this.editIndex === index) {
      this.resetForm();
    }

    const currentData = { ...this.evolutionService.dataRecords() };
    const currentParametro = [...currentData.parametro];
    const eliminatedRecord = currentParametro[index];
    currentParametro.splice(index, 1);

    this.evolutionService.dataRecords.update((current) => ({
      ...current,
      parametro: currentParametro,
    }));

    // Verificar si había registros guardados previamente (para usarlo en sendDataToEndpoint)
    const tieneRegistrosGuardados = (eliminatedRecord && eliminatedRecord.id && eliminatedRecord.id > 0) || this.hasPreviousRecords;
    
    // Si se eliminó el último registro y había registros guardados, marcar que hay cambios
    if (currentParametro.length === 0 && tieneRegistrosGuardados) {
      this.hasUnsavedChanges.emit(false);
      this.changesMade.emit(true); // Esto habilitará el botón Guardar
    } else {
      // Comportamiento normal
      this.hasUnsavedChanges.emit(false);
      this.changesMade.emit(true);
    }

    this.toast.open('Registro eliminado', 'Cerrar', {
      duration: 2000,
    });
  }

  resetForm() {
    this.formData.reset({
      datetimeUpdate: this.getCurrentDateTimeString(),
      observations_1: '',
      forecast: '',
      status: 1,
      end_date: null,
      emergencyPlanActivated: null,
      phases: null,
      nivel: null,
      operativa: null,
    });
    this.editIndex = -1;
    this.hasUnsavedChanges.emit(false);
    // No emitimos changesMade aquí para no habilitar el botón Guardar
  }

  // Método para obtener la descripción del estado
  getEstadoDescripcion(idEstado: number): string {
    const estadoEncontrado = this.status().find((s) => s.id === idEstado);
    return estadoEncontrado ? estadoEncontrado.descripcion : 'Desconocido';
  }

  // Método para obtener la descripción del plan de emergencia
  getPlanEmergenciaDescripcion(idPlan: number): string {
    const planEncontrado = this.typesPlans().find((p) => p.id === idPlan);
    return planEncontrado ? planEncontrado.descripcion : 'Ninguno';
  }

  cancel() {
    this.showForm = false;
    this.changesMade.emit(false);
    this.hasUnsavedChanges.emit(false);
  }

  new() {
    this.showForm = true;
    // No emitimos los eventos aquí, solo mostramos el formulario
  }

  async changeSituacionEquivalent(event: { value: number }) {
    try {
      const idPlan = this.formData.get('emergencyPlanActivated')?.value;
      
      if (!idPlan) {
        return;
      }

      this.spinner.show();

      const response = await this.recordsService.getFaseNivelSituacion(idPlan, event.value);
      this.spinner.hide();

      if (!response) {
        return;
      }

      this.formData.get('phases')?.setValue(response.faseEmergencia.id);
      
      await this.loadSituationPlans({ value: response.faseEmergencia.id });

      this.formData.get('nivel')?.setValue(response.id);
    } catch (error) {
      this.spinner.hide();
      this.toast.open('Error al obtener la información', 'Cerrar', {
        duration: 3000,
      });
    }
  }
}
