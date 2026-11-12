using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Fantasy_quest.enums;

namespace Fantasy_quest.Dominio
{
    internal class ResultadoPartida
    {
        public string? NombreJugador { get; } // null = partida anonima
        public razas RazaPrincipal { get; }
        public razas RazaSecundaria { get; }
        public CombinacionRaza Combinacion { get; }
        public clase Clase { get; }
        public Dictionary<razas, int> PuntosRaza { get; }
        public Dictionary<clase, int> PuntosClase { get; }

        public ResultadoPartida(string? nombreJugador, razas razaPrincipal, razas razaSecundaria,
                                CombinacionRaza combinacion, clase clase,
                                Dictionary<razas, int> puntosRaza, Dictionary<clase, int> puntosClase)
        {
            NombreJugador = nombreJugador;
            RazaPrincipal = razaPrincipal;
            RazaSecundaria = razaSecundaria;
            Combinacion = combinacion;
            Clase = clase;
            PuntosRaza = puntosRaza;
            PuntosClase = puntosClase;
        }

        // el orden no importa: humano + elfo = elfo + humano
        public static CombinacionRaza ObtenerCombinacion(razas a, razas b)
        {
            bool Son(razas x, razas y) => (a == x && b == y) || (a == y && b == x);

            if (Son(razas.humano, razas.orco)) return CombinacionRaza.semiorco;
            if (Son(razas.humano, razas.elfo)) return CombinacionRaza.semielfo;
            if (Son(razas.humano, razas.enano)) return CombinacionRaza.enanogigantismo;
            if (Son(razas.elfo, razas.enano)) return CombinacionRaza.ElfoYunqu;
            if (Son(razas.elfo, razas.orco)) return CombinacionRaza.ElfoOscuro;
            if (Son(razas.enano, razas.orco)) return CombinacionRaza.RompeMontaña;

            throw new ArgumentException($"No hay combinacion para {a} + {b}");
        }

        // raza pura: cuando la raza secundaria no llega al umbral de mezcla, el resultado
        // es la raza con mas puntos sin mestizaje. Cada raza tiene su propia entrada en
        // CombinacionRaza con el mismo nombre que la raza
        public static CombinacionRaza ObtenerPura(razas raza) => raza switch
        {
            razas.humano => CombinacionRaza.humano,
            razas.elfo => CombinacionRaza.elfo,
            razas.orco => CombinacionRaza.orco,
            razas.enano => CombinacionRaza.enano,
            _ => throw new ArgumentException($"No hay pura para {raza}")
        };
    }
}
