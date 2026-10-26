using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fantasy_quest.Dominio
{
    internal class Jugador
    {
        public int Id { get; }
        public string Nombre { get; }

        public Jugador(int id, string nombre)
        {
            Id = id;
            Nombre = nombre;
        }
    }
}
