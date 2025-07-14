import { animate, state, style, transition, trigger } from '@angular/animations';
import { CommonModule } from '@angular/common';
import { Component, inject, Renderer2 } from '@angular/core';
import { NgxSpinnerModule, NgxSpinnerService } from 'ngx-spinner';
import { FlexLayoutModule } from '@angular/flex-layout';
import { MatChipListboxChange, MatChipsModule } from '@angular/material/chips';
import { MAT_DIALOG_DATA, MatDialogRef } from '@angular/material/dialog';
import { FireDetail } from '../../types/fire-detail.type';
import { EmergencyNationalComponent } from './emergency-national/emergency-national.component';
import { ZagepComponent } from './zagep/zagep.component';
import { CecodComponent } from './cecod/cecod.component';
import { NotificationsComponent } from './notifications/notifications.component';
import { MobilizationComponent } from './mobilization/mobilization.component';
import { ActivationPlanComponent } from './activation-plan/activation-plan.component';
import { SystemsActivationComponent } from './systems-activation/systems-activation.component';
import { ActionsRelevantService } from '../../services/actions-relevant.service';
import { AlertService } from '../../shared/alert/alert.service';
import { _isNumberValue } from '@angular/cdk/coercion';
import moment from 'moment';
import { ActuacionRelevante, Movilizacion } from '../../types/mobilization.type';
import { DragDropModule } from '@angular/cdk/drag-drop';
import { MatSnackBar } from '@angular/material/snack-bar';

@Component({
  selector: 'app-fire-actions-relevant',
  standalone: true,
  imports: [
    NgxSpinnerModule,
    FlexLayoutModule,
    MatChipsModule,
    CommonModule,
    EmergencyNationalComponent,
    ZagepComponent,
    CecodComponent,
    NotificationsComponent,
    ActivationPlanComponent,
    SystemsActivationComponent,
    MobilizationComponent,
    DragDropModule,
  ],
  animations: [
    trigger('fadeInOut', [
      state('void', style({ opacity: 0, transform: 'translateY(20px)' })),
      transition(':enter', [animate('900ms ease-out')]),
      transition(':leave', [animate('0ms ease-in')]),
    ]),
  ],
  templateUrl: './fire-actions-relevant.component.html',
  styleUrl: './fire-actions-relevant.component.scss',
})
export class FireActionsRelevantComponent {
  private dialogRef = inject(MatDialogRef<FireActionsRelevantComponent>);
  private spinner = inject(NgxSpinnerService);
  public renderer = inject(Renderer2);
  public actionsRelevantSevice = inject(ActionsRelevantService);
  public alertService = inject(AlertService);

  // PCD
  public snackBar = inject(MatSnackBar);
  // FIN PCD

  selectedOption: MatChipListboxChange = { source: null as any, value: 1 };

  data = inject(MAT_DIALOG_DATA) as {
    title: string;
    idIncendio: number;
    fireDetail?: FireDetail;
    valoresDefecto?: number;
    fire?: any;
  };

  editData: any;
  isDataReady = false;
  idReturn = 0;
  isEdit = false;
  estado: number | undefined;

  readonly sections = [
    { id: 1, label: 'Movilización de medios' },
    { id: 2, label: 'Convocatoria CECOD' },
    { id: 3, label: 'Activación de planes' },
    { id: 4, label: 'Notificaciones oficiales' },
    { id: 5, label: 'Activación de sistemas' },
    { id: 6, label: 'Declaración ZAGEP' },
    { id: 7, label: 'Emergencia nacional' },
  ];

  dataMaestros: any = {};

  async ngOnInit() {
    console.log('🚀 ~ FireCreateComponent ~ ngOnInit ~ this.data.fire:', this.data.fire);
    this.spinner.show();
    await this.loadData();
    this.isToEdit();
  }

  async loadData() {
    const tipoNotificaciones = await this.actionsRelevantSevice.getTipoNotificacion();
    const tipoPlanes = await this.actionsRelevantSevice.getAllPlanes();
    const tiposActivacion = await this.actionsRelevantSevice.getTipoActivacion();
    const modosActivacion = await this.actionsRelevantSevice.getModosActivacion();
    const tiposGestion = await this.actionsRelevantSevice.getTipoGestion();
    const procedencia = await this.actionsRelevantSevice.getProcedencia();
    const destinos = await this.actionsRelevantSevice.getDestinos();
    const capacidades = await this.actionsRelevantSevice.getCapacidades();
    const tipoAdmin = await this.actionsRelevantSevice.getTipoAdministracion();
    console.log('🚀 ~ FireActionsRelevantComponent ~ loadData ~ tipoAdmin:', tipoAdmin);

    this.dataMaestros = {
      tipoNotificaciones,
      tipoPlanes,
      tiposActivacion,
      modosActivacion,
      tiposGestion,
      procedencia,
      destinos,
      capacidades,
      tipoAdmin,
    };

    return this.dataMaestros;
  }

