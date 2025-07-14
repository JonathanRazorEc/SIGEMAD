import { CommonModule } from '@angular/common';
import { Component, Input, OnInit, inject, signal } from '@angular/core';
import { FormGroup, ReactiveFormsModule, FormsModule, FormControl } from '@angular/forms';
import { MatCardModule } from '@angular/material/card';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { MatSelectModule } from '@angular/material/select';
import { FlexLayoutModule } from '@angular/flex-layout';
import { Countries } from '../../../../../../types/country.type';
import { MatCheckboxModule } from '@angular/material/checkbox';
import { HttpClient } from '@angular/common/http';
import { firstValueFrom, Observable, of, catchError, debounceTime, startWith, switchMap, merge } from 'rxjs';
import { Province } from '../../../../../../types/province.type';
import { Municipality } from '../../../../../../types/municipality.type';
import { MatAutocompleteModule } from '@angular/material/autocomplete';
import { BaseItem } from '../../../../../../types/base-item.type';

// Interfaz para los municipios extranjeros
interface MunicipioExtranjero {
  id: number;
  idDistrito: number;
  descripcion: string;
  distrito?: {
    id: number;
    descripcion: string;
  };
  pais?: {
    id: number;
    descripcion: string;
    fronterizo: boolean;
  };
}

// Interfaz extendida para el distrito
interface Distrito {
  id: number;
  descripcion: string;
  nombre?: string; // Opcional para compatibilidad
  pais?: Countries;
}

// Versión simplificada para el autocompletado
interface SimpleCountry {
  id: number;
  descripcion: string;
  nombre?: string;
  x?: number;
  y?: number;
  geoPosicion?: any;
}

@Component({
  selector: 'app-fire-foreign-form',
  standalone: true,
  imports: [
    CommonModule,
    ReactiveFormsModule,
    FormsModule,
    MatCardModule,
    MatFormFieldModule,
    MatInputModule,
    MatSelectModule,
    FlexLayoutModule,
    MatCheckboxModule,
    MatAutocompleteModule
  ],
  templateUrl: './fire-foreign-form.component.html',
  styleUrls: ['./fire-foreign-form.component.scss']
})
export class FireForeignFormComponent implements OnInit {
  @Input() formData!: FormGroup;
  @Input() foreignCountries = signal<Countries[]>([]);
  @Input() listClassEvent = signal<any[]>([]);
  @Input() listEventStatus = signal<any[]>([]);
  @Input() fechaMinimaDateTime!: string;
  @Input() fechaMaximaDateTime!: string;
  @Input() skipFirstFields: boolean = false;
  
  foreignMunicipalities = signal<MunicipioExtranjero[]>([]);
  showForeignMunicipalities = false;
  foreignMunicipalitiesFilteredOptions!: Observable<MunicipioExtranjero[]>;
  
  distritos = signal<Distrito[]>([]);
  showDistritos = false; 
  distritosfilteredOptions!: Observable<Distrito[]>;
  
  showLimitSpainOption = false;
  provincefilteredOptions!: Observable<Province[]>;
  municipalityfilteredOptions!: Observable<Municipality[]>;
  
  countryfilteredOptions!: Observable<Countries[]>;
  
  private readonly FRANCIA_ID = 65;
  private readonly PORTUGAL_ID = 1;
  
  // Bandera para indicar que el país se actualizó desde el distrito
  private isCountryUpdatedFromDistrito = false;
  
  // Bandera para evitar resetear municipio al cambiar país/distrito
  private isMunicipioBeingUpdated = false;
  
  private http = inject(HttpClient);
  
