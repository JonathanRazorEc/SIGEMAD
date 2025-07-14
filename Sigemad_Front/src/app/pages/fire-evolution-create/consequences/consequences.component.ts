import { CommonModule } from '@angular/common';
import { Component, computed, EventEmitter, inject, Input, OnInit, Output, signal, ViewChild, effect } from '@angular/core';
import { FlexLayoutModule } from '@angular/flex-layout';
import { FormBuilder, FormControl, FormGroup, ReactiveFormsModule, Validators, FormGroupDirective } from '@angular/forms';
import { Observable, of, startWith, map } from 'rxjs';
import { MatAutocompleteModule } from '@angular/material/autocomplete';
import { MatButtonModule } from '@angular/material/button';
import { DateAdapter, MAT_DATE_FORMATS, MatNativeDateModule, NativeDateAdapter } from '@angular/material/core';
import { MatDatepickerModule } from '@angular/material/datepicker';
import { MAT_DIALOG_DATA, MatDialog } from '@angular/material/dialog';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatGridListModule } from '@angular/material/grid-list';
import { MatIconModule } from '@angular/material/icon';
import { MatInputModule } from '@angular/material/input';
import { MatSelectModule } from '@angular/material/select';
import { MatSnackBar } from '@angular/material/snack-bar';
import { MatSort } from '@angular/material/sort';
import { MatTableDataSource, MatTableModule } from '@angular/material/table';
import moment from 'moment';
import { NgxSpinnerModule, NgxSpinnerService } from 'ngx-spinner';
import { ConsequenceService } from '../../../services/consequence.service';
import { EvolutionService } from '../../../services/evolution.service';
import { ImpactTypeService } from '../../../services/impact-type.service';
import { MunicipalityService } from '../../../services/municipality.service';
import { ProvinceService } from '../../../services/province.service';
import { RecordsService } from '@services/records.service';
import { CoordinationAddress } from '../../../types/coordination-address';
import { MinorEntity } from '../../../types/minor-entity.type';
import { Municipality } from '../../../types/municipality.type';
import { Province } from '../../../types/province.type';

import { UtilsService } from '../../../shared/services/utils.service';
import { SavePayloadModal } from '../../../types/save-payload-modal';
import { MatTooltipModule } from '@angular/material/tooltip';
import { AlertService } from 'src/app/shared/alert/alert.service';

const FORMATO_FECHA = {
  parse: {
    dateInput: 'LL',
  },
  display: {
    dateInput: 'LL',
    monthYearLabel: 'MMM YYYY',
    dateA11yLabel: 'LL',
    monthYearA11yLabel: 'MMMM YYYY',
  },
};

interface ConsequencesForm {
  tipo: any;
  estimado: number;
  total: number;
  descripcion: any;
  cantidad: number;
  provincia: any;
  municipio: any;
  observaciones: string;
  fechaHora?: string;
  idTipoImpacto?: any;
  impacto?: string;
  cantidadImpacto?: number;
}

@Component({
  selector: 'app-consequences',
  standalone: true,
  imports: [
    ReactiveFormsModule,
    MatFormFieldModule,
    MatDatepickerModule,
    MatNativeDateModule,
    CommonModule,
    MatInputModule,
    FlexLayoutModule,
    MatGridListModule,
    MatButtonModule,
    MatSelectModule,
    MatTableModule,
    MatIconModule,
    NgxSpinnerModule,
    MatAutocompleteModule,
    MatTooltipModule,
  ],
  providers: [
    { provide: DateAdapter, useClass: NativeDateAdapter },
    { provide: MAT_DATE_FORMATS, useValue: FORMATO_FECHA },
  ],
  templateUrl: './consequences.component.html',
  styleUrl: './consequences.component.scss',
})
export class ConsequencesComponent implements OnInit {
  @ViewChild(MatSort) sort!: MatSort;
  data = inject(MAT_DIALOG_DATA) as { title: string; idIncendio: number; municipio: any };
  @Output() save = new EventEmitter<any>();
  @Input() dataProp: any;
  @Input() editData: any;
  @Input() esUltimo: boolean | undefined;
  @Input() fire: any;
  @Input() isNewRecord: boolean | undefined;
  @Input() registroId: number | null = null;
  @Output() changesMade = new EventEmitter<boolean>();
  @Output() hasUnsavedChanges = new EventEmitter<boolean>();

