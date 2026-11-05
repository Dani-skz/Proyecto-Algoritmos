# DER v2 — fantasy_quest_bdd

Diagrama entidad-relación de la versión 2 de la base (`Base_de_datos/fantasy_quest_bdd.sql`), hecho con [Mermaid](https://mermaid.js.org/). GitHub y VS Code (con la extensión de vista previa de Markdown + Mermaid) lo dibujan directamente; para exportarlo como imagen se puede pegar el bloque en https://mermaid.live.

```mermaid
erDiagram
    jugadores ||--o{ partidas : "juega"
    partidas ||--o{ respuestas_partida : "guarda"
    partidas ||--o{ puntajes_partida : "obtiene"
    partidas ||--o{ historial_partidas : "registra"

    jugadores {
        int id_jugador PK "auto_increment"
        varchar(100) nombre UK "minimo 2 caracteres"
        datetime fecha_alta "default now"
    }

    partidas {
        int id_partida PK "auto_increment"
        int id_jugador FK
        varchar(30) modo "enum modo_juego de C#"
        enum estado "EN_CURSO, FINALIZADA, CANCELADA"
        datetime fecha_inicio "default now"
        datetime fecha_fin "null hasta terminar"
        varchar(30) raza_principal "enum razas de C#, null hasta terminar"
        varchar(30) raza_secundaria "enum razas de C#, distinta de la principal"
        varchar(50) combinacion "enum CombinacionRaza de C#"
        varchar(30) clase "enum clase de C#"
    }

    respuestas_partida {
        int id_partida PK, FK
        int nro_pregunta PK "posicion en la lista de C#, desde 0"
        int nro_respuesta "posicion de la respuesta elegida, 0 a 5"
        datetime fecha "default now"
    }

    puntajes_partida {
        int id_partida PK, FK
        enum tipo PK "RAZA, CLASE"
        varchar(30) variable PK "nombre del enum razas o clase"
        int puntos "mayor o igual a 0"
    }

    historial_partidas {
        int id_historial PK "auto_increment"
        int id_partida FK
        varchar(50) accion "INICIO, FINALIZADA, CANCELADA"
        datetime fecha "default now"
        varchar(255) detalle
    }
```

## Reglas que no se ven en el diagrama

- **Una sola partida `EN_CURSO` por jugador**: lo controla `sp_registrar_partida` (índice `idx_partida_jugador_estado` sobre `id_jugador, estado`).
- **Partida `FINALIZADA` con resultado completo**: `chk_resultado` exige `raza_principal`, `raza_secundaria`, `combinacion` y `clase` no nulos.
- **Fechas**: `chk_fechas` exige `fecha_fin >= fecha_inicio`.
- **Respuestas en orden**: `sp_guardar_respuesta` solo acepta la pregunta siguiente a la última guardada.
- **Sin tablas de catálogo**: razas, clases, combinaciones y modos de juego viven en los enums de C# y se guardan como texto (`Enum.ToString()` / `Enum.Parse()`). En la v1 eran tablas propias (`razas`, `clases`, `combinaciones_raza`, `modos_juego`, `estados_partida`).

## Vistas y procedimientos

| Objeto | Tipo | Usa |
| --- | --- | --- |
| `vw_partidas_pendientes` | vista | `partidas`, `jugadores`, `respuestas_partida` |
| `vw_historial` | vista | `partidas`, `jugadores` |
| `fn_partida_pendiente` | función | `partidas` |
| `sp_registrar_jugador`, `sp_modificar_jugador` | procedimientos | `jugadores` |
| `sp_registrar_partida`, `sp_finalizar_partida`, `sp_cancelar_partida` | procedimientos | `partidas`, `historial_partidas` |
| `sp_guardar_respuesta` | procedimiento | `respuestas_partida` |
| `sp_guardar_puntaje` | procedimiento | `puntajes_partida` |