  ngOnInit() {
    this.showLimitSpainOption = false;
    this.showDistritos = false;
    
    if (!this.formData.get('idMunicipioExtranjero')) {
      this.formData.addControl('idMunicipioExtranjero', new FormControl(null));
    }
    
    if (!this.formData.get('ubicacion')) {
      this.formData.addControl('ubicacion', new FormControl(null));
    }
    
    if (!this.formData.get('distrito')) {
      this.formData.addControl('distrito', new FormControl(null));
    }
    
    // Deshabilitar el FormControl de distrito por defecto
    this.formData.get('distrito')?.disable();
    
    if (!this.formData.get('provinciaLimitrofe')) {
      this.formData.addControl('provinciaLimitrofe', new FormControl(null));
    }
    
    if (!this.formData.get('municipioLimitrofe')) {
      this.formData.addControl('municipioLimitrofe', new FormControl(null));
    }
    
    // Deshabilitar los controles de provincia y municipio por defecto
    this.formData.get('provinciaLimitrofe')?.disable();
    this.formData.get('municipioLimitrofe')?.disable();
    
    this.formData.get('limitSpain')?.setValue(false);
    this.formData.get('limitSpain')?.disable();
    
    // Suscribirse a cambios en el checkbox limitSpain para habilitar/deshabilitar los campos
    this.formData.get('limitSpain')?.valueChanges.subscribe(isChecked => {
      if (isChecked) {
        this.formData.get('provinciaLimitrofe')?.enable();
        this.formData.get('municipioLimitrofe')?.enable(); // Habilitar municipio junto con provincia
      } else {
        this.formData.get('provinciaLimitrofe')?.disable();
        this.formData.get('municipioLimitrofe')?.disable();
        
        // Limpiar los valores
        this.formData.get('provinciaLimitrofe')?.setValue(null);
        this.formData.get('municipioLimitrofe')?.setValue(null);
      }
      
      // Actualizar la denominación según si es limítrofe o no
      this.actualizarDenominacion();
    });
    
    // Inicializar el autocomplete para distritos
    this.initializeDistritoAutocomplete();
    
    // Inicializar el autocomplete para municipios extranjeros
    this.initializeForeignMunicipalityAutocomplete();
    
    // Suscribirse a cambios en el municipio extranjero
    this.formData.get('idMunicipioExtranjero')?.valueChanges.subscribe(municipio => {
      if (municipio && typeof municipio === 'object') {
        // Activar bandera para evitar que se resetee municipio durante actualizaciones
        this.isMunicipioBeingUpdated = true;
        
        // Si el municipio tiene información del distrito, actualizar el campo distrito
        if (municipio.distrito) {
          const distritoObj = {
            id: municipio.distrito.id,
            descripcion: municipio.distrito.descripcion,
            nombre: municipio.distrito.descripcion, // Usar descripción como nombre también
            pais: municipio.distrito.pais // Incluir la información del país en el distrito
          };
          
          // Si el distrito actual es diferente o no hay distrito, actualizar
          const currentDistrito = this.formData.get('distrito')?.value;
          const currentDistritoId = currentDistrito && typeof currentDistrito === 'object' ? currentDistrito.id : currentDistrito;
          
          if (!currentDistritoId || currentDistritoId !== municipio.distrito.id) {
            this.formData.get('distrito')?.setValue(distritoObj);
          }

          
          
          // También actualizar el país si está disponible dentro del distrito
          if (municipio.distrito.pais) {
            // Buscar el país completo en la lista de países
            const paisCompleto = this.foreignCountries().find(c => c.id === municipio.distrito.pais.id);
            
            if (paisCompleto) {
              // Si el país actual es diferente o no hay país, actualizar
              const currentCountry = this.formData.get('country')?.value;
              const currentCountryId = currentCountry && typeof currentCountry === 'object' ? currentCountry.id : currentCountry;
              
              if (!currentCountryId || currentCountryId !== municipio.distrito.pais.id) {
                this.isCountryUpdatedFromDistrito = true; // Usar la misma bandera para evitar reseteos
                this.formData.get('country')?.setValue(paisCompleto);
                
                // Actualizar la UI según el país
                if (paisCompleto.id === this.FRANCIA_ID || paisCompleto.id === this.PORTUGAL_ID) {
                  this.showForeignMunicipalities = true;
                  this.showLimitSpainOption = true;
                  this.showDistritos = true;
                  this.formData.get('limitSpain')?.enable();
                  this.formData.get('distrito')?.enable();
                }
              }
            }
          }
        } else if (municipio.pais) {
          // Si el municipio tiene información directa del país (caso alternativo)
          // Buscar el país completo en la lista de países
          const paisCompleto = this.foreignCountries().find(c => c.id === municipio.pais?.id);
          
          if (paisCompleto) {
            // Si el país actual es diferente o no hay país, actualizar
            const currentCountry = this.formData.get('country')?.value;
            const currentCountryId = currentCountry && typeof currentCountry === 'object' ? currentCountry.id : currentCountry;
            
            if (!currentCountryId || currentCountryId !== municipio.pais.id) {
              this.isCountryUpdatedFromDistrito = true; // Usar la misma bandera para evitar reseteos
              this.formData.get('country')?.setValue(paisCompleto);
              
              // Actualizar la UI según el país
              if (paisCompleto.id === this.FRANCIA_ID || paisCompleto.id === this.PORTUGAL_ID) {
                this.showForeignMunicipalities = true;
                this.showLimitSpainOption = true;
                this.formData.get('limitSpain')?.enable();
                this.formData.get('distrito')?.enable();
              }
            }
          }
        }
        
        // Desactivar bandera después de las actualizaciones
        setTimeout(() => {
          this.isMunicipioBeingUpdated = false;
        }, 0);
      }
      
      // Actualizar la denominación según si es limítrofe o no
      this.actualizarDenominacion();
    });
    
    // Suscribirse a cambios en el distrito
    this.formData.get('distrito')?.valueChanges.subscribe(distrito => {
      if (distrito && typeof distrito === 'object') {
        // Si el distrito tiene información del país, actualizar el campo país
        if (distrito.pais) {
          const currentCountry = this.formData.get('country')?.value;
          const currentCountryId = currentCountry && typeof currentCountry === 'object' ? currentCountry.id : currentCountry;
          
          // Solo actualizar si es un país diferente o no hay país seleccionado
          if (!currentCountryId || currentCountryId !== distrito.pais.id) {
            // Establecer la bandera antes de actualizar el país
            this.isCountryUpdatedFromDistrito = true;
            
            this.formData.get('country')?.setValue(distrito.pais);
            
            // Activar lógica para países fronterizos si corresponde
            if (distrito.pais.id === this.FRANCIA_ID || distrito.pais.id === this.PORTUGAL_ID) {
              this.showForeignMunicipalities = true;
              this.showLimitSpainOption = true;
              this.formData.get('limitSpain')?.enable();
              this.formData.get('distrito')?.enable();
            }
          }
        }
      }
      
      // Actualizar la denominación según si es limítrofe o no
      this.actualizarDenominacion();
    });
    
    this.initializeCountryAutocomplete();
    
    this.formData.get('country')?.valueChanges.subscribe(country => {
      const countryId = country && typeof country === 'object' ? country.id : country;
      
      if (countryId) {
        // Solo resetear el distrito si el país no fue actualizado desde el distrito
        if (!this.isCountryUpdatedFromDistrito) {
          this.formData.get('distrito')?.setValue(null);
        } else {
          // Restablecer la bandera para futuras actualizaciones
          this.isCountryUpdatedFromDistrito = false;
        }
        
        this.showDistritos = countryId === this.FRANCIA_ID || countryId === this.PORTUGAL_ID;
        
        // Habilitar o deshabilitar el control del formulario
        if (countryId === this.FRANCIA_ID || countryId === this.PORTUGAL_ID) {
          this.formData.get('distrito')?.enable();
        } else {
          this.formData.get('distrito')?.disable();
        }
        
        this.showLimitSpainOption = countryId === this.FRANCIA_ID || countryId === this.PORTUGAL_ID;
        
        if (countryId !== this.FRANCIA_ID && countryId !== this.PORTUGAL_ID) {
          this.formData.get('limitSpain')?.setValue(false);
          this.formData.get('limitSpain')?.disable();
        } else {
          this.formData.get('limitSpain')?.enable();
        }
      } else {
        this.distritos.set([]);
        this.showDistritos = false;
        this.formData.get('distrito')?.disable();
        this.showLimitSpainOption = false;
        this.formData.get('limitSpain')?.setValue(false);
        this.formData.get('limitSpain')?.disable();
      }
      
      if (countryId === this.FRANCIA_ID || countryId === this.PORTUGAL_ID) {
        this.showForeignMunicipalities = true;
        this.showLimitSpainOption = true;
        
        // Resetear los campos de provincia y municipio cuando cambia el país
        if (!this.formData.get('limitSpain')?.value) {
          this.formData.get('provinciaLimitrofe')?.disable();
          this.formData.get('municipioLimitrofe')?.disable();
        }
        
        // Solo resetear el municipio si no está siendo actualizado por autocomplete
        if (!this.isMunicipioBeingUpdated) {
          this.formData.get('idMunicipioExtranjero')?.setValue(null);
        }
      } else {
        this.showForeignMunicipalities = false;
        this.showLimitSpainOption = false;
        this.foreignMunicipalities.set([]);
        if (this.formData.get('limitSpain')?.value) {
          this.formData.get('limitSpain')?.setValue(false);
        }
        this.formData.get('limitSpain')?.disable();
        this.formData.get('provinciaLimitrofe')?.disable();
        this.formData.get('municipioLimitrofe')?.disable();
      }
      
      // Actualizar la denominación según si es limítrofe o no
      this.actualizarDenominacion();
    });
    
    this.initializeProvinceAutocomplete();
    this.initializeMunicipalityAutocomplete();
    
    const country = this.formData.get('country')?.value;
    const countryId = country && typeof country === 'object' ? country.id : country;
    
    if (countryId === this.FRANCIA_ID || countryId === this.PORTUGAL_ID) {
      this.showForeignMunicipalities = true;
      this.showLimitSpainOption = true;
      this.showDistritos = true;
      this.formData.get('distrito')?.enable();
      this.formData.get('limitSpain')?.enable();
      
      // Revisar si el checkbox está seleccionado para habilitar los campos
      if (this.formData.get('limitSpain')?.value) {
        this.formData.get('provinciaLimitrofe')?.enable();
        this.formData.get('municipioLimitrofe')?.enable();
      } else {
        this.formData.get('provinciaLimitrofe')?.disable();
        this.formData.get('municipioLimitrofe')?.disable();
      }
    } else {
      this.showDistritos = false;
      this.formData.get('distrito')?.disable();
      this.showLimitSpainOption = false;
      this.formData.get('limitSpain')?.setValue(false);
      this.formData.get('limitSpain')?.disable();
      this.formData.get('provinciaLimitrofe')?.disable();
      this.formData.get('municipioLimitrofe')?.disable();
    }
    
    // Suscribirse a cambios en provincia y municipio limítrofe para actualizar denominación
    this.formData.get('provinciaLimitrofe')?.valueChanges.subscribe(() => {
      this.actualizarDenominacion();
    });
    
    this.formData.get('municipioLimitrofe')?.valueChanges.subscribe(() => {
      this.actualizarDenominacion();
    });
    
    // Suscribirse a cambios en idMunicipioExtranjero para actualizar denominación
    this.formData.get('idMunicipioExtranjero')?.valueChanges.subscribe(() => {
      this.actualizarDenominacion();
    });
    
    // Suscribirse a cambios en country para actualizar denominación
    this.formData.get('country')?.valueChanges.subscribe(() => {
      this.actualizarDenominacion();
    });
    
    // Inicializar el valor de denominación
    this.actualizarDenominacion();
    }
  
