using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fantasy_quest.enums
{
    public enum CombinacionRaza
    {
        // mestizas: la raza secundaria llega al 33% del total de puntos de raza
        semiorco, // humano + orco
        semielfo, // humano + elfo
        enanogigantismo, //enano+humano
        ElfoYunqu, //elfo + enano
        ElfoOscuro, // elfo + orco
        RompeMontaña, // enano + orco

        // puras: la raza secundaria no llega al 33%, el resultado es la raza con mas puntos
        humano,
        elfo,
        orco,
        enano
    }
}
