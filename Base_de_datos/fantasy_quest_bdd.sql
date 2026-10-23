-- base de datos para proyecto final de algoritmos

create database if not exists fantasy_quest_bdd;

use fantasy_quest_bdd;

-- ============================ VERSION 2 ==================================
-- La base SOLO guarda jugadores, partidas y su progreso (para poder
-- reanudar partidas sin terminar) y el resultado final.
--
-- Razas, clases, combinaciones y modos de juego viven en los enums de C#
-- (razas, clase, CombinacionRaza, modo de juego). En la base se guardan
-- como texto con el nombre exacto del enum: Enum.ToString() al guardar y
-- Enum.Parse() al leer. Ej: 'elfo', 'semielfo', 'guerrero'.
--
-- La version 1 queda comentada al final del archivo.

-- ============================ DROPS ======================================
-- procedimientos / funciones / vistas 
drop procedure if exists sp_registrar_jugador;
drop procedure if exists sp_modificar_jugador;
drop procedure if exists sp_registrar_partida;
drop procedure if exists sp_guardar_respuesta;
drop procedure if exists sp_guardar_puntaje;
drop procedure if exists sp_finalizar_partida;
drop procedure if exists sp_cancelar_partida;
drop function if exists fn_partida_pendiente;
drop view if exists vw_partidas_pendientes;
drop view if exists vw_historial;

-- tablas (primero las hijas por las FK)
drop table if exists historial_partidas;
drop table if exists respuestas_partida;
drop table if exists puntajes_partida;
drop table if exists puntajes_raza;       -- v1, reemplazada por puntajes_partida
drop table if exists puntajes_clase;      -- v1, reemplazada por puntajes_partida
drop table if exists partidas;
drop table if exists combinaciones_raza;  -- v1, ahora enum CombinacionRaza en C#
drop table if exists razas;               -- v1, ahora enum razas en C#
drop table if exists clases;              -- v1, ahora enum clase en C#
drop table if exists modos_juego;         -- v1, ahora enum en C#
drop table if exists estados_partida;     -- v1, ahora ENUM en partidas
drop table if exists jugadores;

-- =============== TABLAS JUGADORES ===================
create table jugadores(
	id_jugador int auto_increment primary key,
    nombre varchar(100) not null unique,
    fecha_alta datetime not null default current_timestamp,
    constraint chk_nombre_jugador check(char_length(trim(nombre))>= 2)
);

-- ============== partidas =====================
create table partidas( -- main table
	id_partida int auto_increment primary key,
    id_jugador int not null,
    modo varchar(30) not null, -- enum de modo de juego en C#
    estado enum('EN_CURSO', 'FINALIZADA', 'CANCELADA') not null default 'EN_CURSO',
    fecha_inicio datetime not null default current_timestamp,
    fecha_fin datetime null,
    -- resultado: se asigna al terminar
    raza_principal varchar(30) null,  -- enum razas
    raza_secundaria varchar(30) null, -- enum razas
    combinacion varchar(50) null,     -- enum CombinacionRaza
    clase varchar(30) null,           -- enum clase
    constraint chk_fechas check(fecha_fin is null or fecha_fin >= fecha_inicio),
    constraint chk_modo check(char_length(trim(modo)) > 0),
    constraint chk_razas_distintas check(raza_principal is null or raza_secundaria is null
                                         or raza_principal <> raza_secundaria),
    -- una partida finalizada tiene que tener su resultado completo
    constraint chk_resultado check(estado <> 'FINALIZADA'
                                   or (raza_principal is not null and raza_secundaria is not null
                                       and combinacion is not null and clase is not null)),
    constraint fk_partida_jugador foreign key (id_jugador) references jugadores(id_jugador),
    index idx_partida_jugador_estado (id_jugador, estado)
);

-- respuestas dadas: permite reanudar una partida sin terminar.
-- C# vuelve a aplicar estas respuestas sobre su lista de preguntas y
-- reconstruye el perfil (puntos) y la pregunta actual.
create table respuestas_partida(
    id_partida int not null,
    nro_pregunta int not null,   -- posicion de la pregunta en la lista de C# (0, 1, 2...)
    nro_respuesta int not null,  -- posicion de la respuesta elegida (0..5)
    fecha datetime not null default current_timestamp,
    primary key (id_partida, nro_pregunta),
    constraint chk_nro_pregunta check (nro_pregunta >= 0),
    constraint chk_nro_respuesta check (nro_respuesta >= 0),
    constraint fk_rp_partida foreign key (id_partida) references partidas(id_partida)
);

