import { CommonModule } from '@angular/common';
import { Component, ElementRef, EventEmitter, Input, OnInit, Output, ViewChild, forwardRef, CUSTOM_ELEMENTS_SCHEMA } from '@angular/core';
import { ControlValueAccessor, FormControl, NG_VALUE_ACCESSOR, ReactiveFormsModule } from '@angular/forms';
import { MatAutocompleteModule, MatAutocompleteSelectedEvent } from '@angular/material/autocomplete';
import { MatChipsModule } from '@angular/material/chips';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatIconModule } from '@angular/material/icon';
import { MatInputModule } from '@angular/material/input';
import { Observable, map, startWith, of } from 'rxjs';

@Component({
  selector: 'app-multi-select-autocomplete',
  standalone: true,
  imports: [CommonModule, ReactiveFormsModule, MatFormFieldModule, MatInputModule, MatAutocompleteModule, MatChipsModule, MatIconModule],
  templateUrl: './multi-select-autocomplete.component.html',
  styleUrls: ['./multi-select-autocomplete.component.scss'],
  schemas: [CUSTOM_ELEMENTS_SCHEMA],
  providers: [
    {
      provide: NG_VALUE_ACCESSOR,
      useExisting: forwardRef(() => MultiSelectAutocompleteComponent),
      multi: true,
    },
  ],
})
export class MultiSelectAutocompleteComponent implements OnInit, ControlValueAccessor {
  @Input() options: any[] = [];
  @Input() label: string = 'Seleccione';
  @Input() placeholder: string = 'Buscar...';
  @Input() isRequired: boolean = false;
  @Input() displayProperty: string = 'descripcion';
  @Input() alternativeDisplayProperty: string | null = null;
  @Input() valueProperty: string = 'id';

  @Output() selectionChange = new EventEmitter<any[]>();

  @ViewChild('autocompleteInput') autocompleteInput!: ElementRef<HTMLInputElement>;

  inputCtrl = new FormControl('');
  selectedItems: any[] = [];
  filteredOptions!: Observable<any[]>;

  private onChange: any = () => {};
  private onTouched: any = () => {};

  ngOnInit(): void {
    this.setupFilteredOptions();
  }

  private setupFilteredOptions(): void {
    this.filteredOptions = this.inputCtrl.valueChanges.pipe(
      startWith(''),
      map((value) => {
        if (!value || value === '') {
          return this._filterNotSelected();
        }
        return this._filter(value);
      })
    );
  }

  private _filter(value: string | any): any[] {
    if (!this.options) return [];

    let filterValue = '';
    if (typeof value === 'string') {
      filterValue = value.toLowerCase();
    } else if (typeof value === 'object') {
      return this._filterNotSelected();
    }

    return this.options.filter((option) => {
      if (this.isSelected(option)) {
        return false;
      }

      if (filterValue.length > 0) {
        const displayText = this.getDisplayText(option).toLowerCase();
        return displayText.includes(filterValue);
      }

      return true;
    });
  }

  isSelected(option: any): boolean {
    return this.selectedItems.some((item) => this.getValue(item) === this.getValue(option));
  }

  getDisplayText(option: any): string {
    if (!option) return '';

    if (typeof option === 'object') {
      if (option[this.displayProperty]) {
        return option[this.displayProperty];
      }
      if (this.alternativeDisplayProperty && option[this.alternativeDisplayProperty]) {
        return option[this.alternativeDisplayProperty];
      }
      return '';
    }

    const foundOption = this.options.find((opt) => this.getValue(opt) === option);
    return foundOption ? this.getDisplayText(foundOption) : String(option);
  }

  getValue(option: any): any {
    if (!option) return null;

    if (typeof option === 'object') {
      return option[this.valueProperty];
    }

    return option;
  }

  selected(event: MatAutocompleteSelectedEvent): void {
    const option = event.option.value;

    if (!this.isSelected(option)) {
      this.selectedItems.push(option);
      this.emitChanges();
    }

    this.autocompleteInput.nativeElement.value = '';
    this.inputCtrl.setValue('');
  }

  removeItem(item: any): void {
    const index = this.selectedItems.indexOf(item);

    if (index >= 0) {
      this.selectedItems.splice(index, 1);
      this.emitChanges();
    }
  }

  isEmpty(): boolean {
    return this.selectedItems.length === 0;
  }

  private emitChanges(): void {
    const values = this.selectedItems.map((item) => this.getValue(item));
    this.onChange(values);
    this.onTouched();
    this.selectionChange.emit(values);
  }

  writeValue(values: any[]): void {
    if (Array.isArray(values)) {
      this.selectedItems = [];

      values.forEach((value) => {
        const option = this.options.find((opt) => this.getValue(opt) === value);
        if (option) {
          this.selectedItems.push(option);
        } else if (value !== null && value !== undefined) {
          this.selectedItems.push(value);
        }
      });
    } else {
      this.selectedItems = [];
    }
  }

  registerOnChange(fn: any): void {
    this.onChange = fn;
  }

  registerOnTouched(fn: any): void {
    this.onTouched = fn;
  }

  setDisabledState(isDisabled: boolean): void {
    if (isDisabled) {
      this.inputCtrl.disable();
    } else {
      this.inputCtrl.enable();
    }
  }

  showAllOptions(): void {
    this.inputCtrl.setValue('');
  }

  private _filterNotSelected(): any[] {
    return this.options.filter((option) => !this.isSelected(option));
  }
}
