import { FormControl, FormGroupDirective, NgForm } from '@angular/forms';
import { ErrorStateMatcher } from '@angular/material/core';

// Sirve para solo mostrar errores de validación si el formulario ha sido enviado
export class SubmitOnlyErrorStateMatcher implements ErrorStateMatcher {
  constructor(private isSubmitted: () => boolean) {}

  isErrorState(control: FormControl | null, form: FormGroupDirective | NgForm | null): boolean {
    return !!(control && control.invalid && this.isSubmitted());
  }
}
