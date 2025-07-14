import { Component, ElementRef, ViewChild, Renderer2, Input, inject, signal, CUSTOM_ELEMENTS_SCHEMA, Signal, AfterViewInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ReactiveFormsModule, FormsModule, FormBuilder, FormGroup, Validators, FormControl } from '@angular/forms';
import { MatButtonModule } from '@angular/material/button';
import { MatCardModule } from '@angular/material/card';
import { MatDatepickerModule } from '@angular/material/datepicker';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatIconModule } from '@angular/material/icon';
import { MatInputModule } from '@angular/material/input';
import { MatSelectModule } from '@angular/material/select';
import { MatNativeDateModule } from '@angular/material/core';
import { FlexLayoutModule } from '@angular/flex-layout';
import { MatMenuModule } from '@angular/material/menu';
import { MatAutocompleteModule, MatAutocompleteSelectedEvent } from '@angular/material/autocomplete';
import { MatChipsModule } from '@angular/material/chips';
import { MatTooltipModule } from '@angular/material/tooltip';
import { AreaComponent } from '../../fire-evolution-create/area/area.component';
import { InterventionComponent } from '../../fire-evolution-create/intervention/intervention.component';
import { ConsequencesComponent } from '../../fire-evolution-create/consequences/consequences.component';
import { RecordsComponent } from '../../fire-evolution-create/records/records.component';
import { MAT_DIALOG_DATA, MatDialogRef } from '@angular/material/dialog';
import { FireDetail } from '@type/fire-detail.type';
import { MasterDataEvolutionsService } from '../../../services/master-data-evolutions.service';
import { InputOutput } from '@type/input-output.type';
import { Media } from '@type/media.type';
import { OriginDestination } from '@type/origin-destination.type';
import { RecordsService } from '../../../services/records.service';
import { MatSnackBar } from '@angular/material/snack-bar';
import { MatProgressSpinnerModule } from '@angular/material/progress-spinner';
import { NgxSpinnerModule, NgxSpinnerService } from 'ngx-spinner';
import { EvolutionService } from '@services/evolution.service';
import { AlertService } from '../../../shared/alert/alert.service';
import { FECHA_MAXIMA_DATETIME, FECHA_MINIMA_DATETIME } from '@type/constants';
import { InterventionDataService } from '../../../services/intervention-data.service';
import { map, Observable, startWith } from 'rxjs';
import { MultiSelectAutocompleteComponent } from '../../../shared/components/multi-select-autocomplete/multi-select-autocomplete.component';
import moment from 'moment-timezone';
import { FechaValidator } from '@shared/validators/fecha-validator';
import { DirectionCoordinationComponent } from './direction-coordination/direction-coordination.component';
import { SpecialPlansActivationComponent } from './special-plans-activation/special-plans-activation.component';
import { DateUtils } from '@shared/utils/date-utils';
import { SystemsActivationComponent } from '../../fire-actions-relevant/systems-activation/systems-activation.component';
import { ActionsRelevantService } from '@services/actions-relevant.service';

interface MenuPosition {
  left: string;
  top: string;
}

@Component({
  selector: 'app-actions-records',
  standalone: true,
  imports: [
    CommonModule,
    FormsModule,
    ReactiveFormsModule,
    MatButtonModule,
    MatCardModule,
    MatDatepickerModule,
    MatFormFieldModule,
    MatIconModule,
    MatInputModule,
    MatSelectModule,
    MatNativeDateModule,
    FlexLayoutModule,
    MatMenuModule,
    MatAutocompleteModule,
    MatChipsModule,
    MultiSelectAutocompleteComponent,
    MatTooltipModule,
    AreaComponent,
    InterventionComponent,
    ConsequencesComponent,
    RecordsComponent,
    DirectionCoordinationComponent,
    MatProgressSpinnerModule,
    NgxSpinnerModule,
    SpecialPlansActivationComponent,
    SystemsActivationComponent
  ],
  templateUrl: './actions-records.component.html',
  styleUrls: ['./actions-records.component.scss'],
})
export class ActionsRecordsComponent implements AfterViewInit {
  @Input() fire: any = {
    // Datos mínimos para evitar el error
    provincia: { id: 1 },
    municipio: { id: 1 },
  };
  editData: any;

  showEvolutivosMenu = false;
  showCoordinacionMenu = false;
  showActuacionesMenu = false;
  showOtraMenu = false;
  showDocuMenu = false;
  showSucesosMenu = false;
  showConsolidadosMenu = false;
  activeIcon: string = '';
  selectedOption: string = '';
  menuPositions: { [key: string]: MenuPosition } = {};
  data = inject(MAT_DIALOG_DATA) as {
    title: string;
    idIncendio: number;
    fireDetail?: FireDetail;
    valoresDefecto?: number;
    fire?: any;
    idRegistroActualizacion?: number;
    isNewRecord?: boolean;
  };
  private dialogRef = inject(MatDialogRef<ActionsRecordsComponent>);
  private masterDataService = inject(MasterDataEvolutionsService);
  private fb = inject(FormBuilder);
  private recordsService = inject(RecordsService);
  private snackBar = inject(MatSnackBar);
  private spinner = inject(NgxSpinnerService);
  public evolutionSevice = inject(EvolutionService);
  private alertService = inject(AlertService);
  private actionsRelevantSevice = inject(ActionsRelevantService);

  // Señal para controlar el estado de carga
  public isLoading = signal<boolean>(false);

  // Variable para almacenar el ID del registro
  public registroId = signal<number | null>(null);

  // Signal para controlar la habilitación del botón Guardar
  public enableSaveButton = signal<boolean>(false);

  public registrosPosterioresConAreasAfectadas = signal<boolean>(false);

  // Usar signal para las opciones
  public inputOutputOptions = signal<InputOutput[]>([]);
  public mediaOptions = signal<Media[]>([]);
  public originDestinationOptions = signal<OriginDestination[]>([]);

  public mediofilteredOptions!: Observable<Media[]>;

  // Fecha y hora actual para el campo datetime-local
  public currentDateTime = signal<string>(this.getCurrentDateTimeString());

  // Formulario para los datos de registro
  public registerForm: FormGroup;
  public fechaMinimaDateTime = FECHA_MINIMA_DATETIME;
  public fechaMaximaDateTime = FECHA_MAXIMA_DATETIME;
  public dataMaestros: any = {};
  estado: number | undefined;
  private interventionDataService = inject(InterventionDataService);

  private hasMadeChanges: boolean = false;
  private hasUnsavedChanges: boolean = false;

  // ViewChild para el componente de dirección y coordinación
  @ViewChild(DirectionCoordinationComponent) directionCoordinationComponent!: DirectionCoordinationComponent;

