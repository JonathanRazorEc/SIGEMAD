import { CommonModule } from '@angular/common';
import { Component, Input, signal } from '@angular/core';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatSelectModule } from '@angular/material/select';
import { FormControl, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
import { FlexLayoutModule } from '@angular/flex-layout';
import { Capacidad, GenericMaster } from '../../../../types/actions-relevant.type';
import { DateAdapter, MAT_DATE_FORMATS, MatNativeDateModule, NativeDateAdapter } from '@angular/material/core';
import { MatDatepickerModule } from '@angular/material/datepicker';
import { MatInputModule } from '@angular/material/input';

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
  selector: 'app-step5',
  standalone: true,
  imports: [
    CommonModule,
    MatFormFieldModule,
    MatSelectModule,
    ReactiveFormsModule,
    FlexLayoutModule,
    MatDatepickerModule,
    MatNativeDateModule,
    MatInputModule,
  ],
  templateUrl: './step5.component.html',
  styleUrl: './step5.component.scss',
  providers: [
    { provide: DateAdapter, useClass: NativeDateAdapter },
    { provide: MAT_DATE_FORMATS, useValue: FORMATO_FECHA },
  ],
})
export class Step5Component {
  @Input() formGroup!: FormGroup;
  @Input() dataMaestros: any;
  public capacidad = signal<Capacidad[]>([]);
  public tipoAdmin = signal<GenericMaster[]>([]);
  showMedioNoCatalogado = false;
  showTipoAdmin = false;

  async ngOnInit() {
    this.capacidad.set(this.dataMaestros.capacidades);
    this.tipoAdmin.set(this.dataMaestros.tipoAdmin);
    console.log('🚀 ~ Step5Component ~ ngOnInit ~  this.tipoAdmin:', this.tipoAdmin());
  }

  getForm(controlName: string): FormControl {
    return this.formGroup.get(controlName) as FormControl;
  }

  loadMedio(event: any) {
    const id = event.value.id;

    const tipoAdminId = event?.value?.entidad?.organismo?.administracion?.tipoAdministracion?.id;

    const foundTipoAdmin = this.tipoAdmin().find((item) => item.id === tipoAdminId);
    const controlTipoAdmin = this.formGroup.get('IdTipoAdministracion');
    if (foundTipoAdmin) {
      console.log("🚀 ~ Step5Component ~ loadMedio ~ foundTipoAdmin:", foundTipoAdmin)
      controlTipoAdmin?.setValue(foundTipoAdmin);
      controlTipoAdmin?.disable();
    }

    const medioControl = this.formGroup.get('MedioNoCatalogado');
    if (id === 92) {
      medioControl?.enable();
      medioControl?.setValidators(Validators.required);
      this.showMedioNoCatalogado = true;
      controlTipoAdmin?.clearValidators();
      controlTipoAdmin?.enable();
      controlTipoAdmin?.updateValueAndValidity();
    } else {
      this.showMedioNoCatalogado = false;
      medioControl?.disable();
      medioControl?.clearValidators();
    }
    medioControl?.updateValueAndValidity();
  }
}
