using System.Collections.Generic;

namespace MovieSuggestionApp
{
    public class Movie
    {
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public int Year { get; set; }
        public List<string> Genre { get; set; } = new();
        public double Rating { get; set; }
        public string Plot { get; set; } = string.Empty;
        public string Director { get; set; } = string.Empty;
        public string Runtime { get; set; } = string.Empty;
    }

    public static class MovieRepository
    {
        public static List<Movie> GetMovies()
        {
            return new List<Movie>
            {
                new Movie { Id = 1, Title = "Inception", Year = 2010, Genre = new List<string> { "Sci-Fi", "Action", "Thriller" }, Rating = 8.8, Plot = "A thief who steals corporate secrets through the use of dream-sharing technology...", Director = "Christopher Nolan", Runtime = "148 min" },
                new Movie { Id = 2, Title = "The Grand Budapest Hotel", Year = 2014, Genre = new List<string> { "Comedy", "Drama" }, Rating = 8.1, Plot = "A writer encounters the owner of an aging high-class hotel...", Director = "Wes Anderson", Runtime = "99 min" },
                new Movie { Id = 3, Title = "Mad Max: Fury Road", Year = 2015, Genre = new List<string> { "Action", "Sci-Fi" }, Rating = 8.1, Plot = "In a post-apocalyptic wasteland, a woman rebels against a tyrannical ruler...", Director = "George Miller", Runtime = "120 min" },
                new Movie { Id = 4, Title = "Parasite", Year = 2019, Genre = new List<string> { "Thriller", "Drama", "Comedy" }, Rating = 8.6, Plot = "Greed and class discrimination threaten the newly formed symbiotic relationship...", Director = "Bong Joon Ho", Runtime = "132 min" },
                new Movie { Id = 5, Title = "Spider-Man: Into the Spider-Verse", Year = 2018, Genre = new List<string> { "Animation", "Action", "Adventure" }, Rating = 8.4, Plot = "Teen Miles Morales becomes the Spider-Man of his universe...", Director = "Bob Persichetti", Runtime = "117 min" },
                new Movie { Id = 6, Title = "Knives Out", Year = 2019, Genre = new List<string> { "Mystery", "Comedy", "Crime" }, Rating = 7.9, Plot = "A detective investigates the death of a patriarch of an eccentric, combative family.", Director = "Rian Johnson", Runtime = "130 min" },
                new Movie { Id = 7, Title = "The Dark Knight", Year = 2008, Genre = new List<string> { "Action", "Crime", "Drama" }, Rating = 9.0, Plot = "When the menace known as the Joker wreaks havoc and chaos on the people of Gotham...", Director = "Christopher Nolan", Runtime = "152 min" },
                new Movie { Id = 8, Title = "Everything Everywhere All At Once", Year = 2022, Genre = new List<string> { "Action", "Adventure", "Sci-Fi" }, Rating = 7.8, Plot = "A middle-aged Chinese immigrant is swept up into an insane adventure...", Director = "The Daniels", Runtime = "139 min" },
                new Movie { Id = 9, Title = "Get Out", Year = 2017, Genre = new List<string> { "Horror", "Mystery", "Thriller" }, Rating = 7.7, Plot = "A young African-American visits his white girlfriend's parents for the weekend...", Director = "Jordan Peele", Runtime = "104 min" },
                new Movie { Id = 10, Title = "Arrival", Year = 2016, Genre = new List<string> { "Sci-Fi", "Drama" }, Rating = 7.9, Plot = "A linguist works with the military to communicate with alien lifeforms...", Director = "Denis Villeneuve", Runtime = "116 min" }
            };
        }
    }
}
