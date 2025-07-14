import { Injectable } from '@angular/core';
import { NativeDateAdapter } from '@angular/material/core';

@Injectable()
export class CustomDateAdapter extends NativeDateAdapter {
  override getFirstDayOfWeek(): number {
    return 1; // Lunes como primer día
  }

  override parse(value: any): Date | null {
    if (typeof value === 'string' && /^\d{4}-\d{2}-\d{2}$/.test(value)) {
      // Convertir una cadena "YYYY-MM-DD" a un objeto Date en UTC
      const [year, month, day] = value.split('-').map(Number);
      return new Date(Date.UTC(year, month - 1, day));
    }
    return super.parse(value); // Manejo predeterminado
  }

  override format(date: Date, displayFormat: string): string {
    if (displayFormat === 'input') {
      const day = date.getDate();
      const month = date.getMonth() + 1; // Los meses comienzan en 0
      const year = date.getFullYear();
      return `${day}/${month}/${year}`; // Formato DD/MM/YYYY
    }
    return super.format(date, displayFormat);
  }
}