  // ViewChild para el componente de activación de planes especiales
  @ViewChild(SpecialPlansActivationComponent) specialPlansActivationComponent!: SpecialPlansActivationComponent;

  // ViewChild para el componente de consecuencias
  @ViewChild(ConsequencesComponent) consequencesComponent!: ConsequencesComponent;

  // ViewChild para el componente de área
  @ViewChild('areaComponent', { static: false }) areaComponent: any;
  private areaComponentLoaded = false;

  constructor(private renderer: Renderer2) {
    // Cerrar menús al hacer clic fuera
    this.renderer.listen('window', 'click', (e: Event) => {
      const target = e.target as HTMLElement;
      if (!target.closest('.action-icon') && !target.closest('.menu-item')) {
        this.closeAllMenus();
      }
    });

    this.shortcut();

    // Inicializar el formulario
    this.registerForm = this.fb.group({
      idSuceso: [0],
      idRegistroActualizacion: [0],

      fechaHoraEvolucion: new FormControl(DateUtils.getCurrentCESTDate(), [
        Validators.required,
        FechaValidator.validarFechaPosteriorHoy(),
        FechaValidator.validarFecha,
      ]),

      idEntradaSalida: [null, Validators.required],
      idMedio: [null, Validators.required],
      registroProcedenciasDestinos: [[], Validators.required],
    });

    this.registerForm.get('idEntradaSalida')?.valueChanges.subscribe((value) => {
      if (value === 5) {
        this.registerForm.patchValue({
          registroProcedenciasDestinos: [181],
        });
      }
    });

    // Inicializar el botón Guardar como deshabilitado
    this.enableSaveButton.set(false);

    // Detectar cambios en el formulario principal
    this.registerForm.valueChanges.subscribe(() => {
      if (this.registerForm.dirty) {
        this.hasUnsavedChanges = true;
        // Habilitar el botón Guardar cuando hay cambios en los inputs de Datos del registro
        this.enableSaveButton.set(true);
      }
    });
  }

  ngAfterViewInit() {
    // Marcar el componente como cargado después de la inicialización de la vista
    setTimeout(() => {
      this.areaComponentLoaded = true;
      console.log('Vista inicializada, areaComponent disponible:', !!this.areaComponent);
    });
  }

