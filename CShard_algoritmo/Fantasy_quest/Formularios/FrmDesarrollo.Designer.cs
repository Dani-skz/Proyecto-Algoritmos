namespace Fantasy_quest.Formularios
{
    partial class FrmDesarrollo
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
            lblConexion = new Label();
            btnFlujoCompleto = new Button();
            btnRegistro = new Button();
            btnLogin = new Button();
            btnMenuJugador = new Button();
            btnPartidaAnonima = new Button();
            btnResultado = new Button();
            btnScoreboard = new Button();
            SuspendLayout();
            //
            // lblTitulo
            //
            lblTitulo.Font = new Font("Segoe UI", 16F, FontStyle.Bold);
            lblTitulo.Location = new Point(0, 15);
            lblTitulo.Name = "lblTitulo";
            lblTitulo.Size = new Size(800, 35);
            lblTitulo.TabIndex = 0;
            lblTitulo.Text = "Menú de desarrollo (solo Debug)";
            lblTitulo.TextAlign = ContentAlignment.MiddleCenter;
            //
            // lblConexion
            //
            lblConexion.Location = new Point(0, 50);
            lblConexion.Name = "lblConexion";
            lblConexion.Size = new Size(800, 25);
            lblConexion.TabIndex = 1;
            lblConexion.Text = "Base de datos: ...";
            lblConexion.TextAlign = ContentAlignment.MiddleCenter;
            //
            // btnFlujoCompleto
            //
            btnFlujoCompleto.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            btnFlujoCompleto.Location = new Point(250, 90);
            btnFlujoCompleto.Name = "btnFlujoCompleto";
            btnFlujoCompleto.Size = new Size(300, 40);
            btnFlujoCompleto.TabIndex = 2;
            btnFlujoCompleto.Text = "Flujo completo (menú principal)";
            btnFlujoCompleto.UseVisualStyleBackColor = true;
            btnFlujoCompleto.Click += btnFlujoCompleto_Click;
            //
            // btnRegistro
            //
            btnRegistro.Location = new Point(250, 140);
            btnRegistro.Name = "btnRegistro";
            btnRegistro.Size = new Size(300, 40);
            btnRegistro.TabIndex = 3;
            btnRegistro.Text = "Registro";
            btnRegistro.UseVisualStyleBackColor = true;
            btnRegistro.Click += btnRegistro_Click;
            //
            // btnLogin
            //
            btnLogin.Location = new Point(250, 190);
            btnLogin.Name = "btnLogin";
            btnLogin.Size = new Size(300, 40);
            btnLogin.TabIndex = 4;
            btnLogin.Text = "Login";
            btnLogin.UseVisualStyleBackColor = true;
            btnLogin.Click += btnLogin_Click;
            //
            // btnMenuJugador
            //
            btnMenuJugador.Location = new Point(250, 240);
            btnMenuJugador.Name = "btnMenuJugador";
            btnMenuJugador.Size = new Size(300, 40);
            btnMenuJugador.TabIndex = 5;
            btnMenuJugador.Text = "Menú jugador (Aragorn)";
            btnMenuJugador.UseVisualStyleBackColor = true;
            btnMenuJugador.Click += btnMenuJugador_Click;
            //
            // btnPartidaAnonima
            //
            btnPartidaAnonima.Location = new Point(250, 290);
            btnPartidaAnonima.Name = "btnPartidaAnonima";
            btnPartidaAnonima.Size = new Size(300, 40);
            btnPartidaAnonima.TabIndex = 6;
            btnPartidaAnonima.Text = "Partida anónima";
            btnPartidaAnonima.UseVisualStyleBackColor = true;
            btnPartidaAnonima.Click += btnPartidaAnonima_Click;
            //
            // btnResultado
            //
            btnResultado.Location = new Point(250, 340);
            btnResultado.Name = "btnResultado";
            btnResultado.Size = new Size(300, 40);
            btnResultado.TabIndex = 7;
            btnResultado.Text = "Resultado de ejemplo (sin base)";
            btnResultado.UseVisualStyleBackColor = true;
            btnResultado.Click += btnResultado_Click;
            //
            // btnScoreboard
            //
            btnScoreboard.Location = new Point(250, 390);
            btnScoreboard.Name = "btnScoreboard";
            btnScoreboard.Size = new Size(300, 40);
            btnScoreboard.TabIndex = 8;
            btnScoreboard.Text = "Scoreboard";
            btnScoreboard.UseVisualStyleBackColor = true;
            btnScoreboard.Click += btnScoreboard_Click;
            //
            // FrmDesarrollo
            //
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(800, 500);
            Controls.Add(lblTitulo);
            Controls.Add(lblConexion);
            Controls.Add(btnFlujoCompleto);
            Controls.Add(btnRegistro);
            Controls.Add(btnLogin);
            Controls.Add(btnMenuJugador);
            Controls.Add(btnPartidaAnonima);
            Controls.Add(btnResultado);
            Controls.Add(btnScoreboard);
            FormBorderStyle = FormBorderStyle.FixedSingle;
            MaximizeBox = false;
            Name = "FrmDesarrollo";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "Fantasy Quest - Desarrollo";
            Load += FrmDesarrollo_Load;
            ResumeLayout(false);
        }

        #endregion

        private Label lblTitulo;
        private Label lblConexion;
        private Button btnFlujoCompleto;
        private Button btnRegistro;
        private Button btnLogin;
        private Button btnMenuJugador;
        private Button btnPartidaAnonima;
        private Button btnResultado;
        private Button btnScoreboard;
    }
}
