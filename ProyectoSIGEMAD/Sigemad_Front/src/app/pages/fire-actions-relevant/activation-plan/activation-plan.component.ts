import { CommonModule } from '@angular/common';
import { Component, EventEmitter, Input, OnInit, Output, ViewChild, inject, signal } from '@angular/core';
import { FormBuilder, FormGroup, FormGroupDirective, ReactiveFormsModule, Validators } from '@angular/forms';
import { MatButtonModule } from '@angular/material/button';
import { MatChipListboxChange, MatChipsModule } from '@angular/material/chips';
import { MatDatepickerModule } from '@angular/material/datepicker';
import { MAT_DIALOG_DATA, MatDialogRef } from '@angular/material/dialog';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { MatSelectModule } from '@angular/material/select';
import { MatSort } from '@angular/material/sort';
import { MatTableDataSource, MatTableModule } from '@angular/material/table';

import { MatSnackBar } from '@angular/material/snack-bar';

import { DateAdapter, MAT_DATE_FORMATS, MatNativeDateModule, NativeDateAdapter } from '@angular/material/core';
import { MatIconModule } from '@angular/material/icon';
import moment from 'moment';
import { FileSystemDirectoryEntry, FileSystemFileEntry, NgxFileDropEntry, NgxFileDropModule } from 'ngx-file-drop';
import { NgxSpinnerModule, NgxSpinnerService } from 'ngx-spinner';
import { ActionsRelevantService } from '../../../services/actions-relevant.service';
import { MasterDataEvolutionsService } from '../../../services/master-data-evolutions.service';
import { TipoDocumentoService } from '../../../services/tipo-documento.service';
import { AlertService } from '../../../shared/alert/alert.service';
import { FireDetail } from '../../../types/fire-detail.type';
import { Media } from '../../../types/media.type';
import { OriginDestination } from '../../../types/origin-destination.type';
import { FireDocumentationService } from '../../../services/fire-documentation.service';
import { SavePayloadModal } from '../../../types/save-payload-modal';
import { GenericMaster } from '../../../types/actions-relevant.type';
import { FlexLayoutModule } from '@angular/flex-layout';
import { _isNumberValue } from '@angular/cdk/coercion';

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

interface FormType {
  id?: string;
  idDocumento?: string;
  fecha: Date;
  hora: any;
  procendenciaDestino: any;
  fechaSolicitud: Date;
  horaSolicitud: any;
  tipoDocumento: { id: string; descripcion: string };
  descripcion: string;
  file?: any;
}

@Component({
  selector: 'app-activation-plan',
  standalone: true,
  templateUrl: './activation-plan.component.html',
  styleUrl: './activation-plan.component.scss',
  imports: [
    CommonModule,
    ReactiveFormsModule,
    MatChipsModule,
    MatFormFieldModule,
    MatInputModule,
    MatSelectModule,
    MatDatepickerModule,
    MatNativeDateModule,
    MatButtonModule,
    MatTableModule,
    NgxSpinnerModule,
    NgxFileDropModule,
    MatIconModule,
    FlexLayoutModule,
  ],
  providers: [
    { provide: DateAdapter, useClass: NativeDateAdapter },
    { provide: MAT_DATE_FORMATS, useValue: FORMATO_FECHA },
  ],
})
export class ActivationPlanComponent implements OnInit {
  constructor(
    private dialogRef: MatDialogRef<ActivationPlanComponent>,
    private spinner: NgxSpinnerService,
    public alertService: AlertService
  ) {}

  @ViewChild(MatSort) sort!: MatSort;
  @Output() save = new EventEmitter<SavePayloadModal>();
  @Input() editData: any;
  @Input() esUltimo: boolean | undefined;
  @Input() fire: any;
  @Input() dataMaestros: any;

  file: File | null = null;
  public files: NgxFileDropEntry[] = [];
  fileFlag: boolean = false;

  private fb = inject(FormBuilder);
  dataProps = inject(MAT_DIALOG_DATA) as {
    title: string;
    fire: any;
    fireDetail: FireDetail;
  };

  formData!: FormGroup;

  readonly sections = [{ id: 1, label: 'Documentación' }];

  selectedOption: MatChipListboxChange = { source: null as any, value: 1 };

  onSelectionChange(event: MatChipListboxChange): void {
    this.selectedOption = event;
  }

