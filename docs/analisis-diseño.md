## UNIVERSIDAD ABIERTA INTERAMERICANA

```
Facultad de Tecnología Informática
Materia: Trabajo Final Docente: Maximiliano Bonaccorsi
```
**Título de Proyecto** (^) **Versión**
Sistema de Gestión de Activos IT y Entornos de Desarrollo

- Objetivo del Sistema Índice :
   - Objetivo General
   - Objetivos Particulares
- Alcance del Sistema (Gestiones y Módulos)
   - Gestión de Configuración y Perfiles (Core del negocio)
      - 1. Módulo de Gestión de Secretos
      - 2. Módulo de Definición de Stack
      - 3. Módulo de Variables de Entorno
   - Gestión de Activos IT:
      - 4. Módulo de Inventario
      - 5. Módulo de Asignación:
   - Gestión de Seguridad y Acceso:
      - 6. Módulo de Autenticación
      - 7. Módulo de Permisos (Patrón Composite)
- Organigrama de la Empresa
- ——————————Segunda Entrega———————
- Diagramas de actividades de cada gestión.
   - Gestión de Configuración y Perfiles (Core del negocio)
   - Gestión de Activos IT
   - Gestión de Seguridad y Acceso
- Casos de Uso
   - Especificación de Casos de Uso
      - CU-01: Restaurar Entorno de Desarrollo
      - CU-02: Definir Stack Tecnológico
      - CU-03: Asignar Hardware a Empleado
- Diagrama de clases (UML)
- Diagramas de Secuencias (UML) de cada Caso de uso
      - CU-01: Restaurar Entorno de Desarrollo
      - CU-02: Definir Stack Tecnológico
- D.E.R
- Resumen


## Objetivo del Sistema Índice :

### Objetivo General

Desarrollar una plataforma integral para la gestión de activos informáticos y la
automatización de la configuración de entornos de desarrollo, garantizando la continuidad
operativa ante recambios de hardware o reinstalaciones de sistema.

### Objetivos Particulares

1. Centralizar el inventario de hardware y licencias de la organización.
2. Automatizar la restauración de archivos de configuración técnica (.npmrc,
    settings.xml) mediante almacenamiento criptográfico.
3. Reducir el tiempo de _onboarding_ y _setup_ de desarrolladores mediante scripts de
    instalación de stack tecnológico (Java, Node, K8s, etc.).

## Alcance del Sistema (Gestiones y Módulos)

### Gestión de Configuración y Perfiles (Core del negocio)

Esta gestión permite al desarrollador definir su "huella digital de trabajo" para poder
replicarla en cualquier equipo.

#### 1. Módulo de Gestión de Secretos

```
Almacena credenciales de repositorios y nexos de dependencia (Nexus, Maven) de
forma segura.
```
#### 2. Módulo de Definición de Stack

```
Permite seleccionar las herramientas necesarias (JDK 17, Maven, AWS CLI,
PostgreSQL) y generar scripts de instalación automatizada.
```
#### 3. Módulo de Variables de Entorno

```
Gestiona el switching entre perfiles (Dev/Qa/Prod) para herramientas de
orquestación como Kubernetes.
```
### Gestión de Activos IT:

Controla el flujo físico de los recursos de la empresa.

#### 4. Módulo de Inventario

```
Registro de equipos y periféricos.
```

#### 5. Módulo de Asignación:

```
Vinculación de hardware a legajos de empleados.
```
### Gestión de Seguridad y Acceso:

Abarca las políticas internas de la empresa para administrar quién tiene acceso a qué
recursos dentro del sistema y el respaldo de la información.

#### 6. Módulo de Autenticación

```
Validación de usuarios con encriptación de contraseñas.
```
#### 7. Módulo de Permisos (Patrón Composite)

```
Definición de roles como Administrador IT, Líder Técnico y Desarrollador.
```
## Organigrama de la Empresa

```
● Gerencia de Tecnología (CTO): Responsable de la estrategia de activos.
○ Líder Técnico: Define los perfiles de stack permitidos para cada proyecto.
■ Administrador de Infraestructura: Gestiona el hardware físico.
■ Desarrollador (Usuario Final): Consume los perfiles para configurar
su estación de trabajo.
```

## ——————————Segunda Entrega———————

## Diagramas de actividades de cada gestión.

### Gestión de Configuración y Perfiles (Core del negocio)


### Gestión de Activos IT

### Gestión de Seguridad y Acceso


