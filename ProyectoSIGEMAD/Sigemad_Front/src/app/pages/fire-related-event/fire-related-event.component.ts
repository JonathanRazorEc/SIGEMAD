import { CommonModule } from '@angular/common';
import { Component, OnInit, ViewChild, inject, signal } from '@angular/core';
import { FormBuilder, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
import { MatChipListboxChange, MatChipsModule } from '@angular/material/chips';
import { MatDatepickerModule } from '@angular/material/datepicker';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { MatSelectModule } from '@angular/material/select';

import { MatButtonModule } from '@angular/material/button';
import { MAT_DIALOG_DATA, MatDialogRef } from '@angular/material/dialog';
import { MatIconModule } from '@angular/material/icon';
import { MatSort } from '@angular/material/sort';
import { MatTableDataSource, MatTableModule } from '@angular/material/table';
import { NgxSpinnerModule, NgxSpinnerService } from 'ngx-spinner';

import { DateAdapter, MAT_DATE_FORMATS, MatNativeDateModule, NativeDateAdapter } from '@angular/material/core';
import moment from 'moment';

import { SucesosRelacionadosService } from '../../services/sucesos-relacionados.service';
import { AlertService } from '../../shared/alert/alert.service';
import { FireDetail } from '../../types/fire-detail.type';
import { FireRelatedEventForm } from './components/fire-related-event-form/fire-related-event-form.component';
import { DragDropModule } from '@angular/cdk/drag-drop';
import { MatSnackBar } from '@angular/material/snack-bar';

const FORMATO_FECHA = {
  parse: {
    dateInput: 'LL', // Definir el formato de entrada
  },
  display: {
    dateInput: 'LL', // Definir cómo mostrar la fecha
    monthYearLabel: 'MMM YYYY',
    dateA11yLabel: 'LL',
    monthYearA11yLabel: 'MMMM YYYY',
  },
};

@Component({
  selector: 'app-fire-create',
  standalone: true,
  templateUrl: './fire-related-event.component.html',
  styleUrls: ['./fire-related-event.component.scss'],
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
    MatIconModule,
    NgxSpinnerModule,
    FireRelatedEventForm,
    DragDropModule,
  ],
  providers: [
    { provide: DateAdapter, useClass: NativeDateAdapter },
    { provide: MAT_DATE_FORMATS, useValue: FORMATO_FECHA },
  ],
})
export class FireRelatedEventComponent implements OnInit {
  constructor(
    private dialogRef: MatDialogRef<FireRelatedEventComponent>,
    private spinner: NgxSpinnerService,
    public alertService: AlertService
  ) {}

  @ViewChild(FireRelatedEventForm) FireRelatedEventForm!: FireRelatedEventForm;
  @ViewChild(MatSort) sort!: MatSort;

  public sucesosRelacionadosService = inject(SucesosRelacionadosService);

  public snackBar = inject(MatSnackBar);

  private fb = inject(FormBuilder);

  dataProps = inject(MAT_DIALOG_DATA) as {
    title: string;
    fire: any;
    fireDetail: FireDetail;
  };

  formData!: FormGroup;

  readonly sections = [{ id: 1, label: 'Sucesos Relacionados' }];
  selectedOption: MatChipListboxChange = { source: null as any, value: 1 };

  onSelectionChange(event: MatChipListboxChange): void {
    this.selectedOption = event;
  }

  ////////////////////////////////
  public dataRelatedEvent = signal<any>([]);
  public isCreate = signal<number>(-1);
  public isSaving = signal<boolean>(false);

  public listadoClaseSuceso = signal<any[]>([]);
  public listadoTerritorio = signal<any[]>([]);
  public listadoPaises = signal<any[]>([]);
  public listadoCCAA = signal<any[]>([]);
  public listadoProvincia = signal<any[]>([]);
  public listadoMunicipio = signal<any[]>([]);

  public dataSource = new MatTableDataSource<any>([]);

  public displayedColumns: string[] = ['fecha', 'Tipo de suceso', 'Estado', 'Denominación', 'opciones'];

  public hasRelatedRecords = false;

  async ngOnInit() {
    this.formData = this.fb.group({
      fecha: ['', Validators.required],
      hora: ['', Validators.required],
      procendenciaDestino: ['', Validators.required],
      medio: ['', Validators.required],
      asunto: ['', Validators.required],
      observaciones: ['', Validators.required],
      CCAA: ['', Validators.required],
      pais: ['', Validators.required],
      territorio: ['', Validators.required],
      municipio: ['', Validators.required],
      provincia: ['', Validators.required],
    });
  }

  trackByFn(index: number, item: any): number {
    return item.id;
  }

  async callSave() {
    this.FireRelatedEventForm.guardarAgregar();
  }

  async delete() {
    this.spinner.show();

    this.alertService
      /*
      .showAlert({
        title: '¿Estás seguro?',
        text: '¡No podrás revertir esto!',
        icon: 'warning',
        showCancelButton: true,
        cancelButtonColor: '#d33',
        confirmButtonText: '¡Sí, eliminar!',
      })
        */

      // PCD
      .showAlert({
        title: '¿Estás seguro de eliminar el registro?',
        showCancelButton: true,
        cancelButtonColor: '#d33',
        confirmButtonText: '¡Sí, eliminar!',
        cancelButtonText: 'Cancelar',
        customClass: {
          title: 'sweetAlert-fsize20',
        },
      })
      // FIN PCD
      .then(async (result) => {
        if (result.isConfirmed) {
          await this.sucesosRelacionadosService.delete(this.dataProps.fireDetail.id);
          this.closeModal({ refresh: true });
          this.spinner.hide();

          this.snackBar
            .open('Datos eliminados correctamente!', '', {
              duration: 3000,
              horizontalPosition: 'center',
              verticalPosition: 'bottom',
              panelClass: ['snackbar-verde'],
            });
        } else {
          this.spinner.hide();
        }
      });
  }

  seleccionarItem(index: number) {
    this.isCreate.set(index);
  }

  editarItem(index: number) {
    /*
    const dataEditada = this.formData.value;
    this.dataOtherInformation.update((data) => {
      data[index] = { ...data[index], ...dataEditada };
      return [...data];
    });
    this.isCreate.set(-1);
    this.formData.reset();
    */
  }

  eliminarItem(index: number) {
    /*
    this.dataOtherInformation.update((data) => {
      data.splice(index, 1);
      return [...data];
    });
    */
  }

  getFormatdate(date: any) {
    return moment(date).format('DD/MM/YY');
  }

  closeModal(params?: any) {
    this.dialogRef.close(params);
  }

  getDescripcionProcedenciaDestion(procedenciaDestino: any[]) {
    return procedenciaDestino?.map((obj) => obj?.descripcion).join(', ');
  }

  getFechaHora(fecha: Date, hora: string): string {
    const [horas, minutos] = hora.split(':').map(Number);
    const fechaHora = new Date(fecha);
    fechaHora.setHours(horas, minutos, 0, 0);

    return moment(fechaHora).format('MM/DD/YY HH:mm');
    //return fechaHora.toISOString();
  }

  handleRecordsChange(hasRecords: boolean): void {
    this.hasRelatedRecords = hasRecords;
  }
}