  public evolutionService = inject(EvolutionService);
  public toast = inject(MatSnackBar);
  private provinceService = inject(ProvinceService);
  private fb = inject(FormBuilder);
  public matDialog = inject(MatDialog);
  private spinner = inject(NgxSpinnerService);
  private municipalityService = inject(MunicipalityService);
  private tiposImpactoService = inject(ImpactTypeService);
  private consecuenciaService = inject(ConsequenceService);
  public utilsService = inject(UtilsService);
  public alertService = inject(AlertService);
  private recordsService = inject(RecordsService);

  public displayedColumns: string[] = ['tipo', 'estimado', 'total', 'desglose', 'opciones'];
  public displayedColumnsImpactos: string[] = ['descripcion', 'cantidad', 'informacionComplementaria', 'placeholder', 'opciones'];

  formData: FormGroup = this.fb.group({});
  formDataComplementarios!: FormGroup;
  formDataDinamico!: FormGroup;

  public provinces = signal<Province[]>([]);
  public municipalities = signal<Municipality[]>([]);
  public listadoTipoImpacto = signal<any[]>([]);
  public listadoDescripciones = signal<any[]>([]);
  public camposDinamicos = signal<any[]>([]);
  public opcionesCamposSelect: { [key: string]: any[] } = {};
  public showForm = signal(false);
  public formReady = signal(false);

  public selectedTipoImpacto = signal<any>(null);
  public editingContext = signal<{ group: any; evolution: any } | null>(null);

  private isEditing = false;

  constructor() {
    // effect(() => {
    //   if (this.formReady() && this.tipoImpactos().length === 0 && !this.showForm()) {
    //     this.prepararNuevoTipoImpacto();
    //   }
    // }, { allowSignalWrites: true });
  }

  public tipoImpactos = computed(() => {
    const flatData = this.evolutionService.dataConse();
    const groups: { [key: string]: any[] } = {};

    for (const conse of flatData) {
      const groupId = conse.tipo?.id;
      if (groupId) {
        if (!groups[groupId]) {
          groups[groupId] = [];
        }
        groups[groupId].push(conse);
      }
    }

    return Object.values(groups).map((groupEvolutions) => {
      const firstEvo = groupEvolutions[0];

      // Lógica para generar el desglose
      const desgloseMap = new Map<string, number>();
      groupEvolutions.forEach(evo => {
        const desc = evo.descripcion?.descripcion?.trim();
        if (desc) {
          desgloseMap.set(desc, (desgloseMap.get(desc) || 0) + (Number(evo.cantidad) || 0));
        }
      });

      const desgloseString = Array.from(desgloseMap.entries())
        .map(([desc, total]) => `${desc}: ${total}`)
        .join(' ; ');

      return {
        id: firstEvo.tipo.id,
        tipo: firstEvo.tipo,
        estimado: firstEvo.estimado,
        total: firstEvo.total,
        desglose: desgloseString,
        ImpactosEvoluciones: groupEvolutions,
      };
    });
  });

  public dataSourceTipoImpactos = computed(() => {
    const selected = this.selectedTipoImpacto();
    const allTipos = this.tipoImpactos();
    
    if (selected) {
      return [selected];
    } else {
      return allTipos;
    }
  });

  public impactosEvoluciones = computed(() => {
    const evolutions = this.selectedTipoImpacto()?.ImpactosEvoluciones || [];
    // Filtramos los placeholders para que no se muestren en la tabla de desglose
    return evolutions.filter((evo: any) => !evo._isPlaceholder);
  });

  public dataSourceImpactosEvoluciones = computed(() => {
    const context = this.editingContext();
    const allImpactos = this.impactosEvoluciones();
    if (context && context.evolution) {
      return [context.evolution];
    }
    return allImpactos;
  });

  filteredTipoOptions: Observable<any[]> = of([]);
  filteredDescripciones: Observable<any[]> = of([]);
  provincefilteredOptions: Observable<any[]> = of([]);
  municipalityfilteredOptions: Observable<any[]> = of([]);
  noResults: boolean = false;

  private getCurrentDateTimeLocal(): string {
    return moment().format('YYYY-MM-DDTHH:mm');
  }