  private shortcut() {
    this.renderer.listen('window', 'keydown', (event: KeyboardEvent) => {
      // Atajo de teclado [Alt+E] para abrir el menú evolutivos
      if (event.altKey && (event.key === 'e' || event.key === 'E') && this.registroId()) {
        event.preventDefault();
        // Busca el primer icono evolutivos habilitado
        const icons = document.querySelectorAll('.action-icon');
        for (const icon of Array.from(icons)) {
          if (!icon.classList.contains('disabled') && icon.querySelector('img[src*="evolutivos"]')) {
            const fakeEvent = { stopPropagation: () => {}, currentTarget: icon } as unknown as MouseEvent;
            this.setActiveIcon('evolutivos', fakeEvent);
            break;
          }
        }
      }

      // Atajo de teclado [Alt+C] para coordinación
      if (event.altKey && (event.key === 'c' || event.key === 'C') && this.registroId()) {
        event.preventDefault();
        const icons = document.querySelectorAll('.action-icon');
        for (const icon of Array.from(icons)) {
          if (!icon.classList.contains('disabled') && icon.querySelector('img[src*="coordinacion"]')) {
            const fakeEvent = { stopPropagation: () => {}, currentTarget: icon } as unknown as MouseEvent;
            this.setActiveIcon('coordinacion', fakeEvent);
            break;
          }
        }
      }

      // Atajo de teclado [Alt+R] para actuaciones relevantes
      if (event.altKey && (event.key === 'r' || event.key === 'R') && this.registroId()) {
        event.preventDefault();
        const icons = document.querySelectorAll('.action-icon');
        for (const icon of Array.from(icons)) {
          if (!icon.classList.contains('disabled') && icon.querySelector('img[src*="actuaciones"]')) {
            const fakeEvent = { stopPropagation: () => {}, currentTarget: icon } as unknown as MouseEvent;
            this.setActiveIcon('actuaciones', fakeEvent);
            break;
          }
        }
      }

      // Atajo de teclado [Alt+O] para otra información
      if (event.altKey && (event.key === 'o' || event.key === 'O') && this.registroId()) {
        event.preventDefault();
        const icons = document.querySelectorAll('.action-icon');
        for (const icon of Array.from(icons)) {
          if (!icon.classList.contains('disabled') && icon.querySelector('img[src*="otra"]')) {
            const fakeEvent = { stopPropagation: () => {}, currentTarget: icon } as unknown as MouseEvent;
            this.setActiveIcon('otra', fakeEvent);
            break;
          }
        }
      }

      // Atajo de teclado [Alt+D] para documentación
      if (event.altKey && (event.key === 'd' || event.key === 'D') && this.registroId()) {
        event.preventDefault();
        const icons = document.querySelectorAll('.action-icon');
        for (const icon of Array.from(icons)) {
          if (!icon.classList.contains('disabled') && icon.querySelector('img[src*="docu"]')) {
            const fakeEvent = { stopPropagation: () => {}, currentTarget: icon } as unknown as MouseEvent;
            this.setActiveIcon('docu', fakeEvent);
            break;
          }
        }
      }

      // Atajo de teclado [Alt+S] para sucesos relacionados
      if (event.altKey && (event.key === 's' || event.key === 'S') && this.registroId()) {
        event.preventDefault();
        const icons = document.querySelectorAll('.action-icon');
        for (const icon of Array.from(icons)) {
          if (!icon.classList.contains('disabled') && icon.querySelector('img[src*="sucesos"]')) {
            const fakeEvent = { stopPropagation: () => {}, currentTarget: icon } as unknown as MouseEvent;
            this.setActiveIcon('sucesos', fakeEvent);
            break;
          }
        }
      }

      // Atajo de teclado [Alt+N] para datos consolidados
      if (event.altKey && (event.key === 'n' || event.key === 'N') && this.registroId()) {
        event.preventDefault();
        const icons = document.querySelectorAll('.action-icon');
        for (const icon of Array.from(icons)) {
          if (!icon.classList.contains('disabled') && icon.querySelector('img[src*="consolidados"]')) {
            const fakeEvent = { stopPropagation: () => {}, currentTarget: icon } as unknown as MouseEvent;
            this.setActiveIcon('consolidados', fakeEvent);
            break;
          }
        }
      }

      // Atajos de teclado para menu Evolutivos cuando está abierto
      if (this.showEvolutivosMenu) {
        const key = event.key.toLowerCase();
        const keyToOption: { [k: string]: string } = {
          p: 'parametros',
          a: 'area',
          c: 'consecuencias',
          i: 'intervencion',
        };
        if (key in keyToOption) {
          event.preventDefault();
          this.handleMenuItemClick(keyToOption[key]);
        }
      }

      // Atajos de teclado para menu Actuaciones cuando está abierto
      if (this.showActuacionesMenu) {
        const key = event.key.toLowerCase();
        // Map shortcut keys to menu item indices (order as in HTML)
        const keyToIndex: { [k: string]: number } = {
          m: 0, // Movilización de Medios
          c: 1, // Convocatoria CECOD
          p: 2, // Planes estatales
          n: 3, // Notificaciones oficiales
          s: 4, // Sistemas
          z: 5, // Zagep
          o: 6, // Otras actuaciones
          e: 7, // Emergencia nacional
        };
        if (key in keyToIndex) {
          event.preventDefault();
          // Busca el menú abierto de actuaciones
          const menus = document.querySelectorAll('.action-icon .menu-desplegable');
          let menu: Element | null = null;
          menus.forEach((m) => {
            if (m && m.parentElement?.classList.contains('active')) {
              menu = m;
            }
          });
          if (!menu) {
            // fallback: busca el primero visible
            menu = Array.from(menus).find((m) => (m as HTMLElement).offsetParent !== null) || null;
          }
          if (menu) {
            const items = menu.querySelectorAll('.menu-item');
            const idx = keyToIndex[key];
            if (items[idx]) {
              (items[idx] as HTMLElement).click();
            }
          }
        }
      }

      // Atajos de teclado para menu Coordinacion cuando está abierto
      if (this.showCoordinacionMenu) {
        const key = event.key.toLowerCase();
        // D: Dirección y Coordinación, P: Planes especiales
        const keyToIndex: { [k: string]: number } = {
          d: 0,
          p: 1,
        };
        if (key in keyToIndex) {
          event.preventDefault();
          const menus = document.querySelectorAll('.action-icon .menu-desplegable');
          let menu: Element | null = null;
          menus.forEach((m) => {
            if (m && m.parentElement?.classList.contains('active')) {
              menu = m;
            }
          });
          if (!menu) {
            menu = Array.from(menus).find((m) => (m as HTMLElement).offsetParent !== null) || null;
          }
          if (menu) {
            const items = menu.querySelectorAll('.menu-item');
            const idx = keyToIndex[key];
            if (items[idx]) {
              (items[idx] as HTMLElement).click();
            }
          }
        }
      }

      // Atajos de teclado para menu Docu cuando está abierto
      if (this.showDocuMenu && (event.key === 'd' || event.key === 'D')) {
        event.preventDefault();
        const menus = document.querySelectorAll('.action-icon .menu-desplegable');
        let menu: Element | null = null;
        menus.forEach((m) => {
          if (m && m.parentElement?.classList.contains('active')) {
            menu = m;
          }
        });
        if (!menu) {
          menu = Array.from(menus).find((m) => (m as HTMLElement).offsetParent !== null) || null;
        }
        if (menu) {
          const items = menu.querySelectorAll('.menu-item');
          if (items[0]) {
            (items[0] as HTMLElement).click();
          }
        }
      }

      // Atajos de teclado  para menu Sucesos cuando está abierto
      if (this.showSucesosMenu && (event.key === 's' || event.key === 'S')) {
        event.preventDefault();
        const menus = document.querySelectorAll('.action-icon .menu-desplegable');
        let menu: Element | null = null;
        menus.forEach((m) => {
          if (m && m.parentElement?.classList.contains('active')) {
            menu = m;
          }
        });
        if (!menu) {
          menu = Array.from(menus).find((m) => (m as HTMLElement).offsetParent !== null) || null;
        }
        if (menu) {
          const items = menu.querySelectorAll('.menu-item');
          if (items[0]) {
            (items[0] as HTMLElement).click();
          }
        }
      }

      // Atajos de teclado para menu Otra está abierto
      if (this.showOtraMenu && (event.key === 'o' || event.key === 'O')) {
        event.preventDefault();
        const menus = document.querySelectorAll('.action-icon .menu-desplegable');
        let menu: Element | null = null;
        menus.forEach((m) => {
          if (m && m.parentElement?.classList.contains('active')) {
            menu = m;
          }
        });
        if (!menu) {
          menu = Array.from(menus).find((m) => (m as HTMLElement).offsetParent !== null) || null;
        }
        if (menu) {
          const items = menu.querySelectorAll('.menu-item');
          if (items[0]) {
            (items[0] as HTMLElement).click();
          }
        }
      }
      
      // Atajos de teclado para menu Consolidados cuando está abierto
      if (this.showConsolidadosMenu && (event.key === 'd' || event.key === 'D')) {
        event.preventDefault();
        const menus = document.querySelectorAll('.action-icon .menu-desplegable');
        let menu: Element | null = null;
        menus.forEach((m) => {
          if (m && m.parentElement?.classList.contains('active')) {
            menu = m;
          }
        });
        if (!menu) {
          menu = Array.from(menus).find((m) => (m as HTMLElement).offsetParent !== null) || null;
        }
        if (menu) {
          const items = menu.querySelectorAll('.menu-item');
          if (items[0]) {
            (items[0] as HTMLElement).click();
          }
        }
      }
    });
  }

  /**
   * Devuelve la fecha actual en la zona local del cliente (CEST en tu caso)
   * con formato "YYYY-MM-DDTHH:mm:ss.mmm" (sin sufijo "Z").
   * Si luego haces `.substring(0,16)` obtendrás "YYYY-MM-DDTHH:mm".
   */
  private getLocalIsoString(): string {
    const now = new Date();

    const pad2 = (n: number) => n.toString().padStart(2, '0');
    const pad3 = (n: number) => n.toString().padStart(3, '0');

    const year = now.getFullYear();
    const month = pad2(now.getMonth() + 1);
    const day = pad2(now.getDate());
    const hour = pad2(now.getHours());
    const minute = pad2(now.getMinutes());
    const second = pad2(now.getSeconds());
    const ms = pad3(now.getMilliseconds());

    // Ejemplo result: "2025-05-12T15:37:19.691"
    return `${year}-${month}-${day}T${hour}:${minute}:${second}.${ms}`;
  }

  // Método para obtener la fecha y hora actual en formato ISO para input datetime-local
  private getCurrentDateTimeString(): string {
    return this.getLocalIsoString().substring(0, 16);
  }

