using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Fantasy_quest.Dominio;
using Fantasy_quest.Gestoras;

namespace Fantasy_quest.Formularios
{
    // menu del jugador registrado / logueado: Iniciar o Continuar
    internal partial class FrmMenuJugador : Form
    {
        private readonly Jugador jugador;
        private readonly sql_repository repositorio = new sql_repository();
        private int? idPartidaPendiente;

        public FrmMenuJugador(Jugador jugador)
        {
            InitializeComponent();
            this.jugador = jugador;
            lblSaludo.Text = "Hola, " + jugador.Nombre;
        }

        private void FrmMenuJugador_Load(object sender, EventArgs e)
        {
            Refrescar();
        }

        private void Refrescar()
        {
            try
            {
                idPartidaPendiente = repositorio.ObtenerPartidaPendiente(jugador.Id);
            }
            catch (Exception ex)
            {
                idPartidaPendiente = null;
                Mensajes.MostrarError(ex);
            }

            bool hayPendiente = idPartidaPendiente != null;
            btnJugar.Text = hayPendiente ? "Continuar" : "Iniciar";
            lblEstado.Text = hayPendiente ? "Tenés una partida sin terminar." : "";
            btnAbandonar.Visible = hayPendiente;
        }

        private void btnJugar_Click(object sender, EventArgs e)
        {
            GestorPartida gestor;
            try
            {
                gestor = idPartidaPendiente == null
                    ? GestorPartida.Nueva(jugador, repositorio)
                    : GestorPartida.Reanudar(jugador, idPartidaPendiente.Value, repositorio);
            }
            catch (Exception ex)
            {
                Mensajes.MostrarError(ex);
                return;
            }

            Navegador.Jugar(this, gestor);
            Refrescar();
        }

        // sin esto no se podria empezar una nueva: la base no deja tener dos EN_CURSO
        private void btnAbandonar_Click(object sender, EventArgs e)
        {
            if (idPartidaPendiente == null)
            {
                return;
            }

            DialogResult respuesta = MessageBox.Show("¿Abandonar la partida pendiente? Se pierde el progreso.",
                                                     "Abandonar partida", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (respuesta != DialogResult.Yes)
            {
                return;
            }

            try
            {
                repositorio.CancelarPartida(idPartidaPendiente.Value, "Abandonada por el jugador");
            }
            catch (Exception ex)
            {
                Mensajes.MostrarError(ex);
            }
            Refrescar();
        }

        private void btnAtras_Click(object sender, EventArgs e)
        {
            Close();
        }
    }
}