  // Nueva función para actualizar la denominación según la regla de negocio
  actualizarDenominacion() {
    // Obtener el valor del checkbox limítrofe con España
    const esLimitrofe = this.formData.get('limitSpain')?.value;
    
    // Obtener el valor del municipio extranjero (ubicación)
    const municipioExtranjero = this.formData.get('idMunicipioExtranjero')?.value;
    const ubicacion = municipioExtranjero && typeof municipioExtranjero === 'object' ? 
      municipioExtranjero.descripcion : municipioExtranjero;
    
    // Obtener el país
    const country = this.formData.get('country')?.value;
    const nombrePais = country && typeof country === 'object' ? country.descripcion : '';
    
    if (!esLimitrofe) {
      // Si no es limítrofe, usar solo la ubicación
      if (ubicacion) {
        this.formData.get('denomination')?.setValue(ubicacion + (nombrePais ? ` (${nombrePais})` : ''));
      }
    } else {
      // Si es limítrofe, construir el formato especificado
      const provinciaLimitrofe = this.formData.get('provinciaLimitrofe')?.value;
      const municipioLimitrofe = this.formData.get('municipioLimitrofe')?.value;
      
      const nombreProvinciaLimitrofe = provinciaLimitrofe && typeof provinciaLimitrofe === 'object' ? 
        provinciaLimitrofe.descripcion : '';

      const nombreMunicipioLimitrofe = municipioLimitrofe && typeof municipioLimitrofe === 'object' ? 
        municipioLimitrofe.descripcion : '';
      
      if (ubicacion && nombrePais) {
        let denominacion = `${ubicacion} (${nombrePais})`;
        
        if (nombreMunicipioLimitrofe && nombreProvinciaLimitrofe) {
          denominacion += ` - Frontera con ${nombreMunicipioLimitrofe} (${nombreProvinciaLimitrofe})`;
        }
        
        this.formData.get('denomination')?.setValue(denominacion);
      }
    }
  }
  
