using AluraTunes.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;

namespace AluraTunes
{
    internal class Program
    {
        static void Main(string[] args)
        {
            using (var contexto = new AluraTunesEntities() )
            {
                var buscaArtista = "Led Zeppelin";
                var buscaAlbum = "Graffiti";

                /*var query = from f in contexto.Faixas
                            where f.Album.Artista.Nome.Contains(buscaArtista)
                            select f;

                if (!string.IsNullOrEmpty(buscaAlbum))
                {
                    query = query.Where(q => q.Album.Titulo.Contains(buscaAlbum));
                }

                query = query.OrderBy(q => q.Album.Titulo).ThenBy(q => q.Nome);*/


                var query = from f in contexto.Faixas
                            where f.Album.Artista.Nome.Contains(buscaArtista)
                            && (!string.IsNullOrEmpty(buscaAlbum) 
                            ? f.Album.Titulo.Contains(buscaAlbum) 
                            : true)
                            orderby f.Album.Titulo, f.Nome
                            select f;

                foreach (var faixa in query)
                {
                    Console.WriteLine("{0}\t{1}", faixa.Album.Titulo.PadRight(40), faixa.Nome);
                }
            }
            //JoinLinQToEntities();
            //LinQToEntities();
            //QueryXML();
            //QuerysBasicas();
        }

        private static void JoinLinQToEntities()
        {
            using (var contexto = new AluraTunesEntities())
            {
                var textoBusca = "Led";

                var query = from a in contexto.Artistas
                            join alb in contexto.Albums
                            on a.ArtistaId equals alb.ArtistaId
                            where a.Nome.Contains(textoBusca)
                            select new
                            {
                                NomeArtista = a.Nome,
                                NomeAlbum = alb.Titulo
                            };

                foreach (var item in query)
                {
                    Console.WriteLine("{0}\t{1}", item.NomeArtista, item.NomeAlbum);
                }

                var query2 = contexto.Artistas.Where(a => a.Nome.Contains(textoBusca));

                Console.WriteLine();
                foreach (var artista in query2)
                {
                    Console.WriteLine("{0}\t{1}", artista.ArtistaId, artista.Nome);
                }

                var query3 = from alb in contexto.Albums
                             where alb.Artista.Nome.Contains(textoBusca)
                             select new
                             {
                                 NomeArtista = alb.Artista.Nome,
                                 NomeAlbum = alb.Titulo
                             };

                Console.WriteLine();
                foreach (var album in query3)
                {
                    Console.WriteLine("{0}\t{1}", album.NomeArtista, album.NomeAlbum);
                }
            }
        }

        private static void LinQToEntities()
        {
            using (var contexto = new AluraTunesEntities())
            {
                var query = from g in contexto.Generos
                            select g;

                foreach (var genero in query)
                {
                    Console.WriteLine("{0}\t{1}", genero.GeneroId, genero.Nome);
                }

                var faixaEGenero = from g in contexto.Generos
                                   join f in contexto.Faixas
                                   on g.GeneroId equals f.GeneroId
                                   select new
                                   {
                                       f,
                                       g
                                   };

                faixaEGenero = faixaEGenero.Take(10);

                contexto.Database.Log = Console.WriteLine;

                Console.WriteLine();

                foreach (var item in faixaEGenero)
                {
                    Console.WriteLine("{0}\t{1}", item.f.Nome, item.g.Nome);
                }

            }
        }

        private static void QueryXML()
        {
            XElement root = XElement.Load(@"C:\Users\Heverton Reis\Documents\Curso-Alura-LinQ\AluraTunes\AluraTunes\Data\AluraTunes.xml");

            var queryXML =
                from g in root.Element("Generos").Elements("Genero")
                select g;

            foreach (var genero in queryXML)
            {
                Console.WriteLine("{0}\t{1}", genero.Element("GeneroId").Value, genero.Element("Nome").Value);
            }

            Console.WriteLine();
            var query = from g in root.Element("Generos").Elements("Genero")
                        join m in root.Element("Musicas").Elements("Musica")
                        on g.Element("GeneroId").Value equals m.Element("GeneroId").Value
                        select new
                        {
                            Musica = m.Element("Nome").Value,
                            Genero = g.Element("Nome").Value
                        };

            Console.WriteLine();
            foreach (var musicaEGenero in query)
            {
                Console.WriteLine("{0}\t{1}", musicaEGenero.Musica, musicaEGenero.Genero);
            }
        }

        private static void QuerysBasicas()
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

            var musicas = new List<Musica>
            {
                new Musica { Id = 1, Nome = "Sweet Child O'Mine", GeneroId = 1},
                new Musica { Id = 2, Nome = "I Shot The Sheriff", GeneroId = 2},
                new Musica { Id = 3, Nome = "Danúbio Azul", GeneroId = 5},
            };

            var musicaQuery = from m in musicas
                              join g in generos on m.GeneroId equals g.Id
                              select new { m, g };

            Console.WriteLine();
            foreach (var musica in musicaQuery)
            {
                Console.WriteLine("{0}\t{1}\t{2}", musica.m.Id, musica.m.Nome, musica.g.Nome);
            }

            var musicaPorGeneroId = from m in musicas
                                    join g in generos on m.GeneroId equals g.Id
                                    where g.Id == 1
                                    select new { m, g };

            Console.WriteLine();
            foreach (var musica in musicaPorGeneroId)
            {
                Console.WriteLine("{0}\t{1}\t{2}", musica.m.Id, musica.m.Nome, musica.g.Nome);
            }

            var listarReggae = from m in musicas
                               join g in generos on m.GeneroId equals g.Id
                               where g.Nome == "Reggae"
                               select new { m, g };

            Console.WriteLine();
            foreach (var musica in listarReggae)
            {
                Console.WriteLine("{0}\t{1}\t{2}", musica.m.Id, musica.m.Nome, musica.g.Nome);
            }
        }
    }

    class Genero
    {
        public int Id { get; set; }
        public string Nome { get; set; }
    }

    class Musica
    {
        public int Id { get; set; }
        public string Nome { get; set; }
        public int GeneroId { get; set; }
    }
}
