import { CommonModule } from '@angular/common';
import { Component, Input, Output, EventEmitter, OnInit, signal } from '@angular/core';
import { FormGroup, ReactiveFormsModule, FormsModule } from '@angular/forms';
import { FlexLayoutModule } from '@angular/flex-layout';
import { MatAutocompleteModule } from '@angular/material/autocomplete';
import { MatButtonModule } from '@angular/material/button';
import { MatDatepickerModule } from '@angular/material/datepicker';
import { MatExpansionModule } from '@angular/material/expansion';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatIconModule } from '@angular/material/icon';
import { MatInputModule } from '@angular/material/input';
import { MatSelectModule } from '@angular/material/select';
import { MatCardModule } from '@angular/material/card';
import { TooltipDirective } from '../../../../../../shared/directive/tooltip/tooltip.directive';
import { Observable } from 'rxjs';
import { Municipality } from '../../../../../../types/municipality.type';
import { Province } from '../../../../../../types/province.type';
import { Event } from '../../../../../../types/event.type';
import { EventStatus } from '../../../../../../types/eventStatus.type';
import Feature from 'ol/Feature';
import { Geometry } from 'ol/geom';

@Component({
  selector: 'app-fire-national-form',
  standalone: true,
  imports: [
    CommonModule,
    ReactiveFormsModule,
    FormsModule,
    MatFormFieldModule,
    MatInputModule,
    MatSelectModule,
    MatButtonModule,
    MatAutocompleteModule,
    MatIconModule,
    FlexLayoutModule,
    MatExpansionModule,
    MatDatepickerModule,
    TooltipDirective,
    MatCardModule
  ],
  templateUrl: './fire-national-form.component.html',
  styleUrls: ['./fire-national-form.component.scss']
})
export class FireNationalFormComponent implements OnInit {
  @Input() formData!: FormGroup;
  @Input() provinces = signal<Province[]>([]);
  @Input() municipalities = signal<Municipality[]>([]);
  @Input() listClassEvent = signal<Event[]>([]);
  @Input() listEventStatus = signal<EventStatus[]>([]);
  @Input() provincefilteredOptions!: Observable<Province[]>;
  @Input() municipalityfilteredOptions!: Observable<Municipality[]>;
  @Input() geometry = signal<any>([]);
  @Input() fechaMinimaDateTime!: string;
  @Input() fechaMaximaDateTime!: string;
  @Input() skipFirstFields: boolean = false;
  
  @Output() onMapOpen = new EventEmitter<void>();
  @Output() provinceChange = new EventEmitter<any>();
  @Output() municipalityChange = new EventEmitter<any>();

  ngOnInit() {}

  getForm(atributo: string): any {
    return this.formData.controls[atributo];
  }

  openModalMap() {
    this.onMapOpen.emit();
  }

  displayProvince = (province: Province): string => {
    return province && province.descripcion ? province.descripcion : '';
  }

  displayMunicipality = (municipality: Municipality): string => {
    return municipality && municipality.descripcion ? municipality.descripcion : '';
  }
} 