  async ngOnInit() {
    this.spinner.show();
    this.formDataDinamico = this.fb.group({});

    const listadoTipoImpacto: any = await this.tiposImpactoService.get();
    this.listadoTipoImpacto.set(listadoTipoImpacto.items || listadoTipoImpacto);

    try {
      const provinces = await this.provinceService.get();
      this.provinces.set(provinces);
    } catch (error) {
      console.error('Error cargando provincias:', error);
      this.toast.open('Error al cargar las provincias', 'Cerrar', { duration: 3000 });
    }

    this.buildForm();
    this.formReady.set(true);
    await this.cargarDescripciones();
    this.initAutocompleteFilters();

 
      try {
        const registroData = await this.recordsService.getById(Number(this.registroId));
        let datosAProcesar = registroData?.tipoImpactosEvoluciones;

        if (datosAProcesar?.[0]?.tipoImpactosEvoluciones?.length > 0) {
          datosAProcesar = datosAProcesar[0].tipoImpactosEvoluciones;
        }
        console.log("🚀 ~ ConsequencesComponent ~ ngOnInit ~ datosAProcesar:", datosAProcesar)

        if (!datosAProcesar || datosAProcesar.length === 0) {
            const registrosAnteriores = await this.recordsService.getRegistrosAnteriores(Number(this.fire.idSuceso), this.registroId!);
            if (registrosAnteriores?.[0]?.tipoImpactosEvoluciones?.length > 0) {
                datosAProcesar = registrosAnteriores[0].tipoImpactosEvoluciones;
            }
        }
        
        if (datosAProcesar && datosAProcesar.length > 0) {
          
            const processGroup = (group: any) => {
              // SI EL GRUPO NO TIENE EVOLUCIONES, CREAMOS UN PLACEHOLDER
              if (!group.impactosEvoluciones || group.impactosEvoluciones.length === 0) {
                return [Promise.resolve({
                  _isPlaceholder: true,
                  id: `placeholder_${group.id}`,
                  tipo: group.tipoImpacto,
                  estimado: group.estimado,
                  total: group.total || 0,
                  descripcion: null,
                  cantidad: null,
                  observaciones: null,
                  _camposDinamicos: []
                })];
              }
              
              return group.impactosEvoluciones.map(async (evolution: any) => {
                const flatRecord: any = {
                  ...evolution,
                  id: evolution.id,
                  tipo: group.tipoImpacto,
                  descripcion: evolution.impactoClasificado,
                  cantidad: evolution.numero,
                  estimado: group.estimado,
                  total: group.total,
                  _camposDinamicos: [],
                  esModificado: evolution.esModificado,
                };
                
                // Normalizar los campos dinámicos (convertir primera letra a minúscula)
                if (evolution) {
                  Object.keys(evolution).forEach(key => {
                    // Si la clave comienza con mayúscula y no es un objeto o array
                    if (key.charAt(0) === key.charAt(0).toUpperCase() && 
                        key.charAt(0) !== key.charAt(0).toLowerCase() && 
                        typeof evolution[key] !== 'object') {
                      // Crear nueva clave con primera letra minúscula
                      const normalizedKey = key.charAt(0).toLowerCase() + key.slice(1);
                      flatRecord[normalizedKey] = evolution[key];
                    }
                  });
                }
                
                if (evolution.impactoClasificado && evolution.impactoClasificado.id) {
                  try {
                    const campos = await this.consecuenciaService.getValidacionImpactoClasificado(evolution.impactoClasificado.id);
                    flatRecord._camposDinamicos = campos;
                  } catch (error) {
                    console.error(`Error cargando campos dinámicos para impacto ${evolution.impactoClasificado.id}`, error);
                  }
                }
                delete flatRecord.numero; 
                return flatRecord;
              });
            };

            const allEvolutionsPromises = datosAProcesar.flatMap(processGroup);
            const allEvolutionsFlat = await Promise.all(allEvolutionsPromises);
            this.evolutionService.dataConse.set(allEvolutionsFlat.flat());
        }
      } catch (error) {
          console.error('Error al cargar los datos de consecuencias:', error);
          this.toast.open('Error al cargar los datos de consecuencias', 'Cerrar', { duration: 3000 });
      }
    
    
    // Si tenemos datos del incendio, pre-cargamos la provincia y el municipio.

    if(this.esTerritorioNacional()){

    if (this.fire && this.fire.provincia && this.fire.municipio) {
      const provinciaObj = this.provinces().find((p) => p.id === this.fire.provincia.id);
      if (provinciaObj) {
        this.formData.get('provincia')?.setValue(provinciaObj);
        
        const municipalities = await this.municipalityService.get(this.fire.provincia.id);
        this.municipalities.set(municipalities);
        this.formData.get('municipio')?.enable();

        const municipioObj = municipalities.find((m) => m.id === this.fire.municipio.id);
        if (municipioObj) {
          this.formData.get('municipio')?.setValue(municipioObj);
        }
      }
    }
  }

    this.spinner.hide();
  }

