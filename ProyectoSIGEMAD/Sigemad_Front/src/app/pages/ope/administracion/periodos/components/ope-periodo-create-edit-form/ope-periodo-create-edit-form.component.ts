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
import { OpeErrorsService } from '@shared/services/ope/ope-errors.service';
// FIN PCD

@Component({
  selector: 'ope-periodo-create-edit',
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
  templateUrl: './ope-periodo-create-edit-form.component.html',
  styleUrl: './ope-periodo-create-edit-form.component.scss',
})
export class OpePeriodoCreateEdit implements OnInit {
  constructor(
    private filtrosOpePeriodosService: LocalFiltrosOpePeriodos,
    private opePeriodosService: OpePeriodosService,
    private opePeriodosTiposService: OpePeriodosTiposService,
    public dialogRef: MatDialogRef<OpePeriodoCreateEdit>,
    private matDialog: MatDialog,
    public alertService: AlertService,
    private router: Router,

    @Inject(MAT_DIALOG_DATA) public data: { opePeriodo: any }
  ) {}

  //public filtrosIncendioService = inject(LocalFiltrosIncendio);

  public opePeriodosTipos = signal<OpePeriodoTipo[]>([]);

  public formData!: FormGroup;

  public today: string = new Date().toISOString().split('T')[0];

  private spinner = inject(NgxSpinnerService);

  //PCD
  public snackBar = inject(MatSnackBar);
  public opeErrorsService = inject(OpeErrorsService);
  public fechaMinimaDatePicker = FECHA_MINIMA_DATEPICKER;
  public fechaMaximaDatePicker = FECHA_MAXIMA_DATEPICKER;
  // FIN PCD

  async ngOnInit() {
    this.formData = new FormGroup(
      {
        nombre: new FormControl('', Validators.required),
        opePeriodoTipo: new FormControl('', Validators.required),
        fechaInicioFaseSalida: new FormControl(null, [Validators.required, FechaValidator.validarFecha]),
        fechaFinFaseSalida: new FormControl(null, [Validators.required, FechaValidator.validarFecha]),
        fechaInicioFaseRetorno: new FormControl(null, [Validators.required, FechaValidator.validarFecha]),
        fechaFinFaseRetorno: new FormControl(null, [Validators.required, FechaValidator.validarFecha]),
      },
      {
        validators: [
          FechaValidator.validarFechaFinPosteriorFechaInicio('fechaInicioFaseSalida', 'fechaFinFaseSalida'),
          FechaValidator.validarFechaFinPosteriorFechaInicio('fechaInicioFaseRetorno', 'fechaFinFaseRetorno'),
        ],
      }
    );

    if (this.data.opePeriodo?.id) {
      this.formData.patchValue({
        id: this.data.opePeriodo.id,
        nombre: this.data.opePeriodo.nombre,
        opePeriodoTipo: this.data.opePeriodo.idOpePeriodoTipo,
        fechaInicioFaseSalida: moment(this.data.opePeriodo.fechaInicioFaseSalida).format('YYYY-MM-DD'),
        fechaFinFaseSalida: moment(this.data.opePeriodo.fechaFinFaseSalida).format('YYYY-MM-DD'),
        fechaInicioFaseRetorno: moment(this.data.opePeriodo.fechaInicioFaseRetorno).format('YYYY-MM-DD'),
        fechaFinFaseRetorno: moment(this.data.opePeriodo.fechaFinFaseRetorno).format('YYYY-MM-DD'),
      });
    }

    const opePeriodosTipos = await this.opePeriodosTiposService.get();
    this.opePeriodosTipos.set(opePeriodosTipos);
  }

