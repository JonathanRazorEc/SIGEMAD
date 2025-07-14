import { AbstractControl, ValidationErrors, ValidatorFn } from '@angular/forms';
import moment from 'moment';

// VALIDACIONES COMUNES PARA OPE
export class OpeValidator {
  // Valida que el campo 'campoValidar' sea cero si 'campoFecha' es una fecha futura
  static validarCampoCeroSiFechaFutura(campoFecha: string, campoValidar: string): ValidatorFn {
    return (control: AbstractControl): ValidationErrors | null => {
      const formGroup = control.parent;
      if (!formGroup) return null;

      const fecha = formGroup.get(campoFecha)?.value;
      const valorCampo = control.value;

      const fechaMoment = moment(fecha);

      // Si la fecha no es válida, no hacemos la validación de futuro.
      if (!fechaMoment.isValid()) {
        return null;
      }

      // Si la fecha es futura y el valor del campo no es 0, devolvemos error.
      if (fechaMoment.isAfter(moment(), 'day') && valorCampo !== 0) {
        return { valorCampoFuturoDatosNoCero: true };
      }

      return null;
    };
  }

  // Valida que un control sea requerido si otro control está habilitado
  static requiredIfEnabled(controlToCheckName: string, controlToValidateName: string): ValidatorFn {
    return (form: AbstractControl): ValidationErrors | null => {
      const controlToCheck = form.get(controlToCheckName);
      const controlToValidate = form.get(controlToValidateName);

      //if (controlToCheck?.enabled && !controlToValidate?.value) {
      const value = controlToValidate?.value;
      if (controlToCheck?.enabled && (value === null || value === undefined || value === '')) {
        const existingErrors = controlToValidate?.errors || {};
        controlToValidate?.setErrors({ ...existingErrors, required: true });
        return { requiredIfEnabled: true };
      }

      // Limpiar el error si ya no aplica
      if (controlToValidate?.errors) {
        const { required, ...otherErrors } = controlToValidate.errors;
        controlToValidate.setErrors(Object.keys(otherErrors).length ? otherErrors : null);
      }

      return null;
    };
  }

  // Valida que un control sea requerido si otro control está habilitado y 'criterioNumericoRadio' es igual a 'conditionValue'
  static requiredIfCriterioNumericoRadioEnabledConCantidad(controlToCheckName: string, controlToValidateName: string): ValidatorFn {
    return (form: AbstractControl): ValidationErrors | null => {
      const controlToCheck = form.get(controlToCheckName);
      const controlToValidate = form.get(controlToValidateName);

      const radioControl = form.get('criterioNumericoRadio');
      const radioValue = radioControl?.value;

      // Solo validamos si 'criterioNumericoRadio' es igual a 'conditionValue' (por defecto 'cantidad')
      if (controlToCheck?.enabled && radioValue === 'cantidad') {
        const value = controlToValidate?.value;

        // Si 'controlToValidate' está vacío, marcamos el error como requerido
        if (!value) {
          const existingErrors = controlToValidate?.errors || {};
          controlToValidate?.setErrors({ ...existingErrors, required: true });
          return { requiredIfEnabled: true };
        }
      }

      // Limpiar el error si ya no aplica
      if (controlToValidate?.errors) {
        const { required, ...otherErrors } = controlToValidate.errors;
        controlToValidate.setErrors(Object.keys(otherErrors).length ? otherErrors : null);
      }

      return null;
    };
  }

  static opcionValidaDeSelectPorId(availableOptionsFn: () => { id: any }[]): ValidatorFn {
    return (control: AbstractControl) => {
      const value = control.value;

      // Si no hay valor, no hay error (esto lo controla el required aparte)
      if (!value) {
        return null;
      }

      const isValid = availableOptionsFn().some((option) => option.id === value);

      return isValid ? null : { invalidOption: true };
    };
  }

  //
  static opcionValidaDeSelectPorOption(availableOptionsFn: () => { id: any }[]): ValidatorFn {
    return (control: AbstractControl) => {
      const value = control.value;

      if (!value) {
        return null;
      }

      const isValid = value && value.id !== undefined ? availableOptionsFn().some((option) => option.id === value.id) : false;

      return isValid ? null : { invalidOption: true };
    };
  }
  //

  static optionalIntegerValidator(): ValidatorFn {
    return (control: AbstractControl): ValidationErrors | null => {
      const value = control.value;

      // Permitir valor nulo o vacío
      if (value === null || value === '') {
        return null;
      }

      const numberValue = Number(value);

      // Validar si es un número entero
      if (!Number.isInteger(numberValue)) {
        return { pattern: true };
      }

      // Validar rango
      if (numberValue < 0) {
        return { min: true };
      }

      if (numberValue > 9999999) {
        return { max: true };
      }

      return null;
    };
  }
}
