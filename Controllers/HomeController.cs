using ABCDMall.Models;
using ABCDMall.Services;
using System.Configuration;
using System.Linq;
using System.Web.Mvc;

namespace ABCDMall.Controllers
{
    public class HomeController : Controller
    {
        //Khởi tạo DBContext
        private readonly ABCDMallEntities db = new ABCDMallEntities();

        public ActionResult Index()
        {
            ViewBag.gallery = db.Galleries.ToList();
            ViewData["selectedNav"] = "Home";

            return View();
        }

        //Test thanh toán
        public ActionResult TestPayment()
        {
            VNPayService vnp = new VNPayService();
            //Truyền số tiền cần thanh toán, nội dung thanh toán, URL trả về
            return Redirect(vnp.CreateRequestUrl(100000, "Test", ConfigurationManager.AppSettings["weburl"] + "/home/testresult"));
        }

        //Test xử lý kết quả thanh toán
        public ActionResult TestResult()
        {
            VNPayService vnp = new VNPayService();
            //True nếu thanh toán thành công, False nếu ngược lại
            ViewBag.Status = vnp.ValidateSignature(Request);
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