  async ngOnInit() {
    // Inicializar el estado del botón Guardar como deshabilitado
    this.enableSaveButton.set(false);
    
    this.spinner.show();
    console.log('🚀 ~ ActionsRecordsComponent ~ data=inject ~ data:', this.data);

    // Mostrar en consola si es nuevo registro o edición
    console.log('🚀 ~ ActionsRecordsComponent ~ Es nuevo registro:', this.data.isNewRecord);

    // Establecer el idSuceso al valor de data.idIncendio
    if (this.data && this.data.idIncendio) {
      this.registerForm.patchValue({
        idSuceso: this.data.idIncendio,
      });
    }

    try {
      const inputOutputData = await this.masterDataService.getInputOutput();
      this.inputOutputOptions.set(inputOutputData);

      const mediaData = await this.masterDataService.getMedia();
      this.mediaOptions.set(mediaData);

      this.mediofilteredOptions = this.registerForm.get('idMedio')!.valueChanges.pipe(
        startWith(''),
        map((value) => {
          if (typeof value === 'number') {
            const medioObj = mediaData.find((m) => m.id === value);
            return this._filterMedia(medioObj || '');
          }
          return this._filterMedia(value);
        })
      );

      const originDestinationData = await this.masterDataService.getOriginDestination();
      this.originDestinationOptions.set(originDestinationData);

      if (this.data && this.data.idRegistroActualizacion) {
        this.registroId.set(this.data.idRegistroActualizacion);
        this.registerForm.patchValue({
          idRegistroActualizacion: this.data.idRegistroActualizacion,
        });

        try {
          const registroData = await this.recordsService.getById(this.data.idRegistroActualizacion);
          console.log('🚀 ~ ActionsRecordsComponent ~ ngOnInit ~ registroData:', registroData);

          // PCD
          this.registroDataCargado.set(registroData);
          // FIN PCD

          if (registroData.registrosPosterioresConAreasAfectadas) {
            this.registrosPosterioresConAreasAfectadas.set(true);
            console.log(
              '🚀 ~ ActionsRecordsComponent ~ ngOnInit ~ registrosPosterioresConAreasAfectadas:',
              registroData.registrosPosterioresConAreasAfectadas
            );
          }

          let medioObj = null;
          if (registroData.medio?.id) {
            medioObj = mediaData.find((m) => m.id === registroData.medio.id);
          }

          this.registerForm.patchValue({
            fechaHoraEvolucion: DateUtils.fromUtcToCest(registroData.fechaHoraEvolucion),
            idEntradaSalida: registroData.entradaSalida?.id,
            idMedio: medioObj || registroData.medio?.id, // Usar el objeto completo si lo encontramos
          });

          if (registroData.procedenciaDestinos?.length) {
            this.registerForm.patchValue({
              registroProcedenciasDestinos: registroData.procedenciaDestinos.map((item: any) => item.id),
            });
          }

          if (registroData.activacionSistemas.length > 0) {
            this.actionsRelevantSevice.dataSistemas.set(registroData.activacionSistemas);
            console.table(this.actionsRelevantSevice.dataSistemas())
          }
        } catch (error) {
          console.error('Error al cargar datos del registro:', error);
          this.spinner.hide();
        }
      }

      await this.loadData();

      this.spinner.hide();
    } catch (error) {
      console.error('Error al cargar datos maestros:', error);
      this.spinner.hide();
    }
  }

  async loadData() {
    const tiposActivacion = await this.actionsRelevantSevice.getTipoActivacion();
    const modosActivacion = await this.actionsRelevantSevice.getModosActivacion();

    this.dataMaestros = {
      tiposActivacion,
      modosActivacion,
    };
  }

  getOriginDestinationName(id: number): string {
    const option = this.originDestinationOptions().find((o) => o.id === id);
    return option ? option.descripcion : '';
  }

  onOriginDestinationChange(selectedValues: any[]): void {
    console.log('Procedencia/Destino seleccionados:', selectedValues);
  }

