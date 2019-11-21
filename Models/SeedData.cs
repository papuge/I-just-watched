using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using IJustWatched.Data;
using Microsoft.AspNetCore.Identity;

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
                if (!context.Films.Any())
                {
                    context.Films.AddRange(
                    new Film
                    {
                        Title = "Joker",
                        ReleaseDate = DateTime.Parse("2019-10-4"),
                        Directors = "Todd Phillips",
                        PosterURL = "https://m.media-amazon.com/images/M/MV5BNGVjNWI4ZGUtNzE0MS00YTJmLWE0ZDctN2ZiYTk2YmI3NTYyXkEyXkFqcGdeQXVyMTkxNjUyNQ@@._V1_SX300.jpg",
                        TrailerURL = "https://www.youtube.com/embed/zAGVQLHvwOY"
                    },

                    new Film
                    {
                        Title = "Fight Club",
                        ReleaseDate = DateTime.Parse("1999-10-15"),
                        Directors = "David Fincher",
                        PosterURL = "https://m.media-amazon.com/images/M/MV5BMmEzNTkxYjQtZTc0MC00YTVjLTg5ZTEtZWMwOWVlYzY0NWIwXkEyXkFqcGdeQXVyNzkwMjQ5NzM@._V1_SX300.jpg",
                        TrailerURL = "https://www.youtube.com/embed/SUXWAEX2jlg"
                    },

                    new Film
                    {
                        Title = "Blade runner",
                        ReleaseDate = DateTime.Parse("1982-6-25"),
                        Directors = "Ridley Scott",
                        PosterURL = "https://m.media-amazon.com/images/M/MV5BNzQzMzJhZTEtOWM4NS00MTdhLTg0YjgtMjM4MDRkZjUwZDBlXkEyXkFqcGdeQXVyNjU0OTQ0OTY@._V1_SX300.jpg",
                        TrailerURL = "https://www.youtube.com/embed/eogpIG53Cis"
                    },

                    new Film
                    {
                        Title = "Pulp ficiton",
                        ReleaseDate = DateTime.Parse("1994-10-14"),
                        Directors = "Quentin Tarantino",
                        PosterURL = "https://m.media-amazon.com/images/M/MV5BNGNhMDIzZTUtNTBlZi00MTRlLWFjM2ItYzViMjE3YzI5MjljXkEyXkFqcGdeQXVyNzkwMjQ5NzM@._V1_SX300.jpg",
                        TrailerURL = "https://www.youtube.com/embed/5ZAhzsi1ybM"
                    },
                    new Film()
                    {
                        Title = "Interstellar",
                        ReleaseDate = DateTime.Parse("2014-11-7"),
                        Directors = "Christopher Nolan",
                        PosterURL = "https://m.media-amazon.com/images/M/MV5BZjdkOTU3MDktN2IxOS00OGEyLWFmMjktY2FiMmZkNWIyODZiXkEyXkFqcGdeQXVyMTMxODk2OTU@._V1_SX300.jpg",
                        TrailerURL = "https://www.youtube.com/embed/zSWdZVtXT7E"
                    },
                    new Film()
                    {
                        Title = "Mommy",
                        ReleaseDate = DateTime.Parse("2014-10-4"),
                        Directors = "Xavier Dolan",
                        PosterURL = "https://m.media-amazon.com/images/M/MV5BMGI3YWFmNDQtNjc0Ny00ZDBjLThlNjYtZTc1ZTk5MzU2YTVjXkEyXkFqcGdeQXVyNzA4ODc3ODU@._V1_SX300.jpg",
                        TrailerURL = "https://www.youtube.com/embed/dcmueTy2FoM"
                    }
                    );
                    context.SaveChanges();
                }

                if (!context.Users.Any())
                {
                    var userManager = serviceProvider.GetRequiredService<UserManager<User>>();
                    using (var roleContext = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>())
                    {
                        roleContext.CreateAsync(new IdentityRole("admin"));
                        roleContext.CreateAsync(new IdentityRole("moderator"));
                        roleContext.CreateAsync(new IdentityRole("user"));
                    }
                    var users = new List<User>()
                    {
                        new User()
                        {
                            Email = "veronika@mail.ru",
                            UserName = "coose",
                            BirthdayDate = DateTime.Now,
                        },
                        new User()
                        {
                            Email = "sss@gmail.com",
                            UserName = "goblinn",
                            BirthdayDate = DateTime.Now,
                        },
                        new User()
                        {
                            Email = "muser@gmail.com",
                            UserName = "muser",
                            BirthdayDate = DateTime.Now,
                        },
                    };

                    var admin = userManager.CreateAsync(users[0], "Qwerty_9");
                    var moderator = userManager.CreateAsync(users[1], "Mo_1234");
                    var user = userManager.CreateAsync(users[2], "Us_1234");

                    if (admin.IsCompletedSuccessfully && moderator.IsCompletedSuccessfully
                                                      && user.IsCompletedSuccessfully)
                    {
                        userManager.AddToRoleAsync(users[0], "admin");
                        userManager.AddToRoleAsync(users[1], "moderator");
                        userManager.AddToRoleAsync(users[2], "user");
                    }
                    context.SaveChanges();
                }
            }
        }
    }
}