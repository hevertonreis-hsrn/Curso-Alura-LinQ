using System;
using System.Collections.Generic;
using System.Linq;

namespace AluraTunes
{
    internal class Program
    {
        static void Main(string[] args)
        {
            var generos = new List<Genero>
            {
                new Genero { Id = 1, Nome = "Rock"},
                new Genero { Id = 2, Nome = "Reggae"},
                new Genero { Id = 3, Nome = "Rock Progressivo"},
                new Genero { Id = 4, Nome = "Punk Rock"},
                new Genero { Id = 5, Nome = "Clássico"},
            };

            foreach (var genero in generos)
            {
                if (genero.Nome.Contains("Rock"))
                {
                    Console.WriteLine("{0}\t{1}", genero.Id, genero.Nome);
                }
            }

            var query = from g in generos
                        where g.Nome.Contains("Rock")
                        select g;

            Console.WriteLine();
            foreach (var genero in query)
            {
                Console.WriteLine("{0}\t{1}", genero.Id, genero.Nome);
            }


        }
    }

    class Genero
    {
        public int Id { get; set; }
        public string Nome { get; set; }
    }
}