  buildForm() {
    this.formData = this.fb.group({
      tipo: [null, Validators.required],
      estimado: [null, [ Validators.min(0)]],
      total: [{ value: 0, disabled: true }, [Validators.required, Validators.min(0)]],
      descripcion: [null, Validators.required],
      cantidad: [null, [Validators.required, Validators.min(0)]],
      provincia: [null],
      municipio: [null],
      observaciones: [''],
    });

    // Mantenemos la suscripción para actualizar el total, pero sin emitir eventos
    this.formData.get('cantidad')?.valueChanges.subscribe(nuevaCantidad => {
      this.recalculateTotalInForm(nuevaCantidad);
    });

    const provinciaControl = this.formData.get('provincia');
    const municipioControl = this.formData.get('municipio');

    if (this.esTerritorioNacional()){
      // Los campos provincia y municipio no son obligatorios
      provinciaControl?.clearValidators();
      municipioControl?.clearValidators();
      provinciaControl?.updateValueAndValidity();
      municipioControl?.updateValueAndValidity();
      this.formData.get('municipio')?.disable();
    }else{
      provinciaControl?.clearValidators();
      municipioControl?.clearValidators();
      provinciaControl?.updateValueAndValidity();
      municipioControl?.updateValueAndValidity();
    }
  }

  recalculateTotalInForm(nuevaCantidad: number) {
    const context = this.editingContext();
    if (!context || !context.group) {
      this.formData.patchValue({ total: nuevaCantidad || 0 });
      return;
    }

    const tipoId = context.group.id;
    const evolutionsInGroup = this.evolutionService.dataConse().filter(e => e.tipo.id === tipoId);
    
    // Excluimos la evolución que estamos editando actualmente para no contar su valor antiguo.
    const otherEvolutions = context.evolution 
      ? evolutionsInGroup.filter(e => e !== context.evolution) 
      : evolutionsInGroup;

    const sumOfOthers = otherEvolutions.reduce((sum, evo) => sum + (Number(evo.cantidad) || 0), 0);
    const newTotal = sumOfOthers + (Number(nuevaCantidad) || 0);

    this.formData.patchValue({ total: newTotal });
  }

  initAutocompleteFilters() {
    this.initFilteredTipoOptions();
    this.initFilteredDescripciones();

    if (this.esTerritorioNacional()){
      this.initProvinceFilter();
      this.initMunicipalityFilter();
    }
  }

  prepararNuevoTipoImpacto() {
    this.showForm.set(true);
    this.selectedTipoImpacto.set(null);
    this.editingContext.set({ group: null, evolution: null });
    this.resetAndPrepareForm();
    this.formData.get('tipo')?.enable();
    this.formData.get('estimado')?.enable();

    // Si tenemos datos del incendio, los aplicamos después de resetear el formulario.
    if (this.fire && this.fire.provincia && this.fire.municipio) {
      this.setDefaultLocation();
    }

    // No emitimos hasUnsavedChanges para que el botón Guardar no se habilite
    this.changesMade.emit(false);
  }

  async setDefaultLocation() {
  
    if (this.esTerritorioNacional()){
 
    const provinciaObj = this.provinces().find((p) => p.id === this.fire.provincia.id);
    if (provinciaObj) {
      this.formData.get('provincia')?.setValue(provinciaObj);

      const municipalities = await this.municipalityService.get(this.fire.provincia.id);
          this.municipalities.set(municipalities);
      this.formData.get('municipio')?.enable();

      const municipioObj = municipalities.find((m) => m.id === this.fire.municipio.id);
      if (municipioObj) {
        this.formData.get('municipio')?.setValue(municipioObj);
      }
    }
  }
  }