## Casos de Uso

```
Identificador Nombre Descripción
CU-01 Restaurar Entorno de
Desarrollo
Genera scripts de
instalación y recupera
credenciales tras un
formateo.
CU-02 Definir Stack Tecnológico Configura las herramientas
base (Java, Angular, Maven,
Node) para un perfil.
CU-03 Asignar Hardware a
Empleado
Vincula un equipo físico
(MAC Address) al legajo de
un desarrollador.
CU-04 Gestionar Secretos
Personales
Carga y encripta archivos
sensibles (.npmrc,
settings.xml, tokens AD).
CU-05 Validar Asignación de
Hardware
Verifica que la PC actual
pertenezca al usuario.
(Incluido en CU-01).
```

### Especificación de Casos de Uso

#### CU-01: Restaurar Entorno de Desarrollo

**Actor:** Desarrollador
**Propósito:** Automatizar la instalación de herramientas y recuperar credenciales de forma
segura.
**Componente Detalle**
Precondiciones Usuario autenticado en el portal.
Usuario posee un perfil de stack y hardware
asignado.
Flujo Básico 1. El usuario selecciona la opción
"Restaurar mi Entorno".

2. El sistema invoca al CU-05 para validar
el hardware actual.
3. El sistema lee el perfil de stack del
usuario.
4. El sistema desencripta los archivos de
configuración (.npmrc, settings.xml).
5. El sistema ensambla un script instalador
(ej. PowerShell).
6. El usuario descarga y ejecuta el script.
7. El sistema registra la actividad en la
bitácora.


```
Flujo Alternativo FA-01: Validación de Hardware Fallida.
El sistema detiene el proceso, oculta los
secretos y notifica al Administrador IT.
Postcondiciones Entorno de desarrollo configurado y listo
para operar.
Reglas de Negocio RN-01 : La clave maestra de
desencriptación no debe viajar en texto
plano en el script.
```
#### CU-02: Definir Stack Tecnológico

**Actor:** Líder Técnico
**Propósito:** Crear plantillas con las versiones exactas de las herramientas a utilizar en un
proyecto.
**Componente Detalle**
Precondiciones Usuario con rol "Líder Técnico" (Patrón
Composite).
Flujo Básico 1. El usuario selecciona "Nuevo Perfil de
Stack".

2. Ingresa el nombre del perfil (ej. "Backend
Java/Spring").
3. Selecciona las herramientas de la lista


```
maestra (JDK 17, Maven, PostgreSQL).
```
4. Define variables de entorno globales
(Dev/Prod).
5. Guarda el perfil en la base de datos.
Flujo Alternativo FA-01: Herramienta Incompatible.
El sistema alerta si se seleccionan dos
versiones de la misma herramienta (ej. Java
8 y Java 17) y pide confirmación.
Postcondiciones Perfil disponible para ser asignado a los
Desarrolladores.
Reglas de Negocio RN-02: Todo perfil debe contener al menos
un compilador/intérprete y un gestor de
paquetes.

#### CU-03: Asignar Hardware a Empleado

**Actor:** Administrador IT
**Propósito:** Vincular una PC física al legajo de un usuario para garantizar la trazabilidad.
**Componente Detalle**


Precondiciones Equipo cargado previamente en el inventario.
Flujo Básico 1. El usuario ingresa a "Asignación de
Activos".

2. Busca el legajo del desarrollador.
3. Ingresa la MAC Address o N° de Serie del
equipo.
4. El sistema valida que el equipo esté
"Disponible".
5. El sistema confirma la vinculación y
actualiza el estado a "Asignado".
Flujo Alternativo FA-01: Equipo ya asignado.
El sistema bloquea la acción indicando a qué
legajo pertenece actualmente.
Postcondiciones Legajo y MAC Address vinculados en la base
de datos.
Reglas de Negocio RN-03: Un desarrollador no puede tener más
de un equipo principal asignado en
simultáneo.


## Diagrama de clases (UML)

## Diagramas de Secuencias (UML) de cada Caso de uso

#### CU-01: Restaurar Entorno de Desarrollo


#### CU-02: Definir Stack Tecnológico

## D.E.R

## Resumen

Este sistema transforma un problema técnico (el formateo de PCs) en una métrica de ahorro
para la empresa. Si un desarrollador tarda 2 días en volver a estar operativo, y logramos
automatizar y resguardar su entorno para que lo recupere en 2 horas, estamos aumentando
la productividad.