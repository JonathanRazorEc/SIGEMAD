import { CommonModule } from '@angular/common';
import { Component, OnInit, ViewChild, inject, signal } from '@angular/core';
import { FormBuilder, FormGroup, FormGroupDirective, ReactiveFormsModule, Validators } from '@angular/forms';
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
import { MatSnackBar } from '@angular/material/snack-bar';
import moment from 'moment';
import { FireOtherInformationService } from '../../services/fire-other-information.service';
import { MasterDataEvolutionsService } from '../../services/master-data-evolutions.service';
import { AlertService } from '../../shared/alert/alert.service';
import { FireDetail } from '../../types/fire-detail.type';
import { Media } from '../../types/media.type';
import { OriginDestination } from '../../types/origin-destination.type';
import { DragDropModule } from '@angular/cdk/drag-drop';
import { FechaValidator } from '../../shared/validators/fecha-validator';
import { MomentDateAdapter } from '@angular/material-moment-adapter';
import { FORMATO_FECHA } from '@type/date-formats';

interface FormType {
  id?: string;
  //fecha: Date;
  //hora: any;
  // PCD
  fechaHora: Date;
  // FIN PCD
  procendenciaDestino: { id: string; descripcion: string }[];
  medio: { id: string; descripcion: string };
  asunto: string;
  observaciones: string;
}

@Component({
  selector: 'app-fire-create',
  standalone: true,
  templateUrl: './fire-other-information.component.html',
  styleUrls: ['./fire-other-information.component.scss'],
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
    DragDropModule,
  ],
  providers: [
    { provide: DateAdapter, useClass: MomentDateAdapter },
    { provide: MAT_DATE_FORMATS, useValue: FORMATO_FECHA },
  ],
})
export class FireOtherInformationComponent implements OnInit {
  constructor(
    private masterData: MasterDataEvolutionsService,
    private dialogRef: MatDialogRef<FireOtherInformationComponent>,
    private otherInformationService: FireOtherInformationService,
    private spinner: NgxSpinnerService,
    public alertService: AlertService
  ) {}

  @ViewChild(MatSort) sort!: MatSort;

  public snackBar = inject(MatSnackBar);

  private fb = inject(FormBuilder);
  dataProps = inject(MAT_DIALOG_DATA) as {
    title: string;
    fire: any;
    fireDetail: FireDetail;
  };

  formData!: FormGroup;

  readonly sections = [{ id: 1, label: 'Otra información' }];
  selectedOption: MatChipListboxChange = { source: null as any, value: 1 };

  onSelectionChange(event: MatChipListboxChange): void {
    this.selectedOption = event;
  }

  public listadoProcedenciaDestino = signal<OriginDestination[]>([]);
  public listadoMedios = signal<Media[]>([]);
  public dataOtherInformation = signal<FormType[]>([]);
  public isCreate = signal<number>(-1);
  public isSaving = signal<boolean>(false);

  public dataSource = new MatTableDataSource<any>([]);

  public displayedColumns: string[] = ['fechaHora', 'procendenciaDestino', 'medio', 'asunto', 'opciones'];
  public idRegistro = 0;

  async ngOnInit() {
    this.formData = this.fb.group({
      //fecha: [moment().toDate(), Validators.required],
      //hora: [moment().format('HH:mm'), Validators.required],
      // PCD
      fechaHora: [moment().format('YYYY-MM-DD HH:mm'), [Validators.required, FechaValidator.validarFecha]],
      // FIN PCD
      procendenciaDestino: ['', Validators.required],
      medio: ['', Validators.required],
      asunto: ['', Validators.required],
      observaciones: ['', Validators.required],
    });

    const procedenciasDestino = await this.masterData.getOriginDestination();
    this.listadoProcedenciaDestino.set(procedenciasDestino);

    const medios = await this.masterData.getMedia();
    this.listadoMedios.set(medios);

    this.isToEditDocumentation();
  }

