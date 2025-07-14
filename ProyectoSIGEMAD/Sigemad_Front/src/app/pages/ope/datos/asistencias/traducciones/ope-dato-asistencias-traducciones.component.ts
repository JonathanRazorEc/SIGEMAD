import { CommonModule } from '@angular/common';
import { Component, EventEmitter, Input, OnChanges, OnInit, Output, SimpleChanges, ViewChild, inject, signal } from '@angular/core';
import { FormBuilder, FormGroup, FormGroupDirective, ReactiveFormsModule, Validators } from '@angular/forms';
import { MatDatepickerModule } from '@angular/material/datepicker';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { MatSelectModule } from '@angular/material/select';
import { MatButtonModule } from '@angular/material/button';
import { MatIconModule } from '@angular/material/icon';
import { MatSort } from '@angular/material/sort';
import { MatTableDataSource, MatTableModule } from '@angular/material/table';
import { NgxSpinnerModule, NgxSpinnerService } from 'ngx-spinner';
import { UtilsService } from '@shared/services/utils.service';
import { FlexLayoutModule } from '@angular/flex-layout';
import { MatSnackBar } from '@angular/material/snack-bar';
import { OpeDatoAsistenciaTraduccion } from '@type/ope/datos/ope-dato-asistencia-traduccion.type';
import { TooltipDirective } from '@shared/directive/tooltip/tooltip.directive';
import { OpeDatosAsistenciasValidator } from '@shared/validators/ope/datos/ope-datos-asistencias-validator';

interface FormType {
  id?: number;
  numero: number;
  observaciones: string;
}

@Component({
  selector: 'app-ope-dato-asistencias-traducciones',
  standalone: true,
  imports: [
    CommonModule,
    ReactiveFormsModule,
    MatFormFieldModule,
    MatInputModule,
    MatSelectModule,
    MatDatepickerModule,
    MatButtonModule,
    MatTableModule,
    MatIconModule,
    NgxSpinnerModule,
    FlexLayoutModule,
    TooltipDirective,
  ],
  templateUrl: './ope-dato-asistencias-traducciones.component.html',
  styleUrl: './ope-dato-asistencias-traducciones.component.scss',
})
export class OpeDatoAsistenciasTraduccionesComponent implements OnInit, OnChanges {
  @Input() opeDatosAsistenciasTraducciones: OpeDatoAsistenciaTraduccion[] = [];
  @Output() opeDatosAsistenciasTraduccionesChange = new EventEmitter<any[]>();
  @Input() fecha!: string;

  constructor(public utilsService: UtilsService) {}

  @ViewChild(MatSort) sort!: MatSort;

  public snackBar = inject(MatSnackBar);

  private fb = inject(FormBuilder);
  formData!: FormGroup;

  public dataOpeDatosAsistenciasTraducciones = signal<FormType[]>([]);

  public isCreate = signal<number>(-1);
  public isSaving = signal<boolean>(false);

  public dataSource = new MatTableDataSource<any>([]);

  public displayedColumns: string[] = ['numero', 'observaciones', 'opciones'];

  ////
  filaSeleccionadaIndex: number | null = null;
  ///

  async ngOnInit() {
    this.formData = this.fb.group({
      numero: [
        null,
        [
          Validators.required,
          Validators.min(0),
          Validators.max(9999999),
          Validators.pattern(/^\d+$/),
          OpeDatosAsistenciasValidator.validarCampoCeroSiFechaFuturaDatosAsistencias(this.fecha, 'numero'),
          OpeDatosAsistenciasValidator.validarCampoNoCeroSiFechaNoFuturaDatosAsistencias(this.fecha, 'numero'),
        ],
      ],
      observaciones: ['', [Validators.maxLength(1000)]],
    });

    if (this.opeDatosAsistenciasTraducciones?.length > 0) {
      this.dataOpeDatosAsistenciasTraducciones.set(this.opeDatosAsistenciasTraducciones);
    }
  }

  //
  ngOnChanges(changes: SimpleChanges): void {
    if (changes['fecha']) {
      this.reConfigurarFormulario();
    }
  }

