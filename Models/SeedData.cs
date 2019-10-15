using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
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
                    },

                    new Film
                    {
                        Title = "Fight Club",
                        ReleaseDate = DateTime.Parse("1999-10-15")
                    },

                    new Film
                    {
                        Title = "The hateful eight",
                        ReleaseDate = DateTime.Parse("2015-12-3")
                    },

                    new Film
                    {
                        Title = "Rio Bravo",
                        ReleaseDate = DateTime.Parse("1959-4-15")
                    }
                );
                context.SaveChanges();
            }
        }
    }
}