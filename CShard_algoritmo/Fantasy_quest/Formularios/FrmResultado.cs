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
            lblCombinacion.Text = Capitalizar(resultado.Combinacion);
            lblDetalle.Text = $"Raza principal: {Capitalizar(resultado.RazaPrincipal)}\n" +
                              $"Raza secundaria: {Capitalizar(resultado.RazaSecundaria)}\n" +
                              $"Clase: {Capitalizar(resultado.Clase)}";

            pbAvatar.Image = Tema.ComponerAvatar(resultado.Combinacion, resultado.RazaPrincipal);

            // si la clase todavia no tiene icono (bufon, druida, hechicero, asesino)
            // el PictureBox queda invisible para no mostrar un cuadro vacio
            Image? iconoClase = Tema.ObtenerIconoClase(resultado.Clase);
            pbClase.Image = iconoClase;
            pbClase.Visible = iconoClase != null;
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

        // los enums estan en minuscula para que coincidan con lo que se guarda en la BDD
        // (Enum.ToString / Enum.Parse), pero en pantalla queda mejor con la primera en mayuscula
        private static string Capitalizar<T>(T valor) where T : Enum
        {
            string texto = valor.ToString();
            return string.IsNullOrEmpty(texto) ? texto : char.ToUpper(texto[0]) + texto.Substring(1);
        }
    }
}
