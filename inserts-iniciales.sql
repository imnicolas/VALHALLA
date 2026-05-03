
-- 1. Insertar Roles (Padre)
INSERT INTO Roles (nombreRol) 
VALUES ('Desarrollador'), ('Administrador IT'), ('Lider Tecnico');

-- 2. Insertar Permisos (Padre)
INSERT INTO Permisos (nombreRecurso) 
VALUES ('Restaurar_Entorno'), ('Gestionar_Inventario'), ('Definir_Stack');

-- 3. Vincular Roles y Permisos (Tabla Intermedia)
-- El Desarrollador (ID 1) puede Restaurar Entorno (ID 1)
-- El Admin IT (ID 2) puede Gestionar Inventario (ID 2)
INSERT INTO Roles_Permisos (idRol, idPermiso) 
VALUES (1, 1), (2, 2);

-- 4. Insertar Perfil Stack (Padre)
INSERT INTO PerfilesStack (nombrePerfil) 
VALUES ('Full-Stack C# / Angular');

-- 5. Insertar Herramientas (Padre)
INSERT INTO Herramientas (nombre, version, scriptBase) 
VALUES 
('Visual Studio', '2022', 'winget install Microsoft.VisualStudio.2022.Community'),
('Node.js', '20.x', 'winget install OpenJS.NodeJS'),
('Angular CLI', '17.x', 'npm install -g @angular/cli');

-- 6. Vincular Perfil y Herramientas (Tabla Intermedia)
-- Asignamos las 3 herramientas al perfil Full-Stack (ID 1)
INSERT INTO Perfil_Herramienta (idPerfilStack, idHerramienta) 
VALUES (1, 1), (1, 2), (1, 3);

-- 7. Insertar Usuario (Núcleo)
-- Se asigna el Rol 1 (Desarrollador) y el Perfil 1 (Full-Stack)
INSERT INTO Usuarios (legajo, email, passwordHash, idRol, idPerfilStack)
VALUES ('B00099732-T4', 'nicolas.suarez@devtech.com', '03ac674216f3e15c761ee1a5e255f067953623c8b388b4459e13f978d7c846f4', 1, 1);
GO