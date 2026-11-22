# Fantasy Quest — Diccionario de datos

Versión final · 22 de noviembre de 2026

Este documento describe todos los elementos de datos del sistema: las tablas y objetos de la base de datos MySQL, las clases y enums del algoritmo en C#, y las constantes y reglas que unen las dos capas. Los enums de C# se guardan en la BDD como texto con el nombre exacto del valor (`Enum.ToString()` al guardar y `Enum.Parse()` al leer).

---

## 1. Base de datos

Base: `fantasy_quest_bdd` (MySQL / MariaDB). Script: `Base_de_datos/fantasy_quest_bdd.sql` (versión 2; la versión 1 con tablas de razas, clases y modos queda comentada al final del script).

### 1.1 Tabla `jugadores`

Registra a cada jugador que se registra en el juego.

| Columna | Tipo | Nulo | Clave | Default | Descripción |
| --- | --- | --- | --- | --- | --- |
| id_jugador | int | No | PK, autoincremental | — | Identificador único del jugador |
| nombre | varchar(100) | No | Único | — | Nombre visible del jugador, único en el sistema. C# lo limita a 30 caracteres y le hace trim |
| contrasena_hash | char(64) | No | — | — | Hash SHA-256 en hexadecimal (minúsculas) de la contraseña. Nunca se guarda en claro: C# hashea antes de llamar al SP |
| fecha_alta | datetime | No | — | current_timestamp | Fecha y hora en que el jugador se registró |

**Restricciones:**
- `chk_nombre_jugador`: el nombre (con trim) tiene que tener al menos 2 caracteres.
- `chk_contrasena_hash`: el hash tiene que tener exactamente 64 caracteres.

### 1.2 Tabla `partidas`

Una sesión de juego de un jugador, desde que la inicia hasta que la finaliza o la cancela. Es la tabla principal del dominio.

| Columna | Tipo | Nulo | Clave | Default | Descripción |
| --- | --- | --- | --- | --- | --- |
| id_partida | int | No | PK, autoincremental | — | Identificador único de la partida |
| id_jugador | int | No | FK → jugadores | — | Jugador al que pertenece la partida |
| modo | varchar(30) | No | — | — | Modo de juego (enum `modo_juego`). La versión final guarda siempre `narrativo` |
| estado | enum | No | — | 'EN_CURSO' | `EN_CURSO`, `FINALIZADA` o `CANCELADA` |
| fecha_inicio | datetime | No | — | current_timestamp | Cuándo se inició la partida |
| fecha_fin | datetime | Sí | — | null | Cuándo terminó. Null mientras sigue en curso |
| raza_principal | varchar(30) | Sí | — | null | Raza con más puntos (enum `razas`). Se asigna al finalizar |
| raza_secundaria | varchar(30) | Sí | — | null | Segunda raza con más puntos. Se guarda siempre, también si el resultado es puro |
| combinacion | varchar(50) | Sí | — | null | Raza resultante (enum `CombinacionRaza`): una de las 6 mestizas o una de las 4 puras |
| clase | varchar(30) | Sí | — | null | Clase final (enum `clase`) |

**Restricciones:**
- `chk_fechas`: `fecha_fin` null o mayor o igual a `fecha_inicio`.
- `chk_modo`: el modo no puede estar vacío.
- `chk_razas_distintas`: la raza principal y la secundaria tienen que ser distintas.
- `chk_resultado`: si el estado es `FINALIZADA`, raza_principal, raza_secundaria, combinacion y clase no pueden ser null.
- `fk_partida_jugador`: FK hacia `jugadores(id_jugador)`.
- Índice `idx_partida_jugador_estado (id_jugador, estado)` para buscar la partida pendiente.
- Regla de negocio (en `sp_registrar_partida`): un jugador puede tener una sola partida `EN_CURSO`.

### 1.3 Tabla `respuestas_partida`

Guarda, en orden, las respuestas elegidas durante la partida. Permite reanudar: C# vuelve a aplicar estas respuestas sobre el banco de preguntas para reconstruir el perfil y saber en qué pregunta seguir.

| Columna | Tipo | Nulo | Clave | Default | Descripción |
| --- | --- | --- | --- | --- | --- |
| id_partida | int | No | PK (compuesta), FK → partidas | — | Partida a la que pertenece la respuesta |
| nro_pregunta | int | No | PK (compuesta) | — | Posición de la pregunta en `BancoPreguntas` (0 a 14) |
| nro_respuesta | int | No | — | — | Posición de la respuesta elegida dentro de la pregunta (0 a 5) |
| fecha | datetime | No | — | current_timestamp | Cuándo se respondió |

