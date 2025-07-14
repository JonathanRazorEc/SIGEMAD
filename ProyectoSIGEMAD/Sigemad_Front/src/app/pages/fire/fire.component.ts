import { CommonModule } from '@angular/common';
import { Component, EventEmitter, inject, OnDestroy, OnInit, Output, signal } from '@angular/core';
import { NavigationEnd, Router, RouterOutlet } from '@angular/router';
import { Subject } from 'rxjs';
import { takeUntil } from 'rxjs/operators';

import { COUNTRIES_ID, DEFAULT_PAGESIZE } from '@type/constants';
import moment from 'moment';
import { getStorageItem } from 'src/app/shared/utils/storage-utils';
import { FireService } from '../../services/fire.service';
import { LocalFiltrosIncendio } from '../../services/local-filtro-incendio.service';
import { ApiResponse } from '../../types/api-response.type';
import { Fire } from '../../types/fire.type';
import { FireFilterFormComponent } from './components/fire-filter-form/fire-filter-form.component';
import { FireTableComponent } from './components/fire-table/fire-table.component';


@Component({
  selector: 'app-fire',
  standalone: true,
  imports: [CommonModule, FireFilterFormComponent, FireTableComponent, RouterOutlet],
  templateUrl: './fire.component.html',
  styleUrl: './fire.component.scss',
})
export class FireComponent implements OnInit, OnDestroy {
  @Output() updateTable = new EventEmitter<any>();

  public filtros = signal<any>({});

  public isLoading = true;
  public refreshFilterForm = true;
  public childActivated = false;

  public page = 1;
  public pageSize = DEFAULT_PAGESIZE;

  public fires: ApiResponse<Fire[]> = {
    count: 0,
    page: 0,
    Page: 0,
    pageSize: 5,
    data: [],
    pageCount: 0,
  };

  // Flags para evitar múltiples llamadas
  private isLoadingData = false; // Controla si hay una solicitud en curso para cargar datos
  private isSubmittingFilters = false; // Controla si hay una solicitud en curso para enviar filtros
  private destroy$ = new Subject<void>();
  
  public fireService = inject(FireService);
  public filtrosIncendioService = inject(LocalFiltrosIncendio);
  private router = inject(Router);

  async ngOnInit() {
    this.filtros.set(this.filtrosIncendioService.getFilters());
    await this.loadData(this.page, this.pageSize); // Cargar datos iniciales

    // Suscribirse a los eventos de navegación
    this.router.events.pipe(
      takeUntil(this.destroy$)
    ).subscribe(async (event) => {
      if (event instanceof NavigationEnd && event.url === '/fire') {
        await this.loadData(this.page, this.pageSize);
      }
    });
  }

  ngOnDestroy() {
    this.destroy$.next();
    this.destroy$.complete();
  }

  public triggerSubmit = signal(false);

  async onPageChanged(event: { page: number; pageSize: number }) {
    const { page, pageSize } = event;
  
    // Evitar múltiples llamadas si ya hay una solicitud en curso
    if (this.isLoadingData) {
      return;
    }
  
    this.page = page;
    this.pageSize = pageSize;
  
    // Obtener los filtros actuales
    const filters = this.filtrosIncendioService.getFilters();
  
    // Formatear los filtros para enviarlos al backend
    const formattedFilters = {
      IdTerritorio: filters.territory,
      IdPais: filters.country,
      IdCcaa: filters.autonomousCommunity?.id || null,
      IdProvincia: filters.province?.id || null,
      IdEstadoSuceso: filters.eventStatus?.id || null,
      IdEstadoIncendio: filters.fireStatus?.id || null,
      IdSituacionEquivalente: filters.situationEquivalent?.id || null,
      IdSuperficieAfectada: filters.affectedArea?.id || null,
      IdMovimiento: filters.move,
      IdComparativoFecha: filters.between,
      FechaInicio: moment(filters.fechaInicio).format('YYYY-MM-DD'),
      FechaFin: moment(filters.fechaFin).format('YYYY-MM-DD'),
      denominacion: filters.name,
      search: filters.name,
      idClaseSuceso: filters.eventTypes,
    };
  
    // Cargar datos con los filtros y los nuevos parámetros de paginación
    await this.loadData(page, pageSize, formattedFilters);
  }
  
