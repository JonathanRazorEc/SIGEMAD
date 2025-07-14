import { CommonModule } from '@angular/common';
import { Component, EventEmitter, inject, Input, Output, signal, ViewChild } from '@angular/core';
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
import Feature from 'ol/Feature';
import { Geometry } from 'ol/geom';
import { CoordinationAddressService } from '../../../services/coordination-address.service';
import { MunicipalityService } from '../../../services/municipality.service';
import { ProvinceService } from '../../../services/province.service';
import { MapCreateComponent } from '../../../shared/mapCreate/map-create.component';
import { CoordinationAddress } from '../../../types/coordination-address';
import { Municipality } from '../../../types/municipality.type';
import { Province } from '../../../types/province.type';
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
  selector: 'app-pma',
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
  templateUrl: './pma.component.html',
  styleUrl: './pma.component.scss',
  providers: [
    { provide: DateAdapter, useClass: NativeDateAdapter },
    { provide: MAT_DATE_FORMATS, useValue: FORMATO_FECHA },
  ],
})
export class PmaComponent {
  @ViewChild(MatSort) sort!: MatSort;
  data = inject(MAT_DIALOG_DATA) as { title: string; idIncendio: number };
  @Output() save = new EventEmitter<SavePayloadModal>();
  @Input() editData: any;
  @Input() esUltimo: boolean | undefined;
  @Input() dataMaestros: any;
  @Input() fire: any;

  public polygon = signal<any>([]);
  public coordinationServices = inject(CoordinationAddressService);
  public toast = inject(MatSnackBar);
  private fb = inject(FormBuilder);
  public matDialog = inject(MatDialog);
  private spinner = inject(NgxSpinnerService);
  private provinceService = inject(ProvinceService);
  private municipalityService = inject(MunicipalityService);

  public displayedColumns: string[] = ['fechaHora', 'procendenciaDestino', 'descripcion', 'fichero', 'opciones'];

  formData!: FormGroup;

  public coordinationAddress = signal<CoordinationAddress[]>([]);
  public isCreate = signal<number>(-1);
  public provinces = signal<Province[]>([]);
  public municipalities = signal<Municipality[]>([]);

  public dataSource = new MatTableDataSource<any>([]);

  async ngOnInit() {
    this.provinces.set(this.dataMaestros.provinces);

    this.formData = this.fb.group({
      provincia: ['', Validators.required],
      municipio: ['', Validators.required],
      fechaInicio: [new Date(), Validators.required],
      fechaFin: [''],
      lugar: ['', Validators.required],
      observaciones: [''],
    });

    const defaultProvincia = this.provinces().find((provincia) => provincia.id === this.fire.idProvincia);
    console.log('🚀 ~ CecopiComponent ~ ngOnInit ~ this.fire:', this.fire);
    this.formData.get('provincia')?.setValue(defaultProvincia);

    const municipalities = await this.municipalityService.get(this.fire.idProvincia);
    this.municipalities.set(municipalities);
    const defaultMuni = this.municipalities().find((muni) => muni.id === this.fire.idMunicipio);
    this.formData.get('municipio')?.setValue(defaultMuni);

    if (this.editData) {
      console.log('Información recibida en el hijo:', this.editData);
      if (this.coordinationServices.dataPma().length === 0) {
        this.coordinationServices.dataPma.set(this.editData);
        this.polygon.set(this.editData.geoPosicion?.coordinates[0]);
      }
    }
    this.spinner.hide();
  }

  onSubmit(formDirective: FormGroupDirective): void {
    if (this.formData.valid) {
      const data = this.formData.value;
      if (this.isCreate() == -1) {
        data.geoPosicion = {
          type: 'Polygon',
          coordinates: [this.polygon()],
        };
        this.coordinationServices.dataPma.set([data, ...this.coordinationServices.dataPma()]);
      } else {
        this.editarItem(this.isCreate());
      }

      formDirective.resetForm();
      this.formData.reset({
        fechaInicio: new Date(),
        fechaFin: null,
      });
      this.formData.get('municipio')?.disable();
    } else {
      this.formData.markAllAsTouched();
    }
  }

  async sendDataToEndpoint() {
    if (this.coordinationServices.dataPma().length > 0 && !this.editData) {
      this.save.emit({ save: true, delete: false, close: false, update: false });
    } else {
      if (this.editData) {
        this.save.emit({ save: false, delete: false, close: false, update: true });
      }
    }
  }

  formatDate(date: Date | string): string {
    const d = new Date(date);
    const year = d.getFullYear();
    const month = (d.getMonth() + 1).toString().padStart(2, '0');
    const day = d.getDate().toString().padStart(2, '0');
    return `${year}-${month}-${day}`;
  }

  async loadMunicipalities(event: any) {
    const province_id = event?.value?.id ?? event.id;
    const municipalities = await this.municipalityService.get(province_id);
    this.municipalities.set(municipalities);
    this.formData.get('municipio')?.enable();
  }
  onChangeMunicipio(event: any) {
    this.polygon.set([]);
  }

  openModalMap() {
    if (!this.formData.value.municipio) {
      return;
    }
    const municipioSelected = this.municipalities().find((item) => item.id == this.formData.value.municipio.id);

    if (!municipioSelected) {
      return;
    }

    const dialogRef = this.matDialog.open(MapCreateComponent, {
      width: '780px',
      maxWidth: '780px',
      disableClose: true,
      //height: '780px',
      //maxHeight: '780px',
      data: {
        municipio: municipioSelected,
        listaMunicipios: this.municipalities(),
        defaultPolygon: this.polygon(),
      },
    });

    dialogRef.componentInstance.save.subscribe((features: Feature<Geometry>[]) => {
      this.polygon.set(features);
    });
  }

  editarItem(index: number) {
    const dataEditada = this.formData.value;
    this.coordinationServices.dataPma.update((data) => {
      data[index] = { ...data[index], ...dataEditada };
      return [...data];
    });
    this.isCreate.set(-1);
    this.formData.reset();
  }

  eliminarItem(index: number) {
    this.coordinationServices.dataPma.update((data) => {
      data.splice(index, 1);
      return [...data];
    });
  }

  async seleccionarItem(index: number) {
    this.isCreate.set(index);
    const selectedItem = this.coordinationServices.dataPma()[index];

    const provinciaSeleccionada = () =>
      this.provinces().find((provincia) => provincia.id === Number(this.coordinationServices.dataPma()[index].provincia.id));

    await this.loadMunicipalities(provinciaSeleccionada());

    const municipioSeleccionado = () =>
      this.municipalities().find((municipio) => municipio.id === Number(this.coordinationServices.dataPma()[index].municipio.id));

    this.formData.patchValue({
      ...this.coordinationServices.dataPma()[index],
      provincia: provinciaSeleccionada(),
      municipio: municipioSeleccionado(),
    });
    this.polygon.set(this.coordinationServices.dataPma()[index]?.geoPosicion?.coordinates[0]);

    if (selectedItem.municipio) {
      this.formData.get('municipio')?.enable();
    } else {
      this.formData.get('municipio')?.disable();
    }
  }

  getFormatdate(date: any) {
    return moment(date).format('DD/MM/YY');
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
    this.save.emit({ save: true, delete: false, close: false, update: false });
  }
}
