export interface ActuacionRelevante {
  IdActuacionRelevante: number;
  IdSuceso: number;
  Movilizaciones: Movilizacion[];
}

export interface Movilizacion {
  Id: number;
  Solicitante: string;
  Pasos: Paso[];
}

export type Paso =
  | PasoSolicitud
  | PasoTramitacion
  | PasoOfrecimiento
  | PasoCancelacion
  | PasoAportacion
  | PasoDespliegue
  | PasoIntervencion
  | PasoLlegadaBase;

export interface PasoBase {
  Id: number;
  TipoPaso: number;
  Descripcion: string;
  Observaciones?: string;
}

/** Paso 1 - Solicitud */
export interface PasoSolicitud extends PasoBase {
  TipoPaso: number;
  IdProcedenciaMedio: number;
  AutoridadSolicitante: string;
  FechaHoraSolicitud: string;
}

/** Paso 2 - Tramitación */
export interface PasoTramitacion extends PasoBase {
  TipoPaso: 2;
  IdDestinoMedio: number;
  TitularMedio: string;
  FechaHoraTramitacion: string;
  PublicadoCECIS?: boolean;
}

/** Paso 3 - Ofrecimiento */
export interface PasoOfrecimiento extends PasoBase {
  TipoPaso: 3;
  TitularMedio: string;
  FechaHoraOfrecimiento: string;
  FechaHoraDisponibilidad: string;
  GestionCECOD?: boolean;
}

/** Paso 3 - Cancelación */
export interface PasoCancelacion extends PasoBase {
  TipoPaso: 4;
  TitularMedio: string;
  FechaHoraOfrecimiento: string;
  FechaHoraDisponibilidad: string;
  GestionCECOD?: boolean;
}

/** Paso 5 - Aportación */
export interface PasoAportacion extends PasoBase {
  TipoPaso: 5;
  IdCapacidad: number;
  IdTipoAdministracion: number;
  MedioNoCatalogado: string;
  TitularMedio: string;
  FechaHoraAportacion: string;
}

/** Paso 6 - Despliegue */
export interface PasoDespliegue extends PasoBase {
  TipoPaso: 6;
  IdCapacidad: number;
  MedioNoCatalogado: string;
  FechaHoraDespliegue: string;
  FechaHoraInicioIntervencion: string;
}

/** Paso 7 - Fin de intervención */
export interface PasoIntervencion extends PasoBase {
  TipoPaso: 7;
  IdCapacidad: number;
  MedioNoCatalogado: string;
  FechaHoraInicioIntervencion: string;
}

/** Paso 8 - Llegada a Base */
export interface PasoLlegadaBase extends PasoBase {
  TipoPaso: 8;
  IdCapacidad: number;
  MedioNoCatalogado: string;
  FechaHoraLlegada: string;
}