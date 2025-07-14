import { NgModule } from '@angular/core';
import { MatDatepickerModule } from '@angular/material/datepicker';
import { MatNativeDateModule, MAT_DATE_LOCALE, MAT_DATE_FORMATS, DateAdapter } from '@angular/material/core';
import { CustomDateAdapter } from './custom-date-adapter';
import { CUSTOM_DATE_FORMATS } from './custom-date-formats';

@NgModule({
  imports: [MatDatepickerModule, MatNativeDateModule],
  providers: [
    { provide: MAT_DATE_LOCALE, useValue: 'es-ES' }, // Idioma español
    { provide: DateAdapter, useClass: CustomDateAdapter }, // Adaptador personalizado
    { provide: MAT_DATE_FORMATS, useValue: CUSTOM_DATE_FORMATS }, // Formatos personalizados
  ],
  exports: [MatDatepickerModule, MatNativeDateModule], // Exportar módulos de Angular Material
})
export class CalendarConfigModule {}
