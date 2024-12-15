using ABCDMall.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ABCDMall.Controllers
{
    public class TicketController : Controller
    {
        // GET: Ticket
        private readonly ABCDMallEntities db = new ABCDMallEntities();
        public ActionResult Index(int variance = 0)
        {
            ViewBag.movies = db.Movies.Where(movie => movie.Showtimes.Count > 0 && movie.Showtimes.Any(showtime => DbFunctions.TruncateTime(showtime.StartingTime) == DateTime.Now.AddDays(variance))).ToList();
            return View();
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