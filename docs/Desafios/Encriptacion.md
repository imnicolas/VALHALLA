# DESAFÍO

# NRO II

## TRABAJO FINAL

## Profesor: DARIO CARDACCI


El desafío nro 2 le propone desarrollar un módulo de servicios que permita encriptar los
datos sensibles de su sistema.
Le sugerimos que para encarar este desafío primero defina el concepto de dato sensible
y luego los identifique dentro de su sistema.
La consigna establece que seleccione un método de encriptación seguro y lo aplique en
sus datos sensibles. Los datos encriptados deben permanecer siempre en ese estado
mientras estén en un medio de almacenamiento secundario.
Los datos encriptados solo se deberán desencriptar cuando se expongan en las
interfaces gráficas de su sistema.
Considere la posibilidad que un usuario no autorizado ingrese a su base de datos y altere
la información encriptada, borre un registro o bien agregue un registro inexistente.
Elabore un plan que permita detectar esto al realizar el arranque del sistema. Si se
detectan inconsistencia evite que el sistema inicie. El sistema debe otorgarle al
administrador la posibilidad de detectar donde se produce la inconsistencia y
solucionarla.
Para resolver este desafío puede considerar el uso de claves hash o bien dígitos
verificadores a nivel de registros y tablas.