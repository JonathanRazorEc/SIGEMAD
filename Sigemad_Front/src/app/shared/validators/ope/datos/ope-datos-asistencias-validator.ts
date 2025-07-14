import { AbstractControl, FormArray, ValidationErrors, ValidatorFn } from '@angular/forms';
import moment from 'moment';

export class OpeDatosAsistenciasValidator {
  // Valida que el campo 'campoValidar' sea cero si 'campoFecha' es una fecha futura
  static validarCampoCeroSiFechaFuturaDatosAsistencias(fecha: string, campo: string): ValidatorFn {
    return (control: AbstractControl): ValidationErrors | null => {
      // Si no hay fecha pasada como Input, no realizamos la validación
      if (!fecha) {
        return null;
      }

      // Convertimos la fecha pasada a un objeto Moment para validación
      const fechaMoment = moment(fecha);

      // Si la fecha no es válida, no hacemos la validación de futuro
      if (!fechaMoment.isValid()) {
        return null;
      }

      const valorCampo = control.value;

      // Si la fecha es futura y el valor del campo no es 0, devolvemos error
      if (fechaMoment.isAfter(moment(), 'day') && valorCampo !== 0) {
        return { valorCampoFuturoDatosNoCero: true };
      }

      return null; // No hay error
    };
  }

  static validarCampoNoCeroSiFechaNoFuturaDatosAsistencias(fecha: string, campo: string): ValidatorFn {
    return (control: AbstractControl): ValidationErrors | null => {
      // Si no hay fecha pasada como Input, no realizamos la validación
      if (!fecha) {
        return null;
      }

      // Convertimos la fecha pasada a un objeto Moment para validación
      const fechaMoment = moment(fecha);

      // Si la fecha no es válida, no hacemos la validación de futuro
      if (!fechaMoment.isValid()) {
        return null;
      }

      const valorCampo = control.value;

      // Si la fecha es futura y el valor del campo no es 0, devolvemos error
      if (!fechaMoment.isAfter(moment(), 'day') && valorCampo === 0) {
        return { valorCampoNoFuturoDatosCero: true };
      }

      return null; // No hay error
    };
  }

  /**
   * Valida si un tipo ya existe en la lista, comparando por ID.
   * @param lista Lista actual de elementos
   * @param idCampoFunc Función que obtiene el ID del tipo en cada elemento
   * @param nuevoId ID del nuevo tipo que se quiere insertar
   */
  static existeTipoDuplicadoEnLista<T>(lista: T[], getId: (item: T) => any, idComparar: any, indexAExcluir: number): boolean {
    return lista.some((item, index) => index !== indexAExcluir && getId(item) === idComparar);
  }

  // Validación para verificar que la fecha y los números de asistencia sean consistentes (0 para fechas futuras, > 0 para hoy o pasadas)
  static validarFechaNumeroConsistente(keysConSublistas: { [key: string]: string[] }, nombreCampoFecha: string = 'fecha'): ValidatorFn {
    return (control: AbstractControl): ValidationErrors | null => {
      const campoFecha = control.get(nombreCampoFecha);
      if (!campoFecha) return null;

      const fecha = campoFecha.value;
      if (!fecha) {
        // limpiar errores si no hay fecha
        campoFecha.setErrors(null);
        return null;
      }

      const hoy = new Date();
      hoy.setHours(0, 0, 0, 0);

      const fechaSeleccionada = new Date(fecha);
      fechaSeleccionada.setHours(0, 0, 0, 0);

      const esperadoCero = fechaSeleccionada > hoy;

      let errorEncontrado = false;

      for (const [key, subkeys] of Object.entries(keysConSublistas)) {
        const itemsControl = control.get(key);
        const items = itemsControl?.value || [];

        for (let i = 0; i < items.length; i++) {
          const item = items[i];
          const itemControl = itemsControl instanceof FormArray ? itemsControl.at(i) : null;

          const num = item?.numero;
          if ((esperadoCero && num !== 0) || (!esperadoCero && (!num || num <= 0))) {
            errorEncontrado = true;
          }

          for (const subkey of subkeys) {
            const subitems = item?.[subkey] || [];
            const subitemsControl = itemControl?.get(subkey) as FormArray | undefined;

            for (let j = 0; j < subitems.length; j++) {
              const subitem = subitems[j];
              const snum = subitem?.numero;

              if ((esperadoCero && snum !== 0) || (!esperadoCero && (!snum || snum <= 0))) {
                errorEncontrado = true;
              }
            }
          }
        }
      }

      if (errorEncontrado) {
        // Añadir error solo al control fecha
        campoFecha.setErrors({
          ...campoFecha.errors,
          ...(esperadoCero ? { inconsistenciaNumerosFechaFutura: true } : { inconsistenciaNumerosFechaActualOPasado: true }),
        });
      } else {
        // Limpiar esos errores específicos si antes existían
        if (campoFecha.hasError('inconsistenciaNumerosFechaFutura') || campoFecha.hasError('inconsistenciaNumerosFechaActualOPasado')) {
          const errores = { ...campoFecha.errors };
          delete errores['inconsistenciaNumerosFechaFutura'];
          delete errores['inconsistenciaNumerosFechaActualOPasado'];
          campoFecha.setErrors(Object.keys(errores).length ? errores : null);
        }
      }

      return null;
    };
  }
}
