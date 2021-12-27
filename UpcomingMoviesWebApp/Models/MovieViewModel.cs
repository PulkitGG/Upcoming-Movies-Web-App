using System;
namespace UpcomingMoviesWebApp.Models
{
    public class MovieViewModel
    {
        public string MovieName { get; set; }
        public string ImagePath { get; set; }
        public DateTime? ReleaseDate { get; set; }
        public string Genre { get; set; }
        public string Description { get; set; }
        public int Votes { get; set; }
        public decimal Rating { get; set; }
        public int MovieId { get; set; }
    }
}
