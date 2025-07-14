import { CommonModule } from '@angular/common';
import { Component, Inject, inject, OnInit, signal, SimpleChanges } from '@angular/core';
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
import { MatSelectModule } from '@angular/material/select';
import { NgxSpinnerModule, NgxSpinnerService } from 'ngx-spinner';
import { DragDropModule } from '@angular/cdk/drag-drop';
import { AlertService } from '@shared/alert/alert.service';
import { OpePorcentajesOcupacionAreasEstacionamientoService } from '@services/ope/administracion/ope-porcentajes-ocupacion-areas-estacionamiento.service';
import { UtilsService } from '@shared/services/utils.service';
import { MomentDateAdapter } from '@angular/material-moment-adapter';
import { FORMATO_FECHA } from '@type/date-formats';
import { OpeUtilsService } from '@shared/services/ope/ope-utils.service';
import { LocalFiltrosOpePorcentajesOcupacionAreasEstacionamiento } from '@services/ope/administracion/local-filtro-ope-porcentajes-ocupacion-areas-estacionamiento.service';
import { OpeOcupacionesService } from '@services/ope/administracion/ope-ocupaciones.service';
import { OpeOcupacion } from '@type/ope/administracion/ope-ocupacion.type';
import { OpeErrorsService } from '@shared/services/ope/ope-errors.service';
import { MatSnackBar } from '@angular/material/snack-bar';
import moment from 'moment';

@Component({
  selector: 'ope-porcentaje-ocupacion-area-estacionamiento-create-edit',
  standalone: true,
  imports: [
    CommonModule,
    ReactiveFormsModule,
    FormsModule,
    MatDialogModule,
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
    DragDropModule,
  ],
  providers: [
    { provide: DateAdapter, useClass: MomentDateAdapter },
    { provide: MAT_DATE_FORMATS, useValue: FORMATO_FECHA },
  ],
  templateUrl: './ope-porcentaje-ocupacion-area-estacionamiento-create-edit-form.component.html',
  styleUrl: './ope-porcentaje-ocupacion-area-estacionamiento-create-edit-form.component.scss',
})
export class OpePorcentajeOcupacionAreaEstacionamientoCreateEdit implements OnInit {
  constructor(
    private filtrosOpePorcentajesOcupacionAreasEstacionamientoService: LocalFiltrosOpePorcentajesOcupacionAreasEstacionamiento,
    private opePorcentajesOcupacionAreasEstacionamientoService: OpePorcentajesOcupacionAreasEstacionamientoService,
    private opeOcupacionesService: OpeOcupacionesService,
    public dialogRef: MatDialogRef<OpePorcentajeOcupacionAreaEstacionamientoCreateEdit>,
    public alertService: AlertService,
    @Inject(MAT_DIALOG_DATA) public data: { opePorcentajeOcupacionAreaEstacionamiento: any }
  ) {}

  public opeOcupaciones = signal<OpeOcupacion[]>([]);

  public formData!: FormGroup;

  //public today: string = new Date().toISOString().split('T')[0];

  private spinner = inject(NgxSpinnerService);

  //PCD
  public utilsService = inject(UtilsService);
  public opeUtilsService = inject(OpeUtilsService);
  public snackBar = inject(MatSnackBar);
  public opeErrorsService = inject(OpeErrorsService);
  // FIN PCD

  async ngOnInit() {
    this.formData = new FormGroup({
      opeOcupacion: new FormControl('', Validators.required),
      porcentajeInferior: new FormControl(null, [Validators.required, Validators.min(0), Validators.max(9999), Validators.pattern(/^\d+$/)]),
      porcentajeSuperior: new FormControl(null, [Validators.required, Validators.min(0), Validators.max(9999), Validators.pattern(/^\d+$/)]),
    });

    if (this.data.opePorcentajeOcupacionAreaEstacionamiento?.id) {
      this.formData.patchValue({
        id: this.data.opePorcentajeOcupacionAreaEstacionamiento.id,
        opeOcupacion: this.data.opePorcentajeOcupacionAreaEstacionamiento.idOpeOcupacion,
        porcentajeInferior: this.data.opePorcentajeOcupacionAreaEstacionamiento.porcentajeInferior,
        porcentajeSuperior: this.data.opePorcentajeOcupacionAreaEstacionamiento.porcentajeSuperior,
      });
    }

    const opeOcupaciones = await this.opeOcupacionesService.get();
    this.opeOcupaciones.set(opeOcupaciones);
  }

  async onSubmit() {
    if (this.formData.valid) {
      this.spinner.show();
      const data = this.formData.value;

      if (this.data.opePorcentajeOcupacionAreaEstacionamiento?.id) {
        data.id = this.data.opePorcentajeOcupacionAreaEstacionamiento.id;
        await this.opePorcentajesOcupacionAreasEstacionamientoService
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
            this.snackBar.open(this.opeErrorsService.obtenerMensajeError(error, 'opePorcentajesOcupacionAreaEstacionamiento'), '', {
              duration: 3000,
              horizontalPosition: 'center',
              verticalPosition: 'bottom',
              panelClass: ['snackbar-rojo'],
            });
            console.error('Error', error);
          });
      } else {
        await this.opePorcentajesOcupacionAreasEstacionamientoService
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
            this.snackBar.open(this.opeErrorsService.obtenerMensajeError(error, 'opePorcentajesOcupacionAreaEstacionamiento'), '', {
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

  getFechaFormateadaConHorasMinutosYSegundos = (fecha: any) => {
    if (!fecha) {
      return '';
    }
    return moment(fecha).format('DD/MM/YYYY HH:mm:ss');
  };
}
