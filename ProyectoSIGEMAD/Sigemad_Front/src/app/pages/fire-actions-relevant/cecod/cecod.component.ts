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
  selector: 'app-cecod',
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
  templateUrl: './cecod.component.html',
  styleUrl: './cecod.component.scss',
})
export class CecodComponent {
  @ViewChild(MatSort) sort!: MatSort;
  data = inject(MAT_DIALOG_DATA) as { title: string; idIncendio: number };
  @Output() save = new EventEmitter<SavePayloadModal>();
  @Input() editData: any;
  @Input() esUltimo: boolean | undefined;
  @Input() fire: any;

  public cecodService = inject(ActionsRelevantService);
  public toast = inject(MatSnackBar);
  private fb = inject(FormBuilder);
  public matDialog = inject(MatDialog);
  private spinner = inject(NgxSpinnerService);

  public displayedColumns: string[] = ['fechaInicio', 'fechaFin', 'lugar', 'opciones'];

  formData!: FormGroup;

  public isCreate = signal<number>(-1);
  public dataSource = new MatTableDataSource<any>([]);

  async ngOnInit() {
    this.formData = this.fb.group({
      fechaInicio: [new Date(), Validators.required],
      fechaFin: [null],
      lugar: ['', Validators.required],
      convocados: ['', Validators.required],
      participantes: [''],
      observaciones: [''],
    });

    if (this.editData) {
      console.log('🚀 ~ CecodComponent ~ ngOnInit ~ this.editData:', this.editData);
      console.log('🚀 ~ CecodComponent ~ ngOnInit ~ this.cecodService.dataCecod():', this.cecodService.dataCecod());
      if (this.cecodService.dataCecod().length === 0) {
        this.cecodService.dataCecod.set(this.editData.convocatoriasCECOD);
      }
    }
    this.spinner.hide();
  }

  async onSubmit(formDirective: FormGroupDirective) {
    if (this.formData.valid) {
      const data = this.formData.value;
      if (this.isCreate() == -1) {
        this.cecodService.dataCecod.set([data, ...this.cecodService.dataCecod()]);
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
    if (this.cecodService.dataCecod().length > 0 && !this.editData) {
      this.save.emit({ save: true, delete: false, close: false, update: false });
    } else {
      if (this.editData) {
        this.save.emit({ save: false, delete: false, close: false, update: true });
      }
    }
  }

  editarItem(index: number) {
    const dataEditada = this.formData.value;
    this.cecodService.dataCecod.update((data) => {
      data[index] = { ...data[index], ...dataEditada };
      return [...data];
    });
    this.isCreate.set(-1);
    this.formData.reset();
  }

  eliminarItem(index: number) {
    this.cecodService.dataCecod.update((data) => {
      data.splice(index, 1);
      return [...data];
    });
  }

  async seleccionarItem(index: number) {
    this.isCreate.set(index);
    const data = this.cecodService.dataCecod()[index];
    this.formData.get('fechaInicio')?.setValue(data.fechaInicio);
    this.formData.get('fechaFin')?.setValue(data.fechaFin);
    this.formData.get('lugar')?.setValue(data.lugar);
    this.formData.get('convocados')?.setValue(data.convocados);
    this.formData.get('participantes')?.setValue(data.participantes);
    this.formData.get('observaciones')?.setValue(data.observaciones);
    this.spinner.show();
    this.spinner.hide();
  }

  getFormatdate(date: any) {
    if (date) {
      return moment(date).format('DD/MM/YYYY');
    } else {
      return 'Sin fecha selecionada.';
    }
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
