// src/main.ts
import { bootstrapApplication } from '@angular/platform-browser';
import { AppComponent } from './app/app.component';
import { appConfig } from './app/app.config';

/**
 * Función para cargar la configuración en runtime.
 * Se realiza una petición a '/assets/config.json' y, una vez obtenido,
 * se almacena en una variable global o se podría inyectar mediante un provider.
 */
function loadRuntimeConfig(): Promise<any> {
  return fetch('/assets/config.json')
    .then((response) => {
      if (!response.ok) {
        throw new Error(`Error al cargar config.json: ${response.statusText}`);
      }
      return response.json();
    })
    .then((config) => {
      console.log('Loaded config:', config);
      (window as any)['runtimeConfig'] = config;
      return config;
    });
}

// Primero se carga la configuración y, una vez obtenida, se inicializa la aplicación.
loadRuntimeConfig()
  .then(() => bootstrapApplication(AppComponent, appConfig))
  .catch((err) => console.error('Error al inicializar la app:', err));