  // Inicializar autocomplete para distritos
  initializeDistritoAutocomplete() {
    this.distritosfilteredOptions = this.formData.get('distrito')?.valueChanges.pipe(
      startWith(''),
      debounceTime(500),
      switchMap(async value => {
        const country = this.formData.get('country')?.value;
        const countryId = country && typeof country === 'object' ? country.id : country;
        
        if (typeof value === 'string' && value.trim().length >= 3) {
          return await this.searchDistritos(value, countryId);
        } else if (countryId) {
          return await this.searchDistritos('', countryId);
        } else {
          return [];
        }
      }),
      catchError(() => of<Distrito[]>([]))
    ) || of([]);
  }
  
  // Buscar distritos
  async searchDistritos(searchText: string, countryId?: number): Promise<Distrito[]> {
    try {
      let endpoint = `/paises/busqueda/distritos`;
      
      let params = [];
      
      // Añadir parámetro de búsqueda si hay texto
      if (searchText && searchText.trim().length >= 3) {
        params.push(`busqueda=${searchText}`);
      }
      
      // Añadir parámetro de país si está disponible
      if (countryId) {
        params.push(`idPais=${countryId}`);
      }
      
      // Construir la URL con los parámetros
      if (params.length > 0) {
        endpoint += `?${params.join('&')}`;
      }
      
      const result = await firstValueFrom(
        this.http.get<Distrito[]>(endpoint)
      );
      return result;
    } catch (error) {
      console.error('Error al buscar distritos:', error);
      return [];
    }
  }
  
