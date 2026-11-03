using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fantasy_quest.Dominio
{
    internal class Pregunta
    {
        public string Titulo { get; }   // nombre del capitulo, se muestra en FrmIntroduccion
        public string Historia { get; } // introduccion narrativa que se muestra antes de la pregunta
        public string Texto { get; }
        public List<Respuesta> Respuestas { get; } // maximo 6 (botones de FrmPartida)

        public Pregunta(string titulo, string historia, string texto, List<Respuesta> respuestas)
        {
            Titulo = titulo;
            Historia = historia;
            Texto = texto;
            Respuestas = respuestas;
        }
    }
}
