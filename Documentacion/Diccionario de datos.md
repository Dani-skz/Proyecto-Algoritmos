# Fantasy Quest — Diccionario de datos

Este documento describe todos los elementos de datos del sistema: por un lado
las tablas de la base de datos MySQL, y por otro las clases y enums del
algoritmo en C#. Los enums de C# se guardan en la BDD como texto con el
nombre exacto del valor del enum (`Enum.ToString()` al guardar y
`Enum.Parse()` al leer).

---

## 1. Base de datos

Base: `fantasy_quest_bdd` (MySQL)

### 1.1 Tabla `jugadores`

Registra a cada jugador que se registra en el juego.

| Columna | Tipo | Nulo | Clave | Default | Descripción |
| --- | --- | --- | --- | --- | --- |
| id_jugador | int | No | PK, autoincremental | — | Identificador único del jugador |
| nombre | varchar(100) | No | Único | — | Nombre visible del jugador, único en el sistema |
| fecha_alta | datetime | No | — | current_timestamp | Fecha y hora en que el jugador se registró |

**Restricciones:**
- `chk_nombre_jugador`: el nombre (sin espacios) tiene que tener al menos 2 caracteres.

### 1.2 Tabla `partidas`

Representa una sesión de juego de un jugador, desde que la inicia hasta que
la finaliza, cancela o abandona. Es la tabla principal del dominio.