  reConfigurarFormulario(): void {
    // Guardamos los valores actuales antes de reconfigurar
    const currentValues = this.formData ? this.formData.value : {};

    // Reconfiguramos el formulario con los nuevos validadores y configuración
    this.formData = this.fb.group({
      numero: [
        currentValues.numero ?? null, // Usamos el valor actual si existe
        [
          Validators.required,
          Validators.min(0),
          Validators.max(9999999),
          Validators.pattern(/^\d+$/),
          OpeDatosAsistenciasValidator.validarCampoCeroSiFechaFuturaDatosAsistencias(this.fecha, 'numero'),
          OpeDatosAsistenciasValidator.validarCampoNoCeroSiFechaNoFuturaDatosAsistencias(this.fecha, 'numero'),
        ],
      ],
      observaciones: [currentValues.observaciones ?? '', [Validators.maxLength(1000)]],
    });

    // Después de reconfigurar, asignamos los valores anteriores para no perderlos
    this.formData.patchValue(currentValues);
  }
  //

  onSubmit(formDirective: FormGroupDirective): void {
    if (this.formData.valid) {
      const data = this.formData.value;
      if (this.isCreate() == -1) {
        this.dataOpeDatosAsistenciasTraducciones.set([data, ...this.dataOpeDatosAsistenciasTraducciones()]);
      } else {
        this.editarItem(this.isCreate());
      }

      this.opeDatosAsistenciasTraduccionesChange.emit(this.dataOpeDatosAsistenciasTraducciones());

      formDirective.resetForm();
      this.formData.reset({
        //fecha: moment().toDate(),
        //hora: moment().format('HH:mm'),
        // PCD
        //fechaHora: moment().format('YYYY-MM-DD HH:mm'),
        // FIN PCD
      });
      this.filaSeleccionadaIndex = null;
    } else {
      this.formData.markAllAsTouched();
    }
  }

  seleccionarItem(index: number) {
    this.isCreate.set(index);

    this.formData.patchValue({
      ...this.dataOpeDatosAsistenciasTraducciones()[index],
    });

    //this.formData.patchValue(this.dataOtherInformation()[index]);

    //
    this.filaSeleccionadaIndex = index;
    //
  }

  editarItem(index: number) {
    const dataEditada = this.formData.value;
    this.dataOpeDatosAsistenciasTraducciones.update((data) => {
      data[index] = { ...data[index], ...dataEditada };
      return [...data];
    });
    this.isCreate.set(-1);
    this.formData.reset();
  }

  eliminarItem(index: number) {
    this.dataOpeDatosAsistenciasTraducciones.update((data) => {
      data.splice(index, 1);
      return [...data];
    });
    this.opeDatosAsistenciasTraduccionesChange.emit(this.dataOpeDatosAsistenciasTraducciones());
  }

  //Función para guardar en base de datos
  /*
  async saveList() {
    if (this.isSaving()) {
      return;
    }
    this.isSaving.set(true);
    if (this.dataOpeDatosAsistenciasTraducciones().length <= 0) {
      this.snackBar.open('Debe introducir algún elemento en la lista!', '', {
        duration: 3000,
        horizontalPosition: 'center',
        verticalPosition: 'bottom',
        panelClass: ['snackbar-rojo'],
      });
      this.isSaving.set(false);
      return;
    }

    const arrayToSave = this.dataOpeDatosAsistenciasTraducciones().map((item) => {
      return {
        id: item.id ?? null,
        idOpeDatoAsistencia: this.opeDatoAsistencia.id,
        numero: item.numero,
        observaciones: item.observaciones,
      };
    });
    const objToSave = {
      //IdRegistroActualizacion: this.idRegistro,
      //IdSuceso: this.dataProps?.fire?.id,
      //IdOpeDatoAsistencia: this.opeDatoAsistencia.id,
      Id: this.opeDatoAsistencia.id,
      IdOpePuerto: this.opeDatoAsistencia.idOpePuerto,
      Fecha: this.opeDatoAsistencia.fecha,
      Lista: arrayToSave,
    };

    try {
      this.spinner.show();

      const resp: { idOpeDatoAsistencia: string | number } | any = await this.opeDatosAsistenciasTraduccionesService.post(objToSave);
      if (resp!.idOpeDatoAsistencia > 0) {
        this.isSaving.set(false);
        //this.closeModal({ refresh: true });
        this.spinner.hide();

        this.snackBar.open('Datos guardados correctamente!', '', {
          duration: 3000,
          horizontalPosition: 'center',
          verticalPosition: 'bottom',
          panelClass: ['snackbar-verde'],
        });
      } else {
        this.spinner.hide();
      }
    } catch (error) {
      console.info({ error });
      this.spinner.hide();
    }
  }
  */

  getForm(atributo: string): any {
    return this.formData.controls[atributo];
  }
}
