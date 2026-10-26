using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Fantasy_quest.Dominio;
using Fantasy_quest.Gestoras;

namespace Fantasy_quest.Formularios
{
    internal enum ModoIngreso
    {
        Registro,
        Login
    }

    // registro y login comparten ventana: solo se pide el nombre.
    // Si sale bien se cierra con DialogResult.OK y deja el Jugador cargado
    internal partial class FrmIngreso : Form
    {
        private readonly ModoIngreso modo;
        private readonly sql_repository repositorio = new sql_repository();

        public Jugador? Jugador { get; private set; }

        public FrmIngreso(ModoIngreso modo)
        {
            InitializeComponent();
            this.modo = modo;

            bool esRegistro = modo == ModoIngreso.Registro;
            lblTitulo.Text = esRegistro ? "Registrarse" : "Iniciar sesión";
            btnAceptar.Text = esRegistro ? "Registrarme" : "Entrar";
        }

        private void btnAceptar_Click(object sender, EventArgs e)
        {
            try
            {
                if (modo == ModoIngreso.Registro)
                {
                    Jugador = repositorio.RegistrarJugadorNuevo(txtNombre.Text);
                }
                else
                {
                    Jugador = repositorio.IniciarSesion(txtNombre.Text);
                    if (Jugador == null)
                    {
                        MessageBox.Show("No existe un jugador con ese nombre. Registrate primero.", "Iniciar sesión",
                                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                        return;
                    }
                }
                DialogResult = DialogResult.OK;
            }
            catch (Exception ex)
            {
                Mensajes.MostrarError(ex);
            }
        }

        private void btnAtras_Click(object sender, EventArgs e)
        {
            Close();
        }
    }
}
