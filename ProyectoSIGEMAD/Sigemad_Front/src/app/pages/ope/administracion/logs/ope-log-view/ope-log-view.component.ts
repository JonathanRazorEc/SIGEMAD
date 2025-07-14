import { CommonModule } from '@angular/common';
import { Component, Inject, inject, OnInit, signal } from '@angular/core';
import { FormControl, FormGroup, FormsModule, ReactiveFormsModule, Validators } from '@angular/forms';
import { FlexLayoutModule } from '@angular/flex-layout';
import { MatAutocompleteModule } from '@angular/material/autocomplete';
import { MatButtonModule } from '@angular/material/button';
import { DateAdapter, MAT_DATE_FORMATS, MatNativeDateModule, NativeDateAdapter } from '@angular/material/core';
import { MatDatepickerModule } from '@angular/material/datepicker';
import { MAT_DIALOG_DATA, MatDialog, MatDialogModule, MatDialogRef } from '@angular/material/dialog';
import { MatExpansionModule } from '@angular/material/expansion';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatIconModule } from '@angular/material/icon';
import { MatInputModule } from '@angular/material/input';
import { MatSelectChange, MatSelectModule } from '@angular/material/select';
import { Router } from '@angular/router';
import { NgxSpinnerModule, NgxSpinnerService } from 'ngx-spinner';
import { MatSnackBar } from '@angular/material/snack-bar';

// PCD
import { DragDropModule } from '@angular/cdk/drag-drop';
import { FormFieldComponent } from '@shared/Inputs/field.component';
import { TooltipDirective } from '@shared/directive/tooltip/tooltip.directive';
import { AlertService } from '@shared/alert/alert.service';
import { LocalFiltrosOpePeriodos } from '@services/ope/administracion/local-filtro-ope-periodos.service';
import { OpePeriodosService } from '@services/ope/administracion/ope-periodos.service';
import moment from 'moment';
import { FechaValidator } from '@shared/validators/fecha-validator';

import { FORMATO_FECHA } from '@type/date-formats';
import { FECHA_MAXIMA_DATEPICKER, FECHA_MINIMA_DATEPICKER } from '@type/constants';
import { MomentDateAdapter } from '@angular/material-moment-adapter';
import { OpePeriodosTiposService } from '@services/ope/administracion/ope-periodos-tipos.service';
import { OpePeriodoTipo } from '@type/ope/administracion/ope-periodo-tipo.type';
// FIN PCD

@Component({
  selector: 'ope-log-view',
  standalone: true,
  imports: [
    CommonModule,
    ReactiveFormsModule,
    FormsModule,
    MatDialogModule,
    FormFieldComponent,
    MatFormFieldModule,
    MatInputModule,
    MatSelectModule,
    MatButtonModule,
    MatAutocompleteModule,
    MatIconModule,
    FlexLayoutModule,
    MatExpansionModule,
    MatDatepickerModule,
    MatNativeDateModule,
    NgxSpinnerModule,
    TooltipDirective,
    DragDropModule,
  ],
  providers: [
    { provide: DateAdapter, useClass: MomentDateAdapter },
    { provide: MAT_DATE_FORMATS, useValue: FORMATO_FECHA },
  ],
  templateUrl: './ope-log-view.component.html',
  styleUrl: './ope-log-view.component.scss',
})
export class OpeLogView {
  constructor(
    public dialogRef: MatDialogRef<OpeLogView>,

    @Inject(MAT_DIALOG_DATA) public data: { opeLog: any }
  ) {}

  getKeys(obj: any): string[] {
    return Object.keys(obj);
  }

  formatValue(value: any): string {
    if (value === null || value === undefined) {
      return '-';
    }
    if (typeof value === 'object') {
      return JSON.stringify(value, null, 2);
    }
    return value;
  }

  getChunks<T>(array: T[], chunkSize: number): T[][] {
    const chunks = [];
    for (let i = 0; i < array.length; i += chunkSize) {
      chunks.push(array.slice(i, i + chunkSize));
    }
    return chunks;
  }
}
