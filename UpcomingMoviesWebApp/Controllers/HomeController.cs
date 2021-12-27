using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Net.Http;
using System.Web;
using Newtonsoft.Json.Linq;
using System.Threading;
using System.IO;
using System.Net.TMDb;
using UpcomingMoviesWebApp.Models;

namespace UpcomingMoviesWebApp.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public async Task<ActionResult> IndexAsync(CancellationToken cancellationToken)
        {
            //var client = new HttpClient();
            //var response = client.GetAsync("https://api.themoviedb.org/4/list/1?api_key=1f54bd990f1cdfb230adb312546d765d&page=1");
            //var products = response.Content.ReadAsAsync<IEnumerable<MovieViewModel>>().Result;
            MovieViewModel mv;
            List<MovieViewModel> mvList = new List<MovieViewModel>();

            using (var client = new ServiceClient("1f54bd990f1cdfb230adb312546d765d"))
            {
                for (int i = 1, count = 1000; i <= count; i++)
                {
                    var movies = await client.Movies.GetUpcomingAsync(null, i, cancellationToken);
                    count = 2; // keep track of the actual page count

                    foreach (Movie m in movies.Results)
                    {
                        mv = new MovieViewModel();
                        var movie = await client.Movies.GetAsync(m.Id, null, true, cancellationToken);

                        mv.MovieName = movie.OriginalTitle;
                        mv.ImagePath = "http://image.tmdb.org/t/p/w500" + movie.Poster;
                        mv.ReleaseDate = movie.ReleaseDate;
                        mv.Description = movie.Overview.Replace(",", "").Replace("'", "");
                        mv.Votes = movie.VoteCount;
                        mv.Rating = movie.VoteAverage;


                        List<string> genrelist = new List<string>();

                        foreach (var genre in movie.Genres)
                        {
                            genrelist.Add(genre.Name);
                        }

                        mv.Genre = string.Join("/", genrelist);
                        mvList.Add(mv);
                    }
                }
            }

            return View(mvList);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
