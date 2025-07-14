import { CommonModule } from '@angular/common';
import { Component, EventEmitter, inject, Input, OnInit, Output, signal, SimpleChanges } from '@angular/core';
import { DateAdapter, MAT_DATE_FORMATS, NativeDateAdapter } from '@angular/material/core';
import { FlexLayoutModule } from '@angular/flex-layout';
import { FormBuilder, FormControl, FormGroup, FormsModule, ReactiveFormsModule, Validators } from '@angular/forms';
import { MatAutocompleteModule } from '@angular/material/autocomplete';
import { MatButtonModule } from '@angular/material/button';
import { MatDatepickerModule } from '@angular/material/datepicker';
import { MatExpansionModule, MatExpansionPanel } from '@angular/material/expansion';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatIconModule } from '@angular/material/icon';
import { MatInputModule } from '@angular/material/input';
import { MatSelectModule } from '@angular/material/select';
import { MatDialog, MatDialogModule } from '@angular/material/dialog';
import { MenuItemActiveService } from '@services/menu-item-active.service';
import { ApiResponse } from '@type/api-response.type';
import { FormFieldComponent } from '@shared/Inputs/field.component';
import moment from 'moment';
import { OpeAreasEstacionamientoService } from '@services/ope/administracion/ope-areas-estacionamiento.service';
import { LocalFiltrosOpeAreasEstacionamiento } from '@services/ope/administracion/local-filtro-ope-areas-estacionamiento.service';
import { OpeAreaEstacionamientoCreateEdit } from '../ope-area-estacionamiento-create-edit-form/ope-area-estacionamiento-create-edit-form.component';
import { ComparativeDateService } from '@services/comparative-date.service';
import { ComparativeDate } from '@type/comparative-date.type';
import { FORMATO_FECHA } from '@type/date-formats';
import { MomentDateAdapter } from '@angular/material-moment-adapter';
import { OpeAreaEstacionamiento } from '@type/ope/administracion/ope-area-estacionamiento.type';
import { MatSnackBar } from '@angular/material/snack-bar';
import { OpeUtilsService } from '@shared/services/ope/ope-utils.service';
import { IDS_TABLAS_LOGS_OPE } from '@type/ope/ope-constants';

@Component({
  selector: 'app-ope-area-estacionamiento-filter-form',
  standalone: true,
  imports: [
    CommonModule,
    ReactiveFormsModule,
    FormsModule,
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
    MatDialogModule,
  ],
  providers: [
    { provide: DateAdapter, useClass: MomentDateAdapter },
    { provide: MAT_DATE_FORMATS, useValue: FORMATO_FECHA },
  ],
  templateUrl: './ope-areas-estacionamiento-filter-form.component.html',
  styleUrl: './ope-areas-estacionamiento-filter-form.component.scss',
})
export class OpeAreaEstacionamientoFilterFormComponent implements OnInit {
  @Input() opeAreasEstacionamiento: ApiResponse<OpeAreaEstacionamiento[]> | undefined;
  @Input() filtros: any;
  @Input() isLoading: boolean = true;
  @Input() refreshFilterForm: boolean = true;
  @Output() opeAreasEstacionamientoChange = new EventEmitter<ApiResponse<OpeAreaEstacionamiento[]>>();
  @Output() isLoadingChange = new EventEmitter<boolean>();
  @Output() refreshFilterFormChange = new EventEmitter<boolean>();

  public filtrosOpeAreasEstacionamientoService = inject(LocalFiltrosOpeAreasEstacionamiento);
  public opeAreasEstacionamientoService = inject(OpeAreasEstacionamientoService);
  public comparativeDateService = inject(ComparativeDateService);

  private snackBar = inject(MatSnackBar);

  public comparativeDates = signal<ComparativeDate[]>([]);

  public menuItemActiveService = inject(MenuItemActiveService);
  private dialog = inject(MatDialog);
  public formData!: FormGroup;

  myForm!: FormGroup;

  showFilters = false;

  public showDateEnd = signal<boolean>(true);

  // LOGS
  public opeUtilsService = inject(OpeUtilsService);
  public ID_TABLA_LOGS = IDS_TABLAS_LOGS_OPE.AREASESTACIONAMIENTO;
  // FIN LOGS

