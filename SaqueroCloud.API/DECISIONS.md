# Decisiones tecnicas

## Base de datos

He decidido usar SQLite para desarrollo por su simplicidad. En produccion usaria PostgreSQL.

## Arquitectura

Separacion en Controllers, Services y Repositories para mantener el codigo limpio y desacoplado.

## DTOs

Uso DTOs para no exponer directamente las entidades de base de datos.

## Autenticacion

JWT para autenticacion sin estado y proteccion de endpoints.
