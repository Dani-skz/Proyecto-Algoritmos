namespace Fantasy_quest.Formularios
{
    partial class FrmScoreboard
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            lblTitulo = new Label();
            tabs = new TabControl();
            tabPartidas = new TabPage();
            dgvPartidas = new DataGridView();
            tabRanking = new TabPage();
            dgvRanking = new DataGridView();
            btnVerResultado = new Button();
            btnAtras = new Button();
            ((System.ComponentModel.ISupportInitialize)dgvPartidas).BeginInit();
            ((System.ComponentModel.ISupportInitialize)dgvRanking).BeginInit();
            tabs.SuspendLayout();
            tabPartidas.SuspendLayout();
            tabRanking.SuspendLayout();
            SuspendLayout();
            //
            // lblTitulo
            //
            lblTitulo.Font = new Font("Segoe UI", 18F, FontStyle.Bold);
            lblTitulo.Location = new Point(0, 15);
            lblTitulo.Name = "lblTitulo";
            lblTitulo.Size = new Size(800, 40);
            lblTitulo.TabIndex = 0;
            lblTitulo.Text = "Scoreboard";
            lblTitulo.TextAlign = ContentAlignment.MiddleCenter;
            //
            // tabs
            //
            tabs.Controls.Add(tabPartidas);
            tabs.Controls.Add(tabRanking);
            tabs.Location = new Point(40, 65);
            tabs.Name = "tabs";
            tabs.SelectedIndex = 0;
            tabs.Size = new Size(720, 370);
            tabs.TabIndex = 1;
            //
            // tabPartidas
            //
            tabPartidas.Controls.Add(dgvPartidas);
            tabPartidas.Location = new Point(4, 24);
            tabPartidas.Name = "tabPartidas";
            tabPartidas.Padding = new Padding(3);
            tabPartidas.Size = new Size(712, 342);
            tabPartidas.TabIndex = 0;
            tabPartidas.Text = "Partidas";
            tabPartidas.UseVisualStyleBackColor = true;
            //
            // dgvPartidas
            //
            dgvPartidas.AllowUserToAddRows = false;
            dgvPartidas.AllowUserToDeleteRows = false;
            dgvPartidas.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgvPartidas.Dock = DockStyle.Fill;
            dgvPartidas.MultiSelect = false;
            dgvPartidas.Name = "dgvPartidas";
            dgvPartidas.ReadOnly = true;
            dgvPartidas.RowHeadersVisible = false;
            dgvPartidas.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvPartidas.TabIndex = 0;
            dgvPartidas.CellDoubleClick += dgvPartidas_CellDoubleClick;
            //
            // tabRanking
            //
            tabRanking.Controls.Add(dgvRanking);
            tabRanking.Location = new Point(4, 24);
            tabRanking.Name = "tabRanking";
            tabRanking.Padding = new Padding(3);
            tabRanking.Size = new Size(712, 342);
            tabRanking.TabIndex = 1;
            tabRanking.Text = "Ranking";
            tabRanking.UseVisualStyleBackColor = true;
            //
            // dgvRanking
            //
            dgvRanking.AllowUserToAddRows = false;
            dgvRanking.AllowUserToDeleteRows = false;
            dgvRanking.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgvRanking.Dock = DockStyle.Fill;
            dgvRanking.MultiSelect = false;
            dgvRanking.Name = "dgvRanking";
            dgvRanking.ReadOnly = true;
            dgvRanking.RowHeadersVisible = false;
            dgvRanking.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvRanking.TabIndex = 0;
            //
            // btnVerResultado
            //
            btnVerResultado.Location = new Point(610, 453);
            btnVerResultado.Name = "btnVerResultado";
            btnVerResultado.Size = new Size(150, 35);
            btnVerResultado.TabIndex = 2;
            btnVerResultado.Text = "Ver resultado";
            btnVerResultado.UseVisualStyleBackColor = true;
            btnVerResultado.Click += btnVerResultado_Click;
            //
            // btnAtras
            //
            btnAtras.Location = new Point(12, 453);
            btnAtras.Name = "btnAtras";
            btnAtras.Size = new Size(110, 35);
            btnAtras.TabIndex = 3;
            btnAtras.Text = "← Atrás";
            btnAtras.UseVisualStyleBackColor = true;
            btnAtras.Click += btnAtras_Click;
            //
            // FrmScoreboard
            //
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(800, 500);
            Controls.Add(lblTitulo);
            Controls.Add(tabs);
            Controls.Add(btnVerResultado);
            Controls.Add(btnAtras);
            FormBorderStyle = FormBorderStyle.FixedSingle;
            MaximizeBox = false;
            Name = "FrmScoreboard";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "Fantasy Quest";
            Load += FrmScoreboard_Load;
            ((System.ComponentModel.ISupportInitialize)dgvPartidas).EndInit();
            ((System.ComponentModel.ISupportInitialize)dgvRanking).EndInit();
            tabs.ResumeLayout(false);
            tabPartidas.ResumeLayout(false);
            tabRanking.ResumeLayout(false);
            ResumeLayout(false);
        }

        #endregion

        private Label lblTitulo;
        private TabControl tabs;
        private TabPage tabPartidas;
        private DataGridView dgvPartidas;
        private TabPage tabRanking;
        private DataGridView dgvRanking;
        private Button btnVerResultado;
        private Button btnAtras;
    }
}