-- puntaje final de cada variable (una fila por raza / clase)
create table puntajes_partida(
    id_partida int not null,
    tipo enum('RAZA', 'CLASE') not null,
    variable varchar(30) not null, -- nombre del enum razas o clase
    puntos int not null default 0,
    primary key (id_partida, tipo, variable),
    constraint chk_puntos check (puntos >= 0),
    constraint chk_variable check (char_length(trim(variable)) > 0),
    constraint fk_pp_partida foreign key (id_partida) references partidas(id_partida)
);

-- ======================== historial ====================

create table historial_partidas(
	id_historial int auto_increment primary key,
    id_partida int not null,
    accion varchar(50) not null, -- INICIO, FINALIZADA, CANCELADA
	fecha datetime not null default current_timestamp,
    detalle varchar(255),
    constraint fk_his_partida foreign key (id_partida) references partidas(id_partida)
);

-- ===================== Procediimentos ==============================
-- sp_registrar_jugador, sp_modificar_jugador, sp_registrar_partida,
-- sp_guardar_respuesta, sp_guardar_puntaje, sp_finalizar_partida, sp_cancelar_partida

-- =================== // REGISTRAR JUGADOR \\ ========================
-- si el jugador ya existe devuelve su id (asi puede reanudar su partida)
DELIMITER //
create procedure sp_registrar_jugador(
    in  p_nombre varchar(100),
    out p_id_jugador int
)
begin
    set p_id_jugador = null;
    -- limpiar espacios
    set p_nombre = trim(p_nombre);

    -- validar nombre (mismo criterio que el CHECK de la tabla)
    if p_nombre is null or char_length(p_nombre) < 2 then
        signal sqlstate '45000'
            set message_text = 'El nombre debe tener al menos 2 caracteres';
    end if;

    select id_jugador into p_id_jugador
    from jugadores where nombre = p_nombre;

    if p_id_jugador is null then
        insert into jugadores (nombre)
        values (p_nombre);
        set p_id_jugador = last_insert_id();
    end if;
end //
DELIMITER ;

-- ===================== // MODIFICAR JUGADOR \\ =====================
DELIMITER //

create procedure sp_modificar_jugador(
    in p_id_jugador int,
    in p_nombre varchar(100)
)
begin
    set p_nombre = trim(p_nombre);
    -- validar que el jugador exista
    if not exists (select 1 from jugadores where id_jugador = p_id_jugador) then
        signal sqlstate '45000'
            set message_text = 'El jugador no existe';
    end if;
    -- validar nombre
    if p_nombre is null or char_length(p_nombre) < 2 then
        signal sqlstate '45000'
            set message_text = 'El nombre debe tener al menos 2 caracteres';
    end if;
    -- validar que el nombre no lo use OTRO jugador
    if exists (select 1 from jugadores
               where nombre = p_nombre
                 and id_jugador <> p_id_jugador) then
        signal sqlstate '45000'
            set message_text = 'Ya existe otro jugador con ese nombre';
    end if;
    update jugadores
    set nombre = p_nombre
    where id_jugador = p_id_jugador;
end //
DELIMITER ;

-- ================ // REGISTRAR PARTIDA \\ ======================
-- un jugador no puede tener dos partidas en curso: primero debe
-- reanudar o cancelar la que tiene pendiente
DELIMITER //

create procedure sp_registrar_partida(
    in  p_id_jugador int,
    in  p_modo varchar(30),
    out p_id_partida int
)
begin
    declare exit handler for sqlexception
    begin
        rollback;
        resignal;
    end;

    set p_modo = trim(p_modo);

    if not exists (select 1 from jugadores where id_jugador = p_id_jugador) then
        signal sqlstate '45000' set message_text = 'El jugador no existe';
    end if;

    if p_modo is null or p_modo = '' then
        signal sqlstate '45000' set message_text = 'Debe indicar el modo de juego';
    end if;

    if exists (select 1 from partidas
               where id_jugador = p_id_jugador and estado = 'EN_CURSO') then
        signal sqlstate '45000' set message_text = 'El jugador tiene una partida sin terminar';
    end if;

    start transaction;

    insert into partidas (id_jugador, modo)
    values (p_id_jugador, p_modo);

    set p_id_partida = last_insert_id();

    insert into historial_partidas (id_partida, accion, detalle)
    values (p_id_partida, 'INICIO', concat('Partida iniciada en modo ', p_modo));

    commit;