  prepararNuevoImpactoEvolucion() {
    const group = this.selectedTipoImpacto();
    if (!group) return;

    this.showForm.set(true);
    this.editingContext.set({ group, evolution: null });
    this.resetAndPrepareForm();

    this.formData.patchValue({ 
      tipo: group.tipo,
      estimado: group.estimado
    });
    this.formData.get('tipo')?.disable();
    this.formData.get('estimado')?.enable();

    if (this.fire && this.fire.provincia && this.fire.municipio) {
      this.setDefaultLocation();
    }

    if (group.tipo?.id) {
      this.cargarDescripciones(group.tipo.id);
    }

    // No emitimos hasUnsavedChanges para que el botón Guardar no se habilite
    this.changesMade.emit(false);
  }

  async editarImpactoEvolucion(evolution: any) {
    const group = this.selectedTipoImpacto();
    if (!group) return;

    this.isEditing = true; // Prevenimos el reseteo de la descripción
    this.showForm.set(true);
    this.editingContext.set({ group, evolution });
    this.resetAndPrepareForm();

    // Carga los datos para los autocompletados
    if (evolution.tipo?.id) {
      await this.cargarDescripciones(evolution.tipo.id);
    }
    if (evolution.provincia?.id) {
      await this.onProvinceChange({ option: { value: evolution.provincia } }, false);
    }

    // Rellena el formulario principal
      this.formData.patchValue({
      ...evolution,
      tipo: group.tipo,
      estimado: group.estimado,
      total: group.total,
    });

    // Carga y rellena los campos dinámicos
    if (evolution.descripcion?.id) {
      await this.cargarCamposDinamicos({ value: evolution.descripcion }, false);
      this.formDataDinamico.patchValue(evolution);
    }

    // Ajusta estado y libera la bandera
    this.formData.get('tipo')?.disable();
    this.formData.get('estimado')?.enable();
    // No emitimos hasUnsavedChanges para que el botón Guardar no se habilite
    this.isEditing = false;
  }

  resetAndPrepareForm() {
    this.formData.reset();
    this.formDataDinamico = this.fb.group({});
    
    this.camposDinamicos.set([]);
    this.listadoDescripciones.set([]);
    this.municipalities.set([]);

    // Estado por defecto para un formulario nuevo
    this.formData.patchValue({ total: 0 });
    this.formData.get('total')?.disable();
    this.formData.get('municipio')?.disable();

    // No emitimos changesMade para que el botón Guardar no se habilite
  }

  cancelarEdicion() {
    this.showForm.set(false);
    this.editingContext.set(null);

    this.changesMade.emit(false);
    this.hasUnsavedChanges.emit(false);     
  }

  async onSubmit(formDirective: FormGroupDirective) {
    if (!this.formData.valid || !this.validarFormularioDinamico()) {
      this.formData.markAllAsTouched();
      this.formDataDinamico.markAllAsTouched();
      return;
    }

    const context = this.editingContext();
    if (!context) return;

    const formValue = this.formData.getRawValue();
    const dynamicData = this.formDataDinamico.value;
    const newEvolutionData = { 
      ...formValue, 
      ...dynamicData,
      _camposDinamicos: this.camposDinamicos() // Guardamos la metadata
    };
    const tipoId = newEvolutionData.tipo.id;

    this.evolutionService.dataConse.update(currentConse => {
      let evolutionsWithNewData: any[];

      if (context.evolution) {
        evolutionsWithNewData = currentConse.map(e => e === context.evolution ? newEvolutionData : e);
      } else {
        evolutionsWithNewData = [...currentConse, newEvolutionData];
      }

      const evolutionsInGroup = evolutionsWithNewData.filter(e => e.tipo.id === tipoId);
      const newTotal = evolutionsInGroup.reduce((sum, evo) => sum + (Number(evo.cantidad) || 0), 0);

      return evolutionsWithNewData.map(c => {
        if (c.tipo.id === tipoId) {
          return { ...c, estimado: newEvolutionData.estimado, total: newTotal };
        }
        return c;
      });
    });

    // After updating the data, we need to refresh the selected group
    // to reflect the changes in the UI without needing manual interaction.
    const groupId = context?.group?.id ?? newEvolutionData.tipo.id;
    const updatedGroup = this.tipoImpactos().find(g => g.id === groupId);

    if (updatedGroup) {
      this.selectedTipoImpacto.set(updatedGroup);
    }

    this.cancelarEdicion();
    this.hasUnsavedChanges.emit(false);
    // Emitimos changesMade para que el botón Guardar se habilite
    this.changesMade.emit(true);    
  }

