namespace Fantasy_quest.Formularios
{
    partial class FrmPuntajes
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
            lblRazas = new Label();
            dgvRazas = new DataGridView();
            lblClases = new Label();
            dgvClases = new DataGridView();
            btnAtras = new Button();
            ((System.ComponentModel.ISupportInitialize)dgvRazas).BeginInit();
            ((System.ComponentModel.ISupportInitialize)dgvClases).BeginInit();
            SuspendLayout();
            //
            // lblTitulo
            //
            lblTitulo.Font = new Font("Segoe UI", 18F, FontStyle.Bold);
            lblTitulo.Location = new Point(0, 20);
            lblTitulo.Name = "lblTitulo";
            lblTitulo.Size = new Size(800, 40);
            lblTitulo.TabIndex = 0;
            lblTitulo.Text = "Puntajes";
            lblTitulo.TextAlign = ContentAlignment.MiddleCenter;
            //
            // lblRazas
            //
            lblRazas.Font = new Font("Segoe UI", 12F, FontStyle.Bold);
            lblRazas.Location = new Point(60, 70);
            lblRazas.Name = "lblRazas";
            lblRazas.Size = new Size(320, 25);
            lblRazas.TabIndex = 1;
            lblRazas.Text = "Razas";
            //
            // dgvRazas
            //
            dgvRazas.AllowUserToAddRows = false;
            dgvRazas.AllowUserToDeleteRows = false;
            dgvRazas.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgvRazas.Location = new Point(60, 100);
            dgvRazas.Name = "dgvRazas";
            dgvRazas.ReadOnly = true;
            dgvRazas.RowHeadersVisible = false;
            dgvRazas.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvRazas.Size = new Size(320, 120);
            dgvRazas.TabIndex = 2;
            //
            // lblClases
            //
            lblClases.Font = new Font("Segoe UI", 12F, FontStyle.Bold);
            lblClases.Location = new Point(420, 70);
            lblClases.Name = "lblClases";
            lblClases.Size = new Size(320, 25);
            lblClases.TabIndex = 3;
            lblClases.Text = "Clases";
            //
            // dgvClases
            //
            dgvClases.AllowUserToAddRows = false;
            dgvClases.AllowUserToDeleteRows = false;
            dgvClases.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgvClases.Location = new Point(420, 100);
            dgvClases.Name = "dgvClases";
            dgvClases.ReadOnly = true;
            dgvClases.RowHeadersVisible = false;
            dgvClases.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvClases.Size = new Size(320, 316);
            dgvClases.TabIndex = 4;
            //
            // btnAtras
            //
            btnAtras.Location = new Point(12, 453);
            btnAtras.Name = "btnAtras";
            btnAtras.Size = new Size(110, 35);
            btnAtras.TabIndex = 5;
            btnAtras.Text = "← Atrás";
            btnAtras.UseVisualStyleBackColor = true;
            btnAtras.Click += btnAtras_Click;
            //
            // FrmPuntajes
            //
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(800, 500);
            Controls.Add(lblTitulo);
            Controls.Add(lblRazas);
            Controls.Add(dgvRazas);
            Controls.Add(lblClases);
            Controls.Add(dgvClases);
            Controls.Add(btnAtras);
            FormBorderStyle = FormBorderStyle.FixedSingle;
            MaximizeBox = false;
            Name = "FrmPuntajes";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "Fantasy Quest";
            ((System.ComponentModel.ISupportInitialize)dgvRazas).EndInit();
            ((System.ComponentModel.ISupportInitialize)dgvClases).EndInit();
            ResumeLayout(false);
        }

        #endregion

        private Label lblTitulo;
        private Label lblRazas;
        private DataGridView dgvRazas;
        private Label lblClases;
        private DataGridView dgvClases;
        private Button btnAtras;
    }
}
