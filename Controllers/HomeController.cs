using ABCDMall.Models;
using ABCDMall.Service;
using ABCDMall.Services;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.Entity;
using System.Diagnostics;
using System.Linq;
using System.Web.Mvc;
using System.Windows.Documents;
using System.Xml.Linq;

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



        //***********phần của Phuc***************
        public ActionResult ShoppingCenters()
        {
            ViewData["selectedNav"] = "ShoppingCenters";
            ViewBag.shops = db.Shops
                              .Include(s => s.ShopType) // Nạp dữ liệu của ShopType
                              .Where(s => s.ShopType.ID == 1)
                              .ToList();
            return View();
        }

        public ActionResult AboutUs()
        {
            ViewData["selectedNav"] = "AboutUs_ContactUs";
            return View();
        }

        public ActionResult ShopDetails(int id)
        {
            var shop = db.Shops.FirstOrDefault(s => s.ID == id);

            if (shop == null)
            {
                // Nếu không tìm thấy shop, hiển thị trang lỗi
                ViewBag.ErrorMessage = "Shop not found!";
                return View("Error");
            }

            // Trả về view với dữ liệu shop
            return View(shop);
        }






        public ActionResult Malldiagram()
        {
            ViewData["selectedNav"] = "Malldiagram";
            return View();
        }

        [HttpPost]
        public ActionResult Payment(List<int> seats, int total, String name, String email, int showtime)
        {
            

            Session["CurrentTicketIds"] = seats;
            Session["Name"] = name;
            Session["Email"] = email;
            Session["Showtime"] = showtime;
            VNPayService vnp = new VNPayService();
            //Truyền số tiền cần thanh toán, nội dung thanh toán, URL trả về
            return Redirect(vnp.CreateRequestUrl(total, "Test", ConfigurationManager.AppSettings["weburl"] + "/home/result"));
        }

        public ActionResult Result()
        {
            VNPayService vnp = new VNPayService();
            //True nếu thanh toán thành công, False nếu ngược lại
            var check = vnp.ValidateSignature(Request);
            ViewBag.Status = check;

            if (check)
            {
                List<int> seats = (List<int>) Session["CurrentTicketIds"];
                var bodyTickets = "";
                int y = Convert.ToInt32(Session["Showtime"] ?? 0);
                Showtime showtime = db.Showtimes.FirstOrDefault(x => x.ID == y);
                List<Ticket> tickets = seats.Select(seat => new Ticket
                {
                    Seat = seat,
                    Showtime = showtime.ID,
                    CustomerName = Session["Name"] as String,
                    CustomerEmail = Session["Email"] as String,
                    Price = db.Seats.FirstOrDefault(x => x.ID == seat)?.SeatType1.Price ?? 0,
                    PaymentTime = DateTime.Now
                }).ToList();
                db.Tickets.AddRange(tickets);
                foreach (var ticket in tickets)
                {
                    ticket.PaymentTime = DateTime.Now;
                    bodyTickets += $@"<tr>
                                    <td>
                                        {ticket.Seat1.Name}
                                    </td>
                                    <td>
                                        {ticket.Seat1.SeatType1.Name}
                                    </td>
                                    <td>
                                        {ticket.Seat1.SeatType1.Price}
                                    </td>
                                </tr>";
                }
                EmailSender es = new EmailSender();
                string emailBody = $@"
                <html>

                <body>
                    <p>Dear {Session["Name"] as String},</p>
                    <p>We are pleased to inform you that your ticket has been successfully booked. Below are the details of your ticket:
                    </p>
                    <div>
                        <h3>Ticket Information</h3>
                        <h4>Movie: {showtime.Movie1.Name}</h4>
                        <h4>Cinema: {showtime.Cinema1.Name}</h4>
                        <h4>Showtime: {showtime.StartingTime}</h4>
                        <table border='1'>
                            <thead>
                                <th>Seat</th>
                                <th>Seat Type</th>
                                <th>Price</th>
                            </thead>
                            <tbody>
                                {bodyTickets}
                            </tbody>
                        </table>
                    </div>
                    <p>Thank you for booking with us. We hope you have a great experience!</p>
                    <footer>Best regards, <br> ABCD Mall Support Team</footer>
                </body>

                </html>";
                ViewBag.tickets = tickets;
                ViewBag.showtime = showtime;
                es.SendEmail(Session["Email"] as String, "Booked Tickets Information", emailBody);
            }
            else
            {

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