  async loadData(page: number, pageSize: number, filters: any = {}) {
    if (this.isLoadingData) {
      return; // Si ya hay una solicitud en curso, no hacer nada
    }
  
    this.isLoadingData = true; // Activar el flag de carga
    this.isLoading = true;
  
    try {

    const savedFilters = getStorageItem<any>('fire-filters');
    let initialFilters = this.filtros();

    if (savedFilters) {
      try {
        savedFilters.country = savedFilters.country !== '' ? Number(savedFilters.country) : COUNTRIES_ID.SPAIN;
        console.log('Filtros guardados:', savedFilters);
        initialFilters = this.formatFiltersLocalStorage(savedFilters);    
      } catch (e) {
        console.warn('No se pudo parsear filtros guardados', e);
      }
    }

    // Usar initialFilters solo si filters está vacío
    const effectiveFilters = (filters && Object.keys(filters).length > 0) ? filters : initialFilters;

    const response = await this.fireService.getOnMainPage(effectiveFilters, page, pageSize);

  
      // Actualizar los datos locales
      this.fires = response;
      this.fires.Page = response.Page;
      this.onFiresChange(response);
    } catch (error) {
      console.error('Error al cargar datos:', error);
    } finally {
      this.isLoadingData = false; // Desactivar el flag de carga
      this.isLoading = false;
    }
  }

  onFiresChange(fires: ApiResponse<Fire[]>): void {
      this.fires = fires;
  }

  async onSubmitFilters() {
    // Evitar múltiples llamadas si ya hay una solicitud en curso
    if (this.isSubmittingFilters || this.isLoadingData) {
      return;
    }
  
    this.isSubmittingFilters = true; // Activar el flag de envío de filtros
    this.isLoading = true;
  
    try {
      const filters = this.filtros(); // Obtener los filtros actuales
  
      // Formatear los filtros para enviarlos al backend
      const formattedFilters = {
        IdTerritorio: filters.territory,
        IdPais: filters.country,
        IdCcaa: filters.autonomousCommunity?.id || null,
        IdProvincia: filters.province?.id || null,
        IdEstadoSuceso: filters.eventStatus?.id || null,
        IdEstadoIncendio: filters.fireStatus?.id || null,
        IdSituacionEquivalente: filters.situationEquivalent?.id || null,
        IdSuperficieAfectada: filters.affectedArea?.id || null,
        IdMovimiento: filters.move,
        IdComparativoFecha: filters.between,
        FechaInicio: moment(filters.fechaInicio).format('YYYY-MM-DD'),
        FechaFin: moment(filters.fechaFin).format('YYYY-MM-DD'),
        denominacion: filters.name,
        search: filters.name,
        idClaseSuceso: filters.eventTypes,
      };
  
      // Llamar al servicio con los filtros y los parámetros de paginación
      const response = await this.fireService.get(formattedFilters, this.page, this.pageSize);
  
      // Actualizar los datos locales
      this.fires = response;
      this.onFiresChange(response);
    } catch (error) {
      console.error('Error al aplicar filtros:', error);
    } finally {
      this.isSubmittingFilters = false; // Desactivar el flag de envío de filtros
      this.isLoading = false;
    }
  }

formatFiltersLocalStorage(filters: any): any {
  return {
    IdTerritorio: filters.territory,
    IdPais: (filters.territory != 1 && filters.country == 60) ? null : filters.country,
    IdCcaa: filters.CCAA?.id || null,
    IdProvincia: filters.provincia?.id || null,
    IdEstadoSuceso: filters.eventStatus || null,
    IdEstadoIncendio: filters.fireStatus || null,
    IdSituacionEquivalente: filters.situationEquivalent || null,
    IdSuperficieAfectada: filters.affectedArea || null,
    IdMovimiento: filters.move,
    IdComparativoFecha: filters.between,
    FechaInicio: moment(filters.fechaInicio).format('YYYY-MM-DD'),
    FechaFin: moment(filters.fechaFin).format('YYYY-MM-DD'),
    denominacion: filters.name,
    search: filters.name,
    idClaseSuceso: filters.eventTypes,
  };
}

}
