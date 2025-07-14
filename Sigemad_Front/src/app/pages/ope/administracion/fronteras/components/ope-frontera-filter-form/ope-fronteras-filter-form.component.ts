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
import { OpeFronterasService } from '@services/ope/administracion/ope-fronteras.service';
import { LocalFiltrosOpeFronteras } from '@services/ope/administracion/local-filtro-ope-fronteras.service';
import { OpeFronteraCreateEdit } from '../ope-frontera-create-edit-form/ope-frontera-create-edit-form.component';
import { ComparativeDateService } from '@services/comparative-date.service';
import { ComparativeDate } from '@type/comparative-date.type';
import { FORMATO_FECHA } from '@type/date-formats';
import { MomentDateAdapter } from '@angular/material-moment-adapter';
import { OpeFrontera } from '@type/ope/administracion/ope-frontera.type';
import { MatSnackBar } from '@angular/material/snack-bar';
import { saveAs } from 'file-saver';
import { firstValueFrom } from 'rxjs';
import { IDS_TABLAS_LOGS_OPE, NUMEROREGISTROSLOGS } from '@type/ope/ope-constants';
import { CatalogsComponent } from 'src/app/pages/catalogs/catalogs.component';
import { OpeUtilsService } from '@shared/services/ope/ope-utils.service';
@Component({
  selector: 'app-ope-frontera-filter-form',
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
  templateUrl: './ope-fronteras-filter-form.component.html',
  styleUrl: './ope-fronteras-filter-form.component.scss',
})
export class OpeFronteraFilterFormComponent implements OnInit {
  @Input() opeFronteras: ApiResponse<OpeFrontera[]> | undefined;
  @Input() filtros: any;
  @Input() isLoading: boolean = true;
  @Input() refreshFilterForm: boolean = true;
  @Output() opeFronterasChange = new EventEmitter<ApiResponse<OpeFrontera[]>>();
  @Output() isLoadingChange = new EventEmitter<boolean>();
  @Output() refreshFilterFormChange = new EventEmitter<boolean>();

  public filtrosOpeFronterasService = inject(LocalFiltrosOpeFronteras);
  public opeFronterasService = inject(OpeFronterasService);
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
  public ID_TABLA_LOGS = IDS_TABLAS_LOGS_OPE.FRONTERAS;
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
    this.menuItemActiveService.set.emit('/ope-fronteras');

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

    this.opeFronterasChange.emit({
      count: 0,
      page: 1,
      pageSize: 10,
      data: [],
      pageCount: 0,
    });
    this.isLoading = true;
    this.isLoadingChange.emit(true);

    const { between, fechaInicioFaseSalida, fechaFinFaseSalida, fechaInicioFaseRetorno, fechaFinFaseRetorno, nombre } = this.formData.value;

    const opeFronteras = await this.opeFronterasService.get({
      IdComparativoFecha: between,
      fechaInicioFaseSalida: moment(fechaInicioFaseSalida).format('YYYY-MM-DD'),
      fechaFinFaseSalida: moment(fechaFinFaseSalida).format('YYYY-MM-DD'),
      fechaInicioFaseRetorno: moment(fechaInicioFaseRetorno).format('YYYY-MM-DD'),
      fechaFinFaseRetorno: moment(fechaFinFaseRetorno).format('YYYY-MM-DD'),
      nombre: nombre,
    });
    this.filtrosOpeFronterasService.setFilters(this.formData.value);
    this.opeFronteras = opeFronteras;
    this.opeFronterasChange.emit(this.opeFronteras);
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
    const dialogRef = this.dialog.open(OpeFronteraCreateEdit, {
      width: '65vw',
      maxWidth: 'none',
      disableClose: true,
      data: {
        title: 'Nuevo - Datos Frontera',
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

  //excel

  exportToExcel(): void {
    this.isLoading = true;
    this.isLoadingChange.emit(true);

    const v = this.formData.value;
    const filtros: any = {
      nombre: v.nombre || undefined,
      fechaInicioFaseSalida: v.fechaInicioFaseSalida ? moment(v.fechaInicioFaseSalida).format('YYYY-MM-DD') : undefined,
      fechaFinFaseSalida: v.fechaFinFaseSalida ? moment(v.fechaFinFaseSalida).format('YYYY-MM-DD') : undefined,
      fechaInicioFaseRetorno: v.fechaInicioFaseRetorno ? moment(v.fechaInicioFaseRetorno).format('YYYY-MM-DD') : undefined,
      fechaFinFaseRetorno: v.fechaFinFaseRetorno ? moment(v.fechaFinFaseRetorno).format('YYYY-MM-DD') : undefined,
    };

    this.opeFronterasService.exportarExcel(filtros).subscribe({
      next: (blob: Blob) => {
        const fileName = `OpeDatosFronteras_${moment().format('DD-MM-YYYY_HH-mm')}.xlsx`;
        saveAs(blob, fileName);
        this.isLoading = false;
        this.isLoadingChange.emit(false);
      },
      error: (err: any) => {
        console.error('Error al exportar Excel', err);
        this.snackBar.open('Error al exportar a Excel', '', {
          duration: 3000,
          panelClass: ['snackbar-rojo'],
        });
        this.isLoading = false;
        this.isLoadingChange.emit(false);
      },
    });
  }
}
