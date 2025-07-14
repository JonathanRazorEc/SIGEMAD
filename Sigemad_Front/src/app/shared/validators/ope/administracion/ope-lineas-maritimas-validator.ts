import { AbstractControl, ValidationErrors, ValidatorFn } from '@angular/forms';
import moment from 'moment';

export class OpeLineasMaritimasValidator {
  // Valida que el número de pasajeros sea mayor o igual al número de vehículos
  static validarNumeroPasajerosMayorOIgualAvehiculos(pasajerosKey: string, vehiculosKey: string): ValidatorFn {
    return (form: AbstractControl): ValidationErrors | null => {
      const pasajerosControl = form.get(pasajerosKey);
      const vehiculosControl = form.get(vehiculosKey);

      const numeroPasajeros = pasajerosControl?.value;
      const numeroVehiculos = vehiculosControl?.value;

      if (numeroPasajeros == null || numeroVehiculos == null || isNaN(numeroPasajeros) || isNaN(numeroVehiculos)) {
        return null;
      }

      if (numeroPasajeros < numeroVehiculos) {
        const existingErrors = pasajerosControl?.errors || {};
        pasajerosControl?.setErrors({ ...existingErrors, numeroPasajerosMenorQueVehiculos: true });
        return { numeroPasajerosMenorQueVehiculos: true };
      }

      // Limpiar el error si ya no aplica
      if (pasajerosControl?.errors) {
        const { numeroPasajerosMenorQueVehiculos, ...otrosErrores } = pasajerosControl.errors;
        pasajerosControl.setErrors(Object.keys(otrosErrores).length ? otrosErrores : null);
      }

      return null;
    };
  }

  // Valida que los pueros 'origen' y 'destino' no sean iguales
  static opePuertosOrigenDestinoDiferentesValidator(origenKey: string, destinoKey: string): ValidatorFn {
    return (form: AbstractControl): ValidationErrors | null => {
      const origenControl = form.get(origenKey);
      const destinoControl = form.get(destinoKey);

      const origen = origenControl?.value;
      const destino = destinoControl?.value;

      if (origen && destino && origen === destino) {
        // Si son iguales, devolvemos el error
        const existingErrors = origenControl?.errors || {};
        destinoControl?.setErrors({ ...existingErrors, opePuertosOrigenDestinoIguales: true });
        return { opePuertosOrigenDestinoIguales: true };
      }

      // Limpiar el error si ya no aplica
      if (destinoControl?.errors) {
        const { opePuertosOrigenDestinoIguales, ...otrosErrores } = destinoControl.errors;
        destinoControl.setErrors(Object.keys(otrosErrores).length ? otrosErrores : null);
      }

      return null;
    };
  }
}
