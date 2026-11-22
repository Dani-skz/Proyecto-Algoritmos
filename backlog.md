# Backlog — Fantasy Quest

Tareas que quedaron pendientes del sprint actual y pasan al backlog para
los siguientes. Orden sugerido por prioridad para la entrega final.

## Código

[ ] Avanzar algoritmo del juego, de la forma más sencilla posible
    - Estado: el algoritmo funciona de punta a punta (15 preguntas, cálculo
      de raza principal/secundaria, combinación y clase, persistencia en BDD).
    - Pendiente opcional: formalizar regla de desempate en `Perfil.CalcularResultado`
      (hoy funciona por orden de inserción del Dictionary, pero no está documentado
      como regla).

## Documentación

[X] Nuevo DER
    - Entregado.

[X] Diccionario de datos BDD y Algoritmo
    - Entregado en `Documentacion/Diccionario de datos.md`.

## Arte / Assets

[X] Acomodar / mejorar / quitar fondos blancos de las imágenes de razas
    para el fin del juego
    - Resuelto: los marcos warcraftcn-ui integrados en `FrmResultado` tapan
      los bordes y dan un acabado prolijo sobre cualquier fondo.

## Release y QA

[X] Pruebas / QA final antes de la entrega
    - Validadas manualmente por el equipo.

[ ] Build release / empaquetado final del `.exe` para la entrega
    - Compilar en modo Release.
    - Verificar que los recursos UI y audios estén embebidos.
    - Ajustar connection string antes de empaquetar (hoy apunta a localhost
      con credenciales de desarrollo).
