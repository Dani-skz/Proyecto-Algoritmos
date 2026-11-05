using System.Runtime.InteropServices;
using Fantasy_quest.Formularios;

namespace Fantasy_quest.ui
{
    // Reemplaza la barra de titulo de Windows (Tema.Aplicar deja el formulario sin borde).
    // Muestra el Text del formulario y los botones minimizar y cerrar; arrastrandola se mueve la ventana.
    // Cerrar sale de toda la app desde cualquier ventana (Atras es el que vuelve a la de abajo)
    internal class BarraTitulo : Control
    {
        public const int Alto = 32;
        private const int AnchoBoton = 44;

        private static readonly Color Fondo = Color.FromArgb(16, 12, 8);
        private static readonly Color Linea = Color.FromArgb(110, 70, 20);
        private static readonly Color Texto = Color.FromArgb(240, 198, 90);
        private static readonly Color Encima = Color.FromArgb(60, 46, 32);
        private static readonly Color EncimaCerrar = Color.FromArgb(150, 30, 20);

        private const int WM_NCLBUTTONDOWN = 0xA1;
        private const int HTCAPTION = 2;

        [DllImport("user32.dll")]
        private static extern bool ReleaseCapture();

        [DllImport("user32.dll")]
        private static extern IntPtr SendMessage(IntPtr hWnd, int msg, IntPtr wParam, IntPtr lParam);

        private readonly Form formulario;
        private Point? mouse; // null: el mouse no esta sobre la barra

        public BarraTitulo(Form formulario)
        {
            this.formulario = formulario;
            SetStyle(ControlStyles.OptimizedDoubleBuffer | ControlStyles.ResizeRedraw, true);
            Font = new Font("Palatino Linotype", 10, FontStyle.Bold);
            formulario.TextChanged += (sender, e) => Invalidate();
        }

        private Rectangle BotonCerrar => new Rectangle(Width - AnchoBoton, 0, AnchoBoton, Height - 1);
        private Rectangle BotonMinimizar => new Rectangle(Width - AnchoBoton * 2, 0, AnchoBoton, Height - 1);

        protected override void OnPaint(PaintEventArgs e)
        {
            Graphics g = e.Graphics;
            g.Clear(Fondo);
            using (Pen linea = new Pen(Linea))
            {
                g.DrawLine(linea, 0, Height - 1, Width, Height - 1);
            }

            Rectangle zonaTitulo = new Rectangle(12, 0, BotonMinimizar.Left - 12, Height - 1);
            TextRenderer.DrawText(g, formulario.Text, Font, zonaTitulo, Texto,
                                  TextFormatFlags.Left | TextFormatFlags.VerticalCenter | TextFormatFlags.EndEllipsis);

            DibujarBoton(g, BotonMinimizar, Encima);
            DibujarBoton(g, BotonCerrar, EncimaCerrar);

            // los simbolos se dibujan con lineas: no dependen de la fuente
            using Pen simbolo = new Pen(Texto, 1.5f);
            Point centro = Centro(BotonMinimizar);
            g.DrawLine(simbolo, centro.X - 6, centro.Y, centro.X + 6, centro.Y);

            centro = Centro(BotonCerrar);
            g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
            g.DrawLine(simbolo, centro.X - 5, centro.Y - 5, centro.X + 5, centro.Y + 5);
            g.DrawLine(simbolo, centro.X - 5, centro.Y + 5, centro.X + 5, centro.Y - 5);
        }

        private void DibujarBoton(Graphics g, Rectangle boton, Color colorEncima)
        {
            if (mouse != null && boton.Contains(mouse.Value))
            {
                using SolidBrush pincel = new SolidBrush(colorEncima);
                g.FillRectangle(pincel, boton);
            }
        }

        private static Point Centro(Rectangle r) => new Point(r.Left + r.Width / 2, r.Top + r.Height / 2);

        protected override void OnMouseMove(MouseEventArgs e)
        {
            base.OnMouseMove(e);
            mouse = e.Location;
            Invalidate();
        }

        protected override void OnMouseLeave(EventArgs e)
        {
            base.OnMouseLeave(e);
            mouse = null;
            Invalidate();
        }

        protected override void OnMouseDown(MouseEventArgs e)
        {
            base.OnMouseDown(e);
            if (e.Button != MouseButtons.Left || BotonMinimizar.Contains(e.Location) || BotonCerrar.Contains(e.Location))
            {
                return;
            }
            // Windows hace el arrastre como si se hubiera tomado su propia barra de titulo
            ReleaseCapture();
            SendMessage(formulario.Handle, WM_NCLBUTTONDOWN, (IntPtr)HTCAPTION, IntPtr.Zero);
        }

        // con una partida abierta (tambien si se esta leyendo su introduccion) se pregunta antes.
        // Environment.Exit y no Application.Exit: este ultimo cierra las ventanas una por una y
        // la navegacion por capas intentaria volver a mostrar las de abajo ya cerradas
        private void SalirDelJuego()
        {
            bool jugando = Application.OpenForms.OfType<FrmPartida>().Any();
            if (jugando)
            {
                DialogResult respuesta = MessageBox.Show(formulario, "Estás en medio de una partida. ¿Salir del juego?",
                                                         "Salir", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (respuesta != DialogResult.Yes)
                {
                    return;
                }
            }
            Environment.Exit(0);
        }

        // el clic se confirma al soltar sobre el mismo boton, como en Windows
        protected override void OnMouseUp(MouseEventArgs e)
        {
            base.OnMouseUp(e);
            if (e.Button != MouseButtons.Left)
            {
                return;
            }
            if (BotonCerrar.Contains(e.Location))
            {
                SalirDelJuego();
            }
            else if (BotonMinimizar.Contains(e.Location))
            {
                mouse = null;
                formulario.WindowState = FormWindowState.Minimized;
            }
        }
    }
}
