SET QUOTED_IDENTIFIER ON;

-- ====================================================================
-- ROLES
-- ====================================================================
-- Insertar el rol de Administrador
INSERT INTO AspNetRoles (Id, Name, NormalizedName, ConcurrencyStamp)
VALUES ('E6248C8C-C043-4D11-B711-A4F8E34A940E', 'Administrador', 'ADMINISTRADOR', NULL);

-- Insertar el rol de Usuario Interno
--INSERT INTO AspNetRoles (Id, Name, NormalizedName, ConcurrencyStamp)
--VALUES ('FDAD9EC8-0720-4C10-9A07-739D20D1E42B', 'UsuarioInterno', 'USUARIOINTERNO', NULL);

-- Insertar el rol de Usuario Externo
--INSERT INTO AspNetRoles (Id, Name, NormalizedName, ConcurrencyStamp)
--VALUES ('FC99E2FD-8544-4CDC-8B52-17C75DB077E1', 'UsuarioExterno', 'USUARIOEXTERNO', NULL);

INSERT INTO AspNetRoles (Id, Name, NormalizedName, ConcurrencyStamp)
VALUES ('445FE762-0650-470E-AFB1-2B2FA5D94207', 'Operador_SIGEMAD', 'OPERADOR_SIGEMAD', NULL);

INSERT INTO AspNetRoles (Id, Name, NormalizedName, ConcurrencyStamp)
VALUES ('E0FD3E1B-A5CE-4484-BEC7-B026DE7ADC51', 'Gestor_SIGEMAD', 'GESTOR_SIGEMAD', NULL);

INSERT INTO AspNetRoles (Id, Name, NormalizedName, ConcurrencyStamp)
VALUES ('2A56FAE9-2D38-4C61-8B66-4B8CEDBE56EF', 'Operador_OPE', 'OPERADOR_OPE', NULL);

INSERT INTO AspNetRoles (Id, Name, NormalizedName, ConcurrencyStamp)
VALUES ('9052884B-D94E-4CB6-89F5-59EFB039393F', 'Gestor_OPE', 'GESTOR_OPE', NULL);

INSERT INTO AspNetRoles (Id, Name, NormalizedName, ConcurrencyStamp)
VALUES ('87264C14-55F6-4F32-9494-9E385B0FBD7F', 'Lectura_SIGEMAD', 'LECTURA_SIGEMAD', NULL);

-- select * from AspNetRoles order by 2


-- ====================================================================
-- USUARIOS
-- ====================================================================

-- Insertar un usuario administrador
INSERT INTO AspNetUsers (Id, UserName, NormalizedUserName, Email, NormalizedEmail, EmailConfirmed, PasswordHash, SecurityStamp, ConcurrencyStamp, PhoneNumberConfirmed, TwoFactorEnabled, LockoutEnabled, AccessFailedCount)
VALUES ('B3C14CF0-58C8-4781-94D0-F9EAB4296C7A', 'admin@example.com', 'ADMIN@EXAMPLE.COM', 'admin@example.com', 'ADMIN@EXAMPLE.COM', 1, 'AQAAAAEAACcQAAAAECxCbHO2Bd0R9J7kBz6DH+ndUhHk0hifdcNQVg+btQUaV6jT5AuiQf61SPPoFEeb6w==', NEWID(), NEWID(), 0, 0, 0, 0);

-- Insertar un usuario interno
--INSERT INTO AspNetUsers (Id, UserName, NormalizedUserName, Email, NormalizedEmail, EmailConfirmed, PasswordHash, SecurityStamp, ConcurrencyStamp, PhoneNumberConfirmed, TwoFactorEnabled, LockoutEnabled, AccessFailedCount)
--VALUES ('C596AD0C-17D1-48D0-B08B-F635B83D14C2', 'interno@example.com', 'INTERNO@EXAMPLE.COM', 'interno@example.com', 'INTERNO@EXAMPLE.COM', 1, 'AQAAAAEAACcQAAAAECaWXRR0KIKEpE++JFanlo7bjiotVXQTzrloTEa6vmD1u9CNvAElAo49x4zpuwSDgQ==',  NULL, NULL, 0, 0, 0, 0);

