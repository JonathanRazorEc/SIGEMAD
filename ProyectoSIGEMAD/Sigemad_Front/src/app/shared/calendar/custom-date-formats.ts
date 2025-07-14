import { MatDateFormats } from '@angular/material/core';

export const CUSTOM_DATE_FORMATS: MatDateFormats = {
  parse: {
    dateInput: 'DD/MM/YYYY', // Entrada del usuario
  },
  display: {
    dateInput: 'DD/MM/YYYY', // Cómo se muestra en el input
    monthYearLabel: 'MMMM YYYY', // Mes y año en el selector
    dateA11yLabel: 'LL', // Formato accesible
    monthYearA11yLabel: 'MMMM YYYY', // Mes y año accesible
  },
};
