import { CommonModule } from '@angular/common';
import { Component, inject, Input, signal } from '@angular/core';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatSelectModule } from '@angular/material/select';
import { FormControl, FormGroup, ReactiveFormsModule } from '@angular/forms';
import { FlexLayoutModule } from '@angular/flex-layout';
import { Capacidad, GenericMaster } from '../../../../types/actions-relevant.type';
import { DateAdapter, MAT_DATE_FORMATS, MatNativeDateModule, NativeDateAdapter } from '@angular/material/core';
import { MatDatepickerModule } from '@angular/material/datepicker';
import { MatInputModule } from '@angular/material/input';
import { ActionsRelevantService } from '../../../../services/actions-relevant.service';
import { PasoAportacion, PasoIntervencion } from '../../../../types/mobilization.type';

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
  selector: 'app-step7',
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
  templateUrl: './step7.component.html',
  styleUrl: './step7.component.scss',
  providers: [
    { provide: DateAdapter, useClass: NativeDateAdapter },
    { provide: MAT_DATE_FORMATS, useValue: FORMATO_FECHA },
  ],
})
export class Step7Component {
  @Input() formGroup!: FormGroup;
  @Input() dataMaestros: any;
  public capacidad = signal<Capacidad[]>([]);
  public movilizacionService = inject(ActionsRelevantService);
  showMedio: boolean = false;

  async ngOnInit() {
    this.capacidad.set(this.dataMaestros.capacidades);
    const pasosTipo5 = this.movilizacionService
      .dataMovilizacion()
      .flatMap((actuacion) => actuacion.Movilizaciones.flatMap((movilizacion) => movilizacion.Pasos.filter((paso) => paso.TipoPaso === 5)));

    const pasoAportacion = pasosTipo5[0] as PasoAportacion;

    const foundCapacidad = this.capacidad().find((item) => item.id === pasoAportacion.IdCapacidad);
    console.log('🚀 ~ Step7Component ~ ngOnInit ~ foundCapacidad:', foundCapacidad);
    const capacidadForm = this.formGroup.get('IdCapacidad');
    capacidadForm?.setValue(foundCapacidad);
    capacidadForm?.disable();
    if (pasoAportacion.IdCapacidad === 92) {
      this.showMedio = true;
      const medioForm = this.formGroup.get('MedioNoCatalogado');
      medioForm?.setValue(pasoAportacion.MedioNoCatalogado);
      medioForm?.disable();
    }
  }

  getForm(controlName: string): FormControl {
    return this.formGroup.get(controlName) as FormControl;
  }

  compareFn(option1: any, option2: any): boolean {
    return option1 && option2 ? option1.id === option2.id : option1 === option2;
  }
}
