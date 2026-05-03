# DESAFÍO

# NRO I

## TRABAJO FINAL

## Profesor: DARIO CARDACCI


El desafío nro 1 le propone desarrollar un módulo que permita administrar el acceso de
los usuarios al sistema. Se sugiere establecer dos momentos independientes.
El primero es el arranque y parada (start up - down) del sistema
El segundo es el log in – log out de los usuarios.
El arranque y parada del sistema debe utilizarse para poner en funcionamiento los
servicios que este dispone y desactivarlos al finalizar su ejecución.
En el caso del log in – log out, deberá cubrir la funcionalidad que permite identificar y
autenticar a los usuarios. Para el enfoque que adoptaremos identificar al usuario
significa corroborar que el mimo está dado de alta en el sistema y autenticarlo que el
usuario es quien dice ser. Puede evaluar para la identificación del usuario utilizar un
apodo o nombre de usuario y para la autenticación algún sistema de contraseña.
La asignación y utilización de contraseñas debe se dinámica, esto implica que al dar de
alta un usuario se le genera una contraseña automática, que solo conoce el
administrador y él. Con el primer ingreso al sistema se le solicitará que cambie la
contraseña, que deberá cumplir con los cánones básicos de una contraseña segura.
También se deberá considerar que el usuario puede olvidar su contraseña. Esto implica
derivarlo a un servicio de restauración de contraseñas. Puede considerar el uso de
palabras clave o bien algún sistema seguro.
El software deberá controlar el ciclo de vida de los nombres de usuarios y las
contraseñas. Vencido el plazo de validez deberá obligar al usuario a que ejecute su
cambio. El cambio del nombre de usuario y la contraseña deben contemplar que no se
ingresen valores similares a las ya utilizados o bien datos personales del usuario como
fecha de nacimiento, apellido etc.
También se debe observar que cuando un usuario intente ingresar al sistema de forma
errónea una cantidad de veces predeterminada, este sea bloqueado. El proceso de
desbloqueo del usuario debe ser parte de este módulo.
Los valores referidos a: intentos de accesos, características de la contraseña, tiempo de
vigencia, etc. deben ser parametrizados dentro del sistema para adecuarlos a la
configuración que desee darle el administrador.
Puede complementar este módulo con aquellos aspectos que mejoren su performance
o lo hagan más completo desde su funcionalidad.