end //
DELIMITER ;

-- ================ // guardar respuesta \\ ==================
-- se llama despues de cada respuesta del usuario.
-- las preguntas deben guardarse en orden: 0, 1, 2...
DELIMITER //

create procedure sp_guardar_respuesta(
    in p_id_partida int,
    in p_nro_pregunta int,
    in p_nro_respuesta int
)
begin
    declare v_estado varchar(20) default null;
    declare v_siguiente int default 0;

    select estado into v_estado
    from partidas where id_partida = p_id_partida;

    if v_estado is null then
        signal sqlstate '45000' set message_text = 'La partida no existe';
    end if;

    if v_estado <> 'EN_CURSO' then
        signal sqlstate '45000' set message_text = 'La partida no esta en curso';
    end if;

    if p_nro_respuesta is null or p_nro_respuesta < 0 then
        signal sqlstate '45000' set message_text = 'Respuesta invalida';
    end if;

    -- la pregunta que toca es la cantidad de respuestas ya guardadas
    select count(*) into v_siguiente
    from respuestas_partida where id_partida = p_id_partida;

    if p_nro_pregunta is null or p_nro_pregunta <> v_siguiente then
        signal sqlstate '45000' set message_text = 'La pregunta no corresponde al orden de la partida';
    end if;

    insert into respuestas_partida (id_partida, nro_pregunta, nro_respuesta)
    values (p_id_partida, p_nro_pregunta, p_nro_respuesta);
end //
DELIMITER ;

-- ================ // guardar puntaje \\ ==================
-- guarda el puntaje TOTAL de una variable (si ya existe lo reemplaza)
DELIMITER //

create procedure sp_guardar_puntaje(
    in p_id_partida int,
    in p_tipo varchar(10),       -- 'RAZA' o 'CLASE'
    in p_variable varchar(30),   -- nombre del enum (ej: 'elfo', 'mago')
    in p_puntos int
)
begin
    declare v_estado varchar(20) default null;

    set p_tipo = upper(trim(p_tipo));
    set p_variable = trim(p_variable);

    select estado into v_estado
    from partidas where id_partida = p_id_partida;

    if v_estado is null then
        signal sqlstate '45000' set message_text = 'La partida no existe';
    end if;

    if v_estado <> 'EN_CURSO' then
        signal sqlstate '45000' set message_text = 'La partida no esta en curso';
    end if;

    if p_tipo is null or p_tipo not in ('RAZA', 'CLASE') then
        signal sqlstate '45000' set message_text = 'Tipo invalido: use RAZA o CLASE';
    end if;

    if p_variable is null or p_variable = '' then
        signal sqlstate '45000' set message_text = 'Debe indicar la raza o clase';
    end if;

    if p_puntos is null or p_puntos < 0 then
        signal sqlstate '45000' set message_text = 'Los puntos no pueden ser negativos';
    end if;

    insert into puntajes_partida (id_partida, tipo, variable, puntos)
    values (p_id_partida, p_tipo, p_variable, p_puntos)
    on duplicate key update puntos = p_puntos;
end //
DELIMITER ;

-- ======================== // finalizar partida \\ ========================
-- C# calcula el resultado con sus enums y lo envia aca.
-- la base valida que sea coherente con los puntajes guardados.
DELIMITER //

