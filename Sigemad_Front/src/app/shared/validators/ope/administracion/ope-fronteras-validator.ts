import { AbstractControl, ValidationErrors, ValidatorFn } from '@angular/forms';
import moment from 'moment';

export class OpeFronterasValidator {
  static validarTransitoAltoMayorQueMedio(campoMedio: string, campoAlto: string) {
    return (form: AbstractControl): ValidationErrors | null => {
      const medioControl = form.get(campoMedio);
      const altoControl = form.get(campoAlto);

      if (!medioControl || !altoControl) return null;

      const medio = medioControl.value;
      const alto = altoControl.value;

      if (medio === null || medio === '' || alto === null || alto === '') {
        return null; // No validamos si vacíos
      }

      if (!(alto > medio)) {
        const existingErrors = altoControl.errors || {};
        altoControl.setErrors({ ...existingErrors, transitoAltoNoMayor: true });
        return { transitoAltoNoMayor: true };
      }

      // Limpiar el error si ya no aplica
      if (altoControl.errors) {
        const { transitoAltoNoMayor, ...otherErrors } = altoControl.errors;
        altoControl.setErrors(Object.keys(otherErrors).length ? otherErrors : null);
      }

      return null;
    };
  }
}
