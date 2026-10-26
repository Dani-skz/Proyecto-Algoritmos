using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Fantasy_quest.Dominio;
using Fantasy_quest.Gestoras;

namespace Fantasy_quest.Formularios
{
    // Solo arranca en Debug (ver Program.cs): acceso directo a cada ventana
    // para probarlas sueltas. La version Release arranca en FrmPrincipal.
    internal partial class FrmDesarrollo : Form
    {
        private const string NombreJugadorPrueba = "Aragorn";

        private readonly sql_repository repositorio = new sql_repository();

        public FrmDesarrollo()
        {
            InitializeComponent();
        }

        private void FrmDesarrollo_Load(object sender, EventArgs e)
        {
            lblConexion.Text = new DAO_gestora().ProbarConexion()
                ? "Base de datos: conectada"
                : "Base de datos: SIN CONEXIÓN (revisar la cadena en DAO_gestora)";
        }

        private void btnFlujoCompleto_Click(object sender, EventArgs e)
        {
            using FrmPrincipal frmPrincipal = new FrmPrincipal();
            Navegador.Abrir(this, frmPrincipal);
        }

        private void btnRegistro_Click(object sender, EventArgs e)
        {
            using FrmIngreso frmIngreso = new FrmIngreso(ModoIngreso.Registro);
            Navegador.Abrir(this, frmIngreso);
        }

        private void btnLogin_Click(object sender, EventArgs e)
        {
            using FrmIngreso frmIngreso = new FrmIngreso(ModoIngreso.Login);
            Navegador.Abrir(this, frmIngreso);
        }

        // si el jugador de prueba no existe lo crea (sp_registrar_jugador reutiliza el existente)
        private void btnMenuJugador_Click(object sender, EventArgs e)
        {
            Jugador jugador;
            try
            {
                jugador = new Jugador(repositorio.RegistrarJugador(NombreJugadorPrueba), NombreJugadorPrueba);
            }
            catch (Exception ex)
            {
                Mensajes.MostrarError(ex);
                return;
            }

            using FrmMenuJugador frmMenu = new FrmMenuJugador(jugador);
            Navegador.Abrir(this, frmMenu);
        }

        private void btnPartidaAnonima_Click(object sender, EventArgs e)
        {
            Navegador.Jugar(this, GestorPartida.NuevaAnonima(repositorio));
        }

        // no usa la base: aplica la respuesta 3 de la primera pregunta
        private void btnResultado_Click(object sender, EventArgs e)
        {
            Perfil perfil = new Perfil();
            perfil.Aplicar(BancoPreguntas.Crear()[0].Respuestas[2]);

            using FrmResultado frmResultado = new FrmResultado(perfil.CalcularResultado("Prueba"));
            Navegador.Abrir(this, frmResultado);
        }

        private void btnScoreboard_Click(object sender, EventArgs e)
        {
            using FrmScoreboard frmScoreboard = new FrmScoreboard();
            Navegador.Abrir(this, frmScoreboard);
        }
    }
}