  // Función para mostrar el nombre del distrito en el autocomplete
  displayDistrito = (distrito: Distrito): string => {
    return distrito && distrito.descripcion ? distrito.descripcion : '';
  };
  
  // Inicializar autocomplete para municipios extranjeros
  initializeForeignMunicipalityAutocomplete(countryId?: number) {
    this.foreignMunicipalitiesFilteredOptions = this.formData.get('idMunicipioExtranjero')?.valueChanges.pipe(
      startWith(''),
      debounceTime(500),
      switchMap(async value => {
        if (typeof value === 'string' && value.trim().length >= 3) {
          // Obtener los valores de país y distrito si están disponibles
          const country = this.formData.get('country')?.value;
          const distrito = this.formData.get('distrito')?.value;
          
          const countryId = country && typeof country === 'object' ? country.id : country;
          const distritoId = distrito && typeof distrito === 'object' ? distrito.id : distrito;
          
          return await this.searchForeignMunicipalities(value, countryId, distritoId);
        } else {
          return [];
        }
      }),
      catchError(() => of<MunicipioExtranjero[]>([]))
    ) || of([]);
  }
  
  async searchForeignMunicipalities(searchText: string, countryId?: number, distritoId?: number): Promise<MunicipioExtranjero[]> {
    try {
      // Verificar que haya texto de búsqueda
      if (!searchText || searchText.trim().length < 3) {
        return [];
      }
      
      let endpoint = `/v2-Municipios/busqueda/extranjeros`;
      let params = [];
      
      // Añadir parámetro de búsqueda
      params.push(`busqueda=${searchText}`);
      
      // Añadir parámetro de país si está disponible
      if (countryId) {
        params.push(`idPais=${countryId}`);
      }
      
      // Añadir parámetro de distrito si está disponible
      if (distritoId) {
        params.push(`idDistrito=${distritoId}`);
      }
      
      // Construir la URL con los parámetros
      if (params.length > 0) {
        endpoint += `?${params.join('&')}`;
      }
      
      const municipios = await firstValueFrom(
        this.http.get<MunicipioExtranjero[]>(endpoint)
      );
      return municipios;
    } catch (error) {
      console.error('Error al buscar municipios extranjeros:', error);
      return [];
    }
  }
  
