namespace Fantasy_quest.ui
{
    // Los dos canales de audio del juego. Cada uno tiene su slider de volumen en el menu principal
    internal static class Sonido
    {
        public const string MusicaFondo = "standard sound.m4a";
        public const string CancionFinal = "ending song.mp3";

        // Narracion-N.mp3 en cada introduccion (FrmIntroduccion)
        public static readonly Pista Narracion = new Pista();

        // musica de fondo todo el juego; al ver el resultado de una partida suena la cancion final
        public static readonly Pista Musica = new Pista();

        public static void NarrarCapitulo(int numero)
        {
            Narracion.Reproducir($"Narracion-{numero}.mp3");
        }

        public static void TocarMusicaFondo()
        {
            Musica.Reproducir(MusicaFondo, repetir: true);
        }
    }
}