**Restricciones:**
- PK compuesta `(id_partida, nro_pregunta)`: no se puede responder dos veces la misma pregunta.
- `chk_nro_pregunta`: `nro_pregunta >= 0`. `chk_nro_respuesta`: `nro_respuesta >= 0`.
- `fk_rp_partida`: FK hacia `partidas(id_partida)`.
- Regla de negocio (en `sp_guardar_respuesta`): `nro_pregunta` tiene que ser el siguiente al último guardado (no se saltean preguntas).

### 1.4 Tabla `puntajes_partida`

Puntaje final acumulado por cada raza y cada clase al finalizar la partida. C# guarda todas las variables, también las de 0 puntos: 4 filas `RAZA` + 13 filas `CLASE` = 17 filas por partida finalizada.

| Columna | Tipo | Nulo | Clave | Default | Descripción |
| --- | --- | --- | --- | --- | --- |
| id_partida | int | No | PK (compuesta), FK → partidas | — | Partida a la que pertenece el puntaje |
| tipo | enum | No | PK (compuesta) | — | `RAZA` o `CLASE` |
| variable | varchar(30) | No | PK (compuesta) | — | Nombre del enum (ej: 'humano', 'guerrero') |
| puntos | int | No | — | 0 | Puntaje total acumulado |

**Restricciones:**
- PK compuesta `(id_partida, tipo, variable)`.
- `chk_puntos`: `puntos >= 0`. `chk_variable`: la variable no puede estar vacía.
- `fk_pp_partida`: FK hacia `partidas(id_partida)`.

### 1.5 Tabla `historial_partidas`

Log de auditoría de cada acción importante sobre una partida.

| Columna | Tipo | Nulo | Clave | Default | Descripción |
| --- | --- | --- | --- | --- | --- |
| id_historial | int | No | PK, autoincremental | — | Identificador del registro |
| id_partida | int | No | FK → partidas | — | Partida a la que corresponde la acción |
| accion | varchar(50) | No | — | — | `INICIO`, `FINALIZADA` o `CANCELADA` |
| fecha | datetime | No | — | current_timestamp | Cuándo ocurrió |
| detalle | varchar(255) | Sí | — | null | Texto libre. Al finalizar: `combinacion (principal + secundaria), clase X`. Al cancelar: el motivo (ej: "Abandonada por el jugador") |

**Restricciones:**
- `fk_his_partida`: FK hacia `partidas(id_partida)`.

### 1.6 Procedimientos almacenados

Todos validan con `signal sqlstate '45000'`; C# muestra ese mensaje tal cual al usuario (`Mensajes.MostrarError`, error MySQL 1644).

| Nombre | Parámetros | Qué hace |
| --- | --- | --- |
| sp_registrar_jugador | IN `p_nombre`, IN `p_contrasena_hash`, OUT `p_id_jugador` | Registra un jugador. Falla si el nombre tiene menos de 2 caracteres, falta la contraseña o el nombre ya existe |
| sp_modificar_jugador | IN `p_id_jugador`, IN `p_nombre` | Cambia el nombre de un jugador. Falla si no existe o si el nombre nuevo ya lo usa otro jugador (no se usa desde la UI) |
| sp_registrar_partida | IN `p_id_jugador`, IN `p_modo`, OUT `p_id_partida` | Crea una partida `EN_CURSO` y registra `INICIO` en el historial. Falla si el jugador no existe o ya tiene una partida sin terminar |
| sp_guardar_respuesta | IN `p_id_partida`, IN `p_nro_pregunta`, IN `p_nro_respuesta` | Guarda una respuesta. Falla si la partida no está en curso, la respuesta es inválida o la pregunta no es la siguiente en orden |
| sp_guardar_puntaje | IN `p_id_partida`, IN `p_tipo`, IN `p_variable`, IN `p_puntos` | Guarda el puntaje de una raza o clase. Falla con tipo inválido, variable vacía o puntos negativos |
| sp_finalizar_partida | IN `p_id_partida`, IN `p_raza_principal`, IN `p_raza_secundaria`, IN `p_combinacion`, IN `p_clase` | Pasa la partida a `FINALIZADA` con el resultado, en transacción. Exige respuestas, razas distintas y puntaje guardado para la raza principal, la secundaria y la clase |
| sp_cancelar_partida | IN `p_id_partida`, IN `p_motivo` | Pasa la partida a `CANCELADA` y registra el motivo. Solo se puede cancelar una partida en curso |

El login no usa un SP: `sql_repository.IniciarSesion` lee el hash con un `select` parametrizado y lo compara en C#.

### 1.7 Funciones y vistas