create procedure sp_finalizar_partida(
    in p_id_partida int,
    in p_raza_principal varchar(30),
    in p_raza_secundaria varchar(30),
    in p_combinacion varchar(50),
    in p_clase varchar(30)
)
begin
    declare v_estado varchar(20) default null;

    declare exit handler for sqlexception
    begin
        rollback;
        resignal;
    end;

    select estado into v_estado
    from partidas where id_partida = p_id_partida;

    if v_estado is null then
        signal sqlstate '45000' set message_text = 'La partida no existe';
    end if;

    if v_estado <> 'EN_CURSO' then
        signal sqlstate '45000' set message_text = 'La partida no esta en curso';
    end if;

    if not exists (select 1 from respuestas_partida where id_partida = p_id_partida) then
        signal sqlstate '45000' set message_text = 'La partida no tiene respuestas';
    end if;

    if p_raza_principal is null or p_raza_secundaria is null
       or p_combinacion is null or p_clase is null then
        signal sqlstate '45000' set message_text = 'Falta el resultado de la partida';
    end if;

    if p_raza_principal = p_raza_secundaria then
        signal sqlstate '45000' set message_text = 'La raza principal y la secundaria deben ser distintas';
    end if;

    -- las razas y la clase del resultado tienen que tener puntaje guardado
    if not exists (select 1 from puntajes_partida
                   where id_partida = p_id_partida and tipo = 'RAZA'
                     and variable = p_raza_principal) then
        signal sqlstate '45000' set message_text = 'Falta el puntaje de la raza principal';
    end if;

    if not exists (select 1 from puntajes_partida
                   where id_partida = p_id_partida and tipo = 'RAZA'
                     and variable = p_raza_secundaria) then
        signal sqlstate '45000' set message_text = 'Falta el puntaje de la raza secundaria';
    end if;

    if not exists (select 1 from puntajes_partida
                   where id_partida = p_id_partida and tipo = 'CLASE'
                     and variable = p_clase) then
        signal sqlstate '45000' set message_text = 'Falta el puntaje de la clase';
    end if;

    start transaction;

    update partidas
    set estado = 'FINALIZADA',
        fecha_fin = now(),
        raza_principal = p_raza_principal,
        raza_secundaria = p_raza_secundaria,
        combinacion = p_combinacion,
        clase = p_clase
    where id_partida = p_id_partida;

    insert into historial_partidas (id_partida, accion, detalle)
    values (p_id_partida, 'FINALIZADA',
            concat(p_combinacion, ' (', p_raza_principal, ' + ', p_raza_secundaria,
                   '), clase ', p_clase));

    commit;
end //
DELIMITER ;

-- ========================== // cancelar partida \\ ============================
DELIMITER //

create procedure sp_cancelar_partida(
    in p_id_partida int,
    in p_motivo varchar(200)
)
begin
    declare v_estado varchar(20) default null;

    declare exit handler for sqlexception
    begin
        rollback;
        resignal;
    end;

    select estado into v_estado
    from partidas where id_partida = p_id_partida;

    if v_estado is null then
        signal sqlstate '45000' set message_text = 'La partida no existe';
    end if;

    if v_estado <> 'EN_CURSO' then
        signal sqlstate '45000' set message_text = 'Solo se puede cancelar una partida en curso';
    end if;

    start transaction;

    update partidas
    set estado = 'CANCELADA',
        fecha_fin = now()
    where id_partida = p_id_partida;

    insert into historial_partidas (id_partida, accion, detalle)
    values (p_id_partida, 'CANCELADA', coalesce(p_motivo, 'Sin motivo'));

    commit;
end //
DELIMITER ;

-- ========================== // FUNCIONES \\ ============================
-- devuelve la partida en curso del jugador, o null si no tiene
DELIMITER //

create function fn_partida_pendiente(p_id_jugador int)
returns int
reads sql data
begin
    declare v_id_partida int default null;

    select id_partida into v_id_partida
    from partidas
    where id_jugador = p_id_jugador and estado = 'EN_CURSO'
    order by fecha_inicio desc
    limit 1;

    return v_id_partida;
end //
DELIMITER ;

-- ========================== // VISTAS \\ ============================
-- partidas sin terminar y cuantas preguntas lleva respondidas
create view vw_partidas_pendientes as
select p.id_partida,
       j.id_jugador,
       j.nombre as jugador,
       p.modo,
       p.fecha_inicio,
       (select count(*) from respuestas_partida r
        where r.id_partida = p.id_partida) as preguntas_respondidas
from partidas p
join jugadores j on j.id_jugador = p.id_jugador
where p.estado = 'EN_CURSO';

-- historial de todas las partidas
create view vw_historial as
select p.id_partida,
       j.nombre as jugador,
       p.modo,
       p.estado,
       p.fecha_inicio,
       p.fecha_fin,
       p.raza_principal,
       p.raza_secundaria,
       p.combinacion,
       p.clase
