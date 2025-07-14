import { AbstractControl, ValidationErrors, ValidatorFn } from '@angular/forms';
import moment from 'moment';

export class OpeAreasEstacionamientoValidator {
  // Valida que el campo 'opePuerto' sea requerido si 'instalacionPortuaria' es verdadero
  static validarOpePuertoSiInstalacionPortuaria(instalacionPortuariaKey: string, opePuertoKey: string): ValidatorFn {
    return (form: AbstractControl): ValidationErrors | null => {
      const instalacionPortuariaControl = form.get(instalacionPortuariaKey);
      const opePuertoControl = form.get(opePuertoKey);

      const instalacionPortuaria = instalacionPortuariaControl?.value;
      const opePuerto = opePuertoControl?.value;

      // Si 'instalacionPortuaria' es verdadero y 'opePuerto' está vacío, es un error
      if (instalacionPortuaria && !opePuerto) {
        const existingErrors = opePuertoControl?.errors || {};
        opePuertoControl?.setErrors({ ...existingErrors, required: true });
        return { required: true };
      }

      // Limpiar el error si ya no aplica
      if (opePuertoControl?.errors) {
        const { required, ...otrosErrores } = opePuertoControl.errors;
        opePuertoControl.setErrors(Object.keys(otrosErrores).length ? otrosErrores : null);
      }

      return null;
    };
  }
}
