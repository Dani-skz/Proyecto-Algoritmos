using System.ComponentModel;

namespace Fantasy_quest.ui
{
    // Slider horizontal de 0 a 100, con los colores de BarraDesplazamiento.
    // Clic o arrastre en cualquier punto lleva la pieza ahi; la rueda sube o baja de a 5.
    internal class BarraVolumen : Control
    {
        private const int AnchoPieza = 12;
        private const int PasoRueda = 5;

        private int valor;

        public event EventHandler? ValorCambiado;

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public int Valor
        {
            get => valor;
            set
            {
                int nuevo = Math.Clamp(value, 0, 100);
                if (nuevo == valor)
                {
                    return;
                }
                valor = nuevo;
                Invalidate();
                ValorCambiado?.Invoke(this, EventArgs.Empty);
            }
        }

        public BarraVolumen()
        {
            SetStyle(ControlStyles.SupportsTransparentBackColor | ControlStyles.OptimizedDoubleBuffer, true);
            BackColor = Color.Transparent;
        }

        private int Recorrido => Width - AnchoPieza;
        private int IzquierdaPieza => Recorrido * valor / 100;

        protected override void OnPaint(PaintEventArgs e)
        {
            int medio = Height / 2;
            using (SolidBrush pincelRiel = new SolidBrush(BarraDesplazamiento.Riel))
            {
                e.Graphics.FillRectangle(pincelRiel, 0, medio - 2, Width, 4);
            }

            using SolidBrush pincelPieza = new SolidBrush(BarraDesplazamiento.Pieza);
            using Pen borde = new Pen(BarraDesplazamiento.BordePieza);
            // la parte del riel que ya esta "llena" va dorada
            e.Graphics.FillRectangle(pincelPieza, 0, medio - 2, IzquierdaPieza, 4);

            Rectangle pieza = new Rectangle(IzquierdaPieza, 1, AnchoPieza - 1, Height - 3);
            e.Graphics.FillRectangle(pincelPieza, pieza);
            e.Graphics.DrawRectangle(borde, pieza);
        }

        protected override void OnMouseDown(MouseEventArgs e)
        {
            base.OnMouseDown(e);
            MoverA(e.X);
        }

        protected override void OnMouseMove(MouseEventArgs e)
        {
            base.OnMouseMove(e);
            if (e.Button == MouseButtons.Left)
            {
                MoverA(e.X);
            }
        }

        protected override void OnMouseWheel(MouseEventArgs e)
        {
            base.OnMouseWheel(e);
            Valor += e.Delta / 120 * PasoRueda;
        }

        // el centro de la pieza queda debajo del mouse
        private void MoverA(int x)
        {
            if (Recorrido > 0)
            {
                Valor = (x - AnchoPieza / 2) * 100 / Recorrido;
            }
        }
    }
}
