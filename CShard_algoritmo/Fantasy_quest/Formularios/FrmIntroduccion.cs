using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Fantasy_quest.Dominio;
using Fantasy_quest.ui;

namespace Fantasy_quest.Formularios
{
    // introduccion narrativa: FrmPartida la abre antes de cada pregunta.
    // Continuar (o la X) la cierra, corta la narracion y deja ver la pregunta
    internal partial class FrmIntroduccion : Form
    {
        private readonly int numero;

        public FrmIntroduccion(Pregunta pregunta, int numero, int total)
        {
            InitializeComponent();
            Tema.Aplicar(this);

            this.numero = numero;
            lblCapitulo.Text = $"Capítulo {numero} de {total}";
            lblTitulo.Text = pregunta.Titulo;
            lblHistoria.Text = pregunta.Historia;
            MostrarSonido();
            PrepararDesplazamiento();
        }

        private void FrmIntroduccion_Shown(object? sender, EventArgs e)
        {
            Sonido.NarrarCapitulo(numero);
        }

        private void FrmIntroduccion_FormClosed(object? sender, FormClosedEventArgs e)
        {
            Sonido.Narracion.Detener();
        }

        // silencia solo la narracion (la musica de fondo sigue) y queda puesto para las siguientes
        private void btnSonido_Click(object sender, EventArgs e)
        {
            Sonido.Narracion.Silenciado = !Sonido.Narracion.Silenciado;
            MostrarSonido();
        }

        private void MostrarSonido()
        {
            btnSonido.Text = Sonido.Narracion.Silenciado ? "Activar sonido" : "Silenciar";
        }

        // pnlTexto recorta el texto; si no entra, la barra mueve lblHistoria hacia arriba
        private void PrepararDesplazamiento()
        {
            lblHistoria.Height = TextRenderer.MeasureText(lblHistoria.Text, lblHistoria.Font,
                                                          new Size(lblHistoria.Width, int.MaxValue),
                                                          TextFormatFlags.WordBreak).Height;
            int sobrante = lblHistoria.Height - pnlTexto.Height;

            barraHistoria.Visible = sobrante > 0;
            barraHistoria.Maximo = Math.Max(0, sobrante);
            barraHistoria.Pagina = pnlTexto.Height;
            barraHistoria.ValorCambiado += (sender, e) => lblHistoria.Top = -barraHistoria.Valor;

            lblHistoria.MouseWheel += RuedaSobreHistoria;
            pnlTexto.MouseWheel += RuedaSobreHistoria;
            pnlHistoria.MouseWheel += RuedaSobreHistoria;
        }

        private void RuedaSobreHistoria(object? sender, MouseEventArgs e)
        {
            barraHistoria.Rodar(e.Delta);
            // marcado como atendido: asi no sube tambien al panel de afuera y se mueve el doble
            ((HandledMouseEventArgs)e).Handled = true;
        }

        private void btnContinuar_Click(object sender, EventArgs e)
        {
            Close();
        }
    }
}