  // Método para manejar el envío del formulario
  async onSubmit() {
    if (this.registerForm.valid) {
      this.spinner.show();
      this.isLoading.set(true);
      try {
        // 1) Clonamos el valor del formulario
        const formData: any = { ...this.registerForm.value };

        // 2) Convertimos la fecha local (CEST) a UTC ISO con Z
        if (formData.fechaHoraEvolucion) {
          console.log('🚀 ~ ActionsRecordsComponent ~ onSubmit ~ formData.fechaHoraEvolucion:', formData.fechaHoraEvolucion);
          formData.fechaHoraEvolucion = DateUtils.fromCestToUtc(formData.fechaHoraEvolucion);
          console.log('🚀 ~ ActionsRecordsComponent ~ onSubmit ~ formData.fechaHoraEvolucion:', formData.fechaHoraEvolucion);
        }

        // 3) Normalizamos el medio si viene como objeto
        if (formData.idMedio && typeof formData.idMedio === 'object') {
          formData.idMedio = formData.idMedio.id;
        }

        // Preparar los datos en el formato esperado por el endpoint
        const payloadData = {
          ...formData,
          // Mantener la misma estructura para creación y actualización
          idRegistroActualizacion: this.registroId() || 0,
          areaAfectadas: [],
          parametros: [],
          impactos: [],
        };

        console.log(payloadData);
        const cleanPayload = this.normalize(payloadData);
        console.log('🚀 ~ ActionsRecordsComponent ~ onSubmit ~ cleanPayload:', cleanPayload);

        const response = await this.recordsService.post(cleanPayload);

        if (this.registroId()) {
          // Solo procesar datos del componente actualmente seleccionado
          console.log('Procesando datos del componente seleccionado:', this.selectedOption);
          
          switch (this.selectedOption) {
            case 'parametros':
              if (this.evolutionSevice.dataRecords().parametro.length > 0) {
                // Caso normal: hay parámetros para guardar
                this.evolutionSevice.dataRecords().idRegistroActualizacion = this.registroId() || 0;
                const result: any = await this.evolutionSevice.postData(this.evolutionSevice.dataRecords());
              } else if (this.hasMadeChanges) {
                // Caso especial: no hay parámetros pero es porque se eliminaron todos los registros
                console.log('Enviando array vacío de parámetros para eliminar registros guardados');
                const emptyData = {
                  idSuceso: this.data.idIncendio,
                  idRegistroActualizacion: this.registroId() || 0,
                  parametro: []
                };
                const result: any = await this.evolutionSevice.postData(emptyData);
              }
              break;

            case 'area':
              if (this.evolutionSevice.dataAffectedArea().length > 0) {
                console.log('🚀 ~ ActionsRecordsComponent ~ onSubmit ~ this.evolutionSevice.dataAffectedArea():', this.evolutionSevice.dataAffectedArea());
                await this.handleDataProcessing(
                  this.evolutionSevice.dataAffectedArea(),
                  (item) => ({
                    id: item.id ?? 0,
                    fechaHora: item.fechaHora,
                    idProvincia: item.provincia.id ?? item.provincia,
                    idMunicipio: item.municipio.id ?? item.municipio,
                    idEntidadMenor: item.entidadMenor?.id ?? item.entidadMenor ?? null,
                    observaciones: item.observaciones,
                    GeoPosicion: this.isGeoPosicionValid(item) ? item.geoPosicion : null,
                    superficieAfectadaHectarea: item.superficieAfectadaHectarea,
                  }),
                  this.evolutionSevice.postAreas.bind(this.evolutionSevice),
                  'areasAfectadas'
                );
              }
              break;

            case 'intervencion':
              let interventionData = this.evolutionSevice.dataIntervention();
              if (interventionData) {
                interventionData.IdRegistroActualizacion = this.registroId() || 0;
                console.log('🚀 ~ ActionsRecordsComponent ~ onSubmit ~ interventionData:', interventionData);
                if (interventionData.Intervenciones && interventionData.Intervenciones.length > 0) {
                  const result = (await this.evolutionSevice.postIntervencion(interventionData)) as { idRegistroActualizacion: number };
                  console.log('🚀 ~ ActionsRecordsComponent ~ onSubmit ~ result:', result);
                }
              }
              break;

            case 'consecuencias':
              if (this.evolutionSevice.dataConse().length > 0) {
                const dataSave = {
                  idSuceso: this.data.idIncendio,
                  idRegistroActualizacion: this.registroId(),
                  TipoImpactos: this.consequencesComponent.getPayload(),
                };
                const result: any = await this.evolutionSevice.postConse(dataSave);
                console.info('result', result);
              }
              break;

            case 'direction-coordination':
              // Guardar datos de dirección y coordinación
              if (this.directionCoordinationComponent && this.directionCoordinationComponent.hasDataToSave()) {
                console.log('Guardando datos de dirección y coordinación...');
                await this.directionCoordinationComponent.saveAllData(this.data.idIncendio, this.registroId() || 0);
              }
              break;

            case 'special-plans-activation':
              // Guardar datos de activación de planes especiales
              if (this.specialPlansActivationComponent && this.specialPlansActivationComponent.hasDataToSave()) {
                console.log('Guardando datos de activación de planes especiales...');
                await this.specialPlansActivationComponent.saveAllData(this.data.idIncendio, this.registroId() || 0);
              }
              break;

            case 'systems-activation':
              if (this.actionsRelevantSevice.dataSistemas().length > 0) {
                const details = this.actionsRelevantSevice.dataSistemas().map(
                  (item) => ({
                    ...item,
                    fechaActivacion: item.fechaActivacion && DateUtils.formatDate(item.fechaActivacion, 'YYYY-MM-DD'),
                    fechaAceptacion: item.fechaAceptacion && DateUtils.formatDate(item.fechaAceptacion, 'YYYY-MM-DD'),
                  })
                );

                const req = {
                  idRegistroActualizacion: this.registroId() || 0,
                  idSuceso: this.data.idIncendio,
                  detalles: details,
                };

                await this.actionsRelevantSevice.postSystemsActivations(req);
              }
              break;

            default:
              console.log('No hay componente específico seleccionado o no requiere procesamiento especial');
              break;
          }
        } else {
          console.log('🚀 ~ ActionsRecordsComponent ~ onSubmit ~ Nuevo registro creado:', response);
          // Guardar el ID del registro en la variable global
          const nuevoRegistroId = (response as any).idRegistroActualizacion;
          this.registroId.set(nuevoRegistroId);
          console.log('Nuevo registro ID:', nuevoRegistroId);
          
          // Si es un nuevo registro, verificar si hay áreas existentes antes de generar automáticamente un registro
          try {
            // Verificar si es territorio nacional
            if (this.esTerritorioNacional()) {
              console.log('Es territorio nacional, verificando si se debe generar área automática...');
              
              // Verificar si ya existen registros de áreas
            const registroData = await this.recordsService.getById(Number(this.registroId()));
            console.log('Verificando si hay áreas existentes en el registro:', registroData);
            
            let tieneAreasExistentes = false;
            
            // Verificar si hay áreas en el registro actual
            if (registroData?.areaAfectadas && registroData.areaAfectadas.length > 0) {
              console.log('Se encontraron áreas en el registro actual:', registroData.areaAfectadas);
              tieneAreasExistentes = true;
      
            } else {
              // Si no hay áreas en el registro actual, buscar en registros anteriores
              const registrosAnteriores = await this.recordsService.getRegistrosAnteriores(Number(this.data.idIncendio), Number(this.registroId()));
              console.log('Verificando si hay áreas en registros anteriores:', registrosAnteriores);
              
              if (registrosAnteriores && registrosAnteriores.length > 0 && 
                  registrosAnteriores[0] && registrosAnteriores[0].areaAfectadas?.length > 0) {
                console.log('Se encontraron áreas en registros anteriores:', registrosAnteriores[0].areaAfectadas);
                tieneAreasExistentes = true;
             
              }
            }

            if (!tieneAreasExistentes) {
              // Primero cambiamos a la pestaña de área para asegurarnos de que el componente se cargue
              this.handleMenuItemClick('area');
            }

             
              
              // Esperar a que el componente de área se inicialice completamente
              // Usamos un intervalo para verificar periódicamente si el componente está disponible
              let intentos = 0;
              const maxIntentos = 10;
              const interval = setInterval(async () => {
                intentos++;
                console.log(`Intento ${intentos} de acceder al componente de área...`);
                
                if (this.areaComponent) {
                  clearInterval(interval);
                  console.log('Componente de área encontrado, procediendo con la generación automática');
                  await this.procesarAreaAutomatica();
                } else if (intentos >= maxIntentos) {
                  clearInterval(interval);
                  console.error(`No se pudo acceder al componente de área después de ${maxIntentos} intentos`);
                }
              }, 500); // Verificar cada 500ms
            } else {
              console.log('No es territorio nacional, omitiendo generación automática de áreas');
              // Actualizar registroDataCargado para reflejar el estado actual
              await this.getRecordData();
            }
          } catch (areaError) {
            console.error('Error al procesar el área:', areaError);
            // No lanzamos el error para que no interrumpa el flujo principal
          }
        }

        this.hasUnsavedChanges = false;
        this.hasMadeChanges = false;
        this.enableSaveButton.set(false);
        this.getRecordData();
        this.disabledDelete();

        this.spinner.hide();
        this.snackBar.open('Registro guardado correctamente', '', {
          duration: 3000,
          horizontalPosition: 'center',
          verticalPosition: 'bottom',
          panelClass: ['snackbar-verde'],
        });
      } catch (error) {
        this.spinner.hide();
        console.error('Error al guardar el registro:', error);

        const errorMsg =
          typeof error === 'object' && error !== null && 'Message' in error && (error as any).StatusCode === 400
            ? ' : ' + (error as any).Message
            : '';
        this.snackBar.open('Error al guardar el registro' + errorMsg, 'Cerrar', {
          duration: 5000,
          horizontalPosition: 'center',
          verticalPosition: 'top',
        });
      } finally {
        this.isLoading.set(false);
      }
    } else {
      console.log('Formulario inválido, por favor completa todos los campos requeridos');
      this.registerForm.markAllAsTouched();

      // Mostrar mensaje de error de validación
      this.snackBar.open('Por favor complete todos los campos requeridos', 'Cerrar', {
        duration: 3000,
        horizontalPosition: 'center',
        verticalPosition: 'top',
      });
    }
  }