  seleccionarTipoImpacto(tipoImpacto: any) {
    this.selectedTipoImpacto.set(this.selectedTipoImpacto() === tipoImpacto ? null : tipoImpacto);
    this.cancelarEdicion();
    // No emitimos ningún evento al seleccionar un tipo de impacto
  }

  async eliminarImpactoEvolucion(impactoAEliminar: any) {
    const tipoImpactoId = impactoAEliminar.tipo?.id;
    if (!tipoImpactoId) return;

    const evolucionesDelGrupo = this.evolutionService.dataConse().filter(c => c.tipo.id === tipoImpactoId);
    const esElUltimo = evolucionesDelGrupo.length === 1;

    const result = await this.alertService.showAlert({
      title: "¿Estás seguro de eliminar el dato?",
      text: esElUltimo
        ? 'Es el único dato de desglose para el tipo de consecuencia-actuación asociado. Al eliminarlo, se borrará también el tipo.'
        : 'Se procederá a eliminar el dato de desglose.',
      showCancelButton: true,
      confirmButtonText: '¡Sí, eliminar!',
      cancelButtonText: 'Cancelar',
      customClass: {
        title: 'sweetAlert-fsize20',
      },
    });

    if (result.isConfirmed) {
      if (esElUltimo) {
        this.evolutionService.dataConse.update(conse => 
          conse.filter(c => c.tipo.id !== tipoImpactoId)
        );
        if (this.selectedTipoImpacto()?.id === tipoImpactoId) {
          this.selectedTipoImpacto.set(null);
        }
      } else {
        this.evolutionService.dataConse.update(currentConse => {
          const withoutDeleted = currentConse.filter(c => c !== impactoAEliminar);
          
          const evolutionsInGroup = withoutDeleted.filter(c => c.tipo.id === tipoImpactoId);
          const newTotal = evolutionsInGroup.reduce((sum, evo) => sum + (Number(evo.cantidad) || 0), 0);

          return withoutDeleted.map(c => {
              if (c.tipo.id === tipoImpactoId) {
                  return { ...c, total: newTotal };
              }
              return c;
          });
        });

        // Forzamos la actualización de la selección para que la UI se refresque
        const updatedGroup = this.tipoImpactos().find(g => g.id === tipoImpactoId);
        if (updatedGroup) {
          this.selectedTipoImpacto.set(updatedGroup);
        }
      }

      this.hasUnsavedChanges.emit(false);
      // Emitimos changesMade para que el botón Guardar se habilite
      this.changesMade.emit(true);
      this.toast.open('Registro eliminado correctamente.', 'Cerrar', { duration: 3000 });
    }
  }
  
  public getPayload(): any[] {
    const groupedData = this.tipoImpactos(); // Usamos la signal computada que ya agrupa los datos.
    
    return groupedData.map(group => {
      const evoluciones = group.ImpactosEvoluciones.map((evo: any) => {
        // Mapeo de campos básicos del formulario
        const payloadEvo: any = {
          Id:  0,
          IdTipoImpactoEvolucion: group.id || 0, 
          IdImpactoClasificado: evo.descripcion?.id || null,
          Numero: evo.cantidad || 0,
          Observaciones: evo.observaciones || '',
          IdProvincia: evo.provincia?.id || null,
          IdMunicipio: evo.municipio?.id || null,
        };

        // Mapeo de campos dinámicos
        if (evo._camposDinamicos && evo._camposDinamicos.length > 0) {
          evo._camposDinamicos.forEach((campo: any) => {
            const key = campo.campo;
            if (evo.hasOwnProperty(key)) {
              // Aseguramos capitalización correcta para el backend
              const backendKey = key.charAt(0).toUpperCase() + key.slice(1);
              payloadEvo[backendKey] = evo[key];
            } else {
              // Intentar con la versión en minúscula
              const lowerKey = key.charAt(0).toLowerCase() + key.slice(1);
              if (evo.hasOwnProperty(lowerKey)) {
                const backendKey = key.charAt(0).toUpperCase() + key.slice(1);
                payloadEvo[backendKey] = evo[lowerKey];
              }
            }
          });
        }

        return payloadEvo;
      });

      return {
        Id:  0,
        IdTipoImpacto: group.tipo.id,
        estimado: group.estimado,
        ImpactosEvoluciones: evoluciones,
      };
    });
  }

