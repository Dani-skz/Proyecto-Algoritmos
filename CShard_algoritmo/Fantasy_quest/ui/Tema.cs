using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Reflection;

namespace Fantasy_quest.ui
{
    // Estetica de warcraftcn-ui sobre controles comunes de Windows Forms:
    // cada formulario llama a Tema.Aplicar(this) despues de InitializeComponent.
    // Las imagenes (ui/*.png) van embebidas en el ejecutable.
    internal static class Tema
    {
        private const string Fuente = "Palatino Linotype";

        private static readonly Color Pergamino = Color.FromArgb(232, 216, 176);
        private static readonly Color Dorado = Color.FromArgb(240, 198, 90);
        private static readonly Color Madera = Color.FromArgb(24, 18, 13);
        private static readonly Color MaderaClara = Color.FromArgb(44, 34, 25);
        private static readonly Color Seleccion = Color.FromArgb(28, 58, 130);

        private static readonly Image Fondo = CargarImagen("fondo.png");
        private static readonly Image ImagenBoton = CargarImagen("boton.png");
        private static readonly Image ImagenEntrada = CargarImagen("entrada.png");

        public static void Aplicar(Form formulario)
        {
            formulario.BackColor = Madera;
            formulario.BackgroundImage = Fondo;
            formulario.BackgroundImageLayout = ImageLayout.Stretch;

            foreach (Control control in formulario.Controls)
            {
                control.Font = new Font(Fuente, control.Font.Size + 1, control.Font.Style);

                if (control is Button boton)
                {
                    AplicarBoton(boton);
                }
                else if (control is Label etiqueta)
                {
                    AplicarEtiqueta(etiqueta);
                }
                else if (control is TextBox entrada)
                {
                    AplicarEntrada(entrada, formulario);
                }
                else if (control is DataGridView grilla)
                {
                    AplicarGrilla(grilla);
                }
            }
        }

        private static void AplicarBoton(Button boton)
        {
            // transparente: en las esquinas de la imagen se ve el fondo del formulario
            boton.UseVisualStyleBackColor = false;
            boton.BackColor = Color.Transparent;
            boton.FlatStyle = FlatStyle.Flat;
            boton.FlatAppearance.BorderSize = 0;
            boton.FlatAppearance.MouseOverBackColor = Color.Transparent;
            boton.FlatAppearance.MouseDownBackColor = Color.Transparent;
            boton.Paint += PintarBoton;
        }

        private static void PintarBoton(object? sender, PaintEventArgs e)
        {
            Button boton = (Button)sender!;
            bool encima = boton.ClientRectangle.Contains(boton.PointToClient(Cursor.Position));
            bool presionado = encima && Control.MouseButtons == MouseButtons.Left;
            float brillo = presionado ? 0.75f : encima ? 1.25f : 1f;

            DibujarMarco(e.Graphics, ImagenBoton, boton.ClientRectangle, 24, 10, brillo);

            Rectangle zonaTexto = Rectangle.Inflate(boton.ClientRectangle, -10, -4);
            TextRenderer.DrawText(e.Graphics, boton.Text, boton.Font, zonaTexto, Color.White,
                                  TextFormatFlags.HorizontalCenter | TextFormatFlags.VerticalCenter | TextFormatFlags.WordBreak);

            if (boton.Focused)
            {
                using Pen borde = new Pen(Dorado);
                e.Graphics.DrawRectangle(borde, Rectangle.Inflate(boton.ClientRectangle, -5, -5));
            }
        }

        private static void AplicarEtiqueta(Label etiqueta)
        {
            bool esTitulo = etiqueta.Font.Size >= 18;
            etiqueta.BackColor = Color.Transparent;
            etiqueta.ForeColor = esTitulo ? Dorado : Pergamino;
            etiqueta.BorderStyle = BorderStyle.None;
        }

        // el marco se dibuja sobre el formulario, alrededor del TextBox.
        // El TextBox se achica a lo ancho para que el marco ocupe su ancho original
        private static void AplicarEntrada(TextBox entrada, Form formulario)
        {
            const int margen = 12;
            entrada.BorderStyle = BorderStyle.None;
            entrada.BackColor = MaderaClara;
            entrada.ForeColor = Pergamino;
            entrada.Left += margen;
            entrada.Width -= margen * 2;
            formulario.Paint += (sender, e) =>
                DibujarMarco(e.Graphics, ImagenEntrada, Rectangle.Inflate(entrada.Bounds, margen, 8), 32, 8, 1f);
        }

        private static void AplicarGrilla(DataGridView grilla)
        {
            grilla.BackgroundColor = Madera;
            grilla.GridColor = MaderaClara;
            grilla.BorderStyle = BorderStyle.None;
            grilla.EnableHeadersVisualStyles = false;
            grilla.ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.Single;
            grilla.RowTemplate.Height = 22;

            grilla.DefaultCellStyle.BackColor = Madera;
            grilla.DefaultCellStyle.ForeColor = Pergamino;
            grilla.DefaultCellStyle.SelectionBackColor = Seleccion;
            grilla.DefaultCellStyle.SelectionForeColor = Color.White;

            grilla.ColumnHeadersDefaultCellStyle.BackColor = MaderaClara;
            grilla.ColumnHeadersDefaultCellStyle.ForeColor = Dorado;
            grilla.ColumnHeadersDefaultCellStyle.SelectionBackColor = MaderaClara;
            grilla.ColumnHeadersDefaultCellStyle.SelectionForeColor = Dorado;
        }

        // Equivalente a border-image de CSS: las esquinas conservan su tamaño y solo
        // se estiran los lados y el centro. "corte" se mide en la imagen y "borde" en pantalla.
        private static void DibujarMarco(Graphics g, Image imagen, Rectangle destino, int corte, int borde, float brillo)
        {
            int[] origenX = { 0, corte, imagen.Width - corte, imagen.Width };
            int[] origenY = { 0, corte, imagen.Height - corte, imagen.Height };
            int[] destinoX = { destino.Left, destino.Left + borde, destino.Right - borde, destino.Right };
            int[] destinoY = { destino.Top, destino.Top + borde, destino.Bottom - borde, destino.Bottom };

            using ImageAttributes atributos = new ImageAttributes();
            atributos.SetWrapMode(WrapMode.TileFlipXY); // sin esto se notan las uniones entre partes
            atributos.SetColorMatrix(new ColorMatrix { Matrix00 = brillo, Matrix11 = brillo, Matrix22 = brillo });

            g.InterpolationMode = InterpolationMode.HighQualityBicubic;
            g.PixelOffsetMode = PixelOffsetMode.HighQuality;

            for (int fila = 0; fila < 3; fila++)
            {
                for (int columna = 0; columna < 3; columna++)
                {
                    Rectangle parte = Rectangle.FromLTRB(destinoX[columna], destinoY[fila], destinoX[columna + 1], destinoY[fila + 1]);
                    g.DrawImage(imagen, parte, origenX[columna], origenY[fila],
                                origenX[columna + 1] - origenX[columna], origenY[fila + 1] - origenY[fila],
                                GraphicsUnit.Pixel, atributos);
                }
            }
        }

        private static Image CargarImagen(string nombre)
        {
            // el Bitmap necesita el stream abierto mientras exista la imagen
            Stream stream = Assembly.GetExecutingAssembly().GetManifestResourceStream("Fantasy_quest.ui." + nombre)!;
            return new Bitmap(stream);
        }
    }
}
