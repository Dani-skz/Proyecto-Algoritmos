namespace Fantasy_quest.Formularios
{
    partial class FrmMenuJugador
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
            lblSaludo = new Label();
            lblEstado = new Label();
            btnJugar = new Button();
            btnAbandonar = new Button();
            btnAtras = new Button();
            SuspendLayout();
            //
            // lblSaludo
            //
            lblSaludo.Font = new Font("Segoe UI", 20F, FontStyle.Bold);
            lblSaludo.Location = new Point(0, 60);
            lblSaludo.Name = "lblSaludo";
            lblSaludo.Size = new Size(800, 45);
            lblSaludo.TabIndex = 0;
            lblSaludo.Text = "Hola, jugador";
            lblSaludo.TextAlign = ContentAlignment.MiddleCenter;
            //
            // lblEstado
            //
            lblEstado.Location = new Point(0, 120);
            lblEstado.Name = "lblEstado";
            lblEstado.Size = new Size(800, 25);
            lblEstado.TabIndex = 1;
            lblEstado.TextAlign = ContentAlignment.MiddleCenter;
            //
            // btnJugar
            //
            btnJugar.Font = new Font("Segoe UI", 12F, FontStyle.Bold);
            btnJugar.Location = new Point(300, 170);
            btnJugar.Name = "btnJugar";
            btnJugar.Size = new Size(200, 50);
            btnJugar.TabIndex = 2;
            btnJugar.Text = "Iniciar";
            btnJugar.UseVisualStyleBackColor = true;
            btnJugar.Click += btnJugar_Click;
            //
            // btnAbandonar
            //
            btnAbandonar.Location = new Point(300, 230);
            btnAbandonar.Name = "btnAbandonar";
            btnAbandonar.Size = new Size(200, 35);
            btnAbandonar.TabIndex = 3;
            btnAbandonar.Text = "Abandonar partida pendiente";
            btnAbandonar.UseVisualStyleBackColor = true;
            btnAbandonar.Visible = false;
            btnAbandonar.Click += btnAbandonar_Click;
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
            // FrmMenuJugador
            //
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(800, 500);
            Controls.Add(lblSaludo);
            Controls.Add(lblEstado);
            Controls.Add(btnJugar);
            Controls.Add(btnAbandonar);
            Controls.Add(btnAtras);
            FormBorderStyle = FormBorderStyle.FixedSingle;
            MaximizeBox = false;
            Name = "FrmMenuJugador";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "Fantasy Quest";
            Load += FrmMenuJugador_Load;
            ResumeLayout(false);
        }

        #endregion

        private Label lblSaludo;
        private Label lblEstado;
        private Button btnJugar;
        private Button btnAbandonar;
        private Button btnAtras;
    }
}
