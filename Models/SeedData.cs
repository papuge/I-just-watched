using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using IJustWatched.Data;

namespace IJustWatched.Models
{
    public static class SeedData
    {
        public static void InitializeFilms(IServiceProvider serviceProvider)
        {
            using (var context = new IJustWatchedContext(
                serviceProvider.GetRequiredService<
                    DbContextOptions<IJustWatchedContext>>()))
            {
                if (context.Films.Any())
                {
                    return;   // DB has been seeded
                }

                context.Films.AddRange(
                    new Film
                    {
                        Title = "Joker",
                        ReleaseDate = DateTime.Parse("2019-10-4"),
                        Directors = "Todd Phillips",
                        PosterURL = "https://m.media-amazon.com/images/M/MV5BNGVjNWI4ZGUtNzE0MS00YTJmLWE0ZDctN2ZiYTk2YmI3NTYyXkEyXkFqcGdeQXVyMTkxNjUyNQ@@._V1_SX300.jpg"
                    },

                    new Film
                    {
                        Title = "Fight Club",
                        ReleaseDate = DateTime.Parse("1999-10-15"),
                        Directors = "David Fincher",
                        PosterURL = "https://m.media-amazon.com/images/M/MV5BMmEzNTkxYjQtZTc0MC00YTVjLTg5ZTEtZWMwOWVlYzY0NWIwXkEyXkFqcGdeQXVyNzkwMjQ5NzM@._V1_SX300.jpg"
                    },

                    new Film
                    {
                        Title = "Blade runner",
                        ReleaseDate = DateTime.Parse("1982-6-25"),
                        Directors = "Ridley Scott",
                        PosterURL = "https://m.media-amazon.com/images/M/MV5BNzQzMzJhZTEtOWM4NS00MTdhLTg0YjgtMjM4MDRkZjUwZDBlXkEyXkFqcGdeQXVyNjU0OTQ0OTY@._V1_SX300.jpg"
                    },

                    new Film
                    {
                        Title = "Pulp ficiton",
                        ReleaseDate = DateTime.Parse("1994-10-14"),
                        Directors = "Quentin Tarantino",
                        PosterURL = "https://m.media-amazon.com/images/M/MV5BNGNhMDIzZTUtNTBlZi00MTRlLWFjM2ItYzViMjE3YzI5MjljXkEyXkFqcGdeQXVyNzkwMjQ5NzM@._V1_SX300.jpg"
                    },
                    new Film()
                    {
                        Title = "Interstellar",
                        ReleaseDate = DateTime.Parse("2014-11-7"),
                        Directors = "Christopher Nolan",
                        PosterURL = "https://m.media-amazon.com/images/M/MV5BZjdkOTU3MDktN2IxOS00OGEyLWFmMjktY2FiMmZkNWIyODZiXkEyXkFqcGdeQXVyMTMxODk2OTU@._V1_SX300.jpg"
                    },
                    new Film()
                    {
                        Title = "Mommy",
                        ReleaseDate = DateTime.Parse("2014-10-4"),
                        Directors = "Xavier Dolan",
                        PosterURL = "https://m.media-amazon.com/images/M/MV5BMGI3YWFmNDQtNjc0Ny00ZDBjLThlNjYtZTc1ZTk5MzU2YTVjXkEyXkFqcGdeQXVyNzA4ODc3ODU@._V1_SX300.jpg"
                    }
                );
                context.SaveChanges();
            }
        }
    }
}