-- base de datos para proyecto final de algoritmos

create database fantasy_quest_bdd;

use fantasy_quest_bdd;

-- Procedimiento
-- sp_registrar_jugador, sp_modificar_jugador, sp_registrar_partida, sp_guardar_puntaje
-- sp_finalizar_partida, sp_anular_partida
-- CHECK (id_raza_principal <> id_raza_secundaria)

-- drop table raza_clases

create table razas(
	id_raza int auto_increment primary key,
    nombre varchar(100) not null unique
);

create table clases(
	id_clase int auto_increment primary key,
    nombre varchar(100) not null unique
);

create table modos_juego(
	id_modo int auto_increment primary key,
    nombre varchar(50) not null unique,
    descripcion varchar(200)
);
-- drop table estados_oartida
create table estados_partida(
	id_estado int auto_increment primary key,
    nombre varchar(50) not null unique
);

create table combinaciones_raza(
	id_combinacion int auto_increment primary key,
    id_raza_principal int not null,
    id_raza_secundaria int not null,
    nombre varchar(100) not null,
    descripcion text,
    constraint uq_combinacion unique (id_raza_principal, id_raza_secundaria),
    constraint chk_razas_distintas check (id_raza_principal <> id_raza_secundaria),
    constraint fk_comb_principal foreign key (id_raza_principal) references razas(id_raza),
    constraint fk_comb_secundaria foreign key (id_raza_secundaria) references razas(id_raza)
);

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
    id_modo int not null,
    id_estado int not null default 1, -- 1 = en curso
    fecha_inicio datetime not null default current_timestamp,
    fecha_fin datetime null,
    id_combinacion int null, -- se asigna al terminar
    id_clase int null, -- se asigna al terminar
    constraint chk_fechas check(fecha_fin is null or fecha_fin >= fecha_inicio),
    constraint fk_partida_jugador foreign key (id_jugador) references jugadores(id_jugador),
    constraint fk_partida_modo foreign key (id_modo) references modos_juego(id_modo),
    constraint fk_partida_estado foreign key (id_estado) references estados_partida(id_estado),
    constraint fk_partida_combinacion foreign key (id_combinacion) references combinaciones_raza(id_combinacion),
    constraint fk_partida_clase foreign key (id_clase) references clases(id_clase)
);

create table puntajes_raza(
    id_partida int not null,
    id_raza int not null,
    puntos int not null default 0,
    primary key (id_partida, id_raza),
    constraint chk_puntos_raza check (puntos >= 0),
    constraint fk_pr_partida foreign key (id_partida) references partidas(id_partida),
    constraint fk_pr_raza foreign key (id_raza) references razas(id_raza)
);

create table puntajes_clase(
    id_partida int not null,
    id_clase int not null,
    puntos int not null default 0,
    primary key (id_partida, id_clase),
    constraint chk_puntos_clase check (puntos >= 0),
    constraint fk_pc_partida foreign key (id_partida) references partidas(id_partida),
    constraint fk_pc_clase foreign key (id_clase) references clases(id_clase)
);

-- ======================== historial ====================

create table historial_partidas(
	id_historial int auto_increment primary key,
    id_partida int not null,
    accion varchar(50) not null, --
	fecha datetime not null default current_timestamp,
    detalle varchar(255),
    constraint fk_his_partida foreign key (id_partida) references partidas(id_partida)
);

