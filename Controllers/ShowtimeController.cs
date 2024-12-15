using ABCDMall.Filters;
using ABCDMall.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Diagnostics;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.TaskbarClock;

namespace ABCDMall.Controllers
{
    public class ShowtimeController : Controller
    {
        private readonly ABCDMallEntities db = new ABCDMallEntities();
        // GET: Showtime
        [AdminFilter]
        public ActionResult Index()
        {
            ViewBag.Cinema = db.Cinemas.ToList();
            return View();
        }

        [AdminFilter]
        public ActionResult Showtime(int cinema, int variance = 0)
        {
            DateTime date = DateTime.Now.Date.AddDays(variance);
            ViewBag.movies = db.Movies.Where(movie => movie.Active == 1).OrderBy(movie => movie.RealeaseDate).ToList();
            ViewBag.cinema = cinema;
            ViewBag.variance = variance;
            return View();
        }

        [AdminFilter]
        public ActionResult Add(int cinema,  int movie, int variance = 0)
        {
            ViewBag.cinema = cinema;
            ViewBag.date = DateTime.Now.AddDays(variance);
            ViewBag.movie = db.Movies.FirstOrDefault(m => m.ID == movie);
            return View();
        }
        [AdminFilter]
        [HttpPost]
        public ActionResult Add(int cinema, int movie, string startingTime, string endingTime, string date)
        {
            DateTime startingT = DateTime.Parse($"{date} {startingTime}");
            DateTime endingT = DateTime.Parse($"{date} {endingTime}");
            Showtime st = new Showtime();
            st.Cinema = cinema;
            st.Movie = movie;
            st.StartingTime = startingT;
            st.EndingTime = endingT;
            db.Showtimes.Add(st);
            db.SaveChanges();
            return Redirect("/showtime/index");
        }

        [AdminFilter]
        [HttpPost]
        public ActionResult Check(int cinema, string startingTime, string endingTime, string date)
        {
            DateTime startingT = DateTime.Parse($"{date} {startingTime}");
            DateTime endingT = DateTime.Parse($"{date} {endingTime}");
            var check = db.Showtimes.Where(st => st.Cinema == cinema).Any(st => (st.StartingTime <= startingT && startingT <= st.EndingTime) || (st.StartingTime <= endingT && endingT <= st.EndingTime));
            return Json(check);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}