-- Insertar un usuario externo
--INSERT INTO AspNetUsers (Id, UserName, NormalizedUserName, Email, NormalizedEmail, EmailConfirmed, PasswordHash, SecurityStamp, ConcurrencyStamp, PhoneNumberConfirmed, TwoFactorEnabled, LockoutEnabled, AccessFailedCount)
--VALUES ('FB161AB0-96C6-4549-9395-2C1EC5FD9B46', 'externo@example.com', 'EXTERNO@EXAMPLE.COM', 'externo@example.com', 'EXTERNO@EXAMPLE.COM', 1, 'AQAAAAEAACcQAAAAECaWXRR0KIKEpE++JFanlo7bjiotVXQTzrloTEa6vmD1u9CNvAElAo49x4zpuwSDgQ==',  NULL, NULL, 0, 0, 0, 0);

-- Insertar un usuario Operador_SIGEMAD 
INSERT INTO AspNetUsers (Id, UserName, NormalizedUserName, Email, NormalizedEmail, EmailConfirmed, PasswordHash, SecurityStamp, ConcurrencyStamp, PhoneNumberConfirmed, TwoFactorEnabled, LockoutEnabled, AccessFailedCount)
VALUES ('FCC05656-3B84-4747-A197-0BB6FE50C69A', 'operadorSigemad@example.com', 'OPERADORSIGEMAD@EXAMPLE.COM', 'operadorSigemad@example.com', 'OPERADORSIGEMAD@EXAMPLE.COM', 1, 'AQAAAAEAACcQAAAAECxCbHO2Bd0R9J7kBz6DH+ndUhHk0hifdcNQVg+btQUaV6jT5AuiQf61SPPoFEeb6w==',  NEWID(), NEWID(), 0, 0, 0, 0);

-- Insertar un usuario Gestor_SIGEMAD 
INSERT INTO AspNetUsers (Id, UserName, NormalizedUserName, Email, NormalizedEmail, EmailConfirmed, PasswordHash, SecurityStamp, ConcurrencyStamp, PhoneNumberConfirmed, TwoFactorEnabled, LockoutEnabled, AccessFailedCount)
VALUES ('F96E3FD0-EB93-4970-86ED-41CD7AC9DCA5', 'GestorSigemad@example.com', 'GESTORSIGEMAD@EXAMPLE.COM', 'GestorSigemad@example.com', 'GESTORSIGEMAD@EXAMPLE.COM', 1, 'AQAAAAEAACcQAAAAECxCbHO2Bd0R9J7kBz6DH+ndUhHk0hifdcNQVg+btQUaV6jT5AuiQf61SPPoFEeb6w==',  NEWID(), NEWID(), 0, 0, 0, 0);

-- Insertar un usuario Operador_OPE 
INSERT INTO AspNetUsers (Id, UserName, NormalizedUserName, Email, NormalizedEmail, EmailConfirmed, PasswordHash, SecurityStamp, ConcurrencyStamp, PhoneNumberConfirmed, TwoFactorEnabled, LockoutEnabled, AccessFailedCount)
VALUES ('1CC5AA78-FDA4-42C7-BFFE-912B12543950', 'operadorOpe@example.com', 'OPERADOROPE@EXAMPLE.COM', 'operadorOpe@example.com', 'OPERADOROPE@EXAMPLE.COM', 1, 'AQAAAAEAACcQAAAAECxCbHO2Bd0R9J7kBz6DH+ndUhHk0hifdcNQVg+btQUaV6jT5AuiQf61SPPoFEeb6w==',  NEWID(), NEWID(), 0, 0, 0, 0);

-- Insertar un usuario Gestor_OPE 
INSERT INTO AspNetUsers (Id, UserName, NormalizedUserName, Email, NormalizedEmail, EmailConfirmed, PasswordHash, SecurityStamp, ConcurrencyStamp, PhoneNumberConfirmed, TwoFactorEnabled, LockoutEnabled, AccessFailedCount)
VALUES ('4CF3BCC9-CE1F-4F08-8FE3-1A1FF2926A19', 'GestorOpe@example.com', 'GESTOROPE@EXAMPLE.COM', 'GestorOpe@example.com', 'GESTOROPE@EXAMPLE.COM', 1, 'AQAAAAEAACcQAAAAECxCbHO2Bd0R9J7kBz6DH+ndUhHk0hifdcNQVg+btQUaV6jT5AuiQf61SPPoFEeb6w==',  NEWID(), NEWID(), 0, 0, 0, 0);

