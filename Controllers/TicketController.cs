using ABCDMall.Filters;
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


        [AdminFilter]
        public ActionResult Index(int variance = 0)
        {
            DateTime date = DateTime.Now.AddDays(variance);
            ViewBag.movies = db.Movies.Where(movie => movie.Active == 1 && movie.Showtimes.Any(showtime => DbFunctions.TruncateTime(showtime.StartingTime) == date.Date)).ToList();
            ViewBag.date = date;
            ViewBag.variance = variance;
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