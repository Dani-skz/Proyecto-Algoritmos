using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Fantasy_quest.Dominio;
using Fantasy_quest.Gestoras;
using Fantasy_quest.ui;

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
        // limites del nombre. la BDD declara varchar(100), dejamos margen
        // y pedimos un minimo razonable para evitar nombres de 1 letra
        private const int LargoMinimo = 2;
        private const int LargoMaximo = 30;

        // limites de la contrasena. Se guarda hasheada (SHA-256 = 64 chars fijos)
        // asi que el maximo es el que aceptamos al tipear, no el de la BDD
        private const int LargoMinimoContrasena = 4;
        private const int LargoMaximoContrasena = 30;

        // letras (con acentos y ñ), numeros y espacios internos.
        // nada de simbolos ni emojis: los nombres se muestran en la UI y en el scoreboard
        private static readonly Regex CaracteresValidos =
            new Regex(@"^[A-Za-zÁÉÍÓÚáéíóúÑñÜü0-9 ]+$", RegexOptions.Compiled);

        private readonly ModoIngreso modo;
        private readonly sql_repository repositorio = new sql_repository();

        public Jugador? Jugador { get; private set; }

        public FrmIngreso(ModoIngreso modo)
        {
            InitializeComponent();
            Tema.Aplicar(this);
            this.modo = modo;

            bool esRegistro = modo == ModoIngreso.Registro;
            lblTitulo.Text = esRegistro ? "Registrarse" : "Iniciar sesión";
            btnAceptar.Text = esRegistro ? "Registrarme" : "Entrar";

            // corta el texto en el textbox desde el teclado: no deja pegar/escribir
            // un nombre mas largo del que la BDD aguanta
            txtNombre.MaxLength = LargoMaximo;
        }

        private void btnAceptar_Click(object sender, EventArgs e)
        {
            // trim: espacios al inicio o al final no cuentan como parte del nombre.
            // asi " Pepe " y "Pepe" son el mismo jugador (y la BDD guarda uno solo)
            string nombre = txtNombre.Text.Trim();

            // la contrasena NO se trimea: los espacios al principio o al final
            // son parte legitima si el usuario los eligio
            string contrasena = txtContrasena.Text;

            // primero las validaciones locales para no golpear la base
            // con nombres obviamente invalidos
            string? error = ValidarNombre(nombre);
            if (error != null)
            {
                MessageBox.Show(error, "Nombre inválido", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtNombre.Focus();
                txtNombre.SelectAll();
                return;
            }

            error = ValidarContrasena(contrasena);
            if (error != null)
            {
                MessageBox.Show(error, "Contraseña inválida", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtContrasena.Focus();
                txtContrasena.SelectAll();
                return;
            }

            try
            {
                if (modo == ModoIngreso.Registro)
                {
                    // RegistrarJugadorNuevo tira si el nombre ya existe (lo levanta el SP)
                    Jugador = repositorio.RegistrarJugadorNuevo(nombre, contrasena);
                }
                else
                {
                    // IniciarSesion devuelve null si el nombre no existe y tira si la
                    // contrasena no coincide; cada caso tiene su mensaje
                    Jugador = repositorio.IniciarSesion(nombre, contrasena);
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

        // devuelve el mensaje de error, o null si el nombre es valido.
        // se chequea en este orden porque cada regla asume que la anterior ya paso
        private static string? ValidarNombre(string nombre)
        {
            if (string.IsNullOrWhiteSpace(nombre))
            {
                return "Ingresá un nombre.";
            }

            if (nombre.Length < LargoMinimo)
            {
                return $"El nombre tiene que tener al menos {LargoMinimo} caracteres.";
            }

            if (nombre.Length > LargoMaximo)
            {
                // tecnicamente el MaxLength del textbox ya lo evita, pero lo
                // dejamos por si en el futuro se carga el nombre desde otro lado
                return $"El nombre no puede tener más de {LargoMaximo} caracteres.";
            }

            if (!CaracteresValidos.IsMatch(nombre))
            {
                return "El nombre solo puede tener letras, números y espacios.";
            }

            // no aceptamos dos espacios seguidos: "Juan  Perez" se ve como un typo
            // y ademas confunde las busquedas por nombre
            if (nombre.Contains("  "))
            {
                return "El nombre no puede tener espacios dobles.";
            }

            return null;
        }

        private static string? ValidarContrasena(string contrasena)
        {
            if (string.IsNullOrEmpty(contrasena))
            {
                return "Ingresá una contraseña.";
            }

            if (contrasena.Length < LargoMinimoContrasena)
            {
                return $"La contraseña tiene que tener al menos {LargoMinimoContrasena} caracteres.";
            }

            if (contrasena.Length > LargoMaximoContrasena)
            {
                return $"La contraseña no puede tener más de {LargoMaximoContrasena} caracteres.";
            }

            return null;
        }

        private void btnAtras_Click(object sender, EventArgs e)
        {
            Close();
        }
    }
}
