# DESAFÍO

# NRO III

## TRABAJO FINAL

## Profesor: DARIO CARDACCI


El desafío nro 3 le propone desarrollar un módulo de servicio que opere como ORM
(Object Relational Mapping, Mapeador Objeto - Modelo Relacional) en su sistema.
La idea de este desafío es que diseñe la manera en que su sistema podrá descomponer
los grafos de objetos (entidades de su sistema) y persistirlos en la estructura relacional
que posee su base de datos.
Considere que la operación debe ser bidireccional. En un sentido, se deben transformar
los grafos de objetos en estructuras de datos que se puedan persistir en la base de datos
relacional.
En el sentido contrario, deberá poder transformar los datos obtenidos en las consultas
a la base de datos relacional, en grafos de objetos que su sistema gestionará para lograr
las funcionalidades por Ud. propuesta.
Este servicio de ORM deberá diseñarse como si fuera una capa para un sistema de
información.
El desafío implica también que la estructura de las clases que utilice para materializar el
objetivo posea el mayor nivel de reutilización posible.
Evalúe utilizar herencia, interfaces y genéricos en la implementación del servicio.