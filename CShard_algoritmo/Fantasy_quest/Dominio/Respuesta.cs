using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Fantasy_quest.enums;

namespace Fantasy_quest.Dominio
{
    internal class Respuesta
    {
        public string Texto { get; }
        public Dictionary<razas, int> PuntosRaza { get; } = new Dictionary<razas, int>();
        public Dictionary<clase, int> PuntosClase { get; } = new Dictionary<clase, int>();

        public Respuesta(string texto)
        {
            Texto = texto;
        }

        // devuelven la misma respuesta para poder encadenar:
        // new Respuesta("Pocion").Suma(razas.elfo, 1).Suma(clase.mago, 1)
        public Respuesta Suma(razas raza, int puntos)
        {
            PuntosRaza[raza] = puntos;
            return this;
        }

        public Respuesta Suma(clase clase, int puntos)
        {
            PuntosClase[clase] = puntos;
            return this;
        }
    }
}
