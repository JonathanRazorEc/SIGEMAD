SET IDENTITY_INSERT TipoDocumento ON;

INSERT INTO TipoDocumento (Id,Descripcion,Borrado,Editable) VALUES
	 (1,N'Imágenes/Fotos ',0,1),
	 (2,N'Comprimido.zip',0,1),
	 (3,N'Documentación General',0,1),
	 (4,N'Intervención de medios DGMNPF (Provisional)',0,1),
	 (8,N'Informe de situación',0,1),
	 (9,N'Intervenciones UME',0,1),
	 (10,N'Aportaciones de ayuda',0,1),
	 (11,N'Solicitud de ayuda',0,1),
	 (12,N'Nota de prensa',0,1),
	 (13,N'Simulación',0,1),
	 (15,N'Previsión de peligro de incendio',0,1);

SET IDENTITY_INSERT TipoDocumento OFF;



