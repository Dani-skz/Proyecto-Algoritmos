using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Fantasy_quest.Dominio;
using Fantasy_quest.enums;

namespace Fantasy_quest.Gestoras
{
    // Coordina una partida: preguntas, puntos y guardado en la base.
    // Los formularios solo muestran y llaman a estos metodos.
    //
    // Registrada: cada respuesta se guarda al momento (se puede continuar despues).
    // Anonima: todo queda en memoria; si al final se registra, GuardarEn() la vuelca a la base.
    internal class GestorPartida
    {
        // por ahora todas son narrativas; cuando haya selector de modo se pasa por parametro
        private const modo_juego Modo = modo_juego.narrativo;

        private readonly sql_repository repositorio;
        private readonly List<int> respuestas = new List<int>(); // respuesta elegida en cada pregunta

        public Jugador? Jugador { get; private set; }
        public int? IdPartida { get; private set; }
        public List<Pregunta> Preguntas { get; } = BancoPreguntas.Crear();
        public Perfil Perfil { get; } = new Perfil();
        public ResultadoPartida? Resultado { get; private set; } // null hasta que termina

        public bool EsAnonima => Jugador == null;
        public int IndiceActual => respuestas.Count;
        public bool Terminada => IndiceActual >= Preguntas.Count;
        public Pregunta PreguntaActual => Preguntas[IndiceActual];

        private GestorPartida(Jugador? jugador, int? idPartida, sql_repository repositorio)
        {
            Jugador = jugador;
            IdPartida = idPartida;
            this.repositorio = repositorio;
        }

        // ======================= CREAR =======================

        public static GestorPartida NuevaAnonima(sql_repository repositorio)
        {
            return new GestorPartida(null, null, repositorio);
        }

        public static GestorPartida Nueva(Jugador jugador, sql_repository repositorio)
        {
            int idPartida = repositorio.RegistrarPartida(jugador.Id, Modo);
            return new GestorPartida(jugador, idPartida, repositorio);
        }

        // vuelve a aplicar las respuestas guardadas para reconstruir los puntos
        public static GestorPartida Reanudar(Jugador jugador, int idPartida, sql_repository repositorio)
        {
            GestorPartida gestor = new GestorPartida(jugador, idPartida, repositorio);

            foreach (int nroRespuesta in repositorio.ObtenerRespuestas(idPartida))
            {
                // pasa si se cambiaron las preguntas del juego despues de guardar la partida
                if (gestor.Terminada || nroRespuesta >= gestor.PreguntaActual.Respuestas.Count)
                {
                    throw new InvalidOperationException(
                        "La partida pendiente no coincide con las preguntas actuales. Abandonala y empezá una nueva.");
                }
                gestor.Aplicar(nroRespuesta);
            }
            return gestor;
        }

        // ======================= JUGAR =======================

        public void Responder(int nroRespuesta)
        {
            // primero la base: si falla no se aplica, asi memoria y base no se desfasan
            if (IdPartida != null)
            {
                repositorio.GuardarRespuesta(IdPartida.Value, IndiceActual, nroRespuesta);
            }
            Aplicar(nroRespuesta);
        }

        public ResultadoPartida Finalizar()
        {
            ResultadoPartida resultado = Perfil.CalcularResultado(Jugador?.Nombre);

            if (IdPartida != null)
            {
                // se guardan todas las variables (tambien las de 0 puntos) porque
                // sp_finalizar_partida exige puntaje para la raza secundaria
                repositorio.GuardarPuntajes(IdPartida.Value, Perfil.PuntosRaza, Perfil.PuntosClase);
                repositorio.FinalizarPartida(IdPartida.Value, resultado.RazaPrincipal, resultado.RazaSecundaria,
                                             resultado.Combinacion, resultado.Clase);
            }

            Resultado = resultado;
            return resultado;
        }

        // partida anonima terminada -> se guarda completa a nombre del jugador recien registrado
        public void GuardarEn(Jugador jugador)
        {
            if (!EsAnonima || !Terminada)
            {
                throw new InvalidOperationException("Solo se puede guardar una partida anónima terminada");
            }

            int idPartida = repositorio.RegistrarPartida(jugador.Id, Modo);
            for (int i = 0; i < respuestas.Count; i++)
            {
                repositorio.GuardarRespuesta(idPartida, i, respuestas[i]);
            }

            Jugador = jugador;
            IdPartida = idPartida;
            Finalizar();
        }

        private void Aplicar(int nroRespuesta)
        {
            Perfil.Aplicar(PreguntaActual.Respuestas[nroRespuesta]);
            respuestas.Add(nroRespuesta);
        }
    }
}