| Nombre | Tipo | Devuelve / muestra |
| --- | --- | --- |
| fn_partida_pendiente(id_jugador) | Función | `id_partida` de la partida `EN_CURSO` más reciente del jugador, o null |
| vw_partidas_pendientes | Vista | Partidas `EN_CURSO`: id_partida, id_jugador, jugador, modo, fecha_inicio, preguntas_respondidas |
| vw_historial | Vista | Todas las partidas: id_partida, jugador, modo, estado, fecha_inicio, fecha_fin, raza_principal, raza_secundaria, combinacion, clase. La usan el Scoreboard y "Ver resultado" |

---

## 2. Algoritmo (clases C#)

Namespace raíz: `Fantasy_quest`. Proyecto WinForms en .NET 9 con `UseWPF` (para el `MediaPlayer`).

### 2.1 Clases del dominio

Namespace: `Fantasy_quest.Dominio`

#### `Jugador`

Jugador registrado. Inmutable.

| Propiedad | Tipo | Descripción |
| --- | --- | --- |
| Id | int | `jugadores.id_jugador` |
| Nombre | string | `jugadores.nombre` |

#### `Pregunta`

Una pregunta del banco. La historia se muestra en `FrmIntroduccion` y la pregunta en `FrmPartida`.

| Propiedad | Tipo | Descripción |
| --- | --- | --- |
| Titulo | string | Nombre del capítulo |
| Historia | string | Texto narrativo que se lee antes de la pregunta |
| Texto | string | La pregunta en sí (ej: "¿Qué desayunarías?") |
| Respuestas | List\<Respuesta\> | Máximo 6. En la versión final todas tienen 6 |

#### `Respuesta`

Una opción de una pregunta. Suma puntos a una o más razas y clases.

| Propiedad | Tipo | Descripción |
| --- | --- | --- |
| Texto | string | Texto del botón |
| PuntosRaza | Dictionary\<razas, int\> | Puntos que aporta a cada raza |
| PuntosClase | Dictionary\<clase, int\> | Puntos que aporta a cada clase |

**Método encadenable:** `Suma(razas, int)` / `Suma(clase, int)` setea los puntos y devuelve la misma Respuesta.

#### `Perfil`

Puntos acumulados durante una partida.

| Propiedad | Tipo | Descripción |
| --- | --- | --- |
| PuntosRaza | Dictionary\<razas, int\> | Total por raza. Las 4 arrancan en 0 |
| PuntosClase | Dictionary\<clase, int\> | Total por clase. Las 13 arrancan en 0 |

**Métodos:**
- `Aplicar(Respuesta)`: suma los puntos de la respuesta.
- `CalcularResultado(nombreJugador)`: arma el `ResultadoPartida` con estas reglas:
  1. Ordena las razas por puntos descendente: la primera es la **principal** y la segunda la **secundaria**.
  2. **Mestiza** si `puntos(secundaria) × 3 >= total de puntos de raza` (la secundaria llega al 33 %); se calcula con enteros para no arrastrar el redondeo de 1/3. Si no, el resultado es la raza **pura** de la principal.
  3. La clase es la de más puntos.
  4. Desempate: a igual puntaje gana la que está primero en el enum (pendiente de formalizar).

#### `ResultadoPartida`

Resultado final de una partida. Inmutable.

| Propiedad | Tipo | Descripción |
| --- | --- | --- |
| NombreJugador | string? | Null si fue partida anónima |
| RazaPrincipal | razas | Raza con más puntos |
| RazaSecundaria | razas | Segunda raza con más puntos |
| Combinacion | CombinacionRaza | Raza resultante (mestiza o pura) |
| Clase | clase | Clase con más puntos |
| PuntosRaza | Dictionary\<razas, int\> | Copia del detalle por raza |
| PuntosClase | Dictionary\<clase, int\> | Copia del detalle por clase |

**Métodos estáticos:**
- `ObtenerCombinacion(razaA, razaB)`: la mestiza correspondiente, sin importar el orden.
- `ObtenerPura(raza)`: el valor puro de `CombinacionRaza` para esa raza.

#### `BancoPreguntas` (static)

`Crear()` devuelve las 15 preguntas fijas del juego. Viven en el código, no en la BDD; la BDD guarda la **posición** de la pregunta y de la respuesta, por eso no se puede cambiar el orden de las existentes (las nuevas van al final). El texto sale de `Documentacion/pregunta.md`.

### 2.2 Enums

Namespace: `Fantasy_quest.enums`

#### `razas`

| Valor | Descripción |
| --- | --- |
| orco | Fuerza, combate y caos |
| elfo | Magia, conocimiento y naturaleza |
| humano | Versátil, aparece en muchas respuestas |
| enano | Herrería, montañas y resistencia |