  async onSubmit() {
    if (this.formData.valid) {
      this.spinner.show();
      const data = this.formData.value;

      if (this.data.opePeriodo?.id) {
        data.id = this.data.opePeriodo.id;
        await this.opePeriodosService
          .update(data)
          .then((response) => {
            // PCD
            /*
            this.snackBar
              .open('Datos modificados correctamente!', '', {
                duration: 3000,
                horizontalPosition: 'center',
                verticalPosition: 'bottom',
                panelClass: ['snackbar-verde'],
              })
              .afterDismissed()
              .subscribe(() => {
             */
            this.closeModal({ refresh: true });
            this.spinner.hide();
            //});
            // FIN PCD
          })
          .catch((error) => {
            this.spinner.hide();
            this.snackBar.open(this.opeErrorsService.obtenerMensajeError(error, 'opePeriodos'), '', {
              duration: 3000,
              horizontalPosition: 'center',
              verticalPosition: 'bottom',
              panelClass: ['snackbar-rojo'],
            });
            console.error('Error', error);
          });
      } else {
        await this.opePeriodosService
          .post(data)
          .then((response) => {
            /*
            this.snackBar
              .open('Datos creados correctamente!', '', {
                duration: 3000,
                horizontalPosition: 'center',
                verticalPosition: 'bottom',
                panelClass: ['snackbar-verde'],
              })
              .afterDismissed()
              .subscribe(() => {
              */
            this.closeModal({ refresh: true });
            this.spinner.hide();
            //});
          })
          .catch((error) => {
            this.spinner.hide();
            this.snackBar.open(this.opeErrorsService.obtenerMensajeError(error, 'opePeriodos'), '', {
              duration: 3000,
              horizontalPosition: 'center',
              verticalPosition: 'bottom',
              panelClass: ['snackbar-rojo'],
            });
            console.error('Error', error);
          });
      }
    } else {
      this.formData.markAllAsTouched();
    }
  }

  closeModal(params?: any) {
    this.dialogRef.close(params);
  }

  getForm(atributo: string): any {
    return this.formData.controls[atributo];
  }

  onChangeTipoPeriodo(event: MatSelectChange) {
    const selectedOption = event.source.selected as any;
    const textoSeleccionado = selectedOption?.viewValue || '';
    const fechaInicioFaseSalida = this.formData.get('fechaInicioFaseSalida')?.value;
    const anio =
      fechaInicioFaseSalida && !isNaN(Date.parse(fechaInicioFaseSalida)) ? new Date(fechaInicioFaseSalida).getFullYear() : new Date().getFullYear();

    let fechaInicioFaseSalidaFormateada = fechaInicioFaseSalida;
    let fechaFinFaseSalidaFormateada = this.formData.get('fechaFinFaseSalida')?.value;
    let fechaInicioFaseRetornoFormateada = this.formData.get('fechaInicioFaseRetorno')?.value;
    let fechaFinFaseRetornoFormateada = this.formData.get('fechaFinFaseRetorno')?.value;

    if (textoSeleccionado === 'Verano') {
      fechaInicioFaseSalidaFormateada = `${anio}-06-15`;
      fechaFinFaseSalidaFormateada = `${anio}-08-15`;
      fechaInicioFaseRetornoFormateada = `${anio}-07-15`;
      fechaFinFaseRetornoFormateada = `${anio}-09-15`;
    } else {
      fechaInicioFaseSalidaFormateada = null;
      fechaFinFaseSalidaFormateada = null;
      fechaInicioFaseRetornoFormateada = null;
      fechaFinFaseRetornoFormateada = null;
    }

    this.formData.patchValue({
      //nombre: textoSeleccionado + ' ' + anio,
      nombre: textoSeleccionado === 'Otro' ? '' : textoSeleccionado + ' ' + anio,

      fechaInicioFaseSalida:
        fechaInicioFaseSalidaFormateada && !isNaN(Date.parse(fechaInicioFaseSalidaFormateada))
          ? moment(fechaInicioFaseSalidaFormateada).format('YYYY-MM-DD')
          : null,
      fechaFinFaseSalida:
        fechaFinFaseSalidaFormateada && !isNaN(Date.parse(fechaFinFaseSalidaFormateada))
          ? moment(fechaFinFaseSalidaFormateada).format('YYYY-MM-DD')
          : null,
      fechaInicioFaseRetorno:
        fechaInicioFaseRetornoFormateada && !isNaN(Date.parse(fechaInicioFaseRetornoFormateada))
          ? moment(fechaInicioFaseRetornoFormateada).format('YYYY-MM-DD')
          : null,
      fechaFinFaseRetorno:
        fechaFinFaseRetornoFormateada && !isNaN(Date.parse(fechaFinFaseRetornoFormateada))
          ? moment(fechaFinFaseRetornoFormateada).format('YYYY-MM-DD')
          : null,
    });
  }

  getFechaFormateadaConHorasMinutosYSegundos = (fecha: any) => {
    if (!fecha) {
      return '';
    }
    return moment(fecha).format('DD/MM/YYYY HH:mm:ss');
  };
}