  async isToEdit() {
    let dataCordinacion: any;

    try {
      console.log('🚀 ~ FireActionsRelevantComponent ~ isToEdit ~ this.data.fireDetail:', this.data.fireDetail);
      if (this.data.fireDetail?.id) {
        dataCordinacion = await this.actionsRelevantSevice.getByIdRegistro(Number(this.data.idIncendio), Number(this.data?.fireDetail?.id));
      } else {
        dataCordinacion = await this.actionsRelevantSevice.getById(Number(this.data.idIncendio));
      }
      this.editData = dataCordinacion;
    } catch (error) {
      console.log('🚀 ~ FireActionsRelevantComponent ~ isToEdit ~ error:', error);
    }

    this.isDataReady = true;
  }

  async onSaveFromChild(value: { save: boolean; delete: boolean; close: boolean; update: boolean }) {
    const keyWithTrue = (Object.keys(value) as Array<keyof typeof value>).find((key) => value[key]);
    this.isEdit = false;

    if (keyWithTrue) {
      switch (keyWithTrue) {
        case 'save':
          await this.save();
          break;
        case 'delete':
          await this.delete();
          break;
        case 'close':
          this.spinner.hide();
          this.actionsRelevantSevice.clearData();
          this.closeModal(true);
          break;
        case 'update':
          this.isEdit = true;
          await this.save();
          break;
        default:
          console.error('Clave inesperada');
      }
    } else {
      console.log('Ninguna clave tiene valor true');
    }
  }

  async save() {
    this.spinner.show();
    const toolbar = document.querySelector('mat-toolbar');
    this.renderer.setStyle(toolbar, 'z-index', '1');
    await this.processData();

    this.actionsRelevantSevice.clearData();

    this.renderer.setStyle(toolbar, 'z-index', '5');
    this.isDataReady = false;
    const dataActuaciones: any = await this.actionsRelevantSevice.getByIdRegistro(this.data.idIncendio, Number(this.idReturn));
    this.editData = dataActuaciones;
    this.isDataReady = true;
    this.spinner.hide();

    this.snackBar.open('Datos guardados correctamente!', '', {
      duration: 3000,
      horizontalPosition: 'center',
      verticalPosition: 'bottom',
      panelClass: ['snackbar-verde'],
    });
  }