#### `clase`

| Valor | Descripción |
| --- | --- |
| arquero | Arquero |
| paladin | Paladín |
| picaro | Pícaro |
| guerrero | Guerrero |
| mago | Mago |
| granjero | Granjero ("campesino" en el documento narrativo) |
| esclavo | Esclavo |
| herrero | Herrero |
| bufon | Bufón |
| explorador | Explorador |
| druida | Druida |
| hechicero | Hechicero |
| asesino | Asesino |

#### `CombinacionRaza`

Raza resultante. Las 6 primeras son mestizas (orden indistinto); las 4 últimas, puras.

| Valor | Tipo | Origen | Imagen |
| --- | --- | --- | --- |
| semiorco | Mestiza | humano + orco | raza_semiorco.png |
| semielfo | Mestiza | humano + elfo | raza_semielfo.png |
| enanogigantismo | Mestiza | humano + enano | raza_enanogigantismo.png |
| ElfoYunqu | Mestiza | elfo + enano | raza_elfoyunqu.png |
| ElfoOscuro | Mestiza | elfo + orco | raza_elfooscuro.png |
| RompeMontaña | Mestiza | enano + orco | raza_rompemontana.png |
| humano | Pura | secundaria < 33 % | raza_humano.png |
| elfo | Pura | secundaria < 33 % | raza_elfo.png |
| orco | Pura | secundaria < 33 % | raza_orco.png |
| enano | Pura | secundaria < 33 % | raza_enano.png |

#### `modo_juego`

Se guarda en `partidas.modo`. `GestorPartida` usa siempre `narrativo`.

| Valor | Descripción |
| --- | --- |
| clasico | Preguntas una tras otra, sin introducción narrada |
| narrativo | Un fragmento de historia antes de cada pregunta |

#### `ModoIngreso` (`Fantasy_quest.Formularios`)

| Valor | Descripción |
| --- | --- |
| Registro | `FrmIngreso` registra un jugador nuevo |
| Login | `FrmIngreso` inicia sesión con un jugador existente |

### 2.3 Gestoras

Namespace: `Fantasy_quest.Gestoras`

#### `GestorPartida`

Coordina una partida. Registrada: cada respuesta se guarda al momento. Anónima: todo queda en memoria hasta `GuardarEn`.

| Propiedad | Tipo | Descripción |
| --- | --- | --- |
| Jugador | Jugador? | Null en partida anónima |
| IdPartida | int? | `partidas.id_partida`; null en partida anónima |
| Preguntas | List\<Pregunta\> | Copia de `BancoPreguntas.Crear()` |
| Perfil | Perfil | Puntos acumulados |
| Resultado | ResultadoPartida? | Null hasta que termina |
| EsAnonima | bool | `Jugador == null` |
| IndiceActual | int | Cantidad de respuestas dadas = índice de la pregunta actual |
| Terminada | bool | `IndiceActual >= Preguntas.Count` |
| PreguntaActual | Pregunta | `Preguntas[IndiceActual]` |

| Método | Descripción |
| --- | --- |
| NuevaAnonima / Nueva / Reanudar | Fábricas. `Reanudar` reaplica las respuestas guardadas y falla si no coinciden con el banco actual |
| Responder(nroRespuesta) | Guarda primero en la BDD (si es registrada) y después aplica al perfil |
| Finalizar() | Calcula el resultado, guarda los 17 puntajes y llama a `sp_finalizar_partida` |
| GuardarEn(jugador) | Vuelca una partida anónima terminada a la BDD a nombre del jugador recién registrado |

#### `sql_repository`

Única puerta de los formularios hacia la BDD. Traduce enums ↔ texto y hashea contraseñas (SHA-256 hex).

| Método | Objeto de BDD |
| --- | --- |
| RegistrarJugadorNuevo | sp_registrar_jugador |
| IniciarSesion | select sobre jugadores |
| ModificarJugador | sp_modificar_jugador |
| RegistrarPartida | sp_registrar_partida |
| GuardarRespuesta | sp_guardar_respuesta |
| GuardarPuntaje / GuardarPuntajes | sp_guardar_puntaje |
| FinalizarPartida | sp_finalizar_partida |
| CancelarPartida | sp_cancelar_partida |
| ObtenerPartidaPendiente | fn_partida_pendiente |
| ObtenerRespuestas | select sobre respuestas_partida |
| ObtenerResultado | vw_historial + puntajes_partida |
| ObtenerScoreboard | vw_historial (solo `FINALIZADA`, por fecha_fin desc) |
| ObtenerPartidasPendientes / ObtenerHistorial / ObtenerHistorialJugador | vw_partidas_pendientes / vw_historial |