from partidas p
join jugadores j on j.id_jugador = p.id_jugador;

-- // TESTEOOSSSS ==================
-- partida completa
call sp_registrar_jugador('Aragorn', @jug);
call sp_registrar_partida(@jug, 'clasico', @par);

call sp_guardar_respuesta(@par, 0, 0);
call sp_guardar_respuesta(@par, 1, 1);
call sp_guardar_respuesta(@par, 2, 0);

call sp_guardar_puntaje(@par, 'RAZA', 'humano', 10);
call sp_guardar_puntaje(@par, 'RAZA', 'elfo', 5);
call sp_guardar_puntaje(@par, 'CLASE', 'guerrero', 8);

call sp_finalizar_partida(@par, 'humano', 'elfo', 'semielfo', 'guerrero');

select * from partidas where id_partida = @par;
select * from historial_partidas where id_partida = @par;

-- partida que queda pendiente para reanudar
call sp_registrar_jugador('Legolas', @jug2);
call sp_registrar_partida(@jug2, 'narrativo', @par2);
call sp_guardar_respuesta(@par2, 0, 3);

call sp_registrar_jugador('Legolas', @jug2); -- devuelve el mismo id
select fn_partida_pendiente(@jug2) as partida_pendiente;
select * from respuestas_partida where id_partida = fn_partida_pendiente(@jug2) order by nro_pregunta;
select * from vw_partidas_pendientes;
select * from vw_historial;


