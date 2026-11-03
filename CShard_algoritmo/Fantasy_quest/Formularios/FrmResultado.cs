using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Fantasy_quest.Dominio;
using Fantasy_quest.ui;

namespace Fantasy_quest.Formularios
{
    // sirve para el final de la partida y para "Ver resultado" del scoreboard.
    // Solo muestra el resultado; los puntos de cada raza y clase estan en FrmPuntajes.
    // Al final de la partida tambien esta "Volver a jugar": cierra con DialogResult.Retry
    // y Navegador.Jugar vuelve hasta el menu principal
    internal partial class FrmResultado : Form
    {
        private readonly ResultadoPartida resultado;

        public FrmResultado(ResultadoPartida resultado, bool finDePartida = false)
        {
            InitializeComponent();
            Tema.Aplicar(this);
            this.resultado = resultado;
            btnVolverAJugar.Visible = finDePartida;

            lblTitulo.Text = resultado.NombreJugador == null
                ? "Tu resultado"
                : "Resultado de " + resultado.NombreJugador;
            lblCombinacion.Text = resultado.Combinacion.ToString();
            lblDetalle.Text = $"Raza principal: {resultado.RazaPrincipal}\n" +
                              $"Raza secundaria: {resultado.RazaSecundaria}\n" +
                              $"Clase: {resultado.Clase}";

            // por ahora todas las clases usan el mismo avatar
            pbAvatar.Image = Tema.AvatarDefault;
        }

        private void btnPuntajes_Click(object sender, EventArgs e)
        {
            using FrmPuntajes frmPuntajes = new FrmPuntajes(resultado);
            Navegador.Abrir(this, frmPuntajes);
        }

        private void btnAtras_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void btnVolverAJugar_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Retry;
        }
    }
}