  onFormGroupChange(formCamposComplementario: any) { this.formDataComplementarios = formCamposComplementario; }
  
  async cargarDescripciones(tipoId?: number) {
    // Si se hace clic sin tener un tipo seleccionado, se usa undefined para traer todo.
    const idTipoAUsar = tipoId ?? this.formData.get('tipo')?.value?.id;

    try {
      this.spinner.show();
      const descripciones: any = await this.consecuenciaService.getImpactosClasificados(idTipoAUsar);
      this.listadoDescripciones.set(descripciones);
    } catch (error) {
      console.error('Error al cargar descripciones:', error);
    } finally {
      this.spinner.hide();
    }
  }

  displayFn(option: any): string {
    return option && option.descripcion ? option.descripcion : '';
  }

  refreshDescripciones() {
    // Forzamos la re-evaluación del valueChanges para que el filtro se aplique de nuevo
    // al hacer clic en el input.
    const descripcionControl = this.formData.get('descripcion');
    if (descripcionControl) {
      descripcionControl.updateValueAndValidity({ onlySelf: true, emitEvent: true });
    }
  }

  private _normalizeValue(value: string): string { if (!value) return ''; return value.toLowerCase().replace(/\s/g, '');}
  
  initFilteredTipoOptions() {
    const tipoControl = this.formData.get('tipo');
    if (tipoControl) {
      this.filteredTipoOptions = tipoControl.valueChanges.pipe( startWith(''), map((value) => { const filterValue = this._normalizeValue(typeof value === 'string' ? value : value?.descripcion || ''); const filtered = this.listadoTipoImpacto().filter((option) => this._normalizeValue(option.descripcion || '').includes(filterValue)); this.noResults = filtered.length === 0; if (value && typeof value !== 'string' && value.id && !this.isEditing) { this.cargarDescripciones(value.id); this.formData.get('descripcion')?.setValue(null); this.camposDinamicos.set([]); this.formDataDinamico = this.fb.group({}); } else if (!value) { this.cargarDescripciones(); this.formData.get('descripcion')?.setValue(null); this.formData.get('descripcion')?.updateValueAndValidity(); this.camposDinamicos.set([]); this.formDataDinamico = this.fb.group({}); } return filtered; }) );
    }
  }
  
  initFilteredDescripciones() {
    const descripcionControl = this.formData.get('descripcion');
    if (descripcionControl) {
      this.filteredDescripciones = descripcionControl.valueChanges.pipe(
        startWith(''),
      map((value) => {
        const filterValue = this._normalizeValue(typeof value === 'string' ? value : value?.descripcion || '');
          return this.listadoDescripciones().filter((option) =>
            this._normalizeValue(option.descripcion || '').includes(filterValue)
          );
      })
    );
  }
  }

