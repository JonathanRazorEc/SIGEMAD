import { Pipe, PipeTransform } from '@angular/core';
import * as moment from 'moment-timezone';

@Pipe({
  name: 'formatFechaCEST',
  standalone: true
})
export class FormatFechaCESTPipe implements PipeTransform {
  transform(
    value: string | Date | undefined,
    formato: string = 'DD/MM/YYYY HH:mm'
  ): string {
    if (!value) return '';

    // 1. Asegurarnos de trabajar siempre con un string
    let iso = typeof value === 'string' ? value : value.toISOString();

    // 2. Recortar cualquier exceso de milisegundos, conservando zona Z o +HH:mm
    //    Ej: .4896357Z → .489Z    o   .4896357+02:00 → .489+02:00
    iso = iso.replace(/(\.\d{3})\d+([Z+-].*)$/, '$1$2');

    // 3. Si no tenía zona (poco probable tras el POST), añadimos Z para tratarlo como UTC
    if (!/[Z+-]/.test(iso)) {
      iso = iso + 'Z';
    }

    // 4. Finalmente parseamos como UTC y convertimos a CEST
    return moment
      .utc(iso)
      .tz('Europe/Madrid')
      .format(formato);
  }
}
