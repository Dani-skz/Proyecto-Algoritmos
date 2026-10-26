using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Fantasy_quest.enums;

namespace Fantasy_quest.Dominio
{
    // Las preguntas viven en el codigo, no en la base.
    // IMPORTANTE: la base guarda la POSICION de la pregunta y de la respuesta
    // (nro_pregunta, nro_respuesta). No cambien el orden de las que ya existen:
    // las nuevas se agregan al final. Maximo 6 respuestas por pregunta.
    internal static class BancoPreguntas
    {
        public static List<Pregunta> Crear()
        {
            return new List<Pregunta>
            {
                new Pregunta(
                    "[Historia pendiente] Aca va el fragmento de la historia antes de la pregunta 1.",
                    "¿Qué desayunarías?",
                    new List<Respuesta>
                    {
                        new Respuesta("Cerveza y pan").Suma(razas.enano, 1).Suma(clase.herrero, 1),
                        new Respuesta("Carne").Suma(razas.orco, 1).Suma(razas.enano, 1).Suma(clase.guerrero, 1),
                        new Respuesta("Frutos secos").Suma(razas.elfo, 1).Suma(razas.humano, 1).Suma(clase.druida, 1),
                        new Respuesta("Pan, queso y avena").Suma(razas.humano, 1).Suma(clase.granjero, 1), // "campesino"
                        new Respuesta("Poción energética").Suma(razas.elfo, 1).Suma(clase.mago, 1),
                        new Respuesta("Carne de goblin").Suma(razas.orco, 1).Suma(clase.asesino, 1),
                    }),
            };
        }
    }
}
