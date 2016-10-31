using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Vidly.Models;
using Vidly.ViewModels;
using System.Data.Entity;


namespace Vidly.Controllers
{
    public class MoviesController : Controller
    {
        private ApplicationDbContext _movieContext;

        public MoviesController()
        {
            _movieContext = new ApplicationDbContext();

        }

        protected override void Dispose(bool disposing)
        {
            _movieContext.Dispose() ;
        }

        // GET: Movies/Random
        public ActionResult Randomx()
        {
            var movie = new Movie() { Name = "Shrek!" };

            var customers = new List<Customer>
            {
                new Customer {Name = "Customer 1" },
                new Customer {Name = "Customer 2"}

            };
            var viewModel = new RandomMovieViewModel
                                
            {
                Movie = movie,
                Customers = customers
            };
            
            return View(viewModel);
        }

        public ActionResult New()
        {
            var genres = _movieContext.Genre.ToList();
            var date = new DateTime(1, 1, 1);
            var movie = new Movie
            {
                NumberInStock = 0,
                DateAdded = date
            };

            var myViewModel = new MovieFormViewModel
            {
                Movie = movie,
                Genres = genres

            };

            return View("MovieForm", myViewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Save(Movie movie)//model binding
        {
            if (!ModelState.IsValid)
            {
                var myViewModel = new MovieFormViewModel
                {
                    Movie = movie,
                    Genres = _movieContext.Genre.ToList()
                };

                return View("MovieForm", myViewModel);

            }

            if (movie.Id == 0)
            {
                movie.DateAdded = DateTime.Now;

                _movieContext.Movie.Add(movie);

            }
            else
            {
                var movieInDb = _movieContext.Movie.Single(m => m.Id == movie.Id);

                movieInDb.Name = movie.Name;
                movieInDb.GenreID = movie.GenreID;
                movieInDb.ReleaseDate = movie.ReleaseDate;
                movieInDb.NumberInStock = movie.NumberInStock;

            }
            _movieContext.SaveChanges();

            return RedirectToAction("Index", "Movies");
        }

        
        public ActionResult Edit(int id)
        {
            var Movie = _movieContext.Movie.SingleOrDefault(m => m.Id == id );
            //var movieCount = _movieContext.Movie.GroupBy(count => count.Name == Movie.Name);
            if (Movie == null)
                return HttpNotFound();

            var viewModel = new MovieFormViewModel
            {
                Movie = Movie,
                Genres = _movieContext.Genre.ToList(),
              //  iMovieCount = movieCount.Count()

            };

            return View("MovieForm",viewModel);
        }

        //attribute method
       //[Route("movies/released/{year}/{month:regex(\\d{4}):range(1,12)}")]
        public ActionResult ByReleaseDate(int year, int month)
        {
            return Content(year+"/"+month);

        }

        public ActionResult Details(int id)
        {
            var movie = _movieContext.Movie.Include(g=>g.Genre).SingleOrDefault(m => m.Id == id);

            if (movie == null)
                return HttpNotFound();

            return View(movie);
        }

        public ActionResult Index()
        {
            var movies = _movieContext.Movie.Include(g => g.Genre).ToList();
            return View(movies);

        }
    }
}