  async processData(): Promise<void> {
    if (this.actionsRelevantSevice.dataMovilizacion().length > 0 && 
      this.actionsRelevantSevice.dataMovilizacion()[0]?.Movilizaciones.length > 0)
    {
      const formData = new FormData();
      formData.append('data', JSON.stringify(this.actionsRelevantSevice.dataMovilizacion()[0]));
      const resp: { idRegistroActualizacion: string | number } | any = await this.actionsRelevantSevice.postMovilizaciones(formData);
      this.idReturn = resp.idRegistroActualizacion;
    }

    if (this.actionsRelevantSevice.dataEmergencia().length > 0) {
      // this.editData ? (this.idReturn = this.editData.id) : 0;
      this.data?.fireDetail?.id ? (this.idReturn = Number(this.data?.fireDetail?.id)) : 0;
      this.idReturn ? (this.actionsRelevantSevice.dataEmergencia()[0].idRegistroActualizacion = this.idReturn) : 0;
      const result: any = await this.actionsRelevantSevice.postData(this.actionsRelevantSevice.dataEmergencia()[0]);
      this.idReturn = result.idRegistroActualizacion;
    }

    if (this.actionsRelevantSevice.dataPlanes().length > 0) {
      console.log(
        '🚀 ~ FireActionsRelevantComponent ~ processData ~ this.actionsRelevantSevice.dataPlanes():',
        this.actionsRelevantSevice.dataPlanes()
      );

      const arrayToSave = this.actionsRelevantSevice.dataPlanes().map((item, index) => {
        return {
          id: item.id ?? null,
          idTipoPlan: _isNumberValue(item.idTipoPlan) ? item.idTipoPlan : item.idTipoPlan?.id,
          nombrePlan: item.nombrePlan,
          nombrePlanPersonalizado: item.nombrePlanPersonalizado,
          fechaInicio: this.formatDate(item.fechaInicio),
          fechaFin: item.fechaFin ? this.formatDate(item.fechaFin) : null,
          autoridad: item.autoridad,
          observaciones: item.observaciones,
          archivo: item.file,
        };
      });

      const objToSave = {
        detallesDocumentaciones: arrayToSave,
      };

      const formData = new FormData();

      this.data?.fireDetail?.id ? (this.idReturn = Number(this.data?.fireDetail?.id)) : 0;
      this.actionsRelevantSevice.dataPlanes()[0].idRegistroActualizacion = this.idReturn !== 0 ? (this.actionsRelevantSevice.dataPlanes()[0].idRegistroActualizacion = this.idReturn) : 0;
    
      formData.append('idRegistroActualizacion', this.actionsRelevantSevice.dataPlanes()[0].idRegistroActualizacion);
      formData.append('idSuceso', this.data.idIncendio.toString());

      objToSave.detallesDocumentaciones.forEach((detalle, index) => {
        formData.append(`ActivacionPlanes[${index}].Id`, (detalle.id ?? '0').toString());
        formData.append(`ActivacionPlanes[${index}].IdTipoPlan`, detalle.idTipoPlan.toString());
        formData.append(`ActivacionPlanes[${index}].FechaInicio`, detalle.fechaInicio);
        formData.append(`ActivacionPlanes[${index}].FechaFin`, detalle.fechaFin ?? '');
        formData.append(`ActivacionPlanes[${index}].Autoridad`, detalle.autoridad ?? '');
        formData.append(`ActivacionPlanes[${index}].TipoPlanPersonalizado`, detalle.nombrePlanPersonalizado ?? '');
        formData.append(`ActivacionPlanes[${index}].IdPlanEmergencia`, '3');
        formData.append(`ActivacionPlanes[${index}].PlanEmergenciaPersonalizado`, detalle.nombrePlan ?? '');
        formData.append(`ActivacionPlanes[${index}].Observaciones`, detalle.observaciones ?? '');
        if (!detalle.archivo?.id) {
          formData.append(`ActivacionPlanes[${index}].Archivo`, detalle.archivo);
        }
      });

      const resp: { idRegistroActualizacion: string | number } | any = await this.actionsRelevantSevice.postPlanes(formData);
      console.log('🚀 ~ FireActionsRelevantComponent ~ processData ~ resp:', resp);

      this.idReturn = resp.idRegistroActualizacion;
    }

    if (this.actionsRelevantSevice.dataCecod().length > 0) {
      await this.handleDataProcessing(
        this.actionsRelevantSevice.dataCecod(),
        (item) => ({
          id: item.id ?? 0,
          fechaInicio: this.formatDate(item.fechaInicio),
          fechaFin: item.fechaFin ? this.formatDate(item.fechaFin) : null,
          lugar: item.lugar,
          convocados: item.convocados,
          participantes: item.participantes,
          observaciones: item.observaciones,
        }),
        this.actionsRelevantSevice.postDataCecod.bind(this.actionsRelevantSevice),
        'detalles'
      );
    }

    if (this.actionsRelevantSevice.dataNotificaciones().length > 0) {
      await this.handleDataProcessing(
        this.actionsRelevantSevice.dataNotificaciones(),
        (item) => ({
          id: item.id ?? 0,
          idTipoNotificacion: _isNumberValue(item.idTipoNotificacion) ? item.idTipoNotificacion : item.idTipoNotificacion.id,
          fechaHoraNotificacion: this.formatDate(item.fechaHoraNotificacion),
          organosNotificados: item.organosNotificados,
          ucpm: item.ucpm,
          organismoInternacional: item.organismoInternacional,
          otrosPaises: item.otrosPaises,
          observaciones: item.observaciones,
        }),
        this.actionsRelevantSevice.postDataNotificaciones.bind(this.actionsRelevantSevice),
        'detalles'
      );
    }

    if (this.actionsRelevantSevice.dataSistemas().length > 0) {
      await this.handleDataProcessing(
        this.actionsRelevantSevice.dataSistemas(),
        (item) => ({
          id: item.id ?? 0,
          idTipoSistemaEmergencia: _isNumberValue(item.idTipoSistemaEmergencia) ? item.idTipoSistemaEmergencia : item.idTipoSistemaEmergencia.id,
          fechaHoraActivacion: item.fechaHoraActivacion ? this.formatDate(item.fechaHoraActivacion) : null,
          fechaHoraActualizacion: item.fechaHoraActualizacion ? this.formatDate(item.fechaHoraActualizacion) : null,
          autoridad: item.autoridad,
          descripcionSolicitud: item.descripcionSolicitud,
          observaciones: item.observaciones,
          idModoActivacion: _isNumberValue(item.idModoActivacion) ? item.idModoActivacion : (item.idModoActivacion?.id ?? null),
          fechaActivacion: item.fechaActivacion ? this.formatDate(item.fechaActivacion) : null,
          codigo: item.codigo,
          nombre: item.nombre,
          urlAcceso: item.urlAcceso,
          fechaHoraPeticion: item.fechaHoraPeticion ? this.formatDate(item.fechaHoraPeticion) : null,
          fechaAceptacion: item.fechaAceptacion ? this.formatDate(item.fechaAceptacion) : null,
          peticiones: item.peticiones,
          mediosCapacidades: item.mediosCapacidades,
        }),
        this.actionsRelevantSevice.postSistemas.bind(this.actionsRelevantSevice),
        'detalles'
      );
    }

    if (this.actionsRelevantSevice.dataZagep().length > 0) {
      await this.handleDataProcessing(
        this.actionsRelevantSevice.dataZagep(),
        (item) => ({
          id: item.id ?? 0,
          fechaSolicitud: this.formatDate(item.fechaSolicitud),
          denominacion: item.denominacion,
          observaciones: item.observaciones,
        }),
        this.actionsRelevantSevice.postDataZagep.bind(this.actionsRelevantSevice),
        'detalles'
      );
    }
  }

