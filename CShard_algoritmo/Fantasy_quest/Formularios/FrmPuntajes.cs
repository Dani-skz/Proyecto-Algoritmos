using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Fantasy_quest.Dominio;
using Fantasy_quest.ui;

namespace Fantasy_quest.Formularios
{
    // detalle del resultado: puntos de cada raza y de cada clase, de mayor a menor
    internal partial class FrmPuntajes : Form
    {
        public FrmPuntajes(ResultadoPartida resultado)
        {
            InitializeComponent();
            Tema.Aplicar(this);

            lblTitulo.Text = resultado.NombreJugador == null
                ? "Tus puntajes"
                : "Puntajes de " + resultado.NombreJugador;

            dgvRazas.DataSource = resultado.PuntosRaza
                .OrderByDescending(p => p.Value)
                .Select(p => new { Raza = p.Key.ToString(), Puntos = p.Value })
                .ToList();
            dgvClases.DataSource = resultado.PuntosClase
                .OrderByDescending(p => p.Value)
                .Select(p => new { Clase = p.Key.ToString(), Puntos = p.Value })
                .ToList();
        }

        private void btnAtras_Click(object sender, EventArgs e)
        {
            Close();
        }
    }
}
