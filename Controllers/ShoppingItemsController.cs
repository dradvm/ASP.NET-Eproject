using ABCDMall.Models;
using ABCDMall.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.IO;

namespace ABCDMall.Controllers
{
    public class ShoppingItemsController : Controller
    {
        // GET: Product
        private readonly ABCDMallEntities db = new ABCDMallEntities();

        [AdminFilter]
        public ActionResult Index()
        {
            ViewBag.ShoppingItems = db.Products.Where(product => product.Shop1.ShopType.ID == 1).ToList();
            return View();
        }
        public ActionResult Send(int status = 1)
        {
            ViewBag.status = status;
            return View();
        }





        //*******************Add***********************************
        [AdminFilter]
        public ActionResult Add(int status = 1)
        {
            var shop = db.Shops.Where(item => item.ShopeType == 1).ToList(); // Fetch list of shops from the database
            var shopItems = shop.Select(st => new SelectListItem
            {
                Value = st.ID.ToString(), // Shop ID
                Text = st.Name // Display shop name
            }).ToList();

            ViewBag.shoppingitems = shopItems; // Pass shops to the ViewBag
            ViewBag.status = status;

            // Set error messages based on the status code
            switch (status)
            {
                case 0:
                    ViewBag.Message = "Product name is required.";
                    break;
                case -1:
                    ViewBag.Message = "An image must be selected.";
                    break;
                case -2:
                    ViewBag.Message = "The selected image format is not supported.";
                    break;
                case -3:
                    ViewBag.Message = "The image size must not exceed 10MB.";
                    break;
                case 4:
                    ViewBag.Message = "Product name is required.";
                    break;
                case 5:
                    ViewBag.Message = "Product description is required.";
                    break;
                case 6:
                    ViewBag.Message = "You must select a shop.";
                    break;
                default:
                    ViewBag.Message = string.Empty;
                    break;
            }

            return View();
        }

        [AdminFilter]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Add(string name, HttpPostedFileBase image, int shopID, string description)
        {
            // Create Product object and populate fields
            Product pro = new Product
            {
                Shop = shopID,
                Name = name?.Trim(),
                Description = description?.Trim()
            };

            // Validate fields
            if (string.IsNullOrEmpty(pro.Name))
            {
                return Redirect("/shoppingitems/add?status=0");
            }

            if (image == null || image.ContentLength == 0)
            {
                return Redirect("/shoppingitems/add?status=-1");
            }

            string[] supported = { ".png", ".jpg", ".jpeg", ".svg" };
            if (!supported.Any(format => string.Equals(format, Path.GetExtension(image.FileName), StringComparison.OrdinalIgnoreCase)))
            {
                return Redirect("/shoppingitems/add?status=-2");
            }

            if (image.ContentLength > 10000000) // 10MB
            {
                return Redirect("/shoppingitems/add?status=-3");
            }

            try
            {
                // Save image to server
                string fileName = Path.GetFileNameWithoutExtension(image.FileName) + DateTime.Now.ToString("yyyyMMddHHmmss") + Path.GetExtension(image.FileName);
                string filePath = Path.Combine(Server.MapPath("~/Assets/Images/Product"), fileName);
                image.SaveAs(filePath);

                // Save product data to database
                pro.Image = fileName; // Store the image file name
                db.Products.Add(pro);
                db.SaveChanges();
            }
            catch (Exception ex)
            {
                ViewBag.status = -4;
                ViewBag.Message = $"Failed to save the image: {ex.Message}";
                return View();
            }

            // Redirect to index after successful save
            return Redirect("/shoppingitems/index");
        }






        //*************************update***************************
        [AdminFilter]
        public ActionResult Update(int id, int status = 1)
        {
            // Lấy thông tin cửa hàng theo ID
            Product pro = db.Products.FirstOrDefault(s => s.ID == id);
            if (pro == null)
            {
                return HttpNotFound();
            }

            // Lấy danh sách các loại cửa hàng từ database
            var shop = db.Shops.ToList();
            var shopItems = shop.Select(st => new SelectListItem
            {
                Value = st.ID.ToString(),
                Text = st.Name
            }).ToList();

            // Truyền dữ liệu vào ViewBag
            ViewBag.shoppingitems = shopItems; // Danh sách các loại cửa hàng
            ViewBag.status = status; // Trạng thái trả về từ POST

            return View(pro); // Truyền đối tượng Shop vào View
        }

        // POST: ShoppingCenters/Update/5
        [AdminFilter]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Update(int id, string name, HttpPostedFileBase image, int shopID, string description)
        {
            // Lấy cửa hàng cần cập nhật
            Product pro = db.Products.FirstOrDefault(s => s.ID == id);
            if (pro == null)
            {
                return HttpNotFound();
            }

            // Kiểm tra và cập nhật các trường dữ liệu
            if (string.IsNullOrEmpty(name?.Trim()))
            {
                return Redirect($"/shoppingitems/update/{id}?status=0"); // Kiểm tra tên cửa hàng
            }
            pro.Name = name.Trim();

            if (string.IsNullOrEmpty(description?.Trim()))
            {
                return Redirect($"/shoppingitems/update/{id}?status=-6"); // Kiểm tra mô tả
            }
            pro.Description = description.Trim();

            // Cập nhật ShopType và Floor
            pro.Shop = shopID;

            // Kiểm tra và xử lý logo (nếu có)
            if (image != null && image.ContentLength > 0)
            {
                string[] supported = { ".png", ".jpg", ".jpeg", ".svg" };
                if (!supported.Any(format => string.Equals(format, Path.GetExtension(image.FileName), StringComparison.OrdinalIgnoreCase)))
                {
                    return Redirect($"/shoppingitems/update/{id}?status=-2"); // Kiểm tra định dạng ảnh
                }

                if (image.ContentLength > 10000000)
                {
                    return Redirect($"/shoppingitems/update/{id}?status=-3"); // Kiểm tra kích thước ảnh
                }

                // Xóa ảnh cũ (nếu có)
                string oldLogo = pro.Image;
                if (System.IO.File.Exists(Server.MapPath("~/Assets/Images/Product/" + oldLogo)))
                {
                    System.IO.File.Delete(Server.MapPath("~/Assets/Images/Product/" + oldLogo));
                }

                // Lưu trữ logo mới
                pro.Image = Path.GetFileNameWithoutExtension(image.FileName) + DateTime.Now.ToString("yyyyMMddHHmmss") + Path.GetExtension(image.FileName);
                image.SaveAs(Path.Combine(Server.MapPath("~/Assets/Images/Product"), pro.Image));
            }

            // Lưu thay đổi vào cơ sở dữ liệu
            db.SaveChanges();

            return Redirect("/shoppingitems/index"); // Điều hướng về trang danh sách
        }





        //****************************************DELETE***********************************
        [AdminFilter]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id)
        {
            var pro = db.Products.FirstOrDefault(item => item.ID == id);
            if (pro != null)
            {
                // Xóa logo cũ nếu có
                string oldFile = Path.Combine(Server.MapPath("~/Assets/Images/Product"), pro.Image);
                if (System.IO.File.Exists(oldFile))
                {
                    System.IO.File.Delete(oldFile);
                }

                // Xóa shop khỏi cơ sở dữ liệu
                db.Products.Remove(pro);
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