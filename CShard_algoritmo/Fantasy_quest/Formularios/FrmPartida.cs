using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Fantasy_quest.Dominio;
using Fantasy_quest.Gestoras;

namespace Fantasy_quest.Formularios
{
    // historia + pregunta + hasta 6 respuestas. Toda la logica esta en GestorPartida.
    // Al terminar se cierra con DialogResult.OK y Navegador.Jugar muestra el resultado
    internal partial class FrmPartida : Form
    {
        private readonly GestorPartida gestor;
        private readonly Button[] botonesRespuesta;

        public FrmPartida(GestorPartida gestor)
        {
            InitializeComponent();
            this.gestor = gestor;
            botonesRespuesta = new Button[]
            {
                btnRespuesta1, btnRespuesta2, btnRespuesta3,
                btnRespuesta4, btnRespuesta5, btnRespuesta6
            };
        }

        private void FrmPartida_Load(object sender, EventArgs e)
        {
            if (!gestor.Terminada)
            {
                MostrarPregunta();
            }
        }

        // una partida reanudada puede tener todas las respuestas pero no haberse finalizado
        private void FrmPartida_Shown(object sender, EventArgs e)
        {
            if (gestor.Terminada)
            {
                Terminar();
            }
        }

        private void MostrarPregunta()
        {
            Pregunta pregunta = gestor.PreguntaActual;
            lblProgreso.Text = $"Pregunta {gestor.IndiceActual + 1} de {gestor.Preguntas.Count}";
            lblHistoria.Text = pregunta.Historia;
            lblPregunta.Text = pregunta.Texto;

            for (int i = 0; i < botonesRespuesta.Length; i++)
            {
                bool existe = i < pregunta.Respuestas.Count;
                botonesRespuesta[i].Visible = existe;
                if (existe)
                {
                    botonesRespuesta[i].Text = pregunta.Respuestas[i].Texto;
                }
            }
        }

        // los 6 botones usan este mismo evento
        private void btnRespuesta_Click(object sender, EventArgs e)
        {
            int nroRespuesta = Array.IndexOf(botonesRespuesta, (Button)sender);
            try
            {
                gestor.Responder(nroRespuesta);
            }
            catch (Exception ex)
            {
                Mensajes.MostrarError(ex);
                return;
            }

            if (gestor.Terminada)
            {
                Terminar();
            }
            else
            {
                MostrarPregunta();
            }
        }

        private void Terminar()
        {
            foreach (Button boton in botonesRespuesta)
            {
                boton.Visible = false;
            }
            lblPregunta.Text = "Calculando resultado...";

            try
            {
                gestor.Finalizar();
            }
            catch (Exception ex)
            {
                // registrada: las respuestas ya estan guardadas, se puede volver y Continuar
                Mensajes.MostrarError(ex);
                return;
            }

            if (gestor.EsAnonima)
            {
                OfrecerGuardar();
            }
            DialogResult = DialogResult.OK;
        }

        private void OfrecerGuardar()
        {
            DialogResult respuesta = MessageBox.Show("¡Terminaste la partida!\n¿Querés registrarte para guardarla?",
                                                     "Guardar partida", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (respuesta != DialogResult.Yes)
            {
                return;
            }

            using FrmIngreso frmIngreso = new FrmIngreso(ModoIngreso.Registro);
            if (Navegador.Abrir(this, frmIngreso) != DialogResult.OK || frmIngreso.Jugador == null)
            {
                return;
            }

            try
            {
                gestor.GuardarEn(frmIngreso.Jugador);
                MessageBox.Show("Partida guardada a nombre de " + frmIngreso.Jugador.Nombre, "Guardar partida",
                                MessageBoxButtons.OK, MessageBoxIcon.Information);
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

        // vale para el boton Atras y para la X de la ventana
        private void FrmPartida_FormClosing(object sender, FormClosingEventArgs e)
        {
            // registrada: el progreso ya esta en la base, se sale sin preguntar
            if (gestor.Resultado != null || !gestor.EsAnonima || gestor.IndiceActual == 0)
            {
                return;
            }

            DialogResult respuesta = MessageBox.Show("Es una partida anónima: si salís se pierde el progreso. ¿Salir igual?",
                                                     "Salir de la partida", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (respuesta != DialogResult.Yes)
            {
                e.Cancel = true;
            }
        }
    }
}
