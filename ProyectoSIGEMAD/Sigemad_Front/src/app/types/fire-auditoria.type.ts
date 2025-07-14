export type FireAuditoria = {
    vistaPrimaria: {
      id: number;
      inicio: string;
      estadoIncendio: string;
      notaGeneral: string;
      situacionOperativa: number;
      municipio: string;
      superficieAfectadaHa: number;
      seguimiento: string;
      estadoSuceso: string;
      claseSuceso: string;
    };
    movilizacionMediosExtraOrdinarios: {
      solicitante: string;
      idPasoMovilizacion: number;
    }[];
    convocatoriasCECOD: {
      fechaInicio: string; 
      fechaFin: string;    
      lugar: string;
      convocados: string;
    }[];
    activacionSistemas: {
      tipoActivacion: string;
      fechaActivacion: string;     
      fechaActualizacion: string; 
    }[];
    declaracionZAGEP: {
      fecha: string;        
      denominacion: string;
    }[];
    declaracionEmergenciaInteresNacional: {
      autoridadSolicitante: string;
      descripcionDeLaSolicitud: string;
      fechaSolicitud: string;   
      fechaDeclaracion: string; 
    };
    activacionPlanEmergencias: {
      tipoPlan: number;
      planEmergencia: string;
      fechaInicio: string;  
      fechaFinal: string;   
      autoridad: string;
    }[];
    notificacionesOficiales: {
      idTipoNotificacion: number;
      fecha: string;                 
      organosSNPCNotificados: string;
      organismoInternacional: string;
    }[];
    sucesoRelacionado: {
      tipoPlan: number;
      fechaCreacion: string;  
      denominacion: string;
      estadoSuceso: string;
    };
  };
  