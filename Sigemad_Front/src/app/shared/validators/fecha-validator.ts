import { AbstractControl, ValidationErrors, ValidatorFn } from '@angular/forms';
import { FECHA_MAXIMA_DATETIME, FECHA_MINIMA_DATETIME } from '@type/constants';
import moment from 'moment';

export class FechaValidator {
  static validarFecha(control: AbstractControl): { [key: string]: boolean } | null {
    const valor = control.value;

    if (!valor || valor === '') {
      return null;
    }

    const fecha = new Date(valor);

    if (isNaN(fecha.getTime())) {
      return { fechaInvalida: true };
    }

    if (fecha < new Date(FECHA_MINIMA_DATETIME) || fecha > new Date(FECHA_MAXIMA_DATETIME)) {
      return { fechaInvalida: true };
    }

    return null;
  }

  static validarFechaFinPosteriorFechaInicio(fechaInicioKey: string, fechaFinKey: string, permitirIgualdad: boolean = false) {
    return (form: AbstractControl): ValidationErrors | null => {
      const fechaInicio = form.get(fechaInicioKey)?.value;
      const fechaFin = form.get(fechaFinKey)?.value;
      const fechaFinControl = form.get(fechaFinKey);

      if (!fechaInicio || !fechaFin) {
        return null;
      }

      const inicio = new Date(fechaInicio);
      const fin = new Date(fechaFin);

      const condicionFechaNoValida = permitirIgualdad ? fin < inicio : fin <= inicio;

      if (condicionFechaNoValida) {
        const existingErrors = fechaFinControl?.errors || {};
        fechaFinControl?.setErrors({ ...existingErrors, fechaFinInvalida: true });
        return { fechaFinInvalida: true };
      }

      if (fechaFinControl?.errors) {
        const { fechaFinInvalida, ...otherErrors } = fechaFinControl.errors;
        fechaFinControl.setErrors(Object.keys(otherErrors).length ? otherErrors : null);
      }

      return null;
    };
  }

  static validarFechaPosteriorHoy(): ValidatorFn {
    return (control: AbstractControl): { [key: string]: any } | null => {
      if (!control.value) {
        return null;
      }

      // 1) Parsear con patrón completo
      const fechaIngresada = moment(control.value, 'YYYY-MM-DDTHH:mm');
      // — o simplemente:
      // const fechaIngresada = moment(control.value);

      const fechaActual = moment();

      // 2) Si la fecha ingresada está en el futuro, devolvemos error
      return fechaIngresada.isAfter(fechaActual) ? { fechaPosteriorHoy: true } : null;
    };
  }

  // PARA INTERVALOS DE DATOS DE FRONTERAS
  static validarHoraFinPosteriorHoraInicio(horaInicioKey: string, horaFinKey: string) {
    return (form: AbstractControl): ValidationErrors | null => {
      const horaInicio = form.get(horaInicioKey)?.value;
      const horaFin = form.get(horaFinKey)?.value;
      const horaFinControl = form.get(horaFinKey);

      if (!horaInicio || !horaFin) {
        return null;
      }

      const [hInicio, mInicio] = horaInicio.split(':').map(Number);
      const [hFin, mFin] = horaFin.split(':').map(Number);

      const minutosInicio = hInicio * 60 + mInicio;
      const minutosFin = hFin * 60 + mFin;

      if (minutosFin <= minutosInicio) {
        const existingErrors = horaFinControl?.errors || {};
        horaFinControl?.setErrors({ ...existingErrors, horaFinInvalida: true });
        return { horaFinInvalida: true };
      }

      // Limpia el error si ahora es válido
      if (horaFinControl?.errors) {
        const { horaFinInvalida, ...otrosErrores } = horaFinControl.errors;
        horaFinControl.setErrors(Object.keys(otrosErrores).length ? otrosErrores : null);
      }

      return null;
    };
  }

  static validarHoraDentroRangoIntervalo(horaKey: string, intervaloKey: string, obtenerListaIntervalos: () => any[]) {
    return (form: AbstractControl): ValidationErrors | null => {
      const hora = form.get(horaKey)?.value;
      const intervaloId = form.get(intervaloKey)?.value;
      const horaControl = form.get(horaKey);

      if (!hora || !intervaloId || !horaControl) {
        return null;
      }

      const listaIntervalos = obtenerListaIntervalos();
      const intervalo = listaIntervalos.find((i) => i.id === intervaloId);
      if (!intervalo) {
        return null;
      }

      const [h, m] = hora.split(':').map(Number);
      const minutos = h * 60 + m;

      const [hInicio, mInicio] = intervalo.inicio.split(':').map(Number);
      const [hFin, mFin] = intervalo.fin.split(':').map(Number);
      const minutosInicio = hInicio * 60 + mInicio;
      const minutosFin = hFin * 60 + mFin;

      if (minutos < minutosInicio || minutos > minutosFin) {
        horaControl.setErrors({ ...horaControl.errors, horaFueraDeRango: true });
      } else if (horaControl.hasError('horaFueraDeRango')) {
        const { horaFueraDeRango, ...otrosErrores } = horaControl.errors || {};
        if (Object.keys(otrosErrores).length === 0) {
          horaControl.setErrors(null);
        } else {
          horaControl.setErrors(otrosErrores);
        }
      }

      return null;
    };
  }
}
