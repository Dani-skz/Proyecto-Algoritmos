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
            dgvPartidas = new DataGridView();
            btnVerResultado = new Button();
            btnAtras = new Button();
            ((System.ComponentModel.ISupportInitialize)dgvPartidas).BeginInit();
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
            // dgvPartidas
            //
            dgvPartidas.AllowUserToAddRows = false;
            dgvPartidas.AllowUserToDeleteRows = false;
            dgvPartidas.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgvPartidas.Location = new Point(40, 65);
            dgvPartidas.MultiSelect = false;
            dgvPartidas.Name = "dgvPartidas";
            dgvPartidas.ReadOnly = true;
            dgvPartidas.RowHeadersVisible = false;
            dgvPartidas.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvPartidas.Size = new Size(720, 370);
            dgvPartidas.TabIndex = 1;
            dgvPartidas.CellDoubleClick += dgvPartidas_CellDoubleClick;
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
            Controls.Add(dgvPartidas);
            Controls.Add(btnVerResultado);
            Controls.Add(btnAtras);
            FormBorderStyle = FormBorderStyle.FixedSingle;
            MaximizeBox = false;
            Name = "FrmScoreboard";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "Fantasy Quest";
            Load += FrmScoreboard_Load;
            ((System.ComponentModel.ISupportInitialize)dgvPartidas).EndInit();
            ResumeLayout(false);
        }

        #endregion

        private Label lblTitulo;
        private DataGridView dgvPartidas;
        private Button btnVerResultado;
        private Button btnAtras;
    }
}
