namespace Fantasy_quest.Formularios
{
    partial class FrmResultado
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
            lblCombinacion = new Label();
            lblDetalle = new Label();
            dgvPuntajes = new DataGridView();
            btnAtras = new Button();
            ((System.ComponentModel.ISupportInitialize)dgvPuntajes).BeginInit();
            SuspendLayout();
            //
            // lblTitulo
            //
            lblTitulo.Font = new Font("Segoe UI", 14F);
            lblTitulo.Location = new Point(0, 20);
            lblTitulo.Name = "lblTitulo";
            lblTitulo.Size = new Size(800, 30);
            lblTitulo.TabIndex = 0;
            lblTitulo.Text = "Tu resultado";
            lblTitulo.TextAlign = ContentAlignment.MiddleCenter;
            //
            // lblCombinacion
            //
            lblCombinacion.Font = new Font("Segoe UI", 26F, FontStyle.Bold);
            lblCombinacion.Location = new Point(0, 55);
            lblCombinacion.Name = "lblCombinacion";
            lblCombinacion.Size = new Size(800, 50);
            lblCombinacion.TabIndex = 1;
            lblCombinacion.Text = "Combinacion";
            lblCombinacion.TextAlign = ContentAlignment.MiddleCenter;
            //
            // lblDetalle
            //
            lblDetalle.Location = new Point(0, 110);
            lblDetalle.Name = "lblDetalle";
            lblDetalle.Size = new Size(800, 25);
            lblDetalle.TabIndex = 2;
            lblDetalle.Text = "Raza principal · Raza secundaria · Clase";
            lblDetalle.TextAlign = ContentAlignment.MiddleCenter;
            //
            // dgvPuntajes
            //
            dgvPuntajes.AllowUserToAddRows = false;
            dgvPuntajes.AllowUserToDeleteRows = false;
            dgvPuntajes.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgvPuntajes.Location = new Point(150, 145);
            dgvPuntajes.Name = "dgvPuntajes";
            dgvPuntajes.ReadOnly = true;
            dgvPuntajes.RowHeadersVisible = false;
            dgvPuntajes.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvPuntajes.Size = new Size(500, 295);
            dgvPuntajes.TabIndex = 3;
            //
            // btnAtras
            //
            btnAtras.Location = new Point(12, 453);
            btnAtras.Name = "btnAtras";
            btnAtras.Size = new Size(110, 35);
            btnAtras.TabIndex = 4;
            btnAtras.Text = "← Atrás";
            btnAtras.UseVisualStyleBackColor = true;
            btnAtras.Click += btnAtras_Click;
            //
            // FrmResultado
            //
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(800, 500);
            Controls.Add(lblTitulo);
            Controls.Add(lblCombinacion);
            Controls.Add(lblDetalle);
            Controls.Add(dgvPuntajes);
            Controls.Add(btnAtras);
            FormBorderStyle = FormBorderStyle.FixedSingle;
            MaximizeBox = false;
            Name = "FrmResultado";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "Fantasy Quest";
            ((System.ComponentModel.ISupportInitialize)dgvPuntajes).EndInit();
            ResumeLayout(false);
        }

        #endregion

        private Label lblTitulo;
        private Label lblCombinacion;
        private Label lblDetalle;
        private DataGridView dgvPuntajes;
        private Button btnAtras;
    }
}