  async isToEditDocumentation() {
    console.log('🚀 ~ FireOtherInformationComponent ~ isToEditDocumentation ~ isToEditDocumentation:', 'isToEditDocumentation');
    try {
      let dataOtraInformacion: any;
      if (this.dataProps.fireDetail?.id) {
        dataOtraInformacion = await this.otherInformationService.getByIdRegistro(
          Number(this.dataProps.fire.idSuceso),
          Number(this.dataProps.fireDetail?.id)
        );
      } else {
        dataOtraInformacion = await this.otherInformationService.getById(Number(this.dataProps.fire.idSuceso));
      }

      console.log('🚀 ~ FireOtherInformationComponent ~ isToEditDocumentation ~ dataOtraInformacion:', dataOtraInformacion);
      this.idRegistro = Number(this.dataProps.fireDetail?.id) ?? 0;
      const newData = dataOtraInformacion?.lista?.map((otraInformacion: any) => ({
        id: otraInformacion.id,
        asunto: otraInformacion.asunto,
        fecha: moment(otraInformacion.fechaHora).format('YYYY-MM-DD'),
        hora: moment(otraInformacion.fechaHora).format('HH:mm'),
        medio: otraInformacion.medio,
        observaciones: otraInformacion.observaciones,
        procendenciaDestino: otraInformacion.procedenciasDestinos,
      }));

      this.dataOtherInformation.set(newData);
    } catch (error) {
      console.log('🚀 ~ FireOtherInformationComponent ~ isToEditDocumentation ~ error:', error);
    }
  }

  trackByFn(index: number, item: any): number {
    return item.id;
  }

  onSubmit(formDirective: FormGroupDirective): void {
    if (this.formData.valid) {
      const data = this.formData.value;
      if (this.isCreate() == -1) {
        this.dataOtherInformation.set([data, ...this.dataOtherInformation()]);
      } else {
        this.editarItem(this.isCreate());
      }

      formDirective.resetForm();
      this.formData.reset({
        //fecha: moment().toDate(),
        //hora: moment().format('HH:mm'),
        // PCD
        fechaHora: moment().format('YYYY-MM-DD HH:mm'),
        // FIN PCD
      });
    } else {
      this.formData.markAllAsTouched();
    }
  }

  //Función para guardar en base de datos
  async saveList() {
    if (this.isSaving()) {
      return;
    }
    this.isSaving.set(true);
    if (this.dataOtherInformation().length <= 0) {
      this.snackBar.open('Debe introducir algún elemento en la lista!', '', {
        duration: 3000,
        horizontalPosition: 'center',
        verticalPosition: 'bottom',
        panelClass: ['snackbar-rojo'],
      });
      this.isSaving.set(false);
      return;
    }

    const arrayToSave = this.dataOtherInformation().map((item) => {
      return {
        id: item.id ?? null,
        //fechaHora: this.getFechaHora(item.fecha, item.hora),
        // PCD
        fechaHora: moment(item.fechaHora).format('MM/DD/YY HH:mm'),
        // FIN PCD
        idMedio: item.medio?.id ?? null,
        asunto: item.asunto,
        observaciones: item.observaciones,
        idsProcedenciasDestinos: item.procendenciaDestino.map((procendenciaDestino) => procendenciaDestino.id),
      };
    });
    const objToSave = {
      IdRegistroActualizacion: this.idRegistro,
      IdSuceso: this.dataProps?.fire?.id,
      lista: arrayToSave,
    };

    try {
      this.spinner.show();

      const resp: { idOtraInformacion: string | number } | any = await this.otherInformationService.post(objToSave);
      if (resp!.idRegistroActualizacion > 0) {
        this.isSaving.set(false);
        this.closeModal({ refresh: true });
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

  async delete() {
    //const toolbar = document.querySelector('mat-toolbar');
    //this.renderer.setStyle(toolbar, 'z-index', '1');
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
          await this.otherInformationService.delete(Number(this.dataProps?.fireDetail?.id));
          this.closeModal({ refresh: true });
          this.spinner.hide();

          this.snackBar.open('Datos eliminados correctamente!', '', {
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

    const medioSelected = () => this.listadoMedios().find((medio) => medio.id === Number(this.dataOtherInformation()[index].medio.id));

    const procedenciasSelecteds = () => {
      const idsABuscar = this.dataOtherInformation()[index].procendenciaDestino.map((obj: any) => Number(obj.id));
      return this.listadoProcedenciaDestino().filter((procedencia) => {
        return idsABuscar.includes(Number(procedencia.id));
      });
    };

    this.formData.patchValue({
      ...this.dataOtherInformation()[index],
      medio: medioSelected(),
      procendenciaDestino: procedenciasSelecteds(),
    });

    //this.formData.patchValue(this.dataOtherInformation()[index]);
  }

  editarItem(index: number) {
    const dataEditada = this.formData.value;
    this.dataOtherInformation.update((data) => {
      data[index] = { ...data[index], ...dataEditada };
      return [...data];
    });
    this.isCreate.set(-1);
    this.formData.reset();
  }

  eliminarItem(index: number) {
    this.dataOtherInformation.update((data) => {
      data.splice(index, 1);
      return [...data];
    });
  }

  getFormatdate(date: any) {
    return moment(date).format('DD/MM/YY HH:mm');
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
}
