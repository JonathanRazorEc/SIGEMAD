import { AbstractControl, ValidationErrors, ValidatorFn } from '@angular/forms';
import moment from 'moment';

export class OpeDatosEmbarquesDiariosValidator {
  // Valida que el campo 'pasajeros' y 'vehiculos' no sean obligatorios si 'rotaciones' es cero
  static validarNumeroPasajerosYVehiculosSiRotacionesCero(campoRotaciones: string, campoPasajeros: string, campoVehiculos: string): ValidatorFn {
    return (formGroup: AbstractControl): ValidationErrors | null => {
      const rotaciones = formGroup.get(campoRotaciones)?.value;
      const pasajerosControl = formGroup.get(campoPasajeros);
      const vehiculosControl = formGroup.get(campoVehiculos);

      const setOrClearError = (control: AbstractControl | null, errorKey: string, condition: boolean) => {
        if (!control) return;

        const currentErrors = control.errors || {};

        if (condition) {
          currentErrors[errorKey] = true;
          control.setErrors(currentErrors);
        } else {
          delete currentErrors[errorKey];
          control.setErrors(Object.keys(currentErrors).length ? currentErrors : null);
        }
      };

      if (rotaciones === 0) {
        const errores: any = {};

        const pasajerosDistintoDeCero = pasajerosControl?.value !== 0;
        const vehiculosDistintoDeCero = vehiculosControl?.value !== 0;

        setOrClearError(pasajerosControl, 'numeroPasajerosDistintoDeCeroConRotacionesCero', pasajerosDistintoDeCero);
        setOrClearError(vehiculosControl, 'numeroVehiculosDistintoDeCeroConRotacionesCero', vehiculosDistintoDeCero);

        if (pasajerosDistintoDeCero) {
          errores.numeroPasajerosDistintoDeCeroConRotacionesCero = true;
        }

        if (vehiculosDistintoDeCero) {
          errores.numeroVehiculosDistintoDeCeroConRotacionesCero = true;
        }

        return Object.keys(errores).length > 0 ? errores : null;
      }

      // Si rotaciones > 0, limpia los errores personalizados pero conserva los demás
      setOrClearError(pasajerosControl, 'numeroPasajerosDistintoDeCeroConRotacionesCero', false);
      setOrClearError(vehiculosControl, 'numeroVehiculosDistintoDeCeroConRotacionesCero', false);

      return null;
    };
  }

  // Valida que el campo 'pasajeros' y 'vehiculos' sean obligatorios si 'rotaciones' es mayor a cero
  static validarPasajerosYVehiculosSiRotacionesMayorCero(campoRotaciones: string, campoPasajeros: string, campoVehiculos: string): ValidatorFn {
    return (formGroup: AbstractControl): ValidationErrors | null => {
      const rotaciones = formGroup.get(campoRotaciones)?.value;
      const pasajerosControl = formGroup.get(campoPasajeros);
      const vehiculosControl = formGroup.get(campoVehiculos);

      const setOrClearError = (control: AbstractControl | null, errorKey: string, condition: boolean) => {
        if (!control) return;

        const currentErrors = control.errors || {};

        if (condition) {
          currentErrors[errorKey] = true;
          control.setErrors(currentErrors);
        } else {
          delete currentErrors[errorKey];
          control.setErrors(Object.keys(currentErrors).length ? currentErrors : null);
        }
      };

      if (rotaciones > 0) {
        const errores: any = {};

        const pasajerosInvalido = !pasajerosControl?.value || pasajerosControl.value <= 0;
        const vehiculosInvalido = !vehiculosControl?.value || vehiculosControl.value <= 0;

        setOrClearError(pasajerosControl, 'numeroPasajerosObligatorioConRotaciones', pasajerosInvalido);
        setOrClearError(vehiculosControl, 'numeroVehiculosObligatorioConRotaciones', vehiculosInvalido);

        if (pasajerosInvalido) {
          errores.numeroPasajerosObligatorioConRotaciones = true;
        }

        if (vehiculosInvalido) {
          errores.numeroVehiculosObligatorioConRotaciones = true;
        }

        return Object.keys(errores).length > 0 ? errores : null;
      }

      // Si rotaciones == 0, limpiar errores de este validador (pero conservar otros)
      setOrClearError(pasajerosControl, 'numeroPasajerosObligatorioConRotaciones', false);
      setOrClearError(vehiculosControl, 'numeroVehiculosObligatorioConRotaciones', false);

      return null;
    };
  }

  static validarCamposCeroSiFechaFutura(campos: string[], nombreCampoFecha: string): ValidatorFn {
    return (control: AbstractControl): ValidationErrors | null => {
      const fecha = control.get(nombreCampoFecha)?.value;
      if (!fecha) return null;

      const hoy = new Date();
      hoy.setHours(0, 0, 0, 0);

      const fechaSeleccionada = new Date(fecha);
      fechaSeleccionada.setHours(0, 0, 0, 0);

      const fechaEsFutura = fechaSeleccionada > hoy;

      let errorEncontrado = false;

      campos.forEach((campoNombre) => {
        const campo = control.get(campoNombre);
        if (!campo) return;

        const valor = campo.value;

        if (fechaEsFutura) {
          if (valor !== 0) {
            campo.setErrors({ ...campo.errors, valorCampoFuturoDatosNoCero: true });
            errorEncontrado = true;
          } else if (campo.hasError('valorCampoFuturoDatosNoCero')) {
            const erroresActuales = { ...campo.errors };
            delete erroresActuales['valorCampoFuturoDatosNoCero'];
            campo.setErrors(Object.keys(erroresActuales).length ? erroresActuales : null);
          }
        } else {
          if (campo.hasError('valorCampoFuturoDatosNoCero')) {
            const erroresActuales = { ...campo.errors };
            delete erroresActuales['valorCampoFuturoDatosNoCero'];
            campo.setErrors(Object.keys(erroresActuales).length ? erroresActuales : null);
          }
        }
      });

      return errorEncontrado ? { valorCampoFuturoDatosNoCero: true } : null;
    };
  }
}