| Columna | Tipo | Nulo | Clave | Default | Descripción |
| --- | --- | --- | --- | --- | --- |
| id_partida | int | No | PK, autoincremental | — | Identificador único de la partida |
| id_jugador | int | No | FK → jugadores | — | Jugador al que pertenece la partida |
| modo | varchar(30) | No | — | — | Modo de juego (`clasico` o `narrativo`, enum `modo_juego` en C#) |
| estado | enum | No | — | 'EN_CURSO' | Estado actual: `EN_CURSO`, `FINALIZADA` o `CANCELADA` |
| fecha_inicio | datetime | No | — | current_timestamp | Cuándo se inició la partida |
| fecha_fin | datetime | Sí | — | null | Cuándo terminó. Null mientras la partida sigue en curso |
| raza_principal | varchar(30) | Sí | — | null | Resultado: raza principal (enum `razas` en C#). Se asigna al finalizar |
| raza_secundaria | varchar(30) | Sí | — | null | Resultado: raza secundaria. Distinta de la principal |
| combinacion | varchar(50) | Sí | — | null | Resultado: combinación de las dos razas (enum `CombinacionRaza` en C#) |
| clase | varchar(30) | Sí | — | null | Resultado: clase final (enum `clase` en C#) |

**Restricciones:**
- `chk_fechas`: `fecha_fin` debe ser mayor o igual a `fecha_inicio`.
- `chk_modo`: el modo no puede estar vacío.
- `chk_razas_distintas`: la raza principal y la secundaria tienen que ser distintas.
- `chk_resultado`: si el estado es `FINALIZADA`, raza_principal, raza_secundaria, combinacion y clase no pueden ser null.
- `fk_partida_jugador`: FK hacia `jugadores(id_jugador)`.
- Índice: `idx_partida_jugador_estado (id_jugador, estado)` para búsquedas de partidas pendientes.

### 1.3 Tabla `respuestas_partida`

Guarda, en orden, las respuestas que el jugador fue eligiendo durante la
partida. Permite reanudar partidas sin terminar: C# vuelve a aplicar estas
respuestas sobre su lista de preguntas para reconstruir el perfil y saber
en qué pregunta continuar.

| Columna | Tipo | Nulo | Clave | Default | Descripción |
| --- | --- | --- | --- | --- | --- |
| id_partida | int | No | PK (compuesta), FK → partidas | — | Partida a la que pertenece la respuesta |
| nro_pregunta | int | No | PK (compuesta) | — | Posición de la pregunta en la lista de C# (0, 1, 2…) |
| nro_respuesta | int | No | — | — | Posición de la respuesta elegida dentro de la pregunta (0 a 5) |
| fecha | datetime | No | — | current_timestamp | Cuándo se respondió esta pregunta |

**Restricciones:**
- PK compuesta: `(id_partida, nro_pregunta)` — un jugador no puede responder dos veces la misma pregunta.
- `chk_nro_pregunta`: `nro_pregunta >= 0`.
- `chk_nro_respuesta`: `nro_respuesta >= 0`.
- `fk_rp_partida`: FK hacia `partidas(id_partida)`.

### 1.4 Tabla `puntajes_partida`

Guarda el puntaje final acumulado por cada raza y cada clase al finalizar
la partida. Tiene una fila por cada combinación `(partida, tipo, variable)`.

| Columna | Tipo | Nulo | Clave | Default | Descripción |
| --- | --- | --- | --- | --- | --- |
| id_partida | int | No | PK (compuesta), FK → partidas | — | Partida a la que pertenece el puntaje |
| tipo | enum | No | PK (compuesta) | — | `RAZA` o `CLASE`, indica qué se está puntuando |
| variable | varchar(30) | No | PK (compuesta) | — | Nombre del enum (ej: 'humano', 'guerrero') |
| puntos | int | No | — | 0 | Puntaje total acumulado para esa variable |

**Restricciones:**
- PK compuesta: `(id_partida, tipo, variable)`.
- `chk_puntos`: `puntos >= 0`.
- `chk_variable`: la variable no puede estar vacía.
- `fk_pp_partida`: FK hacia `partidas(id_partida)`.

### 1.5 Tabla `historial_partidas`

Log de auditoría: deja rastro de cada acción importante sobre una partida
(inicio, finalización, cancelación).

| Columna | Tipo | Nulo | Clave | Default | Descripción |
| --- | --- | --- | --- | --- | --- |
| id_historial | int | No | PK, autoincremental | — | Identificador único del registro de historial |
| id_partida | int | No | FK → partidas | — | Partida a la que corresponde la acción |
| accion | varchar(50) | No | — | — | Tipo de acción: `INICIO`, `FINALIZADA`, `CANCELADA` |
| fecha | datetime | No | — | current_timestamp | Cuándo ocurrió la acción |
| detalle | varchar(255) | Sí | — | null | Texto descriptivo opcional (ej: la combinación de resultado) |

**Restricciones:**
- `fk_his_partida`: FK hacia `partidas(id_partida)`.

### 1.6 Objetos adicionales de la BDD

**Procedimientos almacenados:**

| Nombre | Qué hace |
| --- | --- |
| sp_registrar_jugador | Registra un jugador nuevo o devuelve su id si ya existe. Parámetros IN: `p_nombre`. OUT: `p_id_jugador` |
| sp_modificar_jugador | Cambia el nombre de un jugador existente |
| sp_registrar_partida | Crea una nueva partida EN_CURSO. Falla si el jugador ya tiene una partida sin terminar. OUT: `p_id_partida` |
| sp_guardar_respuesta | Guarda una respuesta en orden. Falla si se saltea una pregunta |
| sp_guardar_puntaje | Guarda el puntaje total de una variable (raza o clase) en una partida |
| sp_finalizar_partida | Cierra la partida con el resultado final. Valida que estén cargados todos los puntajes necesarios |
| sp_cancelar_partida | Marca la partida como CANCELADA con un motivo opcional |

**Funciones:**

| Nombre | Qué devuelve |
| --- | --- |
| fn_partida_pendiente(id_jugador) | El `id_partida` de la partida `EN_CURSO` del jugador, o null si no tiene |

**Vistas:**

| Nombre | Qué muestra |
| --- | --- |
| vw_partidas_pendientes | Partidas `EN_CURSO` con jugador, modo, fecha de inicio y cantidad de respuestas dadas |
| vw_historial | Todas las partidas (cualquier estado) con jugador, fechas y resultado |

---

## 2. Algoritmo (clases C#)

Namespace raíz: `Fantasy_quest`

### 2.1 Clases del dominio

Namespace: `Fantasy_quest.Dominio`

#### `Jugador`

Representa a un jugador registrado en el sistema.

| Propiedad | Tipo | Descripción |
| --- | --- | --- |
| Id | int | Id del jugador en la BDD |
| Nombre | string | Nombre del jugador |

#### `Pregunta`

Una pregunta del banco de preguntas. El texto narrativo se muestra en
`FrmIntroduccion` y la pregunta con sus respuestas en `FrmPartida`.

| Propiedad | Tipo | Descripción |
| --- | --- | --- |
| Titulo | string | Nombre del capítulo, se muestra en la introducción |
| Historia | string | Introducción narrativa que se lee antes de la pregunta |
| Texto | string | La pregunta en sí (ej: "¿Qué desayunarías?") |
| Respuestas | List\<Respuesta\> | Hasta 6 respuestas posibles |

#### `Respuesta`

Una de las opciones de una pregunta. Cada respuesta suma puntos a una o
más razas y a una o más clases.

| Propiedad | Tipo | Descripción |
| --- | --- | --- |
| Texto | string | Texto de la respuesta, se muestra en el botón |
| PuntosRaza | Dictionary\<razas, int\> | Puntos que esta respuesta aporta a cada raza |
| PuntosClase | Dictionary\<clase, int\> | Puntos que esta respuesta aporta a cada clase |

**Método encadenable:**
- `Suma(razas, int)` / `Suma(clase, int)`: setea los puntos y devuelve la misma Respuesta.

#### `Perfil`

Puntos acumulados durante una partida. Se aplica cada respuesta sobre el
perfil y al final se calcula el resultado.

| Propiedad | Tipo | Descripción |
| --- | --- | --- |
| PuntosRaza | Dictionary\<razas, int\> | Total de puntos acumulados por cada raza. Todas arrancan en 0 |
| PuntosClase | Dictionary\<clase, int\> | Total de puntos acumulados por cada clase. Todas arrancan en 0 |

**Métodos:**
- `Aplicar(Respuesta)`: suma los puntos de la respuesta al perfil.
- `CalcularResultado(nombreJugador)`: devuelve el `ResultadoPartida` final ordenando razas y clases por puntos descendente.

#### `ResultadoPartida`

Resultado final de una partida: raza principal, secundaria, combinación y
clase, junto con el detalle de puntos.

| Propiedad | Tipo | Descripción |
| --- | --- | --- |
| NombreJugador | string? | Nombre del jugador. Null si fue partida anónima |
| RazaPrincipal | razas | Raza con más puntos |
| RazaSecundaria | razas | Segunda raza con más puntos |
| Combinacion | CombinacionRaza | Combinación resultante de principal + secundaria |
| Clase | clase | Clase con más puntos |
| PuntosRaza | Dictionary\<razas, int\> | Detalle de puntos por raza |
| PuntosClase | Dictionary\<clase, int\> | Detalle de puntos por clase |

**Método estático:**
- `ObtenerCombinacion(razaA, razaB)`: devuelve la `CombinacionRaza` correspondiente, sin importar el orden.

#### `BancoPreguntas` (static)

Devuelve el listado fijo de preguntas del juego mediante `Crear()`. Las
preguntas viven en el código, no en la BDD. La BDD guarda la POSICIÓN de
la pregunta y de la respuesta, por eso no se puede cambiar el orden de las
preguntas existentes (las nuevas se agregan al final).

### 2.2 Enums

Namespace: `Fantasy_quest.enums`

#### `razas`

Las 4 razas base del juego. Cada partida tiene una principal y una secundaria.

| Valor | Descripción |
| --- | --- |
| orco | Raza orca, asociada a fuerza, combate y caos |
| elfo | Raza élfica, asociada a magia, conocimiento y naturaleza |
| humano | Raza humana, versátil, aparece en muchas respuestas |
| enano | Raza enana, asociada a herrería, montañas y resistencia |

#### `clase`

Clase del personaje. Se elige la de mayor puntaje al finalizar.

| Valor | Descripción |
| --- | --- |
| arquero | Arquero |
| paladin | Paladín |
| picaro | Pícaro |
| guerrero | Guerrero |
| mago | Mago |
| granjero | Granjero (llamado "campesino" en el documento narrativo) |
| esclavo | Esclavo |
| herrero | Herrero |
| bufon | Bufón |
| explorador | Explorador |
| druida | Druida |
| hechicero | Hechicero |
| asesino | Asesino |

#### `CombinacionRaza`

Resultado de mezclar las dos razas con más puntos. El orden no importa:
humano + elfo es lo mismo que elfo + humano.

| Valor | Mezcla |
| --- | --- |
| semiorco | humano + orco |
| semielfo | humano + elfo |
| enanogigantismo | humano + enano |
| ElfoYunqu | elfo + enano |
| ElfoOscuro | elfo + orco |
| RompeMontaña | enano + orco |

#### `modo_juego`

Modo de juego. Se guarda en `partidas.modo` como texto. Actualmente solo
se usa `narrativo`.

| Valor | Descripción |
| --- | --- |
| clasico | Preguntas una tras otra, sin introducción narrada |
| narrativo | Un fragmento de historia antes de cada pregunta |
