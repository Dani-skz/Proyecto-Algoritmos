using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Fantasy_quest.Gestoras;

namespace Fantasy_quest.Formularios
{
    // Navegacion por capas: la ventana siguiente se abre modal en la misma posicion
    // y tamaño que la actual, y la actual se oculta: solo se ve la que se esta usando.
    // Cerrarla (boton Atras o la X) vuelve a mostrar la de abajo. Asi "Atras" siempre
    // es simplemente Close().
    internal static class Navegador
    {
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

        // partida y, si se termino, su resultado. Atras en el resultado vuelve a "actual"
        public static void Jugar(Form actual, GestorPartida gestor)
        {
            using (FrmPartida frmPartida = new FrmPartida(gestor))
            {
                Abrir(actual, frmPartida, volver: false);
            }

            if (gestor.Resultado != null)
            {
                using FrmResultado frmResultado = new FrmResultado(gestor.Resultado);
                Abrir(actual, frmResultado);
            }
            else
            {
                Volver(actual);
            }
        }
    }
}
