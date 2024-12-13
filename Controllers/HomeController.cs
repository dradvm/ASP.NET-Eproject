using ABCDMall.Models;
using ABCDMall.Services;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.Entity;
using System.Diagnostics;
using System.Linq;
using System.Web.Mvc;
using System.Windows.Documents;

namespace ABCDMall.Controllers
{
    public class HomeController : Controller
    {
        //Khởi tạo DBContext
        private readonly ABCDMallEntities db = new ABCDMallEntities();

        public ActionResult Index()
        {
            ViewBag.gallery = db.Galleries.ToList();
            return View();
        }

        public ActionResult Movie(int variance = 0)
        {
            DateTime date = DateTime.Now.Date.AddDays(variance);
            ViewBag.movies = db.Movies.Where(movie => movie.Showtimes.Any(showtime => DbFunctions.TruncateTime(showtime.StartingTime) == date.Date)).ToList();
            ViewBag.variance = variance;
            return View();
        }

        public ActionResult Seat(int id)
        {
            ViewBag.showtime = db.Showtimes.FirstOrDefault(showtime => showtime.ID == id);
            
            return View();
        }


        //Test thanh toán
        [HttpPost]
        public ActionResult Payment(List<int> seats, int total, String name, String email, int showtime)
        {
            List<Ticket> tickets = seats.Select(seat => new Ticket
            {
                Seat = seat,
                Showtime = showtime,
                CustomerName = name,
                CustomerEmail = email,
                Price = db.Seats.FirstOrDefault(x => x.ID == seat)?.SeatType1.Price ?? 0,
                PaymentTime = DateTime.Now
            }).ToList();
            db.Tickets.AddRange(tickets);
            db.SaveChanges();

            Session["CurrentTicketIds"] = tickets.Select(t => t.ID).ToList();

            VNPayService vnp = new VNPayService();
            //Truyền số tiền cần thanh toán, nội dung thanh toán, URL trả về
            return Redirect(vnp.CreateRequestUrl(total, "Test", ConfigurationManager.AppSettings["weburl"] + "/home/result"));
        }

        //Test xử lý kết quả thanh toán
        public ActionResult Result()
        {
            VNPayService vnp = new VNPayService();
            //True nếu thanh toán thành công, False nếu ngược lại
            var check = vnp.ValidateSignature(Request);
            ViewBag.Status = check;
            var ids = Session["CurrentTicketIds"] as List<int>;

            if (ids == null || !ids.Any())
            {
                // Không có vé nào cần xử lý
                ViewBag.Status = false;
                return View();
            }

            var tickets = db.Tickets.Where(t => ids.Contains(t.ID) && t.PaymentTime == null).ToList();

            if (check)
            {
                foreach (var ticket in tickets)
                {
                    ticket.PaymentTime = DateTime.Now;
                }
            }
            else
            {
                db.Tickets.RemoveRange(tickets);
            }
            db.SaveChanges();
            return View();
        }

        //Copy đoạn này để tự Dispose DBContext sau khi action hoàn thành, tránh tràn dữ liệu
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