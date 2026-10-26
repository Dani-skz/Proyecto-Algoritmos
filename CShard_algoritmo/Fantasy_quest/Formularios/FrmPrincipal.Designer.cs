namespace Fantasy_quest.Formularios
{
    partial class FrmPrincipal
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
            lblSubtitulo = new Label();
            btnRegistrarse = new Button();
            btnLogin = new Button();
            btnAnonimo = new Button();
            btnScoreboard = new Button();
            btnSalir = new Button();
            SuspendLayout();
            //
            // lblTitulo
            //
            lblTitulo.Font = new Font("Segoe UI", 26F, FontStyle.Bold);
            lblTitulo.Location = new Point(0, 50);
            lblTitulo.Name = "lblTitulo";
            lblTitulo.Size = new Size(800, 50);
            lblTitulo.TabIndex = 0;
            lblTitulo.Text = "Fantasy Quest";
            lblTitulo.TextAlign = ContentAlignment.MiddleCenter;
            //
            // lblSubtitulo
            //
            lblSubtitulo.Location = new Point(0, 100);
            lblSubtitulo.Name = "lblSubtitulo";
            lblSubtitulo.Size = new Size(800, 25);
            lblSubtitulo.TabIndex = 1;
            lblSubtitulo.Text = "¿Qué serías en un mundo medieval de fantasía?";
            lblSubtitulo.TextAlign = ContentAlignment.MiddleCenter;
            //
            // btnRegistrarse
            //
            btnRegistrarse.Location = new Point(300, 160);
            btnRegistrarse.Name = "btnRegistrarse";
            btnRegistrarse.Size = new Size(200, 40);
            btnRegistrarse.TabIndex = 2;
            btnRegistrarse.Text = "Registrarse";
            btnRegistrarse.UseVisualStyleBackColor = true;
            btnRegistrarse.Click += btnRegistrarse_Click;
            //
            // btnLogin
            //
            btnLogin.Location = new Point(300, 210);
            btnLogin.Name = "btnLogin";
            btnLogin.Size = new Size(200, 40);
            btnLogin.TabIndex = 3;
            btnLogin.Text = "Iniciar sesión";
            btnLogin.UseVisualStyleBackColor = true;
            btnLogin.Click += btnLogin_Click;
            //
            // btnAnonimo
            //
            btnAnonimo.Location = new Point(300, 260);
            btnAnonimo.Name = "btnAnonimo";
            btnAnonimo.Size = new Size(200, 40);
            btnAnonimo.TabIndex = 4;
            btnAnonimo.Text = "Jugar anónimo";
            btnAnonimo.UseVisualStyleBackColor = true;
            btnAnonimo.Click += btnAnonimo_Click;
            //
            // btnScoreboard
            //
            btnScoreboard.Location = new Point(300, 310);
            btnScoreboard.Name = "btnScoreboard";
            btnScoreboard.Size = new Size(200, 40);
            btnScoreboard.TabIndex = 5;
            btnScoreboard.Text = "Scoreboard";
            btnScoreboard.UseVisualStyleBackColor = true;
            btnScoreboard.Click += btnScoreboard_Click;
            //
            // btnSalir
            //
            btnSalir.Location = new Point(300, 390);
            btnSalir.Name = "btnSalir";
            btnSalir.Size = new Size(200, 35);
            btnSalir.TabIndex = 6;
            btnSalir.Text = "Salir";
            btnSalir.UseVisualStyleBackColor = true;
            btnSalir.Click += btnSalir_Click;
            //
            // FrmPrincipal
            //
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(800, 500);
            Controls.Add(lblTitulo);
            Controls.Add(lblSubtitulo);
            Controls.Add(btnRegistrarse);
            Controls.Add(btnLogin);
            Controls.Add(btnAnonimo);
            Controls.Add(btnScoreboard);
            Controls.Add(btnSalir);
            FormBorderStyle = FormBorderStyle.FixedSingle;
            MaximizeBox = false;
            Name = "FrmPrincipal";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "Fantasy Quest";
            Shown += FrmPrincipal_Shown;
            ResumeLayout(false);
        }

        #endregion

        private Label lblTitulo;
        private Label lblSubtitulo;
        private Button btnRegistrarse;
        private Button btnLogin;
        private Button btnAnonimo;
        private Button btnScoreboard;
        private Button btnSalir;
    }
}
