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

