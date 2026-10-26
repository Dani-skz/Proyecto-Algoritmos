using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Fantasy_quest.Dominio;
using Fantasy_quest.Gestoras;

namespace Fantasy_quest.Formularios
{
    // partidas finalizadas de todos los jugadores
    internal partial class FrmScoreboard : Form
    {
        private readonly sql_repository repositorio = new sql_repository();

        public FrmScoreboard()
        {
            InitializeComponent();
        }

        private void FrmScoreboard_Load(object sender, EventArgs e)
        {
            try
            {
                dgvPartidas.DataSource = repositorio.ObtenerScoreboard();
                // el id solo se usa para "Ver resultado"
                DataGridViewColumn? columnaId = dgvPartidas.Columns["id_partida"];
                if (columnaId != null)
                {
                    columnaId.Visible = false;
                }
            }
            catch (Exception ex)
            {
                Mensajes.MostrarError(ex);
            }
        }

        private void btnVerResultado_Click(object sender, EventArgs e)
        {
            VerResultado();
        }

        private void dgvPartidas_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                VerResultado();
            }
        }

        private void VerResultado()
        {
            if (dgvPartidas.CurrentRow == null)
            {
                MessageBox.Show("Elegí una partida de la lista.", "Scoreboard",
                                MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            int idPartida = Convert.ToInt32(dgvPartidas.CurrentRow.Cells["id_partida"].Value);
            ResultadoPartida resultado;
            try
            {
                resultado = repositorio.ObtenerResultado(idPartida);
            }
            catch (Exception ex)
            {
                Mensajes.MostrarError(ex);
                return;
            }

            using FrmResultado frmResultado = new FrmResultado(resultado);
            Navegador.Abrir(this, frmResultado);
        }

        private void btnAtras_Click(object sender, EventArgs e)
        {
            Close();
        }
    }
}
