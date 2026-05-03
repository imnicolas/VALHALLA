-- ==============================================================================
-- Creación de Base de Datos: VALHALLA
-- ==============================================================================
IF NOT EXISTS (SELECT name FROM sys.databases WHERE name = N'VALHALLA_DB')
BEGIN
    CREATE DATABASE VALHALLA_DB;
END
GO

USE VALHALLA_DB;
GO

-- ==============================================================================
-- 1. TABLAS PADRE (No tienen dependencias externas)
-- ==============================================================================

CREATE TABLE Roles (
    idRol INT IDENTITY(1,1) PRIMARY KEY,
    nombreRol VARCHAR(50) NOT NULL
);

CREATE TABLE Permisos (
    idPermiso INT IDENTITY(1,1) PRIMARY KEY,
    nombreRecurso VARCHAR(200) NOT NULL
);

CREATE TABLE PerfilesStack (
    idPerfilStack INT IDENTITY(1,1) PRIMARY KEY,
    nombrePerfil VARCHAR(100) NOT NULL
);

CREATE TABLE Herramientas (
    idHerramienta INT IDENTITY(1,1) PRIMARY KEY,
    nombre VARCHAR(50) NOT NULL,
    version VARCHAR(50),
    scriptBase VARCHAR(2500)
);

-- ==============================================================================
-- 2. TABLAS INTERMEDIAS (Independientes de Usuarios)
-- ==============================================================================

CREATE TABLE Roles_Permisos (
    idRol INT,
    idPermiso INT,
    PRIMARY KEY (idRol, idPermiso),
    CONSTRAINT FK_Roles_Permisos_Roles FOREIGN KEY (idRol) REFERENCES Roles(idRol) ON DELETE CASCADE,
    CONSTRAINT FK_Roles_Permisos_Permisos FOREIGN KEY (idPermiso) REFERENCES Permisos(idPermiso) ON DELETE CASCADE
);

CREATE TABLE Perfil_Herramienta (
    idPerfilStack INT,
    idHerramienta INT,
    PRIMARY KEY (idPerfilStack, idHerramienta),
    CONSTRAINT FK_Perfil_Herramienta_PerfilesStack FOREIGN KEY (idPerfilStack) REFERENCES PerfilesStack(idPerfilStack) ON DELETE CASCADE,
    CONSTRAINT FK_Perfil_Herramienta_Herramientas FOREIGN KEY (idHerramienta) REFERENCES Herramientas(idHerramienta) ON DELETE CASCADE
);

-- ==============================================================================
-- 3. TABLAS NÚCLEO (Dependen de las tablas Padre)
-- ==============================================================================

CREATE TABLE Usuarios (
    idUsuario INT IDENTITY(1,1) PRIMARY KEY,
    legajo VARCHAR(200) NOT NULL UNIQUE,
    email VARCHAR(50) NOT NULL UNIQUE,
    passwordHash VARCHAR(500) NOT NULL,
    idRol INT NOT NULL,
    idPerfilStack INT NULL, 
    CONSTRAINT FK_Usuarios_Roles FOREIGN KEY (idRol) REFERENCES Roles(idRol),
    CONSTRAINT FK_Usuarios_PerfilesStack FOREIGN KEY (idPerfilStack) REFERENCES PerfilesStack(idPerfilStack)
);

-- ==============================================================================
-- 4. TABLAS HIJAS (Dependen de Usuarios)
-- ==============================================================================

CREATE TABLE Equipos (
    idEquipo INT IDENTITY(1,1) PRIMARY KEY,
    macAddress VARCHAR(250) UNIQUE,
    nroSerie VARCHAR(250),
    estado VARCHAR(50) NOT NULL,
    idUsuario INT NULL, 
    CONSTRAINT FK_Equipos_Usuarios FOREIGN KEY (idUsuario) REFERENCES Usuarios(idUsuario) ON DELETE SET NULL
);

CREATE TABLE Secretos_Cifrados (
    idSecreto INT IDENTITY(1,1) PRIMARY KEY,
    tipoDocumento VARCHAR(50) NOT NULL,
    blobCifrado VARBINARY(MAX) NOT NULL,
    idUsuario INT NOT NULL,
    CONSTRAINT FK_Secretos_Cifrados_Usuarios FOREIGN KEY (idUsuario) REFERENCES Usuarios(idUsuario) ON DELETE CASCADE
);
GO