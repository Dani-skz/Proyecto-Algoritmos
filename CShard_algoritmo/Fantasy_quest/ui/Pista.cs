using System.Reflection;
using System.Windows.Media;

namespace Fantasy_quest.ui
{
    // Un canal de audio (narracion o musica) con su propio volumen y silencio.
    // Usa el MediaPlayer de WPF (UseWPF en el .csproj), que reproduce mp3 y m4a sin instalar nada.
    // Suena un solo archivo a la vez por canal.
    internal class Pista
    {
        private readonly MediaPlayer reproductor = new MediaPlayer();

        private int volumen = 100;
        private bool silenciado;
        private bool repetir;

        public Pista()
        {
            // musica de fondo: al terminar vuelve a empezar
            reproductor.MediaEnded += (sender, e) =>
            {
                if (repetir)
                {
                    reproductor.Position = TimeSpan.Zero;
                    reproductor.Play();
                }
            };
        }

        // 0 a 100, se elige en el menu principal
        public int Volumen
        {
            get => volumen;
            set
            {
                volumen = Math.Clamp(value, 0, 100);
                reproductor.Volume = volumen / 100.0;
            }
        }

        // silenciar no corta el audio: sigue corriendo y al reactivarlo se escucha por donde va
        public bool Silenciado
        {
            get => silenciado;
            set
            {
                silenciado = value;
                reproductor.IsMuted = silenciado;
            }
        }

        // nombre: archivo embebido (Documentacion/Audios o Documentacion/Banda Sonora)
        public void Reproducir(string nombre, bool repetir = false)
        {
            Detener();
            string? ruta = ExtraerAudio(nombre);
            if (ruta == null)
            {
                return; // sin audio el juego sigue igual
            }
            this.repetir = repetir;
            reproductor.Open(new Uri(ruta));
            // Close() del audio anterior deja el volumen en 0.5 y saca el silencio: se vuelven a poner
            reproductor.Volume = volumen / 100.0;
            reproductor.IsMuted = silenciado;
            reproductor.Play();
        }

        public void Detener()
        {
            repetir = false;
            reproductor.Stop();
            reproductor.Close();
        }

        // los audios van embebidos en el ejecutable, pero MediaPlayer solo abre archivos:
        // se copian a la carpeta temporal la primera vez (o si cambiaron de tamaño)
        private static string? ExtraerAudio(string nombre)
        {
            using Stream? recurso = Assembly.GetExecutingAssembly().GetManifestResourceStream("Fantasy_quest.Audios." + nombre);
            if (recurso == null)
            {
                return null;
            }

            string carpeta = Path.Combine(Path.GetTempPath(), "Fantasy_quest");
            string ruta = Path.Combine(carpeta, nombre);
            if (!File.Exists(ruta) || new FileInfo(ruta).Length != recurso.Length)
            {
                Directory.CreateDirectory(carpeta);
                using FileStream archivo = File.Create(ruta);
                recurso.CopyTo(archivo);
            }
            return ruta;
        }
    }
}
