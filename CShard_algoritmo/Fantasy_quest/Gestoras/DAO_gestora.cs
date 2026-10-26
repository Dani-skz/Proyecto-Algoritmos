using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;

namespace Fantasy_quest.Gestoras
{
    // solo conectarse a MySQL y ejecutar comandos.
    // No usamos aca las tablas ni los procedimientos, eso lo hace sql_repository.cs.
    internal class DAO_gestora
    {
        // cambiar user / password segun su pc. No borren nada, cambien el :this(Uid=user;Pwd=password;) y comenten en anterior
        private readonly string cadenaConexion;

        public DAO_gestora()
            : this("Server=localhost;Port=3306;Database=fantasy_quest_bdd;Uid=root;Pwd=root;") // -> diego
           // : this("Server=localhost;Port=3306;Database=fantasy_quest_bdd;Uid=root;Pwd=root;") // -> agus
           // : this("Server=localhost;Port=3306;Database=fantasy_quest_bdd;Uid=root;Pwd=root;") // -> dani
        {
        }

        public DAO_gestora(string cadenaConexion)
        {
            this.cadenaConexion = cadenaConexion;
        }

        // ======================= CONEXION =======================

        public MySqlConnection CrearConexion()
        {
            return new MySqlConnection(cadenaConexion);
        }

        // true si se puede abrir la conexion (sirve para avisar al iniciar el juego)
        public bool ProbarConexion()
        {
            try
            {
                using MySqlConnection conexion = CrearConexion();
                conexion.Open();
                return true;
            }
            catch (MySqlException)
            {
                return false;
            }
        }

        // ======================= SQL DE TEXTO =======================

        // insert / update / delete. Devuelve las filas afectadas
        public int EjecutarNoQuery(string sql, Dictionary<string, object?>? parametros = null)
        {
            using MySqlConnection conexion = CrearConexion();
            using MySqlCommand comando = CrearComando(conexion, sql, CommandType.Text, parametros);
            conexion.Open();
            return comando.ExecuteNonQuery();
        }

        // devuelve el primer valor de la primera fila (count, select de una funcion)
        public object? EjecutarEscalar(string sql, Dictionary<string, object?>? parametros = null)
        {
            using MySqlConnection conexion = CrearConexion();
            using MySqlCommand comando = CrearComando(conexion, sql, CommandType.Text, parametros);
            conexion.Open();
            object? resultado = comando.ExecuteScalar();
            return resultado == DBNull.Value ? null : resultado;
        }

        // select: devuelve una tabla en memoria (ya con la conexion cerrada)
        public DataTable EjecutarConsulta(string sql, Dictionary<string, object?>? parametros = null)
        {
            using MySqlConnection conexion = CrearConexion();
            using MySqlCommand comando = CrearComando(conexion, sql, CommandType.Text, parametros);
            DataTable tabla = new DataTable();
            using MySqlDataAdapter adaptador = new MySqlDataAdapter(comando);
            adaptador.Fill(tabla);
            return tabla;
        }

        // ======================= PROCEDIMIENTOS =======================

        // procedimiento sin parametros de salida
        public void EjecutarProcedimiento(string nombre, Dictionary<string, object?>? parametros = null)
        {
            using MySqlConnection conexion = CrearConexion();
            using MySqlCommand comando = CrearComando(conexion, nombre, CommandType.StoredProcedure, parametros);
            conexion.Open();
            comando.ExecuteNonQuery();
        }

        // procedimiento con UN parametro OUT de tipo int (ej: p_id_jugador, p_id_partida)
        public int EjecutarProcedimientoConSalida(string nombre, Dictionary<string, object?> parametros, string parametroSalida)
        {
            using MySqlConnection conexion = CrearConexion();
            using MySqlCommand comando = CrearComando(conexion, nombre, CommandType.StoredProcedure, parametros);

            MySqlParameter salida = new MySqlParameter(parametroSalida, MySqlDbType.Int32);
            salida.Direction = ParameterDirection.Output;
            comando.Parameters.Add(salida);

            conexion.Open();
            comando.ExecuteNonQuery();
            return Convert.ToInt32(salida.Value);
        }

        // ======================= AUXILIARES =======================

        private MySqlCommand CrearComando(MySqlConnection conexion, string texto, CommandType tipo,
                                          Dictionary<string, object?>? parametros)
        {
            MySqlCommand comando = new MySqlCommand(texto, conexion);
            comando.CommandType = tipo;

            if (parametros != null)
            {
                foreach (KeyValuePair<string, object?> parametro in parametros)
                {
                    // null de C# -> NULL de SQL
                    comando.Parameters.AddWithValue(parametro.Key, parametro.Value ?? DBNull.Value);
                }
            }
            return comando;
        }
    }
}
