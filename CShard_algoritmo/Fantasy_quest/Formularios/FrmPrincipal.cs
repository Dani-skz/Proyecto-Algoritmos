using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Fantasy_quest.Gestoras;

namespace Fantasy_quest.Formularios
{
    // menu principal (primera capa del juego compilado)
    internal partial class FrmPrincipal : Form
    {
        private readonly sql_repository repositorio = new sql_repository();

        public FrmPrincipal()
        {
            InitializeComponent();
        }

        private void FrmPrincipal_Shown(object sender, EventArgs e)
        {
            if (!new DAO_gestora().ProbarConexion())
            {
                MessageBox.Show("No hay conexión con la base de datos.\n" +
                                "Podés jugar anónimo, pero no se va a poder guardar la partida.",
                                "Fantasy Quest", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void btnRegistrarse_Click(object sender, EventArgs e)
        {
            Ingresar(ModoIngreso.Registro);
        }

        private void btnLogin_Click(object sender, EventArgs e)
        {
            Ingresar(ModoIngreso.Login);
        }

        private void btnAnonimo_Click(object sender, EventArgs e)
        {
            Navegador.Jugar(this, GestorPartida.NuevaAnonima(repositorio));
        }

        private void btnScoreboard_Click(object sender, EventArgs e)
        {
            using FrmScoreboard frmScoreboard = new FrmScoreboard();
            Navegador.Abrir(this, frmScoreboard);
        }

        private void btnSalir_Click(object sender, EventArgs e)
        {
            Close();
        }

        // registro / login y, si sale bien, el menu del jugador.
        // Atras en el menu del jugador vuelve aca (cierra la sesion)
        private void Ingresar(ModoIngreso modo)
        {
            using FrmIngreso frmIngreso = new FrmIngreso(modo);
            if (Navegador.Abrir(this, frmIngreso) != DialogResult.OK || frmIngreso.Jugador == null)
            {
                return;
            }

            using FrmMenuJugador frmMenu = new FrmMenuJugador(frmIngreso.Jugador);
            Navegador.Abrir(this, frmMenu);
        }
    }
}