  displayForeignMunicipality = (municipio: MunicipioExtranjero): string => {
    return municipio && municipio.descripcion ? municipio.descripcion : '';
  };
  
  initializeCountryAutocomplete() {
    this.countryfilteredOptions = this.formData.get('country')?.valueChanges.pipe(
      startWith(''),
      debounceTime(300),
      switchMap(async (value) => {
        if (typeof value === 'string' && value.trim().length >= 2) {
          return await this.searchCountries(value);
        } else if (!value) {
          return await this.searchCountries('');
        } else {
          return this.foreignCountries();
        }
      }),
      catchError(() => of<Countries[]>([]))
    ) || of([]);
  }
  
  async searchCountries(searchText: string): Promise<Countries[]> {
    try {
      if (!searchText || searchText.trim().length < 2) {
        return this.foreignCountries();
      }
      
      const normalizedSearch = searchText.toLowerCase().normalize("NFD").replace(/[\u0300-\u036f]/g, "");
      return this.foreignCountries().filter(country => 
        country.descripcion.toLowerCase().normalize("NFD").replace(/[\u0300-\u036f]/g, "").includes(normalizedSearch)
      );
    } catch (error) {
      console.error('Error al buscar países:', error);
      return this.foreignCountries();
    }
  }
  
  displayCountry = (country: Countries): string => {
    return country && country.descripcion ? country.descripcion : '';
  };
  
  initializeProvinceAutocomplete() {
    this.provincefilteredOptions = this.formData.get('provinciaLimitrofe')?.valueChanges.pipe(
      startWith(''),
      debounceTime(300),
      switchMap(async (value) => {
        if (typeof value === 'string' && value.trim().length >= 2) {
          return await this.searchProvinces(value);
        } else if (!value) {
          return await this.searchProvinces('');
        } else {
          return [];
        }
      }),
      catchError(() => of<Province[]>([]))
    ) || of([]);
  }
  
  initializeMunicipalityAutocomplete() {
    this.municipalityfilteredOptions = merge(
      this.formData.get('municipioLimitrofe')?.valueChanges || of(''),
      this.formData.get('provinciaLimitrofe')?.valueChanges || of(null)
    ).pipe(
      startWith(''),
      debounceTime(300),
      switchMap(async () => {
        const municipioValue = this.formData.get('municipioLimitrofe')?.value;
        const provincia = this.formData.get('provinciaLimitrofe')?.value;
        const idProvincia = provincia && typeof provincia === 'object' ? provincia.id : provincia;
        
        if (typeof municipioValue === 'string' && municipioValue.trim().length >= 2 && idProvincia) {
          return await this.searchMunicipalities(municipioValue, idProvincia);
        } else if (idProvincia) {
          return await this.searchMunicipalities('', idProvincia);
        } else {
          return [];
        }
      }),
      catchError(() => of<Municipality[]>([]))
    );
  }
  
  async searchProvinces(searchText: string): Promise<Province[]> {
    try {
      const endpoint = '/v2-Provincias/Busqueda';
      let params = '';
      
      if (searchText && searchText.trim().length > 0) {
        params = `?busqueda=${searchText}`;
      }
      
      return await firstValueFrom(
        this.http.get<Province[]>(`${endpoint}${params}`)
      );
    } catch (error) {
      console.error('Error al buscar provincias:', error);
      return [];
    }
  }
  
  async searchMunicipalities(searchText: string, provinciaId: number): Promise<Municipality[]> {
    try {
      const endpoint = '/v2-Municipios/Busqueda';
      let params = '';
      
      if (provinciaId) {
        params += `?idProvincia=${provinciaId}`;
      }
      
      if (searchText && searchText.trim().length > 0) {
        params += params ? `&busqueda=${searchText}` : `?busqueda=${searchText}`;
      }
      
      return await firstValueFrom(
        this.http.get<Municipality[]>(`${endpoint}${params}`)
      );
    } catch (error) {
      console.error('Error al buscar municipios:', error);
      return [];
    }
  }
  
  displayProvince = (province: Province): string => {
    return province && province.descripcion ? province.descripcion : '';
  };
  
  displayMunicipality = (municipality: Municipality): string => {
    return municipality && municipality.descripcion ? municipality.descripcion : '';
  };
} 