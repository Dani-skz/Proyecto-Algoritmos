using System.ComponentModel;
using System.Drawing.Drawing2D;

namespace Fantasy_quest.ui
{
    // Slider vertical con la estetica del juego (la barra de scroll de Windows queda blanca).
    // Valor va de 0 a Maximo: son los pixeles que se corre el contenido.
    // Se arrastra la pieza dorada, se hace clic en el riel o se usa la rueda del mouse.
    internal class BarraDesplazamiento : Control
    {
        private const int AltoMinimoPieza = 30;
        private const int PasoRueda = 40;

        // tambien los usa BarraVolumen
        internal static readonly Color Riel = Color.FromArgb(90, 60, 30);
        internal static readonly Color Pieza = Color.FromArgb(240, 198, 90);
        internal static readonly Color BordePieza = Color.FromArgb(110, 70, 20);

        private int valor;
        private int? arrastreDesde; // distancia entre el clic y el borde superior de la pieza

        // Hidden: se cargan desde el codigo del formulario, el diseñador no las guarda
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public int Maximo { get; set; }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public int Pagina { get; set; } // alto visible del contenido: define el tamaño de la pieza

        public event EventHandler? ValorCambiado;

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public int Valor
        {
            get => valor;
            set
            {
                int nuevo = Math.Clamp(value, 0, Maximo);
                if (nuevo == valor)
                {
                    return;
                }
                valor = nuevo;
                Invalidate();
                ValorCambiado?.Invoke(this, EventArgs.Empty);
            }
        }

        public BarraDesplazamiento()
        {
            SetStyle(ControlStyles.SupportsTransparentBackColor | ControlStyles.OptimizedDoubleBuffer, true);
            BackColor = Color.Transparent;
        }

        // para que tambien la rueda sobre el texto mueva la barra
        public void Rodar(int delta)
        {
            Valor -= delta / 120 * PasoRueda;
        }

        private int AltoPieza => Math.Max(AltoMinimoPieza, Height * Pagina / Math.Max(1, Pagina + Maximo));
        private int RecorridoPieza => Height - AltoPieza;
        private int TopePieza => Maximo == 0 ? 0 : RecorridoPieza * valor / Maximo;

        protected override void OnPaint(PaintEventArgs e)
        {
            e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;

            using (SolidBrush pincelRiel = new SolidBrush(Riel))
            {
                e.Graphics.FillRectangle(pincelRiel, Width / 2 - 2, 0, 4, Height);
            }

            Rectangle pieza = new Rectangle(1, TopePieza, Width - 3, AltoPieza - 1);
            using SolidBrush pincelPieza = new SolidBrush(Pieza);
            using Pen borde = new Pen(BordePieza);
            e.Graphics.FillRectangle(pincelPieza, pieza);
            e.Graphics.DrawRectangle(borde, pieza);
        }

        protected override void OnMouseDown(MouseEventArgs e)
        {
            base.OnMouseDown(e);
            bool sobrePieza = e.Y >= TopePieza && e.Y < TopePieza + AltoPieza;
            if (sobrePieza)
            {
                arrastreDesde = e.Y - TopePieza;
            }
            else
            {
                // clic en el riel: avanza o retrocede una pagina
                Valor += e.Y < TopePieza ? -Pagina : Pagina;
            }
        }

        protected override void OnMouseMove(MouseEventArgs e)
        {
            base.OnMouseMove(e);
            if (arrastreDesde != null && RecorridoPieza > 0)
            {
                Valor = (e.Y - arrastreDesde.Value) * Maximo / RecorridoPieza;
            }
        }

        protected override void OnMouseUp(MouseEventArgs e)
        {
            base.OnMouseUp(e);
            arrastreDesde = null;
        }

        protected override void OnMouseWheel(MouseEventArgs e)
        {
            base.OnMouseWheel(e);
            Rodar(e.Delta);
        }
    }
}