  displayProvince(province: any): string { return province && province.descripcion ? province.descripcion : ''; }
  displayMunicipality(municipality: any): string { return municipality && municipality.descripcion ? municipality.descripcion : ''; }
  async onProvinceChange(event: any, showSpinner = true) {
    const province = event.option.value;
    if (province && province.id) {
      if(showSpinner) this.spinner.show();
      try {
        if (this.esTerritorioNacional()) {
        const municipalities = await this.municipalityService.get(province.id);
        this.municipalities.set(municipalities);
        this.formData.get('municipio')?.enable();
        this.formData.patchValue({ municipio: null });
        }
      } catch (error) {
        console.error('Error al cargar municipios:', error);
      } finally {
        if(showSpinner) this.spinner.hide();
      }
    }
  }
  initProvinceFilter() {
    const provinciaControl = this.formData.get('provincia');
    if(provinciaControl) {
      this.provincefilteredOptions = provinciaControl.valueChanges.pipe(startWith(''), map(value => this.provinces().filter(option => this._normalizeValue(option.descripcion).includes(this._normalizeValue(typeof value === 'string' ? value : value?.descripcion))))); 
    }
  }
  initMunicipalityFilter() {
    const municipioControl = this.formData.get('municipio');
    if (municipioControl) {
      this.municipalityfilteredOptions = municipioControl.valueChanges.pipe(startWith(''), map(value => this.municipalities().filter(option => this._normalizeValue(option.descripcion).includes(this._normalizeValue(typeof value === 'string' ? value : value?.descripcion)))));
    }
  }
  async cargarCamposDinamicos(event: any, mostrarSpinner: boolean = true) {
    const descripcionSeleccionada = event.value;
    if (!descripcionSeleccionada || !descripcionSeleccionada.id) { this.camposDinamicos.set([]); return; }

    // Nueva funcionalidad: Autocompletar el campo 'Tipo'
    if (descripcionSeleccionada.tipoImpacto && descripcionSeleccionada.tipoImpacto.id) {
      const tipoControl = this.formData.get('tipo');
      const tipoImpactoObj = this.listadoTipoImpacto().find(t => t.id === descripcionSeleccionada.tipoImpacto.id);
      
      if (tipoControl && tipoImpactoObj) {
        // Establecemos el valor sin emitir el evento para evitar que valueChanges
        // reinicie el campo de descripción que acabamos de seleccionar.
        tipoControl.setValue(tipoImpactoObj, { emitEvent: false });
      }
    }

    if (mostrarSpinner) this.spinner.show();
    try {
      const campos = await this.consecuenciaService.getValidacionImpactoClasificado(descripcionSeleccionada.id);
      this.camposDinamicos.set(campos);
      this.crearControlesDinamicos(campos);
    } catch (error) {
      console.error('Error al cargar campos dinámicos:', error);
    } finally {
      if (mostrarSpinner) this.spinner.hide();
    }
  }
  crearControlesDinamicos(campos: any[]) {
    this.formDataDinamico = this.fb.group({});

    campos.forEach((campo) => {
      let defaultValue = null;
      const etiqueta = campo.etiqueta?.toLowerCase() || '';
      if (etiqueta === 'fecha y hora' || etiqueta === 'fecha y hora inicio') {
        defaultValue = this.getCurrentDateTimeLocal();
      }
      
      // Crear el control con la clave normalizada (primera letra minúscula)
      const normalizedKey = campo.campo.charAt(0).toLowerCase() + campo.campo.slice(1);
      this.formDataDinamico.addControl(
        normalizedKey,
        new FormControl(defaultValue, campo.esObligatorio ? [Validators.required] : [])
      );
    });
  }
  validarFormularioDinamico(): boolean {
    if (this.camposDinamicos().length === 0) return true;
    if (!this.formDataDinamico) return false;
    this.formDataDinamico.markAllAsTouched();
    return this.formDataDinamico.valid;
  }
  getForm(attr: string) { return this.formData.controls[attr]; }
  getDynamicFormControl(campo: string): FormControl { 
    // Normalizar la clave a primera letra minúscula
    const normalizedKey = campo.charAt(0).toLowerCase() + campo.slice(1);
    return this.formDataDinamico.get(normalizedKey) as FormControl; 
  }

  public generarInformacionComplementaria(impacto: any): string {
    if (!impacto || !impacto._camposDinamicos) {
      return '';
    }

    const infoValues: string[] = [];
    const camposFecha = impacto._camposDinamicos.filter((c: any) => c.tipoCampo === 'Datetime');

    for (const campo of camposFecha) {
      // Intentar con la clave original
      let valor = impacto[campo.campo];
      
      // Si no existe, intentar con la primera letra en minúscula
      if (valor === undefined || valor === null) {
        const lowerKey = campo.campo.charAt(0).toLowerCase() + campo.campo.slice(1);
        valor = impacto[lowerKey];
      }
      
      // Si sigue sin existir, intentar con la primera letra en mayúscula
      if (valor === undefined || valor === null) {
        const upperKey = campo.campo.charAt(0).toUpperCase() + campo.campo.slice(1);
        valor = impacto[upperKey];
      }

      if (valor) {
        const formattedDate = moment(valor).format('DD/MM/YYYY HH:mm');
        if (formattedDate !== 'Invalid date') {
          infoValues.push(`${campo.etiqueta}: ${formattedDate}`);
        }
      }
    }

    return infoValues.join(' ; ');
  }

esTerritorioNacional(): boolean {
  return this.fire.idTerritorio == 1;
  }
}
