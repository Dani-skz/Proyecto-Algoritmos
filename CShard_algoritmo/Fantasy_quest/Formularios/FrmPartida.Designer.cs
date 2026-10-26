namespace Fantasy_quest.Formularios
{
    partial class FrmPartida
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
            lblProgreso = new Label();
            lblHistoria = new Label();
            lblPregunta = new Label();
            btnRespuesta1 = new Button();
            btnRespuesta2 = new Button();
            btnRespuesta3 = new Button();
            btnRespuesta4 = new Button();
            btnRespuesta5 = new Button();
            btnRespuesta6 = new Button();
            btnAtras = new Button();
            SuspendLayout();
            //
            // lblProgreso
            //
            lblProgreso.AutoSize = true;
            lblProgreso.Location = new Point(12, 12);
            lblProgreso.Name = "lblProgreso";
            lblProgreso.Size = new Size(91, 15);
            lblProgreso.TabIndex = 0;
            lblProgreso.Text = "Pregunta 1 de 1";
            //
            // lblHistoria
            //
            lblHistoria.BorderStyle = BorderStyle.FixedSingle;
            lblHistoria.Font = new Font("Segoe UI", 11F, FontStyle.Italic);
            lblHistoria.Location = new Point(40, 40);
            lblHistoria.Name = "lblHistoria";
            lblHistoria.Padding = new Padding(8);
            lblHistoria.Size = new Size(720, 125);
            lblHistoria.TabIndex = 1;
            lblHistoria.Text = "Historia";
            //
            // lblPregunta
            //
            lblPregunta.Font = new Font("Segoe UI", 13F, FontStyle.Bold);
            lblPregunta.Location = new Point(40, 178);
            lblPregunta.Name = "lblPregunta";
            lblPregunta.Size = new Size(720, 32);
            lblPregunta.TabIndex = 2;
            lblPregunta.Text = "Pregunta";
            lblPregunta.TextAlign = ContentAlignment.MiddleCenter;
            //
            // btnRespuesta1
            //
            btnRespuesta1.Font = new Font("Segoe UI", 10F);
            btnRespuesta1.Location = new Point(40, 225);
            btnRespuesta1.Name = "btnRespuesta1";
            btnRespuesta1.Size = new Size(230, 85);
            btnRespuesta1.TabIndex = 3;
            btnRespuesta1.Text = "Respuesta 1";
            btnRespuesta1.UseVisualStyleBackColor = true;
            btnRespuesta1.Click += btnRespuesta_Click;
            //
            // btnRespuesta2
            //
            btnRespuesta2.Font = new Font("Segoe UI", 10F);
            btnRespuesta2.Location = new Point(285, 225);
            btnRespuesta2.Name = "btnRespuesta2";
            btnRespuesta2.Size = new Size(230, 85);
            btnRespuesta2.TabIndex = 4;
            btnRespuesta2.Text = "Respuesta 2";
            btnRespuesta2.UseVisualStyleBackColor = true;
            btnRespuesta2.Click += btnRespuesta_Click;
            //
            // btnRespuesta3
            //
            btnRespuesta3.Font = new Font("Segoe UI", 10F);
            btnRespuesta3.Location = new Point(530, 225);
            btnRespuesta3.Name = "btnRespuesta3";
            btnRespuesta3.Size = new Size(230, 85);
            btnRespuesta3.TabIndex = 5;
            btnRespuesta3.Text = "Respuesta 3";
            btnRespuesta3.UseVisualStyleBackColor = true;
            btnRespuesta3.Click += btnRespuesta_Click;
            //
            // btnRespuesta4
            //
            btnRespuesta4.Font = new Font("Segoe UI", 10F);
            btnRespuesta4.Location = new Point(40, 320);
            btnRespuesta4.Name = "btnRespuesta4";
            btnRespuesta4.Size = new Size(230, 85);
            btnRespuesta4.TabIndex = 6;
            btnRespuesta4.Text = "Respuesta 4";
            btnRespuesta4.UseVisualStyleBackColor = true;
            btnRespuesta4.Click += btnRespuesta_Click;
            //
            // btnRespuesta5
            //
            btnRespuesta5.Font = new Font("Segoe UI", 10F);
            btnRespuesta5.Location = new Point(285, 320);
            btnRespuesta5.Name = "btnRespuesta5";
            btnRespuesta5.Size = new Size(230, 85);
            btnRespuesta5.TabIndex = 7;
            btnRespuesta5.Text = "Respuesta 5";
            btnRespuesta5.UseVisualStyleBackColor = true;
            btnRespuesta5.Click += btnRespuesta_Click;
            //
            // btnRespuesta6
            //
            btnRespuesta6.Font = new Font("Segoe UI", 10F);
            btnRespuesta6.Location = new Point(530, 320);
            btnRespuesta6.Name = "btnRespuesta6";
            btnRespuesta6.Size = new Size(230, 85);
            btnRespuesta6.TabIndex = 8;
            btnRespuesta6.Text = "Respuesta 6";
            btnRespuesta6.UseVisualStyleBackColor = true;
            btnRespuesta6.Click += btnRespuesta_Click;
            //
            // btnAtras
            //
            btnAtras.Location = new Point(12, 453);
            btnAtras.Name = "btnAtras";
            btnAtras.Size = new Size(110, 35);
            btnAtras.TabIndex = 9;
            btnAtras.Text = "← Atrás";
            btnAtras.UseVisualStyleBackColor = true;
            btnAtras.Click += btnAtras_Click;
            //
            // FrmPartida
            //
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(800, 500);
            Controls.Add(lblProgreso);
            Controls.Add(lblHistoria);
            Controls.Add(lblPregunta);
            Controls.Add(btnRespuesta1);
            Controls.Add(btnRespuesta2);
            Controls.Add(btnRespuesta3);
            Controls.Add(btnRespuesta4);
            Controls.Add(btnRespuesta5);
            Controls.Add(btnRespuesta6);
            Controls.Add(btnAtras);
            FormBorderStyle = FormBorderStyle.FixedSingle;
            MaximizeBox = false;
            Name = "FrmPartida";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "Fantasy Quest";
            FormClosing += FrmPartida_FormClosing;
            Load += FrmPartida_Load;
            Shown += FrmPartida_Shown;
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Label lblProgreso;
        private Label lblHistoria;
        private Label lblPregunta;
        private Button btnRespuesta1;
        private Button btnRespuesta2;
        private Button btnRespuesta3;
        private Button btnRespuesta4;
        private Button btnRespuesta5;
        private Button btnRespuesta6;
        private Button btnAtras;
    }
}
