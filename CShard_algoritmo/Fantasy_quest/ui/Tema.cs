using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Reflection;
using Fantasy_quest.enums;

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
        private static readonly Color Tinta = Color.FromArgb(30, 16, 6); // texto sobre el contenedor de pergamino

        private static readonly Image Fondo = CargarImagen("fondo.png");
        private static readonly Image ImagenBoton = CargarImagen("boton.png");
        private static readonly Image ImagenEntrada = CargarImagen("entrada.png");
        private static readonly Image ImagenContenedor = CargarImagen("contenedor.png");

        // una imagen por entrada de CombinacionRaza (6 mestizas + 4 puras)
        private static readonly Dictionary<CombinacionRaza, Image> AvataresCombinacion = new Dictionary<CombinacionRaza, Image>
        {
            { CombinacionRaza.semiorco,        CargarImagen("raza_semiorco.png") },
            { CombinacionRaza.semielfo,        CargarImagen("raza_semielfo.png") },
            { CombinacionRaza.enanogigantismo, CargarImagen("raza_enanogigantismo.png") },
            { CombinacionRaza.ElfoYunqu,       CargarImagen("raza_elfoyunqu.png") },
            { CombinacionRaza.ElfoOscuro,      CargarImagen("raza_elfooscuro.png") },
            { CombinacionRaza.RompeMontaña,    CargarImagen("raza_rompemontana.png") },
            { CombinacionRaza.humano,          CargarImagen("raza_humano.png") },
            { CombinacionRaza.elfo,            CargarImagen("raza_elfo.png") },
            { CombinacionRaza.orco,            CargarImagen("raza_orco.png") },
            { CombinacionRaza.enano,           CargarImagen("raza_enano.png") },
        };

        // marcos estilo warcraftcn-ui. El enano no tiene propio: usa el default
        private static readonly Dictionary<razas, Image> MarcosRaza = new Dictionary<razas, Image>
        {
            { razas.humano, CargarImagen("marco_humano.png") },
            { razas.elfo,   CargarImagen("marco_elfo.png") },
            { razas.orco,   CargarImagen("marco_orco.png") },
            { razas.enano,  CargarImagen("marco_default.png") },
        };

        // iconos de cada clase (las 13 del enum)
        private static readonly Dictionary<clase, Image> IconosClase = new Dictionary<clase, Image>
        {
            { clase.arquero,    CargarImagen("clase_arquero.png") },
            { clase.paladin,    CargarImagen("clase_paladin.png") },
            { clase.picaro,     CargarImagen("clase_picaro.png") },
            { clase.guerrero,   CargarImagen("clase_guerrero.png") },
            { clase.mago,       CargarImagen("clase_mago.png") },
            { clase.granjero,   CargarImagen("clase_granjero.png") },
            { clase.esclavo,    CargarImagen("clase_esclavo.png") },
            { clase.herrero,    CargarImagen("clase_herrero.png") },
            { clase.bufon,      CargarImagen("clase_bufon.png") },
            { clase.explorador, CargarImagen("clase_explorador.png") },
            { clase.druida,     CargarImagen("clase_druida.png") },
            { clase.hechicero,  CargarImagen("clase_hechicero.png") },
            { clase.asesino,    CargarImagen("clase_asesino.png") },
        };

        public static Image? ObtenerIconoClase(clase claseJugador)
        {
            return IconosClase.TryGetValue(claseJugador, out Image? icono) ? icono : null;
        }

        // imagen de raza al 60% centrada (equivalente al inset-[20%] del CSS original)
        // con el marco de la raza principal arriba
        public static Image ComponerAvatar(CombinacionRaza combinacion, razas razaPrincipal)
        {
            Image marco = MarcosRaza.TryGetValue(razaPrincipal, out Image? m) ? m : CargarImagen("marco_default.png");

            if (!AvataresCombinacion.TryGetValue(combinacion, out Image? fondo))
            {
                return marco;
            }

            Bitmap resultado = new Bitmap(marco.Width, marco.Height);
            using (Graphics g = Graphics.FromImage(resultado))
            {
                g.InterpolationMode = InterpolationMode.HighQualityBicubic;
                g.SmoothingMode = SmoothingMode.AntiAlias;

                int ancho = (int)(marco.Width * 0.60);
                int alto = (int)(marco.Height * 0.60);
                int x = (marco.Width - ancho) / 2;
                int y = (marco.Height - alto) / 2;
                g.DrawImage(fondo, new Rectangle(x, y, ancho, alto));
                g.DrawImage(marco, new Rectangle(0, 0, marco.Width, marco.Height));
            }
            return resultado;
        }

        public static void Aplicar(Form formulario)
        {
            formulario.BackColor = Madera;
            formulario.BackgroundImage = Fondo;
            formulario.BackgroundImageLayout = ImageLayout.Stretch;
            AplicarControles(formulario);
            AplicarBarraTitulo(formulario);
        }

        // sin la barra de Windows: la propia va arriba, la ventana crece su alto
        // y todo el contenido baja para que nada quede tapado
        private static void AplicarBarraTitulo(Form formulario)
        {
            formulario.FormBorderStyle = FormBorderStyle.None;
            formulario.ClientSize = new Size(formulario.ClientSize.Width, formulario.ClientSize.Height + BarraTitulo.Alto);
            foreach (Control control in formulario.Controls)
            {
                control.Top += BarraTitulo.Alto;
            }

            formulario.Controls.Add(new BarraTitulo(formulario)
            {
                Bounds = new Rectangle(0, 0, formulario.ClientSize.Width, BarraTitulo.Alto)
            });

            // sin borde de Windows: un marco fino separa la ventana del escritorio
            formulario.Paint += (sender, e) =>
            {
                using Pen borde = new Pen(BarraDesplazamiento.BordePieza);
                e.Graphics.DrawRectangle(borde, 0, 0, formulario.ClientSize.Width - 1, formulario.ClientSize.Height - 1);
            };
        }

        // recorre tambien lo que hay dentro de cada Panel
        private static void AplicarControles(Control contenedor)
        {
            foreach (Control control in contenedor.Controls)
            {
                if (control is Panel panel)
                {
                    AplicarPanel(panel);
                    AplicarControles(panel);
                    continue;
                }

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
                    AplicarEntrada(entrada);
                }
                else if (control is DataGridView grilla)
                {
                    AplicarGrilla(grilla);
                }
                else if (control is PictureBox imagen)
                {
                    imagen.BackColor = Color.Transparent;
                }
            }
        }

        // todo Panel es un contenedor de pergamino (como el contenido de los tabs de warcraftcn).
        // Un Panel dentro de otro queda transparente: solo sirve para recortar (ej. texto con slider)
        private static void AplicarPanel(Panel panel)
        {
            panel.BackColor = Color.Transparent;
            if (panel.Parent is Panel)
            {
                return;
            }
            panel.BackgroundImage = ImagenContenedor;
            panel.BackgroundImageLayout = ImageLayout.Stretch;
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
            etiqueta.BorderStyle = BorderStyle.None;

            if (etiqueta.Parent is Panel)
            {
                etiqueta.ForeColor = Tinta;
            }
            else
            {
                etiqueta.ForeColor = esTitulo ? Dorado : Pergamino;
            }
        }

        // el marco se dibuja sobre el contenedor (formulario o panel), alrededor del TextBox.
        // El TextBox se achica a lo ancho para que el marco ocupe su ancho original
        private static void AplicarEntrada(TextBox entrada)
        {
            const int margen = 12;
            entrada.BorderStyle = BorderStyle.None;
            entrada.BackColor = MaderaClara;
            entrada.ForeColor = Pergamino;
            entrada.Left += margen;
            entrada.Width -= margen * 2;
            entrada.Parent!.Paint += (sender, e) =>
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
