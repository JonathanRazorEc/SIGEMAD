import { Component, Inject, OnInit } from '@angular/core';
import { MAT_DIALOG_DATA, MatDialogRef } from '@angular/material/dialog';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { MatButtonModule } from '@angular/material/button';
import { CatalogService } from '@services/catalog.service';
import Swal from 'sweetalert2';
import { MatOption } from '@angular/material/core';
import { MatSelectModule } from '@angular/material/select';
import { MatCheckboxModule } from '@angular/material/checkbox';
import { MatCard } from '@angular/material/card';

@Component({
  selector: 'app-modal-create-edit',
  standalone: true,
  imports: [CommonModule, FormsModule, MatFormFieldModule, MatInputModule, MatSelectModule, MatCheckboxModule, MatButtonModule, MatOption, MatCard],
  templateUrl: './modal-create-edit.component.html',
  styleUrl: './modal-create-edit.component.scss',
})
export class ModalCreateEditComponent implements OnInit {
  formData: any = {};
  //mode: 'create' | 'edit' = 'create';
  //
  mode: 'create' | 'edit' | 'view' = 'view';
  //
  columns: any[] = [];
  catalogId: number = 0;
  overrideWarning: boolean;
  optionsMap: { [key: string]: any[] } = {};

  constructor(
    @Inject(MAT_DIALOG_DATA) public data: any,
    private dialogRef: MatDialogRef<any>,
    private catalogService: CatalogService
  ) {
    this.overrideWarning = data.overrideWarning;
    //if (data.mode === 'edit' && data.item) {
    //
    if ((data.mode === 'edit' || data.mode === 'view') && data.item) {
      //
      this.formData = { ...data.item };
    }
  }

  ngOnInit() {
    this.mode = this.data.mode;
    this.columns = this.data.columns;
    this.catalogId = this.data.catalogId;

    this.optionsMap = {};
    this.columns
      .filter((col) => col.Tipo === 'select')
      .forEach((col) => {
        const columna = col.Columna;
        const nombreCatalogo = col.TablaRelacionada; // Ahora debe tener el nombre del catálogo, ej: 'TipoCapacidad'

        this.loadSelects();
        this.initDefaults();

        if (nombreCatalogo) {
          this.catalogService.getSelectOptions(nombreCatalogo).subscribe({
            next: (data) => {
              this.optionsMap[columna] = data;
            },
            error: (err) => {
              console.error(`Error cargando opciones para ${columna}`, err);
              this.optionsMap[columna] = []; // fallback
            },
          });
        } else {
          this.optionsMap[columna] = [];
        }
      });
    // Inicializar valores por defecto para campos bit en false
    this.columns
      .filter((col) => col.Tipo === 'bit')
      .forEach((col) => {
        if (this.formData[col.Columna] === undefined || this.formData[col.Columna] === null) {
          this.formData[col.Columna] = false;
        }
      });
    // TIPO DATETIME2
    this.columns
      .filter((col) => col.Tipo === 'datetime2')
      .forEach((col) => {
        const campo = col.Columna;
        const val = this.formData[campo];

        if (val) {
          // Asegura el formato correcto: YYYY-MM-DDTHH:mm
          const fecha = new Date(val);
          const iso = fecha.toISOString(); // ej: "2025-05-28T11:59:42.856Z"
          this.formData[campo] = iso.slice(0, 16); // "2025-05-28T11:59"
        }
      });
    // FIN TIPO DATETIME2

    // TIPO DATE
    // Si es date, formatea solo la parte YYYY-MM-DD
    this.columns
      .filter((col) => col.Tipo === 'date')
      .forEach((col) => {
        const campo = col.Columna;
        const val = this.formData[campo];

        if (val) {
          const fecha = new Date(val);
          const iso = fecha.toISOString(); // "2025-06-02T00:00:00.000Z"
          this.formData[campo] = iso.slice(0, 10); // "2025-06-02"
        }
      });
    // FIN TIPO DATE

    // TIPO TIME
    this.columns
      .filter((col) => col.Tipo === 'time')
      .forEach((col) => {
        const campo = col.Columna;
        const val = this.formData[campo];

        if (val) {
          // Asegura que sea una cadena en formato "HH:mm:ss" o "HH:mm:ss.SSS"
          const partes = val.split(':'); // ["HH", "mm", "ss"]
          if (partes.length >= 2) {
            this.formData[campo] = `${partes[0]}:${partes[1]}`; // "HH:mm"
          }
        }
      });

    // FIN TIPO TIME
  }

