import { Injectable } from '@angular/core';
import { FormControl } from '@angular/forms';
import { MatSort } from '@angular/material/sort';

@Injectable({
  providedIn: 'root', // Hace que esté disponible en toda la aplicación
})
export class UtilsService {
  allowOnlyNumbers(event: KeyboardEvent) {
    const charCode = event.which ? event.which : event.keyCode;
    if (charCode === 8 || charCode === 9 || charCode === 13 || charCode === 27) {
      return;
    }
    if (charCode < 48 || charCode > 57) {
      event.preventDefault();
    }
  }

  allowOnlyNumbersAndDecimal(event: KeyboardEvent) {
    const charCode = event.which ? event.which : event.keyCode;
    if (charCode !== 44 && charCode > 31 && (charCode < 48 || charCode > 57)) {
      event.preventDefault();
    }
  }

  /*
  enforceMaxLength(event: any, maxLength: number) {
    const input = event.target as HTMLInputElement;

    if (input.value.length > maxLength) {
      input.value = input.value.slice(0, maxLength);

      // 🔽 Ahora actualizamos también el formControl asociado (Angular)
      const formControl = event.target?.['ngControl']?.control;
      if (formControl) {
        formControl.setValue(input.value, { emitEvent: false }); // actualiza sin bucle infinito
        formControl.updateValueAndValidity(); // recalcula validaciones
      }
    }
  }
  */

  //
  enforceMaxLength(event: any, maxLength: number, formControl: FormControl) {
    if (!formControl) return;

    const input = event.target as HTMLInputElement;

    if (input.value.length > maxLength) {
      input.value = input.value.slice(0, maxLength);
      formControl.setValue(input.value, { emitEvent: false });
      formControl.updateValueAndValidity();
    }
  }
  //

  enforceMaxDecimals(event: any, maxDecimals: number): void {
    const input = event.target as HTMLInputElement;
    const value = input.value;

    const [integerPart, decimalPart] = value.split('.');

    if (decimalPart && decimalPart.length > maxDecimals) {
      // Truncamos los decimales
      const newValue = `${integerPart}.${decimalPart.slice(0, maxDecimals)}`;
      input.value = newValue;

      const formControl = (input as any).ngControl?.control;
      if (formControl) {
        formControl.setValue(newValue, { emitEvent: false }); // evita bucles
        formControl.updateValueAndValidity();
      }
    }
  }

  // Filtra por texto
  filtrarPorTexto(items: any[], filtro: string, startsWith: boolean = true, campo: string = 'nombre'): any[] {
    const normalizar = (str: string) =>
      (str || '')
        .toLowerCase()
        .normalize('NFD')
        .replace(/\p{Diacritic}/gu, '');

    const texto = normalizar(filtro);

    return items.filter((item) => {
      const nombre = normalizar(item?.[campo]);
      return startsWith ? nombre.startsWith(texto) : nombre.includes(texto);
    });
  }

  getSpanishCollatorSortFn(accessor: (item: any, property: string) => any) {
    const collator = new Intl.Collator('es', { sensitivity: 'base' });
    return (data: any[], sort: MatSort) => {
      const { active, direction } = sort;
      if (!active || direction === '') return data;
      return data.slice().sort((a, b) => {
        const aVal = accessor(a, active);
        const bVal = accessor(b, active);
        if (typeof aVal === 'string' && typeof bVal === 'string') {
          return collator.compare(aVal, bVal) * (direction === 'asc' ? 1 : -1);
        }
        return (aVal < bVal ? -1 : 1) * (direction === 'asc' ? 1 : -1);
      });
    };
  }
}
