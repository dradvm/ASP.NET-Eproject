using ABCDMall.Filters;
using ABCDMall.Models;
using ABCDMall.Services;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ABCDMall.Controllers
{
    public class MovieController : Controller
    {
        private readonly ABCDMallEntities db = new ABCDMallEntities();

        [AdminFilter]
        public ActionResult Index()
        {
            ViewBag.movies = db.Movies.ToList();
            return View();
        }

        [AdminFilter]
        public ActionResult Add(int status = 1)
        {
            ViewBag.status = status;
            ViewBag.genres = db.Genres.ToList();
            return View();
        }

        [AdminFilter]
        [HttpPost]
        public ActionResult Add(string name, HttpPostedFileBase image, string director, List<int> genres, DateTime releaseDate, int duration, string description)
        {
            Movie movie = new Movie();
            movie.Active = 1;
            if (name == null || name.Trim().Length == 0)
            {
                return Redirect("/movie/add?status=0");
            }
            movie.Name = name.Trim();
            if (image == null || image.ContentLength == 0)
            {
                return Redirect("/movie/add?status=-1");
            }
            if (director == null || director.Trim().Length == 0)
            {
                return Redirect("/movie/add?status=-2");
            }
            movie.Director = director.Trim();
            if (genres == null || genres.Count == 0)
            {
                return Redirect("/movie/add?status=-3");
            }
            foreach (int genreI in genres)
            {
                Genre genre = db.Genres.FirstOrDefault(item => item.ID == genreI);
                if (genre == null)
                {
                    return Redirect("/movie/add?status=-9");
                }
                movie.Genres.Add(genre);
            }
            if (releaseDate == null)
            {
                return Redirect("/movie/add?status=-4");
            }
            movie.RealeaseDate = releaseDate;
            if (duration <= 0)
            {
                return Redirect("/movie/add?status=-5");
            }
            movie.Duration = duration;
            if (description == null || description.Trim().Length == 0)
            {
                return Redirect("/movie/add?status=-6");
            }
            movie.Description = description.Trim();
            string[] supported = { ".png", ".jpg", ".jpeg", ".svg" };
            if (!supported.Any(format => string.Equals(format, Path.GetExtension(image.FileName), System.StringComparison.OrdinalIgnoreCase)))
            {
                return Redirect("/movie/add?status=-7");
            }
            movie.Image = HashService.GetHash(image.FileName) + DateTime.Now.ToString("yyyyMMddHHmmss") + Path.GetExtension(image.FileName);
            if (image.ContentLength > 10000000)
            {
                return Redirect("/movie/add?status=-8");
            }
            image.SaveAs(Path.Combine(Server.MapPath("~/Assets/Images/Movie"), movie.Image));
            db.Movies.Add(movie);
            db.SaveChanges();
            return Redirect("/movie/add?status=2");
        }

        [AdminFilter]
        public ActionResult Enable(int id)
        {
            Movie movie = db.Movies.FirstOrDefault(item => item.ID == id);
            if (movie != null)
            {
                movie.Active = 1;
                db.SaveChanges();
            }
            return Redirect("/movie/index");
        }

        [AdminFilter]
        public ActionResult Disable(int id)
        {
            Movie movie = db.Movies.FirstOrDefault(item => item.ID == id);
            if (movie != null)
            {
                movie.Active = 0;
                db.SaveChanges();
            }
            return Redirect("/movie/index");
        }

        [AdminFilter]
        public ActionResult Delete(int id)
        {
            Movie movie = db.Movies.FirstOrDefault(item => item.ID == id);
            if (movie != null)
            {
                string oldFile = Path.Combine(Server.MapPath("~/Assets/Images/Movie"), movie.Image);
                if (System.IO.File.Exists(oldFile))
                {
                    System.IO.File.Delete(oldFile);
                }
                db.Movies.Remove(movie);
                db.SaveChanges();
            }
            return Redirect("/movie/index");
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