  private initDefaults() {
    // booleans y fechas…
    this.columns
      .filter((c) => c.Tipo === 'bit')
      .forEach((c) => {
        if (this.formData[c.Columna] == null) this.formData[c.Columna] = false;
      });

    this.columns
      .filter((c) => c.Tipo === 'datetime2')
      .forEach((c) => {
        const v = this.formData[c.Columna];
        if (v) this.formData[c.Columna] = new Date(v).toISOString().slice(0, 16);
      });
  }

  private loadSelects() {
    this.optionsMap = {};
    this.columns
      .filter((c) => c.Tipo === 'select')
      .forEach((c) => {
        this.catalogService.getSelectOptions(c.TablaRelacionada!).subscribe(
          (opts) => (this.optionsMap[c.Columna] = opts),
          () => (this.optionsMap[c.Columna] = [])
        );
      });
  }

  isFormValid(): boolean {
    if (!this.columns || this.columns.length === 0) return false;

    for (let col of this.columns) {
      const value = this.formData[col.Columna];
      if (col.Nulo === false && (value === null || value === undefined || value === '')) {
        return false;
      }
    }
    return true;
  }

  save() {
    if (!this.isFormValid()) {
      Swal.fire({
        icon: 'warning',
        title: 'Formulario incompleto',
        text: 'Por favor complete todos los campos obligatorios.',
      });
      return;
    }

    // Preparo solo las columnas esperadas
    const payload: any = {};
    for (let col of this.data.columns) {
      payload[col.Columna] = this.formData[col.Columna];
    }

    console.log('📦 Payload a guardar:', payload);

    // Si hay override
    if (this.data.mode === 'edit' && this.overrideWarning) {
      payload.overrideWarning = true;
    }

    // Validación de ID solo en modo edición
    if (this.data.mode === 'edit') {
      const id = this.formData?.Id;
      if (!id || typeof id !== 'number') {
        Swal.fire({
          icon: 'error',
          title: 'Error al editar',
          text: 'El identificador del registro no está definido o es inválido.',
        });
        return;
      }

      this.catalogService.updateCatalogItem(this.data.catalogId, id, payload).subscribe({
        next: () => {
          Swal.fire({ icon: 'success', title: 'Actualizado correctamente', timer: 1000, showConfirmButton: false });
          this.dialogRef.close(true);
        },
        error: (err) => {
          console.error('Error al guardar', err);
          Swal.fire({
            icon: 'error',
            title: 'Error al guardar',
            text: err?.error || 'No se pudo actualizar el registro. Verifica los datos o intenta nuevamente.',
          });
        },
      });
    } else {
      // Modo creación
      this.catalogService.createCatalogItem(this.data.catalogId, payload).subscribe({
        next: () => {
          Swal.fire({ icon: 'success', title: 'Creado correctamente', timer: 1000, showConfirmButton: false });
          this.dialogRef.close(true);
        },
        error: (err) => {
          console.error('Error al guardar', err);
          Swal.fire({
            icon: 'error',
            title: 'Error al guardar',
            text: err?.error || 'No se pudo crear el registro. Verifica los datos o intenta nuevamente.',
          });
        },
      });
    }
  }

  cancel() {
    this.dialogRef.close();
  }

  isInvalid(value: any): boolean {
    return value === null || value === undefined || value === '';
  }

  getOptions(colName: string): any[] {
    return this.optionsMap[colName] || [];
  }

  formatOptionLabel(opt: any, campoRelacionado: string): string {
    if (!opt || !campoRelacionado) return '';
    const campos = campoRelacionado.split(';').map((c) => c.trim());
    return campos
      .map((campo) => opt[campo] ?? '')
      .filter((v) => v !== '')
      .join(' - ')
      .trim();
  }

  // TEST
  getTipoMovimientoDescripcion(valor: string): string {
    switch (valor) {
      case 'I':
        return 'Inserción';
      case 'U':
        return 'Modificación';
      case 'D':
        return 'Borrado';
      default:
        return valor; // o 'Desconocido'
    }
  }
  // FIN TEST

  //
  get tituloModal(): string {
    switch (this.mode) {
      case 'create':
        return 'Crear registro';
      case 'edit':
        return 'Editar registro';
      case 'view':
        return 'Ver registro (no editable)';
      default:
        return 'Ver registro (no editable)';
    }
  }

  //
}
