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
import { MatSelectModule } from '@angular/material/select';
import { MatSnackBar } from '@angular/material/snack-bar';
import { MatSort } from '@angular/material/sort';
import { MatTableDataSource, MatTableModule } from '@angular/material/table';
import moment from 'moment';
import { NgxSpinnerModule, NgxSpinnerService } from 'ngx-spinner';
import { ActionsRelevantService } from '../../../services/actions-relevant.service';
import { SavePayloadModal } from '../../../types/save-payload-modal';
import { GenericMaster } from '../../../types/actions-relevant.type';
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

@Component({
  selector: 'app-notifications',
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
  ],
  providers: [
    { provide: DateAdapter, useClass: NativeDateAdapter },
    { provide: MAT_DATE_FORMATS, useValue: FORMATO_FECHA },
  ],
  templateUrl: './notifications.component.html',
  styleUrl: './notifications.component.scss',
})
export class NotificationsComponent {
  @ViewChild(MatSort) sort!: MatSort;
  data = inject(MAT_DIALOG_DATA) as { title: string; idIncendio: number };
  @Output() save = new EventEmitter<SavePayloadModal>();
  @Input() editData: any;
  @Input() esUltimo: boolean | undefined;
  @Input() fire: any;
  @Input() dataMaestros: any;

  public notificacionesService = inject(ActionsRelevantService);
  public toast = inject(MatSnackBar);
  private fb = inject(FormBuilder);
  public matDialog = inject(MatDialog);
  private spinner = inject(NgxSpinnerService);

  public displayedColumns: string[] = ['idTipoNotificacion', 'fechaHoraNotificacion', 'organosNotificados', 'opciones'];

  formData!: FormGroup;

  public isCreate = signal<number>(-1);
  public tiposNotificaciones = signal<GenericMaster[]>([]);
  public dataSource = new MatTableDataSource<any>([]);

  async ngOnInit() {
    this.tiposNotificaciones.set(this.dataMaestros.tipoNotificaciones);

    this.formData = this.fb.group({
      idTipoNotificacion: [null, Validators.required],
      fechaHoraNotificacion: [new Date(), Validators.required],
      organosNotificados: [''],
      ucpm: [''],
      organismoInternacional: [''],
      otrosPaises: [''],
      observaciones: [''],
    });

    if (this.editData) {
      console.log('🚀 ~ CecodComponent ~ ngOnInit ~ this.editData:', this.editData);
      console.log('🚀 ~ CecodComponent ~ ngOnInit ~ this.cecodService.dataCecod():', this.notificacionesService.dataNotificaciones());
      if (this.notificacionesService.dataNotificaciones().length === 0) {
        this.notificacionesService.dataNotificaciones.set(this.editData.notificacionesEmergencias);
      }
    }
    this.spinner.hide();
  }

  async onSubmit(formDirective: FormGroupDirective) {
    if (this.formData.valid) {
      const data = this.formData.value;
      if (this.isCreate() == -1) {
        this.notificacionesService.dataNotificaciones.set([data, ...this.notificacionesService.dataNotificaciones()]);
      } else {
        this.editarItem(this.isCreate());
      }

      formDirective.resetForm({
        fechaHora: new Date(),
      });
      this.formData.reset();
    } else {
      this.formData.markAllAsTouched();
    }
  }

  async sendDataToEndpoint() {
    if (this.notificacionesService.dataNotificaciones().length > 0 && !this.editData) {
      this.save.emit({ save: true, delete: false, close: false, update: false });
    } else {
      if (this.editData) {
        this.save.emit({ save: false, delete: false, close: false, update: true });
      }
    }
  }

  editarItem(index: number) {
    const dataEditada = this.formData.value;
    this.notificacionesService.dataNotificaciones.update((data) => {
      data[index] = { ...data[index], ...dataEditada };
      return [...data];
    });
    this.isCreate.set(-1);
    this.formData.reset();
  }

  eliminarItem(index: number) {
    this.notificacionesService.dataNotificaciones.update((data) => {
      data.splice(index, 1);
      return [...data];
    });
  }

  async seleccionarItem(index: number) {
    this.isCreate.set(index);
    const data = this.notificacionesService.dataNotificaciones()[index];
    console.log('🚀 ~ NotificationsComponent ~ seleccionarItem ~ data:', data.idTipoNotificacion);
    // if (typeof data.idTipoNotificacion === 'number') {
    var ob = this.tiposNotificaciones().find((tipo) =>
      typeof data.idTipoNotificacion === 'number' ? tipo.id === data.idTipoNotificacion : tipo.id === data.idTipoNotificacion.id
    );

    this.formData.get('idTipoNotificacion')?.setValue(ob);

    // }else{
    //   this.formData.get('idTipoNotificacion')?.setValue(data.idTipoNotificacion);
    // }

    this.formData.get('fechaHoraNotificacion')?.setValue(data.fechaHoraNotificacion);
    this.formData.get('organosNotificados')?.setValue(data.organosNotificados);
    this.formData.get('ucpm')?.setValue(data.ucpm);
    this.formData.get('organismoInternacional')?.setValue(data.organismoInternacional);
    this.formData.get('otrosPaises')?.setValue(data.otrosPaises);
    this.formData.get('observaciones')?.setValue(data.observaciones);
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
      tipo = this.tiposNotificaciones().find((tipo) => tipo.id === value) || null;
    } else {
      tipo = this.tiposNotificaciones().find((tipo) => tipo.id === value.id) || null;
    }

    return tipo.descripcion;
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
