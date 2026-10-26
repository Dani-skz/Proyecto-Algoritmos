using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Fantasy_quest.enums;

namespace Fantasy_quest.Gestoras
{
    // Metodos avanzados: usa los procedimientos, la funcion y las vistas de
    // fantasy_quest_bdd. Los enums se guardan como texto (ToString / Enum.Parse).
    //
    // Los errores de validacion de la base (signal sqlstate '45000') llegan como
    // MySqlException y su Message es el texto del procedimiento, ej:
    // "El jugador tiene una partida sin terminar".
    internal class sql_repository
    {
        private readonly DAO_gestora dao;

        public sql_repository()
            : this(new DAO_gestora())
        {
        }

        public sql_repository(DAO_gestora dao)
        {
            this.dao = dao;
        }

        // ======================= JUGADORES =======================

        // si el jugador ya existe devuelve su id (para reanudar su partida)
        public int RegistrarJugador(string nombre)
        {
            Dictionary<string, object?> parametros = new Dictionary<string, object?>
            {
                { "p_nombre", nombre }
            };
            return dao.EjecutarProcedimientoConSalida("sp_registrar_jugador", parametros, "p_id_jugador");
        }

        public void ModificarJugador(int idJugador, string nombre)
        {
            dao.EjecutarProcedimiento("sp_modificar_jugador", new Dictionary<string, object?>
            {
                { "p_id_jugador", idJugador },
                { "p_nombre", nombre }
            });
        }

        // devuelve null si no existe
        public int? BuscarJugador(string nombre)
        {
            object? id = dao.EjecutarEscalar(
                "select id_jugador from jugadores where nombre = @nombre",
                new Dictionary<string, object?> { { "@nombre", nombre.Trim() } });
            return id == null ? null : Convert.ToInt32(id);
        }

        // ======================= PARTIDAS =======================

        // falla si el jugador ya tiene una partida EN_CURSO
        public int RegistrarPartida(int idJugador, string modo)
        {
            Dictionary<string, object?> parametros = new Dictionary<string, object?>
            {
                { "p_id_jugador", idJugador },
                { "p_modo", modo }
            };
            return dao.EjecutarProcedimientoConSalida("sp_registrar_partida", parametros, "p_id_partida");
        }

        // nroPregunta tiene que ser la siguiente en orden: 0, 1, 2...
        public void GuardarRespuesta(int idPartida, int nroPregunta, int nroRespuesta)
        {
            dao.EjecutarProcedimiento("sp_guardar_respuesta", new Dictionary<string, object?>
            {
                { "p_id_partida", idPartida },
                { "p_nro_pregunta", nroPregunta },
                { "p_nro_respuesta", nroRespuesta }
            });
        }

        // guarda el puntaje TOTAL (si ya existia lo reemplaza)
        public void GuardarPuntaje(int idPartida, razas raza, int puntos)
        {
            GuardarPuntaje(idPartida, "RAZA", raza.ToString(), puntos);
        }

        public void GuardarPuntaje(int idPartida, clase clase, int puntos)
        {
            GuardarPuntaje(idPartida, "CLASE", clase.ToString(), puntos);
        }

        public void GuardarPuntajes(int idPartida, Dictionary<razas, int> puntosRaza, Dictionary<clase, int> puntosClase)
        {
            foreach (KeyValuePair<razas, int> par in puntosRaza)
            {
                GuardarPuntaje(idPartida, par.Key, par.Value);
            }
            foreach (KeyValuePair<clase, int> par in puntosClase)
            {
                GuardarPuntaje(idPartida, par.Key, par.Value);
            }
        }

        // las razas y la clase tienen que tener puntaje guardado antes de finalizar
        public void FinalizarPartida(int idPartida, razas principal, razas secundaria,
                                     CombinacionRaza combinacion, clase clase)
        {
            dao.EjecutarProcedimiento("sp_finalizar_partida", new Dictionary<string, object?>
            {
                { "p_id_partida", idPartida },
                { "p_raza_principal", principal.ToString() },
                { "p_raza_secundaria", secundaria.ToString() },
                { "p_combinacion", combinacion.ToString() },
                { "p_clase", clase.ToString() }
            });
        }