  async handleDataProcessing<T>(data: T[], formatter: (item: T) => any, postService: (body: any) => Promise<any>, key: string): Promise<void> {
    if (data.length > 0) {
      const formattedData = data.map(formatter);

      const body = {
        idSuceso: this.data.idIncendio,
        idRegistroActualizacion: this.data?.fireDetail?.id ? this.data?.fireDetail?.id : this.idReturn,
        [key]: formattedData,
      };

      const result = await postService(body);
      console.log('🚀 ~ result:', result);
      this.idReturn = result.idRegistroActualizacion;
    }
  }

  formatDate(date: Date | string): string {
    const d = new Date(date);
    const year = d.getFullYear();
    const month = (d.getMonth() + 1).toString().padStart(2, '0');
    const day = d.getDate().toString().padStart(2, '0');
    return `${year}-${month}-${day}`;
  }

  getFechaHora(fecha: Date, hora: string, format: string = 'MM/DD/YY HH:mm'): any {
    if (hora && fecha) {
      const [horas, minutos] = hora.split(':').map(Number);
      const fechaHora = new Date(fecha);
      fechaHora.setHours(horas, minutos, 0, 0);
      return moment(fechaHora).format(format);
    }
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

  async delete() {
    const toolbar = document.querySelector('mat-toolbar');
    this.renderer.setStyle(toolbar, 'z-index', '1');
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

      .then(async (result) => {
        if (result.isConfirmed) {
          try {
            await this.actionsRelevantSevice.deleteActions(Number(this.data?.fireDetail?.id));
            this.actionsRelevantSevice.clearData();
            this.closeModal(true);
            this.spinner.hide();

            this.snackBar
              .open('Datos eliminados correctamente!', '', {
                duration: 3000,
                horizontalPosition: 'center',
                verticalPosition: 'bottom',
                panelClass: ['snackbar-verde'],
              });
          } catch (error) {
            this.closeModal();
            this.spinner.hide();
            this.snackBar
              .open('No hemos podido eliminar la evolución!', '', {
                duration: 3000,
                horizontalPosition: 'center',
                verticalPosition: 'bottom',
                panelClass: ['snackbar-rojo'],
              });
          }
        } else {
          this.spinner.hide();
        }
      });
  }

  onSelectionChange(event: MatChipListboxChange): void {
    this.selectedOption = event;
  }

  trackByFn(index: number, item: any): number {
    return item.id;
  }

  closeModal(value?: any) {
    this.dialogRef.close(value);
  }
}