  public listadoProcedenciaDestino = signal<OriginDestination[]>([]);
  public listadoTipoDocumento = signal<OriginDestination[]>([]);
  public listadoMedios = signal<Media[]>([]);
  public isCreate = signal<number>(-1);
  public isSaving = signal<boolean>(false);
  public tiposPlanes = signal<GenericMaster[]>([]);
  public planes = signal<GenericMaster[]>([]);
  mostrarCamposAdicionales = signal<boolean>(true);

  public dataSource = new MatTableDataSource<any>([]);
  public displayedColumns: string[] = ['idTipoPlan', 'nombrePlan', 'fechaInicio', 'fechaFin', 'fichero', 'opciones'];

  public toast = inject(MatSnackBar);
  private fireDocumentationService = inject(FireDocumentationService);
  public planesService = inject(ActionsRelevantService);
  public masterData = inject(MasterDataEvolutionsService);

  async ngOnInit() {
    this.tiposPlanes.set(this.dataMaestros.tipoPlanes);
    this.formData = this.fb.group({
      idTipoPlan: [null, Validators.required],
      nombrePlan: [''],
      nombrePlanPersonalizado: [''],
      fechaInicio: [new Date(), Validators.required],
      fechaFin: [null],
      autoridad: ['', Validators.required],
      observaciones: [''],
      file: [null],
    });
    console.log("🚀 ~ ActivationPlanComponent ~ nuevosPlanes ~ this.editData.id:", this.editData.id)
    this.dataSource.data = [];
    if (this.editData) {
      if (this.planesService.dataPlanes().length === 0) {
        if (this.planesService.dataPlanes().length === 0) {
          const nuevosPlanes = this.editData.activacionPlanEmergencias.map((plan: any) => ({
            idRegistroActualizacion: this.editData.id ?? 0,
            idTipoPlan: {
              id: plan.tipoPlan?.id ?? 0,
              descripcion: plan.tipoPlan.descipcion ?? '',
            },
            id: plan.id,
            nombrePlan: plan.planEmergenciaPersonalizado ?? '',
            nombrePlanPersonalizado: plan.tipoPlanPersonalizado ?? '',
            fechaInicio: plan.fechaInicio,
            fechaFin: plan.fechaFin,
            autoridad: plan.autoridad,
            observaciones: plan.observaciones,
            file: plan.archivo
              ? {
                  name: plan.archivo.nombreOriginal,
                  id: plan.archivo.id,
                }
              : null,
            archivoSubido: plan.archivo?.id ?? false,
          }));

          this.planesService.dataPlanes.update((planes) => [...planes, ...nuevosPlanes]);
        }
      }
    }
  }

  trackByFn(index: number, item: any): string {
    return item;
  }

  async onSubmit(formDirective: FormGroupDirective) {
    if (this.formData.valid) {
      const data = this.formData.value;
      if (this.isCreate() == -1) {
        this.planesService.dataPlanes.set([data, ...this.planesService.dataPlanes()]);
        console.log('🚀 ~ onSubmit ~ this.planesService.dataPlanes:', this.planesService.dataPlanes());
      } else {
        this.editarItem(this.isCreate());
      }
      formDirective.resetForm({
            
        fechaInicio: new Date(),
      });
      this.formData.reset();
    } else {
      console.log('🚀 ~ ActivationPlanComponent ~ onSubmit ~ else:', 'else');
      this.formData.markAllAsTouched();
    }
  }

  async sendDataToEndpoint() {
    if (this.planesService.dataPlanes().length > 0 && !this.editData) {
      this.save.emit({ save: true, delete: false, close: false, update: false });
    } else {
      if (this.editData) {
        this.save.emit({ save: false, delete: false, close: false, update: true });
      }
    }
  }

  async loadTipo(event: any) {
    this.spinner.show();
    const tipo_id = event?.value?.id ?? event.id;
    this.mostrarCamposAdicionales.set(tipo_id !== 6);

    if (tipo_id !== 6) {
      // const planes = await this.masterData.getTypesPlansByPlan(this.fire.provincia.idCcaa, tipo_id);
      this.formData.patchValue({
        // nombrePlan: planes[0]?.descripcion ?? '',
        nombrePlanPersonalizado: '',
      });
    } else {
      this.formData.patchValue({ nombrePlan: '' });
    }
    this.spinner.hide();
  }

  getTipo(value: any) {
    var tipo: any;
    if (_isNumberValue(value)) {
      tipo = this.tiposPlanes().find((tipo) => tipo.id === value) || null;
    } else {
      tipo = this.tiposPlanes().find((tipo) => tipo.id === value.id) || null;
    }
    return tipo.descripcion;
  }

