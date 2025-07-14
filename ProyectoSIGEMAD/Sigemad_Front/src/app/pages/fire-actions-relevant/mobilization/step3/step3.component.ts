import { CommonModule } from '@angular/common';
import { Component, Input, signal } from '@angular/core';
import { FlexLayoutModule } from '@angular/flex-layout';
import { FormGroup, ReactiveFormsModule } from '@angular/forms';
import { DateAdapter, MAT_DATE_FORMATS, MatNativeDateModule, NativeDateAdapter } from '@angular/material/core';
import { MatDatepickerModule } from '@angular/material/datepicker';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { MatSelectModule } from '@angular/material/select';
import { GenericMaster } from '../../../../types/actions-relevant.type';
import { MatCheckboxModule } from '@angular/material/checkbox';

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
  selector: 'app-step3',
  standalone: true,
  imports: [
    CommonModule,
    MatFormFieldModule,
    MatSelectModule,
    ReactiveFormsModule,
    FlexLayoutModule,
    MatDatepickerModule,
    MatNativeDateModule,
    MatInputModule,
    MatCheckboxModule,
  ],
  templateUrl: './step3.component.html',
  styleUrl: './step3.component.scss',
  providers: [
    { provide: DateAdapter, useClass: NativeDateAdapter },
    { provide: MAT_DATE_FORMATS, useValue: FORMATO_FECHA },
  ],
})
export class Step3Component {
  @Input() formGroup!: FormGroup;
  @Input() dataMaestros: any;
  public destinos = signal<GenericMaster[]>([]);

  async ngOnInit() {
    this.destinos.set(this.dataMaestros.destinos);
  }

  getForm(atributo: string): any {
    return this.formGroup.controls[atributo];
  }
}
