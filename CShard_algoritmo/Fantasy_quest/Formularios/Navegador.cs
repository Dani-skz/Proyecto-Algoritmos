using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Fantasy_quest.Gestoras;

namespace Fantasy_quest.Formularios
{
    // Navegacion por capas: la ventana siguiente se abre modal ENCIMA de la actual,
    // en la misma posicion y tamaño. Cerrarla (boton Atras o la X) deja ver otra
    // vez la de abajo. Asi "Atras" siempre es simplemente Close().
    internal static class Navegador
    {
        public static DialogResult Abrir(Form actual, Form siguiente)
        {
            siguiente.StartPosition = FormStartPosition.Manual;
            siguiente.Location = actual.Location;
            siguiente.Size = actual.Size;
            siguiente.ShowInTaskbar = false;
            return siguiente.ShowDialog(actual);
        }

        // partida y, si se termino, su resultado. Atras en el resultado vuelve a "actual"
        public static void Jugar(Form actual, GestorPartida gestor)
        {
            using (FrmPartida frmPartida = new FrmPartida(gestor))
            {
                Abrir(actual, frmPartida);
            }

            if (gestor.Resultado != null)
            {
                using FrmResultado frmResultado = new FrmResultado(gestor.Resultado);
                Abrir(actual, frmResultado);
            }
        }
    }
}
