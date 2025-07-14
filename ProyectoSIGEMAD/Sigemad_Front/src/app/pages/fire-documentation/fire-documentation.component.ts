import { CommonModule } from '@angular/common';
import { Component, OnInit, ViewChild, inject, signal } from '@angular/core';
import { FormBuilder, FormGroup, FormGroupDirective, ReactiveFormsModule, Validators } from '@angular/forms';
import { MatButtonModule } from '@angular/material/button';
import { MatChipListboxChange, MatChipsModule } from '@angular/material/chips';
import { MatDatepickerModule } from '@angular/material/datepicker';
import { MAT_DIALOG_DATA, MatDialogRef } from '@angular/material/dialog';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { MatSelectModule } from '@angular/material/select';
import { MatSort } from '@angular/material/sort';
import { MatTableDataSource, MatTableModule } from '@angular/material/table';
import { DateAdapter, MAT_DATE_FORMATS, MatNativeDateModule, NativeDateAdapter } from '@angular/material/core';
import { MatIconModule } from '@angular/material/icon';
import moment from 'moment';
import { FileSystemDirectoryEntry, FileSystemFileEntry, NgxFileDropEntry, NgxFileDropModule } from 'ngx-file-drop';
import { NgxSpinnerModule, NgxSpinnerService } from 'ngx-spinner';
import { FireDocumentationService } from '../../services/fire-documentation.service';
import { MasterDataEvolutionsService } from '../../services/master-data-evolutions.service';
import { TipoDocumentoService } from '../../services/tipo-documento.service';
import { AlertService } from '../../shared/alert/alert.service';
import { FireDetail } from '../../types/fire-detail.type';
import { Media } from '../../types/media.type';
import { OriginDestination } from '../../types/origin-destination.type';
import { DragDropModule } from '@angular/cdk/drag-drop';
import { MatSnackBar } from '@angular/material/snack-bar';
import { MatSnackBarModule } from '@angular/material/snack-bar';
import { FechaValidator } from '../../shared/validators/fecha-validator';

const MY_DATE_FORMATS = {
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

interface FormType {
  id?: string;
  idDocumento?: string;
  fecha: Date;
  hora: any;
  procendenciaDestino: any;
  fechaSolicitud: Date;
  horaSolicitud: any;
  tipoDocumento: { id: string; descripcion: string };
  descripcion: string;
  file?: any;
}

@Component({
  selector: 'app-fire-create',
  standalone: true,
  templateUrl: './fire-documentation.component.html',
  styleUrls: ['./fire-documentation.component.scss'],
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
    NgxSpinnerModule,
    NgxFileDropModule,
    MatIconModule,
    DragDropModule,
  ],
  providers: [
    { provide: DateAdapter, useClass: NativeDateAdapter },
    { provide: MAT_DATE_FORMATS, useValue: MY_DATE_FORMATS },
  ],
})
export class FireDocumentation implements OnInit {
  constructor(
    private masterData: MasterDataEvolutionsService,
    private tipoDocumento: TipoDocumentoService,
    private dialogRef: MatDialogRef<FireDocumentation>,
    private fireDocumentationService: FireDocumentationService,
    private spinner: NgxSpinnerService,
    public alertService: AlertService
  ) {}

  @ViewChild(MatSort) sort!: MatSort;

  file: File | null = null;
  public files: NgxFileDropEntry[] = [];
  fileFlag: boolean = false;

  private fb = inject(FormBuilder);
  dataProps = inject(MAT_DIALOG_DATA) as {
    title: string;
    fire: any;
    fireDetail: FireDetail;
  };

  formData!: FormGroup;

  readonly sections = [{ id: 1, label: 'Documentación' }];

  selectedOption: MatChipListboxChange = { source: null as any, value: 1 };

  onSelectionChange(event: MatChipListboxChange): void {
    this.selectedOption = event;
  }

  public listadoProcedenciaDestino = signal<OriginDestination[]>([]);
  public listadoTipoDocumento = signal<OriginDestination[]>([]);
  public listadoMedios = signal<Media[]>([]);
  public dataOtherInformation = signal<FormType[]>([]);
  public isCreate = signal<number>(-1);
  public isSaving = signal<boolean>(false);

  public dataSource = new MatTableDataSource<any>([]);

  public displayedColumns: string[] = ['fechaHora', 'procendenciaDestino', 'descripcion', 'fichero', 'opciones'];

  // PCD
  public snackBar = inject(MatSnackBar);
  // FIN PCD

