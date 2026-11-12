using Fantasy_quest.Formularios;

namespace Fantasy_quest.ui
{
    // Boton "Ajustes" arriba a la izquierda + dropdown con los controles de volumen
    // (misma logica que FrmPrincipal) y un acceso directo para volver al menu principal.
    // El Panel del dropdown recibe el fondo de pergamino de Tema.AplicarPanel igual que
    // cualquier otro contenedor del juego, asi que Agregar() tiene que llamarse antes
    // de Tema.Aplicar() para que entre en el barrido del tema y en el shift de la barra
    internal static class MenuSettings
    {
        private const int AltoBoton = 35;
        private const int AnchoDropdown = 300;
        private const int AltoDropdown = 210;
        private const int Margen = 10;

        public static void Agregar(Form formulario)
        {
            Button btnAjustes = new Button
            {
                Location = new Point(12, 12),
                Size = new Size(110, AltoBoton),
                Text = "Ajustes",
                TabStop = false
            };

            Panel dropdown = new Panel
            {
                Location = new Point(12, 12 + AltoBoton + 4),
                Size = new Size(AnchoDropdown, AltoDropdown),
                Visible = false
            };

            // los dos renglones de volumen reproducen FrmPrincipal: slider +
            // label con el porcentaje. Sonido es un singleton estatico, asi que
            // cualquier cambio queda guardado para el resto del juego
            Label lblMusica = new Label
            {
                Location = new Point(Margen, Margen),
                Size = new Size(AnchoDropdown - Margen * 2, 22),
                TextAlign = ContentAlignment.MiddleCenter,
                Text = $"Música: {Sonido.Musica.Volumen}%"
            };
            BarraVolumen barraMusica = new BarraVolumen
            {
                Location = new Point(Margen * 2, Margen + 26),
                Size = new Size(AnchoDropdown - Margen * 4, 18),
                Valor = Sonido.Musica.Volumen
            };
            barraMusica.ValorCambiado += (sender, e) =>
            {
                Sonido.Musica.Volumen = barraMusica.Valor;
                lblMusica.Text = $"Música: {barraMusica.Valor}%";
            };

            Label lblNarracion = new Label
            {
                Location = new Point(Margen, Margen + 54),
                Size = new Size(AnchoDropdown - Margen * 2, 22),
                TextAlign = ContentAlignment.MiddleCenter,
                Text = $"Narración: {Sonido.Narracion.Volumen}%"
            };
            BarraVolumen barraNarracion = new BarraVolumen
            {
                Location = new Point(Margen * 2, Margen + 80),
                Size = new Size(AnchoDropdown - Margen * 4, 18),
                Valor = Sonido.Narracion.Volumen
            };
            barraNarracion.ValorCambiado += (sender, e) =>
            {
                Sonido.Narracion.Volumen = barraNarracion.Valor;
                lblNarracion.Text = $"Narración: {barraNarracion.Valor}%";
            };

            Button btnVolverMenu = new Button
            {
                Location = new Point(Margen * 2, AltoDropdown - AltoBoton - Margen),
                Size = new Size(AnchoDropdown - Margen * 4, AltoBoton),
                Text = "Volver al menú principal",
                TabStop = false
            };
            btnVolverMenu.Click += (sender, e) =>
            {
                dropdown.Visible = false;
                Navegador.VolverAlMenuPrincipal(formulario);
            };

            dropdown.Controls.Add(lblMusica);
            dropdown.Controls.Add(barraMusica);
            dropdown.Controls.Add(lblNarracion);
            dropdown.Controls.Add(barraNarracion);
            dropdown.Controls.Add(btnVolverMenu);

            btnAjustes.Click += (sender, e) =>
            {
                dropdown.Visible = !dropdown.Visible;
                if (dropdown.Visible)
                {
                    // refresca los sliders por si el usuario cambio algo en otra ventana
                    barraMusica.Valor = Sonido.Musica.Volumen;
                    barraNarracion.Valor = Sonido.Narracion.Volumen;
                    dropdown.BringToFront();
                }
            };

            formulario.Controls.Add(btnAjustes);
            formulario.Controls.Add(dropdown);
            // Controls.Add deja los controles atras en el z-order: las labels transparentes
            // encima (ej. lblProgreso de FrmPartida, lblCapitulo de FrmIntroduccion) pintan
            // el fondo del form sobre el boton y lo ocultan. BringToFront los pone adelante
            btnAjustes.BringToFront();
            dropdown.BringToFront();
        }
    }
}