  private normalize<T>(obj: T): any {
    if (Array.isArray(obj)) {
      return obj.map((o) => this.normalize(o));
    }

    if (obj !== null && typeof obj === 'object') {
      const keys = Object.keys(obj) as (keyof T)[];

      if (keys.length === 1 && keys[0] === 'id') {
        return (obj as any).id;
      }

      const out: any = {};
      for (const k of keys) {
        const value = (obj as any)[k];
        out[k as string] = this.normalize(value);
      }
      return out;
    }

    // Detecta "YYYY-MM-DDTHH:mm" y ajusta de CEST (UTC+2) a UTC
    // if (typeof obj === 'string' && /^\d{4}-\d{2}-\d{2}T\d{2}:\d{2}$/.test(obj)) {
    //   const localDate = new Date(obj); // Interpreta como local (CEST)
    //   if (!isNaN(localDate.getTime())) {
    //     localDate.setHours(localDate.getHours() - 2); // CEST → UTC

    //     const yyyy = localDate.getFullYear();
    //     const mm = String(localDate.getMonth() + 1).padStart(2, '0');
    //     const dd = String(localDate.getDate()).padStart(2, '0');
    //     const hh = String(localDate.getHours()).padStart(2, '0');
    //     const min = String(localDate.getMinutes()).padStart(2, '0');

    //     return `${yyyy}-${mm}-${dd}T${hh}:${min}`;
    //   }
    // }

    return obj;
  }

  async handleDataProcessing<T>(data: T[], formatter: (item: T) => any, postService: (body: any) => Promise<any>, key: string): Promise<void> {
    console.log('🚀 ~ ActionsRecordsComponent ~ handleDataProcessing ~ data:', data);
    if (data.length > 0) {
      const formattedData = data.map(formatter);
      console.log('🚀 ~ ActionsRecordsComponent ~ handleDataProcessing ~ formattedData:', formattedData);

      // Asegurarse de que el ID del registro está disponible
      const registroId = this.registroId();
      console.log('🚀 ~ ActionsRecordsComponent ~ handleDataProcessing ~ registroId:', registroId);
      
      if (!registroId) {
        console.error('Error: No hay ID de registro disponible para enviar los datos');
        return;
      }

      const body = {
        idSuceso: this.data.idIncendio,
        idRegistroActualizacion: registroId,
        [key]: formattedData,
      };
      console.log('🚀 ~ ActionsRecordsComponent ~ handleDataProcessing ~ body:', body);

      try {
        const result = await postService(body);
        console.log('🚀 ~ ActionsRecordsComponent ~ handleDataProcessing ~ result:', result);
      } catch (error) {
        console.error('Error al enviar datos al servidor:', error);
        throw error;
      }
    } else {
      console.warn('No hay datos para procesar');
    }
  }

  formatDate(date: Date | string): string {
    const d = new Date(date);
    const year = d.getFullYear();
    const month = (d.getMonth() + 1).toString().padStart(2, '0');
    const day = d.getDate().toString().padStart(2, '0');
    return `${year}-${month}-${day}`;
  }

  isGeoPosicionValid(data: any): boolean {
    const geoPosicion = data?.geoPosicion;

    if (!geoPosicion || geoPosicion.type !== 'Polygon' || !Array.isArray(geoPosicion.coordinates)) {
      return false;
    }

    return geoPosicion.coordinates.every(
      (polygon: any[]) =>
        Array.isArray(polygon) &&
        polygon.length > 0 &&
        polygon.every((point) => Array.isArray(point) && point.length === 2 && point.every((coord) => typeof coord === 'number'))
    );
  }

  setActiveIcon(iconName: string, event: MouseEvent) {
    event.stopPropagation();
    const targetElement = event.currentTarget as HTMLElement;

    if (this.activeIcon === iconName) {
      this.activeIcon = '';
      this.closeAllMenus();
      return;
    }

    this.activeIcon = iconName;
    this.closeAllMenusExcept(iconName);

    // Calcular la posición antes de mostrar el menú
    const rect = targetElement.getBoundingClientRect();

    // Valores iniciales de posición
    let left = rect.left;
    let top = rect.bottom + 5;

    // Ajustar si se sale por la derecha (asumimos un ancho aproximado del menú)
    const menuWidth = 200;
    if (left + menuWidth > window.innerWidth) {
      left = window.innerWidth - menuWidth - 10;
    }

    // Guardar la posición calculada
    this.menuPositions[iconName] = {
      left: `${left}px`,
      top: `${top}px`,
    };

    // Mostrar el menú correspondiente
    switch (iconName) {
      case 'evolutivos':
        this.showEvolutivosMenu = true;
        break;
      case 'coordinacion':
        this.showCoordinacionMenu = true;
        break;
      case 'actuaciones':
        this.showActuacionesMenu = true;
        break;
      case 'otra':
        this.showOtraMenu = true;
        break;
      case 'docu':
        this.showDocuMenu = true;
        break;
      case 'sucesos':
        this.showSucesosMenu = true;
        break;
      case 'consolidados':
        this.showConsolidadosMenu = true;
        break;
    }
  }

  selectOption(option: string) {
    console.log('🚀 ~ ActionsRecordsComponent ~ selectOption ~ option:', option);
    this.selectedOption = option;
    // No cerramos el menú inmediatamente para permitir que se procese el click
    setTimeout(() => this.closeAllMenus(), 100);
  }

  getMenuPosition(iconName: string): MenuPosition {
    return this.menuPositions[iconName] || { left: '0px', top: '0px' };
  }

  private closeAllMenusExcept(iconName: string) {
    if (iconName !== 'evolutivos') this.showEvolutivosMenu = false;
    if (iconName !== 'coordinacion') this.showCoordinacionMenu = false;
    if (iconName !== 'actuaciones') this.showActuacionesMenu = false;
    if (iconName !== 'otra') this.showOtraMenu = false;
    if (iconName !== 'docu') this.showDocuMenu = false;
    if (iconName !== 'sucesos') this.showSucesosMenu = false;
    if (iconName !== 'consolidados') this.showConsolidadosMenu = false;
  }

  private closeAllMenus() {
    this.showEvolutivosMenu = false;
    this.showCoordinacionMenu = false;
    this.showActuacionesMenu = false;
    this.showOtraMenu = false;
    this.showDocuMenu = false;
    this.showSucesosMenu = false;
    this.showConsolidadosMenu = false;
    this.menuPositions = {};
  }

