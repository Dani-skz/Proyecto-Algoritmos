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