-- Insert usuario de sistema
INSERT INTO [AspNetUsers] ([Id], [UserName], [NormalizedUserName], [Email], [NormalizedEmail], [EmailConfirmed], [PasswordHash], [SecurityStamp], [ConcurrencyStamp], [PhoneNumber], [PhoneNumberConfirmed], [TwoFactorEnabled], [LockoutEnd], [LockoutEnabled], [AccessFailedCount])
VALUES (
    '00000000-0000-0000-0000-000000000001', 'system_user', 'SYSTEM_USER', 'system@domain.com', 'SYSTEM@DOMAIN.COM',
    1, -- Email confirmado
    NULL, -- No necesita un hash de contraseña
    NEWID(), -- SecurityStamp generado
    NEWID(), -- ConcurrencyStamp generado
    NULL, -- Sin teléfono
    0, -- No se requiere confirmación de teléfono
    0, -- Sin autenticación de dos factores
    NULL, -- No hay fin de bloqueo
    0, -- Bloqueo deshabilitado
    0  -- Sin fallos de acceso
);

-- Insert for Sigemad tempora
INSERT INTO [AspNetUsers] ([Id], [UserName], [NormalizedUserName], [Email], [NormalizedEmail], [EmailConfirmed], [PasswordHash], [SecurityStamp], [ConcurrencyStamp], [PhoneNumber], [PhoneNumberConfirmed], [TwoFactorEnabled], [LockoutEnd], [LockoutEnabled], [AccessFailedCount])
VALUES (
    '0C942A28-96D1-408D-BD5B-6185212B1B1B', 'sigemad', 'SIGEMAD', 'sigemad@sigemad.com', 'SIGEMAD@SIGEMAD.COM',
    1, -- Email confirmado
    'AQAAAAEAACcQAAAAECxCbHO2Bd0R9J7kBz6DH+ndUhHk0hifdcNQVg+btQUaV6jT5AuiQf61SPPoFEeb6w==', -- No necesita un hash de contraseña
    'ED685813-F655-4FAC-85D1-A64FE9160355', -- SecurityStamp generado
    'C80C76B7-55E9-4A65-B47A-0BD3BE6A7C14', -- ConcurrencyStamp generado
    NULL, -- Sin teléfono
    0, -- No se requiere confirmación de teléfono
    0, -- Sin autenticación de dos factores
    NULL, -- No hay fin de bloqueo
    0, -- Bloqueo deshabilitado
    0  -- Sin fallos de acceso
);

-- select * from AspNetUsers order by 2

-- ============================
-- Para tabla ApplicationUsers
-- ============================

-- Insertar un usuario administrador en la tabla ApplicationUsers
INSERT INTO ApplicationUsers (Id, IdentityId, Nombre, Apellidos, Email, Telefono, FechaCreacion, CreadoPor, FechaModificacion, ModificadoPor, Borrado)
VALUES ('AC41965C-7F9E-48C0-BBC1-C4A0487DFB2D', 'B3C14CF0-58C8-4781-94D0-F9EAB4296C7A', 'Admin', 'Administrador', 'admin@example.com', '555-1234', GETDATE(), NULL, NULL, NULL, 0);

-- Insertar un usuario interno en la tabla ApplicationUsers
--INSERT INTO ApplicationUsers (Id, IdentityId, Nombre, Apellidos, Email, Telefono, FechaCreacion, CreadoPor, FechaModificacion, ModificadoPor, Borrado)
--VALUES ('550E683E-0458-43E8-A6E6-20887DC2BDDD', 'C596AD0C-17D1-48D0-B08B-F635B83D14C2', 'Interno', 'Empleado', 'interno@example.com', '555-5678', GETDATE(), NULL, NULL, NULL, 0);

-- Insertar un usuario externo en la tabla ApplicationUsers
--INSERT INTO ApplicationUsers (Id, IdentityId, Nombre, Apellidos, Email, Telefono, FechaCreacion, CreadoPor, FechaModificacion, ModificadoPor, Borrado)
--VALUES ('D3813C04-4EEE-4D37-84B7-49EC293F92D2', 'FB161AB0-96C6-4549-9395-2C1EC5FD9B46', 'Externo', 'Proveedor', 'externo@example.com', '555-9876', GETDATE(), NULL, NULL, NULL, 0);