  async ngOnInit() {
    const fb = new FormBuilder();
    this.myForm = fb.group({
      selectField: ['', Validators.required],
      inputField1: ['', Validators.required],
      inputField2: ['', Validators.required],
    });

    const { nombre, fechaInicioFaseSalida, fechaFinFaseSalida, fechaInicioFaseRetorno, fechaFinFaseRetorno, between } = this.filtros();

    this.formData = new FormGroup({
      nombre: new FormControl(nombre ?? ''),
      between: new FormControl(between ?? 1),
      fechaInicioFaseSalida: new FormControl(fechaInicioFaseSalida ?? moment().subtract(4, 'days').toDate()),
      fechaFinFaseSalida: new FormControl(fechaFinFaseSalida ?? moment().toDate()),
      fechaInicioFaseRetorno: new FormControl(fechaInicioFaseRetorno ?? moment().subtract(4, 'days').toDate()),
      fechaFinFaseRetorno: new FormControl(fechaFinFaseRetorno ?? moment().toDate()),
    });

    //this.clearFormFilter();
    this.menuItemActiveService.set.emit('/ope-areas-estacionamiento');

    const comparativeDates = await this.comparativeDateService.get();
    this.comparativeDates.set(comparativeDates);

    this.onSubmit();
  }

  ngOnChanges(changes: SimpleChanges): void {
    if ('refreshFilterForm' in changes) {
      this.onSubmit();
    }
  }

  /*
  toggleAccordion(panel: MatExpansionPanel) {
    panel.toggle();
  }
  */

  async onSubmit() {
    if (!this.formData) {
      return;
    }

    this.opeAreasEstacionamientoChange.emit({
      count: 0,
      page: 1,
      pageSize: 10,
      data: [],
      pageCount: 0,
    });
    this.isLoading = true;
    this.isLoadingChange.emit(true);

    const { between, fechaInicioFaseSalida, fechaFinFaseSalida, fechaInicioFaseRetorno, fechaFinFaseRetorno, nombre } = this.formData.value;

    const opeAreasEstacionamiento = await this.opeAreasEstacionamientoService.get({
      IdComparativoFecha: between,
      fechaInicioFaseSalida: moment(fechaInicioFaseSalida).format('YYYY-MM-DD'),
      fechaFinFaseSalida: moment(fechaFinFaseSalida).format('YYYY-MM-DD'),
      fechaInicioFaseRetorno: moment(fechaInicioFaseRetorno).format('YYYY-MM-DD'),
      fechaFinFaseRetorno: moment(fechaFinFaseRetorno).format('YYYY-MM-DD'),
      nombre: nombre,
    });
    this.filtrosOpeAreasEstacionamientoService.setFilters(this.formData.value);
    this.opeAreasEstacionamiento = opeAreasEstacionamiento;
    this.opeAreasEstacionamientoChange.emit(this.opeAreasEstacionamiento);
    this.isLoadingChange.emit(false);
    this.isLoading = false;
  }

  clearFormFilter() {
    this.formData.reset();
    this.formData.patchValue({
      between: 1,
      fechaInicioFaseSalida: moment().subtract(4, 'days').toDate(),
      fechaFinFaseSalida: moment().toDate(),
      fechaInicioFaseRetorno: moment().subtract(4, 'days').toDate(),
      fechaFinFaseRetorno: moment().toDate(),
      nombre: '',
    });
  }

  getForm(atributo: string): any {
    return this.formData.controls[atributo];
  }

  goModal() {
    const dialogRef = this.dialog.open(OpeAreaEstacionamientoCreateEdit, {
      width: '65vw',
      maxWidth: 'none',
      disableClose: true,
      data: {
        title: 'Nuevo - Datos Área de Estacionamiento',
        fire: {},
      },
    });

    dialogRef.afterClosed().subscribe((result) => {
      if (result) {
        console.log('Modal result:', result);
        this.snackBar.open('Datos ingresados correctamente!', '', {
          duration: 3000,
          horizontalPosition: 'center',
          verticalPosition: 'bottom',
          panelClass: ['snackbar-verde'],
        });
        this.onSubmit();
        this.onSubmit();
      }
    });
  }

  changeBetween(event: any) {
    this.showDateEnd.set(event.value === 1 || event.value === 5 ? true : false);
  }
}
