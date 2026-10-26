using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;

namespace Fantasy_quest.Formularios
{
    internal static class Mensajes
    {
        // codigo de MySQL para "signal sqlstate '45000'". OJO: MySql.Data deja
        // SqlState vacio en estos errores, por eso se compara Number y no SqlState
        private const int ErrorSignal = 1644;

        // usar en los catch de los formularios: nunca dejar un catch vacio
        public static void MostrarError(Exception ex)
        {
            if (ex is MySqlException mysql && mysql.Number == ErrorSignal)
            {
                // regla de negocio de un procedimiento (signal '45000'): el mensaje ya es para el usuario
                MessageBox.Show(ex.Message, "Atención", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            else if (ex is MySqlException)
            {
                // conexion, usuario/clave, restricciones de tablas, etc.
                MessageBox.Show("Error de base de datos:\n" + ex.Message, "Error",
                                MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else if (ex is InvalidOperationException)
            {
                // reglas propias de C# (ej: "Ya existe un jugador con ese nombre")
                MessageBox.Show(ex.Message, "Atención", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            else
            {
                MessageBox.Show("Error inesperado:\n" + ex.Message, "Error",
                                MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