  delete() {
    this.save.emit({ save: false, delete: true, close: false, update: false });
  }

  async seleccionarItem(index: number) {
    this.isCreate.set(index);
    const data = this.planesService.dataPlanes()[index];

    var ob = this.tiposPlanes().find((tipo) => (typeof data.idTipoPlan === 'number' ? tipo.id === data.idTipoPlan : tipo.id === data.idTipoPlan.id));

    this.formData.get('idTipoPlan')?.setValue(ob);
    this.formData.get('nombrePlan')?.setValue(data.nombrePlan);
    this.formData.get('nombrePlanPersonalizado')?.setValue(data.nombrePlanPersonalizado);
    this.formData.get('fechaInicio')?.setValue(data.fechaInicio);
    this.formData.get('fechaFin')?.setValue(data.fechaFin);
    this.formData.get('autoridad')?.setValue(data.autoridad);
    this.formData.get('observaciones')?.setValue(data.observaciones);
  }

  editarItem(index: number) {
    const dataEditada = this.formData.value;
    console.log('🚀 ~ editarItem ~ dataEditada:', dataEditada);
    this.planesService.dataPlanes.update((data) => {
      data[index] = { ...data[index], ...dataEditada };
      return [...data];
    });
    this.isCreate.set(-1);
    this.formData.reset();
  }

  eliminarItem(index: number) {
    this.planesService.dataPlanes.update((data) => {
      data.splice(index, 1);
      return [...data];
    });
  }

  closeModal() {
    this.save.emit({ save: false, delete: false, close: true, update: false });
  }

  public dropped(files: NgxFileDropEntry[]) {
    for (const droppedFile of files) {
      if (droppedFile.fileEntry.isFile) {
        const fileEntry = droppedFile.fileEntry as FileSystemFileEntry;
        fileEntry.file((file: File) => {
          this.file = file;
          this.fileFlag = true;

          this.formData.patchValue({ file });
        });
      } else {
        const fileEntry = droppedFile.fileEntry as FileSystemDirectoryEntry;
        console.log(droppedFile.relativePath, fileEntry);
      }
    }
  }

  public fileOver(event: any) {
    console.log(event);
  }

  public fileLeave(event: any) {
    console.log(event);
  }

  getDescripcionProcedenciaDestion(procedenciaDestino: any[]) {
    if (procedenciaDestino.length === 0) {
      return 'Sin información selecionada';
    } else {
      return procedenciaDestino.map((obj) => obj.descripcion).join(', ');
    }
  }

  getFechaHora(fecha: Date, hora: string, format: string = 'MM/DD/YY HH:mm'): any {
    if (hora && fecha) {
      const [horas, minutos] = hora.split(':').map(Number);
      const fechaHora = new Date(fecha);
      fechaHora.setHours(horas, minutos, 0, 0);

      return moment(fechaHora).format(format);
    }
  }

  getFechaHoraIso(fechaHora: string): any {
    if (fechaHora) {
      const [fecha, hora] = fechaHora.split(' ');
      const [mes, dia, anio] = fecha.split('/');
      const anioCompleto = `20${anio}`;
      const dateTime = new Date(`${anioCompleto}-${mes}-${dia}T${hora}:00.000Z`);

      return dateTime.toISOString();
    }
  }

  showToast({ title, txt = 'Cerrar' }: { title: string; txt?: string }) {
    this.toast.open(title, txt, {
      duration: 3000,
      horizontalPosition: 'center',
      verticalPosition: 'bottom',
    });
  }

  trackByIdDocumento(index: number, opcion: any): number {
    return opcion.id;
  }

  async onFileNameClick(data: any) {
    console.log('🚀 ~ ActivationPlanComponent ~ onFileNameClick ~ data:', data);
    try {
      const blob = await this.fireDocumentationService.getFile(data.id);
      const url = window.URL.createObjectURL(blob);
      const a = document.createElement('a');
      a.href = url;
      a.download = data.nombreOriginal;
      document.body.appendChild(a);
      a.click();

      window.URL.revokeObjectURL(url);
      document.body.removeChild(a);
    } catch (error) {
      console.error('Error al descargar el archivo:', error);
    }
  }

  getForm(atributo: string): any {
    return this.formData.controls[atributo];
  }

  getFormatdate(date: any) {
    if (date) {
      return moment(date).format('DD/MM/YYYY');
    } else {
      return 'Sin fecha selecionada.';
    }
  }
}
