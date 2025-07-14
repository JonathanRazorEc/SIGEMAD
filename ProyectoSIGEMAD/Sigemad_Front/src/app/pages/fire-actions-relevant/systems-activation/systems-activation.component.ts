import { Component, EventEmitter, inject, Input, Output, signal, ViewChild } from '@angular/core';
import { CommonModule } from '@angular/common';
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
import { MatSelectChange, MatSelectModule } from '@angular/material/select';
import { MatSnackBar } from '@angular/material/snack-bar';
import { MatSort } from '@angular/material/sort';
import { MatTableDataSource, MatTableModule } from '@angular/material/table';
import moment from 'moment';
import { NgxSpinnerModule, NgxSpinnerService } from 'ngx-spinner';
import { ActionsRelevantService } from '../../../services/actions-relevant.service';
import { SavePayloadModal } from '../../../types/save-payload-modal';
import { GenericMaster } from '../../../types/actions-relevant.type';
import { _isNumberValue } from '@angular/cdk/coercion';
import { FECHA_MAXIMA_DATETIME, FECHA_MINIMA_DATETIME } from '@type/constants';
import { DateUtils } from '@shared/utils/date-utils';
import { FormatFechaCESTPipe } from '@shared/pipes/format-fecha-cest.pipe';

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
  selector: 'app-systems-activation',
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
    FormatFechaCESTPipe
  ],
  providers: [
    { provide: DateAdapter, useClass: NativeDateAdapter },
    { provide: MAT_DATE_FORMATS, useValue: FORMATO_FECHA },
  ],
  templateUrl: './systems-activation.component.html',
  styleUrl: './systems-activation.component.scss',
})
export class SystemsActivationComponent {
  @ViewChild(MatSort) sort!: MatSort;
  data = inject(MAT_DIALOG_DATA) as { title: string; idIncendio: number };
  @Output() save = new EventEmitter<SavePayloadModal>();
  @Input() editData: any;
  @Input() esUltimo: boolean | undefined;
  @Input() fire: any;
  @Input() dataMaestros: any;
  @Input() isFromRecords = false;

  public sitemasService = inject(ActionsRelevantService);
  public toast = inject(MatSnackBar);
  private fb = inject(FormBuilder);
  public matDialog = inject(MatDialog);
  private spinner = inject(NgxSpinnerService);

  public displayedColumns: string[] = ['idTipoSistemaEmergencia', 'fechaHoraActivacion', 'fechaHoraActualizacion', 'autoridad', 'opciones'];

  formData!: FormGroup;
  
  // Variables para fechas
  public fechaMinimaDateTime = FECHA_MINIMA_DATETIME;
  public fechaMaximaDateTime = FECHA_MAXIMA_DATETIME;

  public isCreate = signal<number>(-1);
  public tiposActivacion = signal<GenericMaster[]>([]);
  public modosActivacion = signal<GenericMaster[]>([]);
  mostrarCamposAdicionales = signal<Number>(0);
  public dataSource = new MatTableDataSource<any>([]);

  async ngOnInit() {
    this.tiposActivacion.set(this.dataMaestros.tiposActivacion);
    this.modosActivacion.set(this.dataMaestros.modosActivacion);

    this.formData = this.fb.group({
      idTipoSistemaEmergencia: [null, Validators.required],
      fechaHoraActivacion: [DateUtils.getCurrentCESTDate(), Validators.required],
      fechaHoraActualizacion: [null, Validators.required],
      autoridad: [''],
      descripcionSolicitud: [''],
      observaciones: [''],
      idModoActivacion: [null],
      fechaActivacion: [null],
      codigo: [''],
      nombre: [''],
      urlAcceso: [''],
      fechaHoraPeticion: [null],
      fechaAceptacion: [null],
      peticiones: [''],
      mediosCapacidades: [''],
    });

    if (this.editData) {
      console.log('🚀 ~ CecodComponent ~ ngOnInit ~ this.editData:', this.editData);
      console.log('🚀 ~ CecodComponent ~ ngOnInit ~ this.cecodService.dataCecod():', this.sitemasService.dataSistemas());
      if (this.sitemasService.dataSistemas().length === 0) {
        this.sitemasService.dataSistemas.set(this.editData.activacionSistemas);
      }
    }
    this.spinner.hide();
  }

  async onSubmit(formDirective: FormGroupDirective) {
    if (this.formData.valid) {
      const data = this.formData.value;
      const { fechaHoraActivacion, fechaHoraActualizacion, fechaHoraPeticion } = data;

      data.fechaHoraActivacion = DateUtils.fromCestToUtc(fechaHoraActivacion);
      data.fechaHoraActualizacion = fechaHoraActualizacion && DateUtils.fromCestToUtc(fechaHoraActualizacion);
      data.fechaHoraPeticion = fechaHoraActualizacion && DateUtils.fromCestToUtc(fechaHoraPeticion);

      if (this.isCreate() == -1) {
        this.sitemasService.dataSistemas.set([data, ...this.sitemasService.dataSistemas()]);
      } else {
        this.editarItem(this.isCreate());
      }

      formDirective.resetForm({
        fechaHoraActivacion: DateUtils.getCurrentCESTDate(),
      });
      this.formData.reset({
        fechaHoraActivacion: DateUtils.getCurrentCESTDate(),
      });
    } else {
      this.formData.markAllAsTouched();
    }
  }

