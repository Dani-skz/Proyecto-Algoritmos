namespace Fantasy_quest.Formularios
{
    partial class FrmIntroduccion
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
            lblCapitulo = new Label();
            lblTitulo = new Label();
            pnlHistoria = new Panel();
            pnlTexto = new Panel();
            lblHistoria = new Label();
            barraHistoria = new Fantasy_quest.ui.BarraDesplazamiento();
            btnContinuar = new Button();
            pnlHistoria.SuspendLayout();
            pnlTexto.SuspendLayout();
            SuspendLayout();
            //
            // lblCapitulo
            //
            lblCapitulo.Location = new Point(0, 22);
            lblCapitulo.Name = "lblCapitulo";
            lblCapitulo.Size = new Size(800, 22);
            lblCapitulo.TabIndex = 0;
            lblCapitulo.Text = "Capítulo 1 de 1";
            lblCapitulo.TextAlign = ContentAlignment.MiddleCenter;
            //
            // lblTitulo
            //
            lblTitulo.Font = new Font("Segoe UI", 18F, FontStyle.Bold);
            lblTitulo.Location = new Point(0, 44);
            lblTitulo.Name = "lblTitulo";
            lblTitulo.Size = new Size(800, 40);
            lblTitulo.TabIndex = 1;
            lblTitulo.Text = "Titulo";
            lblTitulo.TextAlign = ContentAlignment.MiddleCenter;
            //
            // pnlHistoria
            //
            pnlHistoria.Controls.Add(pnlTexto);
            pnlHistoria.Controls.Add(barraHistoria);
            pnlHistoria.Location = new Point(40, 90);
            pnlHistoria.Name = "pnlHistoria";
            pnlHistoria.Size = new Size(720, 345);
            pnlHistoria.TabIndex = 2;
            //
            // pnlTexto
            //
            pnlTexto.Controls.Add(lblHistoria);
            pnlTexto.Location = new Point(28, 24);
            pnlTexto.Name = "pnlTexto";
            pnlTexto.Size = new Size(640, 297);
            pnlTexto.TabIndex = 0;
            //
            // lblHistoria
            //
            lblHistoria.Font = new Font("Segoe UI", 11F);
            lblHistoria.Location = new Point(0, 0);
            lblHistoria.Name = "lblHistoria";
            lblHistoria.Size = new Size(640, 297);
            lblHistoria.TabIndex = 0;
            lblHistoria.Text = "Historia";
            //
            // barraHistoria
            //
            barraHistoria.Location = new Point(678, 24);
            barraHistoria.Name = "barraHistoria";
            barraHistoria.Size = new Size(14, 297);
            barraHistoria.TabIndex = 1;
            //
            // btnContinuar
            //
            btnContinuar.Location = new Point(610, 445);
            btnContinuar.Name = "btnContinuar";
            btnContinuar.Size = new Size(150, 40);
            btnContinuar.TabIndex = 3;
            btnContinuar.Text = "Continuar →";
            btnContinuar.UseVisualStyleBackColor = true;
            btnContinuar.Click += btnContinuar_Click;
            //
            // FrmIntroduccion
            //
            AcceptButton = btnContinuar;
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(800, 500);
            Controls.Add(lblCapitulo);
            Controls.Add(lblTitulo);
            Controls.Add(pnlHistoria);
            Controls.Add(btnContinuar);
            FormBorderStyle = FormBorderStyle.FixedSingle;
            MaximizeBox = false;
            Name = "FrmIntroduccion";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "Fantasy Quest";
            pnlTexto.ResumeLayout(false);
            pnlHistoria.ResumeLayout(false);
            ResumeLayout(false);
        }

        #endregion

        private Label lblCapitulo;
        private Label lblTitulo;
        private Panel pnlHistoria;
        private Panel pnlTexto;
        private Label lblHistoria;
        private Fantasy_quest.ui.BarraDesplazamiento barraHistoria;
        private Button btnContinuar;
    }
}
