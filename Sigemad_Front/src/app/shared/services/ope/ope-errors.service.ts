import { Injectable } from '@angular/core';

@Injectable({
  providedIn: 'root', // Hace que esté disponible en toda la aplicación
})
export class OpeErrorsService {
  obtenerMensajeError(error: any, componenteOpe: string): string {
    let mensajeError = 'Se ha producido un error al guardar los datos';

    // ADMINISTRACIÓN - PERIODOS
    if (componenteOpe === 'opePeriodos') {
      if (error?.Message) {
        mensajeError = error.Message;
      }
      if (error?.error?.Message || error?.error?.message) {
        const mensajeDelBackend = error?.error?.Message || error?.error?.message;
        if (mensajeDelBackend) {
          return mensajeDelBackend;
        }
      }

      // ADMINISTRACIÓN - FRONTERAS
    } else if (componenteOpe === 'opeFronteras') {
      if (error?.Details) {
        try {
          const detallesError = JSON.parse(error.Details);
          if (detallesError?.TransitoAltoVehiculos?.length > 0) {
            return 'El valor de tránsito alto debe ser mayor que el de tránsito medio.';
          }
        } catch (e) {
          // Si no es JSON válido, no hacemos nada
        }
      }
      // ADMINISTRACIÓN - PUERTOS
    } else if (componenteOpe === 'opePuertos') {
      if (error?.Message) {
        mensajeError = error.Message;
      }
      if (error?.error?.Message || error?.error?.message) {
        const mensajeDelBackend = error?.error?.Message || error?.error?.message;
        if (mensajeDelBackend) {
          return mensajeDelBackend;
        }
      }

      // ADMINISTRACIÓN - LÍNEAS MARÍTIMAS
    } else if (componenteOpe === 'opeLineasMaritimas') {
      if (error?.Details) {
        try {
          const detallesError = JSON.parse(error.Details);
          if (detallesError?.IdOpePuertoDestino?.includes('OpePuertosOrigenDestinoNoIguales')) {
            return 'El puerto de destino no puede ser igual al puerto de origen.';
          }
        } catch (e) {
          // Si no es JSON válido, no hacemos nada
        }
      }
      if (error?.Message) {
        mensajeError = error.Message;
      }
      if (error?.error?.Message || error?.error?.message) {
        const mensajeDelBackend = error?.error?.Message || error?.error?.message;
        if (mensajeDelBackend) {
          return mensajeDelBackend;
        }
      }

      // ADMINISTRACIÓN - PORCENTAJES OCUPACIÓN ÁREAS DE ESTACIONAMIENTO
    } else if (componenteOpe === 'opePorcentajesOcupacionAreaEstacionamiento') {
      if (error?.Message) {
        mensajeError = error.Message;
      }
      if (error?.error?.Message || error?.error?.message) {
        const mensajeDelBackend = error?.error?.Message || error?.error?.message;
        if (mensajeDelBackend) {
          return mensajeDelBackend;
        }
      }

      // DATOS - FRONTERAS
    } else if (componenteOpe === 'opeDatosFronteras') {
      if (error?.Details) {
        try {
          const detallesError = JSON.parse(error.Details);
          if (detallesError?.Fecha?.includes('FechaNoPuedeSerSuperiorAHoy')) {
            return 'La fecha no puede ser superior a hoy.';
          }
        } catch (e) {
          // Si no es JSON válido, no hacemos nada
        }
      }
      mensajeError = error?.Message;
      // DATOS - EMBARQUES DIARIOS
    } else if (componenteOpe === 'opeDatosEmbarquesDiarios') {
      mensajeError = error?.Message;
      // DATOS - ASISTENCIAS
    } else if (componenteOpe === 'opeDatosAsistencias') {
      if (error?.errors) {
        const errores = error?.errors;
        if (errores?.OpeDatosAsistenciasSanitarias?.includes('TiposAsistenciaSanitariaRepetidos')) {
          if (mensajeError) {
            mensajeError += '\n';
          }
          mensajeError += 'No se puede repetir el mismo tipo de asistencia sanitaria.';
        }
        if (errores?.OpeDatosAsistenciasSociales?.includes('TiposAsistenciaSocialRepetidos')) {
          if (mensajeError) {
            mensajeError += '\n';
          }
          mensajeError += 'No se puede repetir el mismo tipo de asistencia social.';
        }
        // Tareas
        if (errores) {
          for (const key in errores) {
            if (key.includes('OpeDatosAsistenciasSociales') && errores[key].includes('TiposAsistenciaSocialTareasRepetidos')) {
              if (mensajeError) {
                mensajeError += '\n';
              }
              mensajeError += 'No se puede repetir el mismo tipo de tarea.';
            }
            if (key.includes('OpeDatosAsistenciasSociales') && errores[key].includes('TiposAsistenciaSocialOrganismosRepetidos')) {
              if (mensajeError) {
                mensajeError += '\n';
              }
              mensajeError += 'No se puede repetir el mismo tipo de organismo.';
            }
          }
        }
      } else if (error?.Message) {
        mensajeError = error.Message;
      }
    }
    /*} /*else if (componenteOpe === 'opeAreasEstacionamiento') {
      alert('ff');
      mensajeError = error?.Message;
    }*/

    return mensajeError;
  }
}