-- =========================================================================
-- razas, clases, modos, estados y combinaciones estaban como tablas.
-- se reemplazo por la version 2 de arriba.
-- =========================================================================
-- -- base de datos para proyecto final de algoritmos
-- 
-- create database fantasy_quest_bdd;
-- 
-- use fantasy_quest_bdd;
-- 
-- -- Procedimiento
-- -- sp_registrar_jugador, sp_modificar_jugador, sp_registrar_partida, sp_guardar_puntaje
-- -- sp_finalizar_partida, sp_anular_partida
-- -- CHECK (id_raza_principal <> id_raza_secundaria)
-- 
-- -- drop table raza_clases
-- 
-- create table razas(
-- 	id_raza int auto_increment primary key,
--     nombre varchar(100) not null unique
-- );
-- 
-- create table clases(
-- 	id_clase int auto_increment primary key,
--     nombre varchar(100) not null unique
-- );
-- 
-- create table modos_juego(
-- 	id_modo int auto_increment primary key,
--     nombre varchar(50) not null unique,
--     descripcion varchar(200)
-- );
-- -- drop table estados_oartida
-- create table estados_partida(
-- 	id_estado int auto_increment primary key,
--     nombre varchar(50) not null unique
-- );
-- 
-- create table combinaciones_raza(
-- 	id_combinacion int auto_increment primary key,
--     id_raza_principal int not null,
--     id_raza_secundaria int not null,
--     nombre varchar(100) not null,
--     descripcion text,
--     constraint uq_combinacion unique (id_raza_principal, id_raza_secundaria),
--     constraint chk_razas_distintas check (id_raza_principal <> id_raza_secundaria),
--     constraint fk_comb_principal foreign key (id_raza_principal) references razas(id_raza),
--     constraint fk_comb_secundaria foreign key (id_raza_secundaria) references razas(id_raza)
-- );
-- 
-- -- =============== TABLAS JUGADORES ===================
-- create table jugadores(
-- 	id_jugador int auto_increment primary key,
--     nombre varchar(100) not null unique,
--     fecha_alta datetime not null default current_timestamp,
--     constraint chk_nombre_jugador check(char_length(trim(nombre))>= 2)
-- );
-- 
-- -- ============== partidas =====================
-- create table partidas( -- main table
-- 	id_partida int auto_increment primary key,
--     id_jugador int not null,
--     id_modo int not null,
--     id_estado int not null default 1, -- 1 = en curso
--     fecha_inicio datetime not null default current_timestamp,
--     fecha_fin datetime null,
--     id_combinacion int null, -- se asigna al terminar
--     id_clase int null, -- se asigna al terminar
--     constraint chk_fechas check(fecha_fin is null or fecha_fin >= fecha_inicio),
--     constraint fk_partida_jugador foreign key (id_jugador) references jugadores(id_jugador),
--     constraint fk_partida_modo foreign key (id_modo) references modos_juego(id_modo),
--     constraint fk_partida_estado foreign key (id_estado) references estados_partida(id_estado),
--     constraint fk_partida_combinacion foreign key (id_combinacion) references combinaciones_raza(id_combinacion),
--     constraint fk_partida_clase foreign key (id_clase) references clases(id_clase)
-- );
-- 
-- create table puntajes_raza(
--     id_partida int not null,
--     id_raza int not null,
--     puntos int not null default 0,
--     primary key (id_partida, id_raza),
--     constraint chk_puntos_raza check (puntos >= 0),
--     constraint fk_pr_partida foreign key (id_partida) references partidas(id_partida),
--     constraint fk_pr_raza foreign key (id_raza) references razas(id_raza)
-- );
-- 
-- create table puntajes_clase(
--     id_partida int not null,
--     id_clase int not null,
--     puntos int not null default 0,
--     primary key (id_partida, id_clase),
--     constraint chk_puntos_clase check (puntos >= 0),
--     constraint fk_pc_partida foreign key (id_partida) references partidas(id_partida),
--     constraint fk_pc_clase foreign key (id_clase) references clases(id_clase)
-- );
-- 
-- -- ======================== historial ====================
-- 
-- create table historial_partidas(
-- 	id_historial int auto_increment primary key,
--     id_partida int not null,
--     accion varchar(50) not null, --
-- 	fecha datetime not null default current_timestamp,
--     detalle varchar(255),
--     constraint fk_his_partida foreign key (id_partida) references partidas(id_partida)
-- );
-- -- ============ INSERCIONES ================
-- insert into estados_partida (id_estado, nombre) values
--     (1, 'En curso'),
--     (2, 'Finalizada'),
--     (3, 'Cancelada');
-- -- modos de juego
-- insert into modos_juego (nombre, descripcion) values
--     ('Normal', 'Partida estandar'),
--     ('Rapido', 'Partida con menos preguntas');
--     -- razas
-- insert into razas (nombre) values
--     ('Humano'), ('Elfo'), ('Enano'), ('Orco');
-- -- clases
-- insert into clases (nombre) values
--     ('Guerrero'), ('Mago'), ('Arquero');
-- -- combinaciones (al menos la que usa la prueba: raza 1 + raza 2)
-- insert into combinaciones_raza (id_raza_principal, id_raza_secundaria, nombre, descripcion) values
--     (1, 2, 'Semielfo', 'Humano con herencia elfica'),
--     (2, 1, 'Elfo de las llanuras', 'Elfo criado entre humanos');
-- 
-- -- ===================== Procediimentos ==============================
-- -- sp_registrar_jugador, sp_modificar_jugador, sp_registrar_partida, sp_guardar_puntaje
-- -- sp_finalizar_partida, sp_anular_partida
-- -- CHECK (id_raza_principal <> id_raza_secundaria)
-- 
-- -- =================== // REGISTRAR JUGADOR \\ ========================
-- DELIMITER //
-- create procedure sp_registrar_jugador(
--     in  p_nombre varchar(100),
--     out p_id_jugador int
-- )
-- begin
--     -- limpiar espacios
--     set p_nombre = trim(p_nombre);
-- 
--     -- validar nombre (mismo criterio que el CHECK de la tabla)
--     if p_nombre is null or char_length(p_nombre) < 2 then
--         signal sqlstate '45000'
--             set message_text = 'El nombre debe tener al menos 2 caracteres';
--     end if;
--     -- validar que no exista (la columna es UNIQUE)
--     if exists (select 1 from jugadores where nombre = p_nombre) then
--         signal sqlstate '45000'
--             set message_text = 'Ya existe un jugador con ese nombre';
--     end if;
--     insert into jugadores (nombre)
--     values (p_nombre);
--     set p_id_jugador = last_insert_id();
-- end //
-- DELIMITER ;
-- 
-- -- ===================== // MODIFICAR JUGADOR \\ =====================
-- DELIMITER //
-- 
-- create procedure sp_modificar_jugador(
--     in p_id_jugador int,
--     in p_nombre varchar(100)
-- )
-- begin
--     set p_nombre = trim(p_nombre);
--     -- validar que el jugador exista
--     if not exists (select 1 from jugadores where id_jugador = p_id_jugador) then
--         signal sqlstate '45000'
--             set message_text = 'El jugador no existe';
--     end if;
--     -- validar nombre
--     if p_nombre is null or char_length(p_nombre) < 2 then
--         signal sqlstate '45000'
--             set message_text = 'El nombre debe tener al menos 2 caracteres';
--     end if;
--     -- validar que el nombre no lo use OTRO jugador
--     if exists (select 1 from jugadores
--                where nombre = p_nombre
--                  and id_jugador <> p_id_jugador) then
--         signal sqlstate '45000'
--             set message_text = 'Ya existe otro jugador con ese nombre';
--     end if;
--     update jugadores
--     set nombre = p_nombre
--     where id_jugador = p_id_jugador;
-- end //
-- DELIMITER ;
-- -- ================ // REGISTRAR PARTIDA \\ ======================
-- DELIMITER //
-- 
-- create procedure sp_registrar_partida(
--     in  p_id_jugador int,
--     in  p_id_modo int,
--     out p_id_partida int
-- )
-- begin
--     declare exit handler for sqlexception
--     begin
--         rollback;
--         resignal;
--     end;
-- 
--     if not exists (select 1 from jugadores where id_jugador = p_id_jugador) then
--         signal sqlstate '45000' set message_text = 'El jugador no existe';
--     end if;
-- 
--     if not exists (select 1 from modos_juego where id_modo = p_id_modo) then
--         signal sqlstate '45000' set message_text = 'El modo de juego no existe';
--     end if;
-- 
--     start transaction;
-- 
--     insert into partidas (id_jugador, id_modo, id_estado)
--     values (p_id_jugador, p_id_modo, 1);
-- 
--     set p_id_partida = last_insert_id();
-- 
--     insert into historial_partidas (id_partida, accion, detalle)
--     values (p_id_partida, 'INICIO', concat('Partida iniciada en modo ', p_id_modo));
-- 
--     commit;
-- end //
-- DELIMITER ;
-- -- ================ // guardar puntaje \\ ==================
-- DELIMITER //
-- 
-- create procedure sp_guardar_puntaje(
--     in p_id_partida int,
--     in p_tipo varchar(10),   -- 'RAZA' o 'CLASE'
--     in p_id int,             -- id_raza o id_clase, según el tipo
--     in p_puntos int
-- )
-- begin
--     declare v_estado int default null;
-- 
--     select id_estado into v_estado
--     from partidas where id_partida = p_id_partida;
-- 
--     if v_estado is null then
--         signal sqlstate '45000' set message_text = 'La partida no existe';
--     end if;
-- 
--     if v_estado <> 1 then
--         signal sqlstate '45000' set message_text = 'La partida no esta en curso';
--     end if;
-- 
--     if p_puntos is null or p_puntos < 0 then
--         signal sqlstate '45000' set message_text = 'Los puntos no pueden ser negativos';
--     end if;
-- 
--     if upper(p_tipo) = 'RAZA' then
--         if not exists (select 1 from razas where id_raza = p_id) then
--             signal sqlstate '45000' set message_text = 'La raza no existe';
--         end if;
-- 
--         insert into puntajes_raza (id_partida, id_raza, puntos)
--         values (p_id_partida, p_id, p_puntos)
--         on duplicate key update puntos = puntos + p_puntos;
-- 
--     elseif upper(p_tipo) = 'CLASE' then
--         if not exists (select 1 from clases where id_clase = p_id) then
--             signal sqlstate '45000' set message_text = 'La clase no existe';
--         end if;
-- 
--         insert into puntajes_clase (id_partida, id_clase, puntos)
--         values (p_id_partida, p_id, p_puntos)
--         on duplicate key update puntos = puntos + p_puntos;
-- 
--     else
--         signal sqlstate '45000' set message_text = 'Tipo invalido: use RAZA o CLASE';
--     end if;
-- end //
-- DELIMITER ;
-- -- ======================== // finalizar partida \\ ========================
-- DELIMITER //
-- 
-- create procedure sp_finalizar_partida(
--     in p_id_partida int
-- )
-- begin
--     declare v_estado int default null;
--     declare v_raza1 int default null;
--     declare v_raza2 int default null;
--     declare v_clase int default null;
--     declare v_combinacion int default null;
-- 
--     declare exit handler for sqlexception
--     begin
--         rollback;
--         resignal;
--     end;
-- 
--     select id_estado into v_estado
--     from partidas where id_partida = p_id_partida;
-- 
--     if v_estado is null then
--         signal sqlstate '45000' set message_text = 'La partida no existe';
--     end if;
-- 
--     if v_estado <> 1 then
--         signal sqlstate '45000' set message_text = 'La partida no esta en curso';
--     end if;
-- 
--     -- raza con mas puntos (principal)
--     select id_raza into v_raza1
--     from puntajes_raza
--     where id_partida = p_id_partida and puntos > 0
--     order by puntos desc, id_raza
--     limit 1;
-- 
--     -- segunda raza con mas puntos (secundaria)
--     select id_raza into v_raza2
--     from puntajes_raza
--     where id_partida = p_id_partida and puntos > 0
--     order by puntos desc, id_raza
--     limit 1, 1;
-- 
--     -- clase con mas puntos
--     select id_clase into v_clase
--     from puntajes_clase
--     where id_partida = p_id_partida and puntos > 0
--     order by puntos desc, id_clase
--     limit 1;
-- 
--     if v_raza1 is null or v_raza2 is null then
--         signal sqlstate '45000' set message_text = 'Se necesitan puntos en al menos dos razas';
--     end if;
-- 
--     if v_clase is null then
--         signal sqlstate '45000' set message_text = 'Se necesitan puntos en al menos una clase';
--     end if;
-- 
--     select id_combinacion into v_combinacion
--     from combinaciones_raza
--     where id_raza_principal = v_raza1
--       and id_raza_secundaria = v_raza2;
-- 
--     if v_combinacion is null then
--         signal sqlstate '45000' set message_text = 'No existe una combinacion para esas razas';
--     end if;
-- 
--     start transaction;
-- 
--     update partidas
--     set id_estado = 2,
--         fecha_fin = now(),
--         id_combinacion = v_combinacion,
--         id_clase = v_clase
--     where id_partida = p_id_partida;
-- 
--     insert into historial_partidas (id_partida, accion, detalle)
--     values (p_id_partida, 'FINALIZADA',
--             concat('Combinacion ', v_combinacion, ', clase ', v_clase));
-- 
--     commit;
-- end //
-- DELIMITER ;
-- -- ========================== // cancelar partida \\ ============================
-- DELIMITER //
-- 
-- create procedure sp_cancelar_partida(
--     in p_id_partida int,
--     in p_motivo varchar(200)
-- )
-- begin
--     declare v_estado int default null;
-- 
--     declare exit handler for sqlexception
--     begin
--         rollback;
--         resignal;
--     end;
-- 
--     select id_estado into v_estado
--     from partidas where id_partida = p_id_partida;
-- 
--     if v_estado is null then
--         signal sqlstate '45000' set message_text = 'La partida no existe';
--     end if;
-- 
--     if v_estado <> 1 then
--         signal sqlstate '45000' set message_text = 'Solo se puede cancelar una partida en curso';
--     end if;
-- 
--     start transaction;
-- 
--     update partidas
--     set id_estado = 3,
--         fecha_fin = now()
--     where id_partida = p_id_partida;
-- 
--     insert into historial_partidas (id_partida, accion, detalle)
--     values (p_id_partida, 'CANCELADA', coalesce(p_motivo, 'Sin motivo'));
-- 
--     commit;
-- end //
-- DELIMITER ;
-- 
-- 
-- -- // TESTEOOSSSS ================== 
-- call sp_registrar_jugador('Aragorn', @jug);
-- call sp_registrar_partida(@jug, 1, @par);
-- 
-- call sp_guardar_puntaje(@par, 'RAZA', 1, 10);
-- call sp_guardar_puntaje(@par, 'RAZA', 2, 5);
-- call sp_guardar_puntaje(@par, 'CLASE', 1, 8);
-- 
-- call sp_finalizar_partida(@par);
-- 
-- select * from partidas where id_partida = @par;
-- select * from historial_partidas where id_partida = @par;
-- 
-- 
-- 
-- select * from modos_juego;
