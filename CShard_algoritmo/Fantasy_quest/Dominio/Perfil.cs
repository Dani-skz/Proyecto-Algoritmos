using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Fantasy_quest.enums;

namespace Fantasy_quest.Dominio
{
    // puntos acumulados durante una partida
    internal class Perfil
    {
        public Dictionary<razas, int> PuntosRaza { get; } = new Dictionary<razas, int>();
        public Dictionary<clase, int> PuntosClase { get; } = new Dictionary<clase, int>();

        public Perfil()
        {
            // todas arrancan en 0, asi siempre hay raza principal y secundaria
            foreach (razas raza in Enum.GetValues<razas>())
            {
                PuntosRaza[raza] = 0;
            }
            foreach (clase clase in Enum.GetValues<clase>())
            {
                PuntosClase[clase] = 0;
            }
        }

        public void Aplicar(Respuesta respuesta)
        {
            foreach (KeyValuePair<razas, int> par in respuesta.PuntosRaza)
            {
                PuntosRaza[par.Key] += par.Value;
            }
            foreach (KeyValuePair<clase, int> par in respuesta.PuntosClase)
            {
                PuntosClase[par.Key] += par.Value;
            }
        }

        public ResultadoPartida CalcularResultado(string? nombreJugador)
        {
            // desempate (pendiente de definir): a igual puntaje gana la que esta primero en el enum
            List<KeyValuePair<razas, int>> orden = PuntosRaza.OrderByDescending(p => p.Value).ToList();
            razas principal = orden[0].Key;
            razas secundaria = orden[1].Key;

            // mestiza si la secundaria llega al 33% del total (con enteros para no
            // arrastrar el redondeo de 1/3)
            int total = PuntosRaza.Values.Sum();
            bool esMestiza = total > 0 && orden[1].Value * 3 >= total;

            CombinacionRaza combinacion = esMestiza
                ? ResultadoPartida.ObtenerCombinacion(principal, secundaria)
                : ResultadoPartida.ObtenerPura(principal);

            clase claseGanadora = PuntosClase.OrderByDescending(p => p.Value).First().Key;

            return new ResultadoPartida(
                nombreJugador,
                principal,
                secundaria,
                combinacion,
                claseGanadora,
                new Dictionary<razas, int>(PuntosRaza),
                new Dictionary<clase, int>(PuntosClase));
        }
    }
}