  async ngOnInit() {
    this.spinner.show();
    // this.formData = this.fb.group({
    //   //fecha: [moment().toDate(), Validators.required],
    //   //hora: [moment().format('HH:mm'), Validators.required],
    //   // PCD
    //   fechaHora: [moment().format('YYYY-MM-DD HH:mm'), [Validators.required, FechaValidator.validarFecha]],
    //   // FIN PCD
    //   fechaSolicitud: [''],
    //   horaSolicitud: [''],
    //   tipoDocumento: ['', Validators.required],
    //   procendenciaDestino: ['', Validators.required],
    //   descripcion: ['', Validators.required],
    //   file: [null, Validators.required],
    // });

    this.formData = this.fb.group({
      fecha: [moment().toDate(), Validators.required],
      hora: [moment().format('HH:mm'), Validators.required],
      fechaSolicitud: [''],
      horaSolicitud: [''],
      tipoDocumento: ['', Validators.required],
      procendenciaDestino: ['', Validators.required],
      descripcion: ['', Validators.required],
      file: [null, Validators.required],
    });

    this.dataSource.data = [];

    const procedenciasDestino = await this.masterData.getOriginDestination();
    this.listadoProcedenciaDestino.set(procedenciasDestino);

    const tipoDocumentos = await this.tipoDocumento.get();
    this.listadoTipoDocumento.set(tipoDocumentos);

    this.isToEditDocumentation();
  }

  async isToEditDocumentation() {

      try {
        let dataDocumentacion: any;
        if (this.dataProps.fireDetail?.id) {
          dataDocumentacion = await this.fireDocumentationService.getByIdRegistro(
            Number(this.dataProps.fire.idSuceso),
            Number(this.dataProps.fireDetail?.id)
          );
        } else {
          dataDocumentacion = await this.fireDocumentationService.getById(Number(this.dataProps.fire.idSuceso));
        }


        const newData = dataDocumentacion?.detalles?.map((documento: any) => {
          const fecha = moment(documento.fechaHora, 'YYYY-MM-DDTHH:mm:ss').toDate();
          const hora = moment(documento.fechaHora).format('HH:mm');
          documento.archivo.name = documento.archivo.nombreOriginal;
          return {
            id: documento.id,
            descripcion: documento.descripcion,
            idSuceso: dataDocumentacion.idSuceso,
            idDocumento: dataDocumentacion.id,
            fecha,
            hora,
            fechaSolicitud: moment(documento.fechaHoraSolicitud).format('YYYY-MM-DD'),
            horaSolicitud: moment(documento.fechaHoraSolicitud).format('HH:mm'),
            procendenciaDestino: documento.procedenciaDestinos,
            tipoDocumento: documento.tipoDocumento,
            archivo: documento.archivo,
            file: documento.archivo,
          };
        });

        console.log('🚀 ~ FireDocumentation ~ newData ~ newData:', newData);
        this.dataOtherInformation.set(newData);
      } catch (error) {
        console.log('🚀 ~ FireDocumentation ~ isToEditDocumentation ~ error:', error);
      }
    

    this.spinner.hide();
  }

  trackByFn(index: number, item: any): string {
    return item;
  }

  onSubmit(formDirective: FormGroupDirective): void {
    if (this.formData.valid) {
      const formValue = this.formData.value;

      const data = {
        ...formValue,
        file: formValue.file,
      };

      if (this.isCreate() == -1) {
        this.dataOtherInformation.set([data, ...this.dataOtherInformation()]);
      } else {
        this.editarItem(this.isCreate());
      }

      formDirective.resetForm();
      this.formData.reset({
        fecha: moment().toDate(),
        hora: moment().format('HH:mm'),
        procendenciaDestino: [],
        tipoDocumento: null,
        file: null,
      });
      this.fileFlag = false;
    } else {
      this.formData.markAllAsTouched();
    }
    console.log('🚀 ~ FireDocumentation ~ onSubmit ~ this.dataOtherInformation():', this.dataOtherInformation());
  }