  isActive(iconName: string): boolean {
    return this.activeIcon === iconName;
  }

  onSaveFromChild(event: any) {
    console.log('Datos guardados desde componente hijo:', event);
    // Si el evento tiene un ID de registro, actualizarlo
    if (event && event.registroId) {
      this.registroId.set(event.registroId);
      console.log('ID de registro actualizado:', this.registroId());
    }

    if (event && (event.save || event.update)) {
      this.hasUnsavedChanges = true;
      // No limpiar los datos después de guardar desde el componente hijo
    }
    
    // Actualizar el estado del botón Guardar
    if (this.selectedOption === 'parametros') {
      this.enableSaveButton.set(false);
    }
    
    this.snackBar.open('Datos guardados correctamente', '', {
      duration: 3000,
      horizontalPosition: 'center',
      verticalPosition: 'bottom',
      panelClass: ['snackbar-verde'],
    });
  }

  handleMenuItemClick(option: string) {
    // Cerrar todos los menús
    this.closeAllMenus();

    // No limpiar datos al cambiar de pestaña, solo se debe limpiar al salir del modal
    // Los datos deben persistir para poder volver a la pestaña y seguir editando

    // Establecer la opción seleccionada
    this.selectedOption = option;

    // Activar el icono correspondiente según la opción
    if (option === 'parametros' || option === 'area' || option === 'consecuencias' || option === 'intervencion') {
      this.activeIcon = 'evolutivos';
    } else if (option === 'direction-coordination' || option === 'special-plans-activation') {
      this.activeIcon = 'coordinacion';
    } else if (option === 'datos-consolidados') {
      this.activeIcon = 'consolidados';
    }

    // Resetear el estado del botón Guardar cuando se cambia de pestaña
    // El botón solo debe estar habilitado si hay cambios en el formulario principal
    // o si hay cambios en el componente al que se navega (esto se manejará en onChangesFromChild)
    this.enableSaveButton.set(false);

    // Emitir evento de cambios
    this.onChangesFromChild(false);
    this.onHasUnsavedChangesFromChild(false);
    
    // Si cambiamos a la pestaña de área y ya tenemos un registro ID, actualizar registroDataCargado
    if (option === 'area' && this.registroId()) {
      console.log('Cambiando a pestaña de área, actualizando registroDataCargado');
      setTimeout(() => {
        this.getRecordData();
      }, 500);
    }
  }

  onChangesFromChild(hasChanges: boolean) {
    this.hasMadeChanges = hasChanges;
    
    // Si hay cambios en algún componente hijo, habilitar el botón Guardar
    if (hasChanges) {
      this.enableSaveButton.set(true);
    } else if (this.selectedOption === 'parametros' || 
               this.selectedOption === 'intervencion' || 
               this.selectedOption === 'consecuencias' || 
               this.selectedOption === 'area' || 
               this.selectedOption === 'direction-coordination' || 
               this.selectedOption === 'special-plans-activation') {
      // Solo deshabilitamos el botón si estamos en uno de estos componentes y no hay cambios
      // Si estamos en la vista principal (sin componente seleccionado), el estado del botón depende
      // de si hay cambios en el formulario principal (gestionado por el valueChanges del formulario)
      this.enableSaveButton.set(false);
    }
  }

  onHasUnsavedChangesFromChild(hasUnsavedChanges: boolean) {
    this.hasUnsavedChanges = hasUnsavedChanges;
    
    // No habilitamos el botón Guardar aquí, solo lo hacemos en onChangesFromChild
    // cuando se presiona el botón "Agregar" o se elimina un registro
  }

  async closeModal(refresh?: boolean) {
    if (this.hasUnsavedChanges || this.hasMadeChanges) {
      const result = await this.alertService.showAlert({
        title: 'Cambios pendientes',
        text: 'Hay cambios pendientes que no se han guardado. Si continúa, perderá estos cambios.',
        icon: 'warning',
        showCancelButton: true,
        confirmButtonText: 'Sí, salir sin guardar',
        cancelButtonText: 'Cancelar',
      });

      if (!result.isConfirmed) {
        return;
      }
    }

    // Limpiar datos de evolución e intervención
    this.evolutionSevice.clearData();
    this.interventionDataService.clearInterventionData();

    // Limpiar datos de dirección y coordinación
    if (this.directionCoordinationComponent) {
      this.directionCoordinationComponent.clearAllData();
    }

    // Limpiar datos de activación de planes especiales
    if (this.specialPlansActivationComponent) {
      this.specialPlansActivationComponent.clearAllData();
    }

    this.dialogRef.close(refresh);
  }

  async delete() {
    if (this.hasUnsavedChanges) {
      const confirmResult = await this.alertService.showAlert({
        title: 'Cambios sin guardar',
        text: 'Hay cambios que no se han guardado. ¿Desea continuar con la eliminación sin guardar estos cambios?',
        icon: 'warning',
        showCancelButton: true,
        confirmButtonText: 'Sí, eliminar sin guardar',
        cancelButtonText: 'Cancelar',
      });

      if (!confirmResult.isConfirmed) {
        return;
      }
    }

    const toolbar = document.querySelector('mat-toolbar');
    this.renderer.setStyle(toolbar, 'z-index', '1');
    this.spinner.show();

    this.alertService
      // PCD
      .showAlert({
        title: '¿Estás seguro de eliminar el registro?',
        showCancelButton: true,
        cancelButtonColor: '#d33',
        confirmButtonText: '¡Sí, eliminar!',
        cancelButtonText: 'Cancelar',
        customClass: {
          title: 'sweetAlert-fsize20',
        },
      })
      // FIN PCD
      .then(async (result: any) => {
        if (result.isConfirmed) {
          try {
            await this.evolutionSevice.deleteConse(Number(this.registroId()));
            this.evolutionSevice.clearData();

            // Limpiar datos de dirección y coordinación
            if (this.directionCoordinationComponent) {
              this.directionCoordinationComponent.clearAllData();
            }

            // Limpiar datos de activación de planes especiales
            if (this.specialPlansActivationComponent) {
              this.specialPlansActivationComponent.clearAllData();
            }

            this.hasUnsavedChanges = false;

            // PCD
            this.snackBar
              .open('Registro eliminado correctamente!', '', {
                duration: 3000,
                horizontalPosition: 'center',
                verticalPosition: 'bottom',
                panelClass: ['snackbar-verde'],
              })
              .afterDismissed()
              .subscribe(() => {
                this.closeModal(true);
                this.spinner.hide();
              });

            // FIN PCD
          } catch (error) {
            this.spinner.hide();
            this.alertService
              .showAlert({
                title: 'No hemos podido eliminar la evolución',
                icon: 'error',
              })
              .then((result: any) => {
                this.closeModal();
              });
          }
        } else {
          this.spinner.hide();
        }
      });
  }

