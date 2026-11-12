using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Fantasy_quest.Gestoras;
using Fantasy_quest.ui;

namespace Fantasy_quest.Formularios
{
    // Navegacion por capas: la ventana siguiente se abre modal en la misma posicion
    // y tamaño que la actual, y la actual se oculta: solo se ve la que se esta usando.
    // Cerrarla (boton Atras o la X) vuelve a mostrar la de abajo. Asi "Atras" siempre
    // es simplemente Close().
    internal static class Navegador
    {
        // propagacion del "Volver al menu principal": cada Abrir que vuelve cierra
        // tambien su capa hasta llegar a FrmPrincipal
        private static bool regresandoAlMenu;

        // volver = false: quien llama abre otra ventana a continuacion (asi no se ve
        // "actual" entre las dos) o la muestra despues con Volver
        public static DialogResult Abrir(Form actual, Form siguiente, bool volver = true)
        {
            siguiente.StartPosition = FormStartPosition.Manual;
            siguiente.Location = actual.Location;
            siguiente.Size = actual.Size;
            // con la dueña oculta, este es el unico boton de la app en la barra de tareas
            siguiente.ShowInTaskbar = true;
            // recien en Shown: la carga puede tardar (base de datos) y no debe quedar un hueco sin ventana
            siguiente.Shown += (sender, e) => actual.Hide();

            DialogResult resultado = siguiente.ShowDialog(actual);

            // si movieron la ventana, la de abajo reaparece en el mismo lugar
            if (siguiente.WindowState == FormWindowState.Normal)
            {
                actual.Location = siguiente.Location;
            }

            if (regresandoAlMenu)
            {
                if (actual is FrmPrincipal)
                {
                    regresandoAlMenu = false;
                    if (volver)
                    {
                        Volver(actual);
                    }
                }
                else
                {
                    actual.Close();
                }
                return resultado;
            }

            if (volver)
            {
                Volver(actual);
            }
            return resultado;
        }

        public static void Volver(Form actual)
        {
            actual.Show();
            actual.Activate();
        }

        public static void VolverAlMenuPrincipal(Form actual)
        {
            regresandoAlMenu = true;
            actual.Close();
        }

        // para los FormClosing que cancelan el cierre (ej. partida anonima con progreso):
        // sin esto, se cerrarian igual las capas de abajo
        public static void CancelarVueltaAlMenu()
        {
            regresandoAlMenu = false;
        }

        // partida y, si se termino, su resultado (con la cancion final).
        // Atras en el resultado vuelve a "actual" y a la musica de fondo.
        // Volver a jugar vuelve al menu principal: si "actual" es el menu del jugador
        // tambien se cierra (cierra la sesion) y queda a la vista el menu principal de abajo
        public static void Jugar(Form actual, GestorPartida gestor)
        {
            using (FrmPartida frmPartida = new FrmPartida(gestor))
            {
                Abrir(actual, frmPartida, volver: false);
            }

            // Abrir ya arranco la propagacion: no mostramos resultado ni volvemos
            if (regresandoAlMenu)
            {
                return;
            }

            if (gestor.Resultado != null)
            {
                using FrmResultado frmResultado = new FrmResultado(gestor.Resultado, finDePartida: true);
                Sonido.Musica.Reproducir(Sonido.CancionFinal);
                DialogResult respuesta = Abrir(actual, frmResultado, volver: false);
                Sonido.TocarMusicaFondo();

                if (respuesta == DialogResult.Retry && actual is FrmMenuJugador)
                {
                    actual.Close();
                }
                else
                {
                    Volver(actual);
                }
            }
            else
            {
                Volver(actual);
            }
        }
    }
}