-- Insertar un usuario Operador_SIGEMAD 
INSERT INTO ApplicationUsers (Id, IdentityId, Nombre, Apellidos, Email, Telefono, FechaCreacion, CreadoPor, FechaModificacion, ModificadoPor, Borrado)
VALUES ('B30C2C73-FE84-4DE0-AA20-8F8689B2888D', 'FCC05656-3B84-4747-A197-0BB6FE50C69A', 'Operador', 'Sigemad', 'operadorSigemad@example.com', '', GETDATE(), NULL, NULL, NULL, 0);

-- Insertar un usuario Gestor_SIGEMAD 
INSERT INTO ApplicationUsers (Id, IdentityId, Nombre, Apellidos, Email, Telefono, FechaCreacion, CreadoPor, FechaModificacion, ModificadoPor, Borrado)
VALUES ('1319CAD1-E508-45F9-A0A0-9153E3943B7C', 'F96E3FD0-EB93-4970-86ED-41CD7AC9DCA5', 'Gestor', 'Sigemad', 'GestorSigemad@example.com', '', GETDATE(), NULL, NULL, NULL, 0);

-- Insertar un usuario Operador_OPE 
INSERT INTO ApplicationUsers (Id, IdentityId, Nombre, Apellidos, Email, Telefono, FechaCreacion, CreadoPor, FechaModificacion, ModificadoPor, Borrado)
VALUES ('F1B0F815-43DA-4D86-8214-A6D42F2B7FB6', '1CC5AA78-FDA4-42C7-BFFE-912B12543950', 'operador', 'Ope', 'operadorOpe@example.com', '', GETDATE(), NULL, NULL, NULL, 0);

-- Insertar un usuario Gestor_OPE 
INSERT INTO ApplicationUsers (Id, IdentityId, Nombre, Apellidos, Email, Telefono, FechaCreacion, CreadoPor, FechaModificacion, ModificadoPor, Borrado)
VALUES ('19ED52DD-7ED4-4D6F-B58C-BAB521B1068F', '4CF3BCC9-CE1F-4F08-8FE3-1A1FF2926A19', 'Gestor', 'Ope', 'GestorOpe@example.com', '', GETDATE(), NULL, NULL, NULL, 0);


INSERT INTO [ApplicationUsers] ([Id], [IdentityId], [Nombre], [Apellidos], [Email], [Telefono], [FechaCreacion], [CreadoPor], [Borrado]
)
VALUES (
    '00000000-0000-0000-0000-000000000001',
    '00000000-0000-0000-0000-000000000001',
    'Usuario',
    'Del Sistema',
    'system@domain.com',
    '', -- Sin teléfono
    GETDATE(),
    NULL, -- Este es el primer registro, por lo tanto CreadoPor puede ser NULL
    0 -- No borrado
);

INSERT INTO [ApplicationUsers] ([Id], [IdentityId], [Nombre], [Apellidos], [Email], [Telefono], [FechaCreacion], [CreadoPor], [Borrado]
)
VALUES (
    '0C942A28-96D1-408D-BD5B-6185212B1B1B',
    '0C942A28-96D1-408D-BD5B-6185212B1B1B',
    'sigemad',
    'sigemad',
    'sigemad@sigemad.com',
    '', -- Sin teléfono
    GETDATE(),
    NULL, -- Este es el primer registro, por lo tanto CreadoPor puede ser NULL
    0 -- No borrado
);


-- select * from ApplicationUsers

-- ====================================================================
-- USUARIOS - ROLES
-- ====================================================================

-- Obtener el Id del rol Administrador
DECLARE @RoleIdAdministrador NVARCHAR(450) = (SELECT Id FROM AspNetRoles WHERE NormalizedName = 'ADMINISTRADOR');

-- Obtener el Id del usuario administrador
DECLARE @UserIdAdministrador NVARCHAR(450) = (SELECT Id FROM AspNetUsers WHERE NormalizedUserName = 'ADMIN@EXAMPLE.COM');

-- Asignar el rol Administrador al usuario administrador
INSERT INTO AspNetUserRoles (UserId, RoleId)
VALUES (@UserIdAdministrador, @RoleIdAdministrador);


-- Obtener el Id del rol UsuarioInterno
--DECLARE @RoleIdInterno NVARCHAR(450) = (SELECT Id FROM AspNetRoles WHERE NormalizedName = 'USUARIOINTERNO');

-- Obtener el Id del usuario interno
--DECLARE @UserIdInterno NVARCHAR(450) = (SELECT Id FROM AspNetUsers WHERE NormalizedUserName = 'INTERNO@EXAMPLE.COM');

