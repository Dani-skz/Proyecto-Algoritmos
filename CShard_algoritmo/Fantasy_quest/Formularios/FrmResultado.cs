using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Fantasy_quest.Dominio;

namespace Fantasy_quest.Formularios
{
    // sirve para el final de la partida y para "Ver resultado" del scoreboard
    internal partial class FrmResultado : Form
    {
        public FrmResultado(ResultadoPartida resultado)
        {
            InitializeComponent();

            lblTitulo.Text = resultado.NombreJugador == null
                ? "Tu resultado"
                : "Resultado de " + resultado.NombreJugador;
            lblCombinacion.Text = resultado.Combinacion.ToString();
            lblDetalle.Text = $"Raza principal: {resultado.RazaPrincipal}   ·   " +
                              $"Raza secundaria: {resultado.RazaSecundaria}   ·   " +
                              $"Clase: {resultado.Clase}";

            // primero las razas y despues las clases, cada grupo de mayor a menor
            var puntosRaza = resultado.PuntosRaza
                .OrderByDescending(p => p.Value)
                .Select(p => new { Tipo = "Raza", Nombre = p.Key.ToString(), Puntos = p.Value });
            var puntosClase = resultado.PuntosClase
                .OrderByDescending(p => p.Value)
                .Select(p => new { Tipo = "Clase", Nombre = p.Key.ToString(), Puntos = p.Value });
            dgvPuntajes.DataSource = puntosRaza.Concat(puntosClase).ToList();
        }

        private void btnAtras_Click(object sender, EventArgs e)
        {
            Close();
        }
    }
}
