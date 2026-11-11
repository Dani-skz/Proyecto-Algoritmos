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

[ ] Nuevo DER
    - Actualizar el diagrama entidad-relación con el esquema final de la BDD
      (tablas: jugadores, partidas, respuestas_partida, puntajes_partida,
      historial_partidas).

[ ] Diccionario de datos BDD y Algoritmo
    - Diccionario de datos de la BDD: cada tabla, columna, tipo, PK/FK y descripción.
    - Diccionario del algoritmo: enums (razas, clase, CombinacionRaza, modo_juego),
      clases del dominio y su rol.

## Arte / Assets

[ ] Acomodar / mejorar / quitar fondos blancos de las imágenes de razas
    para el fin del juego
    - Afecta la pantalla de resultado (`FrmResultado`) donde se muestra el
      avatar según la combinación de razas.

## Release y QA

[ ] Pruebas / QA final antes de la entrega
    - Validar flujo completo: registro, login, partida registrada, partida
      anónima, reanudar partida, scoreboard (ambas pestañas), audios.
    - Probar con BDD caída (mensaje "sin conexión", jugar anónimo).
    - Probar validaciones de formulario de ingreso (vacío, corto, largo,
      caracteres inválidos, duplicado).

[ ] Build release / empaquetado final del `.exe` para la entrega
    - Compilar en modo Release.
    - Verificar que los recursos UI y audios estén embebidos.
    - Ajustar connection string antes de empaquetar (hoy apunta a localhost
      con credenciales de desarrollo).