        public void CancelarPartida(int idPartida, string? motivo = null)
        {
            dao.EjecutarProcedimiento("sp_cancelar_partida", new Dictionary<string, object?>
            {
                { "p_id_partida", idPartida },
                { "p_motivo", motivo }
            });
        }

        // ======================= REANUDAR PARTIDA =======================

        // id de la partida EN_CURSO del jugador, o null si no tiene
        public int? ObtenerPartidaPendiente(int idJugador)
        {
            object? id = dao.EjecutarEscalar(
                "select fn_partida_pendiente(@id_jugador)",
                new Dictionary<string, object?> { { "@id_jugador", idJugador } });
            return id == null ? null : Convert.ToInt32(id);
        }

        // respuestas en orden: la posicion de la lista es nro_pregunta y el valor nro_respuesta.
        // C# las vuelve a aplicar para reconstruir puntos y saber la pregunta actual (= Count)
        public List<int> ObtenerRespuestas(int idPartida)
        {
            DataTable tabla = dao.EjecutarConsulta(
                "select nro_respuesta from respuestas_partida " +
                "where id_partida = @id_partida order by nro_pregunta",
                new Dictionary<string, object?> { { "@id_partida", idPartida } });

            List<int> respuestas = new List<int>();
            foreach (DataRow fila in tabla.Rows)
            {
                respuestas.Add(Convert.ToInt32(fila["nro_respuesta"]));
            }
            return respuestas;
        }

        public string ObtenerModoPartida(int idPartida)
        {
            object? modo = dao.EjecutarEscalar(
                "select modo from partidas where id_partida = @id_partida",
                new Dictionary<string, object?> { { "@id_partida", idPartida } });
            return modo?.ToString() ?? throw new InvalidOperationException("La partida no existe");
        }

        // ======================= PUNTAJES =======================

        public Dictionary<razas, int> ObtenerPuntajesRaza(int idPartida)
        {
            Dictionary<razas, int> puntajes = new Dictionary<razas, int>();
            foreach (DataRow fila in ConsultarPuntajes(idPartida, "RAZA").Rows)
            {
                razas raza = Enum.Parse<razas>(fila["variable"].ToString()!);
                puntajes[raza] = Convert.ToInt32(fila["puntos"]);
            }
            return puntajes;
        }

        public Dictionary<clase, int> ObtenerPuntajesClase(int idPartida)
        {
            Dictionary<clase, int> puntajes = new Dictionary<clase, int>();
            foreach (DataRow fila in ConsultarPuntajes(idPartida, "CLASE").Rows)
            {
                clase clase = Enum.Parse<clase>(fila["variable"].ToString()!);
                puntajes[clase] = Convert.ToInt32(fila["puntos"]);
            }
            return puntajes;
        }

        // ======================= VISTAS (para grillas) =======================

        // id_partida, id_jugador, jugador, modo, fecha_inicio, preguntas_respondidas
        public DataTable ObtenerPartidasPendientes()
        {
            return dao.EjecutarConsulta("select * from vw_partidas_pendientes order by fecha_inicio desc");
        }

        // id_partida, jugador, modo, estado, fechas y resultado
        public DataTable ObtenerHistorial()
        {
            return dao.EjecutarConsulta("select * from vw_historial order by fecha_inicio desc");
        }

        public DataTable ObtenerHistorialJugador(string nombreJugador)
        {
            return dao.EjecutarConsulta(
                "select * from vw_historial where jugador = @jugador order by fecha_inicio desc",
                new Dictionary<string, object?> { { "@jugador", nombreJugador.Trim() } });
        }

        // ======================= AUXILIARES =======================

        private void GuardarPuntaje(int idPartida, string tipo, string variable, int puntos)
        {
            dao.EjecutarProcedimiento("sp_guardar_puntaje", new Dictionary<string, object?>
            {
                { "p_id_partida", idPartida },
                { "p_tipo", tipo },
                { "p_variable", variable },
                { "p_puntos", puntos }
            });
        }

        private DataTable ConsultarPuntajes(int idPartida, string tipo)
        {
            return dao.EjecutarConsulta(
                "select variable, puntos from puntajes_partida " +
                "where id_partida = @id_partida and tipo = @tipo order by puntos desc",
                new Dictionary<string, object?>
                {
                    { "@id_partida", idPartida },
                    { "@tipo", tipo }
                });
        }
    }
}