  async saveList() {
    if (this.isSaving()) {
      return;
    }
    this.isSaving.set(true);
    if (this.dataOtherInformation().length <= 0) {
      //this.showToast({ title: 'Debe meter data en la lista' });
      this.isSaving.set(false);
      return;
    }
    console.log('🚀 ~ FireDocumentation ~ arrayToSave ~ this.dataOtherInformation():', this.dataOtherInformation());
    const arrayToSave = this.dataOtherInformation().map((item, index) => {
      return {
        id: item.id ?? null,
        idDocumento: item.idDocumento ?? null,
        fechaHora: this.getFechaHora(item.fecha, item.hora),
        fechaHoraSolicitud: this.getFechaHora(item.fechaSolicitud, item.horaSolicitud),
        idTipoDocumento: item.tipoDocumento?.id,
        descripcion: item.descripcion,
        archivo: item.file,
        documentacionProcedenciasDestinos:
          item.procendenciaDestino.length > 0 ? item.procendenciaDestino.map((procendenciaDestino: any) => procendenciaDestino.id) : '',
      };
    });

    const objToSave = {
      detallesDocumentaciones: arrayToSave,
    };

    const formData = new FormData();

    formData.append('idSuceso', this.dataProps.fire?.idSuceso.toString());
    objToSave.detallesDocumentaciones.forEach((detalle, index) => {
      formData.append('idDocumento', detalle.idDocumento ?? '0');
      formData.append(`detalles[${index}].fechaHora`, this.getFechaHoraIso(detalle.fechaHora));
      formData.append(`detalles[${index}].fechaHoraSolicitud`, this.getFechaHoraIso(detalle.fechaHora));
      formData.append(`detalles[${index}].idTipoDocumento`, detalle.idTipoDocumento ?? '');
      formData.append(`detalles[${index}].descripcion`, detalle.descripcion ?? '');

      if (detalle.documentacionProcedenciasDestinos.length > 0) {
        detalle.documentacionProcedenciasDestinos.forEach((id: string | Blob, subIndex: any) => {
          formData.append(`detalles[${index}].idsProcedenciasDestinos[${subIndex}]`, id);
        });
      } else {
        formData.append(`detalles[${index}].idsProcedenciasDestinos`, '');
      }
      if (detalle.id) {
        formData.append(`detalles[${index}].id`, detalle.id);
      }
      formData.append(`detalles[${index}].archivo`, detalle.archivo);
    });

    try {
      this.spinner.show();
      const resp: { idOtraInformacion: string | number } | any = await this.fireDocumentationService.post(formData);

      if (resp!.idDocumentacion > 0) {
        this.isSaving.set(false);
        this.closeModal({ refresh: true });
        this.spinner.hide();

        this.snackBar
          .open('Datos modificados correctamente!', '', {
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
    this.spinner.show();

    this.alertService
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
          await this.fireDocumentationService.delete(Number(this.dataProps?.fireDetail?.id));
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

    const documentoSelected = () =>
      this.listadoTipoDocumento().find((documento) => documento.id === Number(this.dataOtherInformation()[index].tipoDocumento.id));

    const procedenciasSelecteds = () => {
      const idsABuscar = this.dataOtherInformation()[index].procendenciaDestino.map((obj: any) => Number(obj.id));
      return this.listadoProcedenciaDestino().filter((procedencia) => {
        return idsABuscar.includes(Number(procedencia.id));
      });
    };

    this.formData.patchValue({
      ...this.dataOtherInformation()[index],
      tipoDocumento: documentoSelected(),
      procendenciaDestino: procedenciasSelecteds(),
    });

    // this.dataOtherInformation.set([data, ...this.dataOtherInformation()]);
  }

  editarItem(index: number) {
    const dataEditada = this.formData.value;
    this.dataOtherInformation.update((data) => {
      data[index] = { ...data[index], ...dataEditada };
      return [...data];
    });
    this.isCreate.set(-1);
    this.formData.reset({
      procendenciaDestino: [],
      tipoDocumento: null,
    });
    this.file = null;
  }

  eliminarItem(index: number) {
    this.dataOtherInformation.update((data) => {
      data.splice(index, 1);
      return [...data];
    });
  }

  closeModal(params?: any) {
    this.dialogRef.close(params);
  }

  public dropped(files: NgxFileDropEntry[]) {
    for (const droppedFile of files) {
      if (droppedFile.fileEntry.isFile) {
        const fileEntry = droppedFile.fileEntry as FileSystemFileEntry;
        fileEntry.file((file: File) => {
          this.file = file;
          this.fileFlag = true;

          this.formData.patchValue({ file });
        });
      } else {
        const fileEntry = droppedFile.fileEntry as FileSystemDirectoryEntry;
        console.log(droppedFile.relativePath, fileEntry);
      }
    }
  }

  public fileOver(event: any) {
    console.log(event);
  }

  public fileLeave(event: any) {
    console.log(event);
  }

  getDescripcionProcedenciaDestion(procedenciaDestino: any[]) {
    if (procedenciaDestino.length === 0) {
      return 'Sin información selecionada';
    } else {
      return procedenciaDestino.map((obj) => obj.descripcion).join(', ');
    }
  }

  getFechaHora(fecha: Date, hora: string, format: string = 'MM/DD/YY HH:mm'): any {
    if (hora && fecha) {
      const [horas, minutos] = hora.split(':').map(Number);
      const fechaHora = new Date(fecha);
      fechaHora.setHours(horas, minutos, 0, 0);

      return moment(fechaHora).format(format);
    }

    //return fechaHora.toISOString();
  }

  getFechaHoraIso(fechaHora: string): any {
    if (fechaHora) {
      const [fecha, hora] = fechaHora.split(' ');
      const [mes, dia, anio] = fecha.split('/');
      const anioCompleto = `20${anio}`;
      const dateTime = new Date(`${anioCompleto}-${mes}-${dia}T${hora}:00.000Z`);

      return dateTime.toISOString();
    }
  }

  trackByIdDocumento(index: number, opcion: any): number {
    return opcion.id;
  }

  async onFileNameClick(data: any) {
    try {
      const blob = await this.fireDocumentationService.getFile(data.id);

      // Crear una URL para el Blob
      const url = window.URL.createObjectURL(blob);

      // Crear un enlace temporal para la descarga
      const a = document.createElement('a');
      a.href = url;
      a.download = data.nombreOriginal; // Nombre del archivo original
      document.body.appendChild(a);
      a.click();

      // Limpia el objeto URL después de la descarga
      window.URL.revokeObjectURL(url);
      document.body.removeChild(a);
    } catch (error) {
      console.error('Error al descargar el archivo:', error);
    }
  }
}
