using ABCDMall.Filters;
using ABCDMall.Models;
using ABCDMall.Services;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data.Entity;

namespace ABCDMall.Controllers
{
    public class ShoppingCentersController : Controller
    {
        //**************************************GET***********************************
        private readonly ABCDMallEntities db = new ABCDMallEntities();

        [AdminFilter]
        public ActionResult Index()
        {
            ViewBag.shops = db.Shops
                              .Include(s => s.ShopType) // Nạp dữ liệu của ShopType
                              .Where(s => s.ShopType.ID == 1)
                              .ToList();
            return View();
        }







        //**************************************ADD***********************************
        [AdminFilter]
        public ActionResult Add(int status = 1, string message = "")
        {
            ViewBag.status = status;
            ViewBag.Message = message;
            return View();
        }

        [AdminFilter]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Add(string name, HttpPostedFileBase logo, int shopTypeID, int floor, string description)
        {
            Shop shop = new Shop
            {
                ShopeType = 1, // Loại cửa hàng (mặc định)
                Floor = floor,
                Name = name?.Trim(),
                Description = description?.Trim()
            };

            // Kiểm tra tên cửa hàng
            if (string.IsNullOrEmpty(shop.Name))
            {
                ViewBag.status = 0;
                ViewBag.Message = "Shop name is required.";
                return View();
            }

            // Kiểm tra logo
            if (logo == null || logo.ContentLength == 0)
            {
                ViewBag.status = -1;
                ViewBag.Message = "A logo must be selected.";
                return View();
            }

            // Kiểm tra định dạng hình ảnh
            string[] supported = { ".png", ".jpg", ".jpeg", ".svg" };
            if (!supported.Any(format => string.Equals(format, Path.GetExtension(logo.FileName), StringComparison.OrdinalIgnoreCase)))
            {
                ViewBag.status = -2;
                ViewBag.Message = "The selected image format is not supported.";
                return View();
            }

            // Kiểm tra kích thước hình ảnh
            if (logo.ContentLength > 10000000)
            {
                ViewBag.status = -3;
                ViewBag.Message = "The image size must not exceed 10MB.";
                return View();
            }

            // Lưu trữ hình ảnh
            try
            {
                string fileName = Path.GetFileNameWithoutExtension(logo.FileName) + DateTime.Now.ToString("yyyyMMddHHmmss") + Path.GetExtension(logo.FileName);
                string path = Path.Combine(Server.MapPath("~/Assets/Images/Shop"), fileName);

                logo.SaveAs(path);
                shop.Logo = fileName;
            }
            catch (Exception ex)
            {
                ViewBag.status = -4;
                ViewBag.Message = $"Failed to save the logo: {ex.Message}";
                return View();
            }

            // Thêm cửa hàng vào database
            db.Shops.Add(shop);
            db.SaveChanges();

            return RedirectToAction("Index", "ShoppingCenters");
        }






        //**************************************UPDATE***********************************
        [AdminFilter]
        public ActionResult Update(int id, int status = 1)
        {
            // Lấy thông tin cửa hàng theo ID
            Shop shop = db.Shops.FirstOrDefault(s => s.ID == id);
            if (shop == null)
            {
                return HttpNotFound();
            }

            // Truyền dữ liệu vào ViewBag để hiển thị trạng thái
            ViewBag.status = status;
            return View(shop);
        }

        [AdminFilter]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Update(int id, string name, HttpPostedFileBase logo, int floor, string description)
        {
            // Lấy cửa hàng cần cập nhật
            Shop shop = db.Shops.FirstOrDefault(s => s.ID == id);
            if (shop == null)
            {
                return HttpNotFound();
            }

            // Kiểm tra và cập nhật các trường dữ liệu
            if (string.IsNullOrEmpty(name?.Trim()))
            {
                return Redirect($"/shoppingcenters/update/{id}?status=0"); // Tên cửa hàng không được để trống
            }
            shop.Name = name.Trim();

            if (string.IsNullOrEmpty(description?.Trim()))
            {
                return Redirect($"/shoppingcenters/update/{id}?status=-6"); // Mô tả không được để trống
            }
            shop.Description = description.Trim();

            // Cập nhật Floor
            shop.Floor = floor;

            // Xử lý logo nếu có
            if (logo != null && logo.ContentLength > 0)
            {
                string[] supported = { ".png", ".jpg", ".jpeg", ".svg" };
                if (!supported.Any(format => string.Equals(format, Path.GetExtension(logo.FileName), StringComparison.OrdinalIgnoreCase)))
                {
                    return Redirect($"/shoppingcenters/update/{id}?status=-2"); // Định dạng ảnh không hợp lệ
                }

                if (logo.ContentLength > 10000000)
                {
                    return Redirect($"/shoppingcenters/update/{id}?status=-3"); // Kích thước ảnh vượt quá giới hạn
                }

                // Xóa ảnh cũ
                string oldLogo = shop.Logo;
                string oldLogoPath = Server.MapPath("~/Assets/Images/Shop/" + oldLogo);
                if (System.IO.File.Exists(oldLogoPath))
                {
                    System.IO.File.Delete(oldLogoPath);
                }

                // Lưu ảnh mới
                shop.Logo = Path.GetFileNameWithoutExtension(logo.FileName) + DateTime.Now.ToString("yyyyMMddHHmmss") + Path.GetExtension(logo.FileName);
                logo.SaveAs(Path.Combine(Server.MapPath("~/Assets/Images/Shop"), shop.Logo));
            }

            // Lưu thay đổi vào cơ sở dữ liệu
            db.SaveChanges();
            return Redirect("/shoppingcenters/index"); // Điều hướng về danh sách
        }






        //****************************************DELETE***********************************
        [AdminFilter]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id)
        {
            var shop = db.Shops.FirstOrDefault(item => item.ID == id);
            if (shop != null)
            {
                // Xóa logo cũ nếu có
                string oldFile = Path.Combine(Server.MapPath("~/Assets/Images/Shop"), shop.Logo);
                if (System.IO.File.Exists(oldFile))
                {
                    System.IO.File.Delete(oldFile);
                }

                // Xóa shop khỏi cơ sở dữ liệu
                db.Shops.Remove(shop);
                db.SaveChanges();

                // Trả về thông báo thành công
                return Json(new { success = true, message = "Delete successful!" });
            }

            // Trả về thông báo lỗi nếu không tìm thấy shop
            return Json(new { success = false, message = "Shop not found." });
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