  private _filterMedia(value: any): Media[] {
    if (!value) return this.mediaOptions();

    const filterValue = typeof value === 'string' ? value.toLowerCase() : value.descripcion.toLowerCase();
    return this.mediaOptions().filter((medio) => medio.descripcion.toLowerCase().includes(filterValue));
  }

  displayMedio = (medio: Media): string => {
    return medio && medio.descripcion ? medio.descripcion : '';
  };

  onMedioSelected(event: MatAutocompleteSelectedEvent): void {
    const medioSeleccionado = event.option.value;
    console.log('Medio seleccionado:', medioSeleccionado);
  }

  // PCD
  registroDataCargado = signal<any>(null);
  tieneDatos<T>(...arrays: (T[] | null | undefined)[]): boolean {
    //console.log(JSON.stringify(arrays, null, 2));

    return arrays.some((arr) => Array.isArray(arr) && arr.length > 0);
  }

  // FIN PCD

  disabledDelete():boolean{
      if(this.data.isNewRecord && this.registroId() == undefined){
        return true;
      }else{
          return this.tieneDatos(
                        this.registroDataCargado()?.parametros,
                        this.registroDataCargado()?.areaAfectadas,
                        this.registroDataCargado()?.tipoImpactosEvoluciones,
                        this.registroDataCargado()?.intervencionMedios,
                        this.registroDataCargado()?.direcciones,
                        this.registroDataCargado()?.activacionPlanEmergencias,
                        this.registroDataCargado()?.coordinacionesPMA,
                        this.registroDataCargado()?.coordinacionesCecopi 
                      )
        }
 }
 
            
public async getRecordData(){
  if(this.registroId() != undefined || this.registroId()!= null){
    const registroData = await this.recordsService.getById(Number(this.registroId()));

    this.registroDataCargado.set(registroData);  
  }  
}

 esTerritorioNacional(): boolean {
  // Verificar que data.fire exista y tenga idTerritorio
  if (!this.data || !this.data.fire || this.data.fire.idTerritorio === undefined) {
    console.warn('No se pudo determinar si es territorio nacional: data.fire no está definido o no tiene idTerritorio');
    return false;
  }
  
  const esTerritorio = this.data.fire.idTerritorio == 1;
  // console.log(`Verificando si es territorio nacional: idTerritorio=${this.data.fire.idTerritorio}, resultado=${esTerritorio}`);
  return esTerritorio;
}

  // Método para procesar la generación automática de áreas
  private async procesarAreaAutomatica(): Promise<void> {
    try {
      // Verificar si el componente de área está disponible
      if (!this.areaComponent) {
        console.error('El componente de área no está disponible');
        
        // Si el componente aún no está cargado, esperamos un poco más
        if (this.areaComponentLoaded) {
          console.error('El componente debería estar cargado pero no se puede acceder a él');
        } else {
          console.log('Esperando a que el componente de área se cargue...');
          // Esperar un poco más y volver a intentar
          await new Promise(resolve => setTimeout(resolve, 1000));
          
          if (!this.areaComponent) {
            console.error('No se pudo acceder al componente de área después de esperar');
            return;
          }
        }
      }
      
      // Verificar si ya existen registros de áreas
      const registroData = await this.recordsService.getById(Number(this.registroId()));
      console.log('Verificando si hay áreas existentes en el registro:', registroData);
      
      let tieneAreasExistentes = false;
      
      // Verificar si hay áreas en el registro actual
      if (registroData?.areaAfectadas && registroData.areaAfectadas.length > 0) {
        console.log('Se encontraron áreas en el registro actual:', registroData.areaAfectadas);
        tieneAreasExistentes = true;
        this.evolutionSevice.dataAffectedArea.set(registroData.areaAfectadas);
      } else {
        // Si no hay áreas en el registro actual, buscar en registros anteriores
        const registrosAnteriores = await this.recordsService.getRegistrosAnteriores(Number(this.data.idIncendio), Number(this.registroId()));
        console.log('Verificando si hay áreas en registros anteriores:', registrosAnteriores);
        
        if (registrosAnteriores && registrosAnteriores.length > 0 && 
            registrosAnteriores[0] && registrosAnteriores[0].areaAfectadas?.length > 0) {
          console.log('Se encontraron áreas en registros anteriores:', registrosAnteriores[0].areaAfectadas);
          tieneAreasExistentes = true;
          this.evolutionSevice.dataAffectedArea.set(registrosAnteriores[0].areaAfectadas);
        }
      }
      
      // Solo generar registro automático si no hay áreas existentes
      if (!tieneAreasExistentes) {
        console.log('No se encontraron áreas existentes, generando registro automático...');
        
        // Asegurarse de que el array de áreas esté vacío antes de generar un nuevo registro
        this.evolutionSevice.dataAffectedArea.set([]);
        
        // Generar el registro automático
        console.log('Llamando a generarRegistroAutomatico en el componente de área...');
        await this.areaComponent.generarRegistroAutomatico();
        
        // Si hay datos en el área, enviarlos al servidor
        if (this.evolutionSevice.dataAffectedArea().length > 0) {
          console.log('Enviando datos de área al servidor...');
          console.log('Registro ID para área:', this.registroId());
          
          await this.handleDataProcessing(
            this.evolutionSevice.dataAffectedArea(),
            (item) => ({
              id: item.id ?? 0,
              fechaHora: item.fechaHora,
              idProvincia: item.provincia.id ?? item.provincia,
              idMunicipio: item.municipio.id ?? item.municipio,
              idEntidadMenor: item.entidadMenor?.id ?? item.entidadMenor ?? null,
              observaciones: item.observaciones,
              GeoPosicion: this.isGeoPosicionValid(item) ? item.geoPosicion : null,
              superficieAfectadaHectarea: item.superficieAfectadaHectarea,
            }),
            this.evolutionSevice.postAreas.bind(this.evolutionSevice),
            'areasAfectadas'
          );
        } else {
          console.error('No hay datos de área para enviar');
        }
      } else {
        console.log('Ya existen áreas, no se genera registro automático');
      }
      
      // Actualizar registroDataCargado para que se marque el menú con la clase tieneDatos
      await this.getRecordData();
      
      // Resetear las variables de cambios después de guardar el área
      this.hasUnsavedChanges = false;
      this.hasMadeChanges = false;
      
      // Actualizar la interfaz de usuario para reflejar que no hay cambios pendientes
      this.enableSaveButton.set(false);
      
      console.log('Proceso de área completado, no hay cambios pendientes');
    } catch (error) {
      console.error('Error en procesarAreaAutomatica:', error);
    }
  }

}


