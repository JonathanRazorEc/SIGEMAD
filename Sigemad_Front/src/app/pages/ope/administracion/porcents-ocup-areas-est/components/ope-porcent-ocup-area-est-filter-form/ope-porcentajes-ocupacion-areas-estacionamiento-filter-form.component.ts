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
import { OpePorcentajeOcupacionAreaEstacionamiento } from '@type/ope/administracion/ope-porcentaje-ocupacion-area-estacionamiento.type';
import { FormFieldComponent } from '@shared/Inputs/field.component';
import moment from 'moment';
import { OpePorcentajesOcupacionAreasEstacionamientoService } from '@services/ope/administracion/ope-porcentajes-ocupacion-areas-estacionamiento.service';
import { OpePorcentajeOcupacionAreaEstacionamientoCreateEdit } from '../ope-porcent-ocup-area-est-create-edit-form/ope-porcentaje-ocupacion-area-estacionamiento-create-edit-form.component';
import { FORMATO_FECHA } from '@type/date-formats';
import { MomentDateAdapter } from '@angular/material-moment-adapter';
import { LocalFiltrosOpePorcentajesOcupacionAreasEstacionamiento } from '@services/ope/administracion/local-filtro-ope-porcentajes-ocupacion-areas-estacionamiento.service';
import { MatSnackBar } from '@angular/material/snack-bar';
import { OpeUtilsService } from '@shared/services/ope/ope-utils.service';
import { IDS_TABLAS_LOGS_OPE } from '@type/ope/ope-constants';
import { OpeOcupacion } from '@type/ope/administracion/ope-ocupacion.type';
import { OpeOcupacionesService } from '@services/ope/administracion/ope-ocupaciones.service';

@Component({
  selector: 'app-ope-porcentaje-ocupacion-area-estacionamiento-filter-form',
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
  templateUrl: './ope-porcentajes-ocupacion-areas-estacionamiento-filter-form.component.html',
  styleUrl: './ope-porcentajes-ocupacion-areas-estacionamiento-filter-form.component.scss',
})
export class OpePorcentajeOcupacionAreaEstacionamientoFilterFormComponent implements OnInit {
  @Input() opePorcentajesOcupacionAreasEstacionamiento: ApiResponse<OpePorcentajeOcupacionAreaEstacionamiento[]> | undefined;
  @Input() filtros: any;
  @Input() isLoading: boolean = true;
  @Input() refreshFilterForm: boolean = true;
  @Output() opePorcentajesOcupacionAreasEstacionamientoChange = new EventEmitter<ApiResponse<OpePorcentajeOcupacionAreaEstacionamiento[]>>();
  @Output() isLoadingChange = new EventEmitter<boolean>();
  @Output() refreshFilterFormChange = new EventEmitter<boolean>();

  public filtrosOpePorcentajesOcupacionAreasEstacionamientoService = inject(LocalFiltrosOpePorcentajesOcupacionAreasEstacionamiento);
  public opePorcentajesOcupacionAreasEstacionamientoService = inject(OpePorcentajesOcupacionAreasEstacionamientoService);
  private opeOcupacionesService = inject(OpeOcupacionesService);

  private snackBar = inject(MatSnackBar);

  public menuItemActiveService = inject(MenuItemActiveService);
  private dialog = inject(MatDialog);
  public formData!: FormGroup;

  myForm!: FormGroup;

  showFilters = false;

  public showDateEnd = signal<boolean>(true);

  public opeOcupaciones = signal<OpeOcupacion[]>([]);

  // LOGS
  public opeUtilsService = inject(OpeUtilsService);
  public ID_TABLA_LOGS = IDS_TABLAS_LOGS_OPE.PORCENTAJESOCUPACIONAREASESTACIONAMIENTO;
  // FIN LOGS

  async ngOnInit() {
    const fb = new FormBuilder();
    this.myForm = fb.group({
      selectField: ['', Validators.required],
      inputField1: ['', Validators.required],
      inputField2: ['', Validators.required],
    });

    const { opeOcupacion } = this.filtros();

    this.formData = new FormGroup({
      opeOcupacion: new FormControl(opeOcupacion ?? ''),
    });

    //this.clearFormFilter();
    this.menuItemActiveService.set.emit('/ope-porcentajes-ocupacion-areas-estacionamiento');

    const opeOcupaciones = await this.opeOcupacionesService.get();
    this.opeOcupaciones.set(opeOcupaciones);

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

    this.opePorcentajesOcupacionAreasEstacionamientoChange.emit({
      count: 0,
      page: 1,
      pageSize: 10,
      data: [],
      pageCount: 0,
    });
    this.isLoading = true;
    this.isLoadingChange.emit(true);

    const { opeOcupacion } = this.formData.value;

    const opePorcentajesOcupacionAreasEstacionamiento = await this.opePorcentajesOcupacionAreasEstacionamientoService.get({
      IdOpeOcupacion: opeOcupacion,
    });
    this.filtrosOpePorcentajesOcupacionAreasEstacionamientoService.setFilters(this.formData.value);
    this.opePorcentajesOcupacionAreasEstacionamiento = opePorcentajesOcupacionAreasEstacionamiento;
    this.opePorcentajesOcupacionAreasEstacionamientoChange.emit(this.opePorcentajesOcupacionAreasEstacionamiento);
    this.isLoadingChange.emit(false);
    this.isLoading = false;
  }

  clearFormFilter() {
    this.formData.reset();
    this.formData.patchValue({
      opeOcupacion: '',
    });
  }

  getForm(atributo: string): any {
    return this.formData.controls[atributo];
  }

  goModal() {
    const dialogRef = this.dialog.open(OpePorcentajeOcupacionAreaEstacionamientoCreateEdit, {
      width: '65vw',
      maxWidth: 'none',
      disableClose: true,
      data: {
        title: 'Nuevo - Datos Porcentaje ocupación área estacionamiento',
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
      }
    });
  }

  changeBetween(event: any) {
    this.showDateEnd.set(event.value === 1 || event.value === 5 ? true : false);
  }
}