-- Asignar el rol Usuario Interno al usuario
--INSERT INTO AspNetUserRoles (UserId, RoleId)
--VALUES (@UserIdInterno, @RoleIdInterno);


-- Obtener el Id del rol UsuarioExterno
--DECLARE @RoleIdExterno NVARCHAR(450) = (SELECT Id FROM AspNetRoles WHERE NormalizedName = 'USUARIOEXTERNO');

-- Obtener el Id del usuario externo
--DECLARE @UserIdExterno NVARCHAR(450) = (SELECT Id FROM AspNetUsers WHERE NormalizedUserName = 'EXTERNO@EXAMPLE.COM');

-- Asignar el rol Usuario Externo al usuario
--INSERT INTO AspNetUserRoles (UserId, RoleId)
--VALUES (@UserIdExterno, @RoleIdExterno);


-- Obtener el Id del rol operadorSigemad
DECLARE @RoleIdOperadorSigemad NVARCHAR(450) = (SELECT Id FROM AspNetRoles WHERE NormalizedName = 'OPERADOR_SIGEMAD');

-- Obtener el Id del usuario operadorSigemad
DECLARE @UserIdOperadorSigemad NVARCHAR(450) = (SELECT Id FROM AspNetUsers WHERE NormalizedUserName = 'OPERADORSIGEMAD@EXAMPLE.COM');

-- Asignar el rol Usuario operadorSigemad al usuario
INSERT INTO AspNetUserRoles (UserId, RoleId)
VALUES (@UserIdOperadorSigemad, @RoleIdOperadorSigemad);


-- Obtener el Id del rol gestorSigemad
DECLARE @RoleIdGestorSigemad NVARCHAR(450) = (SELECT Id FROM AspNetRoles WHERE NormalizedName = 'GESTOR_SIGEMAD');

-- Obtener el Id del usuario gestorSigemad
DECLARE @UserIdGestorSigemad NVARCHAR(450) = (SELECT Id FROM AspNetUsers WHERE NormalizedUserName = 'gestorSigemad@EXAMPLE.COM');

-- Asignar el rol Usuario gestorSigemad al usuario
INSERT INTO AspNetUserRoles (UserId, RoleId)
VALUES (@UserIdGestorSigemad, @RoleIdGestorSigemad);


-- Obtener el Id del rol OperadorOpe
DECLARE @RoleIdOperadorOpe NVARCHAR(450) = (SELECT Id FROM AspNetRoles WHERE NormalizedName = 'OPERADOR_OPE');

-- Obtener el Id del usuario OperadorOpe
DECLARE @UserIdOperadorOpe NVARCHAR(450) = (SELECT Id FROM AspNetUsers WHERE NormalizedUserName = 'OPERADOROPE@EXAMPLE.COM');

-- Asignar el rol Usuario OperadorOpe al usuario
INSERT INTO AspNetUserRoles (UserId, RoleId)
VALUES (@UserIdOperadorOpe, @RoleIdOperadorOpe);

-- Obtener el Id del rol GestorOpe
DECLARE @RoleIdGestorOpe NVARCHAR(450) = (SELECT Id FROM AspNetRoles WHERE NormalizedName = 'GESTOR_OPE');

-- Obtener el Id del usuario GestorOpe
DECLARE @UserIdGestorOpe NVARCHAR(450) = (SELECT Id FROM AspNetUsers WHERE NormalizedUserName = 'GESTOROPE@EXAMPLE.COM');

-- Asignar el rol Usuario GestorOpe al usuario
INSERT INTO AspNetUserRoles (UserId, RoleId)
VALUES (@UserIdGestorOpe, @RoleIdGestorOpe);


-- Obtener el Id del rol Administrador
--DECLARE @RoleIdAdministrador NVARCHAR(450) = (SELECT Id FROM AspNetRoles WHERE NormalizedName = 'ADMINISTRADOR');

-- Obtener el Id del usuario Sigemad
DECLARE @UserIdSigemad NVARCHAR(450) = (SELECT Id FROM AspNetUsers WHERE NormalizedUserName = 'SIGEMAD');

-- Asignar el rol Usuario Sigemad al usuario
INSERT INTO AspNetUserRoles (UserId, RoleId)
VALUES (@UserIdSigemad, @RoleIdAdministrador);

