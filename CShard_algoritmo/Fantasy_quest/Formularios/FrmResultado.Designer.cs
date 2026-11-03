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
            pbAvatar = new PictureBox();
            lblCombinacion = new Label();
            lblDetalle = new Label();
            btnPuntajes = new Button();
            btnAtras = new Button();
            btnVolverAJugar = new Button();
            ((System.ComponentModel.ISupportInitialize)pbAvatar).BeginInit();
            SuspendLayout();
            //
            // lblTitulo
            //
            lblTitulo.Font = new Font("Segoe UI", 14F);
            lblTitulo.Location = new Point(0, 25);
            lblTitulo.Name = "lblTitulo";
            lblTitulo.Size = new Size(800, 30);
            lblTitulo.TabIndex = 0;
            lblTitulo.Text = "Tu resultado";
            lblTitulo.TextAlign = ContentAlignment.MiddleCenter;
            //
            // pbAvatar
            //
            pbAvatar.Location = new Point(70, 85);
            pbAvatar.Name = "pbAvatar";
            pbAvatar.Size = new Size(320, 320);
            pbAvatar.SizeMode = PictureBoxSizeMode.Zoom;
            pbAvatar.TabIndex = 1;
            pbAvatar.TabStop = false;
            //
            // lblCombinacion
            //
            lblCombinacion.Font = new Font("Segoe UI", 26F, FontStyle.Bold);
            lblCombinacion.Location = new Point(410, 150);
            lblCombinacion.Name = "lblCombinacion";
            lblCombinacion.Size = new Size(360, 60);
            lblCombinacion.TabIndex = 2;
            lblCombinacion.Text = "Combinacion";
            lblCombinacion.TextAlign = ContentAlignment.MiddleLeft;
            //
            // lblDetalle
            //
            lblDetalle.Font = new Font("Segoe UI", 12F);
            lblDetalle.Location = new Point(412, 215);
            lblDetalle.Name = "lblDetalle";
            lblDetalle.Size = new Size(358, 100);
            lblDetalle.TabIndex = 3;
            lblDetalle.Text = "Raza principal\nRaza secundaria\nClase";
            //
            // btnPuntajes
            //
            btnPuntajes.Location = new Point(610, 445);
            btnPuntajes.Name = "btnPuntajes";
            btnPuntajes.Size = new Size(150, 40);
            btnPuntajes.TabIndex = 4;
            btnPuntajes.Text = "Ver puntajes";
            btnPuntajes.UseVisualStyleBackColor = true;
            btnPuntajes.Click += btnPuntajes_Click;
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
            // btnVolverAJugar
            //
            btnVolverAJugar.Location = new Point(440, 445);
            btnVolverAJugar.Name = "btnVolverAJugar";
            btnVolverAJugar.Size = new Size(160, 40);
            btnVolverAJugar.TabIndex = 6;
            btnVolverAJugar.Text = "Volver a jugar";
            btnVolverAJugar.UseVisualStyleBackColor = true;
            btnVolverAJugar.Visible = false;
            btnVolverAJugar.Click += btnVolverAJugar_Click;
            //
            // FrmResultado
            //
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(800, 500);
            Controls.Add(lblTitulo);
            Controls.Add(pbAvatar);
            Controls.Add(lblCombinacion);
            Controls.Add(lblDetalle);
            Controls.Add(btnPuntajes);
            Controls.Add(btnAtras);
            Controls.Add(btnVolverAJugar);
            FormBorderStyle = FormBorderStyle.FixedSingle;
            MaximizeBox = false;
            Name = "FrmResultado";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "Fantasy Quest";
            ((System.ComponentModel.ISupportInitialize)pbAvatar).EndInit();
            ResumeLayout(false);
        }

        #endregion

        private Label lblTitulo;
        private PictureBox pbAvatar;
        private Label lblCombinacion;
        private Label lblDetalle;
        private Button btnPuntajes;
        private Button btnAtras;
        private Button btnVolverAJugar;
    }
}