#### `DAO_gestora`

Conexión y ejecución genérica (`EjecutarNoQuery`, `EjecutarEscalar`, `EjecutarConsulta`, `EjecutarProcedimiento`, `EjecutarProcedimientoConSalida`, `ProbarConexion`). La cadena de conexión está en el constructor y apunta a `localhost:3306/fantasy_quest_bdd`.

### 2.4 Datos de presentación y audio

Namespace: `Fantasy_quest.ui`

#### `Pista`

Un canal de audio (WPF `MediaPlayer`). Suena un archivo a la vez.

| Propiedad / método | Tipo | Descripción |
| --- | --- | --- |
| Volumen | int | 0 a 100 (clamp). Se aplica como `Volume = Volumen / 100.0` |
| Silenciado | bool | Mutea sin cortar: al reactivar sigue por donde iba |
| Reproducir(nombre, repetir) | — | Extrae el recurso embebido a `%TEMP%\Fantasy_quest` y lo reproduce; con `repetir` vuelve a empezar al terminar |
| Detener() | — | Corta y cierra el archivo |

#### `Sonido` (static)

| Miembro | Valor | Descripción |
| --- | --- | --- |
| MusicaFondo | "standard sound.m4a" | Música en bucle de todo el juego |
| CancionFinal | "ending song.mp3" | Suena en el resultado de fin de partida |
| Narracion | Pista | Canal de la voz; `NarrarCapitulo(n)` reproduce `Narracion-n.mp3` |
| Musica | Pista | Canal de la música |

#### Recursos visuales (`Tema`)

| Diccionario | Clave → valor | Uso |
| --- | --- | --- |
| AvataresCombinacion | CombinacionRaza → `raza_*.png` (10) | Imagen central del avatar |
| MarcosRaza | razas → `marco_*.png` (4; enano usa `marco_default.png`) | Marco del avatar según la raza principal |
| IconosClase | clase → `clase_*.png` (13) | Ícono de la clase en el resultado |

### 2.5 Constantes y reglas de validación

| Constante | Valor | Dónde | Regla |
| --- | --- | --- | --- |
| LargoMinimo / LargoMaximo | 2 / 30 | FrmIngreso | Largo del nombre (después del trim) |
| CaracteresValidos | `^[A-Za-zÁÉÍÓÚáéíóúÑñÜü0-9 ]+$` | FrmIngreso | Letras con acentos, ñ, números y espacios; sin espacios dobles |
| LargoMinimoContrasena / LargoMaximoContrasena | 4 / 30 | FrmIngreso | Largo de la contraseña (sin trim) |
| Umbral mestizo | 1/3 del total | Perfil | `secundaria × 3 >= total` |
| Máximo de respuestas | 6 | BancoPreguntas / FrmPartida | 6 botones fijos en la pregunta |
| BarraVolumen.PasoRueda | 5 | ui | Paso de la rueda del mouse en los sliders de volumen |
| BarraDesplazamiento.PasoRueda | 40 px | ui | Paso de la rueda en el texto de la introducción |
| BarraTitulo.Alto | 32 px | ui | Alto de la barra de título propia |
| Mensajes.ErrorSignal | 1644 | Formularios | Número de error MySQL de `signal sqlstate '45000'` |

---

## 3. Correspondencia C# ↔ BDD

| Dato en C# | Columna en BDD | Conversión |
| --- | --- | --- |
| Jugador.Id / Nombre | jugadores.id_jugador / nombre | Directa (nombre con trim) |
| contraseña (string) | jugadores.contrasena_hash | SHA-256 → hex de 64 caracteres |
| GestorPartida.IdPartida | partidas.id_partida | OUT de sp_registrar_partida |
| modo_juego | partidas.modo | `ToString()` / `Enum.Parse` |
| ResultadoPartida.RazaPrincipal / RazaSecundaria | partidas.raza_principal / raza_secundaria | `ToString()` / `Enum.Parse` |
| ResultadoPartida.Combinacion | partidas.combinacion | `ToString()` / `Enum.Parse` (10 valores) |
| ResultadoPartida.Clase | partidas.clase | `ToString()` / `Enum.Parse` |
| GestorPartida.IndiceActual | respuestas_partida.nro_pregunta | Posición 0-based en BancoPreguntas |
| índice del botón elegido | respuestas_partida.nro_respuesta | Posición 0-based en Pregunta.Respuestas |
| Perfil.PuntosRaza / PuntosClase | puntajes_partida (tipo RAZA / CLASE) | Una fila por clave del diccionario |
