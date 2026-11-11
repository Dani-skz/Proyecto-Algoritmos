namespace Fantasy_quest.Formularios
{
    partial class FrmIngreso
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
            lblNombre = new Label();
            txtNombre = new TextBox();
            lblContrasena = new Label();
            txtContrasena = new TextBox();
            btnAceptar = new Button();
            btnAtras = new Button();
            SuspendLayout();
            //
            // lblTitulo
            //
            lblTitulo.Font = new Font("Segoe UI", 20F, FontStyle.Bold);
            lblTitulo.Location = new Point(0, 80);
            lblTitulo.Name = "lblTitulo";
            lblTitulo.Size = new Size(800, 45);
            lblTitulo.TabIndex = 0;
            lblTitulo.Text = "Ingreso";
            lblTitulo.TextAlign = ContentAlignment.MiddleCenter;
            //
            // lblNombre
            //
            lblNombre.AutoSize = true;
            lblNombre.Location = new Point(250, 170);
            lblNombre.Name = "lblNombre";
            lblNombre.Size = new Size(54, 15);
            lblNombre.TabIndex = 1;
            lblNombre.Text = "Nombre:";
            //
            // txtNombre
            //
            txtNombre.Location = new Point(250, 203);
            txtNombre.MaxLength = 100;
            txtNombre.Name = "txtNombre";
            txtNombre.Size = new Size(300, 23);
            txtNombre.TabIndex = 2;
            //
            // lblContrasena
            //
            lblContrasena.AutoSize = true;
            lblContrasena.Location = new Point(250, 245);
            lblContrasena.Name = "lblContrasena";
            lblContrasena.Size = new Size(70, 15);
            lblContrasena.TabIndex = 3;
            lblContrasena.Text = "Contraseña:";
            //
            // txtContrasena
            //
            txtContrasena.Location = new Point(250, 278);
            txtContrasena.MaxLength = 30;
            txtContrasena.Name = "txtContrasena";
            txtContrasena.PasswordChar = '•';
            txtContrasena.Size = new Size(300, 23);
            txtContrasena.TabIndex = 4;
            txtContrasena.UseSystemPasswordChar = true;
            //
            // btnAceptar
            //
            btnAceptar.Location = new Point(250, 320);
            btnAceptar.Name = "btnAceptar";
            btnAceptar.Size = new Size(300, 40);
            btnAceptar.TabIndex = 5;
            btnAceptar.Text = "Aceptar";
            btnAceptar.UseVisualStyleBackColor = true;
            btnAceptar.Click += btnAceptar_Click;
            //
            // btnAtras
            //
            btnAtras.Location = new Point(12, 453);
            btnAtras.Name = "btnAtras";
            btnAtras.Size = new Size(110, 35);
            btnAtras.TabIndex = 6;
            btnAtras.Text = "← Atrás";
            btnAtras.UseVisualStyleBackColor = true;
            btnAtras.Click += btnAtras_Click;
            //
            // FrmIngreso
            //
            AcceptButton = btnAceptar;
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            CancelButton = btnAtras;
            ClientSize = new Size(800, 500);
            Controls.Add(lblTitulo);
            Controls.Add(lblNombre);
            Controls.Add(txtNombre);
            Controls.Add(lblContrasena);
            Controls.Add(txtContrasena);
            Controls.Add(btnAceptar);
            Controls.Add(btnAtras);
            FormBorderStyle = FormBorderStyle.FixedSingle;
            MaximizeBox = false;
            Name = "FrmIngreso";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "Fantasy Quest";
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Label lblTitulo;
        private Label lblNombre;
        private TextBox txtNombre;
        private Label lblContrasena;
        private TextBox txtContrasena;
        private Button btnAceptar;
        private Button btnAtras;
    }
}