  async sendDataToEndpoint() {
    if (this.sitemasService.dataSistemas().length > 0 && !this.editData) {
      this.save.emit({ save: true, delete: false, close: false, update: false });
    } else {
      if (this.editData) {
        this.save.emit({ save: false, delete: false, close: false, update: true });
      }
    }
  }

  editarItem(index: number) {
    const dataEditada = this.formData.value;
    this.sitemasService.dataSistemas.update((data) => {
      data[index] = { ...data[index], ...dataEditada };
      return [...data];
    });
    this.isCreate.set(-1);
    this.formData.reset();
  }

  eliminarItem(index: number) {
    this.sitemasService.dataSistemas.update((data) => {
      data.splice(index, 1);
      return [...data];
    });
  }

  async seleccionarItem(index: number) {
    this.isCreate.set(index);
    const data = this.sitemasService.dataSistemas()[index];
    const { fechaHoraActivacion, fechaHoraActualizacion, fechaHoraPeticion } = data;

    data.fechaHoraActivacion = DateUtils.fromUtcToCest(fechaHoraActivacion)!;
    data.fechaHoraActualizacion = fechaHoraActualizacion && DateUtils.fromUtcToCest(fechaHoraActualizacion)!;
    data.fechaHoraPeticion = fechaHoraActualizacion && DateUtils.fromUtcToCest(fechaHoraPeticion)!;

    console.log('🚀 ~ NotificationsComponent ~ seleccionarItem ~ data:', data);
    var ob = this.tiposActivacion().find((tipo) =>
      typeof data.idTipoSistemaEmergencia === 'number' ? tipo.id === data.idTipoSistemaEmergencia : tipo.id === data.idTipoSistemaEmergencia.id
    )?.id;
    console.log('🚀 ~ SystemsActivationComponent ~ seleccionarItem ~ this.modosActivacion():', this.modosActivacion());
    var ob2 =
      this.modosActivacion().find((tipo) =>
        typeof data.idModoActivacion === 'number' ? tipo.id === data.idModoActivacion : tipo.id === data.idModoActivacion?.id
      )?.id ?? null;

    console.log('🚀 ~ SystemsActivationComponent ~ seleccionarItem ~ ob2:', ob2);
    this.mostrarCamposAdicionales.set(ob ?? 0);
    this.formData.get('idTipoSistemaEmergencia')?.setValue(ob);
    this.formData.get('fechaHoraActivacion')?.setValue(data.fechaHoraActivacion);
    this.formData.get('fechaHoraActualizacion')?.setValue(data.fechaHoraActualizacion);
    this.formData.get('autoridad')?.setValue(data.autoridad);
    this.formData.get('descripcionSolicitud')?.setValue(data.descripcionSolicitud);
    this.formData.get('observaciones')?.setValue(data.observaciones);
    this.formData.get('idModoActivacion')?.setValue(ob2);
    this.formData.get('fechaActivacion')?.setValue(data.fechaActivacion);
    this.formData.get('codigo')?.setValue(data.codigo);
    this.formData.get('nombre')?.setValue(data.nombre);
    this.formData.get('urlAcceso')?.setValue(data.urlAcceso);
    this.formData.get('fechaHoraPeticion')?.setValue(data.fechaHoraPeticion);
    this.formData.get('fechaAceptacion')?.setValue(data.fechaAceptacion);
    this.formData.get('peticiones')?.setValue(data.peticiones);
    this.formData.get('mediosCapacidades')?.setValue(data.mediosCapacidades);
  }

  async loadTipo(event: any) {
    this.spinner.show();
    const tipo_id = event?.value?.id ?? event.id ?? event.value;
    console.log('🚀 ~ SystemsActivationComponent ~ loadTipo ~ tipo_id:', tipo_id);
    this.mostrarCamposAdicionales.set(tipo_id);

    this.formData.patchValue({
      idModoActivacion: null,
      fechaActivacion: null,
      codigo: '',
      nombre: '',
      urlAcceso: '',
      fechaHoraPeticion: null,
      fechaAceptacion: null,
      peticiones: '',
      mediosCapacidades: '',
    });

    this.spinner.hide();
  }

  getFormatdate(date: any) {
    if (date) {
      return moment(date).format('DD/MM/YYYY');
    } else {
      return 'Sin fecha selecionada.';
    }
  }

  getTipoNotificacion(value: any) {
    var tipo: any;

    if (_isNumberValue(value)) {
      tipo = this.tiposActivacion().find((tipo) => tipo.id === value) || null;
    } else {
      tipo = this.tiposActivacion().find((tipo) => tipo.id === value.id) || null;
    }

    return tipo.descripcion;
  }

  cleanSelect() {
    this.formData.get('idModoActivacion')?.setValue(null);
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

  isInteger(value: any): boolean {
    return Number.isInteger(value);
  }
}
